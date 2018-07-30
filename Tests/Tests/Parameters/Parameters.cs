
namespace Tests.Parameters
{
    using System.Configuration;

    using Core.Service;

    public class Parameters
    {
       
        private static Parameters instance;
        
        private readonly string applicationUrl;

        private readonly string toolManagerUrl;

        private readonly string projectManagerUrl;

        private readonly string hubUrl;

        private readonly string testRailUrl;

        private readonly string testRailUser;

        private readonly string testRailPassword;

        private readonly string testRailProjectId;

        private readonly string testRailMilestone;
        
        private readonly string browser;

        private readonly string graphQlEndpointUrl;

        private readonly string projectManagerDbConnectionString;

        private readonly string projectManagerGraphQlEndpoint;

        private readonly string linkAppGraphQlEndpoint;

        private string testRailRunId;

        private Parameters()
        {
            var appSettings = ConfigurationManager.AppSettings;

            var environment = System.Environment.GetEnvironmentVariable("ENVIRONMENT")
                              ?? NunitMethods.GetParameter(
                                  "Environment",
                                  appSettings["Environment"]);

            browser = System.Environment.GetEnvironmentVariable("BROWSER") 
                      ?? NunitMethods.GetParameter("Browser", appSettings["Browser"]);

            testRailMilestone = appSettings["Milestone"] != string.Empty ? appSettings["Milestone"] : "Sprint 1";

            var currentParam = ParametersFactory.CreateParameters(environment);

            applicationUrl = currentParam.ApplicationUrl;
            toolManagerUrl = currentParam.ToolManagerUrl;
            hubUrl = this.browser.ToLower().Equals("microsoftedge") ? null : currentParam.HubUrl;
            testRailUrl = currentParam.TestRailUrl;
            testRailUser = currentParam.TestRailUser;
            testRailPassword = currentParam.TestRailPassword;
            testRailProjectId = currentParam.TestRailProjectId;
            projectManagerUrl = currentParam.ProjectManagerUrl;
            this.graphQlEndpointUrl = currentParam.GraphQlToolManagerEndpoint;
            projectManagerDbConnectionString = currentParam.ProjectManagerDbConnectionString;
            projectManagerGraphQlEndpoint = currentParam.ProjectManagerGraphQlEndpoint;
            this.linkAppGraphQlEndpoint = currentParam.LinkAppGraphQlEndpoint;

        }

        public static Parameters ParametersInstance => instance ?? (instance = new Parameters());

        #region Custom params
        public static string ApplicationUrl => ParametersInstance.applicationUrl;

        public static string ToolManagerUrl => ParametersInstance.toolManagerUrl;

        public static string ProjectManagerUrl => ParametersInstance.projectManagerUrl;

        public static string HubUrl => ParametersInstance.hubUrl;

        public static string TestRailUrl => ParametersInstance.testRailUrl;

        public static string TestRailUser => ParametersInstance.testRailUser;

        public static string TestRailPassword => ParametersInstance.testRailPassword;

        public static string TestRailProjectId => ParametersInstance.testRailProjectId;

        public static string Browser => ParametersInstance.browser;

        public static string ToolManagerGraphQlEndpoint => ParametersInstance.graphQlEndpointUrl;

        public static string TestRailMilestone => ParametersInstance.testRailMilestone;

        public static string ProjectManagerDbConnectionString => ParametersInstance.projectManagerDbConnectionString;

        public static string ProjectManagerGraphQlEndpoint => ParametersInstance.projectManagerGraphQlEndpoint;

        public static string LinkAppGraphQlEndpoint => ParametersInstance.linkAppGraphQlEndpoint;


        public static string TestRailRunId
        {
            get => ParametersInstance.testRailRunId;
            set => ParametersInstance.testRailRunId = value;
        }
      
        #endregion
    }
}

