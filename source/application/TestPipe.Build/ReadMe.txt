To Update NuGet Package

- Copy previous package to a new folder with the new version number as suffix on the folder name. Version number follow Semantic Versioning 2.0.0 (semver.org). For prerelease packages add a release tag to the patch number (e.g. 1.0.2-beta):
     - ci - continuous integration build
		 - beta - package for testing
		 - rc - release candidate 
- Update changelog.txt to add the changes in the new package.
- Update .nuspec file to:
     - Update Version
		 - Update Release Notes
		 - Make additional changes to package as necessary.
- Generate new .nupkg from .nuspec

Currently using NuGet Package Explorer (http://npe.codeplex.com/) to manage package data and generate .nupkg and .nuspec files.