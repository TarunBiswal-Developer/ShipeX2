using static ShipeX2.Application.DTOs.ClientModel;

namespace ShipeX2.Application.Interfaces
{
    public interface IClientServices
    {
        Task<List<ClientViewModel>> GetAllClientsAsync();
    }
}
