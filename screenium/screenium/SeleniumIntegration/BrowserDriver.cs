//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace screenium.SeleniumIntegration
{
    class BrowserDriver : IDisposable
    {
        private IWebDriver _driver;

        internal BrowserDriver()
        {
            CreateDriver();
        }

        internal void TestChrome()
        {
            _driver.Navigate().GoToUrl("http://www.google.com/");

            // Find the text input element by its name
            IWebElement query = _driver.FindElement(By.Name("q"));

            // Enter something to search for
            query.SendKeys("Cheese");

            // Now submit the form. WebDriver will find the form for us from the element
            query.Submit();

            // Google's search is rendered dynamically with JavaScript.
            // Wait for the page to load, timeout after 10 seconds
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until((d) => { return d.Title.ToLower().StartsWith("cheese"); });

            // Should see: "Cheese - Google Search"
            Console.WriteLine("Page title is: " + _driver.Title);
        }

        private void CreateDriver()
        {
            _driver = new ChromeDriver();
        }

        internal void OpenUrl(string url, string divSelector, string titleContains)
        {
            _driver.Navigate().GoToUrl(url);

            // Wait for the page to load, timeout after 10 seconds
            //TODO find a better way to wait ...
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until((d) => { return d.Title.ToLower().Contains(titleContains.ToLower()); });

            Outputter.Output("Page title is: " + _driver.Title);

            IWebElement query = _driver.FindElement(By.CssSelector(divSelector));

            if (!query.Displayed)
            {
                throw new InvalidOperationException("the div is not displayed: css selector: " + divSelector);
            }
        }

        internal void SaveDivImageToPath(string divSelector, string tempFilePath, int cropAdjustWidth, int cropAdjustHeight)
        {
            //get screenshot of a particular element, via cropping:
            //http://stackoverflow.com/questions/13832322/how-to-capture-the-screenshot-of-only-a-specific-element-using-selenium-webdrive
            
            // Get html element and its location and dimensions:
            var div = _driver.FindElement(By.CssSelector(divSelector));

            // Get page screenshot and save it in temp folder:
            var pageScreenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            tempFilePath = Path.GetFullPath(tempFilePath);
            var dirPath = Path.GetDirectoryName(tempFilePath);
            if (!Directory.Exists(dirPath))
            {
                throw new DirectoryNotFoundException(dirPath);
            }

            var imageFormat = ImageFormat.Png;
            pageScreenshot.SaveAsFile(tempFilePath, imageFormat);

            // Load temp image as bitmap:
            var fullWindowBitmap = new Bitmap(tempFilePath);

            // crop page screenshot to the match the div:
            var cropSize = new Size(Math.Min(fullWindowBitmap.Width - div.Location.X, div.Location.X + div.Size.Width + cropAdjustWidth),
                Math.Min(fullWindowBitmap.Height - div.Location.Y, div.Location.Y + div.Size.Height + cropAdjustHeight));

            var part = new RectangleF(div.Location.X, div.Location.Y, cropSize.Width, cropSize.Height);
            var divBitmap = fullWindowBitmap.Clone(part, fullWindowBitmap.PixelFormat);
            fullWindowBitmap.Dispose();
            divBitmap.Save(tempFilePath, imageFormat);
        }

        public void Dispose()
        {
            if (_driver != null)
            {
                //Close the browser
                _driver.Quit();
                _driver.Dispose();
                _driver = null;
            }
        }

        internal void SetWindowSize(Size size)
        {
            _driver.Manage().Window.Size = size;
        }
    }
}
