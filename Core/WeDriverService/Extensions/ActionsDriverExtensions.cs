
namespace Core.WeDriverService
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using Core.Service;
    using Core.WeDriverService.Extensions;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.Support.Extensions;
    using OpenQA.Selenium.Support.UI;

    /// <summary>
    /// The webdriver extensions.
    /// </summary>
    public static partial class DriverExtensions
    {

        public static IWebElement Find(this IWebDriver driver, By locator, int timeoutSeconds = 50)
        {
            driver.WaitForPageReady();
            return driver.WaitForElement(locator, timeoutSeconds);
        }

        public static IReadOnlyCollection<IWebElement> Finds(this IWebDriver driver, By locator, int waitTimeSec = 50)
        {
            driver.WaitForPageReady();
            return driver.WaitForElements(locator, waitTimeSec);
//            return driver.FindElements(locator);
        }
        
        public static void Click(this IWebDriver driver, By locator, int timeoutSec = 10)
        {
            int counter = 0;
            WebDriverException exc;
            do
            {
                try
                {
                    exc = null;
                    driver.WaitForElementToBeClickable(locator);
                    driver.Find(locator).Click();
                }
                catch (WebDriverException e)
                {
                    exc = e;
                    Thread.Sleep(500);
                }
            }
            while (exc != null && counter++ < timeoutSec * 2);

            if (counter >= 5 && exc != null)
            {
                throw exc;
            }
        }

        public static void Click(this IWebDriver driver, Func<IWebElement> findAction, int timeoutSec = 10)
        {
            int c = 0;
            StaleElementReferenceException exc = null;
            do
            {
                try
                {
                    exc = null;
                    var el = findAction.Invoke();
                    el.Click();
                }
                catch (StaleElementReferenceException e)
                {
                    Thread.Sleep(500);
                    exc = e;
                }
            }
            while (exc != null && c++ < timeoutSec * 2);

            if (exc != null)
            {
                throw exc;
            }
        }

        /// <summary>
        /// The double click.
        /// </summary>
        /// <param name="driver">
        /// The driver.
        /// </param>
        /// <param name="locator">
        /// The locator.
        /// </param>
        public static void DoubleClick(this IWebDriver driver, By locator)
        {
            driver.WaitForPageReady();
            Actions action = new Actions(driver);
            action.DoubleClick(driver.FindElement(locator)).Perform();
        }

        /// <summary>
        /// The double click.
        /// </summary>
        /// <param name="driver">
        /// The driver.
        /// </param>
        /// <param name="element">
        /// The element.
        /// </param>
        public static void DoubleClick(this IWebDriver driver, IWebElement element)
        {
            driver.WaitForPageReady();
            Actions action = new Actions(driver);
            action.DoubleClick(element).Perform();
        }

        /// <summary>
        /// The hover element.
        /// </summary>
        /// <param name="driver">
        /// The driver.
        /// </param>
        /// <param name="locator">
        /// The locator.
        /// </param>
        public static void HoverElement(this IWebDriver driver, By locator)
        {
            driver.WaitForPageReady();
            Actions action = new Actions(driver);
            action.MoveToElement(driver.FindElement(locator)).Perform();
        }

        /// <summary>
        /// The hover element.
        /// </summary>
        /// <param name="driver">
        /// The driver.
        /// </param>
        /// <param name="element">
        /// The element.
        /// </param>
        public static void HoverElement(this IWebDriver driver, IWebElement element)
        {
            driver.WaitForPageReady();
            Actions action = new Actions(driver);
            action.MoveToElement(element).Perform();
        }

        /// <summary>
        /// The hover element and click.
        /// </summary>
        /// <param name="driver">
        /// The driver.
        /// </param>
        /// <param name="element">
        /// The element.
        /// </param>
        public static void HoverElementAndClick(this IWebDriver driver, IWebElement element)
        {
           Actions action = new Actions(driver);
            action.MoveToElement(element).Perform();
            Thread.Sleep(1000);
            action.Click(element).Perform();
        }

        public static void HoverElementAndClick(this IWebDriver driver, By locator)
        {
            driver.WaitForPageReady();
            var el = driver.FindElement(locator);
            driver.HoverElementAndClick(el);           
        }

        public static void ClickUsingJS(this IWebDriver driver, By locator)
        {
            IJavaScriptExecutor javaScriptExecutor = driver as IJavaScriptExecutor;
            javaScriptExecutor.ExecuteScript("arguments[0].click();", (object)driver.FindElement(locator));
        }
        

        public static void Populate(this IWebDriver driver, By locator, string text)
        {
            var element = driver.Find(locator);
            if (!string.IsNullOrEmpty(element.GetAttribute("value")))
            {
                element.Clear();
            }
            element.SendKeys(text);
        }
     }
}
