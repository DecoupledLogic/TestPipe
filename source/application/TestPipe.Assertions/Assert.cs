namespace TestPipe.Assertions
{
	using System;
	using System.Collections;
	using TestPipe.Core.Exceptions;

	public static class Asserts
	{
		public static void AssignableFrom(object arg1, Type arg2, string message = "", params object[] parameters)
		{
			string reason = string.Format("{0} is not assignable from {1}.", arg1, arg2);

			string failMessage = GetFailMessage(reason, message);

			Asserts.IsTrue(arg1.GetType().IsAssignableFrom(arg2), failMessage, parameters);
		}

		public static void Empty(string arg1, string message = "", params object[] parameters)
		{
			string reason = string.Format("{0} is not an empty string.", arg1);

			string failMessage = GetFailMessage(reason, message);

			Asserts.IsTrue(arg1.Length == 0, failMessage, parameters);
		}

		public static void Empty(object[] arg1, string message = "", params object[] parameters)
		{
			string reason = string.Format("{0} is not an empty string.", arg1);

			string failMessage = GetFailMessage(reason, message);

			Asserts.IsTrue(arg1.Length == 0, failMessage, parameters);
		}

		public static void Empty(ICollection arg1, string message = "", params object[] parameters)
		{
			string reason = string.Format("{0} is not an empty collection.", arg1);

			string failMessage = GetFailMessage(reason, message);

			Asserts.IsTrue(arg1.Count == 0, failMessage, parameters);
		}

		public static void Equal(object arg1, object arg2, string message = "", params object[] parameters)
		{
			string reason = string.Format("{0} is less than or equal to {1}.", arg1, arg2);

			string failMessage = GetFailMessage(reason, message);

			Asserts.IsTrue(arg1.Equals(arg2), failMessage, parameters);
		}

		public static void Equal(string arg1, string arg2, bool ignoreCase = false, string message = "", params object[] parameters)
		{
			string reason = string.Format("{0} is not equal to {1}.", arg1, arg2);

			string failMessage = GetFailMessage(reason, message);

			if (ignoreCase)
			{
				bool result = string.Compare(arg1, arg2, StringComparison.CurrentCultureIgnoreCase) == 0;
				Asserts.IsTrue(result, failMessage, parameters);
				return;
			}

			Asserts.IsTrue(string.Compare(arg1, arg2) == 0, failMessage, parameters);
		}

		public static void Equal<T>(T arg1, T arg2, string message = "", params object[] parameters) where T : IComparable
		{
			string reason = string.Format("{0} is not equal to {1}.", arg1, arg2);

			string failMessage = GetFailMessage(reason, message);

			Asserts.IsTrue(((IComparable)arg1).CompareTo(arg2) == 0, failMessage, parameters);
		}

		public static void Fail(string message = "", params object[] parameters)
		{
			if (string.IsNullOrEmpty(message))
			{
				message = "Assert bombed with unknown reason.";
			}

			throw new AssertBombedException(message, parameters);
		}

		public static void Greater<T>(T arg1, T arg2, string message = "", params object[] parameters) where T : IComparable
		{
			string reason = string.Format("{0} is not greater than {1}.", arg1, arg2);

			string failMessage = GetFailMessage(reason, message);

			Asserts.IsTrue(((IComparable)arg1).CompareTo(arg2) > 0, failMessage, parameters);
		}

		public static void GreaterOrEqual<T>(T arg1, T arg2, string message = "", params object[] parameters) where T : IComparable
		{
			string reason = string.Format("{0} is not greater than or equal to {1}.", arg1, arg2);

			string failMessage = GetFailMessage(reason, message);

			Asserts.IsTrue(((IComparable)arg1).CompareTo(arg2) >= 0, failMessage, parameters);
		}

		public static void Ignored()
		{
			throw new IgnoreException();
		}

		public static void InstanceOfType(object arg1, Type arg2, string message = "", params object[] parameters)
		{
			string reason = string.Format("{0} is not an instance of Type {1}.", arg1, arg2);

			string failMessage = GetFailMessage(reason, message);

			Asserts.IsTrue(arg2.IsInstanceOfType(arg1), failMessage, parameters);
		}

		public static void IsFalse(bool condition, string message = "", params object[] parameters)
		{
			if (!condition)
			{
				return;
			}

			string failMessage = GetFailMessage("Expected false. Actual true.", message);

			Asserts.Fail(failMessage, parameters);
		}

		public static void IsTrue(bool condition, string message = "", params object[] parameters)
		{
			if (condition)
			{
				return;
			}

			string failMessage = "Expected true. Actual false.";

			if (!string.IsNullOrWhiteSpace(message))
			{
				failMessage = message;
			}

			Asserts.Fail(failMessage, parameters);
		}

		public static void Less<T>(T arg1, T arg2, string message = "", params object[] parameters) where T : IComparable
		{
			string reason = string.Format("{0} is not less than {1}.", arg1, arg2);

			string failMessage = GetFailMessage(reason, message);

			Asserts.IsTrue(((IComparable)arg1).CompareTo(arg2) < 0, failMessage, parameters);
		}

		public static void LessOrEqual<T>(T arg1, T arg2, string message = "", params object[] parameters) where T : IComparable
		{
			string reason = string.Format("{0} is not less than or equal to {1}.", arg1, arg2);

			string failMessage = GetFailMessage(reason, message);

			Asserts.IsTrue(((IComparable)arg1).CompareTo(arg2) <= 0, failMessage, parameters);
		}

		public static void Manual()
		{
			throw new ManualOnlyException();
		}

		public static void NaN(double value, string message = "", params object[] parameters)
		{
			string reason = string.Format("Expected {0} but was {1}.", double.NaN, value);

			string failMessage = GetFailMessage(reason, message);

			Asserts.IsTrue(double.IsNaN(value), failMessage, parameters);
		}

		public static void NotAssignableFrom(object arg1, Type arg2, string message = "", params object[] parameters)
		{
			string reason = string.Format("{0} is assignable from {1}.", arg1, arg2);

			string failMessage = GetFailMessage(reason, message);

			Asserts.IsFalse(arg1.GetType().IsAssignableFrom(arg2), failMessage, parameters);
		}

		public static void NotEmpty(string arg1, string message = "", params object[] parameters)
		{
			string reason = string.Format("{0} is an empty string.", arg1);

			string failMessage = GetFailMessage(reason, message);

			Asserts.IsTrue(arg1.Length > 0, failMessage, parameters);
		}

		public static void NotEmpty(ICollection arg1, string message = "", params object[] parameters)
		{
			string reason = string.Format("{0} is an empty collection.", arg1);

			string failMessage = GetFailMessage(reason, message);

			Asserts.IsTrue(arg1.Count > 0, failMessage, parameters);
		}

		public static void NotEqual<T>(T arg1, T arg2, string message = "", params object[] parameters) where T : IComparable
		{
			string reason = string.Format("{0} is equal to {1}.", arg1, arg2);

			string failMessage = GetFailMessage(reason, message);

			Asserts.IsTrue(((IComparable)arg1).CompareTo(arg2) != 0, failMessage, parameters);
		}

		public static void NotInstanceOfType(object arg1, Type arg2, string message = "", params object[] parameters)
		{
			string reason = string.Format("{0} is an instance of Type {1}.", arg1, arg2);

			string failMessage = GetFailMessage(reason, message);

			Asserts.IsFalse(arg2.IsInstanceOfType(arg1), failMessage, parameters);
		}

		public static void NotNaN(double value, string message = "", params object[] parameters)
		{
			string reason = string.Format("Expected {0} but was {1}.", double.NaN, value);

			string failMessage = GetFailMessage(reason, message);

			Asserts.IsTrue(!double.IsNaN(value), failMessage, parameters);
		}

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
				Asserts.InstanceOfType(ex, typeof(TException), "Expected exception was not thrown. ");
				return;
			}

			Asserts.Fail("Expected exception of type " + typeof(TException) + " but no exception was thrown.");
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
				Asserts.InstanceOfType(ex, typeof(TException), "Expected exception was not thrown. ");
				Asserts.Equal(expectedMessage, ex.Message, "Expected exception with a message of '" + expectedMessage + "' but exception with message of '" + ex.Message + "' was thrown instead.");
				return;
			}

			Asserts.Fail("Expected exception of type " + typeof(TException) + " but no exception was thrown.");
		}

		private static string GetFailMessage(string message, string message2 = "")
		{
			if (string.IsNullOrEmpty(message2))
			{
				return message;
			}

			return string.Format("{0} {1}", message, message2);
		}

		public static void Null(object arg1, string message = "", params object[] parameters)
		{
			string reason = string.Format("{0} is not null.", arg1);

			string failMessage = GetFailMessage(reason, message);

			Asserts.IsTrue(arg1 == null, failMessage, parameters);
		}

		public static void NotNull(object arg1, string message = "", params object[] parameters)
		{
			string reason = string.Format("{0} is null.", arg1);

			string failMessage = GetFailMessage(reason, message);

			Asserts.IsFalse(arg1 != null, failMessage, parameters);
		}
	}
}