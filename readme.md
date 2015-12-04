screenium
=========


![screenium_icon_45w.png](https://bitbucket.org/repo/rzKA8y/images/436353880-screenium_icon_45w.png)
screenium = a console tool to automatically test web sites

- easy to configure (just a CSV file containing URLs)

- compares screenshots taken via Selenium WebDriver

- easy to update your 'expected' screenshots

- spend less time testing, and more time coding!

- browsers: currently Google Chrome only

- integration with your application is via URLs only - suitable for SPA or similar...

   |       
-----|------
sample report | ![sample_report_screenshot.PNG](https://bitbucket.org/repo/rzKA8y/images/1379854876-sample_report_screenshot.PNG)

License
=======
The MIT License (MIT)

- screenium code written by Sean Ryan.

- uses 3rd party DLLs (see Dependencies section).

How To Use screenium
====================
1 - install the dependencies (see below)

2 - build the source code using Visual Studio 2013 or later

3 - run the self-test to make sure that everything is working OK

run_self_test.bat

4 - configure the CSV file

see example CSV: 

screenium\TestData\testGoogleHome.csv

5 - run the console tool to save 'expected' images

see example BAT file:

run_google_home_save.bat

6 - run the console tool to compare 'actual' web site, to the saved 'expected' images

see example BAT file: 

run_google_home.bat

A simple text report is output:

```
#!BAT
Test Results:
Google search by image page
Result: Different
Tolerance: 0.01
Distortion: 0.0546446136539016
Finished running tests [OK]
```

For manual inspection, a report containing difference images is generated.
For integration with a build, the console returns error codes as standard.

Usage From Console
==================
To see the arguments and options, simply type:


```
#!BAT

 screenium
```

 and press ENTER:
 

```
#!BAT

screenium 1.2.0.0
Usage: <CSV_FILE_PATH> <TEST_NAME> <OUTPUT_FILE_PATH> <IMAGES_DIR_PATH> <TEMPLATES_DIR_PATH> <OPTIONS>
Options:
-r = Run
-s = Save
-t = TestSelf
```


Dependencies
============
- Visual Studio 2013
- Chrome web browser

- Selenium WebDriver
nuget: Selenium.WebDriver
version: 2.46.0

nuget: WebDriver.Support

- Image Magick.NET
nuget: Magick.NET-Q16-AnyCPU

- ChromeDriver [is included in source code]
version: 2.20 [but its best to use the latest one!]
https://sites.google.com/a/chromium.org/chromedriver/downloads
http://chromedriver.storage.googleapis.com/index.html

- VS C++ 2015 Redistributable [for Image Magick.NET]
x86 AND x64
http://www.microsoft.com/en-us/download/details.aspx?id=48145

References
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
WebDriver works with Chrome through the chromedriver binary (found on the chromium project download page). You need to have both chromedriver and a version of chrome browser installed.

chromedriver needs to be placed somewhere on your system path in order for WebDriver to automatically discover it.

The Chrome browser itself is discovered by chromedriver in the default installation path.
These both can be overridden by environment variables. 
Please refer to the wiki for more information:
https://github.com/SeleniumHQ/selenium/wiki/ChromeDriver

Auto generated API docs - GhostDoc
----------------------------------
This help file was generated with the *free* edition of GhostDoc (a VS extension):
docs\Help\screenium.chm

ref:
http://submain.com/products/ghostdoc.aspx

Auto generated API docs - docfx
-------------------------------
- when built in *Release* config, screenium project uses the docfx tool to generate API docs.

GhostDoc is probably better, but on the other hand, docfx installs itself via nuget,
and runs out-of-the-box as part of the build.

ref:
http://aspnet.github.io/docfx/tutorial/docfx.exe_user_manual.html

nuget package: docfx.msbuild

notes:
- only *public* types and methods are documented
- docfx is still in alpha!
