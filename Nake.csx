#r "tools\MsBuild\Microsoft.Build.Utilities.v4.0.dll"
#r "tools\MsBuild\Microsoft.Build.Framework.dll"
#r "tools\AssemblyInfoManager\AssemblyInfoManager.dll"

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Diagnostics;
using System.Threading;
using Microsoft.Build.Utilities;
using System.Text;
using AssemblyInfoManager;

public static string ApplicationName = "TestPipe";
public static string BuildConfig = "Release";
public static string BuildPlatform = "Any CPU";
public static string CompanyName = "CharlesBryant.com";
public static string Copyright = "Copyright (c) 2014, " + CompanyName;
public static string Trademark = "Trademark by " + CompanyName;
public static string Divider = "---------------------------------";
public static string MajorMinorVersion = "0.0";
public static string ApplicationVersion = MajorMinorVersion + ".0";

public static string MsbuildExe = @"tools\MSBuild\MSBuild.exe";
public static string NuGetExe = @"tools\nuget.exe";
public static string NuGetToolsPath = ApplicationPath + @"\TestPipe.Build\tools\TestPipe";

public static string SolutionFile = "TestPipe.sln";

public static string SourcePath = "source";
public static string ApplicationPath = SourcePath + @"\application";
public static string TestPath = SourcePath + @"\tests";
public static string DemoPath = SourcePath + @"\demo";

public static string BuildPath = "Build";
public static string DocsPath = BuildPath + @"\Docs";
public static string ReportsPath = BuildPath + @"\Reports";
public static string DistroPath = BuildPath + @"\Distro";
public static string PubPath = BuildPath + @"\Pub";
public static string OutputPath = ReportsPath;
public static string[] DistributeAppPaths = new string[] { ApplicationPath, DemoPath };

//Command Line >nake Go "3.0.0"
//Where "3.0.0" is the version of the build
[Task]
public static void Default(string version = "", string configuration = "Release", string platform = "Any CPU", string logPath = "nake.log.txt")
{
	Go(version, configuration, platform, logPath);
}

/// <summary>
/// Build and package TestPipe  
/// </summary>
[Task]
public static void Go(string version = "", string configuration = "Release", string platform = "Any CPU", string logPath = "nake.log.txt")
{
	SetLogging(logPath, true);

	PrintHeader("Go");

	if (string.IsNullOrWhiteSpace(version))
	{
		version = GetVersion();
	}

	Log.Info("Version: " + version);

    NuGetRestore();
		
	Build(version, configuration, platform);

	Distro();

	Package(version);

	Publish();

	PrintFooter("Go");
}

/// <summary>
/// Builds you solution's sources  
/// </summary>
[Task]
public static void Build(string version, string configuration = "Release", string platform = "Any CPU")
{
	PrintHeader("Build");

	if (string.IsNullOrWhiteSpace(SolutionFile))
	{
		string error = "SolutionsFile can not be a null or white space.";
		Log.Error(error);
		throw new Exception(error);
	}

	UpdateAssembly(version);

	Log.Info("Configuration: " + configuration);
	Log.Info("Platform: " + platform);

	MSBuild
		.Projects(SolutionFile)
			.Property("Configuration", configuration)
			.Property("Platform", platform)
			.Property("ReferencePath", BuildPath)
			.Targets(new[] { "Rebuild" })
		.BuildInParallel();

	PrintFooter("Build");
}

/// <summary>
/// Copy build output to distro folder
/// </summary>
[Task]
public static void Distro()
{
	PrintHeader("Distro");

	ResetDirectory(DistroPath);

	try
	{
		foreach(var directory in DistributeAppPaths)
		{
			CopyFilesToDistro(directory);
		}
	}
	catch (System.Exception ex)
	{
		Log.Error(ex);
		throw;
	}

	PrintFooter("Distro");
}

