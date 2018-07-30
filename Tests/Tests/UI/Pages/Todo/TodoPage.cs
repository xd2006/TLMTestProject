
namespace Tests.UI.Pages.Todo
{
    using Core.WeDriverService.Extensions;

    using global::Tests.UI.Pages.PagesTemplates;

    using OpenQA.Selenium;

    public class TodoPage : PageWithGridTemplate
    {

        private By headerLocator = By.XPath("//div[contains(@class, 'layout_body')]/div[.='Upcoming Jobs to be done']");

        public TodoPage(IWebDriver driver)
            : base(driver)
        {
        }

        public bool Opened()
        {
            return Driver.Displayed(this.headerLocator);
        }
    }
}
