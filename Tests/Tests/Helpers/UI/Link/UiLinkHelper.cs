
namespace Tests.Helpers.UI.Link
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using AngleSharp.Css.Values;

    using global::Tests.Managers;

    public class UiLinkHelper : UiCommonHelper
    {
        public UiLinkHelper(ApplicationManager app)
            : base(app)
        {
        }


        private string linkInitialText = "Please scan object now";

        public List<string> GetItemsDescriptions()
        {
           return App.Pages.GeneralPages.LinkPage.GetItemsInfo();
        }

        public void PopulateFirstItem(string itemGuid)
        {
            string initialDescription = this.linkInitialText;
            App.Pages.GeneralPages.LinkPage.PopulateItem(1, itemGuid);

            this.WaitItemDescriptionUpdated(initialDescription, 1);
        }

        public void PopulateSecondItem(string itemGuid)
        {
            string initialDescription = this.linkInitialText;
            App.Pages.GeneralPages.LinkPage.PopulateItem(2, itemGuid);

            this.WaitItemDescriptionUpdated(initialDescription, 2);
        }

        private void WaitItemDescriptionUpdated(string initialDescription, int itemNumber)
        {
            string description = string.Empty;
            int counter = 0;
            do
            {
                description = this.GetItemsDescriptions()[itemNumber - 1];
                if (description.Equals(initialDescription))
                {
                    Thread.Sleep(500);
                }
            }
            while (description == initialDescription && counter++ < 10);

            if (counter >= 10)
            {
                this.App.Logger.Warn("Link item description wasn't updated");
                throw new Exception("Item description wasn't updated");
            }
        }

        public void ClickNewLinkButton()
        {
            App.Pages.GeneralPages.LinkPage.ClickNewLinkButton();
        }

        public List<string> GetItemsInputFiledsData()
        {
            return App.Pages.GeneralPages.LinkPage.GetItemsInputFieldsData();
        }

        public string GetSuccessResultMessage()
        {
            return App.Pages.GeneralPages.LinkPage.GetResultMessage(true);
        }

        public string GetFailureResultMessage()
        {
            return App.Pages.GeneralPages.LinkPage.GetResultMessage(false);
        }

        public void CloseLinkPopup()
        {
            App.Pages.GeneralPages.LinkPage.WaitForPageLoad();
            App.Pages.GeneralPages.LinkPage.Close();
        }
    }
}