[Task]
public static void Package(string version)
{
	PrintHeader("Package");

	ResetDirectory(PubPath);

	try
	{
		foreach (string directory in Directory.GetDirectories(DistroPath))
		{
			Log.Info("Packaging:" + directory);

			//Get nuspec file
			DirectoryInfo directoryInfo = new DirectoryInfo(directory);

			FileInfo file = directoryInfo.EnumerateFiles()
				.Where(f => ".nuspec".Contains(f.Extension.ToLower()))
				.FirstOrDefault();

			if (file == null)
			{
				continue;
			}

			string nuspec = file.FullName;
			if(string.IsNullOrWhiteSpace(version))
			{
				version = GetVersion();
			}
			string projectName = GetProjectName(directory);
			string output = PubPath;

			Log.Info("Creating Folder: " + output);
			FS.MakeDir(output);

			PackageProject(nuspec, version, output, directory);
		}
	}
	catch (System.Exception ex)
	{
		Log.Error(ex);
	}

	PrintFooter("Package");
}

/// <summary>
/// Publish packages to artifact repository
/// </summary>
[Task]
public static void Publish()
{
	PrintHeader("Publish");

		

	PrintFooter("Publish");
}

/// <summary>
/// Generate code coverage report
/// </summary>
[Task]
public static void ReportCoverage(string configuration = "Debug", string outputPath = null)
{
	if(string.IsNullOrWhiteSpace(outputPath))
	{
		outputPath = OutputPath;
	}
		
	TestAndCoverage(configuration, outputPath);

	string reports = @".\Reports\projectCoverageReport.xml";
	string targetdir = @".\Reports\CodeCoverage";
	string reporttypes = @"Html,HtmlSummary^";
	string filters = @"-EmailService.Test.Core*";

	string command = string.Format(@".\packages\ReportGenerator.2.0.0-beta5\ReportGenerator.exe -reports:{0} -targetdir:{1} -reporttypes:{2} -filters:{3}", reports, targetdir, reporttypes, filters);

	Cmd.Exec(command);
}

/// <summary>
/// Runs unit tests
/// </summary>
[Task]
public static void XunitTest(string configuration = "Debug", string outputPath = null)
{
	if (string.IsNullOrWhiteSpace(outputPath))
	{
		outputPath = OutputPath;
	}

	Build(configuration, outputPath);

	var target = @".\packages\xunit.runners.2.0.0-beta-build2650\tools\xunit.console.x86.exe";
	var targetargs = @".\Output\EmailService.Test.Core.dll";

	string command = "{target} {targetargs}";

	Cmd.Exec(command);
}

/// <summary>
/// Runs unit tests and code coverage
/// </summary>
[Task]
public static void TestAndCoverage(string configuration = "Debug", string outputPath = null)
{
	if (string.IsNullOrWhiteSpace(outputPath))
	{
		outputPath = OutputPath;
	}

	Build(configuration, outputPath);

	string tests = new FileSet	{ @"{outputPath}\*.Specs.dll" };

	//Cmd.Exec(@"Packages\NUnit.Runners.2.6.2\tools\nunit-console.exe /framework:net-4.0 /noshadow /nologo {tests}");

	var register = "user";
	var target = @".\packages\xunit.runners.2.0.0-beta-build2650\tools\xunit.console.x86.exe";
	var targetargs = @".\Output\EmailService.Test.Core.dll /noshadow";
	var targetdir = @".\Output";
	var filter = "+[EmailService.Core]* -[EmailService.Core.Test]*";
	var output = @".\Reports\projectCoverageReport.xml";

	CommandLineBuilder builder = new CommandLineBuilder();
	builder.AppendTextUnquoted(".\\packages\\OpenCover.4.5.2506\\OpenCover.Console.exe");
	builder.AppendSwitchIfNotNull(" -register:{register} ", "-target:" + target);
	builder.AppendSwitchIfNotNull(" -targetargs:", targetargs);
	builder.AppendTextUnquoted(" -targetdir:{targetdir}");
	builder.AppendSwitchIfNotNull(" -filter:", filter);
	builder.AppendTextUnquoted(" -mergebyhash");
	builder.AppendTextUnquoted(" -output:{output}");

	Cmd.Exec(builder.ToString());
}

