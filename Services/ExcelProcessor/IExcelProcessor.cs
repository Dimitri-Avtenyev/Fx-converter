using ClosedXML.Excel;

namespace Fx_converter.Services.ExcelProcessor
{
	public interface IExcelProcessor
	{
		byte[] Process(Stream filestream);
		void AddData(IXLWorksheet worksheet, string columnLetter, IXLColumns cols); 
		string? FindColumnWithCurrency(IXLWorksheet worksheet);
		IXLColumns AddColumns(IXLWorksheet worksheet, string columnLetter); 
	}
}
