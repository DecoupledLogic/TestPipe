namespace TestPipe.Assertions
{
    using System;
    using System.Collections;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Contains assertion types that are not provided with the standard MSTest assertions.
    /// </summary>
    public static class CustomAssert
    {
        #region AreEqualIgnoringCase

        #region AreEqualIgnoringCase(string expected, string actual)

        /// <summary>
        /// Asserts that two strings are equal, without regard to case.
        /// </summary>
        /// <param name="expected">The expected string.</param>
        /// <param name="actual">The actual string.</param>
        public static void AreEqualIgnoringCase(string expected, string actual)
        {
            AreEqualIgnoringCase(expected, actual, Properties.Resources.Assertion_GenericFailure, expected, actual);
        }

        #endregion AreEqualIgnoringCase(string expected, string actual)

        #region AreEqualIgnoringCase(string expected, string actual, string message)

        /// <summary>
        /// Asserts that two strings are equal, without regard to case.
        /// </summary>
        /// <param name="expected">The expected string.</param>
        /// <param name="actual">The actual string.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        public static void AreEqualIgnoringCase(string expected, string actual, string message)
        {
            AreEqualIgnoringCase(expected, actual, message, null);
        }

        #endregion AreEqualIgnoringCase(string expected, string actual, string message)

        #region AreEqualIgnoringCase(string expected, string actual, string message, params object[] parameters)

        /// <summary>
        /// Asserts that two strings are equal, without regard to case.
        /// </summary>
        /// <param name="expected">The expected string.</param>
        /// <param name="actual">The actual string.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="parameters">An array of parameters to use when formatting <paramref name="message"/>.</param>
        public static void AreEqualIgnoringCase(string expected, string actual, string message, params object[] parameters)
        {
            Assert.IsTrue(string.Compare(expected, actual, StringComparison.CurrentCultureIgnoreCase) == 0, message, parameters);
        }

        #endregion AreEqualIgnoringCase(string expected, string actual, string message, params object[] parameters)

        #endregion AreEqualIgnoringCase

        #region IsEmpty

        #region IsEmpty(ICollection collection)

        /// <summary>
        /// Assert that an array, list or other collection is empty.
        /// </summary>
        /// <param name="collection">The value to be tested.</param>
        public static void IsEmpty(ICollection collection)
        {
            IsEmpty(collection, Properties.Resources.Assertion_CollectionFailure, 0, collection.Count);
        }

        #endregion IsEmpty(ICollection collection)

        #region IsEmpty(ICollection collection, string message)

        /// <summary>
        /// Assert that an array, list or other collection is empty.
        /// </summary>
        /// <param name="collection">The value to be tested.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        public static void IsEmpty(ICollection collection, string message)
        {
            IsEmpty(collection, message, null);
        }

        #endregion IsEmpty(ICollection collection, string message)

        #region IsEmpty(ICollection collection, string message, params object[] parameters)

        /// <summary>
        /// Assert that an array, list or other collection is empty.
        /// </summary>
        /// <param name="collection">The value to be tested.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="parameters">An array of parameters to use when formatting <paramref name="message"/>.</param>
        public static void IsEmpty(ICollection collection, string message, params object[] parameters)
        {
            Assert.IsTrue(collection.Count == 0, message, parameters);
        }

        #endregion IsEmpty(ICollection collection, string message, params object[] parameters)

        #region IsEmpty(string value)

        /// <summary>
        /// Asserts that a string is empty.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        public static void IsEmpty(string value)
        {
            IsEmpty(value, Properties.Resources.Assertion_GenericFailure, string.Empty, value);
        }

        #endregion IsEmpty(string value)

        #region IsEmpty(string value, string message)

        /// <summary>
        /// Asserts that a string is empty.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        public static void IsEmpty(string value, string message)
        {
            IsEmpty(value, message, null);
        }

        #endregion IsEmpty(string value, string message)

        #region IsEmpty(string value, string message, params object[] parameters)

        /// <summary>
        /// Asserts that a string is empty.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="parameters">An array of parameters to use when formatting <paramref name="message"/>.</param>
        public static void IsEmpty(string value, string message, params object[] parameters)
        {
            Assert.IsTrue(value.Length == 0, message, parameters);
        }

        #endregion IsEmpty(string value, string message, params object[] parameters)

        #endregion IsEmpty

        #region IsNotEmpty

        #region IsNotEmpty(ICollection collection)

        /// <summary>
        /// Assert that an array, list or other collection is not empty.
        /// </summary>
        /// <param name="collection">The value to be tested.</param>
        public static void IsNotEmpty(ICollection collection)
        {
            IsNotEmpty(collection, Properties.Resources.Assertion_CollectionFailure, collection.Count, 0);
        }

        #endregion IsNotEmpty(ICollection collection)

        #region IsNotEmpty(ICollection collection, string message)

        /// <summary>
        /// Assert that an array, list or other collection is not empty.
        /// </summary>
        /// <param name="collection">The value to be tested.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        public static void IsNotEmpty(ICollection collection, string message)
        {
            IsNotEmpty(collection, message, null);
        }

        #endregion IsNotEmpty(ICollection collection, string message)

        #region IsNotEmpty(ICollection collection, string message, params object[] parameters)

        /// <summary>
        /// Assert that an array, list or other collection is not empty.
        /// </summary>
        /// <param name="collection">The value to be tested.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="parameters">An array of parameters to use when formatting <paramref name="message"/>.</param>
        public static void IsNotEmpty(ICollection collection, string message, params object[] parameters)
        {
            Assert.IsFalse(collection.Count == 0, message, parameters);
        }

        #endregion IsNotEmpty(ICollection collection, string message, params object[] parameters)

        #region IsNotEmpty(string value)

        /// <summary>
        /// Asserts that a string is not empty.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        public static void IsNotEmpty(string value)
        {
            IsNotEmpty(value, Properties.Resources.Assertion_GenericFailure, value, string.Empty);
        }

        #endregion IsNotEmpty(string value)

        #region IsNotEmpty(string value, string message)

        /// <summary>
        /// Asserts that a string is not empty.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        public static void IsNotEmpty(string value, string message)
        {
            IsNotEmpty(value, message, null);
        }

        #endregion IsNotEmpty(string value, string message)

        #region IsNotEmpty(string value, string message, params object[] parameters)

        /// <summary>
        /// Asserts that a string is not empty.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="parameters">An array of parameters to use when formatting <paramref name="message"/>.</param>
        public static void IsNotEmpty(string value, string message, params object[] parameters)
        {
            Assert.IsFalse(value.Length == 0, message, parameters);
        }

        #endregion IsNotEmpty(string value, string message, params object[] parameters)

        #endregion IsNotEmpty
    }
}