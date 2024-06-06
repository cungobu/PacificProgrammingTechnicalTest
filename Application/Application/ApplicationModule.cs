using Common;
using Common.AOP;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public class ApplicationModule : IModule
    {
        public Type[] GetMapperProfiles()
        {
            return new Type[] { typeof(Profiles.DefaultProfile) };
        }

        public IServiceCollection RegisterService(IServiceCollection services)
        {
            RegisterApplicationServices(services);
            RegisterInterceptors(services);
            return services;
        }

        private IServiceCollection RegisterApplicationServices(IServiceCollection services)
        {
            var applicationServiceTypes = AssemblyTypes.Where(c => c.Name.EndsWith("Service", StringComparison.CurrentCultureIgnoreCase) && c.IsInterface).ToList();
            foreach (var serviceType in applicationServiceTypes)
            {
                var implementationTypes = AssemblyTypes.Where(c => c.IsAssignableTo(serviceType) && c.IsClass);
                if (implementationTypes.Count() == 1)
                {
                    services.AddScoped(serviceType, implementationTypes.First());
                }
            }
            return services;
        }

        private IServiceCollection RegisterInterceptors(IServiceCollection services)
        {
            var applicationServiceTypes = AssemblyTypes.Where(c => c.Name.EndsWith("Service", StringComparison.CurrentCultureIgnoreCase) && c.IsInterface).ToList();
            var serviceDescriptors = services.Where(c => applicationServiceTypes.Contains(c.ServiceType)).ToArray();
            services.DecorateServices(serviceDescriptors);
            return services;
        }

        private static readonly Lazy<Type[]> assemblyTypes = new Lazy<Type[]>(() => typeof(ApplicationModule).Assembly.GetTypes());

        private static Type[] AssemblyTypes
        {
            get { return assemblyTypes.Value; }
        }

        public void OnStart(IServiceProvider serviceProvider)
        {
        }
    }
}
