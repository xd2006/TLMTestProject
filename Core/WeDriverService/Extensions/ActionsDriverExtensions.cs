
namespace Core.WeDriverService
{
    using System;
    using System.Collections.Generic;

    using Core.Service;
    using Core.WeDriverService.Extensions;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.Support.UI;

    /// <summary>
    /// The webdriver extensions.
    /// </summary>
    public static partial class DriverExtensions
    {

        public static IWebElement Find(this IWebDriver driver, By locator, int timeoutSeconds = 30)
        {
            driver.WaitForPageReady();
            return driver.WaitForElementAndReturn(locator, timeoutSeconds);
        }

        public static IReadOnlyCollection<IWebElement> Finds(this IWebDriver driver, By locator, int waitTimeSec = 30)
        {
            driver.WaitForPageReady();
            return driver.WaitForElements(locator, waitTimeSec);
//            return driver.FindElements(locator);
        }
        
        public static void Click(this IWebDriver driver, By locator)
        {          
            driver.Find(locator).Click();
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
            driver.WaitForPageReady();
            Actions action = new Actions(driver);
            action.MoveToElement(element).Click().Perform();
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
