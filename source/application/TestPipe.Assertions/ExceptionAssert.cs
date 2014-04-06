namespace TestPipe.Assertions
{
	using System;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	/// <summary>
	/// Contains assertion types that are not provided with the standard MSTest assertions.
	/// </summary>
	public static class ExceptionAssert
	{
		/// <summary>
		/// Checks to make sure that the input delegate throws a exception of type exceptionType.
		/// </summary>
		/// <typeparam name="TException">The type of exception expected.</typeparam>
		/// <param name="action">The action to execute to generate the exception.</param>
		public static void Throws<TException>(Action action) where TException : System.Exception
		{
			try
			{
				action();
			}
			catch (Exception ex)
			{
				Assert.IsInstanceOfType(ex, typeof(TException), "Expected exception was not thrown. ");
				return;
			}

			Assert.Fail("Expected exception of type " + typeof(TException) + " but no exception was thrown.");
		}

		/// <summary>
		/// Checks to make sure that the input delegate throws a exception of type exceptionType.
		/// </summary>
		/// <typeparam name="TException">The type of exception expected.</typeparam>
		/// <param name="expectedMessage">The expected message text.</param>
		/// <param name="action">The action to execute to generate the exception.</param>
		public static void Throws<TException>(string expectedMessage, Action action) where TException : System.Exception
		{
			try
			{
				action();
			}
			catch (Exception ex)
			{
				Assert.IsInstanceOfType(ex, typeof(TException), "Expected exception was not thrown. ");
				Assert.AreEqual(expectedMessage, ex.Message, "Expected exception with a message of '" + expectedMessage + "' but exception with message of '" + ex.Message + "' was thrown instead.");
				return;
			}

			Assert.Fail("Expected exception of type " + typeof(TException) + " but no exception was thrown.");
		}
	}
}