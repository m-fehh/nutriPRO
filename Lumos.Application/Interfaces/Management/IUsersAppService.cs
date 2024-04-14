using NutriPro.Application.Configurations;
using NutriPro.Application.Models;
using NutriPro.Data.Models.Management;

namespace NutriPro.Application.Interfaces.Management
{
    public interface IUsersAppService : ITransientDependency
    {
        Task<PaginationResult<Users>> GetAllPaginatedAsync(UserDataTableParams dataTableParams, long? tenantId, List<long> organizationId, bool isHost);
        Task<Users?> ValidateUserCredentials(string email, string password);
        Task CreateAsync(Users entity);
        Task UpdateAsync(Users entity);
    }
}
