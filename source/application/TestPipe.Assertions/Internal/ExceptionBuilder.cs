namespace TestPipe.Assertions.Internal
{
    using System;
    using System.Globalization;
    using TestPipe.Assertions.Internal.Contracts;
    using TestPipe.Assertions.Properties;

    /// <summary>
    /// Provides methods to create specific exceptions.
    /// </summary>
    internal static class ExceptionBuilder
    {
        /// <summary>
        /// Create a new <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that caused the exception.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <returns>A new <see cref="ArgumentException"/>.</returns>
        public static ArgumentException CreateArgumentException(string parameterName, string message)
        {
            Assumes.NotNull(parameterName);
            Assumes.NotNull(message);

            return new ArgumentException(message, parameterName);
        }

        /// <summary>
        /// Create an exception idicating that an array or collection element was <see langword="null"/>.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that caused the exception.</param>
        /// <returns>A new <see cref="ArgumentException"/>.</returns>
        public static ArgumentException CreateContainsNullElement(string parameterName)
        {
            Assumes.NotNull(parameterName);

            string message = Format(Resources.Argument_NullElement, parameterName);

            return new ArgumentException(message, parameterName);
        }

        /// <summary>
        /// Create a new <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <returns>A new <see cref="InvalidOperationException"/>.</returns>
        public static InvalidOperationException CreateInvalidOperation(string message)
        {
            return new InvalidOperationException(message);
        }

        /// <summary>
        /// Create an exception indicating that a member was not overridden by a derived class.
        /// </summary>
        /// <param name="memberName">The name of the member that caused the exception.</param>
        /// <returns>A new <see cref="NotImplementedException"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static NotImplementedException CreateNotOverriddenByDerived(string memberName)
        {
            Assumes.NotNullOrEmpty(memberName);

            string message = Format(Resources.NotImplemented_NotOverriddenByDerived, memberName);

            return new NotImplementedException(message);
        }

        /// <summary>
        /// Create a new <see cref="ObjectDisposedException"/>.
        /// </summary>
        /// <param name="objectName">A string containing the name of the disposed object.</param>
        /// <returns>A new <see cref="ObjectDisposedException"/>.</returns>
        public static ObjectDisposedException CreateObjectDisposed(string objectName)
        {
            Assumes.NotNullOrEmpty(objectName);

            return new ObjectDisposedException(objectName);
        }

        /// <summary>
        /// Replaces the format item in a specified <see cref="String"/> with the text equivalent
        /// of the value of a corresponding <see cref="String"/> instance in a specified array.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arguments">A <see cref="String"/> array containing zero or more strings to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the corresponding
        /// instances of <see cref="String"/> in args.</returns>
        private static string Format(string format, params string[] arguments)
        {
            return string.Format(CultureInfo.CurrentCulture, format, arguments);
        }
    }
}