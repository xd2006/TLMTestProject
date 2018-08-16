
namespace Tests.Models.ToolManager.GraphQlModels.ToolAssembly
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class CutterAssembly
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public int Length { get; set; }

        public int Quantity { get; set; }

        public List<string> UsageMaterials { get; set; }

        public Cutter Cutter { get; set; }

        public Material Material { get; set; }

        public ExchangablePlate ExchangablePlate { get; set; }

        [JsonProperty("__typename")]
        public string Typename { get; set; }
    }
}
