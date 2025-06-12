using ShipeX2.Application.DTOs;

namespace ShipeX2.Application.Interfaces
{
    public interface ICarrierSetupServices
    {
        Task<ModelShipCarrierService> GetCarrierSetupListAsync ();
    }
}
