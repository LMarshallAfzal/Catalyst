using System;

namespace Catalyst.Exceptions
{
    /// <summary>
    /// Represents an exception that occuses when a file upload operation fails.
    /// </summary>
    public class FileUploadFailedException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="FileUploadFailedException"/> class. 
        /// </summary>
        public FileUploadFailedException() : base() {}

        /// <summary>
        /// Initialises a new instance of the <see cref="FileUploadFailedException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public FileUploadFailedException(string message) : base(message) {}

        /// <summary>
        /// Initialises a new instance of the <see cref="FileUploadFailedException"/> class with a specified error message.
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cuase of the current exception.</param>
        public FileUploadFailedException(string message, Exception innerException) : base(message, innerException) {}
    }
}