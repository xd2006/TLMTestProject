
namespace Tests.UI.Pages.Orders
{
    using System.Collections.Generic;

    using Core.WeDriverService;
    using Core.WeDriverService.Extensions;

    using global::Tests.TestsData.Orders.Enums;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public class OrdersCreateTaskPopup : PageTemplate
    {
        public OrdersCreateTaskPopup(IWebDriver driver)
            : base(driver)
        {
        }

        private readonly By nameInputLocator = By.CssSelector("input[name='name']");

        private readonly By durationPerWorkpieceInputLocator = By.CssSelector("input[name='durationPerWorkpiece']");

        private readonly By durationInTotalInputLocator = By.CssSelector("input[name='durationPerTotal']");

        private readonly By startInputLocator = By.CssSelector("input[name='startDate']");

        private readonly By endInputLocator = By.CssSelector("input[name='endDate']");

        private readonly By machineSelectLocator = By.CssSelector("select[name='machineId']");

        private readonly By addButtonLocator = By.CssSelector("button[class*='new-task']");

        private readonly By closeButtonLocator = By.CssSelector("button[class*='close']");

        private Dictionary<WorkpieceDetails.TaskFields, By> FieldsLocators =>
            new Dictionary<WorkpieceDetails.TaskFields, By>
                {
                    { WorkpieceDetails.TaskFields.Name, this.nameInputLocator },
                    { WorkpieceDetails.TaskFields.DurationPerWorkpiece, this.durationPerWorkpieceInputLocator},
                    { WorkpieceDetails.TaskFields.DurationInTotal, this.durationInTotalInputLocator },
                    { WorkpieceDetails.TaskFields.Start, this.startInputLocator },
                    { WorkpieceDetails.TaskFields.End, this.endInputLocator },
                    { WorkpieceDetails.TaskFields.Machine, this.machineSelectLocator },
                };

        #region selects

        private SelectElement MachineSelector => new SelectElement(this.Driver.Find(this.machineSelectLocator));

        #endregion


        public void PopulateField(WorkpieceDetails.TaskFields field, string value)
        {
            if (field.Equals(WorkpieceDetails.TaskFields.Machine))
            {
                this.MachineSelector.SelectByText(value);
            }
            else
            {
                this.Driver.Find(this.FieldsLocators[field]).SendKeys(value);
            }
        }


        public string GetFieldState(WorkpieceDetails.TaskFields field)
        {
            if (field.Equals(WorkpieceDetails.TaskFields.Machine))
            {
                return this.MachineSelector.SelectedOption.Text;
            }

            return this.Driver.Find(this.FieldsLocators[field]).GetAttribute("value");
        }

        public string GetFieldPlaceholder(WorkpieceDetails.TaskFields field)
        {
            return this.Driver.Find(this.FieldsLocators[field]).GetAttribute("placeholder") ?? string.Empty;
        }

        public bool IsOpened()
        {
            return Driver.Displayed(this.addButtonLocator);         
        }

        public void ClickAddTaskButton()
        {           
            Driver.Click(this.addButtonLocator);
        }

        public void Close()
        {
            Driver.Click(this.closeButtonLocator);
        }

        public void WaitForPageLoad()
        {
            Driver.WaitForElementToBeClickable(this.nameInputLocator);
        }
    }
}
