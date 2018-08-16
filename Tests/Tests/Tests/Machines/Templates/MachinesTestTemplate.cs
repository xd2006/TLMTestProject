
namespace Tests.Tests.Machines.Templates
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::Tests.Models.ProjectManager.DbModels.Postgres;

    using NUnit.Framework;

    public abstract class MachinesTestTemplate : TestBase
    {
        [SetUp]
        public void BeforeEachTest()
        {
            this.App.BaseUrl += "machines";
        }

        #region helper methods

        protected Workpiece GetWorkpieceByTaskId(int taskId)
        {
            var workplanId = this.App.Db.ProjectManager.GetTask(taskId).WorkplanId;
            var workplan = this.App.Db.ProjectManager.GetWorkplan(workplanId);
            var workpiece = this.App.Db.ProjectManager.GetWorkpiece(workplan.WorkpieceId);
            return workpiece;
        }

        protected List<Task> DateTasksDictionaryToList(Dictionary<DateTime, List<Task>> upcomingTasks)
        {
            List<Task> expectedUpcomingTasks = new List<Task>();
            foreach (var taskRecord in upcomingTasks)
            {
                foreach (var task in taskRecord.Value)
                {
                    expectedUpcomingTasks.Add(task);
                }
            }

            return expectedUpcomingTasks;
        }

        protected Dictionary<DateTime, List<Task>> GetUpcomingTasks(List<Task> apiMachineTasks, Task currentTask)
        {
            var upcomingTasks = apiMachineTasks.Where(t => !(t.Name.Equals(currentTask.Name) && t.StartDate.Equals(currentTask.StartDate))).ToList();
            upcomingTasks.Sort((x, y) => DateTime.Compare(x.StartDate, y.StartDate));

            Dictionary<DateTime, List<Task>> upcomingTasksAndDates = new Dictionary<DateTime, List<Task>>();

            foreach (var task in upcomingTasks)
            {
                if (upcomingTasksAndDates.ContainsKey(task.StartDate))
                {
                    upcomingTasksAndDates[task.StartDate].Add(task);
                }
                else
                {
                    upcomingTasksAndDates.Add(task.StartDate, new List<Task>());
                    upcomingTasksAndDates[task.StartDate].Add(task);
                }
            }

            foreach (var upcomingTask in upcomingTasksAndDates)
            {
                if (upcomingTask.Value.Count > 1)
                {
                    var tasks = upcomingTasksAndDates[upcomingTask.Key];
                    this.SortTasksByCreatedDate(ref tasks);
                }
            }

            return upcomingTasksAndDates;
        }

        protected Task GetCurrentTask(List<Task> apiMachineTasks)
        {
            var potentiallyStartedtasks = apiMachineTasks.Where(t => t.StartDate <= DateTime.Now).ToList();
            var firstTask = potentiallyStartedtasks.First();
            var currentTaskCandidates = apiMachineTasks.Where(t => t.StartDate.Equals(firstTask.StartDate)).ToList();

            this.SortTasksByCreatedDate(ref currentTaskCandidates);
            var currentTask = currentTaskCandidates.First();
            return currentTask;
        }

        protected void SortTasksByCreatedDate(ref List<Task> currentTaskCandidates)
        {
            if (currentTaskCandidates.Count > 1)
            {
                foreach (var task in currentTaskCandidates)
                {
                    var dbTask = this.App.Db.ProjectManager.GetTask(task.Id);
                    currentTaskCandidates.First(t => t.Id == task.Id).CreatedDate = dbTask.CreatedDate;
                }

                currentTaskCandidates.Sort((x, y) => DateTime.Compare(x.CreatedDate, y.CreatedDate));
            }
        }

        #endregion
    }
}
