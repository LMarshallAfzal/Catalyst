namespace Catalyst.Models;

public interface IFileStorage
{
    Task SaveFileAsync(string filePath, Stream fileStream);
    // Task<Stream> GetFileAsync(string filePath);
    // Task DeleteFileAsync(string filePath);
}