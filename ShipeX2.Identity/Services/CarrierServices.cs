using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShipeX2.Application.DTOs;
using ShipeX2.Application.Interfaces;
using ShipeX2.Identity.Context;

namespace ShipeX2.Identity.Services
{
    public class CarrierServices : ICarrierListServices
    {
        private readonly ILogger<CarrierServices> _logger;
        private readonly ApplicationDbContext _context;
        public CarrierServices ( ILogger<CarrierServices> logger, ApplicationDbContext context )
        {
            _logger = logger;
            _context = context;
        }

        public async Task<List<ModelShipCarrier>> GetCarriersAsync()
        {
            List<ModelShipCarrier> model = new  List<ModelShipCarrier>();
            try
            {
                model = await _context.ShipCarriers
                        .Select(s => new ModelShipCarrier
                        {
                            CarrierId = (int) s.CarrierId,
                            CarrierName = s.CarrierName,
                            DefaultAccountNo = s.DefaultAccountNo,
                            ApiKey1 = s.ApiKey1,
                            ApiKey2 = s.ApiKey2,
                            ApiKey3 = s.ApiKey3,
                            CreatedBy = s.CreatedBy,
                            CreatedDate = s.CreatedDate,
                            ModifiedBy = s.ModifiedBy,
                            ModifiedDate = s.ModifiedDate,
                            Status = s.Status,
                            Mode = s.Mode
                        })
                        .OrderBy(c => c.CarrierName)
                        .ToListAsync();
            }
            catch (Exception ex)
            {
                 _logger.LogError("Error in Carrier Controller (Index):" + ex.Message);
                throw;
            }
            return model;
        }
        
    }
}
