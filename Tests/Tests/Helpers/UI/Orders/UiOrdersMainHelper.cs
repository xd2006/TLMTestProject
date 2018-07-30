
namespace Tests.Helpers.UI.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Core.WeDriverService.Extensions;

    using global::Tests.Managers;
    using global::Tests.Models.ProjectManager.DbModels.Postgres;
    using global::Tests.Models.ProjectManager.UiModels;
    using global::Tests.TestsData.Orders;

    using PetaPoco;

    public class UiOrdersMainHelper : UiCommonHelper
    {
        public UiOrdersMainHelper(ApplicationManager app)
            : base(app)
        {
        }

       public List<string> GetColumnsNames()
        {
            return this.App.Pages.OrdersPages.MainPage.GetOrdersGridColumnsNames();          
        }

        public List<OrderGridRecord> GetOrderGridRecords()
        {
            return this.App.Pages.OrdersPages.MainPage.GetGridRecords();
        }

        public void ClickNextButton()
        {
            this.UpdateGrid(this.App.Pages.OrdersPages.MainPage.ClickNextButton);
        }

        public void ClickPreviousButton()
        {
            this.UpdateGrid(this.App.Pages.OrdersPages.MainPage.ClickPreviousButton);
        }
        
        public int ClickRandomPage()
        {
            var elements = this.App.Pages.OrdersPages.MainPage.GetRawGridLineElements();

            var pages = this.App.Pages.OrdersPages.MainPage.GetAvailablePages();
            Random random = new Random();
            var selectedPage = random.Next(1, pages.Count - 1);
            var page = pages[selectedPage];
            this.App.Pages.ToolsPages.ToolsMainPage.ClickPage(page);


            if (!this.App.Pages.Driver.WaitForElementStaleBool(elements.Last(), 10))
            {
                this.App.Logger.Warn("Grid data possibly wasn't updated");
            }

            return int.Parse(page);
        }

        public int GetActivePage()
        {
            return this.App.Pages.OrdersPages.MainPage.GetActivePage();
        }

        public void SetMaximumNumberOfResults(int number)
        {
            List<int> allowedValues = new List<int> { 5, 10, 25 };
            if (!allowedValues.Contains(number))
            {
                this.App.Logger.Error("Invalid option was provided for 'Number of results' option. Default number is used");
                number = allowedValues.LastOrDefault();
            }

            this.App.Pages.OrdersPages.MainPage.SetNumberOfResultsOption(number);

            int num;
            do
            {
                num = this.App.Pages.OrdersPages.MainPage.GetRawGridLineElements().Count;
            }
            while (num != number);
        }

        public double GetMaximumNumberOfResults()
        {
             return this.App.Pages.OrdersPages.MainPage.GetDefinedNumberOfResultsOption();          
        }

        public void ClickCreateNewOrderButton()
        {
            this.App.Pages.OrdersPages.MainPage.ClickCreateNewOrderButton();
            this.App.Pages.OrdersPages.CreateOrderPopup.WaitForPageLoad();

        }

        public Order FindOrder(string orderId, bool tryLastPageFirstly = false)
        {
            Order foundOrder = null;

            if (tryLastPageFirstly)
            {
                var pages = App.Pages.OrdersPages.MainPage.GetAvailablePages();
                App.Pages.OrdersPages.MainPage.ClickPage(pages.Last());
                var orderRecord = this.App.Pages.OrdersPages.MainPage.GetGridRecords()
                    .FirstOrDefault(r => r.OrderId.Equals(orderId));
                if (orderRecord != null)
                {
                    foundOrder = this.ConvertGridOrderRecordToOrder(orderRecord);
                    return foundOrder;
                }              
                    App.Pages.OrdersPages.MainPage.ClickPage(pages.First());              
            }
           
            bool isNextButtonActive;
            do
            {
                isNextButtonActive = this.App.Pages.OrdersPages.MainPage.IsNextButtonActive();
                var orderRecord = this.App.Pages.OrdersPages.MainPage.GetGridRecords()
                    .FirstOrDefault(r => r.OrderId.Equals(orderId));
                if (orderRecord != null)
                {
                    foundOrder = this.ConvertGridOrderRecordToOrder(orderRecord);
                }

                if (foundOrder == null && isNextButtonActive)
                {
                    this.UpdateGrid(this.App.Pages.OrdersPages.MainPage.ClickNextButton);                   
                }
            }
            while (foundOrder == null && isNextButtonActive);

                return foundOrder;
        }

        public void ClickOrder(string orderId)
        {
           this.FindOrder(orderId);
           this.App.Pages.OrdersPages.MainPage.ClickOrder(orderId);
           this.App.Pages.OrdersPages.OrderDetails.WaitForPageLoad();

        }

        public void OpenOrderDetailsDirectly(string externalOrderId)
        {
//            externalOrderId = externalOrderId.Replace("'", "\'");
            Sql query = new Sql($"where \"ExternalOrderId\" = \"{externalOrderId}\"");
            App.Logger.Info($"Trying to get order by query = {query}");
            var order = this.App.Db.ProjectManager.GetOrders(query).First();
            this.OpenOrderDetailsDirectly(order.Id);
        }

        public void OpenOrderDetailsDirectly(int orderId)
        {
            this.App.Ui.UiHelp.NavigateToUrlRelatively($"orders/{orderId}");
        }

        public void OpenWorkpieceDetailsDirectly(int workpieceId)
        {
            this.App.Ui.UiHelp.NavigateToUrlRelatively($"orders/workpiece/{workpieceId}");
            App.Pages.OrdersPages.WorkpieceDetails.WaitForPageLoad();
        }

        public bool IsOrdersPageOpened()
        {
            return App.Pages.OrdersPages.MainPage.Opened();
        }

        #region private methods
        private void UpdateGrid(Action action, int timeoutSeconds = 10)
        {
            var elements = this.App.Pages.OrdersPages.MainPage.GetRawGridLineElements();
            action.Invoke();

            if (elements.Count > 0)
            {
                if (!this.App.Pages.Driver.WaitForElementStaleBool(elements.First(), timeoutSeconds))
                {
                    this.App.Logger.Warn("Grid data possibly wasn't updated");
                }
            }
        }

        private Order ConvertGridOrderRecordToOrder(OrderGridRecord orderRecord)
        {
            Order order = new Order();

            order.ExternalOrderId = orderRecord.OrderId;
            order.CustomerId = OrdersData.Customers[orderRecord.Customer];
            order.DeliveryDate = DateTime.Parse(orderRecord.DeliveryDate, CultureInfo.InvariantCulture);
         
            return order;
        }
        
        #endregion       
    }
}

