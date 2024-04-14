using NutriPro.Application.Configurations.Filters;
using NutriPro.Application.Interfaces.Management;
using NutriPro.Application.Repositories;
using NutriPro.Application.Services.Management;
using NutriPro.Data.Models.Management;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace NutriPro.Application.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterDependencies(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<NutriProSession>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Registrar automaticamente todos os tipos que implementam ITransientDependency
            RegisterTransientDependencies(services, Assembly.GetExecutingAssembly());


            #region Interface X Classe 

            services.AddScoped<IAuthService, JwtAuthService>();

            AddScopedServices<IUsersAppService, UsersAppService>(services);
            AddScopedServices<ITenantsAppService, TenantsAppService>(services);
            AddScopedServices<IUnitsAppService, UnitsAppService>(services);

            #endregion

            #region ServiceBase X AppService

            services.AddScoped<NutriProAppServiceBase<Users>, UsersAppService>(); 
            services.AddScoped<NutriProAppServiceBase<Tenants>, TenantsAppService>(); 
            services.AddScoped<NutriProAppServiceBase<Units>, UnitsAppService>(); 
            
            #endregion
        }

        public static void RegisterTransientDependencies(IServiceCollection services, Assembly assembly)
        {
            var transientTypes = assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract &&
                               typeof(ITransientDependency).IsAssignableFrom(type));

            foreach (var type in transientTypes)
            {
                services.AddTransient(type);

                // Verifica se o tipo implementa alguma interface genérica e registra essa interface também
                var interfaces = type.GetInterfaces()
                    .Where(i => i.IsGenericType && typeof(ITransientDependency).IsAssignableFrom(i));

                foreach (var @interface in interfaces)
                {
                    services.AddTransient(@interface, type);
                }
            }
        }

        public static void AddScopedServices<TService, TImplementation>(IServiceCollection services) where TService : class where TImplementation : class, TService
        {
            services.AddScoped<TService, TImplementation>();
        }
    }
}
