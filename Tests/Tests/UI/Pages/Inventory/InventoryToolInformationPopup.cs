
namespace Tests.UI.Pages.Inventory
{
    using System.Collections.Generic;
    using System.Linq;

    using Core.WeDriverService;
    using Core.WeDriverService.Extensions;

    using global::Tests.Models.ToolManager.GraphQlModels.ToolAssembly;
    using global::Tests.Models.ToolManager.UiModels;
    using global::Tests.UI.Components.Tools;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;

    public class InventoryToolInformationPopup : PageTemplate
    {
        public InventoryToolInformationPopup(IWebDriver driver)
            : base(driver)
        {
        }

        #region locators

        private readonly By grayedAreaLocator = By.CssSelector("div[class$='Overlay--after-open']"); 

        private readonly By detailsDataTextLineLocator = By.CssSelector("div[class$='generalDescription'] > p, div[class$='generalDescription'] > pre ");

        private readonly By relatedComponentTextCellLocator =
            By.CssSelector("div[class$='details_root'] tbody td[class$='nameCell']");

        private readonly By relatedComponentQuantityCellLocator = 
            By.CssSelector("div[class$='details_root'] tbody td[class$='textAlignCenter']");

        private readonly By relatedComponentTableHeaderLocator =
            By.CssSelector("div[class$='details_root'] table thead th");
        
        private InstancesInStockGrid InstancesGrid => new InstancesInStockGrid(Driver);

        #endregion

        public void Close()
        {
            var grayedArea = this.Driver.Find(this.grayedAreaLocator);
            Actions action = new Actions(this.Driver);
            action.MoveToElement(grayedArea, 10, 10).Click().Perform();
        }

        public bool Opened()
        {
            return this.Driver.Displayed(this.detailsDataTextLineLocator);
        }

        public Dictionary<string, string> GetToolInformaion()
        {
            Dictionary<string, string> info = new Dictionary<string, string>();
            var dataElements = this.Driver.Finds(this.detailsDataTextLineLocator);
            foreach (var element in dataElements)
            {
                var elementInfo = this.GetToolInfoText(element);
                info.Add(elementInfo.Key, elementInfo.Value);
            }
            return info;
        }

        public CutterAssembly GetCuttterAssemblyInfo()
        {
            var info = this.GetToolInformaion();
            CutterAssembly tool = new CutterAssembly();

            tool.Name = info["Name"];
            tool.Cutter = new List<Cutter> { new Cutter() };
            if (info["Size"] != string.Empty)
            {
                tool.Cutter.First().Diameter = int.Parse(info["Size"]);
            }

            tool.Length = int.Parse(info["Length"]);
            tool.Quantity = int.Parse(info["Quantity"]);
            tool.UsageMaterials = new List<string>();
            var usageMaterials = info["Usage material"].Split(',').ToList();
            tool.UsageMaterials = new List<string>();
            usageMaterials.ForEach(e => tool.UsageMaterials.Add(e.Trim()));

            return tool;
        }

        public Holder GetHolderInfo()
        {
            var info = this.GetToolInformaion();
            Holder tool = new Holder();

            tool.Name = info["Name"];
            tool.Length = int.Parse(info["Length"]);
            tool.Quantity = int.Parse(info["Quantity"]);
           
            return tool;
        }



        public ToolAssembly GetToolAssemblyInfo()
        {
            var info = this.GetToolInformaion();
            ToolAssembly tool = new ToolAssembly();
            

            tool.Description = info["Short description"];
            tool.Name = info["Name"];
            tool.CutterAssembly = new CutterAssembly();
            tool.CutterAssembly.Cutter = new List<Cutter> { new Cutter() };
            if (info["Size"] != string.Empty)
            {
                tool.CutterAssembly.Cutter.First().Diameter = int.Parse(info["Size"]);
            }

            tool.Length = int.Parse(info["Length"]);
            tool.Quantity = int.Parse(info["Quantity"]);

            tool.UsageMaterials = new List<string> { string.Empty };
            var usageMaterials = info["Usage material"].Split(',').ToList();
            tool.UsageMaterials = new List<string>();
            usageMaterials.ForEach(e => tool.UsageMaterials.Add(e.Trim()));

            // ToDo: 'Maximum lifetime' usage and 'Operating depth' to be added after defining requirements
            var relatedComponents = this.GetRelatedComponents();

            int i = 0;
            foreach (var component in relatedComponents)
            {
                switch (i)
                {
                    case 0:
                        {
                            tool.CutterAssembly.Cutter.First().Name = component.Key;
                            break;
                        }

                    case 1:
                        {
                            if (relatedComponents.Count > 2)
                            {
                                tool.CutterAssembly.ExchangablePlate = new ExchangablePlate { Name = component.Key };
                                tool.CutterAssembly.Cutter.First().EdgeNr = component.Value;
                            }
                            else
                            {
                                tool.Holders = new List<Holder> { new Holder { Name = component.Key } };
                            }

                            break;
                        }

                    default:
                        {
                            tool.Holders = new List<Holder> { new Holder { Name = component.Key } };
                            break;
                        }
                }

                i++;
            }

            return tool;
        }

        public Dictionary<string, int> GetRelatedComponents()
        {
            Dictionary<string, int> info = new Dictionary<string, int>();
            if (this.Driver.WaitForElementBool(this.relatedComponentTextCellLocator, 3))
            {
                var relatedComponents = this.Driver.Finds(this.relatedComponentTextCellLocator).Select(e => e.Text)
                    .ToList();
                var relatedComponentsQuantities = this.Driver.Finds(this.relatedComponentQuantityCellLocator)
                    .Select(e => e.Text).ToList();

                for (int i = 0; i < relatedComponents.Count; i++)
                {
                    info[relatedComponents[i]] = int.Parse(relatedComponentsQuantities[i]);
                }
            }

            return info;
        }

        public List<string> GetToolInfoFieldsNames()
        {
            var info = this.GetToolInformaion();

            return info.Keys.ToList();
        }

        public List<string> GetToolInfoRelatedComponentsFieldsNames()
        {
            var relatedComponentsTableHeaders = this.Driver
                .Finds(this.relatedComponentTableHeaderLocator).Select(e => e.Text)
                .ToList();
          
            return relatedComponentsTableHeaders;
        }

        public List<InstanceInStockGridRecord> GetInstacesInStock()
        {
            WaitForPageLoad();
            return InstancesGrid.GetRecords();
        }

        public void WaitForPageLoad()
        {
            Driver.WaitForElement(detailsDataTextLineLocator);           
        }

        public List<string> GetInstanceInStockTableColumnsNames()
        {
            return InstancesGrid.GetColumnsNames();
        }


        #region private methods
       
        private KeyValuePair<string, string> GetToolInfoText(IWebElement dataElement)
        {
            var lines = dataElement.Text.Split(':');

            KeyValuePair<string, string> info;
            if (lines.Length < 2)
            {
                info = new KeyValuePair<string, string>(lines.First().Trim(), string.Empty);
                return info;
            }

            info = new KeyValuePair<string, string>(lines.First().Trim(), lines.Last().TrimStart());           
            return info;
        }

        private string GetToolInfoText(string fieldName)
        {
            var info = this.GetToolInformaion();

            return info.ContainsKey(fieldName) ? info[fieldName] : null;
        }

        #endregion

    }
}
