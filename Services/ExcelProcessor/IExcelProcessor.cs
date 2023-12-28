using ClosedXML.Excel;

namespace Fx_converter.Services.ExcelProcessor
{
	public interface IExcelProcessor
	{
		Task<byte[]> Process(Stream filestream);
		Task AddData(IXLWorksheet worksheet, string columnLetter, int[] newCols); 
		string? FindColumnWithCurrency(IXLWorksheet worksheet);
		int[] AddColumns(IXLWorksheet worksheet, string columnLetter); 
	}
}
