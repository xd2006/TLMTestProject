namespace Tests.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Core.Service;
    using Core.WeDriverService;

    using global::Tests.Parameters;

    using Gurock.TestRail;
    using Gurock.TestRail.Gurock.TestRail;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [SetUpFixture]
    public class TestSetup : TestTemplate
    {
        [OneTimeSetUp]
        public void BeforeRun()
        {
            var client = this.InitTestRailClient(Parameters.TestRailUrl);
            
            if (client == null)
            {
                this.App.Logger.Warn("Can't interact with TestRail. TestRailUrl parameter is empty");
            }
            else
            {
                var run = this.GetRunContainsName(client, $"Automated test run for {Parameters.TestRailMilestone}. Browser: {Parameters.Browser}");

                if (run != string.Empty)
                {
                    Parameters.TestRailRunId = run;
                }

                // Get cases in automated state
                var automatedCases = this.GetCasesIdsWithAutomatedState(client, "1");
                var milestoneId = this.GetMilestoneId(client);

                if (string.IsNullOrEmpty(Parameters.TestRailRunId))
                {
                    try
                    {                                                  
                        var runId = client.CreateRun(
                            Parameters.TestRailProjectId,
                            Parameters.TestRailMilestone, 
                            Parameters.Browser,
                            automatedCases,
                            milestoneId);
                        Parameters.TestRailRunId = runId;
                    }
                    catch (Exception)
                    {
                        App.Logger.Warn("Can't define TestRail test run");
                    }
                }
                else
                {
                    var testRun = client.GetRun(Parameters.TestRailRunId);
                    string testRunMilestoneId = testRun["milestone_id"].ToString();

                    try
                    {
                        var runId = (string.IsNullOrEmpty(testRunMilestoneId) || !testRunMilestoneId.Equals(Parameters.TestRailMilestone)) && !string.IsNullOrEmpty(milestoneId)
                                        ? client.UpdateRun(Parameters.TestRailRunId, automatedCases, milestoneId)
                                        : client.UpdateRun(Parameters.TestRailRunId, automatedCases);

                        Parameters.TestRailRunId = runId;
                    }
                    catch (APIException e)
                    {
                        App.Logger.Warn($"Can't update test run. Message: '{e.Message}'");
                    }                   
                }

                UpdateTestRunWithCoverage(client, Parameters.TestRailRunId);
            }

            if (Parameters.Browser.ToLower().Equals("microsoftedge"))
            {
                ServiceMethods.KillAllEdgeBrowsersForCurrentUser();
                ServiceMethods.KillAllEdgeDriversForCurrentUser();
            }
        }

        private void UpdateTestRunWithCoverage(APIClient client, string runId)
        {
            var automatedCasesNumber = this.GetCasesIdsWithAutomatedState(client, "1").Count;
            var toAutomateCasesNumber = this.GetCasesIdsWithAutomatedState(client, "2").Count;

            decimal coverage = (decimal)automatedCasesNumber / (automatedCasesNumber + toAutomateCasesNumber);
            string coverageFormatted = coverage.ToString("P1");

            client.UpdateRunWithDescription(runId, $"Automated tests coverage = {coverageFormatted}");
        }

        [OneTimeTearDown]
        public void AfterRun()
        {
            this.Clean();
        }


        #region Private methods
        private void Clean()
        {
            WebDriverFactory.DismissAll();
        }

        /// <summary>
        /// automation types:
        /// 1 - automated
        /// 2 - to automate
        /// 3 - not applicable
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="automationType">
        /// The automation Type.
        /// </param>
        /// <returns>
        /// list of ids <see cref="List"/>.
        /// </returns>
        private List<string> GetCasesIdsWithAutomatedState(APIClient client, string automationType)
        {
            var cases = client.GetCases(Parameters.TestRailProjectId).Children<JObject>().ToList();
            var automatedCases = cases.Where(c => c["custom_automation_type"].ToString().Equals(automationType))
                .Select(c => c["id"].ToString()).ToList();
            return automatedCases;
        }

        private string GetRunContainsName(APIClient client, string name)
        {
            var runs = client.GetRuns(Parameters.TestRailProjectId);
            var automatedRuns = runs.Children<JObject>().ToList();
            var run = automatedRuns.FirstOrDefault(
                t => t["name"].ToString().Contains(name));

            if (run != null)
            {
                return run["id"].ToString();
            }

            return string.Empty;
        }

        private string GetMilestoneId(APIClient client)
        {
            var milestones = client.GetMilestones(Parameters.TestRailProjectId);
            var milestone =
                milestones.FirstOrDefault(
                    m => m["name"].ToString().Equals(Parameters.TestRailMilestone)
                         && m["is_completed"].ToString().ToLower().Equals("false"))
                ?? milestones.FirstOrDefault(m => m["is_completed"].ToString().ToLower().Equals("false"));

            string milestoneId = string.Empty;
            if (milestone == null)
            {
                this.App.Logger.Warn("There are no active milestones");
            }
            else
            {
                milestoneId = milestone["id"].ToString();
            }

            return milestoneId;
        }

        #endregion
    }
}
