using ClosedXML.Excel;

namespace Fx_converter.Services.ExcelProcessor
{
	public interface IExcelProcessor
	{
		Task<IXLWorkbook> Process();
		void AddData(IXLWorksheet worksheet); 
		string? FindColumnWithCurrency(IXLWorksheet worksheet);
		public void AddColumns(IXLWorksheet worksheet, string columnLetter); 
	}
}
