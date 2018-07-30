
namespace Tests.Helpers.UI.Inventory
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using global::Tests.Managers;
    using global::Tests.Models.ToolManager.GraphQlModels.ToolAssembly;
    using global::Tests.Models.ToolManager.UiModels;
    using global::Tests.TestsData.Common.Enums;

    using NUnit.Framework;

    public class UiToolInfoHelper : UiCommonHelper
    {
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

        public Dictionary<string, int> GetRelatedComponentsInfo()
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
                var names1 = App.Pages.ToolsPages.ToolsMainPage.GetRecords().Select(r => r.Name).ToList();
                Thread.Sleep(1000);
                var names2 = App.Pages.ToolsPages.ToolsMainPage.GetRecords().Select(r => r.Name).ToList();
                equalLists = names1.SequenceEqual(names2);
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

        public List<string> GetInstacesInStockGridColumnsNames()
        {
            return App.Pages.ToolsPages.ToolInfoPage.GetInstanceInStockTableColumnsNames();
        }

        public List<InstanceInStockGridRecord> GetInstancesInStock()
        {
            return App.Pages.ToolsPages.ToolInfoPage.GetInstacesInStock();
        }
    }
}
