using Common.Mapper;
using Microsoft.Extensions.DependencyInjection;

namespace Common
{
    public class AppBoostraper
    {
        private readonly IServiceCollection services;
        private readonly MapperFactory mapperFactory;
        private List<IModule> Modules = new();
        
        public AppBoostraper(IServiceCollection services)
        {
            this.services = services;
        }

        public void RegisterModule(IModule module)
        {
            this.Modules.Add(module);
            module.RegisterService(services);

            var mapperProfiles = module.GetMapperProfiles();         
            mapperFactory.RegisterMapper(mapperProfiles);
        }

        public void RegisterModule<TModule>() where TModule : class, IModule
        {
            TModule module = Activator.CreateInstance<TModule>();
            this.RegisterModule(module);
        }

        public void Start(IServiceProvider serviceProvider)
        {
            Modules.ForEach(module => module.OnStart(serviceProvider));
        }
    }
}
