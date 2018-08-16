namespace Tests.Helpers.UI.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Core.Service;
    using Core.WeDriverService.Extensions;

    using global::Tests.Managers;
    using global::Tests.Models.ProjectManager.DbModels.Postgres;
    using global::Tests.Models.ProjectManager.UiModels;
    using global::Tests.TestsData.Orders;
    using global::Tests.TestsData.Orders.Enums;

    using NUnit.Framework.Constraints;

    using OpenQA.Selenium;

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

        public void PopulateOrderData(Order order, bool populateOnly = false)
        {
            this.PopulateField(
                CreateOrderPopup.OrderFields.Customer,
                OrdersData.Customers.FirstOrDefault(k => k.Value.Equals(order.CustomerId)).Key);
            this.PopulateField(CreateOrderPopup.OrderFields.OrderId, order.ExternalOrderId);

            string deliveryDate = order.DeliveryDate.ToString("yyyy-MM-dd").Equals("0001-01-01")
                                      ? string.Empty
                                      : order.DeliveryDate.ToString("MM/dd/yyyy");        
             this.PopulateField(
                    CreateOrderPopup.OrderFields.OrderDeliveryDate,
                    deliveryDate);
            

            this.PopulateField(CreateOrderPopup.OrderFields.Comments, order.Comments);

            App.Pages.Driver.WaitForPageReady();            
            if (!populateOnly)
            {
                App.Pages.OrdersPages.CreateOrderPopup.ClickSaveButton();
            }
        }
        

        public void AddWorkpieceToOrder(Workpiece workpiece, bool populateOnly = false)
        {
            var materials = App.GraphApi.ToolManager.GetUsageMaterials();
            var material = materials.First(m => int.Parse(m.Id).Equals(workpiece.RawMaterialId));

            if (!App.Pages.OrdersPages.CreateWorkpiecePopup.IsOpened())
            {
                App.Pages.OrdersPages.OrderDetails.ClickCreateNewWorkpieceButton();
            }

            App.Pages.OrdersPages.CreateWorkpiecePopup.PopulateField(CreateWorkpiecePopup.WorkpieceFields.WorkpieceId, workpiece.ExternalWorkpieceId);
            App.Pages.OrdersPages.CreateWorkpiecePopup.PopulateField(CreateWorkpiecePopup.WorkpieceFields.WorkpieceName, workpiece.Name);
            string deliveryDate = workpiece.DeliveryDate.ToString("yyyy-MM-dd").Equals("0001-01-01")
                                      ? string.Empty
                                      : workpiece.DeliveryDate.ToString("MM/dd/yyy");
            App.Pages.OrdersPages.CreateWorkpiecePopup.PopulateField(CreateWorkpiecePopup.WorkpieceFields.WorkpieceDeliveryDate, deliveryDate);
            App.Pages.OrdersPages.CreateWorkpiecePopup.PopulateField(
                CreateWorkpiecePopup.WorkpieceFields.WorkpieceMaterial,
                material.Name);
            App.Pages.OrdersPages.CreateWorkpiecePopup.PopulateField(CreateWorkpiecePopup.WorkpieceFields.WorkpieceQuantity, workpiece.Quantity.ToString());



            if (!populateOnly)
            {
                int counter = 0;
                while (!App.Pages.OrdersPages.CreateWorkpiecePopup.IsSaveButtonEnabled() && counter++ < 5)
                {
                    Thread.Sleep(500);
                }

                if (counter >= 5)
                {
                    throw new Exception("'Add workpiece' button is disabled");
                }

                App.Pages.OrdersPages.CreateWorkpiecePopup.ClickSaveButton();
                ServiceMethods.WaitForOperationPositive(() => !App.Pages.OrdersPages.CreateWorkpiecePopup.IsOpened(), 20);
            }
        }

        public void ClickAddWorkpieceButton()
        {
            this.App.Pages.OrdersPages.OrderDetails.ClickCreateNewWorkpieceButton();
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
                        CreateOrderPopup.OrderFields.OrderId
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

        public List<WorkpieceGridRecord> GetWorkpieceGridRecords(bool shouldFind = false, int timeOutSec = 10)
        {
            List<WorkpieceGridRecord> elements = new List<WorkpieceGridRecord>();
            int count = 0;
            int c = 0;
            do
            {
                App.Pages.OrdersPages.OrderDetails.WaitForPageLoad();
                elements = this.App.Pages.OrdersPages.OrderDetails.GetWorkpieceGridRecords();
                count = elements.Count;
                if (!shouldFind || count > 0)
                {
                    return elements;
                }
            }
            while (count == 0 && c++ <= timeOutSec);

            if (!shouldFind)
            {
                return elements;
            }
            else
            {
                throw new Exception("Can't load workpieces");
            }
        }

        public void ClickWorkpiece(string workpieceId)
        {
            this.App.Pages.OrdersPages.OrderDetails.ClickWorkpieceRecord(workpieceId);
        }

        public void ClickRandomWorkpiece()
        {
            var elements = GetWorkpieceGridRecords(true, 20);
            var index = new Random().Next(0, elements.Count);
            this.App.Pages.OrdersPages.OrderDetails.ClickWorkpieceRecord(index);
        }

        public void ClickBackLink()
        {
            this.App.Pages.OrdersPages.OrderDetails.ClickBackLink();
            this.App.Pages.OrdersPages.MainPage.WaitForPageLoad();
        }
        
        public List<string> GetFilesForWorkpiece()
        {
            return App.Pages.OrdersPages.CreateWorkpiecePopup.GetFilesForWorkpiece();
        }

        public bool SaveButtonShouldBeDisabled()
        {
            try
            {
                ServiceMethods.WaitForOperationNegative(IsSaveButtonEnabled);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
