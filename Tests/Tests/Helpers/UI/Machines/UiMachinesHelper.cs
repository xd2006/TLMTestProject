
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

        public List<TaskAllocationRecord> GetMachinesInfo(bool shouldFind = true, int timeSec = 20)
        {
            int counter = 0;
            App.Pages.MachinesPages.MachinesTaskAllocationPage.WaitForPageLoad();
            List<TaskAllocationRecord> machineInfo = new List<TaskAllocationRecord>();
            do
            {
                machineInfo = App.Pages.MachinesPages.MachinesTaskAllocationPage.GetMachineTasksInfo();
            }
            while (shouldFind && machineInfo.Count == 0 && counter++ < timeSec);

            return machineInfo;
        }

        public List<string> GetGridColumnsNames()
        {
            App.Pages.MachinesPages.MachinesTaskAllocationPage.WaitForPageLoad();
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

        public List<TaskAllocationRecord> GetUpcomingTasksInformationFromMachineDetails(bool shouldFind = false, int timeoutSec = 20)
        {
            return App.Pages.MachinesPages.MachineDetailsPage.GetUpcomingTasks(shouldFind, timeoutSec);
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
