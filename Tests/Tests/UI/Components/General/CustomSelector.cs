
namespace Tests.UI.Components.General
{
    using System;
    using System.Threading;

    using Core.WeDriverService;
    using Core.WeDriverService.Extensions;

    using global::Tests.UI.Components.Interfaces;

    using OpenQA.Selenium;

    public class CustomSelector : ComponentTemplate, ISelector
    {
        private By selectorButtonLocator;

        private By dropdownListLocator = By.CssSelector("div.hpl-OptionFlyout");

        private string selectorItemMask = "//ul/li/button[./span[.='{0}']]";

        public void SelectOption(string optionName)
        {
            var optionLocator = By.XPath(string.Format(this.selectorItemMask, optionName));

            var counter = 0;
            do
            {
                int counterIn = 0;

            do
            {
                try
                {
                    if (counter > 0)
                    {
                        Thread.Sleep(500);
                    }

                    Driver.Find(this.selectorButtonLocator).Click();
                }
                catch (Exception)
                {
                    Thread.Sleep(1000);
                }
            }
            while (!Driver.Displayed(this.dropdownListLocator) && counterIn++ < 10);

            // due to issue - group and type can't be selected from the 1st time
          
                Driver.Find(optionLocator).Click();
                if (counter > 0)
                {
                    Thread.Sleep(500);
                }
            }
            while (Driver.Find(this.selectorButtonLocator).Text != optionName && counter++ < 10);


        }

        public string SelectedOption()
        {
            StaleElementReferenceException exc;
            string text = string.Empty;
            int counter = 0;
            do
            {
                try
                {
                    exc = null;
                    text = Driver.Find(this.selectorButtonLocator).Text;
                }
                catch (StaleElementReferenceException e)
                {
                    exc = e;
                }
            }
            while (exc != null && counter++ < 10);
            if (exc != null)
            {
                throw exc;
            }

            return text;
        }

        public CustomSelector(IWebDriver driver, By selectorButtonLocator)
            : base(driver)
        {
            this.selectorButtonLocator = selectorButtonLocator;
        }
    }
}
