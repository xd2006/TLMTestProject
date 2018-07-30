
namespace Tests.Managers.AuxPageManagers.Templates
{
    using OpenQA.Selenium;

    public abstract class PagesManagerTemplate
    {
       public PagesManagerTemplate(IWebDriver driver)
        {
            this.Driver = driver;
        }

        protected IWebDriver Driver { get; set; }
    }
}
