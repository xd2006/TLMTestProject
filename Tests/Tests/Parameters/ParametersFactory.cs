
namespace Tests.Parameters
{
    using System.Collections.Specialized;
    using System.Configuration;

    using global::Tests.Parameters.ParametersSets;
    
    public class ParametersFactory
    {        
        public static ParametersTemplate CreateParameters(string environment)
        {
        NameValueCollection appSettings = ConfigurationManager.AppSettings;
        environment = environment ?? appSettings["Environment"];
         
            switch (environment.ToLower())
            {
                case "local":
                    return new Parameters_Local(environment);
                case "remote":
                    return new Parameters_Remote(environment);
                case "qa":
                    return new Parameters_qa(environment);
                default:
                    return new Parameters_Local(environment);
            }
        }
    }
}
