namespace TestPipe.Assertions.Internal.Contracts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using TestPipe.Assertions.Properties;

    /// <summary>
    /// Provides a set of methods to simplify code contract requirements.
    /// </summary>
    internal static class Requires
    {
        ////#region IsDefined
        /////// <summary>
        /////// Checks that <paramref name="value"/> is defined in <typeparamref name="TEnum"/>.
        /////// </summary>
        /////// <typeparam name="TEnum">The type of the enum being tested.</typeparam>
        /////// <param name="value">The parameter to test.</param>
        /////// <param name="parameterName">The name of the parameter being tested.</param>
        /////// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /////// <exception cref="ArgumentException">
        /////// <para><typeparamref name="TEnum"/> is not an Enum.</para>
        /////// <para>-or-</para>
        /////// <para>The type of <paramref name="value"/> is not an <typeparamref name="TEnum"/>.</para>
        /////// <para>-or-</para>
        /////// <para>The type of <paramref name="value"/> is not an underlying type of <typeparamref name="TEnum"/>.</para>
        /////// <para>-or-</para>
        /////// <para><paramref name="value"/> is not defined in the enum.</para>
        /////// </exception>
        /////// <exception cref="InvalidOperationException">value is not type <see cref="System.SByte"/>,
        /////// <see cref="System.Int16"/>, <see cref="System.Int32"/>, <see cref="System.Int64"/>,
        /////// <see cref="System.Byte"/>, <see cref="System.UInt16"/>, <see cref="System.UInt32"/>,
        /////// <see cref="System.UInt64"/>, or <see cref="System.String"/>.</exception>
        ////[DebuggerStepThrough]
        ////public static void IsDefined<TEnum>(TEnum value, string parameterName) where TEnum : struct
        ////{
        ////    if (!Enum.IsDefined(typeof(TEnum), value))
        ////    {
        ////        // Cannot throw InvalidEnumArgumentException as it is not defined in Silverlight
        ////        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.ArgumentOutOfRange_InvalidEnum, parameterName, value, typeof(TEnum).Name), parameterName);
        ////    }
        ////}
        ////#endregion

        /// <summary>
        /// Checks for a condition and throws an exception if the condition is <see langword="true"/>.
        /// </summary>
        /// <param name="condition"><see langword="false"/> to prevent a message being displayed; otherwise, <see langword="true"/>.</param>
        /// <exception cref="InvalidOperationException">The condition is <see langword="true"/>.</exception>
        [DebuggerStepThrough]
        public static void IsFalse(bool condition)
        {
            if (condition)
            {
                throw ExceptionBuilder.CreateInvalidOperation(null);
            }
        }

        /// <summary>
        /// Checks for a condition and throws an exception if the condition is <see langword="true"/>.
        /// </summary>
        /// <param name="condition"><see langword="false"/> to prevent a message being displayed; otherwise, <see langword="true"/>.</param>
        /// <param name="message">A message to be used in the resulting exception.</param>
        /// <exception cref="InvalidOperationException">The condition is <see langword="true"/>.</exception>
        [DebuggerStepThrough]
        public static void IsFalse(bool condition, string message)
        {
            if (condition)
            {
                throw ExceptionBuilder.CreateInvalidOperation(message);
            }
        }

        /// <summary>
        /// Checks for a condition and throws an exception if the condition is <see langword="true"/>.
        /// </summary>
        /// <param name="condition"><see langword="false"/> to prevent a message being displayed; otherwise, <see langword="true"/>.</param>
        /// <param name="parameterName">The name of the parameter being tested.</param>
        /// <param name="message">A message to be used in the resulting exception.</param>
        /// <exception cref="ArgumentException">The condition is <see langword="true"/>.</exception>
        [DebuggerStepThrough]
        public static void IsFalse(bool condition, string parameterName, string message)
        {
            if (condition)
            {
                throw ExceptionBuilder.CreateArgumentException(parameterName, message);
            }
        }

        /// <summary>
        /// Checks for a condition and throws an exception if the condition is <see langword="false"/>.
        /// </summary>
        /// <param name="condition"><see langword="true"/> to prevent a message being displayed; otherwise, <see langword="false"/>.</param>
        /// <exception cref="InvalidOperationException">The condition is <see langword="false"/>.</exception>
        [DebuggerStepThrough]
        public static void IsTrue(bool condition)
        {
            if (!condition)
            {
                throw ExceptionBuilder.CreateInvalidOperation(null);
            }
        }

        /// <summary>
        /// Checks for a condition and throws an exception if the condition is <see langword="false"/>.
        /// </summary>
        /// <param name="condition"><see langword="true"/> to prevent a message being displayed; otherwise, <see langword="false"/>.</param>
        /// <param name="message">A message to be used in the resulting exception.</param>
        /// <exception cref="InvalidOperationException">The condition is <see langword="false"/>.</exception>
        [DebuggerStepThrough]
        public static void IsTrue(bool condition, string message)
        {
            if (!condition)
            {
                throw ExceptionBuilder.CreateInvalidOperation(message);
            }
        }

        /// <summary>
        /// Checks for a condition and throws an exception if the condition is <see langword="false"/>.
        /// </summary>
        /// <param name="condition"><see langword="true"/> to prevent a message being displayed; otherwise, <see langword="false"/>.</param>
        /// <param name="parameterName">The name of the parameter being tested.</param>
        /// <param name="message">A message to be used in the resulting exception.</param>
        /// <exception cref="ArgumentException">The condition is <see langword="false"/>.</exception>
        [DebuggerStepThrough]
        public static void IsTrue(bool condition, string parameterName, string message)
        {
            if (!condition)
            {
                throw ExceptionBuilder.CreateArgumentException(parameterName, message);
            }
        }

        /// <summary>
        /// Requires that <paramref name="objectName"/> not be disposed.
        /// </summary>
        /// <param name="condition"><see langword="true"/> to prevent a message being displayed; otherwise, <see langword="false"/>.</param>
        /// <param name="objectName">A string containing the name of the object.</param>
        [DebuggerStepThrough]
        public static void NotDisposed(bool condition, string objectName)
        {
            Assumes.NotNullOrEmpty(objectName);
            if (!condition)
            {
                throw ExceptionBuilder.CreateObjectDisposed(objectName);
            }
        }

        /// <summary>
        /// Checks that <paramref name="value"/> is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value to test.</typeparam>
        /// <param name="value">The parameter to test.</param>
        /// <param name="parameterName">The name of the parameter being tested.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        [DebuggerStepThrough]
        public static void NotNull<T>(T value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Checks that <paramref name="value"/> is not <see langword="null"/> or a zero-length string.
        /// </summary>
        /// <param name="value">The parameter to test.</param>
        /// <param name="parameterName">The name of the parameter being tested.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is a zero-length string.</exception>
        [DebuggerStepThrough]
        public static void NotNullOrEmpty(string value, string parameterName)
        {
            NotNull(value, parameterName);

            if (value.Length == 0)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.ArgumentException_EmptyString, parameterName), parameterName);
            }
        }

        /// <summary>
        /// Checks that <paramref name="values"/> is not <see langword="null"/>
        /// and that all of the elements are not <see langword="null"/>.
        /// </summary>
        /// <param name="values">The parameter to test.</param>
        /// <param name="parameterName">The name of the parameter being tested.</param>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">An element of <paramref name="values"/> is <see langword="null"/>.</exception>
        [DebuggerStepThrough]
        public static void NotNullOrNullElements(IEnumerable values, string parameterName)
        {
            NotNull(values, parameterName);
            NotNullElements(values, parameterName);
        }

        /// <summary>
        /// Checks that <paramref name="values"/> is not <see langword="null"/>
        /// and that all of the elements are not <see langword="null"/>.
        /// </summary>
        /// <param name="values">The parameter to test.</param>
        /// <param name="parameterName">The name of the parameter being tested.</param>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">An element of <paramref name="values"/> is <see langword="null"/>.</exception>
        [DebuggerStepThrough]
        public static void NotNullOrNullOrEmptyElements(IEnumerable<string> values, string parameterName)
        {
            NotNull(values, parameterName);
            NotNullOrEmptyElements(values, parameterName);
        }

        /// <summary>
        /// Checks that all of the elements of <paramref name="values"/>
        /// are not <see langword="null"/> only if <paramref name="values"/>
        /// is also not <see langword="null"/>.
        /// </summary>
        /// <param name="values">The parameter to test.</param>
        /// <param name="parameterName">The name of the parameter being tested.</param>
        /// <exception cref="ArgumentException">An element of <paramref name="values"/> is <see langword="null"/>.</exception>
        [DebuggerStepThrough]
        public static void NullOrNotNullElements(IEnumerable values, string parameterName)
        {
            if (values != null)
            {
                NotNullElements(values, parameterName);
            }
        }

        /// <summary>
        /// Checks for a condition and throws an exception if the condition is <see langword="false"/>.
        /// </summary>
        /// <param name="condition"><see langword="true"/> to prevent a message being displayed; otherwise, <see langword="false"/>.</param>
        /// <param name="parameterName">The name of the parameter being tested.</param>
        /// <exception cref="ArgumentOutOfRangeException">The condition is <see langword="false"/>.</exception>
        [DebuggerStepThrough]
        public static void ValidRange(bool condition, string parameterName)
        {
            ValidRange(condition, parameterName, string.Empty);
        }

        /// <summary>
        /// Checks for a condition and throws an exception if the condition is <see langword="false"/>.
        /// </summary>
        /// <param name="condition"><see langword="true"/> to prevent a message being displayed; otherwise, <see langword="false"/>.</param>
        /// <param name="parameterName">The name of the parameter being tested.</param>
        /// <param name="message">A message to be used in the resulting exception.</param>
        /// <exception cref="ArgumentOutOfRangeException">The condition is <see langword="false"/>.</exception>
        [DebuggerStepThrough]
        public static void ValidRange(bool condition, string parameterName, string message)
        {
            Assumes.NotNullOrEmpty(parameterName);
            Assumes.NotNull(message);

            if (!condition)
            {
                if (string.IsNullOrEmpty(message))
                {
                    throw new ArgumentOutOfRangeException(parameterName);
                }
                else
                {
                    throw new ArgumentOutOfRangeException(parameterName, message);
                }
            }
        }

        /// <summary>
        /// Checks for a condition and throws an exception if the condition is <see langword="false"/>.
        /// </summary>
        /// <param name="condition"><see langword="true"/> to prevent a message being displayed; otherwise, <see langword="false"/>.</param>
        /// <param name="message">A message to be used in the resulting exception.</param>
        /// <exception cref="InvalidOperationException">The condition is <see langword="false"/>.</exception>
        [DebuggerStepThrough]
        public static void ValidState(bool condition, string message)
        {
            if (!condition)
            {
                throw ExceptionBuilder.CreateInvalidOperation(message);
            }
        }

        /// <summary>
        /// Checks that all of the elements of <paramref name="values"/>
        /// are not <see langword="null"/>.
        /// </summary>
        /// <param name="values">The parameter to test.</param>
        /// <param name="parameterName">The name of the parameter being tested.</param>
        /// <exception cref="ArgumentException">An element of <paramref name="values"/> is <see langword="null"/>.</exception>
        private static void NotNullElements(IEnumerable values, string parameterName)
        {
            foreach (object value in values)
            {
                if (value == null)
                {
                    throw ExceptionBuilder.CreateContainsNullElement(parameterName);
                }
            }
        }

        /// <summary>
        /// Checks that all of the elements of <paramref name="values"/>
        /// are not <see langword="null"/>.
        /// </summary>
        /// <param name="values">The parameter to test.</param>
        /// <param name="parameterName">The name of the parameter being tested.</param>
        /// <exception cref="ArgumentException">An element of <paramref name="values"/> is <see langword="null"/>.</exception>
        private static void NotNullOrEmptyElements(IEnumerable<string> values, string parameterName)
        {
            foreach (string value in values)
            {
                NotNullOrEmpty(value, parameterName);
            }
        }
    }
}