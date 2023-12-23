using Fx_converter.Services.ExcelProcessor;
using Microsoft.AspNetCore.Mvc;

namespace Fx_converter.Controllers
{
    [Route("uploadfile")]
    public class FileController : Controller
    {
        private readonly IExcelProcessor _excelProcessor;
        public FileController(IExcelProcessor excelProcessor) {
            _excelProcessor = excelProcessor;
        }
        [HttpPost]
        public async Task<IActionResult> HandleUploadedFile(IFormFile file) {
            if (file == null || file.Length == 0) {
                return BadRequest(new { error = "Invalid req data | no file uploaded" });
            }
            if (Path.GetExtension(file.FileName) != ".xlsx") {
                return BadRequest(new { error = $"Invalid file extension: '{Path.GetExtension(file.FileName)}'" });
            }
            
            try {
                // demo returning the same file without processing
                using (var memoryStream = new MemoryStream()) {
                    file.CopyTo(memoryStream);
                    byte[] processedFile = memoryStream.ToArray();
                    //await _excelProcessor.Process();
                    return File(processedFile, file.ContentType, file.FileName);
                }
            } catch (Exception ex) {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        
            //return File(file.ContentDisposition, file.ContentType);
        }
    }
}
