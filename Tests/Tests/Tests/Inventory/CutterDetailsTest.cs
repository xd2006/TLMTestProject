
namespace Tests.Tests.Inventory
{
    using System.Collections.Generic;
    using System.Linq;

    using Bogus;

    using Core.Service;

    using global::Tests.Tests.Inventory.Templates;
    using global::Tests.TestsData.Inventory.Enums.FilterSearch;

    using NUnit.Framework;

    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [Category("Cutter")]
    public class CutterDetailsTest : InventoryTestsTemplate
    {

        [Test]
        [Property("Reference", "TLM-327")]
        [Property("TestCase", "2840")]
        [Category("UI")]

        public void CheckCuttersInstancesEmptyTableElements()
        {
            this.App.Ui.ToolsMain.SelectToolType(FilterSearchData.ToolsTypes.Cutters);
            var cutters = App.Ui.ToolsMain.GetCuttersResults().Where(c => c.Quantity == 0);
            var cutter = new Faker().PickRandom(cutters).Name;
            App.Ui.ToolsMain.ClickTool(cutter);
            var info = App.Ui.ToolManagerToolInfo.GetCutterInstances();

            Assert.True(info.Count.Equals(0), "Grid should be empty");
        }

        [Test]
        [Property("Reference", "TLM-327")]
        [Property("TestCase", "2841")]
        [Property("TestCase", "2842")]
        [Property("TestCase", "2843")]
        [Category("UI")]

        public void CheckCuttersInstancesTableData()
        {
            var cutterName = "000A300Q02303100";

            var cutterInstance = new Dictionary<string, string>
                                     {
                                         { "ID", "-" },
                                         { "LOCATION", "Magazine 1" },
                                         { "Q-TY", "1" },
                                         { "STATUS", "Ready" }
                                     };

            this.App.Ui.ToolsMain.SelectToolType(FilterSearchData.ToolsTypes.Cutters);
            App.Ui.ToolsMain.PerformSearch(cutterName);
            App.Ui.ToolsMain.ClickTool(cutterName);
            var info = App.Ui.ToolManagerToolInfo.GetCutterInstances();
            var headers = App.Ui.ToolManagerToolInfo.GetInstacesGridColumnsNames();

            if (Parameters.Parameters.Browser.Equals("MicrosoftEdge"))
            {
                headers = ServiceMethods.StringListToUpper(headers);
            }

            Assert.Multiple(
                () =>
                    {
                        Assert.True(info.Count.Equals(1));
                        Assert.True(headers.SequenceEqual(cutterInstance.Keys.ToList()), "Cutter instances table headers are incorrect");
                        Assert.True(info[0].Id.Equals(cutterInstance["ID"]), "Cutter id is incorrect");
                        Assert.True(info[0].Location.Equals(cutterInstance["LOCATION"]), "Cutter location is incorrect");
                        Assert.True(info[0].Quantity.Equals(cutterInstance["Q-TY"]), "Cutter quantity is incorrect");
                        Assert.True(info[0].Status.Equals(cutterInstance["STATUS"]), "Cutter status is incorrect");
                    });
        }
    }
 }
