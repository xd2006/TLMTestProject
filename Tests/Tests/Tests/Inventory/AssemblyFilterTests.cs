
namespace Tests.Tests.Inventory
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Core.Service;

    using global::Tests.Models.ToolManager.GraphQlModels.ToolAssembly;
    using global::Tests.Tests.Inventory.Templates;
    using global::Tests.TestsData.Inventory.Enums.FilterSearch;

    using NUnit.Framework;

    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [Category("Filter")]
    public class FilterTests : InventoryTestsTemplate
    {
        [Test]
        [Category("UI")]
        [Property("TestCase", "129")]
        [Property("Reference", "TLM-64")]

        public void FiltersPresence()
        {
            List<string> filters = new List<string>
                                       {
                                           "Usage material:",
                                           "Tool material:",
                                           "Tool size:",
                                           "Tool length:",
                                           "Tool group:",
                                           "Cooling functionality:",
                                           "Availability in stock:"
                                       };

            var filtersLabels = this.App.Ui.ToolsMain.GetFilterLabels();

            Assert.That(filters.SequenceEqual(filtersLabels), "Wrong labels are displayed");
        }

        [Test]
        [Category("UI")]
        [Property("TestCase", "130")]
        [Property("Reference", "TLM-64")]
        public void FiltersInitialState()
        {
            this.CheckFiltersInitialState();
        }

        [Test, TestCaseSource(typeof(FilteringDataSource), nameof(FilteringDataSource.PositiveTestCases))]
        [Category("UI")]
        //        [Parallelizable(ParallelScope.None)]
        [Property("Reference", "TLM-64")]
        public void AssembliesFilteringPositive(FilterSearchData.Filters filterType, object filterValue)
        {
            int take = 50; //temporary to check performance

            var itemsApi = this.App.GraphApi.ToolManager.FilterToolAssemblies(
                new Dictionary<FilterSearchData.Filters, object> { { filterType, filterValue } },
                take);

            Assert.True(itemsApi.Count > 0, "Graph API didn't return any data");

            var apiItemsAreCorrect = this.CheckToolAssemblyItemsAreCorrect(filterType, filterValue, itemsApi);

            Assert.True(
                apiItemsAreCorrect.Key,
                "Not all items from Graph Api correspond to the applied filter. "
                + $"Wrong items are: {Environment.NewLine} {ServiceMethods.ListToString(apiItemsAreCorrect.Value)}");

            this.App.Ui.ToolsMain.PerformFiltering(
                new Dictionary<FilterSearchData.Filters, object> { { filterType, filterValue } });
            var itemsUi = this.App.Ui.ToolsMain.GetAssembliesResults();

            this.CompareToolAssemblyRecordsFromApiAndUi(itemsApi, itemsUi);
        }

        [Test, TestCaseSource(typeof(FilteringDataSource), nameof(FilteringDataSource.NegativeTestCases))]
        [Property("Reference", "TLM-64")]
        public void FilteringNegative(FilterSearchData.Filters filterType, object filterValue)
        {
            var tools = this.App.GraphApi.ToolManager.FilterToolAssemblies(
                new Dictionary<FilterSearchData.Filters, object> { { filterType, filterValue } });
            Assert.That(tools.Count == 0);
        }

        //ToDo: update values to get more results
        [Test, Pairwise]
        [Property("Reference", "TLM-64")]
        public void FilteringToolAssembliesByCombinations(
            [Values(null)] string usageMaterial, //Update after adding needed materials in db
            [Values("HSS", null)] string toolMaterial,
            [Values(4000000, 4400000, null)] int? toolSize,
            [Values(178000000, 173000000, null)] int? toolLength,
            [Values(true, false)] bool cooling,
            [Values("Mill", "Drill", null)] string toolGroup,
            [Values(false)] bool avaliability)
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
                        { FilterSearchData.Filters.ToolGroup, toolGroup }
                    };


            this.ApiFilteringTest<ToolAssembly>(filters, false);
        }


        [Test, Sequential]
        [Property("Reference", "TLM-64")]
        public void FilteringByGroupAndSubgroupApi(
            [Values("Drill", "Mill", "Mill", "Drill")]
            string toolGroup,
            [Values("Twist", "Cone", "End", "Spot")]
            string subGroup)
        {
            Dictionary<FilterSearchData.Filters, object> filters =
                new Dictionary<FilterSearchData.Filters, object>
                    {
                        { FilterSearchData.Filters.ToolGroup, toolGroup },
                        { FilterSearchData.Filters.ToolSubGroup, subGroup }
                    };


            this.ApiFilteringTest<ToolAssembly>(filters);
        }


        //138 - Specific scenario to update TestRail scenarios since Pairwise cases can't have properties.
        //139 - InventoryMain by all fulters simultaneously
        [Test, TestCaseSource(typeof(FilteringDataSource), nameof(FilteringDataSource.SpecificTestCases))]
        [Property("Reference", "TLM-64")]
        public void FilteringByCombinationsSpecific(
            string usageMaterial,
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


            var results = this.ApiFilteringTest<ToolAssembly>(filters);

            Assert.Multiple(
                () =>
                    {
                        Assert.True(results.Count.Equals(expectedNames.Count), "Invalid number of items was returned");
                        Assert.True(results.All(r => expectedNames.Contains(r.Name)), "Invalid results were returned");
                    });
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-64")]
        [Property("TestCase", "140")]
        public void AssemblyFilterAndSearch()
        {
            List<string> expectedResult = new List<string> { "120W158L000F6060" };

            Dictionary<FilterSearchData.Filters, object> filters =
                new Dictionary<FilterSearchData.Filters, object>
                    {
                        { FilterSearchData.Filters.Search, "W158L" },
                        { FilterSearchData.Filters.UsageMaterial, null },
                        { FilterSearchData.Filters.ToolMaterial, "WPL" },
                        { FilterSearchData.Filters.ToolSize, 12000000 },
                        { FilterSearchData.Filters.ToolLength, 186000000 },
                        { FilterSearchData.Filters.Cooling, false },
                        { FilterSearchData.Filters.ToolGroup, null },
                        {
                            FilterSearchData.Filters.AvaliabilityInStock,
                            false
                        }
                    };

            var resultsApi = this.ApiFilteringTest<ToolAssembly>(filters);

            this.App.Ui.ToolsMain.PerformFiltering(filters);
            var resultsUi = this.App.Ui.ToolsMain.GetAssembliesResults();

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
                        this.CompareToolAssemblyRecordsFromApiAndUi(resultsApi, resultsUi);
                    });
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-64")]
        [Property("TestCase", "136")]
        public void ToolGroupDropdownEnabling()
        {
            List<FilterSearchData.Filters> filtersToCheck =
                new List<FilterSearchData.Filters>()
                    {
                        FilterSearchData.Filters.ToolGroup,
                        FilterSearchData.Filters.ToolSubGroup
                    };


            Dictionary<FilterSearchData.Filters, object> filters =
                new Dictionary<FilterSearchData.Filters, object> { { FilterSearchData.Filters.ToolGroup, "Mill" } };
            this.App.Ui.ToolsMain.PerformFiltering(filters, true);

            var enabledDisabledStates = this.App.Ui.ToolsMain.AreFiltersEnabled(filtersToCheck);
            foreach (var key in enabledDisabledStates.Keys)

                Assert.True(enabledDisabledStates[key], "Wrong availability state for " + key + ". Should be enabled");
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-64")]
        [Property("TestCase", "137")]
        public void ToolGroupAndSubgroupFiltering()
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

            var resultsApi = this.ApiFilteringTest<ToolAssembly>(filters);

            this.App.Ui.ToolsMain.PerformFiltering(filters);
            var resultsUi = this.App.Ui.ToolsMain.GetAssembliesResults();

            Assert.AreEqual(resultsUi.Count, resultsApi.Count, "Different number of results from Graph API and UI");
            this.CompareToolAssemblyRecordsFromApiAndUi(resultsApi, resultsUi);
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-64")]
        [Property("TestCase", "141")]
        public void ResetFiltersAssemblies()
        {
            Dictionary<FilterSearchData.Filters, object> filters =
                new Dictionary<FilterSearchData.Filters, object>
                    {
                        { FilterSearchData.Filters.Search, "W158L" },
                        { FilterSearchData.Filters.UsageMaterial, null },
                        { FilterSearchData.Filters.ToolMaterial, "WPL" },
                        { FilterSearchData.Filters.ToolSize, 12000000 },
                        { FilterSearchData.Filters.ToolLength, 186000000 },
                        { FilterSearchData.Filters.Cooling, false },
                        { FilterSearchData.Filters.ToolGroup, null },
                        { FilterSearchData.Filters.AvaliabilityInStock, false }
                    };
            this.ResetFiltersTest(this.App.Ui.ToolsMain.ResetSearchAndFilters, filters, FilterSearchData.ToolsTypes.Assemblies);        
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-64")]
        [Property("TestCase", "142")]
        public void ResetFiltersByBrowserRefreshAssemblies()
        {
            Dictionary<FilterSearchData.Filters, object> filters =
                                new Dictionary<FilterSearchData.Filters, object>
                                    {
                                        { FilterSearchData.Filters.Search, "W158L" },
                                        { FilterSearchData.Filters.UsageMaterial, null },
                                        { FilterSearchData.Filters.ToolMaterial, "WPL" },
                                        { FilterSearchData.Filters.ToolSize, 12000000 },
                                        { FilterSearchData.Filters.ToolLength, 186000000 },
                                        { FilterSearchData.Filters.Cooling, false },
                                        { FilterSearchData.Filters.ToolGroup, null },
                                        { FilterSearchData.Filters.AvaliabilityInStock, false }
                                    };
            this.ResetFiltersTest(this.App.Ui.ToolsMain.RefreshPage, filters, FilterSearchData.ToolsTypes.Assemblies, true);
        }
    }



    public class FilteringDataSource
    {
        public static IEnumerable PositiveTestCases
        {
            get
            {
                yield return new TestCaseData(FilterSearchData.Filters.UsageMaterial, "STAHL")
                    .SetProperty("TestCase", "131").SetProperty("Bug", "TLM-174");
                yield return new TestCaseData(FilterSearchData.Filters.ToolMaterial, "HSS").SetProperty("TestCase", "132");
                yield return new TestCaseData(FilterSearchData.Filters.ToolSize, 12000000).SetProperty("TestCase", "133");
                yield return new TestCaseData(FilterSearchData.Filters.ToolLength, 131000000).SetProperty("TestCase", "134");
                yield return new TestCaseData(FilterSearchData.Filters.Cooling, true).SetProperty("TestCase", "135");
                yield return new TestCaseData(FilterSearchData.Filters.Cooling, false).SetProperty("TestCase", "341");
                yield return new TestCaseData(FilterSearchData.Filters.ToolGroup, "Mill").SetProperty("TestCase", "197");
                yield return new TestCaseData(FilterSearchData.Filters.AvaliabilityInStock, true).SetProperty(
                    "TestCase",
                    "198");
                yield return new TestCaseData(FilterSearchData.Filters.AvaliabilityInStock, false).SetProperty("TestCase", "344");               
            }
        }

        public static IEnumerable NegativeTestCases
        {
            get
            {
                yield return new TestCaseData(FilterSearchData.Filters.ToolSize, "5875875").SetProperty("TestCase", "342");
                yield return new TestCaseData(FilterSearchData.Filters.ToolLength, "875778757").SetProperty("TestCase", "343");
            }
        }

        public static IEnumerable SpecificTestCases
        {
            get
            {
                yield return new TestCaseData(null, "WPL", 12000000, 186000000, null, null, null, null, new List<string> { "120W158D000F6060", "120W158H000F6060", "120W158L000F6060" }).SetProperty("TestCase", "138");
                yield return new TestCaseData(null, "HSS", 4400000, 173000000, true, "Drill", "Twist", null, new List<string> { "044B044SH0173060" }).SetProperty("TestCase", "139");
            }
        }
    }
}
