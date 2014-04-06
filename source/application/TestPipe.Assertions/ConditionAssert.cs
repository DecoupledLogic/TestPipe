namespace TestPipe.Assertions
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public static class ConditionAssert
    {
        /// <summary>
        /// Verifies that the first value is greater than the second value.
        /// </summary>
        /// <typeparam name="T">The type of the values to compare.</typeparam>
        /// <param name="arg1">The first value, expected to be greater.</param>
        /// <param name="arg2">The second value, expected to be less.</param>
        public static void Greater<T>(T arg1, T arg2) where T : IComparable
        {
            Greater(arg1, arg2, "{0} is less than or equal to {1}.", arg2, arg1);
        }

        /// <summary>
        /// Verifies that the first value is greater than the second value.
        /// </summary>
        /// <typeparam name="T">The type of the values to compare.</typeparam>
        /// <param name="arg1">The first value, expected to be greater.</param>
        /// <param name="arg2">The second value, expected to be less.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        public static void Greater<T>(T arg1, T arg2, string message) where T : IComparable
        {
            if (((IComparable)arg1).CompareTo(arg2) <= 0)
            {
                Assert.Fail(message);
            }
        }

        /// <summary>
        /// Verifies that the first value is greater than the second value.
        /// </summary>
        /// <typeparam name="T">The type of the values to compare.</typeparam>
        /// <param name="arg1">The first value, expected to be greater.</param>
        /// <param name="arg2">The second value, expected to be less.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="parameters">An array of parameters to use when formatting <paramref name="message"/>.</param>
        public static void Greater<T>(T arg1, T arg2, string message, params object[] parameters) where T : IComparable
        {
            if (((IComparable)arg1).CompareTo(arg2) <= 0)
            {
                Assert.Fail(message, parameters);
            }
        }

        /// <summary>
        /// Verifies that the first value is greater than or equal to the second value.
        /// </summary>
        /// <typeparam name="T">The type of the values to compare.</typeparam>
        /// <param name="arg1">The first value, expected to be greater.</param>
        /// <param name="arg2">The second value, expected to be less.</param>
        public static void GreaterOrEqual<T>(T arg1, T arg2) where T : IComparable
        {
            GreaterOrEqual(arg1, arg2, "{0} is less than {1}.", arg2, arg1);
        }

        /// <summary>
        /// Verifies that the first value is greater than or equal to the second value.
        /// </summary>
        /// <typeparam name="T">The type of the values to compare.</typeparam>
        /// <param name="arg1">The first value, expected to be greater.</param>
        /// <param name="arg2">The second value, expected to be less.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        public static void GreaterOrEqual<T>(T arg1, T arg2, string message) where T : IComparable
        {
            if (((IComparable)arg1).CompareTo(arg2) < 0)
            {
                Assert.Fail(message);
            }
        }

        /// <summary>
        /// Verifies that the first value is greater than or equal to the second value.
        /// </summary>
        /// <typeparam name="T">The type of the values to compare.</typeparam>
        /// <param name="arg1">The first value, expected to be greater.</param>
        /// <param name="arg2">The second value, expected to be less.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="parameters">An array of parameters to use when formatting <paramref name="message"/>.</param>
        public static void GreaterOrEqual<T>(T arg1, T arg2, string message, params object[] parameters) where T : IComparable
        {
            if (((IComparable)arg1).CompareTo(arg2) < 0)
            {
                Assert.Fail(message, parameters);
            }
        }

        /// <summary>
        /// Verifies that the value is <see cref="Double.NaN"/>.
        /// </summary>
        /// <param name="value">The value to test.</param>
        public static void IsNaN(double value)
        {
            IsNaN(value, "Expected {0} but was {1}", double.NaN, value);
        }

        /// <summary>
        /// Verifies that the value is <see cref="double.NaN"/>.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        public static void IsNaN(double value, string message)
        {
            IsNaN(value, message, null);
        }

        /// <summary>
        /// Verifies that the value is <see cref="double.NaN"/>.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="parameters">An array of parameters to use when formatting <paramref name="message"/>.</param>
        public static void IsNaN(double value, string message, params object[] parameters)
        {
            if (!double.IsNaN(value))
            {
                Assert.Fail(message, parameters);
            }
        }

        /// <summary>
        /// Verifies that the first value is less than the second value.
        /// </summary>
        /// <typeparam name="T">The type of the values to compare.</typeparam>
        /// <param name="arg1">The first value, expected to be less.</param>
        /// <param name="arg2">The second value, expected to be greater.</param>
        public static void Less<T>(T arg1, T arg2) where T : IComparable
        {
            Less(arg1, arg2, "{0} is greater than or equal to {1}.", arg2, arg1);
        }

        /// <summary>
        /// Verifies that the first value is less than the second value.
        /// </summary>
        /// <typeparam name="T">The type of the values to compare.</typeparam>
        /// <param name="arg1">The first value, expected to be less.</param>
        /// <param name="arg2">The second value, expected to be greater.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        public static void Less<T>(T arg1, T arg2, string message) where T : IComparable
        {
            if (((IComparable)arg1).CompareTo(arg2) >= 0)
            {
                Assert.Fail(message);
            }
        }

        /// <summary>
        /// Verifies that the first value is less than the second value.
        /// </summary>
        /// <typeparam name="T">The type of the values to compare.</typeparam>
        /// <param name="arg1">The first value, expected to be less.</param>
        /// <param name="arg2">The second value, expected to be greater.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="parameters">An array of parameters to use when formatting <paramref name="message"/>.</param>
        public static void Less<T>(T arg1, T arg2, string message, params object[] parameters) where T : IComparable
        {
            if (((IComparable)arg1).CompareTo(arg2) >= 0)
            {
                Assert.Fail(message, parameters);
            }
        }

        /// <summary>
        /// Verifies that the first value is less than or equal to the second value.
        /// </summary>
        /// <typeparam name="T">The type of the values to compare.</typeparam>
        /// <param name="arg1">The first value, expected to be less.</param>
        /// <param name="arg2">The second value, expected to be greater.</param>
        public static void LessOrEqual<T>(T arg1, T arg2) where T : IComparable
        {
            LessOrEqual(arg1, arg2, "{0} is greater than {1}.", arg2, arg1);
        }

        /// <summary>
        /// Verifies that the first value is less than or equal to the second value.
        /// </summary>
        /// <typeparam name="T">The type of the values to compare.</typeparam>
        /// <param name="arg1">The first value, expected to be less.</param>
        /// <param name="arg2">The second value, expected to be greater.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        public static void LessOrEqual<T>(T arg1, T arg2, string message) where T : IComparable
        {
            if (((IComparable)arg1).CompareTo(arg2) > 0)
            {
                Assert.Fail(message);
            }
        }

        /// <summary>
        /// Verifies that the first value is less than or equal to the second value.
        /// </summary>
        /// <typeparam name="T">The type of the values to compare.</typeparam>
        /// <param name="arg1">The first value, expected to be less.</param>
        /// <param name="arg2">The second value, expected to be greater.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="parameters">An array of parameters to use when formatting <paramref name="message"/>.</param>
        public static void LessOrEqual<T>(T arg1, T arg2, string message, params object[] parameters) where T : IComparable
        {
            if (((IComparable)arg1).CompareTo(arg2) > 0)
            {
                Assert.Fail(message, parameters);
            }
        }
    }
}