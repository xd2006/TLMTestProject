using NUnit.Framework;
using Tests.Tests.Inventory.Templates;
using Tests.TestsData.Inventory.Enums.FilterSearch;

namespace Tests.Tests.Inventory
{
    public class ToolScoutTests : InventoryTestsTemplate
    {
        [Test]
        [Category("UI")]
        [Property("TestCase", "2844")]
        [Property("TestCase", "2845")]
        [Property("TestCase", "2846")]
        [Property("TestCase", "2847")]
        [Property("Reference", "TLM-236")]
        public void ToolScoutButtonFunctionality()
        {
            #region parameters
            var materialGroup = "Cutting steels < 850 N/mm²";
            var material = "9 S 20";
            var diameter = "5";
            var depth = "10";
            #endregion

            App.Ui.ToolsMain.SelectToolType(FilterSearchData.ToolsTypes.Tools);
            App.Ui.ToolsMain.OpenToolScout();
            App.Ui.ToolScout.SelectTool(materialGroup, material, diameter, depth);
            App.Ui.ToolScout.GoToTlm();
            Assert.That(App.Ui.ToolsMain.IsInventoryPageOpened(), "tlm application ");
        }
    }
}
