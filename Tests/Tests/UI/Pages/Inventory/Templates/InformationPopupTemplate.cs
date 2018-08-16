
namespace Tests.UI.Pages.Inventory.Templates
{
    using System.Collections.Generic;

    using Core.WeDriverService;
    using Core.WeDriverService.Extensions;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;

    public abstract class InformationPopupTemplate : PageTemplate
    {
        protected InformationPopupTemplate(IWebDriver driver)
            : base(driver)
        {
        }

        private readonly By detailsDataInfoLabelLocator = By.CssSelector("[class$='info-field_label']");

        private readonly By detailsDataInfoValueLocator = By.CssSelector("[class$='info-field_value']");

        protected readonly By DetailsDataInfoLineLocator = By.CssSelector("div[class$='details-section_field']");

        protected readonly By GrayedAreaLocator = By.CssSelector("div[class$='Overlay--after-open']");

        public void WaitForPageLoad(int timeoutSec)
        {
            Driver.WaitForElement(this.DetailsDataInfoLineLocator, timeoutSec);
        }

        public void WaitForPageLoad()
        {
            this.WaitForPageLoad(30);
        }

        public void Close()
        {
            var grayedArea = this.Driver.Find(this.GrayedAreaLocator);
            Actions action = new Actions(this.Driver);
            action.MoveToElement(grayedArea, 10, 10).Click().Perform();
        }

        public bool Opened()
        {
            return this.Driver.Displayed(this.DetailsDataInfoLineLocator);
        }

        public Dictionary<string, string> GetToolInformaion()
        {
            Dictionary<string, string> info = new Dictionary<string, string>();
            var dataElements = this.Driver.Finds(this.DetailsDataInfoLineLocator);
            foreach (var element in dataElements)
            {
                var elementInfo = this.GetToolInfoText(element);
                info.Add(elementInfo.Key, elementInfo.Value);
            }
            return info;
        }

        #region private methods

        private KeyValuePair<string, string> GetToolInfoText(IWebElement dataElement)
        {
            var label = dataElement.FindElement(this.detailsDataInfoLabelLocator).Text;
            var value = dataElement.FindElement(this.detailsDataInfoValueLocator).Text;

            KeyValuePair<string, string> info;

            info = new KeyValuePair<string, string>(label.Trim(), value.TrimStart());
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
