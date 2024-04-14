using NutriPro.Application.Interfaces.Management;
using NutriPro.Application.Repositories;
using NutriPro.Data.Models.Management;

namespace NutriPro.Application.Services.Management
{
    public class UnitsAppService : NutriProAppServiceBase<Units>, IUnitsAppService
    {
        public UnitsAppService(NutriProSession session, IRepository<Units> repository) : base(session, repository)
        {
        }
    }
}
