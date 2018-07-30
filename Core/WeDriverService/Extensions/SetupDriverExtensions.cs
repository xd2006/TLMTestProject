
namespace Core.WeDriverService.Extensions
{
    using System;

    using OpenQA.Selenium;

    public static partial class DriverExtensions
    {

        /// <summary>
        /// Disable implicit timeout.
        /// </summary>
        /// <param name="driver">
        /// The driver.
        /// </param>
        public static void DisableTimeout(this IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
        }

        /// <summary>
        /// Set implicit timeout.
        /// </summary>
        /// <param name="driver">
        /// The driver.
        /// </param>
        /// <param name="timeoutSec">
        /// The timeout sec.
        /// </param>
        public static void SetTimeout(this IWebDriver driver, int timeoutSec = 10)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeoutSec);
        }

    }
}
