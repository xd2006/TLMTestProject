using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.UI.Pages.Inventory
{
    using Core.WeDriverService.Extensions;

    using global::Tests.Models.ToolManager.UiModels;
    using global::Tests.UI.Components.Tools;
    using global::Tests.UI.Pages.Inventory.Templates;

    using OpenQA.Selenium;

    public class InventoryCutterInformationPopup : InformationPopupTemplate
    {
        public InventoryCutterInformationPopup(IWebDriver driver)
            : base(driver)
        {
        }

        private CutterInstancesGrid CutterInstancesGrid => new CutterInstancesGrid(Driver);

        public List<CutterInstanceGridRecord> GetCutterInstances()
        {
            WaitForPageLoad();
            return this.CutterInstancesGrid.GetRecords();
        }
    }
}
