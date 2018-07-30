using System;
using System.IO;
using OpenQA.Selenium;

namespace Core.WeDriverService
{
    public class Screenshoter
    {
        private IWebDriver _driver;

        public Screenshoter(IWebDriver driver)
        {
            _driver = driver;
        }

        public string TakeScreenshot(string scenarioTitle, string logFolder)
        {
            try
            {
                string fileNameBase = $"{scenarioTitle}_{DateTime.Now:yyyy-MM-dd_HHmmss}";
                string folderNameBase = $"{scenarioTitle}_{DateTime.Now:yyyy-MM-dd_HHmm}";

                string artifactDirectory = Path.GetFullPath(Path.Combine(logFolder, folderNameBase));

                if (!Directory.Exists(artifactDirectory))
                {
                    Directory.CreateDirectory(artifactDirectory);
                }

                lock (this._driver)
                {
                    if (this._driver is ITakesScreenshot takesScreenshot)
                    {
                        var screenshot = takesScreenshot.GetScreenshot();

                        string screenshotFilePath = Path.Combine(artifactDirectory, fileNameBase + "_screenshot.png");

                        screenshot.SaveAsFile(screenshotFilePath, ScreenshotImageFormat.Png);

                        return screenshotFilePath;
                    }
                }

                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
     }
}
