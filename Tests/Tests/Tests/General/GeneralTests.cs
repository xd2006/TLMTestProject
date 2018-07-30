
namespace Tests.Tests.General
{
    using System;
    using System.Collections.Generic;

    using global::Tests.Tests.General.Templates;
    using global::Tests.TestsData.Common.Enums;

    using NUnit.Framework;

    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [Category("General")]
    public class GeneralTests : GeneralTestTemplate
    {
        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-161")]
        [Property("TestCase", "1228")]
        [Property("TestCase", "1232")]
        [Property("TestCase", "1237")]
        [Property("TestCase", "1238")]
        public void CheckSideNavigationPanel()
        {
           
        Dictionary<SidePanelData.Sections, string> sectionsNames =
            new Dictionary<SidePanelData.Sections, string>
                {
                    { SidePanelData.Sections.Orders, "Orders" },
                    { SidePanelData.Sections.Tools, "Tools" },
                    { SidePanelData.Sections.Machines, "Machines" },
                    { SidePanelData.Sections.ToolLinking, "Tool linking" },
                    { SidePanelData.Sections.Todo, "Todo" },
                    };

            var dashboardPageOpened = App.Ui.Main.IsPageOpened(SidePanelData.Sections.Dashboard, true);

            Assert.True(dashboardPageOpened, "Dashboard page is not opened");


            foreach (var el in sectionsNames.Keys)
            {
                App.Ui.Main.NavigateToSectionInSideMenu(el);
                var pageOpened = App.Ui.Main.IsPageOpened(el, true);

                Assert.True(pageOpened, $"{el.ToString()} page is not opened");
                if (el != SidePanelData.Sections.ToolLinking)
                {
                    Assert.True(App.Ui.Main.GetActiveSideNavigationPanelSectionName().Equals(sectionsNames[el]));
                }
                else
                {
                    App.Ui.Link.CloseLinkPopup();
                }
            }

            App.Ui.Main.ClickLogo();
            dashboardPageOpened = App.Ui.Main.IsPageOpened(SidePanelData.Sections.Dashboard, true);

            Assert.True(dashboardPageOpened, "Dashboard page is not opened");
        }
    }
}
