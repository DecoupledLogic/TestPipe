namespace TestPipe.Assertions.Internal.Contracts
{
    using System;
    using System.Runtime.Serialization;
    using TestPipe.Assertions.Properties;

    /// <summary>
    /// The exception that is thrown when an assumption fails.
    /// </summary>
    [Serializable]
    public sealed class AssumptionException : Exception
    {
        #region events

        #endregion events

        #region class-wide fields

        #endregion class-wide fields

        #region constructors

        #region AssumptionException()

        /// <summary>
        /// Initializes a new instance of the <see cref="AssumptionException"/> class.
        /// </summary>
        public AssumptionException()
            : base(Resources.AssumptionException_EmptyMessage)
        {
        }

        #endregion AssumptionException()

        #region AssumptionException(string message)

        /// <summary>
        /// Initializes a new instance of the <see cref="AssumptionException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public AssumptionException(string message)
            : base(message)
        {
        }

        #endregion AssumptionException(string message)

        #region AssumptionException(string message, Exception inner)

        /// <summary>
        /// Initializes a new instance of the <see cref="AssumptionException"/> class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a <see langword="null"/> if no inner exception is specified.</param>
        public AssumptionException(string message, Exception inner)
            : base(message, inner)
        {
        }

        #endregion AssumptionException(string message, Exception inner)

        #region AssumptionException(SerializationInfo info, StreamingContext context)

        /// <summary>
        /// Initializes a new instance of the <see cref="AssumptionException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
        /// <exception cref="System.ArgumentNullException">The info parameter is null.</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">The class name is <see langword="null"/> or System.Exception.HResult is zero (0).</exception>
        private AssumptionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion AssumptionException(SerializationInfo info, StreamingContext context)

        #endregion constructors

        #region private and internal properties and methods

        #region properties

        #endregion properties

        #region methods

        #endregion methods

        #endregion private and internal properties and methods

        #region public and protected properties and methods

        #region properties

        #endregion properties

        #region methods

        #endregion methods

        #endregion public and protected properties and methods
    }
}