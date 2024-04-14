using NutriPro.Application.Configurations;
using NutriPro.Application.Models;
using NutriPro.Data.Models.Management;

namespace NutriPro.Application.Interfaces.Management
{
    public interface ITenantsAppService : ITransientDependency
    {
        Task<List<Tenants>> GetAllAsync();
        Task<TId> InsertAndGetIdAsync<TId>(Tenants entity);
        Task<Tenants> GetByIdAsync<TId>(TId id);
    }
}
