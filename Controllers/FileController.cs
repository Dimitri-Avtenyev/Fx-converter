using ClosedXML.Excel;
using Fx_converter.Services.ExcelProcessor;
using Microsoft.AspNetCore.Mvc;

namespace Fx_converter.Controllers
{
	[Route("uploadfile")]
	public class FileController : Controller
	{
		public FileController(IExcelProcessor excelProcessor) {
			_excelProcessor = excelProcessor;	
		}
		private readonly IExcelProcessor _excelProcessor;

		[HttpPost]
		public async Task<IActionResult> HandleUploadedFile(IFormFile file) {
			if (file == null || file.Length == 0) {
				return BadRequest(new { error = "Invalid req data | no file uploaded" });
			}
			if (Path.GetExtension(file.FileName) != ".xlsx") {
				return BadRequest(new { error = $"Invalid file extension: '{Path.GetExtension(file.FileName)}'" });
			}

			try {
				// demo exammple using ClosedXML package
				using (var stream = file.OpenReadStream()) {
					byte[] modifiedFile = _excelProcessor.Process(stream);
					return File(modifiedFile, file.ContentType, file.FileName);
				}
			} catch (Exception ex) {
				Console.WriteLine(ex);
				return StatusCode(500, "Internal server error");
			}

		}
	}
}
