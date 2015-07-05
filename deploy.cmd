@ECHO OFF
SET server=http://wuzlstats.sachsenhofer.com
SET siteName=Wuzlstats
SET username=Administrator

ECHO Publishing to %server%...
SET /p password=Enter password for %username%: 

rmdir /S /Q %~dp0artifacts\publish
Path=%~dp0src\Wuzlstats\node_modules\.bin;C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\\EXTENSIONS\MICROSOFT\WEB TOOLS\External;%PATH%;C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\\EXTENSIONS\MICROSOFT\WEB TOOLS\External\git

@ECHO ON
CALL %HOMEDRIVE%%HOMEPATH%\.dnx\runtimes\dnx-clr-win-x86.1.0.0-beta5-12103\bin\dnu.cmd publish "%~dp0src\Wuzlstats" --out "%~dp0artifacts\publish" --configuration Release --no-source --runtime dnx-clr-win-x64.1.0.0-beta5-12103 --wwwroot-out "wwwroot" --quiet

PAUSE

CALL %~dp0deploy\MsWebDeploy-3.6-beta\msdeploy.exe -verb:sync -source:recycleApp  -dest:recycleApp=%siteName%,recycleMode=StopAppPool,computername=%server%/MSDEPLOYAGENTSERVICE,username=%username%,password=%password%
CALL %~dp0deploy\MsWebDeploy-3.6-beta\msdeploy.exe -verb:sync -source:contentPath=%~dp0artifacts\publish\wwwroot -dest:contentPath=%siteName%,computername=%server%/MSDEPLOYAGENTSERVICE,username=%username%,password=%password%
CALL %~dp0deploy\MsWebDeploy-3.6-beta\msdeploy.exe -verb:sync -source:contentPathLib=%~dp0artifacts\publish\approot -dest:contentPathLib=%siteName%,computername=%server%/MSDEPLOYAGENTSERVICE,username=%username%,password=%password%
CALL %~dp0deploy\MsWebDeploy-3.6-beta\msdeploy.exe -verb:sync -source:recycleApp  -dest:recycleApp=%siteName%,recycleMode=StartAppPool,computername=%server%/MSDEPLOYAGENTSERVICE,username=%username%,password=%password%

PAUSE