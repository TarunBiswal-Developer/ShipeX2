using ShipeX2.Application.DTOs;
using ShipeX2.Application.Wrappers;

namespace ShipeX2.Application.Interfaces
{
    public interface ICarrierSetupServices
    {
        Task<ModelShipCarrierService> GetCarrierSetupListAsync ();
        Task<ApiResult> GetPackingListAsync(int carrierId);

        Task<ApiResult> GetCarrierPackingById(int packingId);
        Task<ApiResult> UpdateCarrierPackingAsync(PackingViewModel model);
        Task<ApiResult> CreateCarrierPackingAsync(PackingViewModel model);
    }
}
