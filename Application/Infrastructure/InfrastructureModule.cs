using Common;
using Common.AOP;
using Infrastructure.Contexts;
using Infrastructure.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public class InfrastructureModule : IModule
    {
        public Type[] GetMapperProfiles()
        {
            return Array.Empty<Type>();
        }

        public IServiceCollection RegisterService(IServiceCollection services)
        {
            RegisterInfrastructureServices(services);
            RegisterInterceptors(services);
            return services;
        }

        private IServiceCollection RegisterInfrastructureServices(IServiceCollection services)
        {
            services.AddScoped(typeof(IDbFactory<>), typeof(DbFactory<>));
            services.AddScoped<Domain.IUnitOfWork>(c => c.GetService<IUnitOfWork<ApplicationDbContext>>());
            services.AddScoped(typeof(IUnitOfWork<ApplicationDbContext>), typeof(UnitOfWork<ApplicationDbContext>));

            services.AddTransient(typeof(Repository<,>));
            services.AddTransient(typeof(ReadRepository<,>));

            #region Add customized entity repositories
            #endregion

            return services;
        }

        private IServiceCollection RegisterInterceptors(IServiceCollection services)
        {
            var decoratedTypes = AssemblyTypes.Where(c => c.Name.EndsWith("Repository", StringComparison.CurrentCultureIgnoreCase) && c.IsInterface).ToList();
            decoratedTypes.Add(typeof(IUnitOfWork<>));

            var serviceDescriptors = services.Where(c => decoratedTypes.Contains(c.ServiceType)).ToArray();
            services.DecorateServices(serviceDescriptors);
            return services;
        }

        private static readonly Lazy<Type[]> assemblyTypes = new Lazy<Type[]>(() => typeof(InfrastructureModule).Assembly.GetTypes());

        public static Type[] AssemblyTypes
        {
            get { return assemblyTypes.Value; }
        }

        public void OnStart(IServiceProvider serviceProvider)
        {
        }
    }
}
