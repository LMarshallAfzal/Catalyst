public class LocalFileStorage : IFileStorage
{
    private readonly string _uploadFolder;

    public LocalFileStorage(IConfiguration? config)
    {
        config ??= new ConfigurationBuilder().Build();
        _uploadFolder = config["FileStorage: UploadFolder"] ?? "/uploads";
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