/// <summary>
/// Update the assembly file for a project.
/// </summary>
[Task]
public static void UpdateAssembly(string version = null)
{
	PrintHeader("UpdateAssembly");

	Log.Info("Version: " + version);

	try
	{
		if (string.IsNullOrWhiteSpace(version))
		{
			version = GetVersion();
		}

		string[] filePaths = Directory.GetFiles(ApplicationPath, "AssemblyInfo.cs", SearchOption.AllDirectories);

		foreach (var path in filePaths)
		{
			Log.Info("Updating: " + path);
			AssemblyInfo assemblyInfo = new AssemblyInfo()
				.Attribute("AssemblyVersion", version)
				.Attribute("AssemblyFileVersion", version)
				.Attribute("AssemblyTitle", ApplicationName)
				.Attribute("AssemblyCopyright", Copyright)
				.Attribute("AssemblyTrademark", Trademark)
				.Reference("System")
				.Reference("System.Reflection")
				.Path(path);

			assemblyInfo.Update();
		}
	}
	catch(Exception ex)
	{
		Log.Error(ex);
	}

	PrintFooter("UpdateAssembly");
}

public static void PackageProject(string nuspec, string version, string output, string basePath)
{
	Log.Info("Nuspec: " + nuspec);
    Log.Info("Version: " + version);
    Log.Info("Output: " + output);
    Log.Info("Base Path: " + basePath);
    Log.Info("NuGetExe: " + NuGetExe);

    string packCmd = string.Format("{0} pack \"{1}\" -Version {2} -OutputDirectory \"{3}\" -BasePath \"{4}\" -NoPackageAnalysis -Symbols", NuGetExe, nuspec, version, output, basePath);

    Log.Info("Pack Command: " + packCmd);

	Cmd.Exec(packCmd);
}

public static void NuGetRestore()
{
	string cmd = string.Format("{0} restore {1} -source \"http://agpjaxsrvbuild2/pne.nuget/nuget\"", NuGetExe, SolutionFile);
    
    Log.Info("Restore Command: " + cmd);
	Cmd.Exec(cmd);
}

public static void CopyFilesToDistro(string path)
{
	foreach (string directory in Directory.GetDirectories(path))
	{
		if (directory.EndsWith("Specs"))
		{
			continue;
		}

		Log.Info("Copying files to distribution path: " + directory);

		string projectName = GetProjectName(directory);

		ProcessDistroSourceFiles(directory, projectName);
	}
}

public static string GetProjectName(string directory)
{
	return directory.Substring(directory.LastIndexOf('\\') + 1);
}

public static string GetVersion()
{
	string defaultVersion = ApplicationVersion;
	return defaultVersion;
}

public static void ProcessDistroSourceFiles(string directory, string projectName)
{
	CopyNuspecFiles(directory, projectName);
	CopyLibFiles(directory, projectName);
	CopyContentFiles(directory, projectName);
	CopyToolsFiles(NuGetToolsPath, projectName);
	CopyLicenseFiles(string.Empty, projectName);
}

public static void CopyNuspecFiles(string directory, string projectName)
{
	Log.Info("Processing " + projectName + " nuspec files");

	string source = directory;

	string dest = string.Format(@"{0}\{1}", DistroPath, projectName);

	string[] include = new[] { ".nuspec" };

	CopyFiles(source, dest, include);
}

public static void CopyLibFiles(string directory, string projectName)
{
	Log.Info("Copying " + projectName + " lib files");

	string source = string.Format(@"{0}\bin\{1}", directory, BuildConfig);
	Log.Info("Lib Source: " + source);

	string dest = string.Format(@"{0}\{1}\lib\net40", DistroPath, projectName);
	Log.Info("Lib Destination:  " + dest);

	string[] include = new[] { ".dll", ".exe", ".config", ".xml", ".pdb" };

	CopyFiles(source, dest, include);
}

public static void CopyContentFiles(string directory, string projectName)
{
	Log.Info("Copying " + projectName + " content files");

	string source = string.Format(@"{0}", directory);

	string dest = string.Format(@"{0}\{1}\content", DistroPath, projectName);

	string[] include = new[] { ".txt", ".xml", ".config", ".htm", ".html", ".js", ".jpg", ".gif", ".png", ".css" };
	string[] exclude = new[] { "packages.config" };

	CopyFiles(source, dest, include, exclude);
}

public static void CopyToolsFiles(string directory, string projectName)
{
	Log.Info("Copying " + projectName + " NuGet tools files");

	string source = directory;

	string dest = string.Format(@"{0}\{1}\tools", DistroPath, projectName);

	string[] include = new[] { ".ps1", ".psm1" };

	CopyFiles(source, dest, include);
}

