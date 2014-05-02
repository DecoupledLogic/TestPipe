using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Diagnostics;
using Nake;

public static string ApplicationName = "TestPipe";
public static string BuildConfig = "Release";
public static string BuildPlatform = "Any CPU";
public static string CompanyName = "CharlesBryant.com";
public static string Copyright = "Copyright (c) 2014, " + CompanyName;
public static string Trademark = "Trademark by " + CompanyName;
public static string Divider = "-----------------------------------";
public static string MajorMinorVersion = "1.0";
public static string ApplicationVersion = MajorMinorVersion + ".0";
public static string MsbuildExe = @"C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe";
public static string NuGetExe = @"C:\_TestPipe\tools\nuget.exe";
public static string SolutionRoot = @"C:\_TestPipe";
public static string BuildPath = SolutionRoot + @"\Build";
public static string SolutionFile = SolutionRoot + @"\TestPipe.sln";
public static string SourcePath = SolutionRoot + @"\source";
public static string ApplicationPath = SourcePath + @"\application";
public static string DemoPath = SourcePath + @"\demo";
public static string ReleasePath = BuildPath + @"\Release";
public static string OutputPath = BuildPath + @"\Output";
public static string PubPath = BuildPath + @"\Pub";
public static string DocsPath = BuildPath + @"\Docs";
public static string DistroPath = BuildPath + @"\Distro";
public static string WebPath = BuildPath + @"\Output_PublishedWebsites";
public static string TestPath = SourcePath + @"\tests";

[Task] public static void Default()
{
	Greeting();
}

/// <summary>
/// Builds you solution's sources  
/// </summary>
[Task] public static void Go(string configuration = "Release", string platform = "Any CPU")
{
	PrintHeader("Begin Task: Go");	

	Clean();

	Init();

	Build(configuration, platform);

	Distro();

	Package();

	PrintFooter("End Task: Go");
}

/// <summary> 
/// Very simple demo task. See other demo tasks for more useful stuff ;)
/// </summary>
[Task] public static void Greeting()
{
	Log.Info("Hello from Nake!");
}

/// <summary> 
/// Delete all build asset folders
/// </summary>
[Task] public static void Clean()
{
	PrintHeader("Begin Task: Clean");

	try
	{
		Log.Info("Removing Folder: " + DocsPath);
		DeleteDirectory(DocsPath);

		Log.Info("Removing Folder: " + DistroPath);
		DeleteDirectory(DistroPath);

		Log.Info("Removing Folder: " + OutputPath);	
		DeleteDirectory(OutputPath);
	}
  catch (System.Exception ex) 
  {
      Log.Error(ex.Message);
			throw;
  }

	PrintFooter("End Task: Clean");
}

/// <summary> 
/// Delete all build asset folders
/// </summary>
[Task] public static void Init()
{
	PrintHeader("Begin Task: Init");
	
	Log.Info("Creating Folder: " + DocsPath);
	FS.MakeDir(DocsPath);

	Log.Info("Creating Folder: " + DistroPath);
	FS.MakeDir(DistroPath);

	Log.Info("Creating Folder: " + OutputPath);	
	FS.MakeDir(OutputPath);

	PrintFooter("End Task: Init");
}

/// <summary>
/// Builds you solution's sources  
/// </summary>
[Task] public static void Build(string configuration = "Release", string platform = "Any CPU")
{
	PrintHeader("Begin Task: Build");

	MSBuild
		.Projects(SolutionFile)
			.Property("Configuration", configuration) 
			.Property("Platform", platform)
			.Targets(new[] {"Rebuild"})
		.BuildInParallel();
			
			//.Property("OutputPath", OutputPath)
			//.Property("ReferencePath", OutputPath)

	PrintFooter("End Task: Build");
}

/// <summary>
/// Copy build output to distro folder
/// </summary>
[Task] public static void Distro()
{
	PrintHeader("Begin Task: Distro");

	try 
  {
		CopyFilesToDistro(ApplicationPath);
		CopyFilesToDistro(DemoPath);
  }
  catch (System.Exception ex) 
  {
      Log.Error(ex.Message);
  }

	PrintFooter("End Task: Distro");
}

