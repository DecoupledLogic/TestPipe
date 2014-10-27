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

	public static string MsbuildExe = @"C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe";
	public static string NuGetExe = @"C:\_TestPipe\tools\nuget.exe";
	public static string NuGetToolsPath = ApplicationPath + @"\TestPipe.Build\tools\TestPipe";

	public static string SolutionRoot = @"C:\_TestPipe";
	public static string SolutionFile = SolutionRoot + @"\TestPipe.sln";

	public static string SourcePath = SolutionRoot + @"\source";
	public static string ApplicationPath = SourcePath + @"\application";
	public static string TestPath = SourcePath + @"\tests";
	public static string DemoPath = SourcePath + @"\demo";

	public static string BuildPath = SolutionRoot + @"\Build";
	public static string DocsPath = BuildPath + @"\Docs";
	public static string ReportsPath = BuildPath + @"\Reports";
	public static string DistroPath = BuildPath + @"\Distro";
	public static string PubPath = BuildPath + @"\Pub";
	public static string OutputPath = ReportsPath;

	[Task]
	public static void Default(string version = "", string configuration = "Release", string platform = "Any CPU", string logPath = "nake.log.txt")
	{
		Go(configuration, platform, version, logPath);
	}

	/// <summary>
	/// Build and package TestPipe  
	/// </summary>
	[Task]
	public static void Go(string version = "", string configuration = "Release", string platform = "Any CPU", string logPath = "nake.log.txt")
	{
		File.Delete(logPath);

		Log.Out = text => {
			File.AppendAllText(logPath, text + Environment.NewLine);
			Console.WriteLine(text);
		};

		PrintHeader("Go");

		Log.Info("Configuration: " + configuration);
		Log.Info("Platform: " + platform);
		Log.Info("Version: " + version);
		
		Clean();

		Init();

		UpdateAssembly(version);

		Build(configuration, platform);

		Distro();

		Package(version);

		Publish();

		PrintFooter("Go");
	}

	/// <summary> 
	/// Delete build artifact folders
	/// </summary>
	[Task]
	public static void Clean()
	{
		PrintHeader("Clean");

		string[] folders = new string[] { BuildPath };
		CleanFolders(folders);

		PrintFooter("Clean");
	}

	/// <summary> 
	/// Creates build artifact folder
	/// </summary>
	[Task]
	public static void Init()
	{
		PrintHeader("Init");

		string[] folders = new string[] { BuildPath, DocsPath, ReportsPath };
		CreateFolders(folders);

		PrintFooter("Init");
	}

	/// <summary>
	/// Builds you solution's sources  
	/// </summary>
	[Task]
	public static void Build(string configuration = "Release", string platform = "Any CPU")
	{
		PrintHeader("Build");

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

		string[] folders = new string[] { DistroPath };
		CleanFolders(folders);
		CreateFolders(folders);

		try
		{
			CopyFilesToDistro(ApplicationPath);
			CopyFilesToDistro(DemoPath);
		}
		catch (System.Exception ex)
		{
			Log.Error(ex.Message);
		}

		PrintFooter("Distro");
	}

	[Task]
	public static void Package(string version)
	{
		PrintHeader("Package");

		Log.Info("Version: " + version);

		string[] folders = new string[] { PubPath };
		CleanFolders(folders);
		CreateFolders(folders);

		try
		{
			foreach (string directory in Directory.GetDirectories(DistroPath))
			{
				Log.Info("Packaging:" + directory);

				if (directory.Contains("TestPipe.Build"))
				{
					continue;
				}

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
				//string output = string.Format(@"{0}\{1}\{2}.{3}", PubPath, projectName, projectName, version);
				string output = PubPath;

				Log.Info("Creating Folder: " + output);
				FS.MakeDir(output);

				PackageProject(nuspec, version, output, directory);
			}
		}
		catch (System.Exception ex)
		{
			Log.Error(ex.Message);
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
			Log.Info(ex.Message + "                    " + ex.StackTrace);
		}

		PrintFooter("UpdateAssembly");
	}

	public static void PackageProject(string nuspec, string version, string output, string basePath)
	{
		string pack = string.Format("{0} pack {1} -Version {2} -OutputDirectory {3} -BasePath {4} -NoPackageAnalysis", NuGetExe, nuspec, version, output, basePath);

		Cmd.Exec(pack);
	}

	public static void CopyFilesToDistro(string path)
	{
		foreach (string directory in Directory.GetDirectories(path))
		{
			if (directory.EndsWith("Specs"))
			{
				continue;
			}

			Log.Info("------ Processing " + directory + "------");

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
		CopyLicenseFiles(SolutionRoot, projectName);
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
		Log.Info("Processing " + projectName + " lib files");

		string source = string.Format(@"{0}\bin\{1}", directory, BuildConfig);
		Log.Info("Lib Source: " + source);

		string dest = string.Format(@"{0}\{1}\lib\net40", DistroPath, projectName);
		Log.Info("Lib Destination:  " + dest);

		string[] include = new[] { ".dll", ".exe", ".config", ".xml" };

		CopyFiles(source, dest, include);
	}

	public static void CopyContentFiles(string directory, string projectName)
	{
		Log.Info("Processing " + projectName + " content files");

		string source = string.Format(@"{0}", directory);

		string dest = string.Format(@"{0}\{1}\content", DistroPath, projectName);

		string[] include = new[] { ".txt", ".xml", ".config", ".htm", ".html", ".js", ".jpg", ".gif", ".png", ".css" };
		string[] exclude = new[] { "packages.config" };

		CopyFiles(source, dest, include, exclude);
	}

	public static void CopyToolsFiles(string directory, string projectName)
	{
		Log.Info("Processing " + projectName + " NuGet tools files");

		string source = directory;

		string dest = string.Format(@"{0}\{1}\tools", DistroPath, projectName);

		string[] include = new[] { ".ps1", ".psm1" };

		CopyFiles(source, dest, include);
	}

	public static void CopyLicenseFiles(string directory, string projectName)
	{
		Log.Info("Processing " + projectName + " license files");

		string source = directory + @"\license.txt";

		string dest = string.Format(@"{0}\{1}", DistroPath, projectName);

		Log.Info(string.Format("Copying {0} to {1}", source, dest));
		FS.Copy(source, dest, true, false);
	}

	public static void Release(string version)
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

	public static void DeleteDirectory(string target)
	{
		if (!Directory.Exists(target))
		{
			return;
		}

		string[] files = Directory.GetFiles(target);
		string[] dirs = Directory.GetDirectories(target);

		foreach (string file in files)
		{
			File.SetAttributes(file, FileAttributes.Normal);
			File.Delete(file);
		}

		Thread.Sleep(500);

		foreach (string dir in dirs)
		{
			DeleteDirectory(dir);
		}

		Directory.Delete(target, false);
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

	public static void CleanFolders(string[] folders)
	{
		try
		{
			foreach (var folder in folders)
			{
				Log.Info("Removing Folder: " + folder);
				DeleteDirectory(folder);
			}
		}
		catch (System.Exception ex)
		{
			Log.Error(ex.Message);
			throw;
		}
	}

	public static void CreateFolders(string[] folders)
	{
		foreach (var folder in folders)
		{
			Log.Info("Creating Folder: " + folder);
			FS.MakeDir(folder);
		}
	}