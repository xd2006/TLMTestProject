
namespace Tests.UI.Pages.General
{
    using Core.WeDriverService;

    using global::Tests.TestsData.Common.Enums;
    using global::Tests.UI.Components.General;

    using OpenQA.Selenium;

    public class AnyPage : PageTemplate
    {
        private By logoLocator = By.CssSelector("svg[class$='sidebar_brandLogo']");

        private string breadcrumbLocatorMask = "div[class$='subheader_root'] a:nth-of-type({0})";

        public AnyPage(IWebDriver driver)
            : base(driver)
        {
        }

        private SidePanel SidePanel => new SidePanel(Driver);

        public void ClickSideMenuSection(SidePanelData.Sections section)
        {
           this.SidePanel.ClickPanelSection(section);
        }

        public string GetActiveSectionName()
        {
            return this.SidePanel.GetActiveSectionName();
        }

        public void ClickLogo()
        {
            Driver.Find(this.logoLocator).Click();
        }

        public void NavigateUsingBreadcumbs(int numberOfElementInBreadcrumb)
        {
            Driver.Find(By.CssSelector(string.Format(this.breadcrumbLocatorMask, numberOfElementInBreadcrumb))).Click();
        }
    }
}
