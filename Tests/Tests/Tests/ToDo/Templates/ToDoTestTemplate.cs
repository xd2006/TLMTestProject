
namespace Tests.Tests.ToDo.Templates
{
    using NUnit.Framework;

    public abstract class ToDoTestTemplate : TestBase
    {
        [SetUp]
        public void BeforeEachTest()
        {
            this.App.BaseUrl = Parameters.Parameters.ApplicationUrl + "todo";
        }

    }
}
