using Tests.Managers;

namespace Tests.Helpers.UI.ToolScout
{
    public class UiToolScoutHelper : UiCommonHelper
    {
        public UiToolScoutHelper(ApplicationManager app) : base(app)
        {
        }

        public void SelectTool(string materialGroup, string material, string diameter, string depth)
        {
            App.Pages.ToolScoutPage.ClickDrillingProcessSelection();
            App.Pages.ToolScoutPage.SelectMaterialGroup(materialGroup);
            App.Pages.ToolScoutPage.SelectMaterial(material);
            App.Pages.ToolScoutPage.ClickContinueBtn();
            App.Pages.ToolScoutPage.ClickStandardDrilling();
            App.Pages.ToolScoutPage.SelectDiameterOfTheHoleD1(diameter);
            App.Pages.ToolScoutPage.EnterDepthOfTheHoleL1(depth);
            if (App.Pages.ToolScoutPage.IsCheckBtnDisplayed)
            {
                App.Pages.ToolScoutPage.ClickCheckBtn();
            }            
            App.Pages.ToolScoutPage.ClickContinueBtn();
        }

        public void GoToTlm()
        {
            App.Pages.ToolScoutPage.GoToTlm();
        }
    }
}
