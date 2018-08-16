
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
    [Category("Filter")]
    public class HolderFilteringTests : InventoryTestsTemplate
    {
       [Test, TestCaseSource(typeof(FilteringDataSource), nameof(FilteringDataSource.PositiveTestCases))]
        [Property("Reference", "TLM-70")]
//        [Parallelizable(ParallelScope.None)]
        public void HoldersFilteringPositive(FilterSearchData.Filters filterType, object filterValue)
        {
            int take = 50; //temporary to check performance

            this.ApiFilteringTest<Holder>(
                new Dictionary<FilterSearchData.Filters, object> { { filterType, filterValue } },
                take,
                true);
        }

        [Test, TestCaseSource(typeof(FilteringDataSource), nameof(FilteringDataSource.NegativeTestCases))]
        [Property("Reference", "TLM-70")]
//        [Parallelizable(ParallelScope.None)]
        public void HoldersFilteringNegative(FilterSearchData.Filters filterType, object filterValue)
        {
            int take = 50; //temporary to check performance

            this.ApiFilteringTest<Holder>(
                new Dictionary<FilterSearchData.Filters, object> { { filterType, filterValue } },
                take,
                false);
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-70")]
        [Property("TestCase", "186")]
        [Property("TestCase", "171")]
        public void ResetFiltersHolder()
        {
            Dictionary<FilterSearchData.Filters, object> filters =
                new Dictionary<FilterSearchData.Filters, object>
                    {
                        { FilterSearchData.Filters.Search, "D32" },
                        { FilterSearchData.Filters.ToolLength, 50000000 },
                        { FilterSearchData.Filters.Cooling, false },
                        { FilterSearchData.Filters.AvaliabilityInStock, true }
                    };
            this.ResetFiltersTest(this.App.Ui.ToolsMain.ResetFiltersAndSearch, filters, FilterSearchData.ToolsTypes.Holders);
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-70")]
        [Property("TestCase", "188")]
        public void ResetFiltersHolderByBrowserRefresh()
        {
            Dictionary<FilterSearchData.Filters, object> filters =
                new Dictionary<FilterSearchData.Filters, object>
                    {
                        { FilterSearchData.Filters.Search, "D32" },
                        { FilterSearchData.Filters.ToolLength, 60000000 },
                        { FilterSearchData.Filters.Cooling, false },
                        { FilterSearchData.Filters.AvaliabilityInStock, true }
                    };
            this.ResetFiltersTest(this.App.Ui.ToolsMain.RefreshPage, filters, FilterSearchData.ToolsTypes.Holders, false);
        }

        [Test]
        [Category("UI")]
        [Property("TestCase", "173")]
        [Property("Reference", "TLM-70")]
        public void HoldersEmptySearchGrid()
        {
            this.CheckEmptySearchGridTest(FilterSearchData.ToolsTypes.Holders);
        }

        [Test, Pairwise]
        [Property("Reference", "TLM-70")]
        public void FilteringHoldersByCombinations(
            [Values(86000000, 50000000, null)] int? toolLength,
            [Values(true, false)] bool cooling,
            [Values(true, false)] bool avaliability)
        {
            Dictionary<FilterSearchData.Filters, object> filters =
                new Dictionary<FilterSearchData.Filters, object>
                    {
                        {
                            FilterSearchData.Filters.ToolLength, toolLength
                        },
                        {
                            FilterSearchData.Filters.Cooling, cooling
                        },
                        {
                        FilterSearchData.Filters.AvaliabilityInStock, avaliability
                        }
                     };


            this.ApiFilteringTest<Holder>(filters, false);
        }


        //183 - Specific scenario to update TestRail scenarios since Pairwise cases can't have properties.
        [Test, TestCaseSource(typeof(FilteringDataSource), nameof(FilteringDataSource.SpecificTestCases))]
        [Property("Reference", "TLM-70")]
        public void FilteringHoldersByCombinationsSpecific(int? toolLength,
                                                    bool cooling,
                                                    bool availableInStock,
                                                    List<string> expectedNames)
        {
            Dictionary<FilterSearchData.Filters, object> filters =
                new Dictionary<FilterSearchData.Filters, object>
                    {
                        {
                            FilterSearchData.Filters.Search, expectedNames[0]
                        },
                        {
                            FilterSearchData.Filters.ToolLength, toolLength
                        },
                        { FilterSearchData.Filters.Cooling, cooling },
                        {
                            FilterSearchData.Filters.AvaliabilityInStock,
                            availableInStock
                        }
                    };
            var results = this.ApiFilteringTest<Holder>(filters);

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
        [Property("TestCase", "185")]
        public void HolderFilterAndSearch()
        {
            List<string> expectedResult = new List<string> { "D04         SCHRUMPF POKOLM        EL075  GL086 HSK63" };
                                                                  

            Dictionary<FilterSearchData.Filters, object> filters =
                new Dictionary<FilterSearchData.Filters, object>
                    {
                        { FilterSearchData.Filters.Search, "D04" },
                        { FilterSearchData.Filters.ToolLength, 86000000 },
                        { FilterSearchData.Filters.Cooling, true },
                        {
                            FilterSearchData.Filters.AvaliabilityInStock,
                            false
                        }
                    };

            var resultsApi = this.ApiFilteringTest<Holder>(filters);

            this.App.Ui.ToolsMain.SelectToolType(FilterSearchData.ToolsTypes.Holders);
            this.App.Ui.ToolsMain.PerformFiltering(filters);
            var resultsUi = this.App.Ui.ToolsMain.GetHoldersResults();

            Assert.Multiple(
                () =>
                {
                    Assert.True(
                        resultsApi.Count.Equals(expectedResult.Count),
                        "Invalid number of items was returned thru Api");
                    Assert.True(
                        resultsUi.Count.Equals(expectedResult.Count),
                        "Invalid number of items was returned thru Ui");

                    Assert.True(
                        resultsApi.All(r => expectedResult.Contains(r.Name)),
                        "Invalid results were returned thru Api");
                    this.CompareHolderRecordsFromApiAndUi(resultsApi, resultsUi);
                });
        }


        private class FilteringDataSource
        {
            public static IEnumerable PositiveTestCases
            {
                get
                {
                    yield return new TestCaseData(FilterSearchData.Filters.ToolLength, 80000000).SetProperty("TestCase", "181");
                    yield return new TestCaseData(FilterSearchData.Filters.Cooling, true).SetProperty("TestCase", "182");
                    yield return new TestCaseData(FilterSearchData.Filters.Cooling, false);
                    yield return new TestCaseData(FilterSearchData.Filters.AvaliabilityInStock, true).SetProperty("TestCase", "814");
                }
            }

            public static IEnumerable NegativeTestCases
            {
                get
                {
                    yield return new TestCaseData(FilterSearchData.Filters.ToolLength, 896867985).SetProperty("TestCase", "384");                   
                }
            }

            public static IEnumerable SpecificTestCases
            {
                get
                {
                    yield return new TestCaseData(
                            126000000,
                            false,
                            false,
                            new List<string> { "D16      SCHRUMPF POKOLM        EL100 HSK50" })
                        .SetProperty("TestCase", "183").SetProperty("TestCase", "184");                  
                }
            }
        }
    }
}
