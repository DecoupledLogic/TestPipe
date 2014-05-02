TestPipe
--------

TestPipe is an automated Test Framework. The primary focus of the framework is 
the testing of HTTP web applications and services.

Current Version
---------------

The current version is 1.0.0. Although, it isn't indicated by the version number,
this is a preview and hasn't been fully tested against a production application.

Download
--------

TestPipe source code is hosted at https://github.com/charlesbyant/TestPipe. 
TestPipe binaries are hosted on Nuget.org. We currently host the binaries as 
seperate packages, but may simplify this to one package if we find it easier as
we finish the implementation of TestPipe in an enterprise development environmet.

These are the packages hosted on NuGet:


- TestPipe.Assertions - This project provides an API to implement custom assertions
  and an implementation based on MSTest.
- TestPipe.Common - Various cross cutting conserns like  logging, exceptions, and 
  shared functionality.
- TestPipe.Core - This project houses the main domain.
- TestPipe.Data - Management for test data, seeding databases, and general data
  access.
- TestPipe.NPoco - A custom implementation of the awesome NPoco micro ORM.
- TestPipe.Runner - This provides the logic for running tests and the glue that
  the parts of TestPipe together.
- TestPipe.Selenium - This is a plug-in that provides a Selenium WebDriver 
  implementation of TestPipe IBrowser interface.
- TestPipe.SpecFlow - SpecFlow provides the framework for writing test in TestPipe.

Documentation
-------------

You can find documentation on the TestPipe wiki.

License
-------

TestPipe is released under GPL v3 and the license can be found in the file named
license.

Contact
-------

If you have any questions or requests, submit a ticket on the TestPipe issue trakcer.
