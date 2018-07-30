
namespace Tests.Tests.Orders
{
    using System.Collections.Generic;
    using System.Linq;

    using Core.Service;

    using global::Tests.Tests.Orders.Templates;

    using NUnit.Framework;

    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [Category("ProjectManagerGeneral")]
    public class PmGeneralTests : OrdersTestTemplate
    {      
        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-145")]
        [Property("TestCase", "693")]
        public void CheckProjectManagerGridContent()
        {
            var expectedColumns = new List<string>
                                      {
                                          "Order ID",
                                          "Customer",
                                          "Delivery Date",
                                          "Editor",
                                          "Creation Date"
                                      };

            if (Parameters.Parameters.Browser != "MicrosoftEdge")
            {
                expectedColumns = ServiceMethods.StringListToUpper(expectedColumns);
            }

            var names = this.App.Ui.OrdersMain.GetColumnsNames();

            Assert.That(names.SequenceEqual(expectedColumns), "Grid columns names are not as expected");
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-145")]
        [Property("TestCase", "816")]
        [Property("TestCase", "695")]
        [Property("TestCase", "697")]
        [Property("TestCase", "698")]
        [Property("TestCase", "699")]
        public void ProjectManagerCheckPaging()
        {
            this.App.GraphApi.ProjectManager.AddOrdersIfNeeded(26);

            this.App.Ui.OrdersMain.SetMaximumNumberOfResults(5);
            var results = this.App.Ui.OrdersMain.GetOrderGridRecords().Select(t => t.OrderId).ToList();
            this.App.Ui.OrdersMain.ClickNextButton();
            var newResults = this.App.Ui.OrdersMain.GetOrderGridRecords().Select(t => t.OrderId).ToList();
            Assert.That(
                newResults.Where(t => results.Contains(t)).ToList().Count == 0,
                "Pages forward navigation doesn't work");
            this.App.Ui.OrdersMain.ClickPreviousButton();
            newResults = this.App.Ui.OrdersMain.GetOrderGridRecords().Select(t => t.OrderId).ToList();
            Assert.That(results.SequenceEqual(newResults), "Pages backward navigation doesn't work");
            var page = this.App.Ui.OrdersMain.ClickRandomPage();
            var activePage = this.App.Ui.OrdersMain.GetActivePage();
            Assert.That(page.Equals(activePage), "Active page is not highlighted");
            newResults = this.App.Ui.OrdersMain.GetOrderGridRecords().Select(t => t.OrderId).ToList();
            Assert.That(
                newResults.Where(t => results.Contains(t)).ToList().Count == 0,
                "Pages navigation doesn't work");
        }

        [Test]
        [Category("UI")]
        [Property("TestCase", "696")]
        [Property("Reference", "TLM-145")]
        public void PageManagerPageQuantity()
        {
            this.App.GraphApi.ProjectManager.AddOrdersIfNeeded(26);
            int quantityByDefault = 25;
            try
            {
                var actualGridSize = this.App.Ui.OrdersMain.GetMaximumNumberOfResults();

                Assert.AreEqual(quantityByDefault, actualGridSize);

                var results = this.App.Ui.OrdersMain.GetOrderGridRecords();

                Assert.AreEqual(quantityByDefault, results.Count, "Results quantity option doesn't work");

                results = this.App.Ui.OrdersMain.GetOrderGridRecords();

                Assert.AreEqual(quantityByDefault, results.Count, "Results quantity option doesn't work");

                this.App.Ui.OrdersMain.SetMaximumNumberOfResults(10);
                results = this.App.Ui.OrdersMain.GetOrderGridRecords();

                Assert.AreEqual(10, results.Count, "Results quantity option doesn't work");
            }
            finally
            {
                this.App.Ui.OrdersMain.SetMaximumNumberOfResults(25);
            }
        }      
    }
}
