using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShipeX2.Application.DTOs;
using ShipeX2.Application.Interfaces;
using ShipeX2.Application.Wrappers;
using ShipeX2.Identity.Context;
using static ShipeX2.Persistence.TableModels.Tables;

namespace ShipeX2.Identity.Services
{
    public class PrinterServices : IPrinterServices
    {
        private readonly ApplicationDbContext _context;
        private readonly CurrentUser _currentUser;
        private readonly ILogger<PrinterServices> _logger;

        public PrinterServices(ILogger<PrinterServices> logger, ApplicationDbContext context, CurrentUser currentUser)
        {
            _logger = logger;
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<List<PrinterModel>> GetPrintersAsync ()
        {
            return await _context.Printers
                .Select(p => new PrinterModel
                {
                    PntId = p.PntId,
                    PntAliasName = p.PntAliasName,
                    PntType = p.PntType,
                    PntMode = p.PntMode,
                    PntIP = p.PntIP,
                    PntIdentifier = p.PntIdentifier,
                    Status = p.Status,
                    CupsPrinterName = p.CupsPrinterName
                }).OrderBy(p => p.PntAliasName)
                .ToListAsync();
        }
        public async Task<ApiResult> CreatePrinterAsync(PrinterModel model)
        {
            var result = new ApiResult();
            try
            {
                bool isExist = await _context.Printers.AnyAsync(w => w.Status && (w.PntAliasName == model.PntAliasName || w.PntIP == model.PntIP));
                if (isExist)
                {
                    result.IsSuccessful = false;
                    result.Message = "A printer with the same name or IP already exists.";
                    result.Data = Array.Empty<string>();
                    return result;
                }
                var printer = new Printer
                {
                    PntAliasName = model.PntAliasName,
                    PntType = model.PntType,
                    PntMode = model.PntMode,
                    PntIP = model.PntIP,
                    PntIdentifier = model.PntIdentifier,
                    CupsPrinterName = model.CupsPrinterName,
                    Createdby = _currentUser.GetCurrentUserId(),
                    Createddate = DateTime.Now,
                    Status = true
                };
                await _context.Printers.AddAsync(printer);
                if (await _context.SaveChangesAsync() > 0)
                {
                    result.IsSuccessful = true;
                    result.Message = "Printer saved successfully.";
                    result.Data = Array.Empty<string>();
                }
                else
                {
                    result.IsSuccessful = false;
                    result.Message = "Failed to save printer.";
                    result.Data = Array.Empty<string>();
                }
            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.Message = "Error saving carrier: " + ex.Message;
                result.Data = Array.Empty<string>();
                _logger.LogError(ex, "Error in PrinterServices (CreatePrinterAsync): "); ;
            }
            return result;
        }
        public async Task<PrinterModel> GetPrinterByIdAsync(long id)
        {
            return await _context.Printers
                .Where(p => p.PntId == id)
                .Select(p => new PrinterModel
                {
                    PntId = p.PntId,
                    PntAliasName = p.PntAliasName,
                    PntType = p.PntType,
                    PntMode = p.PntMode,
                    PntIP = p.PntIP,
                    PntIdentifier = p.PntIdentifier,
                    CupsPrinterName = p.CupsPrinterName,
                    Status = p.Status
                }).FirstOrDefaultAsync() ?? new PrinterModel();
        }
        public async Task<ApiResult> UpdatePrinterAsync(PrinterModel model)
        {
            var result = new ApiResult();
            try
            {
                var printer = await _context.Printers.FindAsync(model.PntId);
                if (printer == null)
                {
                    result.IsSuccessful = false;
                    result.Message = "Printer not found.";
                    return result;
                }
                bool isExist = await _context.Printers.AnyAsync(w => w.Status && w.PntId != model.PntId && (w.PntAliasName == model.PntAliasName || w.PntIP == model.PntIP));
                if (isExist)
                {
                    result.IsSuccessful = false;
                    result.Message = "A printer with the same name or IP already exists.";
                    result.Data = Array.Empty<string>();
                    return result;
                }

                printer.PntAliasName = model.PntAliasName;
                printer.PntType = model.PntType;
                printer.PntMode = model.PntMode;
                printer.PntIP = model.PntIP;
                printer.PntIdentifier = model.PntIdentifier;
                printer.CupsPrinterName = model.CupsPrinterName;
                printer.Modifiedby = _currentUser.GetCurrentUserId();
                printer.Modifieddate = DateTime.Now;
                if (await _context.SaveChangesAsync() > 0)
                {
                    result.IsSuccessful = true;
                    result.Message = "Printer updated successfully.";
                    result.Data = Array.Empty<string>();
                }
                else
                {
                    result.IsSuccessful = false;
                    result.Message = "Failed to update printer.";
                    result.Data = Array.Empty<string>();
                }
            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.Message = "Error updating printer: " + ex.Message;
                result.Data = Array.Empty<string>();
                _logger.LogError(ex, "Error in PrinterServices (UpdatePrinterAsync): ");
            }
            return result;
        }
        public async Task<ApiResult> TogglePrinterStatusAsync(long printerId)
        {
            var result = new ApiResult();
            try
            {
                var printer = await _context.Printers.FindAsync(printerId);
                if (printer == null)
                {
                    result.IsSuccessful = false;
                    result.Message = "Printer not found.";
                    return result;
                }

                printer.Status = !printer.Status;
                printer.Modifiedby = _currentUser.GetCurrentUserId();
                printer.Modifieddate = DateTime.Now;

                if (await _context.SaveChangesAsync() > 0)
                {
                    result.IsSuccessful = true;
                    result.Message = "Printer status updated successfully.";
                    result.Data = Array.Empty<string>();
                }
                else
                {
                    result.IsSuccessful = false;
                    result.Message = "Failed to update printer status.";
                    result.Data = Array.Empty<string>();
                }
            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.Message = "Error toggling printer status: " + ex.Message;
                result.Data = Array.Empty<string>();
                _logger.LogError(ex, "Error in PrinterServices (TogglePrinterStatusAsync): ");
            }
            return result;
        }
    }
}
