
using ClosedXML.Excel;

namespace Fx_converter.Services.ExcelProcessor
{
    public class ExcelProcessor : IExcelProcessor
    {
        private readonly IXLWorkbook _workbook;
        public ExcelProcessor(IXLWorkbook workbook) {
            _workbook = workbook;
        }
        public async Task<IXLWorkbook> Process() {
            // placeholder return unprocessed
            foreach (var worksheet in _workbook.Worksheets) {
               worksheet.Cell("A1").Value = "ADJUSTED value";
            }
            return _workbook;
        }
    }
}
