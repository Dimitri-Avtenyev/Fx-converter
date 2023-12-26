
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;

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
				var columnLetter = FindColumnWithCurrency(worksheet);
				
				if(columnLetter != null) {
					AddColumns(worksheet, columnLetter);
					AddData(worksheet);
				}
			}
			return _workbook;
		}
		public void AddData(IXLWorksheet worksheet) {

		}
		public string? FindColumnWithCurrency(IXLWorksheet worksheet) {
			string? currencyColumn = null;

			foreach(var cell in worksheet.CellsUsed()) {
				if (cell.Value.ToString().ToUpperInvariant() == "EUR") {
					currencyColumn = cell.Address.ColumnLetter;
					break;
				}
			}
			return currencyColumn;
		}


		public void AddColumns(IXLWorksheet worksheet, string columnLetter) {
			var columnCurrencyIndex = XLHelper.GetColumnNumberFromLetter(columnLetter);
			var columnExchangeRate = XLHelper.GetColumnNumberFromLetter(columnLetter) + 2;
			var columnConversion = columnExchangeRate + 1;
			var addressRefValue = worksheet.Column(columnLetter).CellsUsed().First().Value.ToString();
			// add 2 columns for ExchangeRate and for Conversion
			worksheet.Column(columnCurrencyIndex + 1).InsertColumnsAfter(2);

			// add headers to new columns
			foreach (var cell in worksheet.Column(columnLetter).CellsUsed()) {
				if (cell.Value.IsText && cell.Value.ToString() == addressRefValue) {
					worksheet.Column(columnExchangeRate).Cell(cell.Address.RowNumber).Value = "Exchange rate";
					worksheet.Column(columnConversion).Cell(cell.Address.RowNumber).Value = "Conversion";
				}
			}
			
		}
	}
}