public static void CopyLicenseFiles(string directory, string projectName)
{
	Log.Info("Copyng " + projectName + " license files");

	string source = directory + @"license.txt";

	string dest = string.Format(@"{0}\{1}", DistroPath, projectName);

	Log.Info(string.Format("Copying {0} to {1}", source, dest));
	FS.Copy(source, dest, true, false);
}

public static void Stage(string version)
{
	foreach (string source in Directory.GetDirectories(DistroPath))
	{
		Log.Info("Processing " + source);

		string projectName = GetProjectName(source);

		string packageVersion = version;

		string dest = string.Format(@"{0}\{1}.{2}", PubPath, projectName, packageVersion);

		string[] include = new[] { ".nupkg" };

		CopyFiles(source, dest, include);
	}
}

public static void CopyFiles(string source, string dest, string[] include = null, string[] exclude = null)
{
	if (!System.IO.Directory.Exists(source))
	{
		return;
	}

	DirectoryInfo directoryInfo = new DirectoryInfo(source);

	IEnumerable<FileInfo> includedFiles = directoryInfo.EnumerateFiles();

	if (include != null)
	{
		includedFiles = includedFiles.Where(f => include.Contains(f.Extension.ToLower()));
	}

	if (exclude != null)
	{
		includedFiles = includedFiles.Where(f => !exclude.Contains(f.Name.ToLower()));
	}

	FileInfo[] files = includedFiles.ToArray();

	if (files.Length < 1)
	{
		return;
	}

	foreach (FileInfo file in files)
	{
		Log.Info(string.Format("Copying {0} to {1}", file.FullName, dest));
		FS.Copy(file.FullName, dest, true, false);
	}
}

public static void DeleteFiles(string[] files)
{
	foreach (string file in files)
	{
		File.SetAttributes(file, FileAttributes.Normal);
		File.Delete(file);
	}
}

public static void PrintHeader(string message)
{
	PrintMessage(message, "Started");
}

public static void PrintFooter(string message)
{
	PrintMessage(message, "Completed");
}

public static void PrintMessage(string message, string type)
{
	Log.Info(string.Empty);
	Log.Info(Divider);
	Log.Info(string.Format("@@{0} : {1}", message, type));
	Log.Info(string.Format("@@Timestamp: {0}", DateTime.Now.ToString()));
	Log.Info(Divider);
	Log.Info(string.Empty);
}

public static void ResetDirectories(string[] directories)
{
	CleanDirectories(directories);
	CreateDirectories(directories);
}

public static void ResetDirectory(string directory)
{
	string[] directories = new string[] { directory };
	ResetDirectories(directories);
}

public static void DeleteDirectory(string target)
{
	if (!Directory.Exists(target))
	{
		return;
	}

	string[] files = Directory.GetFiles(target);
	IEnumerable<string> dirs = Directory.GetDirectories(target);

	DeleteFiles(files);

	foreach (string dir in dirs)
	{
		Log.Info("Deleting files in " + dir);
		string[] dirFiles = Directory.GetFiles(dir);
		DeleteFiles(dirFiles);
			
		Log.Info("Deleting directory " + dir);
		DeleteDirectory(dir);
	}

	Directory.Delete(target, false);
}

public static void CleanDirectories(string[] directories)
{
	try
	{
		foreach (var folder in directories.Reverse())
		{
			Log.Info("Removing Folder: " + folder);
			DeleteDirectory(folder);
		}
	}
	catch (System.Exception ex)
	{
		Log.Error(ex);
	}
}

public static void CreateDirectories(string[] directories)
{
	foreach (var folder in directories)
	{
		Log.Info("Creating Folder: " + folder);
		FS.MakeDir(folder);
	}
}

public static void Alert(string message)
{
	string log = string.Format("##########------ {0} ------##########", message);
	Log.Info(log);
}

public static void SetLogging(string logPath = "", bool overwrite = false)
{
	if(overwrite)
	{
		File.Delete(logPath);
	}

	//Print log to file and console.
	if (!string.IsNullOrWhiteSpace(logPath))
	{
		Log.Out = text => {
			File.AppendAllText(logPath, text + Environment.NewLine);
			Console.WriteLine(text);
		};
		return;
	}

	Log.Out = text => {
		Console.WriteLine(text);
	};
}