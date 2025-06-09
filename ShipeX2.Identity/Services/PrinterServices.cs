using Microsoft.EntityFrameworkCore;
using ShipeX2.Application.DTOs;
using ShipeX2.Application.Interfaces;
using ShipeX2.Identity.Context;

namespace ShipeX2.Identity.Services
{
    public class PrinterServices : IPrinterServices
    {
        private readonly ApplicationDbContext _context;
        public PrinterServices ( ApplicationDbContext context )
        {
            _context = context;
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
                    Createdby = p.Createdby,
                    Createddate = p.Createddate,
                    Modifiedby = p.Modifiedby,
                    Modifieddate = p.Modifieddate,
                    CupsPrinterName = p.CupsPrinterName
                }).OrderBy(p => p.PntAliasName)
                .ToListAsync();
        }
    }
}
