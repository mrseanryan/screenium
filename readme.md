screenium readme
================

Screenium = a console tool for automated web site testing, via screenshots taken with Selenium.

dependencies
============
- Visual Studio 2013
- Chrome web browser

- Selenium WebDriver
nuget: Selenium.WebDriver
version: 2.46.0

nuget: WebDriver.Support

- ChromeDriver
version: 2.16
http://chromedriver.storage.googleapis.com/index.html?path=2.16/

references
==========

how-to:
-------
http://www.jimmycollins.org/blog/?p=466

C# API:
-------
http://seleniumhq.github.io/selenium/docs/api/dotnet/index.html

Selenium WebDriver nuget package:
---------------------------------
Selenium.WebDriver

http://www.seleniumhq.org/docs/03_webdriver.jsp

http://www.seleniumhq.org/docs/03_webdriver.jsp#chromedriver 
 
chrome driver
-------------
https://sites.google.com/a/chromium.org/chromedriver/

https://sites.google.com/a/chromium.org/chromedriver/getting-started

https://github.com/SeleniumHQ/selenium/wiki/ChromeDriver

Selenium WebDriver & chromedriver
---------------------------------
WebDriver works with Chrome through the chromedriver binary (found on the chromium project’s download page). You need to have both chromedriver and a version of chrome browser installed.

chromedriver needs to be placed somewhere on your system’s path in order for WebDriver to automatically discover it.

The Chrome browser itself is discovered by chromedriver in the default installation path.
These both can be overridden by environment variables. 
Please refer to the wiki for more information:
https://github.com/SeleniumHQ/selenium/wiki/ChromeDriver
