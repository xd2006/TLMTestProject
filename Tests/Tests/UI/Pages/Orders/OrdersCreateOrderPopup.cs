
namespace Tests.UI.Pages.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Core.Service;
    using Core.WeDriverService;
    using Core.WeDriverService.Extensions;

    using global::Tests.TestsData.Orders.Enums;

    using Microsoft.SqlServer.Server;

    using NUnit.Framework.Constraints;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public class OrdersCreateOrderPopup : PageTemplate
    {
        public OrdersCreateOrderPopup(IWebDriver driver)
            : base(driver)
        {
        }

        #region locators

        private readonly By customerSelectLocator = By.Name("customerId");

        private readonly By orderIdSelectorLocator = By.Name("externalOrderId");

        private readonly By orderDeliveryDateInputLocator = By.CssSelector("input[name='deliveryDate'][class*='order-info']");

        private readonly By commentsInputLocator = By.Name("comments");

        private readonly By workpieceIdInputLocator = By.Name("externalWorkpieceId");

        private readonly By workpieceNameInputLocator = By.Name("name");

        private readonly By quantityInputLocator = By.Name("quantity");

        private readonly By workpieceDeliveryDateInputLocator = By.CssSelector("input[name='deliveryDate'][class*='new-workpiece']");

        private readonly By titleLocator = By.CssSelector("div[class$='modal-header_title']");

        private readonly By closeButtonLocator = By.CssSelector("button[class$='close']");

        private readonly By saveButtonLocator = By.XPath("//button[./span[.='Save']]");

        private readonly By addWorkpieceButtonLocator = By.CssSelector("button[class*='new-workpiece']");

        private readonly string addFilesButtonMask =
            "//table[contains(@class, 'workpiece-list_root')]/tbody/tr[./td[.='{0}']]//input[contains(@class, 'file')]";

        private readonly string workpieceFilesMask =
            "//table[contains(@class, 'workpiece-list_root')]/tbody/tr[./td[.='{0}']]/following-sibling::tr[contains(@class,'row_files')]//p";

        #endregion

        private Dictionary<CreateOrderPopup.OrderFields, By> FieldsLocators =>
            new Dictionary<CreateOrderPopup.OrderFields, By>
                {
                    {
                        CreateOrderPopup.OrderFields
                            .Customer,
                        this.customerSelectLocator
                    },
                    { CreateOrderPopup.OrderFields.OrderId, this.orderIdSelectorLocator },
                    { CreateOrderPopup.OrderFields.OrderDeliveryDate, this.orderDeliveryDateInputLocator},
                    { CreateOrderPopup.OrderFields.Comments, this.commentsInputLocator },
                    { CreateOrderPopup.OrderFields.WorkpieceId, this.workpieceIdInputLocator },
                    { CreateOrderPopup.OrderFields.WorkpieceName, this.workpieceNameInputLocator },
                    { CreateOrderPopup.OrderFields.WorkpieceQuantity, this.quantityInputLocator},
                    { CreateOrderPopup.OrderFields.WorkpieceDeliveryDate, this.workpieceDeliveryDateInputLocator }
                };


        #region selects

        private SelectElement CustomerSelector => new SelectElement(this.Driver.Find(this.customerSelectLocator));

        #endregion
        
        public List<string> GetSectionFieldsTitles(string sectionName)
        {
            var sectionLocator = By.XPath($"//p[contains(@class, 'sectionTitle') and .='{sectionName}']/following-sibling::form");

            var section = this.Driver.Find(sectionLocator);
            return section.FindElements(By.CssSelector("label")).Select(t => t.Text).ToList();
        }

        public string GetFieldState(CreateOrderPopup.OrderFields field)
        {
           
            if (field.Equals(CreateOrderPopup.OrderFields.Customer))
           {
               return this.CustomerSelector.SelectedOption.Text;
           }

            return this.Driver.Find(this.FieldsLocators[field]).GetAttribute("value");
        }

        public string GetFieldPlaceholder(CreateOrderPopup.OrderFields field)
        {
            return this.Driver.Find(this.FieldsLocators[field]).GetAttribute("placeholder");
        }

        public bool Opened()
        {
            return this.Driver.Displayed(this.titleLocator);
        }

        #region waits
        public void WaitForPageLoad(int timeout = 30)
        {
            this.Driver.WaitForElement(this.titleLocator, timeout);
        }

        public void WaitForPageClosed(int timeout = 30)
        {
            var pageClosed = this.Driver.WaitForElementNotPresent(this.titleLocator, timeout);

            if (!pageClosed)
            {
                throw new Exception("Can't close Create order popup");
            }
        }
        #endregion

        public void Close()
        {
            var closebuttonElement = this.Driver.Find(this.closeButtonLocator);
            Thread.Sleep(500); //To avoid 'Obscured' exception in Edge
            closebuttonElement.Click();
        }

        public void PopulateField(CreateOrderPopup.OrderFields field, string value)
        {
           
                if (field.Equals(CreateOrderPopup.OrderFields.Customer))
                {
                    if (value != null)
                    {
                        this.CustomerSelector.SelectByText(value);
                    }
                    else
                    {
                        this.CustomerSelector.SelectByText("");
                    }
                }
                else
                {
                    var el = this.Driver.Find(this.FieldsLocators[field]);

                    // el.Clear is not used since validation doesn't work with this method
                    el.SendKeys(Keys.Control + "a");
                    el.SendKeys(Keys.Delete);
                if (value != null)
                    {
                        el.SendKeys(value);
                    }
                }
            }
        
        public void ClickSaveButton()
        {
           this.Driver.Find(this.saveButtonLocator).Click();
        }

        public void ClickAddWorkpieceButton()
        {
            this.Driver.Find(this.addWorkpieceButtonLocator).Click();
        }

        public bool IsSaveButtonEnabled()
        {
            return this.Driver.Find(this.saveButtonLocator).Enabled;
        }

        public bool IsAddWorkpieceButtonEnabled()
        {
            return this.Driver.Find(this.addWorkpieceButtonLocator).Enabled;
        }

        public List<string> GetOrderFieldsTitles()
        {
            var orderFieldLabelLocator = By.CssSelector("form[class*='order-info'] label");

            return Driver.Finds(orderFieldLabelLocator).Select(e => e.Text).ToList();
        }

        public void AddFileForWorkpice(string workPieceId, string filePath)
        {
           Driver.FindElement(By.XPath(string.Format(this.addFilesButtonMask, workPieceId))).SendKeys(filePath);
        }

        public List<string> GetFilesForWorkpiece(string externalWorkpieceId)
        {
            var files = Driver.Finds(By.XPath(string.Format(this.workpieceFilesMask, externalWorkpieceId)))
                .Select(e => e.Text).ToList();
            List<string> results = new List<string>();
            foreach (var file in files)
            {
                var texts = file.Split(new[] { "\r\n" }, StringSplitOptions.None);
                results.Add(texts[0]);
            }

            return results;
        }
    }
}
