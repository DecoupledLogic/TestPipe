namespace TestPipe.Build
{
	using System;
	using Nake;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Xml.Linq;
	using System.Diagnostics;
	using Microsoft.Build.Utilities;
	using System.Text;
	using AssemblyInfoManager;

	public class BuildScript
	{
		public static string ApplicationName = "TestPipe";
		public static string BuildConfig = "Release";
		public static string BuildPlatform = "AnyCPU";
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
		public static string[] DistributeAppPaths = new string[] { ApplicationPath, DemoPath };

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
			TestPipe.Build.Build.SetLogging(logPath, true);

			TestPipe.Build.Build.PrintHeader("Go");

			if (string.IsNullOrWhiteSpace(version))
			{
				version = TestPipe.Build.Build.GetVersion(ApplicationVersion);
			}

			Log.Info("Version: " + version);

			Build(version, configuration, platform);

			Distro();

			Package(version);

			Publish();

			TestPipe.Build.Build.PrintFooter("Go");
		}

		/// <summary>
		/// Builds you solution's sources  
		/// </summary>
		[Task]
		public static void Build(string version, string configuration = "Release", string platform = "Any CPU")
		{
			TestPipe.Build.Build.PrintHeader("Build");

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
				//.Property("Configuration", configuration)
				//.Property("Platform", platform)
					.Property("ReferencePath", BuildPath)
					.Targets(new[] { "Rebuild" })
				.BuildInParallel();

			TestPipe.Build.Build.PrintFooter("Build");
		}

		/// <summary>
		/// Copy build output to distro folder
		/// </summary>
		[Task]
		public static void Distro()
		{
			TestPipe.Build.Build.PrintHeader("Distro");

			TestPipe.Build.Build.ResetDirectory(DistroPath);

			try
			{
				foreach (var directory in DistributeAppPaths)
				{
					TestPipe.Build.Build.CopyFilesToDistro(directory, BuildConfig, DistroPath, NuGetToolsPath, SolutionRoot);
				}
			}
			catch (System.Exception ex)
			{
				Log.Error(ex);
				throw;
			}

			TestPipe.Build.Build.PrintFooter("Distro");
		}

		[Task]
		public static void Package(string version)
		{
			TestPipe.Build.Build.PrintHeader("Package");

			TestPipe.Build.Build.ResetDirectory(PubPath);

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
					if (string.IsNullOrWhiteSpace(version))
					{
						version = TestPipe.Build.Build.GetVersion(ApplicationVersion);
					}
					string projectName = TestPipe.Build.Build.GetProjectName(directory);
					string output = PubPath;

					Log.Info("Creating Folder: " + output);
					FS.MakeDir(output);

					TestPipe.Build.Build.PackageProject(nuspec, version, output, directory, NuGetExe);
				}
			}
			catch (System.Exception ex)
			{
				Log.Error(ex);
			}

			TestPipe.Build.Build.PrintFooter("Package");
		}

		/// <summary>
		/// Publish packages to artifact repository
		/// </summary>
		[Task]
		public static void Publish()
		{
			TestPipe.Build.Build.PrintHeader("Publish");



			TestPipe.Build.Build.PrintFooter("Publish");
		}

		/// <summary>
		/// Generate code coverage report
		/// </summary>
		[Task]
		public static void ReportCoverage(string configuration = "Debug", string outputPath = null)
		{
			if (string.IsNullOrWhiteSpace(outputPath))
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

			string tests = new FileSet { @"{outputPath}\*.Specs.dll" };

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
			TestPipe.Build.Build.PrintHeader("UpdateAssembly");

			Log.Info("Version: " + version);

			try
			{
				if (string.IsNullOrWhiteSpace(version))
				{
					version = TestPipe.Build.Build.GetVersion(ApplicationVersion);
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
			catch (Exception ex)
			{
				Log.Error(ex);
			}

			TestPipe.Build.Build.PrintFooter("UpdateAssembly");
		}
	}
}