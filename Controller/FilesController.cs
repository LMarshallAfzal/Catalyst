using Microsoft.AspNetCore.Mvc;
using Catalyst.Models;
using Catalyst.Exceptions;

namespace Catalyst.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileStorage _fileStorage;
        public FilesController(IFileStorage fileStorage)
        {
            _fileStorage = fileStorage;
        }

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
                return StatusCode(500, "An unexpected error occurred during file upload.");
            }
        }

        private static string SanitiseFileName(string filename)
        {
            var allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-_.";
            var cleanName = new string(filename.Where(c => allowedChars.Contains(c)).ToArray());
            cleanName = cleanName.Trim().Replace(" ", "_");

            return cleanName;
        }
    }
}