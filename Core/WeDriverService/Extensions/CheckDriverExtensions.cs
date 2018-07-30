
namespace Core.WeDriverService.Extensions
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;

    public static partial class DriverExtensions
    {

        public static bool Exists(this IWebDriver driver, By locator)
        {

            Func<IWebDriver, bool> action = webDriver => { return driver.FindElements(locator).Count > 0; };

            return ExecuteWithoutImplicitTimeout(driver, action);
        }

        public static bool Displayed(this IWebDriver driver, By locator)
        {           
            Func<IWebDriver, bool> action = webDriver =>
                {
                   return webDriver.FindElements(locator).Count > 0 
                          && webDriver.FindElements(locator).Any(e => e.Displayed);
                };
            
            return ExecuteWithoutImplicitTimeout(driver, action);
        }       
    }
}
