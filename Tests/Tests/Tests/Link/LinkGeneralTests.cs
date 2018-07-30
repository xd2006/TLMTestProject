
namespace Tests.Tests.Link
{
    using System.Collections;

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
            string itemGuid1 = "P1575ZSYZG90GKOJ";
            string expectedDescription1 = "120W158H000F6060";

            string itemGuid2 = "E00401504C2C2C08";
            string expectedDescription2 = "Machine 1";

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
                            descriptions[0].Equals("Please scan the first item now")
                            && descriptions[1].Equals("Please scan the second item now"),
                            "Initial items description is incorrect");
                        Assert.True(
                            descriptionsAfterScan[0].Equals(expectedDescription1) && descriptionsAfterScan[1]
                                .Equals("Please scan the second item now"),
                            "Items description after scan is incorrect");
                        Assert.True(
                            descriptionsAfterScan2[0].Equals(expectedDescription1) && descriptionsAfterScan2[1]
                                .Equals(expectedDescription2),
                            "Items descriptions after 2nd scan are incorrect");
                        Assert.True(
                            descriptionsAfterNewLink[0].Equals("Please scan the first item now")
                            && descriptionsAfterNewLink[1].Equals("Please scan the second item now"),
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
        public void CheckLinkValidation(string item1, string item2, string expectedErrorMessage)
        {

            App.Ui.Main.NavigateToSectionInSideMenu(SidePanelData.Sections.ToolLinking);
            App.Ui.Link.PopulateFirstItem(item1);
            App.Ui.Link.PopulateSecondItem(item2);

            var message = App.Ui.Link.GetFailureResultMessage();

            Assert.True(message.Equals(expectedErrorMessage), $"Error message is invalid. Expected: {expectedErrorMessage}");
        }

        private class ToolsDataSource
        {
            public static IEnumerable TestCasesNegative
            {
                get
                {
                    yield return new TestCaseData("PFPI8MW9ZCRMJCNH", "6VAY058R5GC9O8EH", "Incompatible types").SetProperty("TestCase", "1227");
                    yield return new TestCaseData("E00401504C2C2C08", "YE9UMDSOIS34VQAA", "No consumer found").SetProperty("TestCase", "1258");
                    yield return new TestCaseData("E00401504C2C2C08", "E10411514C2C2C18", "Incorrect identifier").SetProperty("TestCase", "1259");
                    yield return new TestCaseData("E00411514C2C2C18", "JG9YAXPO8BJQ0Z8R", "Incorrect type").SetProperty("TestCase", "1260");
                    yield return new TestCaseData("WFRQOTTD6WW6FN1J", "WFRQOTTD6WW6FN1J", "Unknown error").SetProperty("TestCase", "1218");
                }
            }
            
        }

    }
}
