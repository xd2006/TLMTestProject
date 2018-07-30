
namespace Tests.Models.ToolManager.GraphQlModels.ToolAssembly
{
    using Newtonsoft.Json;

    public class Cutter
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string GeometryStr { get; set; }

        public int Diameter { get; set; }

        public int EdgeNr { get; set; }

        public bool Cooling { get; set; }

        [JsonProperty("__typename")]
        public string Typename { get; set; }
    }
}
