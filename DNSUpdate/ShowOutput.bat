
set /A fail=0
echo off
cls

:entrance
echo ==================================
DNSUpdate.exe
echo ==================================
echo: 
echo Program exited with %ERRORLEVEL%
if %ERRORLEVEL% == 0 goto exit

:error
set /A fail = %fail% + 1
echo failed %fail% times
echo:
if %fail% == 3 goto stop

goto entrance
:stop
pause
:exit

echo ==================================
if %fail% gtr 0 echo total failed %fail% times
