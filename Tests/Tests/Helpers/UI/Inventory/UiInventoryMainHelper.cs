
namespace Tests.Helpers.UI.Inventory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Core.Service;
    using Core.WeDriverService.Extensions;

    using global::Tests.Managers;
    using global::Tests.Models.ToolManager.GraphQlModels.ToolAssembly;
    using global::Tests.Models.ToolManager.UiModels;
    using global::Tests.TestsData.Inventory.Enums.FilterSearch;

    using OpenQA.Selenium;

    public class UiInventoryMainHelper : UiCommonHelper
    {
        public UiInventoryMainHelper(ApplicationManager app)
            : base(app)
        {
        }

        public bool IsInventoryPageOpened()
        {
            return App.Pages.ToolsPages.ToolsMainPage.Opened();
        }

        public Dictionary<FilterSearchData.Filters, bool> AreFiltersEnabled(List<FilterSearchData.Filters> filters)
        {
            Dictionary<FilterSearchData.Filters, bool> filtersStates = new Dictionary<FilterSearchData.Filters, bool>();

            foreach (var filter in filters)
            {
                var filterState = this.App.Pages.ToolsPages.ToolsMainPage.IsFilterEnabled(filter);
                filtersStates.Add(filter, filterState);
            }

            return filtersStates; 
        }
        
        public void PerformSearch(string searchTerm)
        {
            this.UpdateGrid(
                () =>
                    {
                    this.App.Pages.ToolsPages.ToolsMainPage.PopulateSearchField(searchTerm);
                        this.App.Pages.ToolsPages.ToolsMainPage.ClickSearchButton();
                    },
                5);
        }

        public void PerformFiltering(Dictionary<FilterSearchData.Filters, object> filters, bool populateOnly = false)
        {
            var elements = this.App.Pages.ToolsPages.ToolsMainPage.GetRawGridLineElements();

            foreach (var filter in filters.Keys)
            {
                if (filters[filter] != null)
                {
                    switch (filter)
                    {
                        case FilterSearchData.Filters.Search:
                            {
                                this.App.Pages.ToolsPages.ToolsMainPage.PopulateSearchField((string)filters[filter]);
                                break;
                            }

                        case FilterSearchData.Filters.UsageMaterial:
                            {
                                this.App.Pages.ToolsPages.ToolsMainPage.PopulateUsageMaterial((string)filters[filter]);
                                break;
                            }

                        case FilterSearchData.Filters.ToolMaterial:
                            {
                                this.App.Pages.ToolsPages.ToolsMainPage.PopulateToolMaterial((string)filters[filter]);
                                break;
                            }

                        case FilterSearchData.Filters.ToolSize:
                            {
                                this.App.Pages.ToolsPages.ToolsMainPage.PopulateToolSize((int)filters[filter]);
                                break;
                            }

                        case FilterSearchData.Filters.ToolLength:
                            {
                                this.App.Pages.ToolsPages.ToolsMainPage.PopulateToolLength((int)filters[filter]);
                                break;
                            }

                        case FilterSearchData.Filters.ToolGroup:
                            {
                                this.App.Pages.ToolsPages.ToolsMainPage.PopulateToolGroup((string)filters[filter]);
                                break;
                            }

                        case FilterSearchData.Filters.ToolSubGroup:
                            {
                                this.App.Pages.ToolsPages.ToolsMainPage.PopulateToolSubGroup((string)filters[filter]);
                                break;
                            }

                        case FilterSearchData.Filters.Cooling:
                            {
                                this.App.Pages.ToolsPages.ToolsMainPage.SetCoolingFunctionality((bool)filters[filter]);
                                break;
                            }

                        case FilterSearchData.Filters.AvaliabilityInStock:
                            {
                                this.App.Pages.ToolsPages.ToolsMainPage.SetAvaliabilityInStock((bool)filters[filter]);
                                break;
                            }
                    }
                }
            }

            if (!populateOnly)
            {
                this.App.Pages.ToolsPages.ToolsMainPage.ClickSearchButton();

                if (!this.App.Pages.Driver.WaitForElementStaleBool(elements.Last(), 10))
                {
                    this.App.Logger.Warn("It's possible that grid data wasn't updated");
                }
            }
        }

        #region get methods
        public string GetSearchFiledName()
        {
            return this.App.Pages.ToolsPages.ToolsMainPage.GetSearchFieldLabel();
        }

        public string GetSearchFieldValue()
        {
            return this.App.Pages.ToolsPages.ToolsMainPage.GetSearchFieldValue();
        }

        public int GetActivePage()
        {
            return this.App.Pages.ToolsPages.ToolsMainPage.GetActivePage();
        }
        
        public List<ToolAssembly> GetAssembliesResults()
        {
            return this.GetResultsRecords<ToolAssembly>();
        }

        public List<CutterAssembly> GetCuttersResults()
        {
            return this.GetResultsRecords<CutterAssembly>();
        }

        public List<Holder> GetHoldersResults()
        {
            return this.GetResultsRecords<Holder>();
        }

        public List<ToolGridRecord> GetGridRecords()
        {
            return this.App.Pages.ToolsPages.ToolsMainPage.GetRecords();
        }

        private List<T> GetResultsRecords<T>()
            where T : class, new()
        {
            var uiRecords = this.App.Pages.ToolsPages.ToolsMainPage.GetRecords();
             return this.TransformModels<T>(uiRecords);
         }

        private List<T> TransformModels<T>(List<ToolGridRecord> uiRecords)
            where T : class, new()
        {
            List<T> resultRecords = new List<T>();

            foreach (var record in uiRecords)
            {
                if (typeof(T) == typeof(ToolAssembly))
                {
                    var tool = new ToolAssembly();

                    tool.Name = record.Name;
                    tool.CutterAssembly = new CutterAssembly { Cutter = new List<Cutter> { new Cutter() } };
                    tool.CutterAssembly.Cutter[0].Diameter = record.Size;
                    if (record.Length != null) tool.Length = (int)record.Length;
                    tool.Quantity = record.Quantity;
                    resultRecords.Add(tool as T);
                }
                if (typeof(T) == typeof(CutterAssembly))
                {
                    var tool = new CutterAssembly();

                    tool.Name = record.Name;
                    tool.Cutter = new List<Cutter> {new Cutter()};
                    tool.Cutter[0].Diameter = record.Size;
                    if (record.Length != null) tool.Length = (int)record.Length;
                    tool.Quantity = record.Quantity;
                    resultRecords.Add(tool as T);
                }
                if (typeof(T) == typeof(Holder))
                {
                    var tool = new Holder();

                    tool.Name = record.Name;
                    if (record.Length != null) tool.Length = (int)record.Length;
                    tool.Quantity = record.Quantity;
                    resultRecords.Add(tool as T);
                }
            }

            return resultRecords;
        }

        public int GetMaximumNumberOfResults()
        {
            return this.App.Pages.ToolsPages.ToolsMainPage.GetDefinedNumberOfResultsOption();
        }

        #endregion

        public List<string> GetGridColumnsNames()
        {
            return this.App.Pages.ToolsPages.ToolsMainPage.GetGridColumnsNames();
        }
       
        public void SetMaximumNumberOfResults(int number)
        {
            List<int> allowedValues = new List<int> { 5, 10, 25 };
            if (!allowedValues.Contains(number))
            {
                this.App.Logger.Error("Invalid option was provided for 'Number of results' option. Default number is used");
                number = allowedValues.LastOrDefault();
            }          
            this.App.Pages.ToolsPages.ToolsMainPage.SetNumberOfResultsOption(number);

            int num;
            do
            {
                num = this.App.Pages.ToolsPages.ToolsMainPage.GetRawGridLineElements().Count;               
            }
            while (num != number);
        }


        public void ClickNextButton()
        {
            this.UpdateGrid(this.App.Pages.ToolsPages.ToolsMainPage.ClickNextButton);
        }      

        public void ClickPreviousButton()
        {
            this.UpdateGrid(this.App.Pages.ToolsPages.ToolsMainPage.ClickPreviousButton);
        }

        public int ClickRandomPage()
        {
           
            var elements = this.App.Pages.ToolsPages.ToolsMainPage.GetRawGridLineElements();
          
            var pages = this.App.Pages.ToolsPages.ToolsMainPage.GetAvailablePages();
            Random random = new Random();
            var selectedPage = random.Next(1, pages.Count - 1);
            var page = pages[selectedPage];
            this.App.Pages.ToolsPages.ToolsMainPage.ClickPage(page);


            if (!this.App.Pages.Driver.WaitForElementStaleBool(elements.Last(), 10))
            {
                this.App.Logger.Warn("Grid data possibly wasn't updated");
            }

            return int.Parse(page);          
        }

        public void ClickPage(int pageNumber)
        {                     
            this.UpdateGrid(() => this.App.Pages.ToolsPages.ToolsMainPage.ClickPage(pageNumber.ToString()), 8);                 
        }

        public string ClickRandomTool()
        {
            int counter = 0; 
            List<string> tools = new List<string>();
            do
            {
                tools = this.App.Pages.ToolsPages.ToolsMainPage.GetRecords().Select(t => t.Name).ToList();
                if (tools.Count == 0)
                {
                    Thread.Sleep(1000);
                }
            }
            while (tools.Count == 0 && counter++ < 10);

            if (tools.Count == 0)
            {
                throw new Exception("Grid records weren't loaded");
            }

            Random rnd = new Random();
            var tool = tools[rnd.Next(tools.Count)];
            this.App.Pages.ToolsPages.ToolsMainPage.ClickRecord(tool);
            int count = 0;
            while (!this.App.Pages.ToolsPages.ToolInfoPage.Opened() && count++ < 10)
            {
                System.Threading.Thread.Sleep(1000);
            }
            return tool;
        }

        public void ClickTool(string toolName)
        {
            this.App.Pages.ToolsPages.ToolsMainPage.ClickRecord(toolName);
        }

        public Dictionary<FilterSearchData.Filters, object> GetFiltersStates(List<FilterSearchData.Filters> filters)
        {
            Dictionary<FilterSearchData.Filters, object> filtersStates = new Dictionary<FilterSearchData.Filters, object>();

            foreach (var filter in filters)
            {             
                    var filterState = this.App.Pages.ToolsPages.ToolsMainPage.GetFilterState(filter);
                    filtersStates.Add(filter, filterState);
            }
            
            return filtersStates;
        }

        public Dictionary<FilterSearchData.Filters, object> GetFiltersStates(Dictionary<FilterSearchData.Filters, object> filters)
        {
            var filtersNames = filters.Keys.ToList();

            return this.GetFiltersStates(filtersNames);
        }

        public void ClearSearch()
        {
            this.App.Pages.ToolsPages.ToolsMainPage.ClearSearch();
        }

        public List<string> GetFilterLabels()
        {
            this.App.Pages.ToolsPages.ToolsMainPage.WaitForPageLoad();
            var labels = this.App.Pages.ToolsPages.ToolsMainPage.GetFilterLabels();
            this.App.Logger.Info("Next filters are available: ");
            labels.ForEach(e => this.App.Logger.Info(e));
            return labels;
        }

        public void ResetSearchAndFilters()
        {
            this.UpdateGrid(this.App.Pages.ToolsPages.ToolsMainPage.ResetSearchAndFilters, 5);
        }
       

        public void SelectToolType(FilterSearchData.ToolsTypes toolsType)
        {           
            this.App.Pages.ToolsPages.ToolsMainPage.SelectToolType(toolsType);  
            
        }

        public KeyValuePair<string, string> GetCurrentSorting()
        {
            var columnNames = this.App.Pages.ToolsPages.ToolsMainPage.GetGridColumnsNames();

            KeyValuePair<string, string> state = new KeyValuePair<string, string>();
            int i = 0;
            foreach (var name in columnNames)
            {               
                if (name.Contains('▲'))
                {
                    state = new KeyValuePair<string, string>(ServiceMethods.RemoveStringInString(name, "▲").Trim().ToUpper(), "ASC");
                    i++;
                }
                if (name.Contains('▼'))
                {
                    state = new KeyValuePair<string, string>(ServiceMethods.RemoveStringInString(name, "▼").Trim().ToUpper(), "DESC");
                    i++;
                }               
            }

            if (i > 1)
            {
                throw new Exception("More than one columnas are marked as sorted");
            }

            return state;
        }

        public void ClickColumnName(FilterSearchData.GridColumnsNames name)
        {
            this.UpdateGrid(() => this.App.Pages.ToolsPages.ToolsMainPage.ClickColumnName(name.ToString()), 10);
        }

        #region private methods
        private void UpdateGrid(Action action, int timeoutSeconds = 10)
        {
            var elements = this.App.Pages.ToolsPages.ToolsMainPage.GetRawGridLineElements();          
                action.Invoke();

                if (!this.App.Pages.Driver.WaitForElementStaleBool(elements.Last(), timeoutSeconds))
                {
                    this.App.Logger.Warn("Grid data possibly wasn't updated");
                }
         }
               
        #endregion

        public List<string> GetGroupsLabels()
        {
            return this.App.Pages.ToolsPages.ToolsMainPage.GetGroupsLabels();
        }

        public FilterSearchData.ToolsTypes GetActiveToolType()
        {
            Dictionary<string, FilterSearchData.ToolsTypes> toolTypes =
                new Dictionary<string, FilterSearchData.ToolsTypes>
                    {
                        {
                            "Assemblies",
                            FilterSearchData.ToolsTypes.Assemblies
                        },
                        {
                            "Cutters",
                            FilterSearchData.ToolsTypes.Cutters
                        },
                        {
                            "Holders",
                            FilterSearchData.ToolsTypes.Holders
                        }
                    };

            var type = this.App.Pages.ToolsPages.ToolsMainPage.GetActiveToolType();

            if (!toolTypes.Keys.Contains(type))
            {
                throw new Exception($"Tool type {type} is not valid");
            }
            return toolTypes[type];
        }
    }
}
