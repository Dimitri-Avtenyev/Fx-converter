
using ClosedXML.Excel;

namespace Fx_converter.Services.ExcelProcessor
{
	public class ExcelProcessor : IExcelProcessor
	{
		public ExcelProcessor(IFxDataRepository fxDataRepository) {
			_fxDataRepository = fxDataRepository;
		}
		private readonly IFxDataRepository _fxDataRepository;
		public async Task<byte[]> Process(Stream filestream) {
			using (var workbook = new XLWorkbook(filestream)) {

				foreach (var worksheet in workbook.Worksheets) {
					var columnCurrency = FindColumnWithCurrency(worksheet);

					if (columnCurrency != null) {
						int[] cols = AddColumns(worksheet, columnCurrency);
						await AddData(worksheet, columnCurrency, cols);
					}
				}
				using (var modifiedStream = new MemoryStream()) {
					workbook.SaveAs(modifiedStream);
					return modifiedStream.ToArray();
				}
			}
		}
		public async Task AddData(IXLWorksheet worksheet, string columnCurrency, int[] newCols) {
			var getCurrencyRateTasks = new List<Task>();
			foreach (var currencyCell in worksheet.Column(columnCurrency).CellsUsed()) {
				var currentRow = currencyCell.WorksheetRow();

				foreach(var rowCell in currentRow.CellsUsed()) {
					if (rowCell.Value.IsDateTime) {
						DateTime invoiceDate = rowCell.Value.GetDateTime();
						string symbol = currencyCell.Value.ToString().ToUpper();
						if(symbol == "EUR") {
							currentRow.Cell(newCols[0]).Value = 1;
						} else {
							/*var currencyRate = await _fxDataRepository.GetAsyncCurrencyRate(invoiceDate, symbol);
							currentRow.Cell(newCols[0]).Value = currencyRate.Rate;*/
							getCurrencyRateTasks.Add(UpdateCellValueAsync(currentRow, newCols, invoiceDate, symbol));
						}
					} 
				}
            }
			await Task.WhenAll(getCurrencyRateTasks);
		}
		private async Task UpdateCellValueAsync(IXLRow currentRow, int[] newCols , DateTime invoiceDate, string symbol) {
			var currencyRate = await _fxDataRepository.GetAsyncCurrencyRate(invoiceDate, symbol);
			currentRow.Cell(newCols[0]).Value = currencyRate.Rate;
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

		public int[] AddColumns(IXLWorksheet worksheet, string columnLetter) {
			var columnCurrencyIndex = XLHelper.GetColumnNumberFromLetter(columnLetter);
			// new columns shift 2 spaces
			var columnExchangeRate = XLHelper.GetColumnNumberFromLetter(columnLetter) + 2;
			var columnConversion = columnExchangeRate + 1;
			var addressRefValue = worksheet.Column(columnLetter).CellsUsed().First().Value.ToString();
			// add 2 columns for ExchangeRate and for Conversion
			int[] newColumns = [columnExchangeRate, columnConversion];

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
