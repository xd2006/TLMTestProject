
namespace Tests.Tests.Orders.Service
{
    using NUnit.Framework;

    [TestFixture]
    [Ignore("Service db methods")]
    [Parallelizable(ParallelScope.None)]
    [Category("Service")]
    public class DbService : TestBase
    {

        [Test]        
        public void CleanProjectManagerDb()
        {
            this.App.Db.ProjectManager.CleanDb();
        }
    }
}
