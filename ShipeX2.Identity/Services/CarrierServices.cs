using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShipeX2.Application.DTOs;
using ShipeX2.Application.Interfaces;
using ShipeX2.Application.Wrappers;
using ShipeX2.Identity.Context;
using System.Security.Claims;
using static ShipeX2.Persistence.TableModels.Tables;

namespace ShipeX2.Identity.Services
{
    public class CarrierServices : ICarrierListServices
    {
        private readonly ILogger<CarrierServices> _logger;
        private readonly ApplicationDbContext _context;
        private readonly CurrentUser _currentUser;

        public CarrierServices ( ILogger<CarrierServices> logger, ApplicationDbContext context, CurrentUser currentUser )
        {
            _logger = logger;
            _context = context;
            _currentUser = currentUser;
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

        public async Task<ApiResult> CreateCarrierAsync ( ModelShipCarrier model )
        {
            ApiResult result = new ApiResult();
            try
            {
                var carrier = new ShipCarrier
                {
                    CarrierName = model.CarrierName,
                    DefaultAccountNo = model.DefaultAccountNo,
                    ApiKey1 = model.ApiKey1,
                    ApiKey2 = model.ApiKey2,
                    ApiKey3 = model.ApiKey3,
                    CreatedBy = _currentUser.GetCurrentUserId(),
                    CreatedDate = DateTime.UtcNow
                };
                _context.ShipCarriers.Add(carrier);
                int i =  await _context.SaveChangesAsync();
                if (i > 0)
                {
                    result.IsSuccessful = true;
                    result.Message = "Carrier created successfully.";
                    result.Data = carrier;
                }
                else
                {
                    result.IsSuccessful = false;
                    result.Message = "Failed to create carrier.";
                    result.Data = Array.Empty<string>();
                }
            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.Message = "Error saving carrier: " + ex.Message;
                result.Data = Array.Empty<string>();
                _logger.LogError("Error in CarrierServices (CreateCarrierAsync): " + ex.Message);
            }

            return result;
        }

        public async Task<ApiResult> ToggleModeAsync ( long id )
        {
            ApiResult apiResult = new ApiResult();
            try
            {
                var carrier = await _context.ShipCarriers.FindAsync(id);
                if (carrier == null)
                {
                    return new ApiResult { IsSuccessful = false, Message = "Carrier not found." };
                }
                carrier.Mode = !carrier.Mode;
                int i =  await _context.SaveChangesAsync();
                if (i > 0)
                {
                    apiResult.IsSuccessful = true;
                    apiResult.Message = "Mode changed successfully";
                }
                else
                {
                    apiResult.IsSuccessful = false;
                    apiResult.Message = "Failed to update mode.";
                }
            }
            catch (Exception ex)
            {
                apiResult.Message = $"Error updating mode: {ex.Message}";
                apiResult.IsSuccessful = false;
                _logger.LogError("Error in CarrierServices (ToggleModeAsync): " + ex.Message);
            }
            return apiResult;
        }

        public async Task<ApiResult> ToggleCarrierStatusAsync ( long carrierId )
        {
            ApiResult apiResult = new ApiResult();
            try
            {
                var carrier = await _context.ShipCarriers.FindAsync(carrierId);
                if (carrier == null)
                {
                    apiResult.IsSuccessful = false;
                    apiResult.Message = "Carrier not found.";
                    apiResult.Data = Array.Empty<string>();
                    return apiResult;
                }
                carrier.Status = !carrier.Status;
                int i = await _context.SaveChangesAsync();
                if (i > 0)
                {
                    apiResult.IsSuccessful = true;
                    apiResult.Message = "Carrier status updated successfully.";
                    apiResult.Data = Array.Empty<string>();
                }
                else
                {
                    apiResult.IsSuccessful = false;
                    apiResult.Message = "Failed to update carrier status.";
                    apiResult.Data = Array.Empty<string>();
                }
            }
            catch (Exception ex)
            {
                apiResult.IsSuccessful = false;
                apiResult.Message = "An error occurred while updating the carrier status.";
                _logger.LogError("Error in CarrierServices (ToggleCarrierStatusAsync): " + ex.Message);
                throw;
            }
            return apiResult;
        }

    }
}
