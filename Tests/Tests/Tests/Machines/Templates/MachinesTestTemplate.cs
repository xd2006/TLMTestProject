
namespace Tests.Tests.Machines.Templates
{
    using NUnit.Framework;

    public abstract class MachinesTestTemplate : TestBase
    {
        [SetUp]
        public void BeforeEachTest()
        {
            this.App.BaseUrl += "machines";
        }
    }
}
