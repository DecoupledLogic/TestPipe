namespace TestPipe.Assertions.Specs
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ConditionAssertLessTests
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
                ConditionAssert.Less(9, 4);
            }
            catch (AssertFailedException ex)
            {
                msg = ex.Message;
            }

            StringAssert.Contains("Assert.Fail failed. 4 is greater than or equal to 9.", msg);
        }

        [TestMethod]
        public void Less()
        {
            // Testing all forms after seeing some bugs. CFP
            ConditionAssert.Less(this.i1, this.i2);
            ConditionAssert.Less(this.i1, this.i2, "int");
            ConditionAssert.Less(this.i1, this.i2, "{0}", "int");
            ConditionAssert.Less(this.u1, this.u2, "uint");
            ConditionAssert.Less(this.u1, this.u2, "{0}", "uint");
            ConditionAssert.Less(this.d1, this.d2);
            ConditionAssert.Less(this.d1, this.d2, "double");
            ConditionAssert.Less(this.d1, this.d2, "{0}", "double");
            ConditionAssert.Less(this.de1, this.de2);
            ConditionAssert.Less(this.de1, this.de2, "decimal");
            ConditionAssert.Less(this.de1, this.de2, "{0}", "decimal");
            ConditionAssert.Less(this.f1, this.f2);
            ConditionAssert.Less(this.f1, this.f2, "float");
            ConditionAssert.Less(this.f1, this.f2, "{0}", "float");
        }

        [TestMethod, ExpectedException(typeof(AssertFailedException))]
        public void NotLess()
        {
            ConditionAssert.Less(this.i2, this.i1);
        }

        [TestMethod, ExpectedException(typeof(AssertFailedException))]
        public void NotLessIComparable()
        {
            ConditionAssert.Less(this.e2, this.e1);
        }

        [TestMethod, ExpectedException(typeof(AssertFailedException))]
        public void NotLessWhenEqual()
        {
            ConditionAssert.Less(this.i1, this.i1);
        }
    }
}