/*
Configure: take data configuration files and use them to generate the database structure and the code to access the database.
Compile: compile the application code.
DataLoad: load test data into the database.
Test: run the tests.

Targets:
uninstall		    Removes existing PaySpan IIS Virtual Directories
setup				Creates PaySpan IIS Virtual Directories
deploy				Installs allweb applications, web services, libraries and tools
DeployReports		Deploys SQL Server reporting Services reports
deployDBUpdate		Deploys DB objects (stored procs, views, triggers, UDFs)
deployDB			Creates DB databases and tables and all objects
build-debug			Builds a debug version
build-release		Builds a release version
build-deploy		Calls the build target followed by the deploy target
build-deploy+db     Calls the build target, deploys DB objects followed by the deploy target
build-deploy+db+reports	Builds, deploys apps, updates DB objects and deploys Reports.
uninstallbranding	Removes all virtual directories associated with branded eStub and PaySpan sites
setupbranding		Creates all virtual directories associated with branded eStub and PaySpan sites
retrieve-tag		Uses variable 'gold.tag' to setup tagged revision for build-deploy to QA system (& in the future, the production system)
buildonly			Only builds the solution, does not call packager and or deployment.  used for smoke testing.
emailtemplates		Moves email templates to a unprocessed folder and generates email templates containing styles from the emailstyles.css file in the site's branding folder. 
*/

#r "Tools\Nake\Meta.dll"
#r "Tools\Nake\Utility.dll"
#r "System.Xml"
#r "System.Xml.Linq"

using Nake;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Diagnostics;

public const string RootPath = "$NakeScriptDirectory$";
public const string OutputPath = RootPath + @"\Output";

[Task] public static void Default()
{
        Build();
}

/// <summary> 
/// Wipeout all build output and temporary build files
/// </summary>
[Task] public static void Clean(string path = OutputPath)
{
        FS.RemoveDir(@"**\bin|**\obj");

        FS.Delete(@"{path}\*.*|-:*.vshost.exe");
        FS.RemoveDir(@"{path}\*");
}

/// <summary> 
/// Builds Nake sources  
/// </summary>
[Task] public static void Build(string configuration = "Debug", string outputPath = OutputPath)
{
        Clean(outputPath);

        MSBuild
                
                .Projects("SunGard.PNE.Test.sln")
                        .Property("Platform", "Any CPU")
                        .Property("Configuration", configuration)
                        .Property("OutDir", outputPath)
                        .Property("ReferencePath", outputPath)

        .Build();
}

/// <summary> 
/// Runs unit tests and code coverage
/// </summary>
[Task] public static void TestAndCoverage(string configuration = "Debug", string outputPath = OutputPath)
{
        Build(configuration, outputPath);

        string tests = new FileSet
        {
                @"{outputPath}\*.Specs.dll"
        };

        //Cmd.Exec(@"Packages\NUnit.Runners.2.6.2\tools\nunit-console.exe /framework:net-4.0 /noshadow /nologo {tests}");
				var register = @"user";
				var target = "\"\"C:\\Program Files (x86)\\Microsoft Visual Studio 12.0\\Common7\\IDE\\mstest.exe\"\"";
				var targetargs = @"/testcontainer:.\output\SunGard.PNE.Test.Specs.dll";
				var filter = "\"\"+[SunGard.PNE.Test]* -[SunGard.PNE.Test.*]*\"\"";
				var output = @".\Reports\projectCoverageReport.xml";

				Cmd.Exec(@".\packages\OpenCover.4.5.2316\OpenCover.Console.exe -register:{register} -target:{target} -targetargs:{targetargs} -filter:{filter} -mergebyhash -output:{output}");
}

/// <summary> 
/// Generate code coverage report
/// </summary>
[Task] public static void ReportCoverage(string configuration = "Debug", string outputPath = OutputPath)
{
        TestAndCoverage(configuration, outputPath);

        var reports = "\"\".\\Reports\\projectCoverageReport.xml\"\"";
				var targetdir = "\"\".\\Reports\\CodeCoverage\"\"";
				var reporttypes = @"Html,HtmlSummary^";
				var filters = @"-SunGard.PNE.Test.Specs*";

				Cmd.Exec(@".\packages\ReportGenerator.1.9.1.0\ReportGenerator.exe -reports:{reports} -targetdir:{targetdir} -reporttypes:{reporttypes} -filters:{filters}");
}

/// <summary> 
/// Builds NuGet package
/// </summary>
[Task] public static void Package()
{
        var packagePath = OutputPath + @"\Package";
        var releasePath = packagePath + @"\Release";

        ReportCoverage("Debug", packagePath + @"\Debug");

        var version = FileVersionInfo
                        .GetVersionInfo(@"{releasePath}\SunGard.PNE.Test.dll")
                        .FileVersion;

				//This builds the nake batch file, hold over from the build script I copied this from
				//File.WriteAllText(
				//				@"{releasePath}\Nake.bat",
				//				"@ECHO OFF \r\n" +
				//				@"Packages\Nake.{version}\tools\net45\Nake.exe %*"
				//);

        Cmd.Exec(@"tools\nuget.exe pack -sym source\application\SunGard.PNE.Test\SunGard.PNE.Test.1.0.0.nuspec -Version {version} " +
                          "-OutputDirectory {packagePath} -BasePath {RootPath} -NoPackageAnalysis");
}

/// <summary> 
/// Installs dependencies (packages) from NuGet
/// </summary>
/// <remarks> This is the replacement for ubiquitous Install-Packages.ps1 - just for fun </remarks>
[Task] public static void Install()
{
        var packagesDir = @"{RootPath}\packages";

        var configs = XElement
                .Load(packagesDir + @"\repositories.config")
                .Descendants("repository")
                .Select(x => x.Attribute("path").Value.Replace("..", RootPath)); 

        foreach (var config in configs)
        {
                Cmd.Exec(@"tools\nuGet.exe install {config} -o {packagesDir}");
        }
}