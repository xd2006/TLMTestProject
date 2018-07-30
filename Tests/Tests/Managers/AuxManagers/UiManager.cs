namespace Tests.Managers.AuxManagers
{
    using global::Tests.Helpers.UI;
    using global::Tests.Helpers.UI.Inventory;
    using global::Tests.Helpers.UI.Link;
    using global::Tests.Helpers.UI.Machines;
    using global::Tests.Helpers.UI.Main;
    using global::Tests.Helpers.UI.Orders;
    using global::Tests.Managers.AuxManagers.Templates;

    public class UiManager : AuxManagerTemplate
    {
        public UiManager(ApplicationManager app)
            : base(app)
        {
        }

        private UiCommonHelper uiHelp;

        private UiInventoryMainHelper inventoryMain;

        private UiToolInfoHelper toolInfo;

        private UiOrdersMainHelper ordersMain;

        private UiOrdersOrderHelper ordersOrder;

        private UiOrdersWorkpieceHelper ordersWorkpiece;

        private UiMainHelper main;

        private UiMachinesHelper machines;

        private UiLinkHelper link;

        #region helpers initialization

        public UiCommonHelper UiHelp => this.uiHelp ?? (this.uiHelp = new UiCommonHelper(App));

        public UiInventoryMainHelper ToolsMain => this.inventoryMain ?? (this.inventoryMain = new UiInventoryMainHelper(App));

        public UiToolInfoHelper ToolManagerToolInfo => this.toolInfo ?? (this.toolInfo = new UiToolInfoHelper(App));

        public UiOrdersMainHelper OrdersMain => this.ordersMain ?? (this.ordersMain = new UiOrdersMainHelper(App));

        public UiOrdersOrderHelper OrdersOrder => this.ordersOrder ?? (this.ordersOrder = new UiOrdersOrderHelper(App));

        public UiOrdersWorkpieceHelper OrdersWorkpiece => this.ordersWorkpiece ?? (this.ordersWorkpiece = new UiOrdersWorkpieceHelper(App));

        public UiMainHelper Main => this.main ?? (this.main = new UiMainHelper(App));

        public UiMachinesHelper Machines => this.machines ?? (this.machines = new UiMachinesHelper(App));

        public UiLinkHelper Link => this.link ?? (this.link = new UiLinkHelper(App));

        #endregion
    }
}
