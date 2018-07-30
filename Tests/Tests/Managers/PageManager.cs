namespace Tests.Managers
{
    using System;
    using System.Threading;

    using Core.WeDriverService;

    using global::Tests.Managers.AuxPageManagers;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Remote;
    using OpenQA.Selenium.Support.UI;

    public class PageManager
    {
        /// <summary>
        /// The driver.
        /// </summary>
        private IWebDriver driver;

        private InventoryPages inventoryPages;

        private OrdersPages ordersPages;

        private GeneralPages generalPages;

        private MachinesPages machinesPages;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageManager"/> class.
        /// </summary>
        /// <param name="capabilities">
        /// The capabilities.
        /// </param>
        /// <param name="baseUrl">
        /// The base url.
        /// </param>
        /// <param name="hubUrl">
        /// The hub url.
        /// </param>
        public PageManager(DesiredCapabilities capabilities, string baseUrl, string hubUrl)
        {
            if (string.IsNullOrEmpty(hubUrl) && capabilities.BrowserName.ToLower() != "microsoftedge")
            {               
                new WebDriverManager().SetupDriver(capabilities.BrowserName);
            }
            else
            {
//                capabilities.SetCapability("enableVideo", true);
                capabilities.SetCapability("enableVNC", true);
                //capabilities.Platform = new Platform(PlatformType.Windows);
            }

            this.Driver = WebDriverFactory.GetDriver(hubUrl, capabilities);
            this.Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            this.Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);

            Exception exc = null;
            int counter = 0;

            do
            {
                try
                {
                    this.Driver.Manage().Window.Maximize();
                    exc = null;
                }
                catch (Exception e)
                {
                    exc = e;
                    Thread.Sleep(1000);
                }
            }
            while (exc != null && counter++ < 10);

            if (counter >= 10)
            {
                throw exc;
            }

            IAllowsFileDetection allowsDetection = this.Driver as IAllowsFileDetection;
            if (allowsDetection != null)
            {
                allowsDetection.FileDetector = new LocalFileDetector();
            }
            
            if (!this.Driver.Url.StartsWith(baseUrl))
            {
                this.Driver.Navigate().GoToUrl(baseUrl);
            }
        }
        
        public WebDriverWait Wait(int timeoutSeconds = 20) => new WebDriverWait(this.Driver, TimeSpan.FromSeconds(timeoutSeconds));

        public IWebDriver Driver
        {
            get => this.driver;

            set => this.driver = value;
        }

        #region helper page managers declaration

        public InventoryPages ToolsPages => this.inventoryPages ?? (this.inventoryPages = new InventoryPages(this.Driver));

        public OrdersPages OrdersPages => this.ordersPages ?? (this.ordersPages = new OrdersPages(this.Driver));

        public GeneralPages GeneralPages => this.generalPages ?? (this.generalPages = new GeneralPages(this.Driver));

        public MachinesPages MachinesPages => this.machinesPages ?? (this.machinesPages = new MachinesPages(this.Driver));

        #endregion
    }
}
