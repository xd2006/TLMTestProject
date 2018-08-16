
using Core.Service;

namespace Tests.Tests.Inventory
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using global::Tests.Models.ToolManager.GraphQlModels.ToolAssembly;
    using global::Tests.Tests.Inventory.Templates;
    using global::Tests.TestsData.Inventory.Enums.FilterSearch;

    using NUnit.Framework;

    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [Category("Search")]
    public class HolderSearchTest : InventoryTestsTemplate
    {

        [Test, TestCaseSource(typeof(SearchDataSource), nameof(SearchDataSource.SearchTestCasesPositive))]
        [Property("Reference", "TLM-70")]
        public void HoldersSearchPositive(string searchTerm)
        {
            var holders = this.App.GraphApi.ToolManager.SearchHolders(searchTerm);

            Assert.That(holders.Count > 0, "GraphApi didn't return any data");

            var results = this.CheckHolderItemsAreCorrect(
                FilterSearchData.Filters.Search,
                searchTerm,
                holders);

            Assert.True(
                results.Key,
                $"Next CutterAssemblies don't comply with search term {searchTerm}: {results.Value}");
        }

        [Test, TestCaseSource(typeof(SearchDataSource), nameof(SearchDataSource.SearchTestCasesNegative))]
        [Category("UI")]
        [Property("Reference", "TLM-70")]
        public void HoldersSearchNegative(string searchTerm)
        {
            var cutters = this.App.GraphApi.ToolManager.SearchHolders(searchTerm);

            Assert.That(cutters.Count == 0, "GraphApi shouldn't return any data");

            this.App.Ui.ToolsMain.SelectToolType(FilterSearchData.ToolsTypes.Holders);
            this.App.Ui.ToolsMain.PerformSearch(searchTerm);
            var uiResults = this.App.Ui.ToolsMain.GetCuttersResults();

            Assert.That(uiResults.Count == 0, "Ui shouldn't return any data");
        }

        //177 - Specific scenario to update TestRail scenarios since Pairwise cases can't have properties.
        [Test, TestCaseSource(typeof(CutterFilterTests.FilteringDataSource), nameof(CutterFilterTests.FilteringDataSource.SpecificTestCases))]
        [Property("Reference", "TLM-70")]
        public void FilteringByCombinationsSpecific(string usageMaterial,
                                                    string toolMaterial,
                                                    int? toolSize,
                                                    int? toolLength,
                                                    bool cooling,
                                                    string toolGroup,
                                                    string toolSubGroup,
                                                    bool availableInStock,
                                                    List<string> expectedNames)
        {
            Dictionary<FilterSearchData.Filters, object> filters =
                new Dictionary<FilterSearchData.Filters, object>
                    {
                        {
                            FilterSearchData.Filters.UsageMaterial,
                            usageMaterial
                        },
                        {
                            FilterSearchData.Filters.ToolMaterial,
                            toolMaterial
                        },
                        { FilterSearchData.Filters.ToolSize, toolSize },
                        {
                            FilterSearchData.Filters.ToolLength, toolLength
                        },
                        { FilterSearchData.Filters.Cooling, cooling },
                        { FilterSearchData.Filters.ToolGroup, toolGroup },
                        { FilterSearchData.Filters.Type, toolSubGroup},
                        {
                            FilterSearchData.Filters.AvaliabilityInStock,
                            availableInStock
                        }
                    };
            var results = this.ApiFilteringTest<CutterAssembly>(filters);

            Assert.Multiple(
                () =>
                {
                    Assert.True(results.Count.Equals(expectedNames.Count), "Invalid number of items was returned");
                    Assert.True(results.All(r => expectedNames.Contains(r.Name)), "Invalid results were returned");
                });
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-70")]
        [Property("TestCase", "166")]
        public void HoldersContentOfTheGrid()
        {
            var expectedColumns = new List<string> { "NAME", "SIZE", "LENGTH", "QUANTITY" };

            this.App.Ui.ToolsMain.SelectToolType(FilterSearchData.ToolsTypes.Holders);
            var columnNames = this.App.Ui.ToolsMain.GetGridColumnsNames();
            columnNames = ServiceMethods.RemoveStringsInList(columnNames, new List<string> { "▲", "▼" });

            var columnNamesToUpper = new List<string>();
            columnNames.ForEach(e => columnNamesToUpper.Add(e.ToUpper()));
            Assert.That(columnNamesToUpper.SequenceEqual(expectedColumns), "Grid columns names are not as expected");
        }


        private class SearchDataSource
        {
            public static IEnumerable SearchTestCasesPositive
            {
                get
                {
                    yield return new TestCaseData("CAPTO 3").SetProperty("TestCase", "168");
                    yield return new TestCaseData("REDUZIERUNG").SetProperty("TestCase", "169");
                    yield return new TestCaseData("175").SetProperty("TestCase", "170");
                }
            }

            public static IEnumerable SearchTestCasesNegative
            {
                get
                {
                    yield return new TestCaseData("s#$3 ZZZ");
                }
            }
        }

    }
}
