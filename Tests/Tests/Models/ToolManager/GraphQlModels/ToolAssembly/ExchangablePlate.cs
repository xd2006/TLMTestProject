
namespace Tests.Models.ToolManager.GraphQlModels.ToolAssembly
{
    using Newtonsoft.Json;

    public class ExchangablePlate
    {
        public string Id { get; set; }

        public string Name { get; set; }

        [JsonProperty("__typename")]
        public string Typename { get; set; }
    }
}
