namespace TestPipe.Assertions.Specs
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ExceptionAssertTests
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
        public void Throws()
        {
            ExceptionAssert.Throws<ArgumentException>("Test message", () => { throw new ArgumentException("Test message"); });
        }

        [TestMethod, ExpectedException(typeof(AssertFailedException))]
        public void Throws1()
        {
            ExceptionAssert.Throws<ArgumentException>("Test", () => { new ArgumentException("Test message"); });
        }
    }
}