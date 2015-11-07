//original license: MIT
//
//See the file license.txt for copying permission.

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;

namespace screenium.SeleniumIntegration
{
    class BrowserDriver : IDisposable
    {
        private IWebDriver _driver;
        private readonly ImageFormat _imageFormat = ImageFormat.Png;

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
            wait.Until(d => d.Title.ToLower().StartsWith("cheese"));

            // Should see: "Cheese - Google Search"
            Console.WriteLine("Page title is: " + _driver.Title);

            //save a screenshot, to make sure ImageMagick is working:
            if (!TestSaveScreenshot())
            {
                throw new InvalidOperationException("The screenshot could not be created or could not be saved.");
            }
        }

        private bool TestSaveScreenshot()
        {
            var tempFilePath = Path.GetTempFileName();
            SaveDivImageToPath("", tempFilePath, 0, 0, new TimeSpan());
            FileInfo info = new FileInfo(tempFilePath);
            return info.Length > 0;
        }

        private void CreateDriver()
        {
            _driver = new ChromeDriver();
            Outputter.OutputSeparator();
        }

        internal void OpenUrl(string url, string divSelector, string titleContains)
        {
            _driver.Navigate().GoToUrl(url);

            // Wait for the page to load, timeout after 10 seconds
            //TODO selenium: find a better way to wait ...
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            Outputter.Output("Searching for title text: " + titleContains);
            wait.Until(d => d.Title.ToLower().Contains(titleContains.ToLower()));

            Outputter.Output("Page title is: " + _driver.Title);

            IWebElement query;
            try
            {
                query = _driver.FindElement(By.CssSelector(divSelector));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Could not find the div with CSS selector: " + divSelector, ex);
            }
            if (!query.Displayed)
            {
                throw new InvalidOperationException("The div is not displayed: css selector: " + divSelector);
            }
        }

        internal void SaveDivImageToPath(string divSelector, string imageFilePath, int cropAdjustWidth, int cropAdjustHeight, TimeSpan sleepTimeSpan)
        {
            SleepBeforeScreenshot(sleepTimeSpan);

            SaveScreenshotOfFullPage(ref imageFilePath);

            if (!string.IsNullOrWhiteSpace(divSelector))
            {
                CropScreenshotToMatchDiv(divSelector, imageFilePath, cropAdjustWidth, cropAdjustHeight);
            }
            Outputter.Output("Got screenshot.");
        }

        private void SaveScreenshotOfFullPage(ref string imageFilePath)
        {
            var pageScreenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            imageFilePath = Path.GetFullPath(imageFilePath);
            var dirPath = Path.GetDirectoryName(imageFilePath);
            if (dirPath == null || !Directory.Exists(dirPath))
            {
                throw new DirectoryNotFoundException(dirPath);
            }

            pageScreenshot.SaveAsFile(imageFilePath, _imageFormat);
        }

        private void CropScreenshotToMatchDiv(string divSelector, string imageFilePath, int cropAdjustWidth, int cropAdjustHeight)
        {
            //get screenshot of a particular element, via cropping:
            //http://stackoverflow.com/questions/13832322/how-to-capture-the-screenshot-of-only-a-specific-element-using-selenium-webdrive

            // Get html element and its location and dimensions:
            IWebElement div = _driver.FindElement(By.CssSelector(divSelector));

            // Load temp image as bitmap:
            Bitmap divBitmap;
            //copy the file to temp, to get around issue where Bitmap holds the file open, even after it is Disposed!
            var tempFilePath2 = Path.GetTempFileName();
            File.Copy(imageFilePath, tempFilePath2, true);
            using (var fullWindowBitmap = new Bitmap(tempFilePath2))
            {
                // crop page screenshot to the match the div:
                var cropSize =
                    new Size(
                        Math.Min(fullWindowBitmap.Width - div.Location.X,
                            div.Location.X + div.Size.Width + cropAdjustWidth),
                        Math.Min(fullWindowBitmap.Height - div.Location.Y,
                            div.Location.Y + div.Size.Height + cropAdjustHeight));

                var part = new RectangleF(div.Location.X, div.Location.Y, cropSize.Width, cropSize.Height);
                divBitmap = fullWindowBitmap.Clone(part, fullWindowBitmap.PixelFormat);
            }
            divBitmap.Save(imageFilePath, _imageFormat);
        }

        private void SleepBeforeScreenshot(TimeSpan sleepTimeSpan)
        {
            if (sleepTimeSpan.TotalMilliseconds > 0)
            {
                Outputter.Output("Sleeping before screenshot ...");
                Thread.Sleep(sleepTimeSpan);
            }
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
