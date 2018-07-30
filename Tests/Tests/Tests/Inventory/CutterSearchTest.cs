
namespace Tests.Tests.Inventory
{
    using System.Collections;

    using global::Tests.Tests.Inventory.Templates;
    using global::Tests.TestsData.Inventory.Enums.FilterSearch;

    using NUnit.Framework;

    public class CutterSearchTest : InventoryTestsTemplate
    {
        [Test, TestCaseSource(typeof(SearchDataSource), nameof(SearchDataSource.SearchTestCasesPositive))]
        [Property("Reference", "TLM-70")]
        public void CuttersSearchPositive(string searchTerm)
        {
            var cutters = this.App.GraphApi.ToolManager.SearchCutters(searchTerm);

            Assert.That(cutters.Count > 0, "GraphApi didn't return any data");

            var results = this.CheckCutterAssemblyItemsAreCorrect(
                FilterSearchData.Filters.Search,
                searchTerm,
                cutters);

            Assert.True(
                results.Key,
                $"Next CutterAssemblies don't comply with search term {searchTerm}: {results.Value}");
        }

        [Test, TestCaseSource(typeof(SearchDataSource), nameof(SearchDataSource.SearchTestCasesNegative))]
        [Category("UI")]
        [Property("Reference", "TLM-70")]
        public void CuttersSearchNegative(string searchTerm)
        {
            var cutters = this.App.GraphApi.ToolManager.SearchCutters(searchTerm);

            Assert.That(cutters.Count == 0, "GraphApi shouldn't return any data");

            this.App.Ui.ToolsMain.SelectToolType(FilterSearchData.ToolsTypes.Cutters);
            this.App.Ui.ToolsMain.PerformSearch(searchTerm);
            var uiRsults = this.App.Ui.ToolsMain.GetCuttersResults();

            Assert.That(uiRsults.Count == 0, "Ui shouldn't return any data");
        }


        private class SearchDataSource
        {
            public static IEnumerable SearchTestCasesPositive
            {
                get
                {
                    yield return new TestCaseData("000A").SetProperty("TestCase", "159");
                    yield return new TestCaseData("SANDVIK").SetProperty("TestCase", "160");
                    yield return new TestCaseData("042").SetProperty("TestCase", "161");
                }
            }

            public static IEnumerable SearchTestCasesNegative
            {
                get
                {
                    yield return new TestCaseData("s#$3").SetProperty("TestCase", "164");
                }
            }
        }


    }
}
