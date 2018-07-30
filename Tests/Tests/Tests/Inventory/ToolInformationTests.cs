
namespace Tests.Tests.Inventory
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Core.Service;

    using global::Tests.Models.ToolManager.GraphQlModels.ToolAssembly;
    using global::Tests.Models.ToolManager.UiModels;
    using global::Tests.Tests.Inventory.Templates;
    using global::Tests.TestsData.Inventory.Enums.FilterSearch;

    using NUnit.Framework;

    [TestFixture]
    [Category("ToolInfo")]
    [Parallelizable(ParallelScope.All)]
    public class ToolInformationTests : InventoryTestsTemplate
    {
        [Test]
        [Property("Reference", "TLM-63")]
        [Property("Reference", "TLM-73")]
        [TestCase(true)]
        [TestCase(false)]
        public void ToolAssemblyDetailsPopUpDataApi(bool expandablePlateExists)
        {
            var expectedTool = this.GetExpectedToolAssembly(expandablePlateExists);

            var toolApi = this.App.GraphApi.ToolManager.SearchToolAssemblies(expectedTool.Name).First();

            var detailsApi = this.App.GraphApi.ToolManager.GetToolAssemblyDetails(toolApi.Id);

            this.CompareToolAssemblyDetails(expectedTool, detailsApi);
        }


        [Test,       
        TestCaseSource(
            typeof(TestDetailsDataSource),
        nameof(TestDetailsDataSource.TestCasesPositive))]
        [Category("UI")]
        [Property("Reference", "TLM-63")]
        [Property("Reference", "TLM-73")]
        public void TestToolAssemblyDetailsPopUpData(bool exchangablePlateExists)
        {
            var expectedTool = this.GetExpectedToolAssembly(exchangablePlateExists);

            this.App.Ui.ToolsMain.PerformSearch(expectedTool.Name);
            this.App.Ui.ToolsMain.ClickTool(expectedTool.Name);

            var detailsUi = this.App.Ui.ToolManagerToolInfo.GetToolAssemblyInfo();

            this.CompareToolAssemblyDetails(expectedTool, detailsUi);
        }


        [Test]
        [Category("UI")]
        [Property("TestCase", "143")]
        [Property("TestCase", "144")]
        [Property("TestCase", "152")]
        [Property("Reference", "TLM-63")]
        [Property("Reference", "TLM-73")]
        public void ToolAssemblyDetailsPopUpContent()
        {
            List<string> expectedFields = new List<string>
                                              {
                                                  "Short description",
                                                  "Name",
                                                  "Size",
                                                  "Length",
                                                  "Quantity",
                                                  "Operating depth",
                                                  "Usage material",
                                                  "Maximum lifetime usage allowed"
                                              };

            List<string> expectedRelatedComponentFields = new List<string> { "Component name", "Quantity" };

            this.App.Ui.ToolsMain.ClickRandomTool();
            var actualFields = this.App.Ui.ToolManagerToolInfo.GetToolInfoFieldsNames();

            Assert.That(expectedFields.SequenceEqual(actualFields), "Tool assembly details data is not as expected");

            var actualRelatedComponentsFields = this.App.Ui.ToolManagerToolInfo.GetToolInfoRelatedComponentsFieldsNames();

            Assert.That(
                expectedRelatedComponentFields.SequenceEqual(actualRelatedComponentsFields),
                "Tool assembly related components data is not as expected");



        }

        [Test]
        [Category("UI")]
        [Property("TestCase", "701")]       
        [Property("Reference", "TLM-70")]
       public void CutterDetailsPopUpContent()
        {
            List<string> expectedFields = new List<string>
                                              {
                                                  "Name",
                                                  "Size",
                                                  "Length",
                                                  "Quantity",
                                                  "Operating depth",
                                                  "Usage material",
                                                  "Maximum lifetime usage allowed"
                                              };
            this.App.Ui.ToolsMain.SelectToolType(FilterSearchData.ToolsTypes.Cutters);
            this.App.Ui.ToolsMain.ClickRandomTool();
            var actualFields = this.App.Ui.ToolManagerToolInfo.GetToolInfoFieldsNames();

            Assert.That(expectedFields.SequenceEqual(actualFields), "Cutter assembly details data is not as expected");
            


        }

        [Test]
        [Category("UI")]
        [Property("TestCase", "707")]
        [Property("Reference", "TLM-70")]
        public void HolderDetailsPopUpContent()
        {
            List<string> expectedFields = new List<string>
                                              {
                                                  "Name",
                                                  "Length",
                                                  "Quantity"                                                 
                                              };
            this.App.Ui.ToolsMain.SelectToolType(FilterSearchData.ToolsTypes.Holders);
            this.App.Ui.ToolsMain.ClickRandomTool();
            var actualFields = this.App.Ui.ToolManagerToolInfo.GetToolInfoFieldsNames();

            Assert.That(expectedFields.SequenceEqual(actualFields), "Holder details data is not as expected");
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-70")]
        [Property("TestCase", "712")]
        public void CutterDetailsPopUpData()
        {          
            CutterAssembly expectedTool = new CutterAssembly();
            expectedTool.Name = "000A300Q08514000";
            expectedTool.Cutter = new List<Cutter> { new Cutter() };
            expectedTool.Cutter.First().Diameter = 85000000;
            expectedTool.Length = 400000000;
            expectedTool.Quantity = 0;
            expectedTool.UsageMaterials = new List<string> { "PANTERA ALU", "PANTERA STAHL", "PANTERA WZ.STAHL", "STAHL", "STANDARD" };

            this.App.Ui.ToolsMain.SelectToolType(FilterSearchData.ToolsTypes.Cutters);
            this.App.Ui.ToolsMain.PerformSearch(expectedTool.Name);
            this.App.Ui.ToolsMain.ClickTool(expectedTool.Name);

            var detailsUi = this.App.Ui.ToolManagerToolInfo.GetCutterInfo();

            this.CompareCutterAssemblyDetails(expectedTool, detailsUi);
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-70")]
        [Property("TestCase", "713")]
        public void HolderDetailsPopUpData()
        {
            Holder expectedTool = new Holder();
            expectedTool.Name = "D16      SCHRUMPF POKOLM        EL100 HSK50";
            expectedTool.Length = 126000000;
            expectedTool.Quantity = 16;

            this.App.Ui.ToolsMain.SelectToolType(FilterSearchData.ToolsTypes.Holders);
            this.App.Ui.ToolsMain.PerformSearch(expectedTool.Name);
            this.App.Ui.ToolsMain.ClickTool(expectedTool.Name);

            var detailsUi = this.App.Ui.ToolManagerToolInfo.GetHolderInfo();

            this.CompareHolderDetails(expectedTool, detailsUi);
        }

        [Test]
        [Ignore("Outdated")]
        [Category("UI")]
        [Property("TestCase", "147")]
        [Property("Reference", "TLM-63")]
        public void CloseDetailsPopupClickingOut()
        {
            this.CheckDetailsPopupClosing(this.App.Ui.ToolManagerToolInfo.CloseInfoPage);
        }

        [Test]
        [Category("UI")]
        [Ignore("Outdated")]
        [Property("TestCase", "146")]
        [Property("Reference", "TLM-63")]
        public void CloseDetailsPopupRefreshBrowser()
        {
            this.CheckDetailsPopupClosing(this.App.Ui.ToolManagerToolInfo.RefreshPage);
        }

        [Test]
        [Category("UI")]
        [Ignore("Outdated")]
        [Property("TestCase", "149")]      
        [Property("Reference", "TLM-63")]
        public void CloseDetailsPopupEscButton()
        {
            this.CheckDetailsPopupClosing(this.App.Ui.ToolManagerToolInfo.PressEscButton);
        }

        [Test]
        [Category("UI")]
        [Property("TestCase", "148")]
        [Property("Reference", "TLM-63")]
        public void KeepAssemblySearchStateAfterClosingToolsInfoPopUp()
        {
            var searchTerm = "W158";
            this.App.Ui.ToolsMain.PerformSearch(searchTerm);
            var toolName = this.App.Ui.ToolsMain.ClickRandomTool();
            var infoBefore = this.App.Ui.ToolManagerToolInfo.GetToolAssemblyInfo();
            this.App.Ui.ToolManagerToolInfo.CloseInfoPage();

            var results = App.Ui.ToolsMain.GetAssembliesResults().Select(r => r.Name).ToList();
            Assert.True(!results.Contains(toolName), "Grid state wasn't updated");

            this.App.Ui.ToolsMain.PerformSearch(searchTerm);
            this.App.Ui.ToolsMain.ClickTool(toolName);
            var infoAfter = this.App.Ui.ToolManagerToolInfo.GetToolAssemblyInfo();
            Assert.That(infoBefore.Name
                .Equals(infoAfter.Name));
        }

        [Test]
        [Category("UI")]
        //outdated [Property("TestCase", "703")]
        [Property("TestCase", "705")]
        [Property("Reference", "TLM-70")]
        public void KeepCutterSearchStateAfterClosingToolsInfoPopUp()
        {
            var searchTerm = "APHPM";

            this.App.Ui.ToolsMain.SelectToolType(FilterSearchData.ToolsTypes.Cutters);
            this.App.Ui.ToolsMain.PerformSearch(searchTerm);
            var toolName = this.App.Ui.ToolsMain.ClickRandomTool();
            var infoBefore = this.App.Ui.ToolManagerToolInfo.GetCutterInfo();
            this.App.Ui.ToolManagerToolInfo.CloseInfoPage();

            var results = App.Ui.ToolsMain.GetCuttersResults().Select(r => r.Name).ToList();
            Assert.True(!results.Contains(toolName), "Grid state wasn't updated");

            this.App.Ui.ToolsMain.PerformSearch(searchTerm);

            this.App.Ui.ToolsMain.ClickTool(toolName);
            var infoAfter = this.App.Ui.ToolManagerToolInfo.GetCutterInfo();
            Assert.That(infoBefore.Name
                .Equals(infoAfter.Name));
        }

        [Test]
        [Category("UI")]
        // outdated[Property("TestCase", "709")]
        [Property("TestCase", "711")]
        [Property("Reference", "TLM-70")]
        public void KeepHolderSearchStateAfterClosingToolsInfoPopUp()
        {
            var searchTerm = "SPANNZANGE DEPO";

            this.App.Ui.ToolsMain.SelectToolType(FilterSearchData.ToolsTypes.Holders);
            this.App.Ui.ToolsMain.PerformSearch(searchTerm);
            var toolName = this.App.Ui.ToolsMain.ClickRandomTool();
            var infoBefore = this.App.Ui.ToolManagerToolInfo.GetHolderInfo();
            this.App.Ui.ToolManagerToolInfo.CloseInfoPage();

            var results = App.Ui.ToolsMain.GetAssembliesResults().Select(r => r.Name).ToList();
            Assert.True(!results.Contains(toolName), "Grid state wasn't updated");

            this.App.Ui.ToolsMain.PerformSearch(searchTerm);

            this.App.Ui.ToolsMain.ClickTool(toolName);
            var infoAfter = this.App.Ui.ToolManagerToolInfo.GetHolderInfo();
            Assert.That(infoBefore.Name
                .Equals(infoAfter.Name));
        }

        [Test]
        [Category("UI")]
        [Property("TestCase", "151")]
        [Property("Reference", "TLM-73")]
        public void NoRelatedComponentsForCuttersAndHolders()
        {
            this.App.Ui.ToolsMain.SelectToolType(FilterSearchData.ToolsTypes.Holders);
            this.App.Ui.ToolsMain.ClickRandomTool();
            var holdersComponentsInfo = this.App.Ui.ToolManagerToolInfo.GetRelatedComponentsInfo();
            this.App.Ui.ToolManagerToolInfo.CloseInfoPage();
            this.App.Ui.ToolsMain.SelectToolType(FilterSearchData.ToolsTypes.Cutters);
            this.App.Ui.ToolsMain.ClickRandomTool();
            var cuttersComponentsInfo = this.App.Ui.ToolManagerToolInfo.GetRelatedComponentsInfo();

            Assert.Multiple(() =>
                    {
                        Assert.True(
                            holdersComponentsInfo.Count == 0,
                            "Related components info shouldn't be displayed for holders");
                        Assert.True(
                            cuttersComponentsInfo.Count == 0,
                            "Related components info shouldn't be displayed for cutters");
                    });
        }

        [Test]
        [Category("UI")]
        [Property("TestCase", "345")]
        [Property("Reference", "TLM-73")]
        public void RelatedComponentsNumber()
        {
            var toolWithExpPlate = this.GetExpectedToolAssembly(true);
            var toolWoExpPlate = this.GetExpectedToolAssembly(false);

            this.App.Ui.ToolsMain.PerformSearch(toolWithExpPlate.Name);
            this.App.Ui.ToolsMain.ClickTool(toolWithExpPlate.Name);
            var relatedComponentsInfo = this.App.Ui.ToolManagerToolInfo.GetRelatedComponentsInfo();
            int i = 1;
            foreach (var info in relatedComponentsInfo)
            {
                var expectedValue = i % 2 == 0 ? toolWithExpPlate.CutterAssembly.Cutter.First().EdgeNr : 1;
                Assert.AreEqual(expectedValue, info.Value, "Wrong related components quantity is displayed");
                i++;
            }

            this.App.Ui.ToolManagerToolInfo.CloseInfoPage();
            this.App.Ui.ToolsMain.PerformSearch(toolWoExpPlate.Name);
            this.App.Ui.ToolsMain.ClickTool(toolWoExpPlate.Name);
            relatedComponentsInfo = this.App.Ui.ToolManagerToolInfo.GetRelatedComponentsInfo();
            i = 1;
            foreach (var info in relatedComponentsInfo)
            {
                var expectedValue = 1;
                Assert.AreEqual(expectedValue, info.Value, "Wrong related components quantity is displayed");
                i++;
            }
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-175")]
        [Property("TestCase", "823")]
        [Property("TestCase", "824")]
        public void CheckToolInstancesGridContent()
        {
            List<string> expectedColumnNames = new List<string>
                                                   {
                                                       "Name",
                                                       "ID",
                                                       "Size",
                                                       "Length",
                                                       "Location",
                                                       "Actual Usage Time",
                                                       "Maximum Usage Time",
                                                       "Status"
                                                   };
            string toolToCheck = "120W108D000F6060";

            var filters =
                new Dictionary<FilterSearchData.Filters, object>()
                    {
                        {
                            FilterSearchData.Filters.AvaliabilityInStock,
                            true
                        }
                    };


            App.Ui.ToolsMain.PerformFiltering(filters);
            App.Ui.ToolsMain.ClickTool(toolToCheck);
            var columnNames = App.Ui.ToolManagerToolInfo.GetInstacesInStockGridColumnsNames();
            expectedColumnNames = ServiceMethods.StringListToUpper(expectedColumnNames);
            Assert.True(columnNames.SequenceEqual(expectedColumnNames), "Instances in stock table header is wrong");
        }


        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-175")]
        [Property("TestCase", "826")]
        [Property("TestCase", "827")]
        [Property("TestCase", "828")]
        [Property("TestCase", "829")]
        [Property("TestCase", "830")]
        [Property("TestCase", "831")]
        [Property("TestCase", "832")]
        [Property("TestCase", "833")]
        public void CheckToolInstanceGridDataRelevance()
        {
            var expectedInstanceInStock = new InstanceInStockGridRecord()
                                               {
                                                   Name = "Test",
                                                   Id = 1,
                                                   Size = 98888888,
                                                   Length = 99991111,
                                                   Location = "Magazine 1",
                                                   ActualUsageTime = "02:46:40",
                                                   MaximumUsageTime = "05:33:20",
                                                   Status = "Ready"
                                               };
            string toolToCheck = "120W108D000F6060";

            var filters =
                new Dictionary<FilterSearchData.Filters, object>()
                    {
                        {
                            FilterSearchData.Filters.AvaliabilityInStock,
                            true
                        }
                    };

            App.Ui.ToolsMain.PerformFiltering(filters);
            App.Ui.ToolsMain.ClickTool(toolToCheck);
            var instancesInStock = App.Ui.ToolManagerToolInfo.GetInstancesInStock();

            var instanceToCheck = instancesInStock.First(i => i.Name.Equals(expectedInstanceInStock.Name));

            Assert.True(instanceToCheck.Equals(expectedInstanceInStock), "Instance in stock data is displayed incorrectly on the tool info page");
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-175")]
        [Property("TestCase", "825")]
        public void CheckEmptyToolInstanceGrid()
        {
            var record = App.Ui.ToolsMain.GetGridRecords().First(r => r.Quantity == 0);
            App.Ui.ToolsMain.ClickTool(record.Name);
            var instancesInStock = App.Ui.ToolManagerToolInfo.GetInstancesInStock();

            Assert.True(instancesInStock.Count==1 && instancesInStock.First().Name.Equals("No instances in stock"), "Empty grid is not displayed correctly");

        }



        #region private methods

        private void CheckDetailsPopupClosing(Action closingMethod)
        {
            this.App.Ui.ToolsMain.ClickRandomTool();
            var popUpIsOpened = this.App.Ui.ToolManagerToolInfo.IsInfoPopupOpened();
            Assert.IsTrue(popUpIsOpened, "Can't open popup");
            closingMethod.Invoke();
            popUpIsOpened = this.App.Ui.ToolManagerToolInfo.IsInfoPopupOpened();
            Assert.IsFalse(popUpIsOpened, "Can't close tool info pop up");
        }
       
        private ToolAssembly GetExpectedToolAssembly(bool expPlateExists = false)
        {
            var expectedTool = new ToolAssembly();
            if (expPlateExists)
            {              
                expectedTool.Name = "010K117S000F0155";
                expectedTool.CutterAssembly = new CutterAssembly();
                expectedTool.CutterAssembly.Cutter = new List<Cutter>();
                expectedTool.CutterAssembly.Cutter.Add(
                    new Cutter { Diameter = 1000000, Name = "KF 01.0 35KON 0.9 HITACHI GL80" });
                expectedTool.CutterAssembly.ExchangablePlate =
                    new ExchangablePlate() { Name = "D 16 ZYL HITACHI ABPF GL140" };
                expectedTool.CutterAssembly.Cutter.First().EdgeNr = 2;
                expectedTool.Length = 149000000;
                expectedTool.Quantity = 0;
                expectedTool.Holders = new List<Holder>();
                expectedTool.Holders.Add(new Holder { Name = "D06 SCHRUMPF DEPO EL68 HSK63" });
                expectedTool.UsageMaterials = new List<string>(){ "ALU", "STAHL", "WZ.STAHL" };
            }
            else
            {
                expectedTool.Name = "060K098T000F0650";
                expectedTool.CutterAssembly = new CutterAssembly();
                expectedTool.CutterAssembly.Cutter = new List<Cutter>();
                expectedTool.CutterAssembly.Cutter.Add(
                    new Cutter { Diameter = 6000000, Name = "KF 06.0 35KON 1.5 POKOLM GL75" });
                expectedTool.CutterAssembly.Cutter.First().EdgeNr = 1;
                expectedTool.Length = 126000000;
                expectedTool.Quantity = 0;
                expectedTool.Holders = new List<Holder>();
                expectedTool.Holders.Add(new Holder { Name = "D08 SCHRUMPF POKOLM EL50 HSK50" });
                expectedTool.UsageMaterials = new List<string>() { "ALU", "STAHL", "WZ.STAHL" };

            }

            return expectedTool;
        }


        private void CompareToolAssemblyDetails(ToolAssembly expectedTool, ToolAssembly actualTool)
        {
            Assert.Multiple(
                () =>
                    {
                        Assert.True(actualTool.Name.Equals(expectedTool.Name), "Name is wrong");
                        Assert.True(
                            actualTool.CutterAssembly.Cutter.First().Diameter
                                .Equals(expectedTool.CutterAssembly.Cutter.First().Diameter),
                            $"Size is wrong. Expected is {expectedTool.CutterAssembly.Cutter.First().Diameter} but actual is { actualTool.CutterAssembly.Cutter.First().Diameter}");
                        Assert.True(
                            ServiceMethods.RemoveMultipleInnerWhitespaces(actualTool.CutterAssembly.Cutter.First().Name)
                                .Equals(expectedTool.CutterAssembly.Cutter.First().Name),
                            $"Cutter component name is wrong. Expected '{expectedTool.CutterAssembly.Cutter.First().Name}'");
                        if (expectedTool.CutterAssembly.ExchangablePlate != null)
                        {
                            Assert.True(
                                actualTool.CutterAssembly.ExchangablePlate != null && ServiceMethods.RemoveMultipleInnerWhitespaces(actualTool.CutterAssembly.ExchangablePlate.Name)
                                    .Equals(expectedTool.CutterAssembly.ExchangablePlate.Name),
                                $"Exchangable plate is wrong. Expected '{expectedTool.CutterAssembly.ExchangablePlate.Name}'");
                        }
                        else
                        {
                            Assert.True(actualTool.CutterAssembly.ExchangablePlate == null, "Exchangable plate is wrong");
                        }

                        Assert.True(actualTool.Length.Equals(expectedTool.Length), "Length is wrong");
                        Assert.True(actualTool.Quantity.Equals(expectedTool.Quantity), "Quantity is wrong");
                        Assert.True(
                            ServiceMethods.RemoveMultipleInnerWhitespaces(actualTool.Holders.First().Name)
                                .Equals(expectedTool.Holders.First().Name),
                            "Holder name is wrong");

                        Assert.True(actualTool.CutterAssembly.Cutter.First().EdgeNr.Equals(actualTool.CutterAssembly.Cutter.First().EdgeNr), "Cutters quantity is incorrect");
                        Assert.True(actualTool.UsageMaterials.SequenceEqual(expectedTool.UsageMaterials), "Usage materials are wrong");
                    });
        }

        private void CompareCutterAssemblyDetails(CutterAssembly expectedTool, CutterAssembly actualTool)
        {
            Assert.Multiple(
                () =>
                {
                    Assert.True(actualTool.Name.Equals(expectedTool.Name), "Name is wrong");
                    Assert.True(
                        actualTool.Cutter.First().Diameter
                            .Equals(expectedTool.Cutter.First().Diameter),
                        $"Size is wrong. Expected {expectedTool.Cutter.First().Diameter} but actual {actualTool.Cutter.First().Diameter}");
                   
                    Assert.True(actualTool.Length.Equals(expectedTool.Length), "Length is wrong");
                    Assert.True(actualTool.Quantity.Equals(expectedTool.Quantity), "Quantity is wrong");
                    Assert.True(actualTool.UsageMaterials.SequenceEqual(expectedTool.UsageMaterials), "Usage materials are wrong");
                });
        }

        private void CompareHolderDetails(Holder expectedTool, Holder actualTool)
        {
            Assert.Multiple(
                () =>
                    {
                        Assert.True(actualTool.Name.Equals(expectedTool.Name), "Name is wrong");
                        Assert.True(actualTool.Length.Equals(expectedTool.Length), "Length is wrong");
                        Assert.True(actualTool.Quantity.Equals(expectedTool.Quantity), "Quantity is wrong");
                    });
        }


        private class TestDetailsDataSource
        {
            public static IEnumerable TestCasesPositive
            {
                get
                {
                    yield return new TestCaseData(false).SetProperty("TestCase", "375");
                    yield return new TestCaseData(true).SetProperty("TestCase", "150");
                }
            }
        }

        #endregion
    }  
}
