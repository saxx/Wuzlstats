IF EXIST %~dp0artifacts\publish rmdir /S /Q %~dp0artifacts\publish

dnu.cmd publish "%~dp0src\Wuzlstats" --out "%~dp0artifacts\publish" --configuration Release --no-source --wwwroot-out "wwwroot"


