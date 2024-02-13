using System;

namespace Catalyst.Exceptions
{
    public class FileUploadFailedException : Exception
    {
        public FileUploadFailedException() : base() {}
        public FileUploadFailedException(string message) : base(message) {}
        public FileUploadFailedException(string message, Exception innerException) : base(message, innerException) {}
    }
}