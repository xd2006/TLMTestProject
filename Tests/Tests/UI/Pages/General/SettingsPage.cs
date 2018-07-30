
namespace Tests.UI.Pages.General
{
    using Core.WeDriverService.Extensions;

    using OpenQA.Selenium;

    public class SettingsPage : AnyPage
    {
        private By headerLocator = By.XPath("//div[contains(@class, 'layout_body')]/h1[.='Settings']");
        
        public SettingsPage(IWebDriver driver)
            : base(driver)
        {
        }

        public void WaitForPageLoad(int timeout)
        {
            this.Driver.WaitForElement(this.headerLocator, timeout);
        }

        public void WaitForPageLoad()
        {
            this.WaitForPageLoad(60);
        }


        public bool Opened()
        {
            return Driver.Displayed(this.headerLocator);
        }
    }
}
