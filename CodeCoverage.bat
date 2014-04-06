REM This is to run OpenCover and ReportGenerator to get test coverage data.
REM OpenCover and ReportGenerator where added to the solution via NuGet.
REM Need to make this a real batch file or execute from NANT.
REM See reference, https://github.com/sawilde/opencover/wiki/Usage, http://blog.alner.net/archive/2013/08/15/code-coverage-via-opencover-and-reportgenerator.aspx

REM Bring dev tools into the PATH.
call "C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\Tools\VsDevCmd.bat"

mkdir .\Reports

REM Restore packages
msbuild .\.nuget\NuGet.targets /target:RestorePackages

REM Ensure build is up to date
msbuild "SunGard.PNE.Test.sln" /target:Rebuild /property:Configuration=Release;OutDir=.\Releases\Latest\NET40\

REM Run unit tests
.\packages\OpenCover.4.5.2316\OpenCover.Console.exe -register:user -target:"C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\mstest.exe" -targetargs:"/testcontainer:.\source\tests\SunGard.PNE.Test.Specs\bin\Debug\SunGard.PNE.Test.Specs.dll" -filter:"+[SunGard.PNE.Test]* -[SunGard.PNE.Test.*]*" -mergebyhash -output:.\Reports\projectCoverageReport.xml

REM Generate the report
.\packages\ReportGenerator.1.9.1.0\ReportGenerator.exe -reports:".\Reports\projectCoverageReport.xml" -targetdir:".\Reports\CodeCoverage" -reporttypes:Html,HtmlSummary^ -filters:-SunGard.PNE.Test.Specs*

REM Open the report
start .\Reports\CodeCoverage\index.htm

pause