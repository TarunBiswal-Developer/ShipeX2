using static ShipeX2.Application.DTOs.ImporterModel;

namespace ShipeX2.Application.Interfaces
{
    public interface IImporterService
    {
        Task<List<ImporterExtended>> GetAllImporters();

    }
}
