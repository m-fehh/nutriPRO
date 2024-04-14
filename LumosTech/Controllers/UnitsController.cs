using AutoMapper;
using NutriPro.Application;
using NutriPro.Application.Dtos.Management;
using NutriPro.Data.Models.Management;

namespace NutriPro.Mvc.Controllers
{
    public class UnitsController : NutriProControllerBase<Units, UnitsDto, long>
    {

        public UnitsController(NutriProSession session, IMapper mapper, NutriProAppServiceBase<Units> Unitservice) : base(session, mapper, Unitservice) { }
    }
}
