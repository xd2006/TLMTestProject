
namespace Tests.UI.Pages.Inventory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Core.WeDriverService;
    using Core.WeDriverService.Extensions;

    using global::Tests.Models.ToolManager.GraphQlModels.ToolAssembly;
    using global::Tests.Models.ToolManager.UiModels;
    using global::Tests.UI.Components.Tools;
    using global::Tests.UI.Pages.Inventory.Templates;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;

    public class InventoryToolInformationPopup : InformationPopupTemplate
    {

        public bool IsPictureVisible => Driver.Find(_pictureLocator).Displayed;

        public bool IsPurchaseBtnVisible => Driver.Find(_purchaseBtnLocator).Displayed;

        public bool IsActionBtnVisible => Driver.Find(_actionBtnLocator).Displayed;

        public InventoryToolInformationPopup(IWebDriver driver)
            : base(driver)
        {
        }

        #region locators

       private readonly By relatedComponentTextCellLocator =
            By.CssSelector("div[class$='relatedSection'] tbody td[class$='LeftAligment']");

        private readonly By relatedComponentQuantityCellLocator = 
            By.CssSelector("div[class$='relatedSection'] tbody td[class$='RightAligment']");

        private readonly By relatedComponentTableHeaderLocator =
            By.CssSelector("div[class$='relatedSection'] table thead th");

        private readonly By createNewButtonLocator = By.CssSelector("*[class*='addButton']");

        private readonly By pageTitleLocator = By.CssSelector("*[class*='PageTitle']");

        private readonly By _purchaseBtnLocator = By.XPath("//button[.='Purchase']");

        private readonly By _actionBtnLocator = By.CssSelector("*[class*='actions-dropdown_iconAction']");

        private readonly By _pictureLocator  = By.CssSelector("*[class*='details-section_image']");

        private ToolInstancesGrid ToolInstancesGrid => new ToolInstancesGrid(Driver);

        #endregion
       
        public CutterAssembly GetCuttterAssemblyInfo()
        {
            var info = this.GetToolInformaion();
            CutterAssembly tool = new CutterAssembly();

            tool.Name = info["Cutter type"];
            tool.Cutter = new Cutter();
            if (info["Size"] != string.Empty)
            {
                tool.Cutter.Diameter = int.Parse(info["Size"]);
            }

            tool.Length = int.Parse(info["Length"]);
            tool.Quantity = int.Parse(info["Quantity in stock"]);
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

            tool.Name = info["Holder type"];
            tool.Length = int.Parse(info["Length"]);
            tool.Quantity = int.Parse(info["Quantity in stock"]);
           
            return tool;
        }

        public ToolAssembly GetToolAssemblyInfo()
        {
            var info = this.GetToolInformaion();
            ToolAssembly tool = new ToolAssembly();
            

            tool.Description = info["Short description"];
            tool.Name = info["Tool type"];
            tool.CutterAssembly = new CutterAssembly();
            tool.CutterAssembly.Cutter = new Cutter();
            if (info["Size"] != string.Empty)
            {
                tool.CutterAssembly.Cutter.Diameter = int.Parse(info["Size"]);
            }

            tool.Length = int.Parse(info["Length"]);
            tool.Quantity = int.Parse(info["Quantity in stock"]);

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
                            tool.CutterAssembly.Cutter.Name = component.Name;
                            break;
                        }

                    case 1:
                        {
                            if (relatedComponents.Count > 2)
                            {
                                tool.CutterAssembly.ExchangablePlate = new ExchangablePlate { Name = component.Name };
                                tool.CutterAssembly.Cutter.EdgeNr = component.Quantity;
                            }
                            else
                            {
                                tool.Holders = new List<Holder> { new Holder { Name = component.Name } };
                            }

                            break;
                        }

                    default:
                        {
                            tool.Holders = new List<Holder> { new Holder { Name = component.Name } };
                            break;
                        }
                }

                i++;
            }

            return tool;
        }

        public List<(string Name, int Quantity)> GetRelatedComponents()
        {
            List<(string, int)> info = new List<(string, int)>();
            if (this.Driver.WaitForElementBool(this.relatedComponentTextCellLocator, 3))
            {
                var relatedComponents = this.Driver.Finds(this.relatedComponentTextCellLocator).Select(e => e.Text)
                    .ToList();
                var relatedComponentsQuantities = this.Driver.Finds(this.relatedComponentQuantityCellLocator)
                    .Select(e => e.Text).ToList();

                for (int i = 0; i < relatedComponents.Count; i++)
                {
                   var data = (Name:
                       relatedComponents[i],
                        Quantity: int.Parse(relatedComponentsQuantities[i]));

                    info.Add(data);
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

        public List<ToolInstanceGridRecord> GetToolInstances()
        {
            WaitForPageLoad();
            return this.ToolInstancesGrid.GetRecords();
        }
        
        public void WaitForPageLoad(int timeoutSec)
        {
            Driver.WaitForElement(this.DetailsDataInfoLineLocator, timeoutSec);           
        }

        public void WaitForPageLoad()
        {
            this.WaitForPageLoad(30);
        }

        public List<string> GetInstanceInStockTableColumnsNames()
        {
            return this.ToolInstancesGrid.GetColumnsNames();
        }

        public void ClickCreateNewIntanceBtn()
        {
            Driver.Find(this.createNewButtonLocator).Click();
        }

        public string GetDetailsTabTitle()
        {
            return Driver.Find(this.pageTitleLocator).Text;
        }

        public void Disassemble(int instanceId)
        {
            this.ToolInstancesGrid.Disassemble(instanceId);
        }
    }
}
