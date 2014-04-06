namespace TestPipe.Assertions.Specs
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ConditionAssertTests
    {
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
        public void IsNaN()
        {
            ConditionAssert.IsNaN(double.NaN);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void IsNaNFails()
        {
            ConditionAssert.IsNaN(10.0);
        }
    }
}