
namespace Tests.UI.Pages.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Core.WeDriverService;
    using Core.WeDriverService.Extensions;

    using global::Tests.TestsData.Orders.Enums;
    using global::Tests.UI.Components.General;

    using OpenQA.Selenium;

    public class OrdersCreateWorkpiecePopup : PageTemplate
    {

        private readonly By workpieceIdInputLocator = By.Name("externalWorkpieceId");

        private readonly By workpieceNameInputLocator = By.Name("name");

        private readonly By quantityInputLocator = By.Name("quantity");

        private readonly By workpieceDeliveryDateInputLocator = By.CssSelector("div[class*='datepicker'] input");

        private readonly By saveButtonLocator = By.XPath("//button[./span[.='Save']]");

        private readonly By addFilesInputLocator = By.CssSelector("input[class*='file']");

        private readonly By fileNameLocator = By.CssSelector("span.react-fine-uploader-filename");

        private readonly string deleteFileButtonMask = "//p[./span[.='{0}']]/button";

        private readonly By materialSelectorLocator = By.CssSelector("button[class*='DropDownField']");

        private CustomSelector materialSelector => new CustomSelector(Driver, materialSelectorLocator);


        private Dictionary<CreateWorkpiecePopup.WorkpieceFields, By> FieldsLocators =>
            new Dictionary<CreateWorkpiecePopup.WorkpieceFields, By>
                {
                    { CreateWorkpiecePopup.WorkpieceFields.WorkpieceId, this.workpieceIdInputLocator },
                    { CreateWorkpiecePopup.WorkpieceFields.WorkpieceName, this.workpieceNameInputLocator },
                    { CreateWorkpiecePopup.WorkpieceFields.WorkpieceQuantity, this.quantityInputLocator },
                    { CreateWorkpiecePopup.WorkpieceFields.WorkpieceDeliveryDate, this.workpieceDeliveryDateInputLocator }
                };
        

        public OrdersCreateWorkpiecePopup(IWebDriver driver)
            : base(driver)
        {
        }

        public void PopulateField(CreateWorkpiecePopup.WorkpieceFields field, string value)
        {

            if (field.Equals(CreateWorkpiecePopup.WorkpieceFields.WorkpieceMaterial))
            {
                materialSelector.SelectOption(value);
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

                el.SendKeys(Keys.Enter);
            }
        }

        public void ClickSaveButton()
        {
            this.Driver.Click(this.saveButtonLocator);
        }

        public bool IsSaveButtonEnabled()
        {
            return this.Driver.Find(this.saveButtonLocator).Enabled;
        }


        public bool IsOpened()
        {
            return Driver.Displayed(this.workpieceIdInputLocator);
        }

        public List<string> GetFieldsNames()
        {
            var workpieceFieldLabelLocator = By.CssSelector("label[class$=workpiece-form_label]");

            return Driver.Finds(workpieceFieldLabelLocator).Select(e => e.Text).ToList();
        }

        public string GetFieldPlaceholder(CreateWorkpiecePopup.WorkpieceFields field)
        {
            return this.Driver.Find(this.FieldsLocators[field]).GetAttribute("placeholder");
        }

        public string GetFieldState(CreateWorkpiecePopup.WorkpieceFields field)
        {
            return this.Driver.Find(this.FieldsLocators[field]).GetAttribute("value");           
        }

        public void AddFileForWorkpice(string filePath)
        {
            Driver.FindElement(this.addFilesInputLocator).SendKeys(filePath);
        }

        public List<string> GetFilesForWorkpiece()
        {
          

            if (Driver.Displayed(this.fileNameLocator))
            {
                var files = Driver.Finds(this.fileNameLocator)
                    .Select(e => e.Text).ToList();

                return files;              
            }

            return new List<string>();
        }

        public void DeleteFileForWorkpice(string file)
        {
            Driver.Click(By.XPath(string.Format(this.deleteFileButtonMask, file)));
        }
    }
}
