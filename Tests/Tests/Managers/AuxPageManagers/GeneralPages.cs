
namespace Tests.Managers.AuxPageManagers
{
    using global::Tests.Managers.AuxPageManagers.Templates;
    using global::Tests.UI.Pages.General;
    using global::Tests.UI.Pages.Link;
    using global::Tests.UI.Pages.Machines;
    using global::Tests.UI.Pages.Todo;

    using OpenQA.Selenium;

    public class GeneralPages : PagesManagerTemplate
    {

        private DashboardPage dashboardPage;

        private AnyPage anyPage;
        
        private LinkPage link;

        private TodoPage todo;

        private SettingsPage settings;

        public GeneralPages(IWebDriver driver)
            : base(driver)
        {
        }

        public DashboardPage Dashboard => this.dashboardPage ?? (this.dashboardPage = new DashboardPage(Driver));

        public AnyPage AnyPage => this.anyPage ?? (this.anyPage = new AnyPage(Driver));
        
        public LinkPage LinkPage => this.link ?? (this.link = new LinkPage(Driver));

        public TodoPage TodoPage => this.todo ?? (this.todo = new TodoPage(Driver));

        public SettingsPage SettingsPage => this.settings ?? (this.settings = new SettingsPage(Driver));
    }
}
