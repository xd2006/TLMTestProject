
namespace Tests.Tests.General.Templates
{
    using NUnit.Framework;

    public abstract class GeneralTestTemplate : TestBase
    {
        [SetUp]
        public void BeforeEachTest()
        {
            App.BaseUrl = Parameters.Parameters.ApplicationUrl;
        }


    }
}
