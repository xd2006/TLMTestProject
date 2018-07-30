namespace Tests.UI
{
    using OpenQA.Selenium;

    public abstract class UIElementTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIElementTemplate"/> class.
        /// </summary>
        /// <param name="driver">
        /// The driver.
        /// </param>
        protected UIElementTemplate(IWebDriver driver)
        {
            this.Driver = driver;
        }

        /// <summary>
        /// Gets the driver.
        /// </summary>
        protected IWebDriver Driver { get; }
    }
}
