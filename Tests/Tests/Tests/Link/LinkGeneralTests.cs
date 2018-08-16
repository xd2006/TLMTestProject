
namespace Tests.Tests.Link
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using global::Tests.Models.ToolManager.GraphQlModels.ToolAssembly;
    using global::Tests.Tests.Link.Templates;
    using global::Tests.TestsData.Common.Enums;

    using NUnit.Framework;

    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [Category("Link")]
    public class LinkGeneralTests : LinkTestsTemplate
    {

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-137")]
        [Property("TestCase", "1214")]
        [Property("TestCase", "1215")]
        [Property("TestCase", "1216")]
        [Property("TestCase", "1220")]
        [Property("TestCase", "1225")]
        [Property("TestCase", "1222")]
        public void CheckValidItemIdentifiaction()
        {
            string itemGuid1 = "VEQUJ67U0OEODQLT";
            string expectedDescription1 = "160W128H000F8080";

            string itemGuid2 = "E001504334667457865";
            string expectedDescription2 = "Preset Machine 5";

            App.Ui.Main.NavigateToSectionInSideMenu(SidePanelData.Sections.ToolLinking);
            var descriptions = App.Ui.Link.GetItemsDescriptions();
            App.Ui.Link.PopulateFirstItem(itemGuid1);
            var descriptionsAfterScan = App.Ui.Link.GetItemsDescriptions();
            var items = App.Ui.Link.GetItemsInputFiledsData();
            App.Ui.Link.PopulateSecondItem(itemGuid2);
            var descriptionsAfterScan2 = App.Ui.Link.GetItemsDescriptions();
            App.Ui.Link.ClickNewLinkButton();
            var descriptionsAfterNewLink = App.Ui.Link.GetItemsDescriptions();
            var itemsAfterNewLink = App.Ui.Link.GetItemsInputFiledsData();

            Assert.Multiple(
                () =>
                    {
                        Assert.True(
                            descriptions[0].Equals("Please scan object now")
                            && descriptions[1].Equals("2"),
                            "Initial items description is incorrect");
                        Assert.True(
                            descriptionsAfterScan[0].Equals(expectedDescription1) && descriptionsAfterScan[1]
                                .Equals("Please scan object now"),
                            "Items description after scan is incorrect");
                        Assert.True(
                            descriptionsAfterScan2[0].Equals(expectedDescription1) && descriptionsAfterScan2[1]
                                .Equals(expectedDescription2),
                            "Items descriptions after 2nd scan are incorrect");
                        Assert.True(
                            descriptionsAfterNewLink[0].Equals("Please scan object now")
                            && descriptionsAfterNewLink[1].Equals("2"),
                            "Initial items description is incorrect");
                        Assert.True(
                            items[0].Equals(itemGuid1) && items[1].Equals(string.Empty),
                            "Items codes are not displayed correctly in the input fields");
                        Assert.True(
                            itemsAfterNewLink[0].Equals(string.Empty) && itemsAfterNewLink[1].Equals(string.Empty),
                            "Items codes are not displayed correctly in the input fields");
                    });
        }


        [Test, TestCaseSource(typeof(ToolsDataSource), nameof(ToolsDataSource.TestCasesNegative))]
        [Category("UI")]
        [Property("Reference", "TLM-137")]
        public void CheckLinkValidation(string item1, string item2, List<string> expectedErrorMessage)
        {

            App.Ui.Main.NavigateToSectionInSideMenu(SidePanelData.Sections.ToolLinking);
            App.Ui.Link.PopulateFirstItem(item1);
            App.Ui.Link.PopulateSecondItem(item2);

            var message = App.Ui.Link.GetFailureResultMessage();

            Assert.True(expectedErrorMessage.Contains(message), $"Error message is invalid. Expected: {expectedErrorMessage}");
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-137")]
        [Property("TestCase", "1761")]
        [Property("TestCase", "1762")]
        public void ToolAndLocationLinkingTest()
        {
            #region test data

            Dictionary<string, string> locations =
                new Dictionary<string, string> { { "Magazine 5", "QWER1234" }, { "Magazine 1", "0WW7SYY1QP8QFGWJ" } };

            ToolAssembly targetAssembly = new ToolAssembly
                                              {
                                                  Id = "128",
                                                  Name = "006K066S000F0122"                                                 
                                              };
            string targetAssemblyInstanceGuid = "TESTINGVG1QWERTY1";
            int toolInstanceId = 81;

            #endregion

            App.Ui.ToolsMain.OpenToolAssemblyDirectly(targetAssembly.Id);
            var instanceInStock =
                App.Ui.ToolManagerToolInfo.GetToolInstances().First(i => i.Id.Equals(toolInstanceId));
            
            var targetLocation = locations.First(l => !l.Key.Equals(instanceInStock.Location));

            for (int i = 0; i < 2; i++)
            {
                App.Ui.Main.NavigateToSectionInSideMenu(SidePanelData.Sections.ToolLinking);

                App.Ui.Link.PopulateFirstItem(targetAssemblyInstanceGuid);
                App.Ui.Link.PopulateSecondItem(targetLocation.Value);

                var message = App.Ui.Link.GetSuccessResultMessage();

                Assert.True(message.Equals("Success"), "Linking wasn't successful");

                App.Ui.Link.CloseLinkPopup();
                App.Ui.ToolsMain.OpenToolAssemblyDirectly(targetAssembly.Id);

                instanceInStock = App.Ui.ToolManagerToolInfo.GetToolInstances()
                    .First(ins => ins.Id.Equals(toolInstanceId));

                Assert.True(instanceInStock.Location.Equals(targetLocation.Key));

                targetLocation = locations.First(l => !l.Key.Equals(targetLocation.Key));
            }
        }


        private class ToolsDataSource
        {
            public static IEnumerable TestCasesNegative
            {
                get
                {
                    yield return new TestCaseData("E001504334667457865", "E001504334667546605", new List<string> {"Incompatible types"}).SetProperty("TestCase", "1227");
                    yield return new TestCaseData("E001504334667457865", "SDT8W21IBNOGKYSH", new List<string> {"No consumer found"}).SetProperty("TestCase", "1258");
                    yield return new TestCaseData("I9ZFAIUHGZXJZRMR", "I9ZFAIUHGZXJZRMRT", new List<string> { "Incorrect identifier", "Incorrect id" }).SetProperty("TestCase", "1259");
                    yield return new TestCaseData("I9ZFAIUHGZXJZRMR", "9ULOZ5NJ3PX7NNYU", new List<string> {"Incorrect type", "Unknown type" }).SetProperty("TestCase", "1260");
                    yield return new TestCaseData("I9ZFAIUHGZXJZRMR", "I9ZFAIUHGZXJZRMR", new List<string> {"Unknown error"}).SetProperty("TestCase", "1218");}
                }
            }          
        }
    }
