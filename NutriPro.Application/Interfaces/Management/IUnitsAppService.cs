using NutriPro.Data.Models.Management;

namespace NutriPro.Application.Interfaces.Management
{
    public interface IUnitsAppService : ITransientDependency
    {
        Task CreateAsync(Units entity);
        Task<List<Units>> GetByListIdsAsync<TId>(List<TId> id);
    }
}
