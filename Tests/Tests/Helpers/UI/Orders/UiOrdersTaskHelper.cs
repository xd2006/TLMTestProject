
namespace Tests.Helpers.UI.Orders
{
    using System;
    using System.Collections.Generic;

    using Core.Service;

    using global::Tests.Managers;
    using global::Tests.Models.ToolManager.UiModels;

    public class UiOrdersTaskHelper : UiCommonHelper
    {
        public UiOrdersTaskHelper(ApplicationManager app)
            : base(app)
        {
        }

        public List<string> GetRequiredToolsTableColumnsNames()
        {
            return App.Pages.OrdersPages.TaskRequiredToolsPage.GetTableColumnsNames();
        }

        public List<ToolGridRecord> GetRequiredToolsTasks()
        {
            App.Pages.OrdersPages.TaskRequiredToolsPage.WaitForPageLoad();
            return App.Pages.OrdersPages.TaskRequiredToolsPage.GetRequiredToolsRecords();
        }

        public void CloseAddTaskPopup()
        {
            App.Pages.OrdersPages.CreateTaskPopup.WaitForPageLoad();
            Exception exc;
            int counter = 0;
            do
            {
              try
                {
                    App.Pages.OrdersPages.CreateTaskPopup.Close();
                    exc = null;
                    ServiceMethods.WaitForOperationPositive(() => !App.Pages.OrdersPages.CreateTaskPopup.IsOpened(), 4);
                }
                catch (Exception e)
                {
                    exc = e;
                }
            }
            while (exc != null && counter++ < 5);
        }
    }
}
