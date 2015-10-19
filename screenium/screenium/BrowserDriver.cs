using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace screenium
{
    //TODO make disposable and have IWebDriver as member
    class BrowserDriver
    {
        internal void TestChrome()
        {
            using (IWebDriver driver = new ChromeDriver())
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

                //Close the browser
                driver.Quit();
            }
        }

        internal void OpenUrl(string url, string divSelector, string titleContains)
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                try
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
                finally
                {
                    //Close the browser
                    driver.Quit();
                }
            }
        }

        internal void SaveDivImageToPath(string divSelector, string tempFilePath)
        {
            throw new NotImplementedException();
        }
    }
}
