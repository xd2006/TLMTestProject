
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
    using global::Tests.UI.Components.General;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public class OrdersCreateOrderPopup : PageTemplate
    {
        public OrdersCreateOrderPopup(IWebDriver driver)
            : base(driver)
        {
        }

        #region locators

        private readonly By customerSelectLocator = By.CssSelector("button[class*='DropDownField']");

        private readonly By orderIdInputLocator = By.Name("externalOrderId");

        private readonly By orderDeliveryDateInputLocator =
            By.CssSelector("div[class*='datepicker'] input");

        private readonly By commentsInputLocator = By.Name("comments");

        private readonly By titleLocator = By.CssSelector("div[class$='modal-header_title']");

        private readonly By closeButtonLocator = By.CssSelector("button[class$='close']");

        private readonly By saveButtonLocator = By.XPath("//button[./span[.='Save']]");
       
        #endregion

        private Dictionary<CreateOrderPopup.OrderFields, By> FieldsLocators =>
            new Dictionary<CreateOrderPopup.OrderFields, By>
                {
                    {
                        CreateOrderPopup.OrderFields.Customer,
                        this.customerSelectLocator
                    },
                    {
                        CreateOrderPopup.OrderFields.OrderId,
                        this.orderIdInputLocator
                    },
                    {
                        CreateOrderPopup.OrderFields.OrderDeliveryDate,
                        this.orderDeliveryDateInputLocator
                    },
                    {
                        CreateOrderPopup.OrderFields.Comments,
                        this.commentsInputLocator
                    },
                };


        #region selects

        private CustomSelector CustomerSelector => new CustomSelector(Driver, customerSelectLocator);

        #endregion

        public List<string> GetSectionFieldsTitles(string sectionName)
        {
            var sectionLocator = By.XPath(
                $"//p[contains(@class, 'sectionTitle') and .='{sectionName}']/following-sibling::form");

            var section = this.Driver.Find(sectionLocator);
            return section.FindElements(By.CssSelector("label")).Select(t => t.Text).ToList();
        }

        public string GetFieldState(CreateOrderPopup.OrderFields field)
        {

            if (field.Equals(CreateOrderPopup.OrderFields.Customer))
            {
                return this.CustomerSelector.SelectedOption();
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
                    this.CustomerSelector.SelectOption(value);
                }
                else
                {
                    this.CustomerSelector.SelectOption("");
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



        public bool IsSaveButtonEnabled()
        {
            return this.Driver.Find(this.saveButtonLocator).Enabled;
        }



        public List<string> GetOrderFieldsTitles()
        {
            var orderFieldLabelLocator = By.CssSelector("form[class*='order-form'] label");

            return Driver.Finds(orderFieldLabelLocator).Select(e => e.Text).ToList();
        }
        
       

       

        public void WaitForpageLoad()
        {
            Driver.WaitForElementToBeClickable(this.saveButtonLocator);
        }
    }
}