public static void Package()
{
	PrintHeader("Begin Task: Package");

	try 
  {
		foreach (string directory in Directory.GetDirectories(DistroPath)) 
		{
			//Get nuspec file
			DirectoryInfo directoryInfo = new DirectoryInfo(directory);

			FileInfo file = directoryInfo.EnumerateFiles()
				.Where(f => ".nuspec".Contains(f.Extension.ToLower()))
				.FirstOrDefault();

			if(file == null)
			{
				continue;
			}

			string nuspec = file.FullName;
			string version = GetVersion();
			string projectName = GetProjectName(directory);
			string output = string.Format(@"{0}\{1}\{2}.{3}", PubPath, projectName, projectName, version);

			Log.Info("Creating Folder: " + output);
			FS.MakeDir(output);

			PackageProject(nuspec, version, output, directory);
		}
	}
	catch (System.Exception ex) 
	{
			Log.Error(ex.Message);
	}

	PrintFooter("End Task: Package");
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
		if(directory.EndsWith("Specs"))
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
	string defaultVersion = "1.0.0";
	return defaultVersion;
}

public static void ProcessDistroSourceFiles(string directory, string projectName)
{
	CopyNuspecFiles(directory, projectName);
	CopyLibFiles(directory, projectName);
	CopyContentFiles(directory, projectName);
}

public static void CopyNuspecFiles(string directory, string projectName)
{
	Log.Info("Processing " + projectName + " nuspec files");

	string source = directory;
					
	string dest = string.Format(@"{0}\{1}", DistroPath, projectName);

  string[] fileMatch = new[] { ".nuspec" };

	CopyFiles(source, fileMatch, dest);
}

public static void CopyLibFiles(string directory, string projectName)
{
	Log.Info("Processing " + projectName + " lib files");
	
	string source = string.Format(@"{0}\bin\{1}", directory, BuildConfig);
	Log.Info("Lib Source: " + source);
					
	string dest = string.Format(@"{0}\{1}\lib\net40", DistroPath, projectName);
	Log.Info("Lib Destination:  " + dest);

  string[] fileMatch = new[] { ".dll", ".exe", ".config", ".xml" };

	CopyFiles(source, fileMatch, dest);
}

public static void CopyContentFiles(string directory, string projectName)
{
	Log.Info("Processing " + projectName + " content files");

	string source = string.Format(@"{0}", directory);
					
	string dest = string.Format(@"{0}\{1}\content", DistroPath, projectName);

	string[] fileMatch = new[] { ".txt", ".xml", ".config" };

  CopyFiles(source, fileMatch, dest);
}

public static void Release(string version)
{
	foreach (string directory in Directory.GetDirectories(DistroPath)) 
  {
		Log.Info("Processing " + directory);

		string projectName = GetProjectName(directory);

		string packageVersion = version;
					
		string dest = string.Format(@"{0}\{1}.{2}", PubPath, projectName, packageVersion);

		string[] fileMatch = new[] { ".nupkg" };
    CopyFiles(directory, fileMatch, dest);
  }
}

public static void CopyFiles(string directory, string[] fileMatch, string  dest)
{
	if(!System.IO.Directory.Exists(directory))	
	{
		return;
	}

	DirectoryInfo directoryInfo = new DirectoryInfo(directory);

	FileInfo[] files = directoryInfo.EnumerateFiles()
		.Where(f => fileMatch.Contains(f.Extension.ToLower()))
		.ToArray();

	if(files.Length < 1)
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
	if(!System.IO.Directory.Exists(target))	
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

  foreach (string dir in dirs)
  {
      DeleteDirectory(dir);
  }

  Directory.Delete(target, true);
}

public static void PrintHeader(string message)
{
	Log.Info(string.Empty);
	Log.Info(Divider);
	Log.Info(message);
	Log.Info(Divider);
	Log.Info(string.Empty);
}

public static void PrintFooter(string message)
{
	PrintHeader(message);
}