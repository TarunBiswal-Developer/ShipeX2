using ShipeX2.Application.Wrappers;
using static ShipeX2.Application.DTOs.ShipCareer;

namespace ShipeX2.Application.Interfaces
{
    public interface IShipment
    {
        public Task<ApiResult<List<ClientCarrier>>> ClientsCareerAsync(long clientId);
        public Task<ApiResult<ClientCarrier>> ClientsDefaultAccountInfo(long careerId);
        public Task<ApiResult<List<CarrierService>>> ShipServiceListAsync(long careerId);
        public Task<ApiResult<List<CarrierPacking>>> RetrievePackagingAsync ( string shipViaCode, decimal weight );
        
    }
}
