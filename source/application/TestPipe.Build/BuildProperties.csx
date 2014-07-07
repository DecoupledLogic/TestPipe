using Nake;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Diagnostics;

public static string ApplicationName
{
	get { return "TestPipe"; }
}
public static string CompanyName
{
	get { return "CharlesBryant.com"; }
}
public static string MajorMinorVersion
{
	get { return "1.0"; }
}
public static string ApplicationVersion
{
	get { return MajorMinorVersion + ".0"; }
}
public static string Copyright
{
	get { return "Copyright (c) 2014, " + CompanyName; }
}
public static string Trademark
{
	get { return "Trademark by " + CompanyName; }
}
public static string SolutionRoot
{
	get { return @"C:\_TestPipe"; }
}
public static string BuildPlatform
{
	get { return "Any CPU"; }
}
public static string Divider
{
	get { return "-----------------------------------"; }
}
public static string EchoMessage
{
	get { return ""; }
}
public static string MsbuildExe
{
	get { return @"C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"; }
}
public static string SolutionFile
{
	get { return SolutionRoot + @"\TestPipe.sln"; }
}
public static string BuildPath
{
	get { return SolutionRoot + @"\Build"; }
}
public static string SrcPath
{
	get { return BuildPath + @"\Src"; }
}
public static string DocsPath
{
	get { return BuildPath + @"\Docs"; }
}
public static string DistroPath
{
	get { return BuildPath + @"\Distro"; }
}
public static string PubPath
{
	get { return BuildPath + @"\Pub"; }
}
public static string OutputPath
{
	get { return BuildPath + @"\Output"; }
}
public static string SourcePath
{
	get { return SolutionRoot + @"\source"; }
}
public static string ApplicationPath
{
	get { return SourcePath + @"\application"; }
}
public static string DemoPath
{
	get { return SourcePath + @"\demo"; }
}
public static string TestPath
{
	get { return SourcePath + @"\tests"; }
}
public static string BuildConfig
{
	get { return "Debug"; }
}

[Task] public static void Default()
{
	
}

/// <summary> 
/// Delete all build asset folders
/// </summary>
[Task] public static void Clean()
{
	PrintHeader("Begin Task: Clean");

	try
	{
		Log.Info("Removing Folder: " + SrcPath);
		if(System.IO.Directory.Exists(SrcPath))
			System.IO.Directory.Delete(SrcPath, true);

		Log.Info("Removing Folder: " + DocsPath);
		if(System.IO.Directory.Exists(DocsPath))
			System.IO.Directory.Delete(DocsPath, true);

		Log.Info("Removing Folder: " + DistroPath);
		if(System.IO.Directory.Exists(DistroPath))
			System.IO.Directory.Delete(DistroPath, true);

		Log.Info("Removing Folder: " + PubPath);
		if(System.IO.Directory.Exists(PubPath))
			System.IO.Directory.Delete(PubPath, true);

		Log.Info("Removing Folder: " + OutputPath);	
		if(System.IO.Directory.Exists(OutputPath))
			System.IO.Directory.Delete(OutputPath, true);
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
	
	Log.Info("Creating Folder: " + SrcPath);
	FS.MakeDir(SrcPath);

	Log.Info("Creating Folder: " + DocsPath);
	FS.MakeDir(DocsPath);

	Log.Info("Creating Folder: " + DistroPath);
	FS.MakeDir(DistroPath);

	Log.Info("Creating Folder: " + PubPath);
	FS.MakeDir(PubPath);

	Log.Info("Creating Folder: " + OutputPath);	
	FS.MakeDir(OutputPath);

	PrintFooter("End Task: Init");
}

/// <summary>
/// Builds you solution's sources  
/// </summary>
[Task] public static void Go(string configuration = "Debug", string platform = "Any CPU")
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
/// Builds you solution's sources  
/// </summary>
[Task] public static void Build(string configuration = "Debug", string platform = "Any CPU")
{
	PrintHeader("Begin Task: Build");

	MSBuild
		.Projects(SolutionFile)
			.Property("Configuration", configuration) 
			.Property("Platform", platform)
			.Property("OutDir", OutputPath)
			.Property("ReferencePath", OutputPath)
	.Build();

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
		Log.Info("------ Copying nuspec files.");	

		//Copy nuspec from projects to distro folder
		string nuspecFile = "*.nuspec";
		CopyDirectoryFiles(ApplicationPath, nuspecFile);
		CopyDirectoryFiles(ApplicationPath + @"\NPoco\src\NPoco", nuspecFile);
		CopyDirectoryFiles(DemoPath, nuspecFile);
		CopyDirectoryFiles(TestPath, nuspecFile);

		Log.Info("------ Copying lib files.");	

		//Copy build library files to distro lib folder
		CopyLibFiles();

		Log.Info("------ Copying content files.");	

		//Copy build content files to distro content folder
		CopyContentFiles();

		//Copy build pdb files to distro src folder
  }
  catch (System.Exception ex) 
  {
      Log.Error(ex.Message);
  }

	PrintFooter("End Task: Distro");
}

public static void CopyDirectoryFiles(string searchDirectory, string fileMatch)
{
	foreach (string directory in Directory.GetDirectories(searchDirectory)) 
  {
			Log.Info("Processing " + directory);

			string projectName = directory.Substring(directory.LastIndexOf('\\') + 1);
					
			string dest = DistroPath + @"\" + projectName;

      CopyFiles(directory, fileMatch, dest);
  }
}

public static void CopyLibFiles()
{
	string buildOutput = string.Format(@"{0}\_PublishedApplications", OutputPath);

	foreach (string directory in Directory.GetDirectories(buildOutput)) 
  {
		Log.Info("Processing " + directory);

		string projectName = directory.Substring(directory.LastIndexOf('\\') + 1);
					
		string dest = string.Format(@"{0}\{1}\lib", DistroPath, projectName);

    CopyApplicationExtensionFiles(directory, dest);
  }
}

publis static void CopyApplicationExtensionFiles(string source, string dest)
{
	CopyFiles(source, "*.dll", dest);
	CopyFiles(source, "*.exe", dest);
	CopyFiles(source, "*.config", dest);
	CopyFiles(source, "*.xml", dest);
}

public static void CopyContentFiles()
{
	string buildOutput = string.Format(@"{0}\_PublishedApplications", OutputPath);

	foreach (string directory in Directory.GetDirectories(buildOutput)) 
  {
		Log.Info("Processing " + directory);

		string projectName = directory.Substring(directory.LastIndexOf('\\') + 1);
					
		string dest = string.Format(@"{0}\{1}\content", DistroPath, projectName);

    CopyFiles(directory, "*.txt", dest);
		CopyFiles(directory, "*.xml", dest);
		CopyFiles(directory, "*.config", dest);
  }
}

public static void Package()
{
	foreach (string directory in Directory.GetDirectories(DistroPath)) 
  {
		Log.Info("Processing " + directory);

		string projectName = directory.Substring(directory.LastIndexOf('\\') + 1);

		string packageVersion = "1.0.0";
					
		string dest = string.Format(@"{0}\{1}.{2}", NugetPath, projectName, packageVersion);

    CopyFiles(directory, "*.nupkg", dest);
  }
}

public static void CopyFiles(string directory, string fileMatch, string  dest)
{
	foreach (string file in Directory.GetFiles(directory, fileMatch)) 
  {
		Log.Info(string.Format("Copying {0} to {1}", file, dest));
		FS.Copy(file, dest, true, false);
  }
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