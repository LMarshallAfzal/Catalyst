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

        using var fs = new FileStream(fullPath, FileMode.Create);
        await fileStream.CopyToAsync(fs);
    }
}