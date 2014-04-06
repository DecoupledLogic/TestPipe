namespace TestPipe.Assertions.Internal.Contracts
{
    using System.Diagnostics;
    using System.Globalization;
    using TestPipe.Assertions.Properties;

    /// <summary>
    /// Provides a set of methods to simplify debugging your code.
    /// </summary>
    internal static class Assumes
    {
        /// <summary>
        /// Fails with the provided message as the reason.
        /// </summary>
        /// <param name="message">The message of the resulting <see cref="AssumptionException"/>.</param>
        /// <exception cref="AssumptionException">An assumption failed.</exception>
        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public static void Fail(string message)
        {
            FailFast(message);
        }

        /// <summary>
        /// Checks for a condition and displays a message and throws an exception if the condition is <see langword="true"/>.
        /// </summary>
        /// <param name="condition"><see langword="false"/> to prevent a message being displayed; otherwise, <see langword="true"/>.</param>
        /// <exception cref="AssumptionException">The condition is <see langword="false"/>.</exception>
        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public static void IsFalse(bool condition)
        {
            if (condition)
            {
                Fail(null);
            }
        }

        /// <summary>
        /// Checks for a condition and displays a message and throws an exception if the condition is <see langword="true"/>.
        /// </summary>
        /// <param name="condition"><see langword="false"/> to prevent a message being displayed; otherwise, <see langword="true"/>.</param>
        /// <param name="message">A message to display and to be used in the resulting exception.</param>
        /// <exception cref="AssumptionException">The condition is <see langword="false"/>.</exception>
        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public static void IsFalse(bool condition, string message)
        {
            if (condition)
            {
                Fail(message);
            }
        }

        /// <summary>
        /// Checks for a condition and displays a message and throws an exception if the condition is <see langword="false"/>.
        /// </summary>
        /// <param name="condition"><see langword="true"/> to prevent a message being displayed; otherwise, <see langword="false"/>.</param>
        /// <exception cref="AssumptionException">The condition is <see langword="false"/>.</exception>
        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public static void IsTrue(bool condition)
        {
            if (!condition)
            {
                Fail(null);
            }
        }

        /// <summary>
        /// Checks for a condition and displays a message and throws an exception if the condition is <see langword="false"/>.
        /// </summary>
        /// <param name="condition"><see langword="true"/> to prevent a message being displayed; otherwise, <see langword="false"/>.</param>
        /// <param name="message">A message to display and to be used in the resulting exception.</param>
        /// <exception cref="AssumptionException">The condition is <see langword="false"/>.</exception>
        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public static void IsTrue(bool condition, string message)
        {
            if (!condition)
            {
                Fail(message);
            }
        }

        /// <summary>
        /// Checks that <paramref name="value"/> is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value to test.</typeparam>
        /// <param name="value">The value to test.</param>
        /// <exception cref="AssumptionException"><paramref name="value"/> is <see langword="null"/>.</exception>
        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public static void NotNull<T>(T value)
        {
            IsTrue(value != null);
        }

        /// <summary>
        /// Checks that <paramref name="value1"/> and <paramref name="value2"/> are not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first value to test.</typeparam>
        /// <typeparam name="T2">The type of the second value to test.</typeparam>
        /// <param name="value1">The first value to test.</param>
        /// <param name="value2">The second value to test.</param>
        /// <exception cref="AssumptionException">
        /// <para><paramref name="value1"/> is <see langword="null"/>.</para>
        /// <para>-or-</para>
        /// <para><paramref name="value2"/> is <see langword="null"/>.</para>
        /// </exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "1", Justification = "This is the clearest name.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "2", Justification = "This is the clearest name.")]
        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public static void NotNull<T1, T2>(T1 value1, T2 value2)
        {
            NotNull(value1);
            NotNull(value2);
        }

        /// <summary>
        /// Checks that <paramref name="value1"/>, <paramref name="value2"/>, and <paramref name="value3"/> are not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first value to test.</typeparam>
        /// <typeparam name="T2">The type of the second value to test.</typeparam>
        /// <typeparam name="T3">The type of the third value to test.</typeparam>
        /// <param name="value1">The first value to test.</param>
        /// <param name="value2">The second value to test.</param>
        /// <param name="value3">The third value to test.</param>
        /// <exception cref="AssumptionException">
        /// <para><paramref name="value1"/> is <see langword="null"/>.</para>
        /// <para>-or-</para>
        /// <para><paramref name="value2"/> is <see langword="null"/>.</para>
        /// <para>-or-</para>
        /// <para><paramref name="value3"/> is <see langword="null"/>.</para>
        /// </exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "1", Justification = "This is the clearest name.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "2", Justification = "This is the clearest name.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "3", Justification = "This is the clearest name.")]
        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public static void NotNull<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
        {
            NotNull(value1);
            NotNull(value2);
            NotNull(value3);
        }

        /// <summary>
        /// Checks that <paramref name="value1"/>, <paramref name="value2"/>,
        /// <paramref name="value3"/>, and <paramref name="value4"/> are not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first value to test.</typeparam>
        /// <typeparam name="T2">The type of the second value to test.</typeparam>
        /// <typeparam name="T3">The type of the third value to test.</typeparam>
        /// <typeparam name="T4">The type of the fourth value to test.</typeparam>
        /// <param name="value1">The first value to test.</param>
        /// <param name="value2">The second value to test.</param>
        /// <param name="value3">The third value to test.</param>
        /// <param name="value4">The fourth value to test.</param>
        /// <exception cref="AssumptionException">
        /// <para><paramref name="value1"/> is <see langword="null"/>.</para>
        /// <para>-or-</para>
        /// <para><paramref name="value2"/> is <see langword="null"/>.</para>
        /// <para>-or-</para>
        /// <para><paramref name="value3"/> is <see langword="null"/>.</para>
        /// <para>-or-</para>
        /// <para><paramref name="value4"/> is <see langword="null"/>.</para>
        /// </exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "1", Justification = "This is the clearest name.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "2", Justification = "This is the clearest name.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "3", Justification = "This is the clearest name.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "4", Justification = "This is the clearest name.")]
        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public static void NotNull<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            NotNull(value1);
            NotNull(value2);
            NotNull(value3);
            NotNull(value4);
        }

        /// <summary>
        /// Checks that <paramref name="value"/> is not <see langword="null"/> or a zero-length string.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <exception cref="AssumptionException"><paramref name="value"/> is <see langword="null"/> or zero-length.</exception>
        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public static void NotNullOrEmpty(string value)
        {
            NotNull(value);
            IsTrue(value.Length > 0);
        }

        /// <summary>
        /// Checks that <paramref name="value"/> is <see langword="null"/>.
        /// </summary>
        /// <param name="value">The object to test.</param>
        /// <exception cref="AssumptionException"><paramref name="value"/> is not <see langword="null"/>.</exception>
        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public static void Null(object value)
        {
            IsTrue(value == null);
        }

        /// <summary>
        /// Throws a new <see cref="AssumptionException"/> and a <see cref="Debug.Assert(bool, string)"/> assertion failure.
        /// </summary>
        /// <param name="message">The message of the resulting AssumptionException.</param>
        /// <exception cref="AssumptionException">An assumption failed.</exception>
        private static void FailFast(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                message = Resources.AssumptionException_EmptyMessage;
            }
            else
            {
                message = string.Format(CultureInfo.CurrentUICulture, Resources.AssumptionException_Message, message);
            }

            // Need to use Debug.Assert instead of Debug.Fail because Silverlight doesn't contain Debug.Fail.
            Debug.Assert(false, message);
            throw new AssumptionException(message);
        }
    }
}