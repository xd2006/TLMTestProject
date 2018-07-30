
namespace Tests.Managers.AuxPageManagers
{
    using global::Tests.Managers.AuxPageManagers.Templates;
    using global::Tests.UI.Pages.Machines;

    using OpenQA.Selenium;

    public class MachinesPages : PagesManagerTemplate
    {
        private MachinesTaskAllocationPage machinesTaskAllocation;

        private MachineDetailsPage machineDetailsPage;

        public MachinesPages(IWebDriver driver)
            : base(driver)
        {
        }

        public MachinesTaskAllocationPage MachinesTaskAllocationPage => this.machinesTaskAllocation ?? (this.machinesTaskAllocation = new MachinesTaskAllocationPage(Driver));

        public MachineDetailsPage MachineDetailsPage => machineDetailsPage ?? (this.machineDetailsPage = new MachineDetailsPage(Driver));
    }
}
