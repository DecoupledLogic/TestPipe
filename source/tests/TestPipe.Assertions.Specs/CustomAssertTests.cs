namespace TestPipe.Assertions.Specs
{
    using System;
    using System.Collections;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CustomAssertTests
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
        public void CaseInsensitiveCompare()
        {
            CustomAssert.AreEqualIgnoringCase("name", "NAME");
        }

        [TestMethod, ExpectedException(typeof(AssertFailedException))]
        public void CaseInsensitiveCompareFails()
        {
            CustomAssert.AreEqualIgnoringCase("Name", "NAMES");
        }

        [TestMethod]
        public void IsEmpty()
        {
            CustomAssert.IsEmpty(string.Empty, "Failed on empty String");
            CustomAssert.IsEmpty(new int[0], "Failed on empty Array");
            CustomAssert.IsEmpty(new ArrayList(), "Failed on empty ArrayList");
            CustomAssert.IsEmpty(new Hashtable(), "Failed on empty Hashtable");
        }

        [TestMethod, ExpectedException(typeof(AssertFailedException))]
        public void IsEmptyFailsOnNonEmptyArray()
        {
            CustomAssert.IsEmpty(new int[] { 1, 2, 3 });
        }

        [TestMethod, ExpectedException(typeof(NullReferenceException))]
        public void IsEmptyFailsOnNullString()
        {
            CustomAssert.IsEmpty((string)null);
        }

        [TestMethod, ExpectedException(typeof(AssertFailedException))]
        public void IsEmptyFailsOnString()
        {
            CustomAssert.IsEmpty("Hi!");
        }

        [TestMethod]
        public void IsNotEmpty()
        {
            int[] array = new int[] { 1, 2, 3 };
            ArrayList list = new ArrayList(array);
            Hashtable hash = new Hashtable();
            hash.Add("array", array);

            CustomAssert.IsNotEmpty("Hi!", "Failed on String");
            CustomAssert.IsNotEmpty(array, "Failed on Array");
            CustomAssert.IsNotEmpty(list, "Failed on ArrayList");
            CustomAssert.IsNotEmpty(hash, "Failed on Hashtable");
        }

        [TestMethod, ExpectedException(typeof(AssertFailedException))]
        public void IsNotEmptyFailsOnEmptyArray()
        {
            CustomAssert.IsNotEmpty(new int[0]);
        }

        [TestMethod, ExpectedException(typeof(AssertFailedException))]
        public void IsNotEmptyFailsOnEmptyArrayList()
        {
            CustomAssert.IsNotEmpty(new ArrayList());
        }

        [TestMethod, ExpectedException(typeof(AssertFailedException))]
        public void IsNotEmptyFailsOnEmptyHashTable()
        {
            CustomAssert.IsNotEmpty(new Hashtable());
        }

        [TestMethod, ExpectedException(typeof(AssertFailedException))]
        public void IsNotEmptyFailsOnEmptyString()
        {
            CustomAssert.IsNotEmpty(string.Empty);
        }
    }
}