using ShipeX2.Application.Wrappers;
using static ShipeX2.Application.DTOs.ShipCareer;

namespace ShipeX2.Application.Interfaces
{
    public interface IShipment
    {
         Task<ApiResult<List<ClientCarrier>>> ClientsCareerAsync(long clientId);
         Task<ApiResult<ClientCarrier>> ClientsDefaultAccountInfo(long careerId);
         Task<ApiResult<List<CarrierService>>> ShipServiceListAsync(long careerId);
         Task<ApiResult<List<CarrierPacking>>> RetrievePackagingAsync ( string shipViaCode, decimal weight );
        
    }
}
