
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
            Type,
            AvaliabilityInStock
        }

        public enum ToolsTypes
        {
            Tools,
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
