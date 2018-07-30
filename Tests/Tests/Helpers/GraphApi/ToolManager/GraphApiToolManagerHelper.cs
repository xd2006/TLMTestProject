
namespace Tests.Helpers.GraphApi.ToolManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using global::Tests.Managers;
    using global::Tests.Models.ToolManager.GraphQlModels.ToolAssembly;
    using global::Tests.TestsData.Inventory.Enums.FilterSearch;

    using GraphQL.Client;
    using GraphQL.Common.Request;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class GraphApiToolManagerHelper : GraphApiHelperTemplate
    {
        private delegate GraphQLRequest ToolGetRequest(
            Dictionary<FilterSearchData.Filters, object> filters,
            int take,
            int skip,
            string sortingField,
            string sortingOrder);

        public GraphApiToolManagerHelper(ApplicationManager app)
            : base(app)
        {
        }

        new protected GraphQLClient Client => base.Client("ToolManager");

        public List<ToolAssembly> SearchToolAssemblies(string searchTerm, int take = 25, int skip = 0)
        {
            Dictionary<FilterSearchData.Filters, object> filters =
                new Dictionary<FilterSearchData.Filters, object> { { FilterSearchData.Filters.Search, searchTerm } };

            return this.FilterToolAssemblies(filters, take, skip);
        }

        public List<CutterAssembly> SearchCutters(string searchTerm, int take = 25, int skip = 0)
        {
            Dictionary<FilterSearchData.Filters, object> filters =
                new Dictionary<FilterSearchData.Filters, object> { { FilterSearchData.Filters.Search, searchTerm } };

            return this.FilterCutterAssemblies(filters, take, skip);
        }

        public List<Holder> SearchHolders(string searchTerm, int take = 25, int skip = 0)
        {
            Dictionary<FilterSearchData.Filters, object> filters =
                new Dictionary<FilterSearchData.Filters, object> { { FilterSearchData.Filters.Search, searchTerm } };

            return this.FilterHolders(filters, take, skip);
        }

        public List<CutterAssembly> FilterCutterAssemblies(Dictionary<FilterSearchData.Filters, object> filters, int take = 25, int skip = 0)
        {
            return this.FilterTools<CutterAssembly>(filters, take, skip);
        }

        public List<Holder> FilterHolders(Dictionary<FilterSearchData.Filters, object> filters, int take = 25, int skip = 0)
        {
            return this.FilterTools<Holder>(filters, take, skip);
        }

        public List<ToolAssembly> FilterToolAssemblies(
            Dictionary<FilterSearchData.Filters, object> filters,
            int take = 25,
            int skip = 0)
        {
            return this.FilterTools<ToolAssembly>(filters, take, skip);
        }

        public ToolAssembly GetToolAssemblyDetails(string toolId)
        {
            return this.PostRequestWithRetry<ToolAssembly>(
                this.ToolManagerRequests.Details.GetToolAssemblyDetails(toolId),
                "toolAssembly");
        }


        public List<CuttingMaterial> GetCuttingMaterials()
        {
          return this.PostRequestWithRetry<List<CuttingMaterial>>(
                this.ToolManagerRequests.Service.GetCuttingMaterials(),
                "cuttingMaterials");
        }

        public List<CuttingMaterial> GetUsageMaterials()
        {
            return this.PostRequestWithRetry<List<CuttingMaterial>>(
                this.ToolManagerRequests.Service.GetUsageMaterials(),
                "machinedMaterials");
        }

        public List<CutterGroup> GetCutterGroups()
        {
            var result = this.Client.PostAsync(this.ToolManagerRequests.Service.GetCutterGroups());
            var groups = result.Result.GetDataFieldAs<List<CutterGroup>>("cutterGroups");
            return groups;
        }

        private List<T> FilterTools<T>(Dictionary<FilterSearchData.Filters, object> filters, int take = 25, int skip = 0)
        {
            Dictionary<Type, KeyValuePair<ToolGetRequest, string>> requestsData =
                new Dictionary<Type, KeyValuePair<ToolGetRequest, string>>
                    {
                        {
                            typeof(ToolAssembly), new KeyValuePair<ToolGetRequest, string>(
                                this.ToolManagerRequests.Search.GetToolAssemblies,
                                "toolAssemblies")
                        },
                        {
                        typeof(CutterAssembly), new KeyValuePair<ToolGetRequest, string>(
                                                  this.ToolManagerRequests.Search.GetCutterAssemblies,
                                                  "cutterAssemblies")
                        },
                        {
                            typeof(Holder), new KeyValuePair<ToolGetRequest, string>(
                                this.ToolManagerRequests.Search.GetHolders,
                                "holders")
                        }
                    };

            ToolGetRequest request = requestsData[typeof(T)].Key;
            string targetSection = requestsData[typeof(T)].Value;

            var applyFilters = this.TransformFilters(filters);

            if (take > 100 && skip == 0)
            {
                return this.ExecuteFilterRequestByPages<T>(take, applyFilters, request, targetSection);
            }
            else
            {
                var result = this.Client()
                    .PostAsync(request(applyFilters, take, skip, null, null));
                var tools = result.Result.GetDataFieldAs<List<T>>(targetSection);
                return tools;
            }
        }

        private List<T> ExecuteFilterRequestByPages<T>(int take, Dictionary<FilterSearchData.Filters, object> applyFilters, ToolGetRequest request, string targetSection, int pageSize = 25)
        {
         
            List<T> tools = new List<T>();

            int skipped = 0;
            bool moresResultsExist = true;
            while (moresResultsExist && pageSize > 0)
            {
                pageSize = skipped + pageSize > take ? take - skipped : pageSize;

                if (pageSize > 0)
                {
                    int counter = 0;
                    Exception error;
                    do
                    {
                        error = null;
                        try
                        {
                            var result = this.Client.PostAsync(request(applyFilters, pageSize, skipped, null, null));
                            var resultsToAdd = result.Result.GetDataFieldAs<List<T>>(targetSection);
                            if (resultsToAdd.Count > 0)
                            {
                                tools.AddRange(resultsToAdd);
                            }

                            moresResultsExist = resultsToAdd.Count == pageSize;

                            skipped += pageSize;
                        }
                        catch (Exception e)
                        {
                            counter++;
                            Thread.Sleep(500);
                            error = e;
                        }
                    }
                    while (error != null && counter < 10);

                    if (error != null)
                    {
                        throw error;
                    }
                }
            }

            return tools;
        }

        /// <summary>
        /// Transform filters values to usable 
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        private Dictionary<FilterSearchData.Filters, object> TransformFilters(Dictionary<FilterSearchData.Filters, object> filters)
        {
            Dictionary<FilterSearchData.Filters, object> applyFilters =
                new Dictionary<FilterSearchData.Filters, object>(filters);

            if (filters.ContainsKey(FilterSearchData.Filters.ToolGroup))
            {
                applyFilters[FilterSearchData.Filters.ToolGroup] = this.GetCutterGroups()
                    .First(g => g.Name.Equals(filters[FilterSearchData.Filters.ToolGroup])).Id;
            }

            if (filters.ContainsKey(FilterSearchData.Filters.ToolSubGroup))
            {
                applyFilters[FilterSearchData.Filters.ToolSubGroup] = this.GetCutterGroups()
                    .First(g => g.Name.Equals(filters[FilterSearchData.Filters.ToolSubGroup])).Id;
            }

            if (filters.ContainsKey(FilterSearchData.Filters.ToolMaterial))
            {
                applyFilters[FilterSearchData.Filters.ToolMaterial] = this.GetCuttingMaterials()
                    .First(g => g.Name.Equals(filters[FilterSearchData.Filters.ToolMaterial])).Id;
            }

            if (filters.ContainsKey(FilterSearchData.Filters.UsageMaterial))
            {
                applyFilters[FilterSearchData.Filters.UsageMaterial] = this.GetUsageMaterials()
                    .First(g => g.Name.Equals(filters[FilterSearchData.Filters.UsageMaterial])).Id;
            }

            return applyFilters;
        }

        private T PostRequestWithRetry<T>(GraphQLRequest request, string jsonEntity, int retriesNumber = 10)
            where T : new()
        {
            string message = string.Empty;
            var groups = new T();
            int counter = 0;
            do
            {
                try
                {
                    if (!string.IsNullOrEmpty(message))
                    {
                        System.Threading.Thread.Sleep(1000);
                    }

                    var result = this.Client.PostAsync(request);
                    groups = result.Result.GetDataFieldAs<T>(jsonEntity);
                }
                catch (Exception e)
                {
                    message = e.Message;
                }
            }
            while (!string.IsNullOrEmpty(message) && counter++ < retriesNumber);

            Assert.IsTrue(counter < 10, "Can't execute request successfully");

            return groups;
        }
    }
}
