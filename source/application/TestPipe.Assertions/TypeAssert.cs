﻿namespace TestPipe.Assertions
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Contains assertion types that are not provided with the standard MSTest assertions.
    /// </summary>
    public static class TypeAssert
    {
        #region IsAssignableFrom

        #region IsAssignableFrom(object value, Type expectedType)

        /// <summary>
        /// Asserts that an object may be assigned a value of a given <see cref="Type"/>.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <param name="expectedType">The expected <see cref="Type"/>.</param>
        public static void IsAssignableFrom(object value, Type expectedType)
        {
            IsAssignableFrom(value, expectedType, "Expected {0} to be assignable from {1}", value, expectedType);
        }

        #endregion IsAssignableFrom(object value, Type expectedType)

        #region IsAssignableFrom(object value, Type expectedType, string message)

        /// <summary>
        /// Asserts that an object may be assigned a value of a given <see cref="Type"/>.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <param name="expectedType">The expected <see cref="Type"/>.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        public static void IsAssignableFrom(object value, Type expectedType, string message)
        {
            IsAssignableFrom(value, expectedType, message, null);
        }

        #endregion IsAssignableFrom(object value, Type expectedType, string message)

        #region IsAssignableFrom(object value, Type expectedType, string message, params object[] parameters)

        /// <summary>
        /// Asserts that an object may be assigned a value of a given <see cref="Type"/>.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <param name="expectedType">The expected <see cref="Type"/>.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="parameters">An array of parameters to use when formatting <paramref name="message"/>.</param>
        public static void IsAssignableFrom(object value, Type expectedType, string message, params object[] parameters)
        {
            if (!value.GetType().IsAssignableFrom(expectedType))
            {
                Assert.Fail(message, parameters);
            }
        }

        #endregion IsAssignableFrom(object value, Type expectedType, string message, params object[] parameters)

        #endregion IsAssignableFrom

        #region IsNotAssignableFrom

        #region IsNotAssignableFrom(object value, Type expectedType)

        /// <summary>
        /// Asserts that an object may not be assigned a value of a given <see cref="Type"/>.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <param name="expectedType">The expected <see cref="Type"/>.</param>
        public static void IsNotAssignableFrom(object value, Type expectedType)
        {
            IsNotAssignableFrom(value, expectedType, "Expected {0} to be assignable from {1}", value, expectedType);
        }

        #endregion IsNotAssignableFrom(object value, Type expectedType)

        #region IsNotAssignableFrom(object value, Type expectedType, string message)

        /// <summary>
        /// Asserts that an object may not be assigned a value of a given <see cref="Type"/>.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <param name="expectedType">The expected <see cref="Type"/>.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        public static void IsNotAssignableFrom(object value, Type expectedType, string message)
        {
            IsNotAssignableFrom(value, expectedType, message, null);
        }

        #endregion IsNotAssignableFrom(object value, Type expectedType, string message)

        #region IsNotAssignableFrom(object value, Type expectedType, string message, params object[] parameters)

        /// <summary>
        /// Asserts that an object may not be assigned a value of a given <see cref="Type"/>.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <param name="expectedType">The expected <see cref="Type"/>.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="parameters">An array of parameters to use when formatting <paramref name="message"/>.</param>
        public static void IsNotAssignableFrom(object value, Type expectedType, string message, params object[] parameters)
        {
            if (value.GetType().IsAssignableFrom(expectedType))
            {
                Assert.Fail(message, parameters);
            }
        }

        #endregion IsNotAssignableFrom(object value, Type expectedType, string message, params object[] parameters)

        #endregion IsNotAssignableFrom
    }
}