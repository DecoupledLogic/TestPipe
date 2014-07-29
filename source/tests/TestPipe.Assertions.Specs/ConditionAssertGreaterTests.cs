namespace TestPipe.Assertions.Specs
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using TestPipe.Core.Exceptions;

	[TestClass]
	public class ConditionAssertGreaterTests
	{
		private readonly double d1 = 4.85948654;
		private readonly double d2 = 1.0;
		private readonly decimal de1 = 53.4M;
		private readonly decimal de2 = 33.4M;
		private readonly System.Enum e1 = System.Data.CommandType.TableDirect;
		private readonly System.Enum e2 = System.Data.CommandType.StoredProcedure;
		private readonly float f1 = 3.543F;
		private readonly float f2 = 2.543F;
		private readonly int i1 = 5;
		private readonly int i2 = 4;
		private readonly uint u1 = 12345879;
		private readonly uint u2 = 12345678;
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
				Asserts.Greater(7, 99);
			}
			catch (AssertBombedException ex)
			{
				msg = ex.Message;
			}

			StringAssert.Contains("7 is not greater than 99.", msg);
		}

		[TestMethod]
		public void Greater()
		{
			Asserts.Greater(this.i1, this.i2);
			Asserts.Greater(this.u1, this.u2);
			Asserts.Greater(this.d1, this.d2, "double");
			Asserts.Greater(this.de1, this.de2, "{0}", "decimal");
			Asserts.Greater(this.f1, this.f2, "float");
		}

		[TestMethod, ExpectedException(typeof(AssertBombedException))]
		public void NotGreater()
		{
			Asserts.Greater(this.i2, this.i1);
		}

		[TestMethod, ExpectedException(typeof(AssertBombedException))]
		public void NotGreaterIComparable()
		{
			Asserts.Greater(this.e2, this.e1);
		}

		[TestMethod, ExpectedException(typeof(AssertBombedException))]
		public void NotGreaterWhenEqual()
		{
			Asserts.Greater(this.i1, this.i1);
		}
	}
}