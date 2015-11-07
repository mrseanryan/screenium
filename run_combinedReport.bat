@ECHO OFF
ECHO Running same report suite twice, to demo the Combined Report.
@ECHO ON

SETLOCAL
SET _COMBINED_PATH=temp\combinedReport.xml
IF EXIST "%_COMBINED_PATH%" (del "%_COMBINED_PATH%")

CALL run_google_home.bat

CALL run_google_home.bat
