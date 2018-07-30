
namespace Tests.UI.Components
{
    using OpenQA.Selenium;

    public abstract class ComponentTemplate
    {
        protected ComponentTemplate(IWebDriver driver)
        {
            this.Driver = driver;
        }

        protected IWebDriver Driver { get; set; }
    }
}
