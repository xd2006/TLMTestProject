
namespace Tests.TestsData.Inventory.Enums.FilterSearch
{
    public class FilterSearchData
    {
        public enum Filters
        {
            Search,
            UsageMaterial,
            ToolMaterial,
            ToolSize,
            ToolLength,
            Cooling,
            ToolGroup,
            ToolSubGroup,
            AvaliabilityInStock
        }

        public enum ToolsTypes
        {
            Assemblies,
            Cutters,
            Holders
        }

        public enum GridColumnsNames
        {
            NAME,
            SIZE,
            LENGTH,
            QUANTITY
        }
    }
}
