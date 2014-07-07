namespace TestPipe.SpecFlow.Specs
{
	using System;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using TestPipe.Core;
	using TestPipe.Core.Enums;
	using TestPipe.Runner;

	[TestClass]
	public class RunnerBaseSpec
	{
		private TestContext testContextInstance;

		public RunnerBaseSpec()
		{
		}

		public TestContext TestContext
		{
			get
			{
				return this.testContextInstance;
			}
			set
			{
				this.testContextInstance = value;
			}
		}		
	}
}