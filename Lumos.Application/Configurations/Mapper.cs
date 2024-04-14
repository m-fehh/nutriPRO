using AutoMapper;
using NutriPro.Application.Dtos.Management;
using NutriPro.Application.Dtos.Management.Tenants;
using NutriPro.Data.Models.Management;

namespace NutriPro.Application.Configurations
{
    public static class Mapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Users, UsersDto>().ReverseMap();
            configuration.CreateMap<Tenants, TenantsDto>().ReverseMap();
            configuration.CreateMap<Units, UnitsDto>().ReverseMap();
        }
    }
}
