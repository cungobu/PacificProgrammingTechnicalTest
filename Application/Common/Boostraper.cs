using Common.Mapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common
{
    public class Boostraper
    {
        private IServiceCollection services;
        private MapperFactory mapperFactory;
        private List<IModule> Modules = new();
        private IConfiguration _configuration;

        public Boostraper()
        {
        }

        public Boostraper(IServiceCollection services)
        {
            this.SetServices(services);
        }

        public void SetServices(IServiceCollection services)
        {
            this.services = services;
            this.mapperFactory = new MapperFactory(services);
        }

        public void SetConfiguration(IConfiguration configuration)
        {
            this._configuration = configuration;
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
