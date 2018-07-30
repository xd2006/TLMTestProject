
namespace Gurock.TestRail.Gurock.TestRail
{
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;

    public static class Extensions
    {
        public static void CompleteCase(this APIClient client, string runId, string caseNumber, string message)
        {
            var data = new Dictionary<string, object>
                           {
                               { "status_id", 1 },
                               { "comment", message }
                           };

           client.SendPost($"add_result_for_case/{runId}/{caseNumber}", data);
        }

        public static void FailCase(this APIClient client, string runId, string caseNumber, string message, string defects = null)
        {
            var data = new Dictionary<string, object>
                           {
                               { "status_id", 5 },
                               { "comment", message }                            
                           };

            if (defects != null)
            {
                data.Add("defects", defects);
            }

            client.SendPost($"add_result_for_case/{runId}/{caseNumber}", data);
        }

        public static void BlockCase(this APIClient client, string runId, string caseNumber, string message)
        {
            var data = new Dictionary<string, object>
                           {
                               { "status_id", 2 },
                               { "comment", message }
                           };

            client.SendPost($"add_result_for_case/{runId}/{caseNumber}", data);
        }

        public static void CompleteTest(this APIClient client, string testNumber)
        {
            var data = new Dictionary<string, object>
                           {
                               { "status_id", 1 },
                               { "comment", "This test worked fine!" }
                           };

            client.SendPost($"add_result/{testNumber}", data);
        }


        public static JArray GetRuns(this APIClient client, string projectId)
        {
            JArray array = (JArray)client.SendGet($"get_runs/{projectId}");
            return array;
        }

        public static string CreateRun(this APIClient client, string projectId, string milestoneName, string browser, List<string> cases = null, string milestoneId = "")
        {
            var data = new Dictionary<string, object>
                                  {
                                      {
                                          "name", $"Automated test run for {milestoneName}. Browser: {browser}"
                                      },
                                      { "include_all", true }
                                  };

            if (cases != null)
            {
                data["include_all"] = false;
                data["case_ids"] = cases;
            }

            if (!string.IsNullOrEmpty(milestoneId))
            {
                data["milestone_id"] = milestoneId;
            }

            JObject r = (JObject)client.SendPost($"add_run/{projectId}", data);
            return r["id"].ToString();
        }

        public static string UpdateRun(this APIClient client, string runId, List<string> cases, string milestoneId = "")
        {
            var data = new Dictionary<string, object>();                       
                data["include_all"] = false;
                data["case_ids"] = cases;

            if (!string.IsNullOrEmpty(milestoneId))
            {
                data["milestone_id"] = milestoneId;
            }

            JObject r = (JObject)client.SendPost($"update_run/{runId}", data);
            return r["id"].ToString();
        }

        public static string UpdateRunWithDescription(this APIClient client, string runId, string description)
        {
            var data = new Dictionary<string, object>();
            data["description"] = description;
           
            JObject r = (JObject)client.SendPost($"update_run/{runId}", data);
            return r["id"].ToString();
        }

        public static JArray GetCases(this APIClient client, string projectId)
        {
            JArray array = (JArray)client.SendGet($"get_cases/{projectId}");
            return array;
        }

        public static JArray GetMilestones(this APIClient client, string projectId)
        {
            JArray array = (JArray)client.SendGet($"get_milestones/{projectId}");
            return array;
        }

        public static JObject GetRun(this APIClient client, string runId)
        {
            return (JObject)client.SendGet($"get_run/{runId}");
        }

    }
}
