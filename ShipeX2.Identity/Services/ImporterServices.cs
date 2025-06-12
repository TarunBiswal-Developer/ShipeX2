using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShipeX2.Application.Interfaces;
using ShipeX2.Identity.Context;
using static ShipeX2.Application.DTOs.ImporterModel;

namespace ShipeX2.Identity.Services
{
    public class ImporterServices : IImporterService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ImporterServices> _logger;

        public ImporterServices ( ApplicationDbContext context, ILogger<ImporterServices> logger )
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<ImporterExtended>> GetAllImporters ()
        {
            try
            {
                var importers = await _context.importers
                                 .Join(_context.clients,
                                     im => im.ClientId,
                                     cl => cl.ClientId,
                                     ( im, cl ) => new ImporterExtended
                                     {
                                         ImporterId = im.ImporterId,
                                         BrokerName = im.BrokerName,
                                         ImporterName = im.ImporterName,
                                         ImporterCompany = im.ImporterCompany,
                                         Client = cl.CompanyName + " (" + cl.ClientCode + ")",
                                         ImporterAddress1 = im.ImporterAddress1,
                                         ImporterAddress2 = im.ImporterAddress2,
                                         Country = im.Country,
                                         ImporterState = im.ImporterState,
                                         ImporterCity = im.ImporterCity,
                                         ImporterPostCode = im.ImporterPostCode,
                                         ImporterPhone = im.ImporterPhone,
                                         Status = im.Status
                                     })
                                 .ToListAsync();

                var sortedImporters = importers
                    .OrderByDescending(x => x.ImporterId) 
                    .ToList();

                return sortedImporters;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving importers");
                throw;
            }
        }
    }

}
