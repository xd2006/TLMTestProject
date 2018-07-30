
namespace Tests.Parameters
{
    public abstract class ParametersTemplate
    {
        protected ParametersTemplate(string env)
        {
            this.Environment = env;
        }

        public virtual string Environment { get; }

        public abstract string ApplicationUrl { get; }

        public abstract string ToolManagerUrl { get; }

        public abstract string ProjectManagerUrl { get; }

        public abstract string HubUrl { get; }

        public abstract string TestRailUrl { get; }

        public abstract string TestRailUser { get; }

        public abstract string TestRailPassword { get; }

        public abstract string TestRailProjectId { get; }

        public abstract string GraphQlToolManagerEndpoint { get; }  
        
        public abstract string ProjectManagerDbConnectionString { get; }

        public abstract string ProjectManagerGraphQlEndpoint { get; }

        public abstract string LinkAppGraphQlEndpoint { get; }
    }
}
