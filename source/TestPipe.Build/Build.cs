using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nake;

namespace TestPipe.Build
{
	public class Build
	{
		public static string Divider = "---------------------------------";

		public static void Alert(string message)
		{
			string log = string.Format("##########------ {0} ------##########", message);
			Log.Info(log);
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

		public static void CopyContentFiles(string directory, string projectName, string distroPath)
		{
			Log.Info("Copying " + projectName + " content files");

			string source = string.Format(@"{0}", directory);

			string dest = string.Format(@"{0}\{1}\content", distroPath, projectName);

			string[] include = new[] { ".txt", ".xml", ".config", ".htm", ".html", ".js", ".jpg", ".gif", ".png", ".css" };
			string[] exclude = new[] { "packages.config" };

			CopyFiles(source, dest, include, exclude);
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

		public static void CopyFilesToDistro(string path, string buildConfig, string distroPath, string nuGetToolsPath, string solutionRoot)
		{
			foreach (string directory in Directory.GetDirectories(path))
			{
				if (directory.EndsWith("Specs"))
				{
					continue;
				}

				Log.Info("Copying files to distribution path: " + directory);

				string projectName = GetProjectName(directory);

				ProcessDistroSourceFiles(directory, projectName, buildConfig, distroPath, nuGetToolsPath, solutionRoot);
			}
		}

		public static void CopyLibFiles(string directory, string projectName, string buildConfig, string distroPath)
		{
			Log.Info("Copying " + projectName + " lib files");

			string source = string.Format(@"{0}\bin\{1}", directory, buildConfig);
			Log.Info("Lib Source: " + source);

			string dest = string.Format(@"{0}\{1}\lib\net40", distroPath, projectName);
			Log.Info("Lib Destination:  " + dest);

			string[] include = new[] { ".dll", ".exe", ".config", ".xml", ".pdb" };

			CopyFiles(source, dest, include);
		}

		public static void CopyLicenseFiles(string directory, string projectName, string distroPath)
		{
			Log.Info("Copyng " + projectName + " license files");

			string source = directory + @"\license.txt";

			string dest = string.Format(@"{0}\{1}", distroPath, projectName);

			Log.Info(string.Format("Copying {0} to {1}", source, dest));
			FS.Copy(source, dest, true, false);
		}

		public static void CopyNuspecFiles(string directory, string projectName, string distroPath)
		{
			Log.Info("Processing " + projectName + " nuspec files");

			string source = directory;

			string dest = string.Format(@"{0}\{1}", distroPath, projectName);

			string[] include = new[] { ".nuspec" };

			CopyFiles(source, dest, include);
		}

		public static void CopyToolsFiles(string directory, string projectName, string distroPath)
		{
			Log.Info("Copying " + projectName + " NuGet tools files");

			string source = directory;

			string dest = string.Format(@"{0}\{1}\tools", distroPath, projectName);

			string[] include = new[] { ".ps1", ".psm1" };

			CopyFiles(source, dest, include);
		}

		public static void CreateDirectories(string[] directories)
		{
			foreach (var folder in directories)
			{
				Log.Info("Creating Folder: " + folder);
				FS.MakeDir(folder);
			}
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

		public static void DeleteFiles(string[] files)
		{
			foreach (string file in files)
			{
				File.SetAttributes(file, FileAttributes.Normal);
				File.Delete(file);
			}
		}

		public static string GetProjectName(string directory)
		{
			return directory.Substring(directory.LastIndexOf('\\') + 1);
		}

		public static string GetVersion(string applicationVersion)
		{
			string defaultVersion = applicationVersion;
			return defaultVersion;
		}

		public static void PackageProject(string nuspec, string version, string output, string basePath, string nuGetExe)
		{
			string pack = string.Format("{0} pack {1} -Version {2} -OutputDirectory {3} -BasePath {4} -NoPackageAnalysis -Symbols", nuGetExe, nuspec, version, output, basePath);

			Cmd.Exec(pack);
		}

		public static void PrintFooter(string message)
		{
			PrintMessage(message, "Completed");
		}

		public static void PrintHeader(string message)
		{
			PrintMessage(message, "Started");
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

		public static void ProcessDistroSourceFiles(string directory, string projectName, string buildConfig, string distroPath, string nuGetToolsPath, string solutionRoot)
		{
			CopyNuspecFiles(directory, projectName, distroPath);
			CopyLibFiles(directory, projectName, buildConfig, distroPath);
			CopyContentFiles(directory, projectName, distroPath);
			CopyToolsFiles(nuGetToolsPath, projectName, distroPath);
			CopyLicenseFiles(solutionRoot, projectName, distroPath);
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

		public static void SetLogging(string logPath = "", bool overwrite = false)
		{
			if (overwrite)
			{
				File.Delete(logPath);
			}

			//Print log to file and console.
			if (!string.IsNullOrWhiteSpace(logPath))
			{
				Log.Out = text =>
				{
					File.AppendAllText(logPath, text + Environment.NewLine);
					Console.WriteLine(text);
				};
				return;
			}

			Log.Out = text =>
			{
				Console.WriteLine(text);
			};
		}

		public static void Stage(string version, string distroPath, string pubPath)
		{
			foreach (string source in Directory.GetDirectories(distroPath))
			{
				Log.Info("Processing " + source);

				string projectName = GetProjectName(source);

				string packageVersion = version;

				string dest = string.Format(@"{0}\{1}.{2}", pubPath, projectName, packageVersion);

				string[] include = new[] { ".nupkg" };

				CopyFiles(source, dest, include);
			}
		}
	}
}