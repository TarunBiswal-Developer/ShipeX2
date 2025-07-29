using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShipeX2.Application.Interfaces;
using ShipeX2.Application.Wrappers;
using ShipeX2.Identity.Context;
using static ShipeX2.Application.DTOs.ShipCareer;

namespace ShipeX2.Identity.Services
{
    public class ShipmentServices : IShipment
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ShipmentServices> _logger;

        public ShipmentServices ( ApplicationDbContext context, ILogger<ShipmentServices> logger )
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ApiResult<List<ClientCarrier>>> ClientsCareerAsync ( long clientId )
        {
            ApiResult<List<ClientCarrier>> apiResult = new ApiResult<List<ClientCarrier>>();
            try
            {
                var result = await _context.ShipCarriers.Join(_context.ClientCarriers, sc => sc.CarrierId, cc => cc.CarrierId, ( sc, cc ) => new { sc, cc })
                         .Join(_context.Clients, sc_cc => sc_cc.cc.ClientId, c => c.ClientId, ( sc_cc, c ) => new { sc_cc, c })
                         .Where(x => x.c.ClientId == clientId).OrderBy(x => x.sc_cc.cc.DefaultAccount)
                         .Select(x => new ClientCarrier
                         {
                             CarrierId = x.sc_cc.sc.CarrierId,
                             CarrierName = x.sc_cc.sc.CarrierName,
                             ApiKey1 = x.sc_cc.sc.ApiKey1,
                             ApiKey2 = x.sc_cc.sc.ApiKey2,
                             AccountNo = x.sc_cc.cc.AccountNo,
                             Mode = x.sc_cc.sc.Mode,
                             STDCode = x.c.STDCode
                         }).ToListAsync();
                if (result.Any())
                {
                    apiResult.Data = result;
                    apiResult.IsSuccessful = true;
                    apiResult.Message = "Carrier list loaded successfully.";
                }
                else
                {
                    apiResult.IsSuccessful = false;
                    apiResult.Message = "No carriers found for the specified client.";
                    apiResult.Data = Array.Empty<string>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving client careers for client ID {ClientId}", clientId);
                throw;
            }
            return apiResult;
        }

        public async Task<ApiResult<ClientCarrier>> ClientsDefaultAccountInfo ( long careerId )
        {
            ApiResult<ClientCarrier> apiResult = new ApiResult<ClientCarrier>();
            try
            {
                var result = await _context.ClientCarriers.Where(x => x.CarrierId == careerId)
                    .Select(x => new ClientCarrier
                    {
                        CarrierId = x.CarrierId,
                        AccountNo = x.AccountNo,
                        DefaultAccount = x.DefaultAccount
                    }).FirstOrDefaultAsync();
                if (result != null)
                {
                    apiResult.Data = result;
                    apiResult.IsSuccessful = true;
                    apiResult.Message = "Default account information retrieved successfully.";
                }
                else
                {
                    apiResult.IsSuccessful = false;
                    apiResult.Message = "No default account found for the specified carrier.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving default account info for carrier ID {CareerId}", careerId);
                throw;
            }
            return apiResult;
        }

        public async Task<ApiResult<List<CarrierService>>> ShipServiceListAsync ( long careerId )
        {
            ApiResult<List<CarrierService>> apiResult = new ApiResult<List<CarrierService>>();
            try
            {
                var result = await _context.CarrierServices.Where(x => x.CarrierId == careerId && x.Status == true)
                             .Select(x => new CarrierService
                             {
                                 ServiceId = x.ServiceId,
                                 ServiceName = x.ServiceName,
                                 CarrierId = x.CarrierId,
                                 ServiceCode = x.ServiceCode
                             }).OrderBy(o => o.ServiceName).ToListAsync();
                if (result.Any())
                {
                    apiResult.Data = result;
                    apiResult.IsSuccessful = true;
                    apiResult.Message = "Ship services list loaded successfully.";
                }
                else
                {
                    apiResult.IsSuccessful = false;
                    apiResult.Message = "No ship services found for the specified client.";
                    apiResult.Data = Array.Empty<string>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving ship services for client ID {ClientId}", careerId);
                throw;
            }
            return apiResult;
        }

        public async Task<ApiResult<List<CarrierPacking>>> RetrievePackagingAsync ( string shipViaCode, decimal weight )
        {
            ApiResult<List<CarrierPacking>> apiResult = new ApiResult<List<CarrierPacking>>();
            try
            {
                var query = await _context.CarrierPackings.Where(p => p.Status == true).Join(_context.ServicePacks, p => p.PackingId, s => s.PackingId, ( p, s ) => new { p, s })
                               .Join(_context.CarrierServices, ps => ps.s.ServiceId, cs => cs.ServiceId, ( ps, cs ) => new { ps.p, cs })
                               .Where(x => x.cs.ServiceCode == shipViaCode)
                               .GroupBy(x => new { x.p.PackingId, x.p.PackingName, x.p.MaxWeight })
                               .Select(g => new CarrierPacking
                               {
                                   PackingId = g.Key.PackingId,
                                   PackingName = g.Key.PackingName,
                                   MaxWeight = g.Key.MaxWeight
                               }).OrderBy(p => p.MaxWeight)
                               .ToListAsync();

                // Simulate LAG in memory
                decimal prevMaxWeight = 0;
                for (int i = 0; i < query.Count; i++)
                {
                    var current = query [i];
                    current.IsWeightInRange = (current.MaxWeight >= 0 && prevMaxWeight < 0) ? "Selected" : "";
                    prevMaxWeight = current.MaxWeight;
                }

                if (query.Any())
                {
                    apiResult.Data = query;
                    apiResult.IsSuccessful = true;
                    apiResult.Message = "Packaging list retrieved successfully.";
                }
                else
                {
                    apiResult.IsSuccessful = false;
                    apiResult.Message = "No packaging found for the specified criteria.";
                    apiResult.Data = Array.Empty<string>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving packaging for ship via code {ShipViaCode} and weight {Weight}", shipViaCode, weight);
                throw;
            }
            return apiResult;
        }
    }
}
