using Microsoft.AspNetCore.Mvc;
using Catalyst.Models;
using Catalyst.Exceptions;

namespace Catalyst.Controllers
{
    /// <summary>
    /// Provides an endpoint for handling file uploads.
    /// </summary>
    [Route("v1/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileStorage _fileStorage;

        /// <summary>
        /// Constructs a FilesController instance.
        /// </summary>
        /// <param name="fileStorage">An IFileStorage services for file operations.</param>
        public FilesController(IFileStorage fileStorage)
        {
            _fileStorage = fileStorage;
        }

        /// <summary>
        /// Handles HTTP POST requests to upload a file to the server.
        /// </summary>
        /// <returns>
        /// A 201 Created response if successful, with the santised filename.
        /// BadRequest (400) if a FileUploadFailedException occurs.
        /// InternalServerError (500) for other unexpected errors.
        /// </returns>
        [HttpPost("upload")]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var postedFile = Request.Form.Files[0];
                var uploadFolderPath = Path.Combine("/home/leonard/Projects/Catalyst/uploads");
                Console.WriteLine(uploadFolderPath);

                string safeFileName = SanitiseFileName(postedFile.FileName);

                await _fileStorage.SaveFileAsync(safeFileName, postedFile.OpenReadStream());

                return Created("File uploaded successfully", safeFileName);

            }
            catch (FileUploadFailedException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "An unexpected error occurred during file upload." + e.Message);
            }
        }

        /// <summary>
        /// Sanitises a filename, removing illegal characters and replacing spaces with underscores.
        /// </summary>
        /// <param name="filename">The original filename.</param>
        /// <returns>A sanitised filename suitable for storage.</returns>
        private static string SanitiseFileName(string filename)
        {
            var allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-_.";
            var cleanName = new string(filename.Where(c => allowedChars.Contains(c)).ToArray());
            cleanName = cleanName.Trim().Replace(" ", "_");

            return cleanName;
        }
    }
}