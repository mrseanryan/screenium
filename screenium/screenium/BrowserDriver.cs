//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Drawing;
using System.IO;

namespace screenium
{
    class BrowserDriver : IDisposable
    {
        private IWebDriver driver;

        internal BrowserDriver()
        {
            CreateDriver();
        }

        internal void TestChrome()
        {
            driver.Navigate().GoToUrl("http://www.google.com/");

            // Find the text input element by its name
            IWebElement query = driver.FindElement(By.Name("q"));

            // Enter something to search for
            query.SendKeys("Cheese");

            // Now submit the form. WebDriver will find the form for us from the element
            query.Submit();

            // Google's search is rendered dynamically with JavaScript.
            // Wait for the page to load, timeout after 10 seconds
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until((d) => { return d.Title.ToLower().StartsWith("cheese"); });

            // Should see: "Cheese - Google Search"
            Console.WriteLine("Page title is: " + driver.Title);
        }

        private void CreateDriver()
        {
            driver = new ChromeDriver();
        }

        internal void OpenUrl(string url, string divSelector, string titleContains)
        {
            driver.Navigate().GoToUrl(url);

            // Wait for the page to load, timeout after 10 seconds
            //TODO find a better way to wait ...
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until((d) => { return d.Title.ToLower().Contains(titleContains.ToLower()); });

            Outputter.Output("Page title is: " + driver.Title);

            IWebElement query = driver.FindElement(By.CssSelector(divSelector));

            if (!query.Displayed)
            {
                throw new InvalidOperationException("the div is not displayed: css selector: " + divSelector);
            }
        }

        internal void SaveDivImageToPath(string divSelector, string tempFilePath)
        {
            //get screenshot of a particular element, via cropping:
            //http://stackoverflow.com/questions/13832322/how-to-capture-the-screenshot-of-only-a-specific-element-using-selenium-webdrive
            
            // Get html element and its location and dimensions:
            IWebElement div = driver.FindElement(By.CssSelector(divSelector));
            Point location = div.Location;
            Size logPlotSize = div.Size;

            // Get page screenshot and save it in temp folder:
            Screenshot pageScreenshot = ((ITakesScreenshot)driver).GetScreenshot();
            tempFilePath = Path.GetFullPath(tempFilePath);
            var dirPath = Path.GetDirectoryName(tempFilePath);
            if (!Directory.Exists(dirPath))
            {
                throw new DirectoryNotFoundException(dirPath);
            }

            pageScreenshot.SaveAsFile(tempFilePath, System.Drawing.Imaging.ImageFormat.Png);

            // Load temp image as bitmap:
            Bitmap tempBitmap = new Bitmap(tempFilePath);

            // crop div page screenshot to the match the div:
            RectangleF part = new RectangleF(location.X, location.Y, logPlotSize.Width, logPlotSize.Height);
            Bitmap logPlotBitmap = tempBitmap.Clone(part, tempBitmap.PixelFormat);
            tempBitmap.Dispose();
            logPlotBitmap.Save(tempFilePath, System.Drawing.Imaging.ImageFormat.Png);
        }

        public void Dispose()
        {
            if (driver != null)
            {
                //Close the browser
                driver.Quit();
                driver.Dispose();
                driver = null;
            }
        }
    }
}
