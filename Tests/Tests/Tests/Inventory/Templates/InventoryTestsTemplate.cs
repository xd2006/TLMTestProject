
namespace Tests.Tests.Inventory.Templates
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Core.Service;

    using global::Tests.Models.ToolManager.GraphQlModels.ToolAssembly;
    using global::Tests.TestsData.Inventory.Enums.FilterSearch;

    using NUnit.Framework;

    public abstract class InventoryTestsTemplate : TestBase
    {
        [SetUp]
        public void BeforeEachTest()
        {
           this.App.BaseUrl = Parameters.Parameters.ToolManagerUrl;
        }

        private Dictionary<FilterSearchData.Filters, string> filtersDropDownsAndTextFields =
            new Dictionary<FilterSearchData.Filters, string>()
                {
                    {FilterSearchData.Filters.UsageMaterial, "Select usage material" },
                    { FilterSearchData.Filters.ToolMaterial, "Select material" },
                    { FilterSearchData.Filters.ToolSize, "Tool size" },
                    { FilterSearchData.Filters.ToolLength, "Tool length" },
                    { FilterSearchData.Filters.ToolGroup, "Select tool group" },
                    { FilterSearchData.Filters.Type, "Select type" }
                };

        protected void CheckFiltersInitialState(Dictionary<FilterSearchData.Filters, object> filtersToCheck)
        {
           List<FilterSearchData.Filters> filtersCheckBoxes =
                new List<FilterSearchData.Filters>()
                    {
                        FilterSearchData.Filters.Cooling,
                        FilterSearchData.Filters.AvaliabilityInStock,
                    };

            var filtersToCheckList = filtersToCheck.Keys.ToList();

            var filtersDropDownsAndTextFieldsFilters = filtersToCheckList.Intersect(filtersDropDownsAndTextFields.Keys).ToList();
            filtersCheckBoxes = filtersToCheckList.Intersect(filtersCheckBoxes).ToList();

            App.Ui.ToolsMain.ClickFiltersButton();
            var filtersStates = this.App.Ui.ToolsMain.GetFiltersStates(filtersDropDownsAndTextFieldsFilters);

            foreach (var key in filtersStates.Keys)
            {
                Assert.That(filtersStates[key].Equals(filtersDropDownsAndTextFields[key]), "Wrong initial state for " + key);
            }

            filtersStates = this.App.Ui.ToolsMain.GetFiltersStates(filtersCheckBoxes);

            foreach (var key in filtersStates.Keys)
            {
                Assert.That(filtersStates[key].Equals("False"), "Wrong initial state for " + key);
            }

            var filters = new List<FilterSearchData.Filters>(filtersDropDownsAndTextFieldsFilters);               
            filters.AddRange(filtersCheckBoxes);

            var enabledDisabledStates = this.App.Ui.ToolsMain.AreFiltersEnabled(filters);
            foreach (var key in enabledDisabledStates.Keys)
            {
                if (key != FilterSearchData.Filters.Type)
                {
                    Assert.True(enabledDisabledStates[key], "Wrong availability state for " + key + ". Should be enabled");
                }
                else
                {
                    Assert.False(enabledDisabledStates[key], "Wrong availability state for " + key + ". Should be disabled");
                }
            }
        }

        protected void CheckFiltersInitialState()
        {
            List<FilterSearchData.Filters> filtersToCheck =
                new List<FilterSearchData.Filters>
                    {
                        FilterSearchData.Filters.UsageMaterial,
                        FilterSearchData.Filters.ToolMaterial,
                        FilterSearchData.Filters.ToolSize,
                        FilterSearchData.Filters.ToolLength,
                        FilterSearchData.Filters.ToolGroup,
                        FilterSearchData.Filters.Type,
                        FilterSearchData.Filters.Cooling,
                        FilterSearchData.Filters.AvaliabilityInStock,
                    };

            this.CheckFiltersInitialState(filtersToCheck);
        }

        protected void CheckFiltersInitialState(List<FilterSearchData.Filters> filters)
        {
            Dictionary<FilterSearchData.Filters, object> filtersToCheck = new Dictionary<FilterSearchData.Filters, object>();
            foreach (var filter in filters)
            {
                filtersToCheck.Add(filter, null);
            }

            this.CheckFiltersInitialState(filtersToCheck);
        }

        protected void CompareToolAssemblyRecordsFromApiAndUi(List<ToolAssembly> itemsApi, List<ToolAssembly> itemsUi)
        {
            bool itemsAreEqual = true;
            foreach (var item in itemsUi)
            {
                var itemApi = itemsApi.First(i => i.Name == item.Name);

                if (itemApi.CutterAssembly.Cutter.Diameter != item.CutterAssembly.Cutter.Diameter
                    || itemApi.Length != item.Length || itemApi.Quantity != item.Quantity)
                {
                    itemsAreEqual = false;
                }

                Assert.True(
                    itemsAreEqual,
                    "Items returned on Ui are not equal to items from Graph Api. Wrong UI item is " + item.Name);
            }
        }

        protected void CompareCutterAssemblyRecordsFromApiAndUi(List<CutterAssembly> itemsApi, List<CutterAssembly> itemsUi)
        {
            bool itemsAreEqual = true;
            foreach (var item in itemsUi)
            {
                var itemApi = itemsApi.FirstOrDefault(i => i.Name == item.Name 
                                                           && i.Cutter.Diameter == item.Cutter.Diameter 
                                                           && i.Length == item.Length);

                if (itemApi == null || itemApi.Quantity != item.Quantity)
                {
                    itemsAreEqual = false;
                }

                Assert.True(
                    itemsAreEqual,
                    "Items returned on Ui are not equal to items from Graph Api. Wrong UI item is " + item.Name);
            }
        }

        protected void CompareHolderRecordsFromApiAndUi(List<Holder> itemsApi, List<Holder> itemsUi)
        {
            bool itemsAreEqual = true;
            foreach (var item in itemsUi)
            {
                var itemApi = itemsApi.First(i => i.Name == item.Name);

                if (itemApi.Length != item.Length
                    || itemApi.Quantity != item.Quantity)
                {
                    itemsAreEqual = false;
                }

                Assert.True(
                    itemsAreEqual,
                    "Items returned on Ui are not equal to items from Graph Api. Wrong UI item is " + item.Name);
            }
        }

        protected KeyValuePair<bool, List<string>> CheckToolAssemblyItemsAreCorrect(FilterSearchData.Filters filterType, object filterValue, List<ToolAssembly> items)
        {
            List<string> wrongItemsNames = new List<string>();

            bool apiItemsAreCorrect = true;
            switch (filterType)
            {
                case FilterSearchData.Filters.Search:
                    {
                        string searchTerm = (string)filterValue;
                        apiItemsAreCorrect = items.All(
                            item => item.Name.Contains(searchTerm) || item.LegacyName1.Contains(searchTerm)
                                                                   || item.LegacyName2.Contains(searchTerm)
                                                                   || item.LegacyName3.Contains(searchTerm)
                                                                   || item.LegacyName4.Contains(searchTerm)
                                                                   || item.Description.Contains(searchTerm));
                        //            Todo: update after updating DB. 
                        //            Can't check search against LegacyName1-4, Descriptions on the current stage since these fields are not populated in the DB

                        break;
                    }

                case FilterSearchData.Filters.ToolMaterial:
                    {
                        var materialId = this.App.GraphApi.ToolManager.GetCuttingMaterials().First(e => e.Name.Equals((string)filterValue)).Id;
                        apiItemsAreCorrect =
                            items.All(i => i.CutterAssembly.Material.Id.Equals(materialId));
                        break;
                    }

                case FilterSearchData.Filters.AvaliabilityInStock:
                    {
                        if ((bool)filterValue)
                        {
                            apiItemsAreCorrect = items.All(i => i.Quantity > 0);
                        }
                        else
                        {
                            //Due to current API logic
                            apiItemsAreCorrect = items.All(i => i.Quantity >= 0);
                        }

                        break;
                    }
                case FilterSearchData.Filters.Cooling:
                    {
                        if (!(bool)filterValue)
                        {
                            //Due to current Graph API logic
                            apiItemsAreCorrect = true;
                        }
                        else
                        {
                            apiItemsAreCorrect = items.All(e => e.CutterAssembly.Cutter.Cooling);
                            if (!apiItemsAreCorrect)
                            {
                                wrongItemsNames = items.Where(i => !i.CutterAssembly.Cutter.Cooling)
                                    .Select(i => i.Name).ToList();
                            }
                        }

                        break;
                    }
                case FilterSearchData.Filters.ToolGroup:
                    {
                        var groups = this.App.GraphApi.ToolManager.GetCutterGroups();
                        var currentGroup = groups.First(g => g.Name.Equals(filterValue)).Id;

                        // Check that subgroup of ToolAssembly is child to defined group.
                        apiItemsAreCorrect = items.All(
                            i => groups.First(g => g.Id.Equals(i.CutterAssembly.Cutter.GeometryStr)).ParentId
                                .Equals(currentGroup));
                        break;
                    }

                case FilterSearchData.Filters.Type:
                    {
                        var groups = this.App.GraphApi.ToolManager.GetCutterGroups();
                        var currentGroup = groups.First(g => g.Name.Equals(filterValue)).Id;

                        apiItemsAreCorrect = items.All(
                            i => i.CutterAssembly.Cutter.GeometryStr.Equals(currentGroup));
                        break;
                    }

                case FilterSearchData.Filters.ToolLength:
                    {
                        apiItemsAreCorrect = items.All(i => i.Length == (int)filterValue);
                        break;
                    }

                case FilterSearchData.Filters.ToolSize:
                    {
                        apiItemsAreCorrect = items.All(i => i.CutterAssembly.Cutter.Diameter == (int)filterValue);
                        break;
                    }

                case FilterSearchData.Filters.UsageMaterial:
                    {
                        apiItemsAreCorrect = items.All(i => i.UsageMaterials.Contains(filterValue));
                        if (!apiItemsAreCorrect)
                        {
                            wrongItemsNames = items.Where(i => !i.UsageMaterials.Contains(filterValue))
                                .Select(e => e.Name).ToList();
                        }

                        break;
                    }
            }

            var result = new KeyValuePair<bool, List<string>>(apiItemsAreCorrect, wrongItemsNames);
            return result;
        }

        protected KeyValuePair<bool, List<string>> CheckCutterAssemblyItemsAreCorrect(
            FilterSearchData.Filters filterType,
            object filterValue,
            List<CutterAssembly> items)
        {
            List<string> wrongItemsNames = new List<string>();

            bool apiItemsAreCorrect = true;
            switch (filterType)
            {
                case FilterSearchData.Filters.Search:
                    {
                        string searchTerm = (string)filterValue;
                        apiItemsAreCorrect = items.All(
                            item => item.Name.Contains(searchTerm));                      
                        break;
                    }

                case FilterSearchData.Filters.ToolMaterial:
                    {

                        var materialId = this.App.GraphApi.ToolManager.GetCuttingMaterials().First(e => e.Name.Equals((string)filterValue)).Id;
                        apiItemsAreCorrect =
                            items.All(i => i.Material.Id.Equals(materialId));
                        break;
                    }

                case FilterSearchData.Filters.AvaliabilityInStock:
                    {
                        if ((bool)filterValue)
                        {
                            apiItemsAreCorrect = items.All(i => i.Quantity > 0);
                        }
                        else
                        {
                            //Due to current API logic
                            apiItemsAreCorrect = items.All(i => i.Quantity >= 0);
                        }

                        break;
                    }

                case FilterSearchData.Filters.Cooling:
                    {
                        if (!(bool)filterValue)
                        {
                            //Due to current Graph API logic
                            apiItemsAreCorrect = true;
                        }
                        else
                        {
                            apiItemsAreCorrect = items.All(e => e.Cutter.Cooling);
                            if (!apiItemsAreCorrect)
                            {
                                wrongItemsNames = items.Where(i => !i.Cutter.Cooling)
                                    .Select(i => i.Name).ToList();
                            }
                        }

                        break;
                    }

                case FilterSearchData.Filters.ToolGroup:
                    {
                        var groups = this.App.GraphApi.ToolManager.GetCutterGroups();
                        var currentGroup = groups.First(g => g.Name.Equals(filterValue)).Id;

                        // Check that subgroup of ToolAssembly is child to defined group.
                        apiItemsAreCorrect = items.All(
                            i => groups.First(g => g.Id.Equals(i.Cutter.GeometryStr)).ParentId
                                .Equals(currentGroup));
                        break;
                    }

                case FilterSearchData.Filters.Type:
                    {
                        var groups = this.App.GraphApi.ToolManager.GetCutterGroups();
                        var currentGroup = groups.First(g => g.Name.Equals(filterValue)).Id;

                        apiItemsAreCorrect = items.All(
                            i => i.Cutter.GeometryStr.Equals(currentGroup));
                        break;
                    }

                case FilterSearchData.Filters.ToolLength:
                    {
                        apiItemsAreCorrect = items.All(i => i.Length == (int)filterValue);
                        break;
                    }

                case FilterSearchData.Filters.ToolSize:
                    {
                        apiItemsAreCorrect = items.All(i => i.Cutter.Diameter == (int)filterValue);
                        break;
                    }

                case FilterSearchData.Filters.UsageMaterial:
                    {
                        apiItemsAreCorrect = items.All(i => i.UsageMaterials.Contains(filterValue));
                        if (!apiItemsAreCorrect)
                        {
                            wrongItemsNames = items.Where(i => !i.UsageMaterials.Contains(filterValue))
                                .Select(e => e.Name).ToList();
                        }

                        break;
                    }
            }

            var result = new KeyValuePair<bool, List<string>>(apiItemsAreCorrect, wrongItemsNames);
            return result;
            
        }

        protected KeyValuePair<bool, List<string>> CheckHolderItemsAreCorrect(
           FilterSearchData.Filters filterType,
           object filterValue,
           List<Holder> items)
        {
            List<string> wrongItemsNames = new List<string>();

            bool apiItemsAreCorrect = true;
            switch (filterType)
            {
                case FilterSearchData.Filters.Search:
                    {
                        string searchTerm = (string)filterValue;
                        apiItemsAreCorrect = items.All(
                            item => item.Name.Contains(searchTerm));
                        break;
                    }

               case FilterSearchData.Filters.AvaliabilityInStock:
                    {
                        if ((bool)filterValue)
                        {
                            apiItemsAreCorrect = items.All(i => i.Quantity > 0);
                        }
                        else
                        {
                            // Due to current API logic - if filterVlaue is false it returns both true and false entities
                            apiItemsAreCorrect = items.All(i => i.Quantity >= 0);
                        }

                        break;
                    }

                case FilterSearchData.Filters.Cooling:
                    {
                        if (!(bool)filterValue)
                        {
                            // Due to current API logic - if filterVlaue is false it returns both true and false entities
                            apiItemsAreCorrect = true;
                        }
                        else
                        {
                           apiItemsAreCorrect = items.All(e => e.Cooling);
                            if (!apiItemsAreCorrect)
                            {
                                wrongItemsNames = items.Where(i => !i.Cooling)
                                    .Select(e => e.Name).ToList();
                            }
                        }

                        break;
                    }
               
                case FilterSearchData.Filters.ToolLength:
                    {
                        apiItemsAreCorrect = items.All(i => i.Length == (int)filterValue);
                        break;
                    }
            }

            var result = new KeyValuePair<bool, List<string>>(apiItemsAreCorrect, wrongItemsNames);
            return result;

        }

        protected Dictionary<FilterSearchData.Filters, object> FormDefinedFiltersDictionary(Dictionary<FilterSearchData.Filters, object> filters)
        {
            Dictionary<FilterSearchData.Filters, object> filtersToApply = new Dictionary<FilterSearchData.Filters, object>();

            foreach (var filter in filters)
            {
                if (filter.Value != null)
                {
                    filtersToApply.Add(filter.Key, filter.Value);
                }
            }

            return filtersToApply;
        }

        protected List<T> ApiFilteringTest<T>(
            Dictionary<FilterSearchData.Filters, object> filters,
            bool recordsShouldBeReceived)
        {
            return this.ApiFilteringTest<T>(filters, 1000, recordsShouldBeReceived);
        }

        /// <summary>
        /// recordsShouldBeReceived - sets either records should be received by request or not
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filters"></param>
        /// <param name="take"></param>
        /// <param name="recordsShouldBeReceived"></param>
        /// <param name="positive"></param>
        /// <returns></returns>
        protected List<T> ApiFilteringTest<T>(
            Dictionary<FilterSearchData.Filters, object> filters,
            int take = 1000,
            bool recordsShouldBeReceived = true,
            bool positive = true)
        {
            var filtersToApply = this.FormDefinedFiltersDictionary(filters);

            var results = new List<T>();
            if (typeof(T) == typeof(Holder))
            {
                results = this.App.GraphApi.ToolManager.FilterHolders(filtersToApply, take) as List<T>;
            }

            if (typeof(T) == typeof(CutterAssembly))
            {
                results = this.App.GraphApi.ToolManager.FilterCutterAssemblies(filtersToApply, take) as List<T>;
            }

            if (typeof(T) == typeof(ToolAssembly))
            {
                results = this.App.GraphApi.ToolManager.FilterToolAssemblies(filtersToApply, take) as List<T>;
            }

            KeyValuePair<bool, List<string>> apiItemsAreCorrect =
                new KeyValuePair<bool, List<string>>(true, new List<string>());

            if (recordsShouldBeReceived)
            {
                if (positive)
                {
                    Assert.True(results.Count > 0, "Graph API didn't return any data");
                }
                else
                {
                    Assert.True(
                        results.Count == 0,
                        $"Results shouldn't be returned but they were. Results count is {results.Count}");
                }
            }

            if (results.Count > 0)
            {
                foreach (var filter in filtersToApply)
                {
                    if (typeof(T) == typeof(Holder))
                    {
                        apiItemsAreCorrect = this.CheckHolderItemsAreCorrect(filter.Key, filter.Value, results as List<Holder>);
                    }

                    if (typeof(T) == typeof(CutterAssembly))
                    {
                        apiItemsAreCorrect = this.CheckCutterAssemblyItemsAreCorrect(filter.Key, filter.Value, results as List<CutterAssembly>);
                    }

                    if (typeof(T) == typeof(ToolAssembly))
                    {
                        apiItemsAreCorrect = this.CheckToolAssemblyItemsAreCorrect(filter.Key, filter.Value, results as List<ToolAssembly>);
                    }
                }
            }

            Assert.True(
                apiItemsAreCorrect.Key,
                "Not all results are correct."
                + $"Wrong items are: {Environment.NewLine} {ServiceMethods.ListToString(apiItemsAreCorrect.Value)}");

            return results;
        }

        protected void ResetFiltersTest(
            Action resetMethod,
            Dictionary<FilterSearchData.Filters, object> filters,
            FilterSearchData.ToolsTypes toolType,
            bool resetToolType = false)
        {          
            this.App.Ui.ToolsMain.SelectToolType(toolType);

            var resultsBuforeFilteringUi = this.App.Ui.ToolsMain.GetGridRecords();

            this.App.Ui.ToolsMain.PerformFiltering(filters);
            var resultsUi = this.App.Ui.ToolsMain.GetGridRecords();

            var states = this.App.Ui.ToolsMain.GetFiltersStates(filters);

            foreach (var filter in filters.Keys)
            {
                Assert.AreEqual(
                    filters[filter] == null ? filtersDropDownsAndTextFields[filter] : filters[filter].ToString(),
                    states[filter],
                    $"Filter {filter} in a wrong state. Actual is {states[filter]} but should be {filters[filter]}");
            }
            
            resetMethod.Invoke();
            
            var resultsUiAfterUpdate = this.App.Ui.ToolsMain.GetGridRecords();

            Assert.That(
                resultsUi.Count != resultsUiAfterUpdate.Count &&
                !resultsUiAfterUpdate.Contains(resultsUi.First()),
                "Tools Main page wasn't reset");

            if (!resetToolType)
            {
                Assert.That(
                    resultsUiAfterUpdate.Select(e => e.Name).ToList()
                        .SequenceEqual(resultsBuforeFilteringUi.Select(e => e.Name).ToList()),
                    "Tools Main page wasn't reset");
            }
            else
            {
                Assert.That(this.App.Ui.ToolsMain.GetActiveToolType().Equals(FilterSearchData.ToolsTypes.Tools));
            }

            this.CheckFiltersInitialState(filters);
        }

        protected void CheckEmptySearchGridTest(FilterSearchData.ToolsTypes toolType)
        {
            var expectedColumns = new List<string> { "NAME", "SIZE", "LENGTH", "QUANTITY" };

            this.App.Ui.ToolsMain.SelectToolType(toolType);
            this.App.Ui.ToolsMain.PerformSearch("zz###"); //Not existing tool
            var columnNames = this.App.Ui.ToolsMain.GetGridColumnsNames();
            columnNames = ServiceMethods.RemoveStringsInList(columnNames, new List<string> { "▲", "▼" });
            List<string> processedNames = new List<string>();
            columnNames.ForEach(e => processedNames.Add(e.ToUpper()));

            Assert.That(processedNames.SequenceEqual(expectedColumns));

            var results = this.App.Ui.ToolsMain.GetAssembliesResults();
            Assert.That(results.Count == 0, "There are shouldn't be results for not existing tool");
        }
    }
}
