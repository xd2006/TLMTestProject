
namespace Core.WeDriverService.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;

    using Core.Service;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.Extensions;
    using OpenQA.Selenium.Support.UI;
    using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

    public static partial class DriverExtensions
    {
        /// <summary>
        /// The wait for page ready.
        /// </summary>
        /// <param name="driver">
        /// The driver.
        /// </param>
        /// <param name="timeoutSeconds">
        /// The timeout Seconds.
        /// </param>
        public static void WaitForPageReady(this IWebDriver driver, int timeoutSeconds = 50)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));

            // Check if document is ready
            try
            {
                wait.Until(CustomExpectedConditions.PageReadyCondition);
            }
            catch (WebDriverTimeoutException e)
            {
                throw new Exception(e.Message + " .Page can't be completely loaded");
            }
        }

        public static void WaitForElementStale(this IWebDriver driver, IWebElement element, int timeoutSeconds = 30)
        {
            ExecuteWithoutImplicitTimeout(
                driver,
                () => new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds)).Until(
                    ExpectedConditions.StalenessOf(element)));

        }

        public static bool WaitForElementStaleBool(this IWebDriver driver, IWebElement element, int timeoutSeconds = 30)
        {
            try
            {
                ExecuteWithoutImplicitTimeout(
                    driver,
                    () => new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds)).Until(
                        ExpectedConditions.StalenessOf(element)));

                return true;
            }
            catch (WebDriverException)
            {
                return false;
            }           
        }
        
        public static IWebElement WaitForElement(this IWebDriver driver, By locator, int timeoutSeconds = 30)
        {
            return ExecuteWithoutImplicitTimeout(
                driver,
                webDriver => new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeoutSeconds)).Until(
                    ExpectedConditions.ElementExists(locator)));
        }

        public static IWebElement WaitForElementVisible(this IWebDriver driver, By locator, int timeoutSeconds = 30)
        {
            return ExecuteWithoutImplicitTimeout(
                driver,
                webDriver => new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeoutSeconds)).Until(
                    ExpectedConditions.ElementIsVisible(locator)));
        }

        public static IWebElement WaitForElementToBeClickable(this IWebDriver driver, By locator, int timeoutSeconds = 30)
        {
            return ExecuteWithoutImplicitTimeout(
                driver,
                webDriver => new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeoutSeconds)).Until(
                    ExpectedConditions.ElementToBeClickable(locator)));
        }

        public static bool WaitForElementBool(this IWebDriver driver, By locator, int timeoutSeconds = 30)
        {
            try
            {
                ExecuteWithoutImplicitTimeout(
                    driver,
                    () => new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds)).Until(
                        ExpectedConditions.ElementIsVisible(locator)));
                return true;
            }
            catch (WebDriverException)
            {
                return false;
            }
           
        }

        public static IReadOnlyCollection<IWebElement> WaitForElements(this IWebDriver driver, By locator, int timeoutSeconds = 30)
        {          
               return new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds)).Until(
                        ExpectedConditions.PresenceOfAllElementsLocatedBy(locator));
                
           }

        public static bool WaitForElementNotPresent(this IWebDriver driver, By locator, int timeoutSeconds = 30)
        {
          return ExecuteWithoutImplicitTimeout(
              driver,
              webDriver => new WebDriverWait(
                  webDriver,
                  TimeSpan.FromSeconds(timeoutSeconds)).Until(
                  ExpectedConditions.InvisibilityOfElementLocated(locator)));           
        }

        public static bool WaitForElementNotPresent(this IWebDriver driver, IWebElement element, int timeoutSeconds = 30)
        {
            int counter = 0;
            bool displayed = true;
            do
            {
                try
                {
                    displayed = element.Displayed;
                }
                catch (Exception)
                {
                    displayed = false;
                }
                finally
                {
                    if (displayed)
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
            while (displayed && counter++ < timeoutSeconds);

            return displayed;
        }

        public static IWebElement WaitForSpecificElement(this IWebDriver driver, Func<IWebDriver, IWebElement> method)
        {
            IWebElement element;
            int counter = 0;
            do
            {
                try
                {
                    element = method(driver);
                }
                catch (Exception)
                {
                    element = null;
                }

                if (element == null)
                {
                    Thread.Sleep(1000);
                }
            }
            while (element == null && counter++ < 10);

            if (counter >= 10)
            {
                throw new Exception("Can't find needed element");
            }

            return element;
        }

        public static bool WaitForAnimation(this IWebDriver driver, int timeoutInMilliseconds = 30000)
        {
            try
            {
                new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeoutInMilliseconds)).Until(x => (bool)((IJavaScriptExecutor)driver).ExecuteScript("return ($(':animated').size() < 1);"));
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void ExecuteWithoutImplicitTimeout(IWebDriver driver, Action action)
        {
            if (action == null)
            {
                return;
            }

            try
            {
                driver.DisableTimeout();
                action();
            }
            finally
            {
                driver.SetTimeout();
            }
        }

        private static IWebElement ExecuteWithoutImplicitTimeout(IWebDriver driver, Func<IWebDriver, IWebElement> action)
        {
            if (action == null)
            {
                return null;
            }

            StaleElementReferenceException exc = null;
            int counter = 0;

            do
            {
                try
                {
                    driver.DisableTimeout();
                    var el = action(driver);
                    return el;
                }
                catch (StaleElementReferenceException e)
                {
                    exc = e;
                    Thread.Sleep(500);
                }
                finally
                {
                    driver.SetTimeout();
                }
            }
            while (exc != null && counter++ < 10);

            if (counter >= 10 && exc!=null)
            {
                throw exc;
            }

            return null;
        }

        private static List<IWebElement> ExecuteWithoutImplicitTimeout(IWebDriver driver, Func<IWebDriver, List<IWebElement>> action)
        {
            if (action == null)
            {
                return null;
            }

            try
            {
                driver.DisableTimeout();
                var elements = action(driver);
                return elements;
            }
            finally
            {
                driver.SetTimeout();
            }
        }

        private static bool ExecuteWithoutImplicitTimeout(IWebDriver driver, Func<IWebDriver, bool> action)
        {
            if (action == null)
            {
                return false;
            }

            try
            {
                driver.DisableTimeout();
                var res = action(driver);
                return res;
            }
            catch (StaleElementReferenceException)
            {
                var res = action(driver);
                return res;
            }
            finally
            {
                driver.SetTimeout();
            }
        }
    }
}
