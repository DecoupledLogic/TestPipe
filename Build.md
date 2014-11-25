Build
=====

We use nake to build TestPipe. There is a build file in the root solution directory.
To build TestPipe and produce the NuGet packages run:

    {RootSolutionPath}>nake Go

This will create a Build folder that contains the build artifacts. This will use
the default build configuration defined in the TestPipe.sln solution.

To create versioned artifacts:

    {RootSolutionPath}> nake Go "3.0.0.0"

This will version the artifacts 3.0.0.0.

NuGet Packages
--------------

{RootSolutionPath}\Build\Pub is the path to the NuGet packages. Publish these packages on 
the NuGet server. We use the public gallery and a private hosted NuGet server.