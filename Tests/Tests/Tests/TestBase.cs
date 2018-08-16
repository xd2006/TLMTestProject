
namespace Tests.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Core.WeDriverService;

    using Gurock.TestRail;
    using Gurock.TestRail.Gurock.TestRail;

    using NUnit.Framework;
    using NUnit.Framework.Interfaces;

    using ReportPortal.Shared;

    public class TestBase : TestTemplate
    {      
        [SetUp]
        public void BeforeTest()
        {
            if (Parameters.Parameters.Browser == "MicrosoftEdge"
                && TestContext.CurrentContext.Test.Properties["Category"].Contains("NoEdge"))
            {                
                App.Logger.Info("Test is ignored due to MS Edge restrictions - " + TestContext.CurrentContext.Test.MethodName);
                Assert.Ignore();
            }
            
                    var dir = TestContext.CurrentContext.TestDirectory;
                Environment.CurrentDirectory = dir;
            this.App.Logger.Info("Test started - " + TestContext.CurrentContext.Test.MethodName);
        }

        [TearDown]
        public void AfterTest()
        {
            if (TestContext.CurrentContext.Test.Properties["Category"].Contains("UI")
                && !TestContext.CurrentContext.Test.Properties["Category"].Contains("ConsoleErrorExpected")
                && Parameters.Parameters.Browser.ToLower() != "microsoftedge")
            {
                var errors = App.Ui.UiHelp.GetBrowserLogs();
                if (errors.Count > 0)
                {
                    App.Logger.Error($"Browser console errors were found during test method run: {TestContext.CurrentContext.Test.MethodName}. Errors:" + Environment.NewLine);
                    foreach (var er in errors)
                    {
                        App.Logger.Error(er.Level + " " + er.Message);
                    }
                }
            }


            APIClient testRailClient = null;

            if (!string.IsNullOrEmpty(Parameters.Parameters.TestRailUrl))
            {
                testRailClient = this.InitTestRailClient(Parameters.Parameters.TestRailUrl);
            }

            var testCaseNumbers = TestContext.CurrentContext.Test.Properties.ContainsKey("TestCase") ?
                                     TestContext.CurrentContext.Test.Properties["TestCase"].ToList() :
                                                   new List<object>();
            
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                this.App.Logger.Warn("Test failed - " + TestContext.CurrentContext.Test.MethodName);
                
                    this.ProcessFailedCaseWithTestRail(testCaseNumbers, testRailClient);

                if (this.App.PageManagerExists)
                {
                    this.MakeScreenshot();
                }
              }
            else
            {
                if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Skipped)
                {
                    this.App.Logger.Info("Test completed successfully - " + TestContext.CurrentContext.Test.MethodName);

                    this.ProcessPassedCaseWithTestRail(testCaseNumbers, testRailClient);
                }
            }

            if (this.App.PageManagerExists)
            {
                if (this.App.Pages.Driver != null)
                {
                    WebDriverFactory.DismissLocalThreadDriver();                   
                }            
            }

            // Nullify application mananger after each test
            this.App = null;
        }

        #region private methods

        private void ProcessPassedCaseWithTestRail(List<object> testCaseNumbers, APIClient client)
        {
            if (testCaseNumbers.Count > 0)
            {
                if (client != null)
                {
                    foreach (var testCase in testCaseNumbers)
                    {
                        try
                        {
                            client.CompleteCase(
                                Parameters.Parameters.TestRailRunId,
                                testCase.ToString(),
                                $"Test method: {TestContext.CurrentContext.Test.MethodName}, Result message: {TestContext.CurrentContext.Result.Message}");
                        }
                        catch (Exception e)
                        {
                            // supress exception
                            this.App.Logger.Warn($"Can't update test case '{testCase}' state on TestRail:");
                            this.App.Logger.Warn(e.Message);
                        }
                    }
                }
            }
        }

        private void ProcessFailedCaseWithTestRail(List<object> testCaseNumbers, APIClient client)
        {
            if (testCaseNumbers.Count > 0)
            {
                if (client != null)
                {
                    foreach (var testCase in testCaseNumbers)
                    {
                        try
                        {
                            if (TestContext.CurrentContext.Test.Properties["Category"].Contains("Blocked"))
                            {
                                string reason = TestContext.CurrentContext.Test.Properties.ContainsKey("Reason")
                                                    ? TestContext.CurrentContext.Test.Properties.Get("Reason").ToString()
                                                    : "Blocking reason is not set.";
                                client.BlockCase(Parameters.Parameters.TestRailRunId, testCase.ToString(), reason);
                            }
                            else
                            {
                                string bugsList = string.Empty;
                                if (TestContext.CurrentContext.Test.Properties.ContainsKey("Bug"))
                                {
                                    bugsList = TestContext.CurrentContext.Test.Properties.Get("Bug").ToString();
                                }

                                client.FailCase(
                                    Parameters.Parameters.TestRailRunId,
                                    testCase.ToString(),
                                    $"Test method: {TestContext.CurrentContext.Test.MethodName}, Result message: {TestContext.CurrentContext.Result.Message}",
                                bugsList);
                            }
                        }
                        catch (Exception e)
                        {
                            // supress exception
                            this.App.Logger.Warn($"Can't update test cases '{testCase}' state on TestRail:");
                            this.App.Logger.Warn(e.Message);
                        }
                    }
                }
            }
        }

        private void MakeScreenshot()
        {
            var screenPath = new Screenshoter(this.App.Pages.Driver).TakeScreenshot(
                TestContext.CurrentContext.Test.MethodName,
                TestContext.CurrentContext.TestDirectory + "\\Logs");
            if (!string.IsNullOrEmpty(screenPath))
            {
                //                this.App.Logger.Info("Screenshot {rp#file# " + screenPath + "}");

                //temporary to check screens from azure agent
                this.App.Logger.Info("Screenshot {" + screenPath + "}");

                Bridge.LogMessage(ReportPortal.Client.Models.LogLevel.Info, "Screenshot {rp#file#" + screenPath + "}");
            }
            else          
            {
                this.App.Logger.Warn("Can't make screenshot");
            }
        }

        #endregion
    }
}
