namespace TestPipe.Core.Interfaces
{
    using System;
    using System.Collections;
    using TestPipe.Core.Exceptions;
    using TestPipe.Core.Interfaces;
    using TestPipe.Core.Session;

    public interface IAsserts
    {
        Result Result { get; set; }

        void AssignableFrom(object arg1, Type arg2, string message = "", params object[] parameters);

        void Empty(string arg1, string message = "", params object[] parameters);

        void Empty(object[] arg1, string message = "", params object[] parameters);

        void Empty(ICollection arg1, string message = "", params object[] parameters);

        void Equal(object arg1, object arg2, string message = "", params object[] parameters);

        void Equal(string arg1, string arg2, bool ignoreCase = false, string message = "", params object[] parameters);

        void Equal<T>(T arg1, T arg2, string message = "", params object[] parameters) where T : IComparable;

        void Fail(string message = "", params object[] parameters);

        void Greater<T>(T arg1, T arg2, string message = "", params object[] parameters) where T : IComparable;

        void GreaterOrEqual<T>(T arg1, T arg2, string message = "", params object[] parameters) where T : IComparable;

        void Ignored();

        void InstanceOfType(object arg1, Type arg2, string message = "", params object[] parameters);

        void IsFalse(bool condition, string message = "", params object[] parameters);

        void IsTrue(bool condition, string message = "", params object[] parameters);

        void Less<T>(T arg1, T arg2, string message = "", params object[] parameters) where T : IComparable;

        void LessOrEqual<T>(T arg1, T arg2, string message = "", params object[] parameters) where T : IComparable;

        void Manual();

        void NaN(double value, string message = "", params object[] parameters);

        void NotAssignableFrom(object arg1, Type arg2, string message = "", params object[] parameters);

        void NotEmpty(string arg1, string message = "", params object[] parameters);

        void NotEmpty(ICollection arg1, string message = "", params object[] parameters);

        void NotEqual<T>(T arg1, T arg2, string message = "", params object[] parameters) where T : IComparable;

        void NotInstanceOfType(object arg1, Type arg2, string message = "", params object[] parameters);

        void NotNaN(double value, string message = "", params object[] parameters);

        void NotNull(object arg1, string message = "", params object[] parameters);

        void Null(object arg1, string message = "", params object[] parameters);

        /// <summary>
        /// Checks to make sure that the input delegate throws a exception of type exceptionType.
        /// </summary>
        /// <typeparam name="TException">The type of exception expected.</typeparam>
        /// <param name="action">The action to execute to generate the exception.</param>
        void Throws<TException>(Action action) where TException : System.Exception;

        /// <summary>
        /// Checks to make sure that the input delegate throws a exception of type exceptionType.
        /// </summary>
        /// <typeparam name="TException">The type of exception expected.</typeparam>
        /// <param name="expectedMessage">The expected message text.</param>
        /// <param name="action">The action to execute to generate the exception.</param>
        void Throws<TException>(string expectedMessage, Action action) where TException : System.Exception;
    }
}
