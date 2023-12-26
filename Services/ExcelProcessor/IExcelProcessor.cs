using ClosedXML.Excel;

namespace Fx_converter.Services.ExcelProcessor
{
    public interface IExcelProcessor
    {
        Task<IXLWorkbook> Process();
    }
}
