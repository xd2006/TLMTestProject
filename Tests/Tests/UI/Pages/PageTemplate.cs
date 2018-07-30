
namespace Tests.UI.Pages
{
    using OpenQA.Selenium;

    public abstract class PageTemplate
    {
        public PageTemplate(IWebDriver driver)
        {
            this.Driver = driver;
        }

        protected IWebDriver Driver { get; }
        
    }
}
