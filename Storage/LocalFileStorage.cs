using Catalyst.Exceptions;
using Catalyst.Models;
using Microsoft.Extensions.Options;

namespace Catalyst.Storage;

public class LocalFileStorage : IFileStorage
{
    private readonly string _uploadFolder;

    public LocalFileStorage(IOptions<FileStorageOptions> options)
    {
        _uploadFolder = options.Value.UploadFolder;
    }

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