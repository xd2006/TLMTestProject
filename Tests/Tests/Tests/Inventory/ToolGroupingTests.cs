
namespace Tests.Tests.Inventory
{
    using System.Collections.Generic;
    using System.Linq;

    using global::Tests.Tests.Inventory.Templates;
    using global::Tests.TestsData.Inventory.Enums.FilterSearch;

    using NUnit.Framework;

    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [Category("Grouping")]
    public class ToolGroupingTests : InventoryTestsTemplate
    {
        [Test]
        [Category("UI")]
        [Property("TestCase", "153")]
        [Property("Reference", "TLM-70")]
        public void CheckGroupsNames()
        {
            List<string> expectedNames = new List<string> { "Assemblies", "Cutters", "Holders" };
            var names = this.App.Ui.ToolsMain.GetGroupsLabels();
            Assert.True(names.SequenceEqual(expectedNames), "Tool groups selection buttons have incorrect names");
        }

        [Test]
        [Category("UI")]
        [Property("TestCase", "154")]
        [Property("Reference", "TLM-70")]
        public void CheckTAGridDisplaysTAEntities()
        {
            var results = this.App.Ui.ToolsMain.GetAssembliesResults();

            foreach (var result in results)
            {               
                var resList = this.App.GraphApi.ToolManager.SearchToolAssemblies(result.Name);
                Assert.True(resList.Count.Equals(1), $"TA '{result.Name}' from Ui wasn't found in API");
            }         
        }

        [Test]
        [Category("UI")]
        [Property("TestCase", "155")]
        [Property("Reference", "TLM-70")]
        public void CheckCuttersGridDisplaysCutterEntities()
        {
            this.App.Ui.ToolsMain.SelectToolType(FilterSearchData.ToolsTypes.Cutters);
            var results = this.App.Ui.ToolsMain.GetCuttersResults();

            foreach (var result in results)
            {
                var resList = this.App.GraphApi.ToolManager.SearchCutters(result.Name);
                Assert.True(resList.Count.Equals(1), $"TA '{result.Name}' from Ui wasn't found in API");
            }
        }

        [Test]
        [Category("UI")]
        [Property("TestCase", "157")]
        [Property("Reference", "TLM-70")]
        public void CheckHoldersGridDisplaysHolderEntities()
        {
            this.App.Ui.ToolsMain.SelectToolType(FilterSearchData.ToolsTypes.Holders);
            var results = this.App.Ui.ToolsMain.GetHoldersResults();

            foreach (var result in results)
            {
                var resList = this.App.GraphApi.ToolManager.SearchHolders(result.Name);
                Assert.True(
                    resList.Count > 0 && resList.All(e => e.Name.Contains(result.Name)),
                    $"TA '{result.Name}' from Ui wasn't found in API");
            }
        }

        [Test]
        [Category("UI")]
        [Property("TestCase", "174")]
        [Property("Reference", "TLM-70")]
        public void ResettingFiltersAfterSwithcingToolTypeTabs()
        {
            Dictionary<FilterSearchData.Filters, object> filters =
                new Dictionary<FilterSearchData.Filters, object>
                    {
                        { FilterSearchData.Filters.Search, "060" },
                        {
                            FilterSearchData.Filters.UsageMaterial,
                            "STAHL"
                        },
                        {
                            FilterSearchData.Filters.ToolMaterial,
                            "HSS"
                        },
                        { FilterSearchData.Filters.ToolSize, 6000000 },
                        { FilterSearchData.Filters.ToolGroup, "Drill" }
                    };

            this.App.Ui.ToolsMain.PerformFiltering(filters);
            var results = this.App.Ui.ToolsMain.GetAssembliesResults().Select(e => e.Name).ToList();
            
            this.App.Ui.ToolsMain.SelectToolType(FilterSearchData.ToolsTypes.Cutters);
            this.CheckFiltersInitialState();
            var resultsCutters = this.App.Ui.ToolsMain.GetAssembliesResults().Select(e => e.Name).ToList();
            Assert.That(!resultsCutters.SequenceEqual(results), "Results grid wasn't updated");

            filters.Remove(FilterSearchData.Filters.ToolSize);
            this.App.Ui.ToolsMain.PerformFiltering(filters);

            this.App.Ui.ToolsMain.SelectToolType(FilterSearchData.ToolsTypes.Holders);
            

            List<FilterSearchData.Filters> holderFilters = new List<FilterSearchData.Filters>
                                                               {
                                                                      FilterSearchData.Filters.Search,
                                                                      FilterSearchData.Filters.ToolLength,
                                                                      FilterSearchData.Filters.Cooling,
                                                                      FilterSearchData.Filters.AvaliabilityInStock
                                                                  };
            this.CheckFiltersInitialState(holderFilters);
            var resultsHolders = this.App.Ui.ToolsMain.GetAssembliesResults().Select(e => e.Name).ToList();
            Assert.That(!resultsHolders.SequenceEqual(resultsCutters) && !resultsHolders.SequenceEqual(results), "Results grid wasn't updated");
        }
    }
}
