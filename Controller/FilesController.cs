using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Catalyst.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileStorage _fileStorage;
        public FilesController(IFileStorage fileStorage)
        {
            _fileStorage = fileStorage;
        }

        [HttpPost("/upload")]
        public async Task<IActionResult> Upload() 
        {
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    var postedFile = Request.Form.Files[0];
                    var uploadFolderPath = Path.Combine("/path/to/store/uploads");

                    string safeFileName = SanitiseFileName(postedFile.FileName);
                    
                    await _fileStorage.SaveFileAsync(safeFileName, postedFile.OpenReadStream());

                    return Created("File uploaded successfully", safeFileName);
                }
                else
                {
                    return BadRequest("No file found");
                }
            }
            catch (Exception e)
            {
                return BadRequest("File upload failed: " + e.Message);
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