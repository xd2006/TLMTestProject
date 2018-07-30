
namespace Tests.Managers
{
    using global::Tests.Helpers.Preconditions;
    using global::Tests.Managers.AuxManagers;

    using log4net;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Remote;

    public class ApplicationManager
    {
        private ICapabilities capabilities;

        private string hubUrl;

        /// <summary>
        /// The _pages.
        /// </summary>
        private PageManager pages;

        private ApiManager api;

        private UiManager ui;

        private GraphApiManager graphApi;

        private DbManager db;

        private PreconditionsHelper preconditions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationManager"/> class.
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
        public ApplicationManager(ICapabilities capabilities, string baseUrl, string hubUrl)
        {
            this.capabilities = capabilities;
            this.BaseUrl = baseUrl;
            this.hubUrl = hubUrl;
            this.Logger = LogManager.GetLogger("Logger");
        }

        /// <summary>
        /// Gets or sets the Log4Net logger.
        /// </summary>
        public ILog Logger { get; set; }

        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets the pages manager
        /// </summary>
        public PageManager Pages =>
            this.pages ?? (this.pages = new PageManager(
                               (DesiredCapabilities)this.capabilities,
                               this.BaseUrl,
                               this.hubUrl));

        public ApiManager Api => this.api ?? (this.api = new ApiManager(this));

        public UiManager Ui => this.ui ?? (this.ui = new UiManager(this));

        public GraphApiManager GraphApi => this.graphApi ?? (graphApi = new GraphApiManager(this));

        public DbManager Db => this.db ?? (db = new DbManager(this));

        public PreconditionsHelper Preconditions =>
            this.preconditions ?? (preconditions = new PreconditionsHelper(this));

        public bool PageManagerExists => this.pages != null;
    }
}
