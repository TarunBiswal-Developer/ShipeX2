using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShipeX2.Application.DTOs;
using ShipeX2.Application.Interfaces;
using ShipeX2.Application.Wrappers;
using ShipeX2.Identity.Context;
using static ShipeX2.Persistence.TableModels.Tables;

namespace ShipeX2.Identity.Services
{
    public class CarrierSetupSevices : ICarrierSetupServices
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CarrierSetupSevices> _logger;
        private readonly CurrentUser _currentUser;

        public CarrierSetupSevices(ApplicationDbContext context, ILogger<CarrierSetupSevices> logger, CurrentUser currentUser)
        {
            _context = context;
            _logger = logger;
            _currentUser = currentUser;
        }

        public async Task<ModelShipCarrierService> GetCarrierSetupListAsync()
        {
            ModelShipCarrierService model = new ModelShipCarrierService();
            try
            {
                model.ShipServicelist = await _context.CarrierServices
                                    .Join(_context.ShipCarriers,
                                        cs => cs.CarrierId,
                                        sc => sc.CarrierId,
                                        (cs, sc) => new { cs, sc })
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
                                                    (sp, p) => p.PackingName)
                                                .Distinct()
                                        )
                                    })
                                    .OrderByDescending(x => x.ServiceId)
                                    .ToListAsync();

                model.ShipCarrierlist = await _context.ShipCarriers
                                        .OrderBy(sc => sc.CarrierName)
                                        .Select(sc => new ModelShipCarrier
                                        {
                                            CarrierId = (int)sc.CarrierId,
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
            }
            catch (Exception ex)
            {
                var contextualInfo = new
                {
                    Method = nameof(GetCarrierSetupListAsync),
                    Timestamp = DateTime.UtcNow
                };
                _logger.LogError(ex, "Error retrieving Carrier Setup List. Context: {@ContextualInfo}", contextualInfo);
            }
            return model;
        }
        public async Task<ApiResult> GetPackingListAsync(int carrierId)
        {
            var apiResult = new ApiResult();
            try
            {
                var data = await _context.CarrierPackings
                            .Join(_context.ShipCarriers, CP => CP.CarrierId, SC => SC.CarrierId, (CP, SC) => new { CP, SC })
                            .Where(w => w.CP.CarrierId == carrierId)
                            .Select(s => new PackingViewModel
                            {
                                PackingId = s.CP.PackingId,
                                PackingName = s.CP.PackingName,
                                MaxWeight = s.CP.MaxWeight
                            })
                            .OrderByDescending(o => o.PackingId)
                            .ToListAsync();
                if (data.Count > 0)
                {
                    apiResult.IsSuccessful = true;
                    apiResult.Data = data;
                }
                else
                {
                    apiResult.IsSuccessful = false;
                }
            }
            catch (Exception ex)
            {
                apiResult.IsSuccessful = false;
                apiResult.Message = ex.Message;
                apiResult.Data = Array.Empty<string>();
                _logger.LogError(ex, "Error in PrinterServices (GetPackingListAsync(carrierId)): ");
            }
            return apiResult;
        }

        public async Task<ApiResult> GetCarrierPackingById(int packingId)
        {
            var apiResult = new ApiResult();
            try
            {
                var data = await _context.CarrierPackings
                            .Where(w => w.PackingId == packingId)
                            .Select(s => new PackingViewModel
                            {
                                PackingId = s.PackingId,
                                PackingName = s.PackingName,
                                MaxWeight = s.MaxWeight,
                                FSMCode = s.FSMPackType,
                                Height = s.Height,
                                Width = s.Width,
                                Price = s.Price
                            })
                            .FirstOrDefaultAsync();
                if (data != null)
                {
                    apiResult.IsSuccessful = true;
                    apiResult.Data = data;
                }
                else
                {
                    apiResult.IsSuccessful = false;
                    apiResult.Message = "Carrier Packing Details Not Found";
                }
            }
            catch (Exception ex)
            {
                apiResult.IsSuccessful = false;
                apiResult.Message = ex.Message;
                apiResult.Data = Array.Empty<string>();
                _logger.LogError(ex, "Error in PrinterServices (GetCarrierPackingById(packingId)): ");
            }
            return apiResult;
        }
        public async Task<ApiResult> UpdateCarrierPackingAsync(PackingViewModel model)
        {
            var apiResult = new ApiResult();
            try
            {
                var data = await _context.CarrierPackings.Where(w => w.PackingId == model.PackingId).FirstOrDefaultAsync();
                if (data != null)
                {
                    data.CarrierId = model.CarrierId;
                    data.PackingName = model.PackingName;
                    data.MaxWeight = model.MaxWeight;
                    data.Width = model.Width ?? 0;
                    data.Height = model.Height ?? 0;
                    data.Price = model.Price ?? 0;
                    data.FSMPackType = model.FSMCode;
                    data.ModifiedBy = _currentUser.GetCurrentUserId();
                    data.ModifiedDate = DateTime.UtcNow;
                    apiResult.IsSuccessful = await _context.SaveChangesAsync() > 0;
                    apiResult.Message = apiResult.IsSuccessful ? "Carrier Packing updated successfully." : "Failed to update Carrier Packing.";
                }
                else
                {
                    apiResult.Message = "Package details not found.";
                    apiResult.IsSuccessful = false;
                }
            }
            catch (Exception ex)
            {
                apiResult.IsSuccessful = false;
                apiResult.Message = ex.Message;
                apiResult.Data = Array.Empty<string>();
                _logger.LogError(ex, "Error in PrinterServices (UpdateCarrierPacking(PackingViewModel)): ");
            }
            return apiResult;
        }
        public async Task<ApiResult> CreateCarrierPackingAsync(PackingViewModel model)
        {
            var apiResult = new ApiResult();
            try
            {
                var carrierPackings = new CarrierPacking
                {
                    CarrierId = model.CarrierId,
                    PackingName = model.PackingName,
                    MaxWeight = model.MaxWeight,
                    Width = model.Width ?? 0,
                    Height = model.Height ?? 0,
                    Price = model.Price ?? 0,
                    FSMPackType = model.FSMCode,
                    CreatedBy = _currentUser.GetCurrentUserId(),
                    CreatedDate = DateTime.UtcNow
                };
                await _context.CarrierPackings.AddAsync(carrierPackings);
                apiResult.IsSuccessful = await _context.SaveChangesAsync() > 0;
                apiResult.Message = apiResult.IsSuccessful ? "Carrier Packing created successfully." : "Failed to create Carrier Packing.";
            }
            catch (Exception ex)
            {
                apiResult.IsSuccessful = false;
                apiResult.Message = ex.Message;
                apiResult.Data = Array.Empty<string>();
                _logger.LogError(ex, "Error in PrinterServices (CreateCarrierPacking(PackingViewModel)): ");
            }
            return apiResult;
        }
    }
}
