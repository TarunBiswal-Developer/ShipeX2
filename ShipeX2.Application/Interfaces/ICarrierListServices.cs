using ShipeX2.Application.DTOs;

namespace ShipeX2.Application.Interfaces
{
    public interface ICarrierListServices
    {
        Task<List<ModelShipCarrier>> GetCarriersAsync ();
    }
}
