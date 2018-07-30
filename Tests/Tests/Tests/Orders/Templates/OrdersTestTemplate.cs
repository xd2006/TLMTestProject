
namespace Tests.Tests.Orders.Templates
{
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [Category("ProjectManagerGrid")]
    public abstract class OrdersTestTemplate : TestBase
    {
        [SetUp]
        public void BeforeEachTest()
        {
            this.App.BaseUrl = Parameters.Parameters.ProjectManagerUrl;
        }

    }
}
