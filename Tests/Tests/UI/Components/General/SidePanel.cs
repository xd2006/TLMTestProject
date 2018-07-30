
namespace Tests.UI.Components.General
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Core.Service;
    using Core.WeDriverService;

    using global::Tests.TestsData.Common.Enums;

    using OpenQA.Selenium;

    public class SidePanel : ComponentTemplate
    {
        public SidePanel(IWebDriver driver)
            : base(driver)
        {
        }

        private Dictionary<SidePanelData.Sections, string> sectionsNames =
            new Dictionary<SidePanelData.Sections, string>
                {
                    { SidePanelData.Sections.Orders, "Orders" },
                    { SidePanelData.Sections.Tools, "Tools" },
                    { SidePanelData.Sections.Machines, "Machines" },
                    { SidePanelData.Sections.ToolLinking, "Tool linking" },
                    { SidePanelData.Sections.Todo, "Todo" },
                    { SidePanelData.Sections.Settings, "Settings" }
                };


        private By sectionLocator = By.CssSelector("p[class$='link_text']");

        private By activeSectionLocator = By.CssSelector("div[class$='link_root__active'] > p[class$='route-link_text']");



        public void ClickPanelSection(SidePanelData.Sections section)
        {
            this.Driver.Finds(this.sectionLocator).First(s => s.Text.Equals(sectionsNames[section])).Click();

            if (section != SidePanelData.Sections.ToolLinking)
            {
                int counter = 0;
                while (this.GetActiveSectionName() != sectionsNames[section] && counter++ < 10)
                {
                    Thread.Sleep(1000);
                }

                if (counter >= 10)
                {
                    throw new Exception($"Expected section '{section.ToString()}' is not active after selecting");
                }
            }

        }

        public string GetActiveSectionName()
        {
            return this.Driver.Find(this.activeSectionLocator).Text;
        }

    }
}
