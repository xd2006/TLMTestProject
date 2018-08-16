
namespace Tests.Managers.AuxPageManagers
{
    using System.Collections.Generic;

    using global::Tests.Managers.AuxPageManagers.Templates;
    using global::Tests.UI.Pages.Orders;

    using OpenQA.Selenium;

    public class OrdersPages : PagesManagerTemplate
    {
        private OrdersMainPage ordersMainPage;

        private OrdersCreateOrderPopup ordersCreateOrderPopup;

        private OrdersOrderDetailsPage ordersOrderDetailsPage;

        private OrdersWorkpieceDetailsPage ordersWorkpieceDetailsPage;

        private OrdersWorkpieceTaskRequiredToolsPage ordersWorkpieceTaskRequiredToolsPage;

        private OrdersCreateTaskPopup ordersCreateTaskPopup;

        private OrdersCreateWorkpiecePopup ordersCreateWorkpiecePopup;

        public OrdersPages(IWebDriver driver)
            : base(driver)
        {
        }

        public OrdersMainPage MainPage => this.ordersMainPage ?? (this.ordersMainPage = new OrdersMainPage(this.Driver));

        public OrdersCreateOrderPopup CreateOrderPopup =>
            this.ordersCreateOrderPopup
            ?? (this.ordersCreateOrderPopup = new OrdersCreateOrderPopup(this.Driver));

        public OrdersOrderDetailsPage OrderDetails =>
            this.ordersOrderDetailsPage
            ?? (this.ordersOrderDetailsPage = new OrdersOrderDetailsPage(this.Driver));

        public OrdersWorkpieceDetailsPage WorkpieceDetails =>
            this.ordersWorkpieceDetailsPage
            ?? (this.ordersWorkpieceDetailsPage = new OrdersWorkpieceDetailsPage(this.Driver));

        public OrdersWorkpieceTaskRequiredToolsPage TaskRequiredToolsPage => 
             this.ordersWorkpieceTaskRequiredToolsPage ??
            (this.ordersWorkpieceTaskRequiredToolsPage = new OrdersWorkpieceTaskRequiredToolsPage(Driver));

        public OrdersCreateTaskPopup CreateTaskPopup =>
            this.ordersCreateTaskPopup ??
            (this.ordersCreateTaskPopup = new OrdersCreateTaskPopup(Driver));
        
        public OrdersCreateWorkpiecePopup CreateWorkpiecePopup =>
            this.ordersCreateWorkpiecePopup ??
            (this.ordersCreateWorkpiecePopup = new OrdersCreateWorkpiecePopup(Driver));        
    }
}
