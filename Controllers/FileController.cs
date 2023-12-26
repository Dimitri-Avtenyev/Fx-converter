using ClosedXML.Excel;
using Fx_converter.Services.ExcelProcessor;
using Microsoft.AspNetCore.Mvc;

namespace Fx_converter.Controllers
{
	[Route("uploadfile")]
	public class FileController : Controller
	{
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
				using (var stream = file.OpenReadStream())
				using (var workbook = new XLWorkbook(stream)) {
					ExcelProcessor excelProcessor = new ExcelProcessor(workbook);
					IXLWorkbook processedWorkbook = await excelProcessor.Process();
					// Save the modified content back to a MemoryStream
					using (var modifiedStream = new MemoryStream()) {
						processedWorkbook.SaveAs(modifiedStream);
						// Convert the MemoryStream to a byte array
						byte[] modifiedFile = modifiedStream.ToArray();

						// Return the modified file with the correct content type
						return File(modifiedFile, file.ContentType, file.FileName);
					}
				}

			} catch (Exception ex) {
				Console.WriteLine(ex);
				return StatusCode(500, "Internal server error");
			}

		}
	}
}
