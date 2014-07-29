namespace TestPipe.Assertions.Specs
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using TestPipe.Core.Exceptions;

	[TestClass]
	public class ConditionAsertLessTests
	{
		private readonly double d1 = 4.85948654;
		private readonly double d2 = 8.0;
		private readonly decimal de1 = 53.4M;
		private readonly decimal de2 = 83.4M;
		private readonly System.Enum e1 = System.Data.CommandType.StoredProcedure;
		private readonly System.Enum e2 = System.Data.CommandType.TableDirect;
		private readonly float f1 = 3.543F;
		private readonly float f2 = 8.543F;
		private readonly int i1 = 5;
		private readonly int i2 = 8;
		private readonly uint u1 = 12345678;
		private readonly uint u2 = 12345879;
		private TestContext testContextInstance;

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

		[TestMethod]
		public void FailureMessage()
		{
			string msg = null;

			try
			{
				Asserts.Less(9, 4);
			}
			catch (AssertBombedException ex)
			{
				msg = ex.Message;
			}

			StringAssert.Contains("9 is not less than 4.", msg);
		}

		[TestMethod]
		public void Less()
		{
			// Testing all forms after seeing some bugs. CFP
			Asserts.Less(this.i1, this.i2);
			Asserts.Less(this.i1, this.i2, "int");
			Asserts.Less(this.i1, this.i2, "{0}", "int");
			Asserts.Less(this.u1, this.u2, "uint");
			Asserts.Less(this.u1, this.u2, "{0}", "uint");
			Asserts.Less(this.d1, this.d2);
			Asserts.Less(this.d1, this.d2, "double");
			Asserts.Less(this.d1, this.d2, "{0}", "double");
			Asserts.Less(this.de1, this.de2);
			Asserts.Less(this.de1, this.de2, "decimal");
			Asserts.Less(this.de1, this.de2, "{0}", "decimal");
			Asserts.Less(this.f1, this.f2);
			Asserts.Less(this.f1, this.f2, "float");
			Asserts.Less(this.f1, this.f2, "{0}", "float");
		}

		[TestMethod, ExpectedException(typeof(AssertBombedException))]
		public void NotLess()
		{
			Asserts.Less(this.i2, this.i1);
		}

		[TestMethod, ExpectedException(typeof(AssertBombedException))]
		public void NotLessIComparable()
		{
			Asserts.Less(this.e2, this.e1);
		}

		[TestMethod, ExpectedException(typeof(AssertBombedException))]
		public void NotLessWhenEqual()
		{
			Asserts.Less(this.i1, this.i1);
		}
	}
}