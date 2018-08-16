using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AngleSharp.Parser.Html;
using Core.Service;
using Core.WeDriverService.Extensions;
using OpenQA.Selenium;
using Tests.Models.ProjectManager.UiModels;
using Tests.UI.Components.Interfaces;

namespace Tests.UI.Components.Orders
{
    public class FilesGrid : GridTemplate, IGrid<FilesGridRecord>
    {
        public FilesGrid(IWebDriver driver)
            : base(driver)
        {
        }

        public List<FilesGridRecord> GetRecords()
        {
            var entities = new List<FilesGridRecord>();
            Driver.WaitForElements(GridRowLocator);
           
            var parser = new HtmlParser();  
            var doc = parser.Parse(Driver.PageSource);

            var rows = doc.QuerySelectorAll(GridRowCssLocatorText);

            try
            {
                foreach (var row in rows)
                {
                    var files = new FilesGridRecord();
                    var cells = row.QuerySelectorAll(GridCellCssSelector);
                    var cellsTexts = cells.Select(t => t.TextContent).ToList();
                    files.Name = cellsTexts[0];
                    files.Editor = cellsTexts[1];
                    files.CreationDate = cellsTexts[2];
                    if (Parameters.Parameters.Browser == "MicrosoftEdge")
                    {
                        files.CreationDate = ServiceMethods.ConvertEdgeDateToUtf(files.CreationDate);
                    }
                    entities.Add(files);
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                return entities;
            }         
            return entities;
        }
    }
}