namespace TestPipe.Specs
{
	using System;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using TestPipe.Data.Dto.TestPipeRepo;
	using TestPipe.Runner;

	[TestClass]
	[Ignore]
	public class GenericRunner
	{
		private static User user;

		[TestInitialize]
		public void Initialize()
		{
			RunnerBase.SetupFeature("1. Login");
		}

		[TestMethod]
		public void TestMethod1()
		{
			RunnerBase.SetupScenario("1. Login with valid credentials");
			user = RunnerBase.GetObject<User>();
			Assert.AreEqual("testuser", user.User_Name);
		}
	}
}