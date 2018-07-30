
namespace Tests.Models.ToolManager.GraphQlModels.ToolAssembly
{
    using Newtonsoft.Json;

    public class Holder
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Length { get; set; }

        public int Quantity { get; set; }

        public bool Cooling { get; set; }

        [JsonProperty("__typename")]
        public string Typename { get; set; }
    }
}
