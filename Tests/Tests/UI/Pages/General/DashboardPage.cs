
namespace Tests.UI.Pages.General
{
    using Core.WeDriverService.Extensions;

    using OpenQA.Selenium;

    public class DashboardPage : AnyPage
    {

        private readonly By titleLocator = By.XPath("//h1[.='Dashboard']");

        public DashboardPage(IWebDriver driver)
            : base(driver)
        {
        }

        public bool Opened()
        {
            return this.Driver.Displayed(this.titleLocator);
        }

        public void WaitForPageLoad()
        {
            this.Driver.WaitForElement(this.titleLocator);
        }

    }

}
