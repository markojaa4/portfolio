using System;

namespace FighterLib
{
    /// <summary>
    /// Represents an error that occured through normal use.
    /// </summary>
    public class UsageException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsageException"/> class.
        /// </summary>
        public UsageException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsageException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public UsageException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsageException"/> class with a specified error message and a
        /// reference to the exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public UsageException(string message, Exception innerException) : base(message, innerException) { }
    }
}
