using Catalyst.Exceptions;
using Catalyst.Models;
using Microsoft.Extensions.Options;

namespace Catalyst.Storage
{
    /// <summary>
    /// Provides an implementation of the IFileStorage interface for saving to the local filesystem
    /// </summary>
    public class LocalFileStorage : IFileStorage
    {
        private readonly string _uploadFolder;

        /// <summary>
        /// Creates an instance of LocalFileStorage.
        /// </summary>
        /// <param name="options">Options specifying the upload folder location.</param>
        public LocalFileStorage(IOptions<FileStorageOptions> options)
        {
            _uploadFolder = options.Value.UploadFolder;
        }

        /// <summary>
        /// Saves a file asynchronously to the configured local storage folder.
        /// </summary>
        /// <param name="filePath">The desired path (including filename) within the upload folder.</param>
        /// <param name="fileStream">A stream containing the file content.</param>
        /// <returns>Thrown for issues including insufficient disk space or other IO errors.</returns>
        public async Task SaveFileAsync(string filePath, Stream fileStream)
        {
            var fullPath = Path.Combine(_uploadFolder, filePath);
            var directoryName = Path.GetDirectoryName(fullPath);
            if (directoryName != null) { Directory.CreateDirectory(directoryName); }

            try
            {
                using var fs = new FileStream(fullPath, FileMode.Create);
                await fileStream.CopyToAsync(fs);
            }
            catch (IOException e)
            {
                if (e.HResult == -2147024784)
                {
                    throw new FileUploadFailedException("Failed to save due to insufficient disk space.", e);
                }
                else
                {
                    throw new FileUploadFailedException("File save failded due to an IO error.", e);
                }
            }
        }
    }
}