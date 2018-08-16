
namespace Tests.UI.Pages.PagesTemplates
{
    using System.Collections.Generic;
    using System.Linq;

    using Core.WeDriverService;
    using Core.WeDriverService.Extensions;

    using global::Tests.UI.Pages.General;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public abstract class PageWithGridTemplate : AnyPage
    {

        #region locators

        private readonly By nextButtonLocator = By.XPath("//div[contains(@class, 'pagination_buttons')]/button[contains(.,'>')]");

        private readonly By previousButtonLocator = By.XPath("//div[contains(@class, 'pagination_buttons')]/button[contains(.,'<')]");

        private readonly By availableNavigationPagesButtonsLocator = By.XPath(
            "//div[contains(@class, 'pagination_buttons')]/button[not(contains(.,'>')) and not(contains(.,'<')) and .!='...']");

        private readonly By gridSizeSelectorLocator = By.CssSelector("div[class$='pagination_pageSize'] > select");

        private readonly string navigationPageLocatorMask = "//div[contains(@class, 'pagination_buttons')]/button[.='{0}']";

        private readonly By activePageButtonLocator = By.CssSelector("div[class$='pagination_buttons'] > button[class$='current']");

        private readonly By tableHeaderLocator = By.XPath("//table/thead/tr");

        #endregion

        protected PageWithGridTemplate(IWebDriver driver)
            : base(driver)
        {
        }

        #region selects
        private SelectElement GridSizeSelector => new SelectElement(this.Driver.Find(this.gridSizeSelectorLocator));

        #endregion

        #region waits
        public void WaitForPageLoad(int timeout)
        {
            if (Parameters.Parameters.Browser == "MicrosoftEdge")
            {
                this.Driver.WaitForElement(this.tableHeaderLocator, timeout);
            }
            else
            {
                this.Driver.WaitForElementVisible(this.tableHeaderLocator, timeout);
            }
        }

        public void WaitForPageLoad()
        {
            this.WaitForPageLoad(60);
        }

        #endregion

        public virtual int GetDefinedNumberOfResultsOption()
        {
            var text = GridSizeSelector.SelectedOption.Text;

            return int.Parse(text);
        }

        public virtual void SetNumberOfResultsOption(int number)
        {
            this.GridSizeSelector.SelectByText(number.ToString());
        }

        public virtual void ClickNextButton()
        {
            this.Driver.FindElement(this.nextButtonLocator).Click();
        }

        public virtual bool IsNextButtonActive()
        {
            if (this.Driver.Displayed(this.nextButtonLocator))
            {
                return this.Driver.FindElement(this.nextButtonLocator).Enabled;
            }

            return false;
        }

        public virtual void ClickPreviousButton()
        {
            this.Driver.FindElement(this.previousButtonLocator).Click();
        }

        public virtual void ClickPage(string pageNumber)
        {
            this.Driver.Click(By.XPath(string.Format(this.navigationPageLocatorMask, pageNumber)));
        }

        public virtual List<string> GetAvailablePages()
        {
            return this.Driver.Finds(this.availableNavigationPagesButtonsLocator).Select(b => b.Text).ToList();
        }

        public int GetActivePage()
        {
            return int.Parse(this.Driver.Find(this.activePageButtonLocator).Text);
        }
    }
}
