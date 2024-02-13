/// <summary>
/// Represents configuration options for file storage.
/// </summary>
public class FileStorageOptions
{
    /// <summary>
    /// Gets or sets the relative path to the folder where uploaded files will be stored.
    /// </summary>
    /// <value>The default valuse is "/uploads"</value>
    public string UploadFolder { get; set; } = "/uploads";
}