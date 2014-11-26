namespace TestPipe.Build
{
	using System;

	public class Package
	{
		public bool DevelopmentDependency { get; set; }

		public string Framework { get; set; }

		public string Name { get; set; }

		public string Version { get; set; }
	}
}