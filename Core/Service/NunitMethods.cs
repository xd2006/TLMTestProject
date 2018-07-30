
namespace Core.Service
{
    using NUnit.Framework;

    public class NunitMethods
    {

        public static string GetParameter(string parameter, string def = "")
        {
          return TestContext.Parameters.Get(parameter, def);
        }
    }
}
