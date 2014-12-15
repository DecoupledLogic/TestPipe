TestPipe Build
==============

We are using Nake as the automation engine to write build scripts. Below is a 
description of the various build tasks used to build TestPipe.

Go
--

The default build task. Used to kick of builds. It calls 5 other tasks to 
actually build and package TestPipe.

* Clean
* Init
* Build
* Distro
* Package

Call this task with: 

`{TestPipe Solution Directory}> nake`

Set the version of the package

`{TestPipe Solution Directory}> nake GO "3.1.0"`


Clean
-----------------------------------
Deletes build artifact folders from previous builds. 

Call this task with: 

`{TestPipe Solution Directory}> nake clean`

Init
-----------------------------------
Creates build artifact folders.

Call this task with: 

`{TestPipe Solution Directory}> nake init`

Build
-----------------------------------
Builds TestPipe projects and copies output to \Build folder.

Call this task with: 

`{TestPipe Solution Directory}> nake build`

Distro
-----------------------------------

Package
-----------------------------------
