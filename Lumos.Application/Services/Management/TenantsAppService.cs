using NutriPro.Application.Interfaces.Management;
using NutriPro.Application.Repositories;
using NutriPro.Data.Models.Management;

namespace NutriPro.Application.Services.Management
{
    public class TenantsAppService : NutriProAppServiceBase<Tenants>, ITenantsAppService
    {
        public TenantsAppService(NutriProSession session, IRepository<Tenants> repository) : base(session, repository)
        {
        }
    }
}
