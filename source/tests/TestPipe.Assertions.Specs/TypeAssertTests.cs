﻿namespace TestPipe.Assertions.Specs
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TypeAssertTests
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
        public void IsAssignableFrom()
        {
            int[] array10 = new int[10];
            int[] array2 = new int[2];

            TypeAssert.IsAssignableFrom(array10, array2.GetType());
            TypeAssert.IsAssignableFrom(array10, array2.GetType(), "Type Failure Message");
            TypeAssert.IsAssignableFrom(array10, array2.GetType(), "Type Failure Message", null);
        }

        [TestMethod]
        public void IsAssignableFromFails()
        {
            int[] array10 = new int[10];
            int[,] array2 = new int[2, 2];

            TypeAssert.IsNotAssignableFrom(array10, array2.GetType());
        }

        [TestMethod]
        public void IsNotAssignableFrom()
        {
            int[] array10 = new int[10];
            int[,] array2 = new int[2, 2];

            TypeAssert.IsNotAssignableFrom(array10, array2.GetType());
            TypeAssert.IsNotAssignableFrom(array10, array2.GetType(), "Type Failure Message");
            TypeAssert.IsNotAssignableFrom(array10, array2.GetType(), "Type Failure Message", null);
        }

        [TestMethod, ExpectedException(typeof(AssertFailedException))]
        public void IsNotAssignableFromFails()
        {
            int[] array10 = new int[10];
            int[] array2 = new int[2];

            TypeAssert.IsNotAssignableFrom(array10, array2.GetType());
        }
    }
}