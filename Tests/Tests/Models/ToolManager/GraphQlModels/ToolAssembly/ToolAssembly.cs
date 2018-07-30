
namespace Tests.Models.ToolManager.GraphQlModels.ToolAssembly
{
    using System.Collections.Generic;

    public class ToolAssembly
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public int Length { get; set; }

        public int Quantity { get; set; }

        public CutterAssembly CutterAssembly { get; set; }

        public List<Holder> Holders { get; set; }

        public List<string> UsageMaterials { get; set; }

        public string LegacyName1 { get; set; }

        public string LegacyName2 { get; set; }

        public string LegacyName3 { get; set; }

        public string LegacyName4 { get; set; }

        public string Description { get; set; }

        public ToolAssembly WithName(string name)
        {
            this.Name = name;
            return this;
        }
    }
}
