
namespace Tests.Tests.Inventory
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using global::Tests.Tests.Inventory.Templates;
    using global::Tests.TestsData.Inventory.Enums.FilterSearch;

    using NUnit.Framework;

    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [Category("Ordering")]
    public class OrderingTests : InventoryTestsTemplate
    {
        [Test]
        [Category("UI")]
        [Property("TestCase", "386")]
        [Property("Bug", "TLM-348")]
        [Property("Reference", "TLM-71")]
        public void RemainingSortingAfterChangeFilters()
        {
            var columnName = FilterSearchData.GridColumnsNames.LENGTH;
            this.App.Ui.ToolsMain.ClickColumnName(columnName);

            Dictionary<FilterSearchData.Filters, object> filters =
                new Dictionary<FilterSearchData.Filters, object>
                    {
                        {
                            FilterSearchData.Filters.UsageMaterial,
                            "ALU"
                        },
                        {
                            FilterSearchData.Filters.ToolMaterial,
                            "HSS"
                        },
                        { FilterSearchData.Filters.Cooling, true }                       
                    };


            this.App.Ui.ToolsMain.PerformFiltering(filters);
            var names = this.GetDefinedIntResults(columnName);
            
            var currentSorting = this.App.Ui.ToolsMain.GetCurrentSorting();
            Assert.True(currentSorting.Key.Equals(columnName.ToString()) && currentSorting.Value.Equals("ASC"), "Ordering is not correct");
            Assert.That(names.SequenceEqual(names.OrderBy(n => n).ToList()), "Order is wrong");
        }

        [Test]
        [Category("UI")]
        [Property("TestCase", "387")]
        [Property("Reference", "TLM-71")]
        public void RemainingSortingAfterResetFilters()
        {
            var columnName = FilterSearchData.GridColumnsNames.SIZE;
            this.App.Ui.ToolsMain.ClickColumnName(columnName);

            Dictionary<FilterSearchData.Filters, object> filters =
                new Dictionary<FilterSearchData.Filters, object>
                    {
                        {
                            FilterSearchData.Filters.UsageMaterial,
                            "STAHL"
                        },
                        {
                            FilterSearchData.Filters.ToolMaterial,
                            "VHM1"
                        },
                        { FilterSearchData.Filters.Cooling, true }
                    };


            this.App.Ui.ToolsMain.PerformFiltering(filters);
            this.App.Ui.ToolsMain.ResetSearchAndFilters();

            var names = this.GetDefinedIntResults(columnName);
            var currentSorting = this.App.Ui.ToolsMain.GetCurrentSorting();

            Assert.True(currentSorting.Key.Equals(columnName.ToString()) && currentSorting.Value.Equals("ASC"), "Ordering is not correct");
            Assert.That(names.SequenceEqual(names.OrderBy(n => n).ToList()), "Order is wrong");
        }

        [Test]
        [Category("UI")]
        [Property("TestCase", "388")]
        [Property("Reference", "TLM-71")]
        public void ResettingSortingAfterSwitchingToolTypes()
        {
            var columnName = FilterSearchData.GridColumnsNames.SIZE;
            this.App.Ui.ToolsMain.ClickColumnName(columnName);
            this.App.Ui.ToolsMain.SelectToolType(FilterSearchData.ToolsTypes.Cutters);
          
            var sizes = this.GetDefinedIntResults(columnName);
            var currentSorting = this.App.Ui.ToolsMain.GetCurrentSorting();

            Assert.True(currentSorting.Key.Equals("NAME") && currentSorting.Value.Equals("ASC"), "Ordering is not correct");
            Assert.That(!sizes.SequenceEqual(sizes.OrderBy(n => n).ToList()), "Ordering wasn't reset");
        }



        //ToolAssembly model is used for all Tool types since there is no difference on UI
        [Test, TestCaseSource(typeof(OrderingDataSource), nameof(OrderingDataSource.DefaultOrderingTestCases))]
        [Category("UI")]
        [Property("Reference", "TLM-71")]
        public void GridDefaultSorting(FilterSearchData.ToolsTypes toolType, string searchTerm)
        {
            this.App.Ui.ToolsMain.SelectToolType(toolType);
            if (searchTerm != null)
            {
                this.App.Ui.ToolsMain.PerformSearch(searchTerm);
            }

            var sorting = this.App.Ui.ToolsMain.GetCurrentSorting();

            Assert.That(sorting.Key.Equals("NAME") && sorting.Value.Equals("ASC"));

            this.App.Ui.ToolsMain.ClickRandomPage();

            sorting = this.App.Ui.ToolsMain.GetCurrentSorting();

            Assert.That(sorting.Key.Equals("NAME") && sorting.Value.Equals("ASC"));
        }

        //ToolAssembly model is used for all Tool types since there is no difference on UI
        [Test, TestCaseSource(typeof(OrderingDataSource), nameof(OrderingDataSource.OrderingTestCases))]
        [Category("UI")]
        [Property("Reference", "TLM-71")]
        public void GridSorting(
            FilterSearchData.GridColumnsNames columnName,
            FilterSearchData.ToolsTypes toolType,
            string searchTerm)
        {
            this.App.Ui.ToolsMain.SelectToolType(toolType);
            this.App.Ui.ToolsMain.PerformSearch(searchTerm);

            this.App.Ui.ToolsMain.ClickColumnName(columnName);
            var sorting = this.App.Ui.ToolsMain.GetCurrentSorting();

            Assert.That(sorting.Key.Equals(columnName.ToString()) && sorting.Value.Equals("ASC"), $"Sorting is incorrect. Should be ASC by '{columnName}'");

            var names = this.GetDefinedIntResults(columnName);

            this.App.Ui.ToolsMain.ClickRandomPage();

            names.AddRange(this.GetDefinedIntResults(columnName));
            Assert.That(names.SequenceEqual(names.OrderBy(n => n).ToList()), "Order is wrong");

            App.Ui.ToolsMain.ClickPage(1);

            this.App.Ui.ToolsMain.ClickColumnName(columnName);

            sorting = this.App.Ui.ToolsMain.GetCurrentSorting();

            Assert.That(sorting.Key.Equals(columnName.ToString()) && sorting.Value.Equals("DESC"));

            names = this.GetDefinedIntResults(columnName);

            this.App.Ui.ToolsMain.ClickRandomPage();

            names.AddRange(this.GetDefinedIntResults(columnName));
         
           Assert.That(names.SequenceEqual(names.OrderByDescending(n => n).ToList()), "Order is wrong");            
        }

        //ToolAssembly model is used for all Tool types since there is no difference on UI
        [Test, TestCaseSource(typeof(OrderingDataSource), nameof(OrderingDataSource.OrderingByNameTestCases))]
        [Category("UI")]
        [Property("Reference", "TLM-71")]
        public void GridSortingByName(
            FilterSearchData.ToolsTypes toolType,
            string searchTerm)
        {
            this.App.Ui.ToolsMain.SelectToolType(toolType);
            this.App.Ui.ToolsMain.PerformSearch(searchTerm);

            var sorting = this.App.Ui.ToolsMain.GetCurrentSorting();


            Assert.That(sorting.Key.Equals("NAME") && sorting.Value.Equals("ASC"));

            var namesOriginal = this.GetNames();

            var page = this.App.Ui.ToolsMain.ClickRandomPage();

            namesOriginal.AddRange(this.GetNames());

            if (!toolType.Equals(FilterSearchData.ToolsTypes.Holders))
            {
                Assert.That(namesOriginal.SequenceEqual(namesOriginal.OrderBy(n => n).ToList()), "Order is wrong");
            }

            this.App.Ui.ToolsMain.ClickColumnName(FilterSearchData.GridColumnsNames.NAME);

            sorting = this.App.Ui.ToolsMain.GetCurrentSorting();

            Assert.That(sorting.Key.Equals("NAME") && sorting.Value.Equals("DESC"));

            var namesAfterSorting = this.GetNames();

            this.App.Ui.ToolsMain.ClickPage(page);

            namesAfterSorting.AddRange(this.GetNames());

            if (!toolType.Equals(FilterSearchData.ToolsTypes.Holders))
            {
                Assert.That(
                    namesAfterSorting.SequenceEqual(namesAfterSorting.OrderByDescending(n => n).ToList()),
                    "Order is wrong");
            }
            else
            {
                Assert.That(
                    !namesAfterSorting.SequenceEqual(namesOriginal),
                    "Order is wrong");
            }
        }

        //ToolAssembly model is used for all Tool types since there is no difference on UI
        private List<int> GetDefinedIntResults(FilterSearchData.GridColumnsNames searchFieldName)
        {
            switch (searchFieldName)
            {
                case FilterSearchData.GridColumnsNames.LENGTH:
                    {
                        return this.App.Ui.ToolsMain.GetAssembliesResults().Select(r => r.Length).ToList();
                    }

                case FilterSearchData.GridColumnsNames.QUANTITY:
                    {
                        return this.App.Ui.ToolsMain.GetAssembliesResults().Select(r => r.Quantity).ToList();
                    }

                case FilterSearchData.GridColumnsNames.SIZE:
                    {
                        return this.App.Ui.ToolsMain.GetAssembliesResults()
                            .Select(r => r.CutterAssembly.Cutter.First().Diameter).ToList();
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        private List<string> GetNames()
        {
            return this.App.Ui.ToolsMain.GetAssembliesResults().Select(r => r.Name).ToList();
        }
    }

    public class OrderingDataSource
    {
        public static IEnumerable OrderingTestCases
        {
            get
            {
                yield return new TestCaseData(
                    FilterSearchData.GridColumnsNames.SIZE,
                    FilterSearchData.ToolsTypes.Assemblies,
                    "000").SetProperty("TestCase", "127");
                yield return new TestCaseData(
                    FilterSearchData.GridColumnsNames.LENGTH,
                    FilterSearchData.ToolsTypes.Assemblies,
                    "000").SetProperty("TestCase", "128").SetProperty("Bug", "TLM-348");
                yield return new TestCaseData(
                    FilterSearchData.GridColumnsNames.QUANTITY,
                    FilterSearchData.ToolsTypes.Assemblies,
                    "000").SetProperty("TestCase", "385");

                yield return new TestCaseData(
                    FilterSearchData.GridColumnsNames.SIZE,
                    FilterSearchData.ToolsTypes.Cutters,
                    "00").SetProperty("TestCase", "191");
                yield return new TestCaseData(
                    FilterSearchData.GridColumnsNames.LENGTH,
                    FilterSearchData.ToolsTypes.Cutters,
                    "00").SetProperty("TestCase", "192").SetProperty("Bug", "TLM-348");
                yield return new TestCaseData(
                    FilterSearchData.GridColumnsNames.QUANTITY,
                    FilterSearchData.ToolsTypes.Cutters,
                    "00").SetProperty("TestCase", "390");

                yield return new TestCaseData(
                    FilterSearchData.GridColumnsNames.LENGTH,
                    FilterSearchData.ToolsTypes.Holders,
                    "EL").SetProperty("TestCase", "196").SetProperty("Bug", "TLM-348");
                yield return new TestCaseData(
                    FilterSearchData.GridColumnsNames.QUANTITY,
                    FilterSearchData.ToolsTypes.Holders,
                    "EL").SetProperty("TestCase", "389");
            }
        }

        public static IEnumerable OrderingByNameTestCases
        {
            get
            {
                yield return new TestCaseData(
                    FilterSearchData.ToolsTypes.Assemblies,
                    "000").SetProperty("TestCase", "126");
                yield return new TestCaseData(
                    FilterSearchData.ToolsTypes.Cutters,
                    "00").SetProperty("TestCase", "190");
                yield return new TestCaseData(
                    FilterSearchData.ToolsTypes.Holders,
                    "00").SetProperty("TestCase", "194");
            }
        }

        public static IEnumerable DefaultOrderingTestCases
        {
            get
            {
                yield return new TestCaseData(
                    FilterSearchData.ToolsTypes.Assemblies,
                    null).SetProperty("TestCase", "391");
                yield return new TestCaseData(
                    FilterSearchData.ToolsTypes.Assemblies,
                    "000").SetProperty("TestCase", "125");
                yield return new TestCaseData(
                    FilterSearchData.ToolsTypes.Cutters,
                    "00").SetProperty("TestCase", "189");
                yield return new TestCaseData(
                    FilterSearchData.ToolsTypes.Holders,
                    "EL").SetProperty("TestCase", "193");
            }
        }
    }
}


