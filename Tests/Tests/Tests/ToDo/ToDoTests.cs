
namespace Tests.Tests.ToDo
{
    using System.Linq;

    using global::Tests.Tests.ToDo.Templates;
    using global::Tests.TestsData.Common.Enums;

    using NUnit.Framework;

    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [Category("Link")]
    public class ToDoTests : ToDoTestTemplate
    {
        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-208")]
        [Property("TestCase", "1742")]
        [Property("TestCase", "1743")]
        [Property("TestCase", "1749")]
        [Property("TestCase", "1748")]
        [Property("TestCase", "1744")]
        [Property("TestCase", "1745")]
        [Property("TestCase", "1746")]
        [Property("TestCase", "1747")]
        public void ToDoTasksDisplayingTest()
        {
            var tasks = App.Ui.ToDo.GetTasks(true, 30);

            var tasksDb = App.Db.ProjectManager.GeTasks();

            var filesTasksDb = App.Db.ProjectManager.GetFiles().Select(f => f.TaskId).ToList();

            tasksDb = tasksDb.Where(t => filesTasksDb.Contains(t.Id)).ToList();

            Assert.True(tasksDb.Count.Equals(tasks.Count), $"Different number of tasks from DB and UI. DB - {tasksDb.Count}, UI - {tasks.Count}");
            foreach (var task in tasksDb)
            {
                var workPieceId = App.Db.ProjectManager.GetWorkplan(task.WorkplanId).WorkpieceId;
                var workPiece = App.Db.ProjectManager.GetWorkpiece(workPieceId);

                var uiTask = tasks.First(t => t.Task == task.Name && t.Workpiece == workPiece.Name);

                Assert.True(uiTask.DueDate.Equals(task.StartDate.ToString("MM/dd/yyyy") + " 12:00:00 AM"));              
            }

            Assert.True(
                tasks.TrueForAll(t => t.Picklist.Equals("Create Picklist")),
                "Wrong create picklist button label");
        }
    }
}
