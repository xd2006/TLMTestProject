
namespace Tests.Parameters.ParametersSets
{
    public class Parameters_Remote : ParametersTemplate
    {
        public Parameters_Remote(string env)
            : base(env)
        {
        }

        public override string ApplicationUrl => "http://tlm-autotest.westeurope.cloudapp.azure.com/";

        public override string ToolManagerUrl => ApplicationUrl + "tools";

        public override string ProjectManagerUrl => ApplicationUrl + "orders";

        public override string GraphQlToolManagerEndpoint => "http://tlm-autotest.westeurope.cloudapp.azure.com:5000/graphql";

        public override string LinkAppGraphQlEndpoint =>
            "http://tlm-autotest.westeurope.cloudapp.azure.com:5100/graphql/";

        public override string ProjectManagerGraphQlEndpoint => "http://tlm-autotest.westeurope.cloudapp.azure.com:5050/graphql";
        public override string ProjectManagerDbConnectionString =>
            @"User ID=tlm;Password=tlm;Host=52.232.99.210;Port=5432;Database=ProjectManager;
        Pooling=true;";

        public override string HubUrl => "http://137.117.211.24:4444/wd/hub";
//        public override string HubUrl => "http://localhost:4444/wd/hub";

//        public override string TestRailUrl => string.Empty;
        public override string TestRailUrl => "https://testrailhoffmanngroup.testrail.io/";

        public override string TestRailUser => "aliaksandr_stratsiaichuk@epam.com";

        public override string TestRailPassword => "bqH2bt45rE7ew2mjMIG3-4uBRYe.QaV0YhaDXHTvf";

        public override string TestRailProjectId => "1";     
    }
}
