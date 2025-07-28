using ShipeX2.Application.DTOs;
using ShipeX2.Application.Wrappers;

namespace ShipeX2.Application.Interfaces
{
    public interface IPrinterServices
    {
        Task<List<PrinterModel>> GetPrintersAsync();
        Task<ApiResult> CreatePrinterAsync(PrinterModel model);
        Task<PrinterModel> GetPrinterByIdAsync(long id);
        Task<ApiResult> UpdatePrinterAsync(PrinterModel model);
        Task<ApiResult> TogglePrinterStatusAsync(long printerId);
    }
}
