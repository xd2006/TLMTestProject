namespace Core.Service
{
    using System;

    using OpenQA.Selenium;

    /// <summary>
    /// The custom expected conditions.
    /// </summary>
    public class CustomExpectedConditions
    {
        /// <summary>
        /// The page ready condition.
        /// </summary>
        public static Func<IWebDriver, bool> PageReadyCondition = webDriver =>
            {
                var javaScriptExecutor = webDriver as IJavaScriptExecutor;
                bool c1;
                try
                {
                    c1 = javaScriptExecutor.ExecuteScript(
                        "return document.readyState").ToString().Equals("complete");
                }
                catch
                {
                    c1 = false;
                }
                bool c2;
                try
                {
                    c2 = (long)javaScriptExecutor.ExecuteScript(
                             "return jQuery.active") == 0;
                }
                catch
                {
                    c2 = true;
                }

                return c2 && c1;
            };
    }
}
