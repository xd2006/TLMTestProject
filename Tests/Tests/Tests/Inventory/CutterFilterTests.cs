
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
    public class CutterFilterTests : InventoryTestsTemplate
    {
       

        [Test, TestCaseSource(typeof(FilteringDataSource), nameof(FilteringDataSource.PositiveTestCases))]
        [Property("Reference", "TLM-70")]
//        [Parallelizable(ParallelScope.None)]
        public void CuttersFilteringPositive(FilterSearchData.Filters filterType, object filterValue)
        {
            int take = 50; //temporary to check performance
            
            this.ApiFilteringTest<CutterAssembly>(
                new Dictionary<FilterSearchData.Filters, object> { { filterType, filterValue } },
                take,
                true);       
        }

        [Test, TestCaseSource(typeof(FilteringDataSource), nameof(FilteringDataSource.NegativeTestCases))]
        [Property("Reference", "TLM-70")]
//        [Parallelizable(ParallelScope.None)]
        public void CuttersFilteringNegative(FilterSearchData.Filters filterType, object filterValue)
        {
            int take = 50; //temporary to check performance

            this.ApiFilteringTest<CutterAssembly>(
                new Dictionary<FilterSearchData.Filters, object> { { filterType, filterValue } },
                take,
                false);
        }


        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-70")]
        [Property("TestCase", "180")]
        [Property("TestCase", "162")]
        public void ResetFiltersCutterAssembly()
        {
            Dictionary<FilterSearchData.Filters, object> filters =
                new Dictionary<FilterSearchData.Filters, object>
                    {
                        { FilterSearchData.Filters.Search, "ADW" },
                        { FilterSearchData.Filters.UsageMaterial, null },
                        { FilterSearchData.Filters.ToolMaterial, "WPL" },
                        { FilterSearchData.Filters.ToolSize, 23000000 },
                        { FilterSearchData.Filters.ToolLength, 400000000 },
                        { FilterSearchData.Filters.Cooling, false },
                        { FilterSearchData.Filters.ToolGroup, null },
                        { FilterSearchData.Filters.AvaliabilityInStock, false }
                    };
            this.ResetFiltersTest(this.App.Ui.ToolsMain.ResetSearchAndFilters, filters, FilterSearchData.ToolsTypes.Cutters);
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-70")]
        [Property("TestCase", "187")]
        public void ResetFiltersCutterAssemblyByBrowserRefresh()
        {
            Dictionary<FilterSearchData.Filters, object> filters =
                                new Dictionary<FilterSearchData.Filters, object>
                                    {
                                        { FilterSearchData.Filters.Search, "ADW" },
                                        { FilterSearchData.Filters.UsageMaterial, null },
                                        { FilterSearchData.Filters.ToolMaterial, "WPL" },
                                        { FilterSearchData.Filters.ToolSize, 23000000 },
                                        { FilterSearchData.Filters.ToolLength, 400000000 },
                                        { FilterSearchData.Filters.Cooling, false },
                                        { FilterSearchData.Filters.ToolGroup, null },
                                        { FilterSearchData.Filters.AvaliabilityInStock, false }
                                    };
           this.ResetFiltersTest(this.App.Ui.ToolsMain.RefreshPage, filters, FilterSearchData.ToolsTypes.Cutters, false);
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-70")]
        [Property("TestCase", "379")]
        public void CutterAssemblyGroupAndSubgroupFiltering()
        {

            Dictionary<FilterSearchData.Filters, object> filters = new Dictionary<FilterSearchData.Filters, object>
                                                                       {
                                                                           {
                                                                               FilterSearchData.Filters.ToolGroup, "Mill"
                                                                           },
                                                                           {
                                                                               FilterSearchData.Filters.ToolSubGroup, "Cone"
                                                                           }
                                                                       };

            var resultsApi = this.ApiFilteringTest<CutterAssembly>(filters);

            this.App.Ui.ToolsMain.SelectToolType(FilterSearchData.ToolsTypes.Cutters);
            this.App.Ui.ToolsMain.PerformFiltering(filters);
            var resultsUi = this.App.Ui.ToolsMain.GetCuttersResults();

            Assert.AreEqual(resultsUi.Count, resultsApi.Count, "Different number of results from Graph API and UI");
            this.CompareCutterAssemblyRecordsFromApiAndUi(resultsApi, resultsUi);
        }

        [Test, Pairwise]
        [Property("Reference", "TLM-70")]
        public void FilteringCuttersByCombinations(
            [Values("STAHL", null)] string usageMaterial,
            [Values("WPL", null)] string toolMaterial,
            [Values(12000000, 30000000, null)] int? toolSize,
            [Values(25000000, 400000000, null)] int? toolLength,
            [Values(true, false)] bool cooling,
            [Values("Mill", "Drill", null)] string toolGroup,
            [Values(true, false)] bool avaliability)
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
                        {
                            FilterSearchData.Filters.AvaliabilityInStock, avaliability
                        }
                    };


            this.ApiFilteringTest<CutterAssembly>(filters, false);
        }

        //177 - Specific scenario to update TestRail scenarios since Pairwise cases can't have properties.
        [Test, TestCaseSource(typeof(FilteringDataSource), nameof(FilteringDataSource.SpecificTestCases))]
        [Property("Reference", "TLM-64")]
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
                        { FilterSearchData.Filters.ToolSubGroup, toolSubGroup},
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
        [Property("TestCase", "164")]
        [Property("Reference", "TLM-70")]
        public void CuttersEmptySearchGrid()
        {
            this.CheckEmptySearchGridTest(FilterSearchData.ToolsTypes.Cutters);
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-70")]
        [Property("TestCase", "179")]
        public void CutterFilterAndSearch()
        {
            List<string> expectedResult = new List<string> { "000A300Q03004000" };

            Dictionary<FilterSearchData.Filters, object> filters =
                new Dictionary<FilterSearchData.Filters, object>
                    {
                        { FilterSearchData.Filters.Search, "300Q" },
                        { FilterSearchData.Filters.UsageMaterial, "STANDARD" },
                        { FilterSearchData.Filters.ToolMaterial, "WSP" },
                        { FilterSearchData.Filters.ToolSize, 30000000 },
                        { FilterSearchData.Filters.ToolLength, 400000000 },
                        { FilterSearchData.Filters.Cooling, false },
                        { FilterSearchData.Filters.ToolGroup, "Drill" },
                        { FilterSearchData.Filters.ToolSubGroup, "Boring" },
                        {
                            FilterSearchData.Filters.AvaliabilityInStock,
                            false
                        }
                    };

            var resultsApi = this.ApiFilteringTest<CutterAssembly>(filters);

            this.App.Ui.ToolsMain.SelectToolType(FilterSearchData.ToolsTypes.Cutters);
            this.App.Ui.ToolsMain.PerformFiltering(filters);
            var resultsUi = this.App.Ui.ToolsMain.GetCuttersResults();

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
                    this.CompareCutterAssemblyRecordsFromApiAndUi(resultsApi, resultsUi);
                });
        }
       
        public class FilteringDataSource
        {         
        public static IEnumerable PositiveTestCases
            {
                get
                {
                    yield return new TestCaseData(FilterSearchData.Filters.UsageMaterial, "STAHL")
                        .SetProperty("TestCase", "175");
                    yield return new TestCaseData(FilterSearchData.Filters.ToolMaterial, "HSS").SetProperty(
                        "TestCase",
                        "176");
                    yield return new TestCaseData(FilterSearchData.Filters.ToolSize, 12000000).SetProperty(
                        "TestCase",
                        "376");
                    yield return new TestCaseData(FilterSearchData.Filters.ToolLength, 131000000).SetProperty(
                        "TestCase",
                        "377");
                    yield return
                        new TestCaseData(FilterSearchData.Filters.Cooling, true).SetProperty("TestCase", "380");
                    yield return new TestCaseData(FilterSearchData.Filters.Cooling, false);
                    yield return new TestCaseData(FilterSearchData.Filters.ToolGroup, "Mill").SetProperty(
                        "TestCase",
                        "378");
                    yield return new TestCaseData(FilterSearchData.Filters.AvaliabilityInStock, false).SetProperty(
                        "TestCase",
                        "381");                  
                }
            }

            public static IEnumerable NegativeTestCases
            {
                get
                {
                    yield return new TestCaseData(FilterSearchData.Filters.ToolSize, "5875875").SetProperty(
                        "TestCase",
                        "383");
                    yield return new TestCaseData(FilterSearchData.Filters.ToolLength, 875778757).SetProperty(
                        "TestCase",
                        "382");
                    yield return new TestCaseData(FilterSearchData.Filters.AvaliabilityInStock, true);
                }
            }
            
            public static IEnumerable SpecificTestCases
            {               
                get
                {
                    yield return new TestCaseData("STAHL", "WSP", null, 400000000, false, "Drill", null, false,                   
                        new List<string> { "000A300Q02303100",
                                             "000A300Q03004000",
                                             "000A300Q03905100",
                                             "000A300Q05006500",
                                             "000A300Q06408600",
                                             "000A300Q08514000"}).SetProperty("TestCase", "177");

                    yield return new TestCaseData("STANDARD", "WSP", 30000000, 400000000, false, "Drill", "Boring", false, new List<string> { "000A300Q03004000" }).SetProperty("TestCase", "178");
                }
            }
        }
    }
}
