@ECHO OFF
ECHO Running same report suite twice, to demo the Combined Report.
@ECHO ON

SETLOCAL
SET _COMBINED_PATH_XML=temp\combinedReport.xml
IF EXIST "%_COMBINED_PATH_XML%" (del "%_COMBINED_PATH_XML%")

SET _COMBINED_PATH_HTML=temp\combinedReport.html


CALL run_google_home_quiet.bat -q

CALL run_google_home_quiet.bat -q

explorer "%_COMBINED_PATH_HTML%"
