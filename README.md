TestPipe
--------

![TestPipe Logo](https://github.com/charleslbryant/TestPipe/blob/master/creative/logo/logo.png?raw=true)

TestPipe is an automated Test Framework written with C#. The primary focus of the framework is 
to provide a maintainable means of creating test for HTTP web applications and
services.

Current Version
---------------

The current version is 1.0.0. Although, it isn't indicated by the version number,
this is a preview and hasn't been fully tested against a production application.
So use at your own risk.

Download
--------

TestPipe source code is hosted at https://github.com/charleslbryant/TestPipe. 

TestPipe binaries are hosted on Nuget.org. We currently host the binaries as 
separate packages, but may simplify this to one package if we find it easier as
we finish the implementation of TestPipe in an enterprise development environment.

These are the packages hosted on NuGet:

- https://www.nuget.org/packages/TestPipe.Assertions/TestPipe.Assertions
  This project provides an API to implement custom assertions
  and an implementation based on MSTest.
- https://www.nuget.org/packages/TestPipe.Common/
  TestPipe.Common - Various cross cutting concerns like  logging, exceptions, and 
  shared functionality.
- https://www.nuget.org/packages/TestPipe.Core/
  TestPipe.Core - This project houses the main domain logic for TestPipe.
- https://www.nuget.org/packages/TestPipe.Data/
  TestPipe.Data - Management for test data, seeding databases, and general data
  access.
- https://www.nuget.org/packages/TestPipe.NPoco/
  TestPipe.NPoco - A custom implementation of the awesome NPoco micro ORM.
- https://www.nuget.org/packages/TestPipe.Runner/
  TestPipe.Runner - This provides the logic for running tests and the glue that
  the parts of TestPipe together.
- https://www.nuget.org/packages/TestPipe.Selenium/
  TestPipe.Selenium - This is a plug-in that provides a Selenium WebDriver 
  implementation of TestPipe IBrowser interface.
- https://www.nuget.org/packages/TestPipe.SpecFlow/
  TestPipe.SpecFlow - SpecFlow provides the framework for writing test in TestPipe.

Installation
------------

To install TestPipe you can install the NuGet packages or download the source code and build and
reference the binaries.

Usage
-----

You can see the Demo projects for an example of how to use TestPipe. Further information on usage
can be found in the documentation - https://github.com/charleslbryant/TestPipe/wiki.

Documentation
-------------

You can find documentation on the TestPipe wiki - https://github.com/charleslbryant/TestPipe/wiki.

Contributing
------------

If you are interested in contributing, use your GitHub account to fork the main repository, https://github.com/charleslbryant/TestPipe,
make your changes and submit a pull request (http://help.github.com/pull-requests/).

Contact
-------

If you have any questions, requests, comments, or criticisms, submit a ticket on the TestPipe issue 
tracker - https://github.com/charleslbryant/TestPipe/issues.

License
-------

TestPipe is released under GPL v3 and the license can be found in the file named
license - https://github.com/charleslbryant/TestPipe/blob/master/license.

Copyright
---------

Copyright © 2014 Charles Bryant
