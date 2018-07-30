
namespace Tests.Tests.Inventory
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Core.Service;

    using global::Tests.Tests.Inventory.Templates;
    using global::Tests.TestsData.Inventory.Enums.FilterSearch;

    using NUnit.Framework;

    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [Category("InventoryMain")]
    public class AssemblySearchTests : InventoryTestsTemplate
    {
        [Test]
        [Property("TestCase", "112")] 
        [Property("TestCase", "113")] 
        [Property("Reference", "TLM-62")]
        [Category("UI")]
        public void SearchFieldCheck()
        {
            var searchTerm = "1111";
            var placeholder = this.App.Ui.ToolsMain.GetSearchFiledName();
            Assert.That(placeholder.Equals("Search..."), $"ToolManager field placeholder in not correct. Actual value is '{placeholder}'");
            this.App.Ui.ToolsMain.PerformSearch(searchTerm);
            var actualSearchTerm = this.App.Ui.ToolsMain.GetSearchFieldValue();

            Assert.That(
                actualSearchTerm.Equals(searchTerm),
                $"ToolManager search term is incorrect. Actual term is '{actualSearchTerm}' but should be {searchTerm}");

            this.App.Ui.ToolsMain.ClearSearch();

            actualSearchTerm = this.App.Ui.ToolsMain.GetSearchFieldValue();

            Assert.That(string.IsNullOrEmpty(actualSearchTerm), "ToolManager field should be reset but it wasn't");
        }

        [Test]
        [Category("UI")]
        [Property("TestCase", "115")] 
        [Property("Reference", "TLM-62")]
        public void CheckSearchGridTitles()
        {
            var expectedColumns = new List<string> { "NAME", "SIZE", "LENGTH", "QUANTITY" };
            var searchTerm = "W128";

            this.App.Ui.ToolsMain.PerformSearch(searchTerm);
            var columnNames = this.App.Ui.ToolsMain.GetGridColumnsNames();
            columnNames = ServiceMethods.RemoveStringsInList(columnNames, new List<string> { "▲", "▼" });
            
            var columnNamesToUpper = new List<string>();
            columnNames.ForEach(e => columnNamesToUpper.Add(e.ToUpper()));
            Assert.That(columnNamesToUpper.SequenceEqual(expectedColumns), "Grid columns names are not as expected");
        }

        [Test]
        [Category("UI")]
        [Property("TestCase", "119")]
        [Property("Reference", "TLM-70")]
        public void AssembliesEmptySearchGrid()
        {
            this.CheckEmptySearchGridTest(FilterSearchData.ToolsTypes.Assemblies);
        }
        
        [Test]
        [Category("UI")]
        [Property("TestCase", "121")]
        [Property("Reference", "TLM-62")]
        public void PageQuantity()
        {
            int quantityByDefault = 25;
            try
            {
                var actualGridSize = this.App.Ui.ToolsMain.GetMaximumNumberOfResults();
               
                Assert.AreEqual(quantityByDefault, actualGridSize);

                var results = this.App.Ui.ToolsMain.GetAssembliesResults();

                Assert.AreEqual(quantityByDefault, results.Count, "Results quantity option doesn't work");

                results = this.App.Ui.ToolsMain.GetAssembliesResults();

                Assert.AreEqual(quantityByDefault, results.Count, "Results quantity option doesn't work");

                this.App.Ui.ToolsMain.SetMaximumNumberOfResults(10);
                results = this.App.Ui.ToolsMain.GetAssembliesResults();

                Assert.AreEqual(10, results.Count, "Results quantity option doesn't work");
            }
            finally
            {
                this.App.Ui.ToolsMain.SetMaximumNumberOfResults(25);
            }
        }

        [Test]
        [Category("UI")]
        [Property("TestCase", "122")]
        [Property("TestCase", "123")]
        [Property("TestCase", "124")]
        [Property("TestCase", "120")]
        public void PagingChecks()
        {
            var results = this.App.Ui.ToolsMain.GetAssembliesResults().Select(t => t.Name).ToList();
            this.App.Ui.ToolsMain.ClickNextButton();
            var newResults = this.App.Ui.ToolsMain.GetAssembliesResults().Select(t => t.Name).ToList();
            Assert.That(
                newResults.Where(t => results.Contains(t)).ToList().Count == 0,
                "Pages forward navigation doesn't work");
            this.App.Ui.ToolsMain.ClickPreviousButton();
            newResults = this.App.Ui.ToolsMain.GetAssembliesResults().Select(t => t.Name).ToList();
            Assert.That(results.SequenceEqual(newResults), "Pages backward navigation doesn't work");
            var page = this.App.Ui.ToolsMain.ClickRandomPage();
            var activePage = this.App.Ui.ToolsMain.GetActivePage();
            Assert.That(page.Equals(activePage), "Active page is not highlighted");
            newResults = this.App.Ui.ToolsMain.GetAssembliesResults().Select(t => t.Name).ToList();
            Assert.That(
                newResults.Where(t => results.Contains(t)).ToList().Count == 0,
                "Pages navigation doesn't work");
        }
        
        [Test,
         TestCaseSource(
             typeof(SearchDataSource),
             nameof(SearchDataSource.TestCasesPositive))]
        [Category("UI")]
        [Property("Reference", "TLM-62")]
        public void SearchValidityPositive(string searchTerm)
        {
            var itemsApi = this.App.GraphApi.ToolManager.SearchToolAssemblies(searchTerm, 1000);

            Assert.That(itemsApi.Count > 0, "Data wasn't returned from GraphApi");

            bool validResults = this.CheckToolAssemblyItemsAreCorrect(FilterSearchData.Filters.Search, searchTerm, itemsApi).Key;

            this.App.Ui.ToolsMain.PerformSearch(searchTerm);
            var itemsUi = this.App.Ui.ToolsMain.GetAssembliesResults();

            Assert.Multiple(
                () =>
                    {
                        Assert.True(validResults, "Not all items correspond to the search request");
                        Assert.True(
                            itemsApi.Count.Equals(itemsUi.Count),
                            "Numbers of items from UI and GraphAPI are different");
                    });

            Assert.True(itemsUi.Count.Equals(itemsApi.Count), $"Number of items from API differs from number of items from UI: ");
            this.CompareToolAssemblyRecordsFromApiAndUi(itemsApi, itemsUi);
         }

        [Test,
        TestCaseSource(
            typeof(SearchDataSource),
            nameof(SearchDataSource.TestCasesNegative))]
        [Category("UI")]
        [Property("Reference", "TLM-62")]
        public void SearchValidityNegative(string searchTerm)
        {
            var itemsApi = this.App.GraphApi.ToolManager.SearchToolAssemblies(searchTerm);

            Assert.That(itemsApi.Count == 0, "Data shouldn't be returned from GraphApi");
            
            this.App.Ui.ToolsMain.PerformSearch(searchTerm);
            var itemsUi = this.App.Ui.ToolsMain.GetAssembliesResults();
            
                Assert.True(itemsUi.Count == 0, "Items shouldn't be returned on Ui");           
        }

        private class SearchDataSource
        {
            public static IEnumerable TestCasesPositive
            {
                get
                {
                    yield return new TestCaseData("W128").SetProperty("TestCase", "114");
                    yield return new TestCaseData("SDG").SetProperty("TestCase", "116");
                    yield return new TestCaseData("5050").SetProperty("TestCase", "117");
                }
            }

            public static IEnumerable TestCasesNegative
            {
                get
                {
                    yield return new TestCaseData("s#$3").SetProperty("TestCase", "118");
                }
            }
        }
    }
}

