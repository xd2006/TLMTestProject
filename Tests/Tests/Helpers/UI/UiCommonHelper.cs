
namespace Tests.Helpers.UI
{
    using System.Collections.Generic;
    using System.Linq;

    using global::Tests.Managers;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;

    public class UiCommonHelper : HelperTemplate
    {
        public UiCommonHelper(ApplicationManager app)
            : base(app)
        {          
        }

        /// <summary>
        /// The navigate to base url.
        /// </summary>
        public void NavigateToBaseUrl()
        {
            App.Pages.Driver.Navigate().GoToUrl(Parameters.Parameters.ApplicationUrl);
        }

        public void NavigateToUrlRelatively(string path)
        {
            App.Pages.Driver.Navigate().GoToUrl(Parameters.Parameters.ApplicationUrl + path);
        }

        public void RefreshPage()
        {
            App.Pages.Driver.Navigate().Refresh();
        }

        public void PressEscButton()
        {
            Actions action = new Actions(App.Pages.Driver);
            action.SendKeys(Keys.Escape).Perform();
        }

        public void PressBrowserBackButton()
        {
          App.Pages.Driver.Navigate().Back();
        }

        public List<LogEntry> GetBrowserLogs()
        {
            return App.Pages.Driver.Manage().Logs.GetLog(LogType.Browser).ToList();           
        }
    }
}
