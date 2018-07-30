
namespace Tests.Helpers.UI.Machines
{
    using System;
    using System.Collections.Generic;

    using global::Tests.Managers;
    using global::Tests.Models.Machines.UiModels;

    public class UiMachinesHelper : UiCommonHelper
    {
        public UiMachinesHelper(ApplicationManager app)
            : base(app)
        {
        }

        public List<TaskAllocationRecord> GetMachinesInfo()
        {
            App.Pages.MachinesPages.MachinesTaskAllocationPage.WaitForPageLoad();
            return App.Pages.MachinesPages.MachinesTaskAllocationPage.GetMachineTasksInfo();
        }

        public List<string> GetGridColumnsNames()
        {
            return App.Pages.MachinesPages.MachinesTaskAllocationPage.GetGridColumnNames();
        }

        public void ClickOnMachine(string machine)
        {
            App.Pages.MachinesPages.MachinesTaskAllocationPage.ClickOnMachine(machine);
        }

        public TaskAllocationRecord GetCurrentTaskInformationFromMachineDetails()
        {
            return App.Pages.MachinesPages.MachineDetailsPage.GetCurrentTaskInfo();
        }

        public List<TaskAllocationRecord> GetUpcomingTasksInformationFromMachineDetails()
        {
            return App.Pages.MachinesPages.MachineDetailsPage.GetUpcomingTasks();
        }

        public void ClickBackFromMachineDetails()
        {
            App.Pages.MachinesPages.MachineDetailsPage.ClickBackLink();                       
            this.App.Pages.MachinesPages.MachinesTaskAllocationPage.WaitForPageLoad();
        }

        public string GetMachineNameFromMachineDetailsPage()
        {
           return App.Pages.MachinesPages.MachineDetailsPage.GetMachineName();
        }
    }
}
