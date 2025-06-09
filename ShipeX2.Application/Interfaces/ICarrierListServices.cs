using ShipeX2.Application.DTOs;
using ShipeX2.Application.Wrappers;

namespace ShipeX2.Application.Interfaces
{
    public interface ICarrierListServices
    {
        Task<List<ModelShipCarrier>> GetCarriersAsync ();
        Task<ApiResult> CreateCarrierAsync(ModelShipCarrier model);
        Task<ApiResult> ToggleModeAsync(long id);
        Task<ApiResult> ToggleCarrierStatusAsync(long carrierId);
    }
}
