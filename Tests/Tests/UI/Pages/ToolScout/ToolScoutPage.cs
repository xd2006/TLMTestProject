using System.Linq;
using Core.WeDriverService;
using Core.WeDriverService.Extensions;
using OpenQA.Selenium;

namespace Tests.UI.Pages.ToolScout
{
    public class ToolScoutPage : PageTemplate
    {
        public ToolScoutPage(IWebDriver driver) : base(driver)
        {
        }

        private readonly By _drillingProcessSelection = By.CssSelector("*[alt$='Drilling']");

        private readonly By _materialGroupDropdown =
            By.XPath("//*[@for='materialgroupselection']/following-sibling::div");

        private readonly By _selectMaterialDropdown = By.XPath("//*[@for='materials']/following-sibling::div");

        private readonly By _continueBtn = By.XPath("//*[.='Continue']");

        private readonly By _checkBtn = By.XPath("//button[.='Check']");
        private readonly By _stendartDrillingTypeOfApplication = By.CssSelector("*[alt$='Standard drilling']");

        private readonly By _diameterOfTheHoleD1Dropdown = By.XPath("//*[@for='process.drilling.standard.d1']/following-sibling::div");

        private readonly By _depthfTheHoleL1Txt = By.Name("process.drilling.standard.l1");

        private readonly By _goToTlmBtn = By.XPath("//button[.='Go to TLM']");

        private readonly By _rowsLocator = By.XPath(".//*[contains(@class,'title')]");

        public void ClickDrillingProcessSelection()
        {
            Driver.WaitForElementToBeClickable(_drillingProcessSelection);
            Driver.Find(_drillingProcessSelection).Click();
        }

        public void ClickStandardDrilling()
        {
            Driver.WaitForElementToBeClickable(_stendartDrillingTypeOfApplication);
            Driver.Find(_stendartDrillingTypeOfApplication).Click();
        }

        public void GoToTlm()
        {
            Driver.WaitForElementToBeClickable(_goToTlmBtn);
            Driver.Find(_goToTlmBtn).Click();
        }

        public void ClickContinueBtn()
        {
            Driver.Find(_continueBtn).Click();
        }

        public bool IsCheckBtnDisplayed => Driver.Find(_checkBtn).Displayed;

        public void ClickCheckBtn()
        {
            Driver.Find(_checkBtn).Click();
        }

        public void SelectMaterialGroup(string value)
        {
            SelectFromDropdown(_materialGroupDropdown, value);
        }

        public void SelectMaterial(string value)
        {
            SelectFromDropdown(_selectMaterialDropdown, value);
        }
    
        public void SelectDiameterOfTheHoleD1(string value)
        {
            SelectFromDropdown(_diameterOfTheHoleD1Dropdown, value);
        }

        public void EnterDepthOfTheHoleL1(string value)
        {
            var field = Driver.Find(_depthfTheHoleL1Txt);
            field.SendKeys(value);
        }

        private void SelectFromDropdown(By elementLocator, string value)
        {
            Driver.WaitForElementToBeClickable(elementLocator);
            var dropdown = Driver.Find(elementLocator);
            dropdown.Click();
            dropdown.FindElements(_rowsLocator).First(row => row.Text.Equals(value)).Click();
        }
    }    
}
