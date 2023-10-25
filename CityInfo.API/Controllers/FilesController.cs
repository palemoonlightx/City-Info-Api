using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers {

    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase {

        private readonly FileExtensionContentTypeProvider _fct;

        public FilesController(FileExtensionContentTypeProvider fct) {
            _fct = fct ?? throw new ArgumentNullException(nameof(fct));
        }

        [HttpGet("{fileId}")]
        public ActionResult GetFile(string fileId) {


            var pathToFile = "image.jpg";
            if (!System.IO.File.Exists(pathToFile)) {
                return NotFound();
            }

            if (!_fct.TryGetContentType(pathToFile, out var contentType)) {
                contentType = "application/octet-stream";
            }

            var bytes = System.IO.File.ReadAllBytes(pathToFile);

            return File(bytes, contentType, Path.GetFileName(pathToFile));

        }
    }
}
 