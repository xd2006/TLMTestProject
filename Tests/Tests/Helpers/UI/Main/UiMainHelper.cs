
namespace Tests.Helpers.UI.Main
{
    using System;
    using System.Collections.Generic;

    using global::Tests.Managers;
    using global::Tests.TestsData.Common.Enums;

    public class UiMainHelper : UiCommonHelper
    {
        public UiMainHelper(ApplicationManager app)
            : base(app)
        {
        }

        public bool IsPageOpened(SidePanelData.Sections section, bool wait = false)
        {
            Dictionary<SidePanelData.Sections, Func<bool, bool>> sectionsData =
                new Dictionary<SidePanelData.Sections, Func<bool, bool>>
                    {
                        {
                            SidePanelData.Sections.Orders,
                            IsOrdersPageOpened
                        },
                        {
                            SidePanelData.Sections.Tools,
                            IsInventoryPageOpened
                        },
                        {
                            SidePanelData.Sections.Machines,
                            IsMachinesPageOpened
                        },
                        {
                            SidePanelData.Sections.ToolLinking,
                            IsLinkPageOpened
                        },
                        {
                            SidePanelData.Sections.Todo,
                            IsTodoPageOpened
                        },
                        {
                            SidePanelData.Sections.Settings,
                            IsSettingsPageOpened
                        },
                        {
                            SidePanelData.Sections.Dashboard,
                            this.IsDashboardPageOpened
                        },
                     };

            return sectionsData[section].Invoke(wait);
        }     

        public void NavigateToSectionInSideMenu(SidePanelData.Sections section)
        {
            App.Pages.GeneralPages.AnyPage.ClickSideMenuSection(section);
        }

        public string GetActiveSideNavigationPanelSectionName()
        {
            return App.Pages.GeneralPages.AnyPage.GetActiveSectionName();
        }

        public void ClickLogo()
        {
            App.Pages.GeneralPages.AnyPage.ClickLogo();
        }

        #region private methods

        private bool IsPageOpened(Func<bool> openCheckMethod, bool wait = false, Action waitMethod = null)
        {
            if (wait && waitMethod != null)
            {
                try
                {
                    waitMethod.Invoke();
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return openCheckMethod.Invoke();
        }

        private bool IsDashboardPageOpened(bool wait = true)
        {
            return IsPageOpened(
                App.Pages.GeneralPages.Dashboard.Opened,
                wait,
                App.Pages.GeneralPages.Dashboard.WaitForPageLoad);
        }

        private bool IsOrdersPageOpened(bool wait = true)
        {
            return IsPageOpened(
                App.Pages.OrdersPages.MainPage.Opened,
                wait,
                App.Pages.OrdersPages.MainPage.WaitForPageLoad);
        }

        private bool IsInventoryPageOpened(bool wait = true)
        {
            return IsPageOpened(
                App.Pages.ToolsPages.ToolsMainPage.Opened,
                wait,
                App.Pages.ToolsPages.ToolsMainPage.WaitForPageLoad);
        }

        private bool IsMachinesPageOpened(bool wait = true)
        {
            return IsPageOpened(
                App.Pages.MachinesPages.MachinesTaskAllocationPage.Opened,
                wait,
                App.Pages.MachinesPages.MachinesTaskAllocationPage.WaitForPageLoad);
        }

        private bool IsLinkPageOpened(bool wait = true)
        {
            return IsPageOpened(
                App.Pages.GeneralPages.LinkPage.Opened,
                wait,
                App.Pages.GeneralPages.LinkPage.WaitForPageLoad);
        }

        private bool IsTodoPageOpened(bool wait = true)
        {
            return IsPageOpened(
                App.Pages.GeneralPages.TodoPage.Opened,
                wait,
                App.Pages.GeneralPages.TodoPage.WaitForPageLoad);
        }

        private bool IsSettingsPageOpened(bool wait = true)
        {
            return IsPageOpened(
                App.Pages.GeneralPages.SettingsPage.Opened,
                wait,
                App.Pages.GeneralPages.SettingsPage.WaitForPageLoad);
        }

        #endregion

        public void NavigateUsingBreadcrumbs(int numberOfElementInBreadcrumb)
        {
            App.Pages.GeneralPages.AnyPage.NavigateUsingBreadcumbs(numberOfElementInBreadcrumb);
        }
    }
}
