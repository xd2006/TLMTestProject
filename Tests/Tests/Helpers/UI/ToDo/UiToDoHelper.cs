
namespace Tests.Helpers.UI.ToDo
{
    using System.Collections.Generic;

    using global::Tests.Managers;
    using global::Tests.Models.ToDo.UiModels;

    public class UiToDoHelper : UiCommonHelper
    {
        public UiToDoHelper(ApplicationManager app)
            : base(app)
        {
        }

        public List<ToDoJobRecord> GetTasks(bool shouldFind = false, int waitTimeSec = 20)
        {
            App.Pages.GeneralPages.TodoPage.WaitForPageLoad();
            return App.Pages.GeneralPages.TodoPage.GetJobsRecords(shouldFind, waitTimeSec);
        }

        public void ClickOnPickList(string taskName)
        {
            App.Pages.GeneralPages.TodoPage.WaitForPageLoad();
            App.Pages.GeneralPages.TodoPage.ClickCreatePickList(taskName);         
           
        }
    }
}
