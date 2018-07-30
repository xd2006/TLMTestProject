
namespace Tests.GraphApiRequests.Inventory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::Tests.Models.ToolManager.GraphQlModels.ToolAssembly;
    using global::Tests.TestsData.Inventory.Enums.FilterSearch;

    using GraphQL.Common.Request;

    public class SearchRequests
    {
        private static List<FilterSearchData.Filters> ToolAssemblyFilters =
            new List<FilterSearchData.Filters>()
                {
                    FilterSearchData.Filters.Cooling,
                    FilterSearchData.Filters.AvaliabilityInStock,
                    FilterSearchData.Filters.Search,
                    FilterSearchData.Filters.ToolLength,
                    FilterSearchData.Filters.ToolMaterial,
                    FilterSearchData.Filters.ToolSize,
                    FilterSearchData.Filters.ToolSubGroup,
                    FilterSearchData.Filters.UsageMaterial,
                    FilterSearchData.Filters.ToolGroup
                };
        private static List<FilterSearchData.Filters> CutterFilters =
            new List<FilterSearchData.Filters>()
                {
                    FilterSearchData.Filters.Cooling,
                    FilterSearchData.Filters.AvaliabilityInStock,
                    FilterSearchData.Filters.Search,
                    FilterSearchData.Filters.ToolLength,
                    FilterSearchData.Filters.ToolMaterial,
                    FilterSearchData.Filters.ToolSize,
                    FilterSearchData.Filters.ToolSubGroup,
                    FilterSearchData.Filters.UsageMaterial,
                    FilterSearchData.Filters.ToolGroup
                };
        private static List<FilterSearchData.Filters> HolderFilters =
            new List<FilterSearchData.Filters>()
                {
                    FilterSearchData.Filters.Cooling,
                    FilterSearchData.Filters.AvaliabilityInStock,
                    FilterSearchData.Filters.Search,
                    FilterSearchData.Filters.ToolLength
                 };

        private Dictionary<Type, List<FilterSearchData.Filters>> ToolsFilters =
            new Dictionary<Type, List<FilterSearchData.Filters>>()
                {
                    {
                       typeof(ToolAssembly),
                        ToolAssemblyFilters
                    },
                    {
                       typeof(CutterAssembly),
                        CutterFilters
                    },
                    {
                       typeof(Holder),
                        HolderFilters
                    }
                };


        public GraphQLRequest GetToolAssemblies(Dictionary<FilterSearchData.Filters, object> filters = null, int take = 25, int skip = 0, string sortingField = "name", string sortingOrder = "ASCENDING")
        {
            sortingField = sortingField ?? "name";
            sortingOrder = sortingOrder ?? "ASCENDING";
            Dictionary<string, string> sorting = new Dictionary<string, string>
                    {
                        { "column", sortingField },
                        { "order", sortingOrder }
                    };


            Dictionary<string, string> paging = new Dictionary<string, string>
                                                    {
                                                        { "take", take.ToString() },
                                                        { "skip", skip.ToString() }
                                                    };
            
            var filter = this.BuildFilters<ToolAssembly>(filters);

            var request = new GraphQLRequest
            {
                Query = @"
               query ($_v0_paging: PagingInputType,
	           $_v1_filter: ToolAssembliesFilterInputType,
               $_v2_sorting: SortingInputType)
               {
                toolAssemblies(paging: $_v0_paging, filter: $_v1_filter, sorting: $_v2_sorting ) {
                name
            id
        length
        quantity
        description
        legacyName1
        legacyName2
        legacyName3
        legacyName4
        usageMaterials
            cutterAssembly{
          cuttingMaterial {
                  id
                }
             cutter{
                cooling
                diameter
                geometryStr
                   }
                         }
		          }
                  }",
                OperationName = null,
                Variables = new
                {
                    _v0_paging = paging,
                    _v1_filter = filter,
                    _v2_sorting = sorting
                }
            };


            return request;
        }

        public GraphQLRequest TotalToolAssembly(Dictionary<FilterSearchData.Filters, object> filters = null)
        {
            var filter = this.BuildFilters<ToolAssembly>(filters);

            var request = new GraphQLRequest
            {
                Query = @" query($_v0_filter: ToolAssembliesFilterInputType) {
               toolAssembliesTotal(filter: $_v0_filter)
              }",
                OperationName = null,
                Variables = new
                {
                    _v0_filter = filter
                }
            };


            return request;
        }

        public GraphQLRequest GetCutterAssemblies(Dictionary<FilterSearchData.Filters, object> filters = null, int take = 25, int skip = 0, string sortingField = "name", string sortingOrder = "ASCENDING")
        {
            sortingField = sortingField ?? "name";
            sortingOrder = sortingOrder ?? "ASCENDING";
            Dictionary<string, string> sorting = new Dictionary<string, string>
                    {
                        { "column", sortingField },
                        { "order", sortingOrder }
                    };


            Dictionary<string, string> paging = new Dictionary<string, string>
                                                    {
                                                        { "take", take.ToString() },
                                                        { "skip", skip.ToString() }
                                                    };

            var filter = this.BuildFilters<CutterAssembly>(filters);

            var request = new GraphQLRequest
            {
                Query = @"
               query ($_v0_paging: PagingInputType,
	           $_v1_filter: CutterAssembliesFilterInputType,
               $_v2_sorting: SortingInputType)
               {
                cutterAssemblies(paging: $_v0_paging, filter: $_v1_filter, sorting: $_v2_sorting ) {
    name
    id
    length
    quantity
    usageMaterials
    cuttingMaterial {
                  id
                }
    cutter {
      cooling
      diameter
      geometryStr
      __typename
    }
    exchangablePlate {
      id
    }
    }
                  }",
                OperationName = null,
                Variables = new
                {
                    _v0_paging = paging,
                    _v1_filter = filter,
                    _v2_sorting = sorting
                }
            };


            return request;
        }

        public GraphQLRequest GetHolders(Dictionary<FilterSearchData.Filters, object> filters = null, int take = 25, int skip = 0, string sortingField = "name", string sortingOrder = "ASCENDING")
        {
            sortingField = sortingField ?? "name";
            sortingOrder = sortingOrder ?? "ASCENDING";
            Dictionary<string, string> sorting = new Dictionary<string, string>
                                                     {
                                                         { "column", sortingField },
                                                         { "order", sortingOrder }
                                                     };


            Dictionary<string, string> paging = new Dictionary<string, string>
                                                    {
                                                        { "take", take.ToString() },
                                                        { "skip", skip.ToString() }
                                                    };

            var filter = this.BuildFilters<Holder>(filters);

            var request = new GraphQLRequest
                              {
                                  Query = @"
               query ($_v0_paging: PagingInputType,
	           $_v1_filter: HoldersFilterInputType,
               $_v2_sorting: SortingInputType)
               {
                holders(paging: $_v0_paging, filter: $_v1_filter, sorting: $_v2_sorting ) {
    name
    id
    length
    quantity
    cooling

}
}",
                                  OperationName = null,
                                  Variables = new
                                                  {
                                                      _v0_paging = paging,
                                                      _v1_filter = filter,
                                                      _v2_sorting = sorting
                                                  }
                              };


            return request;
        }

        private Dictionary<string, object> BuildFilters<T>(Dictionary<FilterSearchData.Filters, object> filters)
        {
            Dictionary<FilterSearchData.Filters, string> filtersNames =
                new Dictionary<FilterSearchData.Filters, string>
                    {
                        {
                            FilterSearchData.Filters.Search,
                            "expression"
                        },
                        {
                            FilterSearchData.Filters.ToolLength,
                            "toolLength"
                        },
                        {
                            FilterSearchData.Filters.UsageMaterial,
                            "usageMaterial"
                        },
                        {
                            FilterSearchData.Filters.ToolMaterial,
                            "toolMaterial"
                        },
                        {
                            FilterSearchData.Filters.ToolSize,
                            "toolSize"
                        },
                        {
                            FilterSearchData.Filters.Cooling,
                            "hasCooling"
                        },
                        {
                            FilterSearchData.Filters.AvaliabilityInStock,
                            "isAvailable"
                        },
                        {
                            FilterSearchData.Filters.ToolGroup,
                            "group"
                        },
                        {
                            FilterSearchData.Filters.ToolSubGroup,
                            "subGroup"
                        }
                    };

            Dictionary<string, object> filtersSet = new Dictionary<string, object>();
            foreach (var filter in filters)
            {
               filtersSet.Add(filtersNames[filter.Key], filter.Value);
            }

            // Add needed filters for tool type
             foreach (var filter in this.ToolsFilters[typeof(T)].Except(filters.Keys))
                {
                    filtersSet.Add(filtersNames[filter], null);
                }

            return filtersSet;
        }
    }
}
