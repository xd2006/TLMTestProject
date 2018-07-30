
namespace Tests.Tests
{
    using System.Threading;

    using Gurock.TestRail;
    using global::Tests.Managers;
    using global::Tests.Service;
   
    public abstract class TestTemplate
    {
        private ThreadLocal<ApplicationManager> appM;
        

        /// <summary>
        /// Gets or sets the app. manager
        /// </summary>
        protected ApplicationManager App
        {
            get
            {
                this.appM = this.appM ?? new ThreadLocal<ApplicationManager>(() => new Starter().StartApplicationManager());
                this.appM.Value = appM.Value ?? new Starter().StartApplicationManager();
                return this.appM.Value;
            }

            set
            {
                this.appM = this.appM ?? new ThreadLocal<ApplicationManager>();
                this.appM.Value = value;
            }
        }

        protected APIClient InitTestRailClient(string testRailUrl)
        {
            if (!string.IsNullOrEmpty(testRailUrl))
            {
                APIClient client = new APIClient(testRailUrl)
                                       {
                                           User = Parameters.Parameters.TestRailUser,
                                           Password = Parameters.Parameters.TestRailPassword
                                       };
                return client;
            }
            return null;
        }      
    }
}
