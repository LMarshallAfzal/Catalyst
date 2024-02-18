namespace Catalyst.Models
{
    /// <summary>
    /// Represents a contract for interacting with a file sotrage system.
    /// </summary>
    public interface IFileStorage
    {
        /// <summary>
        /// Asynchronously saves afile to the file storage system.
        /// </summary>
        /// <param name="filePath">The path within the storage system where the file should be saved.</param>
        /// <param name="fileStream">The stream containing the file data.</param>
        /// <returns>A task representing the asychronous operation.</returns>
        Task SaveFileAsync(string filePath, Stream fileStream);
        // Task<Stream> GetFileAsync(string filePath);
        // Task DeleteFileAsync(string filePath);
    }
}