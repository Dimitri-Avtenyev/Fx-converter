
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Fx_converter.Services.ExcelProcessor
{
	public class ExcelProcessor : IExcelProcessor
	{
		public byte[] Process(Stream filestream) {
			using (var workbook = new XLWorkbook(filestream)) {
				// placeholder return unprocessed
				foreach (var worksheet in workbook.Worksheets) {
					var columnCurrency = FindColumnWithCurrency(worksheet);

					if (columnCurrency != null) {
						IXLColumns cols = AddColumns(worksheet, columnCurrency);
						AddData(worksheet, columnCurrency, cols);
						//_workbook.Save();
					}
				}
				using (var modifiedStream = new MemoryStream()) {
					workbook.SaveAs(modifiedStream);
					return modifiedStream.ToArray();
				}
			}
		}
		public void AddData(IXLWorksheet worksheet, string columnCurrency, IXLColumns cols) {
		
			foreach (var currencyCell in worksheet.Cells(columnCurrency)) {
				var currentRow = worksheet.Row(currencyCell.Address.RowNumber);
				var rowCells = currentRow.CellsUsed();

				foreach(var rowCell in rowCells) {
					if (rowCell.Value.IsDateTime) {
						DateTime invoiceDate = rowCell.Value.GetDateTime();

						if(currencyCell.Value.ToString().ToUpper() == "EUR") {
							
							currentRow.Cell("L").Value = 1;
						}
						
					}
				}
            }
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

		public IXLColumns AddColumns(IXLWorksheet worksheet, string columnLetter) {
			var columnCurrencyIndex = XLHelper.GetColumnNumberFromLetter(columnLetter);
			// new columns shift 2 spaces
			var columnExchangeRate = XLHelper.GetColumnNumberFromLetter(columnLetter) + 2;
			var columnConversion = columnExchangeRate + 1;
			var addressRefValue = worksheet.Column(columnLetter).CellsUsed().First().Value.ToString();
			// add 2 columns for ExchangeRate and for Conversion
			var newColumns = worksheet.Column(columnCurrencyIndex + 1).InsertColumnsAfter(2);
			IXLAddress firstAddedCol;
			IXLAddress secondAddedCol;
			foreach (var col in newColumns) {
				var rangeAddress = col.RangeAddress;
				firstAddedCol = rangeAddress.FirstAddress;
				secondAddedCol = rangeAddress.LastAddress;
			}
			// add headers to new columns
			foreach (var cell in worksheet.Column(columnLetter).CellsUsed()) {
				if (cell.Value.IsText && cell.Value.ToString() == addressRefValue) {
					worksheet.Column(columnExchangeRate).Cell(cell.Address.RowNumber).Value = "Exchange rate";
					worksheet.Column(columnConversion).Cell(cell.Address.RowNumber).Value = "Conversion";
				}
			}
			return newColumns;
		}

	}
}
