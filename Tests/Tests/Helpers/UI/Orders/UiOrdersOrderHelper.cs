namespace Tests.Helpers.UI.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Core.Service;

    using global::Tests.Managers;
    using global::Tests.Models.ProjectManager.DbModels.Postgres;
    using global::Tests.Models.ProjectManager.UiModels;
    using global::Tests.TestsData.Orders;
    using global::Tests.TestsData.Orders.Enums;

    public class UiOrdersOrderHelper : UiCommonHelper
    {
        public UiOrdersOrderHelper(ApplicationManager app)
            : base(app)
        {
        }

        public void PopulateField(CreateOrderPopup.OrderFields field, string value)
        {
            this.App.Pages.OrdersPages.CreateOrderPopup.PopulateField(field, value);
        }

        public void PopulateOrderData(Order order)
        {
            this.PopulateField(
                CreateOrderPopup.OrderFields.Customer,
                OrdersData.Customers.FirstOrDefault(k => k.Value.Equals(order.CustomerId)).Key);
            this.PopulateField(CreateOrderPopup.OrderFields.OrderId, order.ExternalOrderId);

            string deliveryDate = order.DeliveryDate.ToString("yyyy-MM-dd").Equals("0001-01-01")
                                      ? string.Empty
                                      : order.DeliveryDate.ToString("yyyy-MM-dd");        
             this.PopulateField(
                    CreateOrderPopup.OrderFields.OrderDeliveryDate,
                    deliveryDate);
            

            this.PopulateField(CreateOrderPopup.OrderFields.Comments, order.Comments);
        }

        public void AddFilesForWorkpiece(string workpieceId, List<string> files)
        {
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    App.Pages.OrdersPages.CreateOrderPopup.AddFileForWorkpice(workpieceId, file);
                }
            }
        }
       

        public void AddWorkpieceToOrder(Workpiece workpiece, bool populateOnly = false)
        {
            this.PopulateField(CreateOrderPopup.OrderFields.WorkpieceId, workpiece.ExternalWorkpieceId);
            this.PopulateField(CreateOrderPopup.OrderFields.WorkpieceName, workpiece.Name);
            this.PopulateField(CreateOrderPopup.OrderFields.WorkpieceQuantity, workpiece.Quantity.ToString());

            string deliveryDate = workpiece.DeliveryDate.ToString("yyyy-MM-dd").Equals("0001-01-01")
                                      ? string.Empty
                                      : workpiece.DeliveryDate.ToString("yyyy-MM-dd");
            this.PopulateField(CreateOrderPopup.OrderFields.WorkpieceDeliveryDate, deliveryDate);
          

            if (!populateOnly)
            {
                int counter = 0;
                while (!App.Pages.OrdersPages.CreateOrderPopup.IsAddWorkpieceButtonEnabled() && counter++ < 5)
                {
                    Thread.Sleep(500);
                }

                if (counter >= 5)
                {
                    throw new Exception("'Add workpiece' button is disabled");
                }

                this.ClickAddWorkpieceButton();
            }
        }

        public void ClickAddWorkpieceButton()
        {
            this.App.Pages.OrdersPages.CreateOrderPopup.ClickAddWorkpieceButton();
            Thread.Sleep(500); // sleep because of Edge. Sometimes Add button doesn't work here.
        }

        public void ClickSaveButton(bool checkPopupClosed = true)
        {
            this.App.Pages.OrdersPages.CreateOrderPopup.ClickSaveButton();

            if (checkPopupClosed)
            {
                int i = 0;
                while (this.App.Pages.OrdersPages.CreateOrderPopup.Opened() && i++ < 20)
                {
                    Thread.Sleep(500);
                }

                if (i == 20)
                {
                    throw new Exception("Order creation popup can't be closed by clicking 'Save' button");
                }
            }
        }

        public List<string> GetSectionFieldsNames(string sectionName)
        {
            var titles = this.App.Pages.OrdersPages.CreateOrderPopup.GetSectionFieldsTitles(sectionName);

            this.App.Logger.Info($"Title for section '{sectionName}' were found: {Environment.NewLine}{ServiceMethods.ListToString(titles)}");

            return titles;
        }

        public List<string> GetOrderFieldsNames()
        {
            return App.Pages.OrdersPages.CreateOrderPopup.GetOrderFieldsTitles();
        }

        public Dictionary<CreateOrderPopup.OrderFields, string> GetCreateOrderFieldsStates()
        {
            List<CreateOrderPopup.OrderFields> fields =
                new List<CreateOrderPopup.OrderFields>
                    {
                        CreateOrderPopup.OrderFields.Comments,
                        CreateOrderPopup.OrderFields.Customer,
                        CreateOrderPopup.OrderFields.OrderDeliveryDate,
                        CreateOrderPopup.OrderFields.OrderId,
                        CreateOrderPopup.OrderFields.WorkpieceQuantity,
                        CreateOrderPopup.OrderFields
                            .WorkpieceDeliveryDate,
                        CreateOrderPopup.OrderFields.WorkpieceName,
                        CreateOrderPopup.OrderFields.WorkpieceId
                    };
            Dictionary<CreateOrderPopup.OrderFields, string> states =
                new Dictionary<CreateOrderPopup.OrderFields, string>();

            foreach (var field in fields)
            {
                var state = this.App.Pages.OrdersPages.CreateOrderPopup.GetFieldState(field);
                states.Add(field, state);
            }

            return states;
        }

        public Dictionary<CreateOrderPopup.OrderFields, string> GetCreateOrderFieldsPlaceholders()
        {
            List<CreateOrderPopup.OrderFields> fields =
                new List<CreateOrderPopup.OrderFields>
                    {
                        CreateOrderPopup.OrderFields.OrderDeliveryDate,
                        CreateOrderPopup.OrderFields
                            .WorkpieceDeliveryDate,
                    };

            Dictionary<CreateOrderPopup.OrderFields, string> placeholders =
                new Dictionary<CreateOrderPopup.OrderFields, string>();

            foreach (var field in fields)
            {
                var fieldPlaceholder = this.App.Pages.OrdersPages.CreateOrderPopup.GetFieldPlaceholder(field);
                placeholders.Add(field, fieldPlaceholder);
            }

            return placeholders;
        }

        public void CloseCreateOrderPopup()
        {
            this.App.Pages.OrdersPages.CreateOrderPopup.Close();

            App.Pages.OrdersPages.CreateOrderPopup.WaitForPageClosed();
        }

        public bool IsSaveButtonEnabled()
        {
            return this.App.Pages.OrdersPages.CreateOrderPopup.IsSaveButtonEnabled();
        }

        public bool IsCreateOrderPopUpOpened()
        {
           return this.App.Pages.OrdersPages.CreateOrderPopup.Opened();
        }

        public OrderGridRecord GetInfoFromOrderDetailsPage()
        {
            OrderGridRecord order = new OrderGridRecord();

            int i = 0;
            do
            {
                order.OrderId = this.App.Pages.OrdersPages.OrderDetails.GetOrderDetailsContent("Order ID");
                if (order.OrderId == string.Empty)
                {
                    Thread.Sleep(500);
                }
            }
            while (order.OrderId == string.Empty && i++ < 10);

            order.Customer = this.App.Pages.OrdersPages.OrderDetails.GetOrderDetailsContent("Customer");
            order.DeliveryDate = this.App.Pages.OrdersPages.OrderDetails.GetOrderDetailsContent("Delivery date");
            order.Editor = this.App.Pages.OrdersPages.OrderDetails.GetOrderDetailsContent("Editor");
            order.CreationDate = this.App.Pages.OrdersPages.OrderDetails.GetOrderDetailsContent("Created at").Split(' ')[0];
           
            return order;

        }

        public List<string> GetWorkpiecesTableTitles()
        {
            return this.App.Pages.OrdersPages.OrderDetails.GetWorkpiecesTableTitles();
        }

        public List<WorkpieceGridRecord> GetWorkpieceGridRecords()
        {
            return this.App.Pages.OrdersPages.OrderDetails.GetWorkpieceGridRecords();
        }

        public void ClickWorkpiece(string workpieceId)
        {
            this.App.Pages.OrdersPages.OrderDetails.ClickWorkpieceRecord(workpieceId);

        }

        public void ClickBackLink()
        {
            this.App.Pages.OrdersPages.OrderDetails.ClickBackLink();
            this.App.Pages.OrdersPages.MainPage.WaitForPageLoad();
        }

        public bool IsAddWorkpieceButtonEnabled()
        {
            return this.App.Pages.OrdersPages.CreateOrderPopup.IsAddWorkpieceButtonEnabled();
        }

        public List<string> GetFilesForWorkpiece(string externalWorkpieceId)
        {
            return App.Pages.OrdersPages.CreateOrderPopup.GetFilesForWorkpiece(externalWorkpieceId);
        }
    }
}
