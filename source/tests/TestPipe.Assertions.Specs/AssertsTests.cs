namespace TestPipe.Assertions.Specs
{
	using System;
	using System.Collections;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using TestPipe.Core.Exceptions;

	[TestClass]
	public class AssertsTests
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
		public void AssignableFrom()
		{
			int[] array10 = new int[10];
			int[] array2 = new int[2];

			Asserts.AssignableFrom(array10, array2.GetType());
			Asserts.AssignableFrom(array10, array2.GetType(), "Type Failure Message");
			Asserts.AssignableFrom(array10, array2.GetType(), "Type Failure Message", null);
		}

		[TestMethod]
		public void CaseInsensitiveCompare()
		{
			Asserts.Equal("name", "NAME", true);
		}

		[TestMethod, ExpectedException(typeof(AssertBombedException))]
		public void CaseInsensitiveCompareFails()
		{
			Asserts.Equal("Name", "NAMES", true);
		}

		[TestMethod]
		public void Empty()
		{
			Asserts.Empty(string.Empty, "Failed on empty String");
			Asserts.Empty(new int[0], "Failed on empty Array");
			Asserts.Empty(new ArrayList(), "Failed on empty ArrayList");
			Asserts.Empty(new Hashtable(), "Failed on empty Hashtable");
		}

		[TestMethod, ExpectedException(typeof(AssertBombedException))]
		public void EmptyFailsOnNonEmptyArray()
		{
			Asserts.Empty(new int[] { 1, 2, 3 });
		}

		[TestMethod, ExpectedException(typeof(NullReferenceException))]
		public void EmptyFailsOnNullString()
		{
			Asserts.Empty((string)null);
		}

		[TestMethod, ExpectedException(typeof(AssertBombedException))]
		public void EmptyFailsOnString()
		{
			Asserts.Empty("Hi!");
		}

		[TestMethod]
		public void IsAssignableFromFails()
		{
			int[] array10 = new int[10];
			int[,] array2 = new int[2, 2];

			Asserts.NotAssignableFrom(array10, array2.GetType());
		}

		[TestMethod, ExpectedException(typeof(AssertBombedException))]
		public void IsNotAssignableFromFails()
		{
			int[] array10 = new int[10];
			int[] array2 = new int[2];

			Asserts.NotAssignableFrom(array10, array2.GetType());
		}

		[TestMethod]
		public void NaN()
		{
			Asserts.NaN(double.NaN);
		}

		[TestMethod]
		[ExpectedException(typeof(AssertBombedException))]
		public void NaNFails()
		{
			Asserts.NaN(10.0);
		}

		[TestMethod]
		public void NotAssignableFrom()
		{
			int[] array10 = new int[10];
			int[,] array2 = new int[2, 2];

			Asserts.NotAssignableFrom(array10, array2.GetType());
			Asserts.NotAssignableFrom(array10, array2.GetType(), "Type Failure Message");
			Asserts.NotAssignableFrom(array10, array2.GetType(), "Type Failure Message", null);
		}

		[TestMethod]
		public void NotEmpty()
		{
			int[] array = new int[] { 1, 2, 3 };
			ArrayList list = new ArrayList(array);
			Hashtable hash = new Hashtable();
			hash.Add("array", array);

			Asserts.NotEmpty("Hi!", "Failed on String");
			Asserts.NotEmpty(array, "Failed on Array");
			Asserts.NotEmpty(list, "Failed on ArrayList");
			Asserts.NotEmpty(hash, "Failed on Hashtable");
		}

		[TestMethod, ExpectedException(typeof(AssertBombedException))]
		public void NotEmptyFailsOnEmptyArray()
		{
			Asserts.NotEmpty(new int[0]);
		}

		[TestMethod, ExpectedException(typeof(AssertBombedException))]
		public void NotEmptyFailsOnEmptyArrayList()
		{
			Asserts.NotEmpty(new ArrayList());
		}

		[TestMethod, ExpectedException(typeof(AssertBombedException))]
		public void NotEmptyFailsOnEmptyHashTable()
		{
			Asserts.NotEmpty(new Hashtable());
		}

		[TestMethod, ExpectedException(typeof(AssertBombedException))]
		public void NotEmptyFailsOnEmptyString()
		{
			Asserts.NotEmpty(string.Empty);
		}

		[TestMethod]
		public void NotNaN()
		{
			Asserts.NotNaN(10.0);
		}

		[TestMethod]
		[ExpectedException(typeof(AssertBombedException))]
		public void NotNaNFails()
		{
			Asserts.NotNaN(double.NaN);
		}

		[TestMethod]
		public void Throws()
		{
			Asserts.Throws<ArgumentException>("Test message", () => { throw new ArgumentException("Test message"); });
		}

		[TestMethod, ExpectedException(typeof(AssertBombedException))]
		public void Throws1()
		{
			Asserts.Throws<ArgumentException>("Test", () => { new ArgumentException("Test message"); });
		}
	}
}