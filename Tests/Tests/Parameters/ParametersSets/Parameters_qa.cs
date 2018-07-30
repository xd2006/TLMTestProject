
namespace Tests.Parameters.ParametersSets
{
    public class Parameters_qa : ParametersTemplate
    {
        public Parameters_qa(string env)
            : base(env)
        {
        }

        public override string ApplicationUrl => "http://draftqa.westeurope.cloudapp.azure.com/";

        public override string ToolManagerUrl => ApplicationUrl + "inventory";

        public override string ProjectManagerUrl => ApplicationUrl + "orders";

        public override string GraphQlToolManagerEndpoint => "http://draftqa.westeurope.cloudapp.azure.com:5000/graphql";

        public override string LinkAppGraphQlEndpoint =>
            "http://draftqa.westeurope.cloudapp.azure.com:5100/graphql/";

        public override string ProjectManagerGraphQlEndpoint => "http://draftqa.westeurope.cloudapp.azure.com:5050/graphql";

        public override string ProjectManagerDbConnectionString =>
            @"User ID=tlm;Password=tlm;Host=52.232.113.1;Port=5432;Database=ProjectManager;
        Pooling=true;";  //Update with QA server IP

        public override string HubUrl => null;

        //        public override string HubUrl => "http://localhost:4444/wd/hub"

        public override string TestRailUrl => null;

//      public override string TestRailUrl => "https://testrailhoffmanngroup.testrail.io/";

        public override string TestRailUser => "aliaksandr_stratsiaichuk@epam.com";

        public override string TestRailPassword => "bqH2bt45rE7ew2mjMIG3-4uBRYe.QaV0YhaDXHTvf";

        public override string TestRailProjectId => "1";

        
    }
}
