
namespace Tests.UI.Pages.Link
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Core.WeDriverService;
    using Core.WeDriverService.Extensions;

    using global::Tests.UI.Pages.General;

    using OpenQA.Selenium;

    public class LinkPage : AnyPage
    {
        private By linkItemInfoElementLocator = By.CssSelector("div[class$='link-item_info']");

        private By linkItemInputFieldLocator = By.CssSelector("input[name = 'code']");

        private By newLinkButtonLocator = By.XPath("//button[./span[contains(.,'New')]]");

        private By validResultsMessageLocator = By.CssSelector("fake selector"); //Todo: update after reproducing success scenario

        private By invalidResultsMessageLocator = By.CssSelector("div[class$='linkAlert']>div[class$='failure']");

        private By CloseButtonLocator = By.CssSelector("button[class$='close']");

        public LinkPage(IWebDriver driver)
            : base(driver)
        {
        }

        public void WaitForPageLoad(int timeout)
        {
            this.Driver.WaitForElement(this.CloseButtonLocator, timeout);
        }

        public void WaitForPageLoad()
        {
            this.WaitForPageLoad(60);
        }


        public bool Opened()
        {
            return Driver.Displayed(this.linkItemInfoElementLocator);
        }

        public List<string> GetItemsInfo()
        {
           return Driver.Finds(this.linkItemInfoElementLocator).Select(e => e.FindElement(By.CssSelector("span")).Text)
                .ToList();
        }

        public void PopulateItem(int itemNumber, string itemGuid)
        {
            Driver.Finds(this.linkItemInputFieldLocator).ToList()[itemNumber - 1].SendKeys(itemGuid + Keys.Enter);
        }

        public void ClickNewLinkButton()
        {
            Driver.Find(this.newLinkButtonLocator).Click();
        }

        public List<string> GetItemsInputFieldsData()
        {
            return Driver.Finds(this.linkItemInputFieldLocator).Select(e => e.GetAttribute("value")).ToList();
        }

        public string GetResultMessage(bool valid)
        {
            if (valid)
            {
               return Driver.Find(validResultsMessageLocator).Text;
            }
            else
            {
                return Driver.Find(invalidResultsMessageLocator).Text;
            }
        }

        public void Close()
        {
            Exception exc;
            int counter = 0;
            do
            {
                try
                {
                    exc = null;
                    Driver.Find(this.CloseButtonLocator).Click();
                    Driver.WaitForElementNotPresent(this.CloseButtonLocator, 5);
                }
                catch (Exception e)
                {
                    exc = e;
                }
            }
            while (exc != null && counter++ < 5);

            if (exc != null)
            {
                throw exc;
            }
        }
    }
}
