namespace TestPipe.Build.Specs
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Xml.Linq;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using TestPipe.Build;

	[TestClass]
	[DeploymentItem("packages.config")]
	[DeploymentItem("TestPipe.Template.nuspec")]
	[DeploymentItem("TestPipe.Build.Specs.nuspec")]
	public class NuspecSpecs
	{
		private Nuspec sut;

		[TestMethod]
		public void GetProjectPackages_Returns_Packages()
		{
			int expected = 3;

			ICollection<Package> actual = sut.GetProjectPackages("packages.config");

			Assert.AreEqual(expected, actual.Count);
		}

		[TestMethod]
		public void UpdateNuspec_Updates_Nuspec_File()
		{ 
			string fileName = Guid.NewGuid().ToString() + ".nuspec";
			File.Copy("TestPipe.Build.Specs.nuspec", fileName);
			XDocument doc = sut.UpdateNuspec(fileName, "TestPipe.Template.nuspec");
			Assert.IsNotNull(doc);
		}

		[TestInitialize]
		public void SetupTest()
		{
			sut = new Nuspec();
		}
	}
}