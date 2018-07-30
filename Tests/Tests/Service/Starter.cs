namespace Tests.Service
{
    using global::Tests.Managers;
    using global::Tests.Parameters;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Remote;

    public class Starter
    {
        /// <summary>
        /// The start application manager.
        /// </summary>
        /// <returns>
        /// The <see cref="ApplicationManager"/>.
        /// </returns>
        public ApplicationManager StartApplicationManager()
        {
            var browser = Parameters.Browser;
            var baseUrl = Parameters.ApplicationUrl;
            var hubUrl = Parameters.HubUrl;

            var capabilities = DefineCapabilities(browser);

            var app = new ApplicationManager(capabilities, baseUrl, hubUrl);
            return app;
        }

        /// <summary>
        /// The define capabilities.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <returns>
        /// The <see cref="ICapabilities"/>.
        /// </returns>
        private static ICapabilities DefineCapabilities(string browser)
        {
            DesiredCapabilities capabilities = new DesiredCapabilities();
            capabilities.SetCapability(CapabilityType.BrowserName, browser);
            if (browser.ToLower().Equals("chrome"))
            {
                capabilities.SetCapability("timeZone", "Europe/London");
            }

            return capabilities;
        }
    }
}
    

