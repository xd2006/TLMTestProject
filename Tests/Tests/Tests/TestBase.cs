
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
            var dir = TestContext.CurrentContext.TestDirectory;
                Environment.CurrentDirectory = dir;
            this.App.Logger.Info("Test started - " + TestContext.CurrentContext.Test.MethodName);
        }

        [TearDown]
        public void AfterTest()
        {
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
                this.App.Logger.Info("Test completed successfully - " + TestContext.CurrentContext.Test.MethodName);

                this.ProcessPassedCaseWithTestRail(testCaseNumbers, testRailClient);
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
                            this.App.Logger.Warn("Can't update test cases state on TestRail:");
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
                            this.App.Logger.Warn("Can't update test cases state on TestRail:");
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
