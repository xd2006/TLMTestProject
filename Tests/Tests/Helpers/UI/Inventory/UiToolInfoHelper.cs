
using Tests.TestsData.Inventory.Enums;

namespace Tests.Helpers.UI.Inventory
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Core.Service;

    using global::Tests.Managers;
    using global::Tests.Models.ToolManager.GraphQlModels.ToolAssembly;
    using global::Tests.Models.ToolManager.UiModels;

    public class UiToolInfoHelper : UiCommonHelper
    {

        public bool IsPictureVisible => App.Pages.ToolsPages.ToolInfoPage.IsPictureVisible;

        public bool IsPurchaseBtnVisible => App.Pages.ToolsPages.ToolInfoPage.IsPurchaseBtnVisible;

        public bool IsActionBtnVisible => App.Pages.ToolsPages.ToolInfoPage.IsActionBtnVisible;

        public bool IsCreateToolPageOpened => App.Pages.ToolsPages.NewToolPopup.Opened();

        public string GetTitleOfCreationToolPage => App.Pages.ToolsPages.NewToolPopup.GetTitleName();

        public void CancelCreationOfNewTool() => App.Pages.ToolsPages.NewToolPopup.ClickCancel();

        public void CloseCreationOfNewTool() => App.Pages.ToolsPages.NewToolPopup.ClickXicon();

        public bool IsSaveEnabled => App.Pages.ToolsPages.NewToolPopup.IsSaveButtonEnabled();
        
        public string GetDetailsTabTitle => App.Pages.ToolsPages.ToolInfoPage.GetDetailsTabTitle();

        public UiToolInfoHelper(ApplicationManager app)
            : base(app)
        {
        }
        
        public ToolAssembly GetToolAssemblyInfo()
        {
            return this.GetToolInfo<ToolAssembly>();
        }

        public CutterAssembly GetCutterInfo()
        {
            return this.GetToolInfo<CutterAssembly>();
        }

        public Holder GetHolderInfo()
        {
            return this.GetToolInfo<Holder>();
        }
        
        public List<(string Name, int Quantity)> GetRelatedComponentsInfo()
        {
            return this.App.Pages.ToolsPages.ToolInfoPage.GetRelatedComponents();
        }

        public List<string> GetToolInfoFieldsNames()
        {
            return this.App.Pages.ToolsPages.ToolInfoPage.GetToolInfoFieldsNames();
        }

        public void CloseInfoPage()
        {
            //ToDo: change this method after tool info page design is implemented
            App.Ui.Main.PressBrowserBackButton();

            App.Pages.ToolsPages.ToolsMainPage.WaitForPageLoad();
            
            //grid is not being updated immediately. Temp solution to wait grid update
            bool equalLists;
            int counter = 0;
            do
            {
                equalLists = this.CheckNamesEqual();
             }
            while (!equalLists && counter++ < 5);
        }
        
        public bool IsInfoPopupOpened()
        {
            return this.App.Pages.ToolsPages.ToolInfoPage.Opened();
        }

        public List<string> GetToolInfoRelatedComponentsFieldsNames()
        {
            return this.App.Pages.ToolsPages.ToolInfoPage.GetToolInfoRelatedComponentsFieldsNames();
        }

        public List<string> GetInstacesGridColumnsNames()
        {
            return App.Pages.ToolsPages.ToolInfoPage.GetInstanceInStockTableColumnsNames();
        }

        public List<ToolInstanceGridRecord> GetToolInstances()
        {
            App.Pages.ToolsPages.ToolInfoPage.WaitForPageLoad(60);
            return App.Pages.ToolsPages.ToolInfoPage.GetToolInstances();
        }

        public List<CutterInstanceGridRecord> GetCutterInstances()
        {
            App.Pages.ToolsPages.ToolInfoPage.WaitForPageLoad(60);
            return App.Pages.ToolsPages.CutterInfoPage.GetCutterInstances();
        }

        public void ClickCreateNewInstanceButton() => App.Pages.ToolsPages.ToolInfoPage.ClickCreateNewIntanceBtn();

        public void CreateToolInstance(string assemblyId)
        {
            var assembliesInitial = App.Ui.ToolManagerToolInfo.GetToolInstances().Count;
            App.Pages.ToolsPages.NewToolPopup.PopulateField(CreateToolInstancePopup.ToolInstanceFileds.AssemblyId, assemblyId);
            App.Pages.ToolsPages.NewToolPopup.ClickSaveButton();
            App.Pages.ToolsPages.NewToolPopup.WaitForPageClosed();
            ServiceMethods.WaitForOperationPositive(() => App.Ui.ToolManagerToolInfo.GetToolInstances().Count > assembliesInitial);
        }
        
        public string GetToolCreationFieldPlaceholder(CreateToolInstancePopup.ToolInstanceFileds field)
        {
            return App.Pages.ToolsPages.NewToolPopup.GetFiledPlaceFolder(field);
        }
        
        public void DisassembleInstance(int instanceId)
        {
            App.Pages.ToolsPages.ToolInfoPage.Disassemble(instanceId);
        }

        #region private methods
        private bool CheckNamesEqual(int pauseSec = 2)
        {
            bool equalLists;
            var names1 = this.App.Pages.ToolsPages.ToolsMainPage.GetRecords().Select(r => r.Name).ToList();
            Thread.Sleep(pauseSec * 1000);
            var names2 = this.App.Pages.ToolsPages.ToolsMainPage.GetRecords().Select(r => r.Name).ToList();
            equalLists = names1.SequenceEqual(names2);
            return equalLists;
        }

        private T GetToolInfo<T>()
            where T : class
        {
            if (typeof(T) == typeof(ToolAssembly))
            {
                return this.App.Pages.ToolsPages.ToolInfoPage.GetToolAssemblyInfo() as T;
            }

            if (typeof(T) == typeof(CutterAssembly))
            {
                return this.App.Pages.ToolsPages.ToolInfoPage.GetCuttterAssemblyInfo() as T;
            }

            if (typeof(T) == typeof(Holder))
            {
                return this.App.Pages.ToolsPages.ToolInfoPage.GetHolderInfo() as T;
            }

            return null;
        }

        #endregion       
    }
}
