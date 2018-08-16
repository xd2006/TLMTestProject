using System;
using System.Collections.Generic;
using System.Threading;

using Core.WeDriverService;
using Core.WeDriverService.Extensions;
using OpenQA.Selenium;
using Tests.TestsData.Inventory.Enums;

namespace Tests.UI.Pages.Inventory
{
    public class CreateNewToolPopup : PageTemplate
    {
        public CreateNewToolPopup(IWebDriver driver) : base(driver)
        {
            Driver.WaitForPageReady();
        }

        private readonly By _titleLocator = By.CssSelector("*[class*='header_title']");
        private readonly By createBtnLocator = By.XPath("//button[./span[.='Create']]");
        private readonly By _cancelBtnLocator = By.XPath("//button[.='Cancel']");
        private readonly By _xIconLocator = By.CssSelector("button[class*='modal-header_close']");
        private readonly By _assemblyIdTextFieldLocator = By.Name("identification");

        private Dictionary<CreateToolInstancePopup.ToolInstanceFileds, By> FieldsLocators =>
            new Dictionary<CreateToolInstancePopup.ToolInstanceFileds, By>
            {
                {
                    CreateToolInstancePopup.ToolInstanceFileds.AssemblyId,
                    this._assemblyIdTextFieldLocator
                }
            };

        public string GetFiledPlaceFolder(CreateToolInstancePopup.ToolInstanceFileds field)
        {
            return Driver.Find(this.FieldsLocators[field]).GetAttribute("placeholder");
        }

        public bool Opened()
        {
            return Driver.Displayed(_titleLocator);
        }

        public string GetTitleName() => Driver.Find(_titleLocator).Text;

        public void ClickCancel()
        {
            Close(_cancelBtnLocator);
        }

        public void ClickXicon()
        {
            Close(_xIconLocator);
        }

        public void ClickSaveButton()
        {
            Driver.Click(this.createBtnLocator);
        }

        public bool IsSaveButtonEnabled()
        {
            return Driver.Find(this.createBtnLocator).Enabled;
        }

        public void PopulateField(CreateToolInstancePopup.ToolInstanceFileds field, string value)
        {
            var el = this.Driver.Find(this.FieldsLocators[field]);
            el.SendKeys(Keys.Control + "a");
            el.SendKeys(Keys.Delete);
            if (value != null)
            {
                 el.SendKeys(value);
            }
        }
        
        public void WaitForPageClosed(int timeout = 30)
        {
            var pageClosed = this.Driver.WaitForElementNotPresent(_titleLocator, timeout);

            if (!pageClosed)
            {
                throw new Exception("Can't close Create order popup");
            }
        }

        private void Close(By locator)
        {
            var closebuttonElement = Driver.Find(locator);
            Thread.Sleep(500); //To avoid 'Obscured' exception in Edge
            closebuttonElement.Click();
            WaitForPageClosed();
        }
    }
}
