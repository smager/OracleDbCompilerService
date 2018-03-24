@echo off

rem %1 = user and pwd and server
rem %2 = filename to compile

if '%2' == '' goto on_exit
if '%1' == '' goto on_exit

if '%~x2' == '.PSP' goto psp
if '%~x2' == '.psp' goto psp
if '%~x2' == '.SQL' goto sql
if '%~x2' == '.sql' goto sql
if '%~x2' == '.PLB' goto sql
if '%~x2' == '.plb' goto sql
goto end

:psp
echo.
echo Compiling PSP file in %1
echo.
echo PSP File: %2
loadpsp -replace -user %1 %2
echo %date% %time%
goto end

:sql
echo.
echo Compiling SQL file in %1
echo.
echo SQL File: %2
REM sqlplus -L %1 @%2
echo exit | sqlplus -S %1 @%2
rem echo compiling :  sqlplus -S %1 @%2
echo %date% %time%
goto end


:on_exit
echo Not enough parameters...


:end


