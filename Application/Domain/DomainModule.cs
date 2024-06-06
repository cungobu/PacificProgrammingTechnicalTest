using Common;
using Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Domain
{
    public class DomainModule : IModule
    {
        public Type[] GetMapperProfiles()
        {
            return Array.Empty<Type>();
        }

        public IServiceCollection RegisterService(IServiceCollection services)
        {
            RegisterDomainServices(services);
            RegisterInterceptors(services);
            return services;
        }

        private IServiceCollection RegisterDomainServices(IServiceCollection services)
        {
            services.AddScoped<UnitOfWorkTransactionProvider, UnitOfWorkTransactionProvider>();
            return services;
        }

        private IServiceCollection RegisterInterceptors(IServiceCollection services)
        {
            return services;
        }

        public void OnStart(IServiceProvider serviceProvider)
        {

        }
    }
}
