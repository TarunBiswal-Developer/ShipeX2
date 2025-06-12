using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShipeX2.Application.DTOs;
using ShipeX2.Application.Interfaces;
using ShipeX2.Identity.Context;

namespace ShipeX2.Identity.Services
{
    public class CarrierSetupSevices : ICarrierSetupServices
    {
		private readonly ApplicationDbContext _context;
		private readonly ILogger<CarrierSetupSevices> _logger;
        public CarrierSetupSevices(ApplicationDbContext context, ILogger<CarrierSetupSevices> logger)
		{
			_context = context;
			_logger = logger;
        }

        public async Task<ModelShipCarrierService> GetCarrierSetupListAsync ()
        {
            ModelShipCarrierService model = new ModelShipCarrierService();
			try
			{
                model.ShipServicelist = await _context.CarrierServices
                                    .Join(_context.ShipCarriers,
                                        cs => cs.CarrierId,
                                        sc => sc.CarrierId,
                                        ( cs, sc ) => new { cs, sc })
                                    .Select(x => new ModelShipCarrierService
                                    {
                                        ServiceId = x.cs.ServiceId,
                                        ServiceName = x.cs.ServiceName,
                                        ServiceCode = x.cs.ServiceCode,
                                        FSMServiceCode = x.cs.FSMServiceType,
                                        CarrierName = x.sc.CarrierName,
                                        Status = x.cs.Status,
                                        PackingName = string.Join(", ",
                                            _context.ServicePacks
                                                .Where(sp => sp.ServiceId == x.cs.ServiceId)
                                                .Join(_context.CarrierPackings,
                                                    sp => sp.PackingId,
                                                    p => p.PackingId,
                                                    ( sp, p ) => p.PackingName)
                                                .Distinct()
                                        )
                                    })
                                    .OrderByDescending(x => x.ServiceId)
                                    .ToListAsync();

                model.ShipCarrierlist = await _context.ShipCarriers
                                        .OrderBy(sc => sc.CarrierName)
                                        .Select(sc => new ModelShipCarrier
                                        {
                                            CarrierId = (int) sc.CarrierId,
                                            CarrierName = sc.CarrierName,
                                            DefaultAccountNo = sc.DefaultAccountNo,
                                            ApiKey1 = sc.ApiKey1,
                                            ApiKey2 = sc.ApiKey2,
                                            ApiKey3 = sc.ApiKey3,
                                            CreatedBy = sc.CreatedBy,
                                            CreatedDate = sc.CreatedDate,
                                            ModifiedBy = sc.ModifiedBy,
                                            ModifiedDate = sc.ModifiedDate,
                                            Status = sc.Status,
                                            Mode = sc.Mode
                                        }).ToListAsync();

                return model;
            }
            catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving Carrier Setup List");
                throw;
			}
        }

    }
}
