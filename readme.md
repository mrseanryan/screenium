screenium
=========

screenium = a console tool to automatically test web sites

- easy to configure (just a CSV file containing URLs) 
- compares screenshots taken via Selenium WebDriver
- easy to update your 'expected' screenshots
- spend less time testing, and more time coding!
- browsers: currently Google Chrome only

License
=======
The MIT License (MIT)

Copyright (c) 2015 Sean Ryan

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

screenium 1.1.0.0
Usage: <CSV_FILE_PATH> <TEST_NAME> <OUTPUT_FILE_PATH> <IMAGES_DIR_PATH> <DIFFERENCE_TOLERANCE> <OPTIONS>
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