

namespace Tests.UI.Components.Interfaces
{
    using System.Collections.Generic;

    using OpenQA.Selenium;

    public interface IGrid<T>
    {
        List<T> GetRecords();

        /// <summary>
        /// To check grid update
        /// </summary>
        /// <returns>line element</returns>
        IReadOnlyCollection<IWebElement> GetGridLineElements();

        void ClickRecord(string recordName);

        List<string> GetColumnsNames();
    }
}
