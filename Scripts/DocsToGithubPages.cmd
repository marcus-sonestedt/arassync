@echo off

:: copy out and call ourselves so we don't overwrite/delete the script when switching branches
if %1"x"=="x" (
    copy /Y "%~f0" "%TEMP%\%~n0.cmd"
    call "%TEMP%\%~n0.cmd" "%~dp0.."
    exit /B %ERRORLEVEL%
)

setlocal
pushd %1

IF %NODOC%"x"=="x" (
  call Scripts\Documentation.cmd
  if ERRORLEVEL 1 goto done
)

echo.
echo *** Switching to and updating gh-pages branch *** 

:: Get current branch
git rev-parse --abbrev-ref HEAD > .git\current_branch.txt
git checkout gh-pages
if ERRORLEVEL 1 goto done
git pull 

echo.
echo *** Removing old files from disk *** 

move .gitignore .git\.gitignore-tmp
dir /A:-D /b > .git\files_to_delete.txt
move .git\.gitignore-tmp .gitignore

git ls-tree -d HEAD --name-only > .git\dirs_to_delete.txt

for /f "delims=" %%d in (.git\dirs_to_delete.txt) do rmdir /s /q "%%d"
if ERRORLEVEL 1 goto done
for /f "delims=" %%f in (.git\files_to_delete.txt) do del /F "%%f"
if ERRORLEVEL 1 goto done

echo.
echo *** Moving Docs\_site files and dirs to repo root ***
move .\Docs\_site\* .
if ERRORLEVEL 1 goto done

echo.
echo *** Committing all changes *** 
git commit -a -m"Updating gh-pages from Docs\_site"

:done
if ERRORLEVEL 1 (
    set ERRLVL=%ERRORLEVEL%
    echo.
    echo FAILURE CODE %ERRORLEVEL%
) ELSE (
    echo.
    echo Success!
)
echo.
echo Cleaning up ...
for /f "delims=" %%f in (.git\current_branch.txt) do git checkout "%%f"
del .git\current_branch.txt
del .git\dirs_to_delete.txt
del .git\files_to_delete.txt
echo.
pause
popd
EXIT /b %ERRLVL%
