using Common;

namespace TechnicalTest
{
    public class WebAPIModule : IModule
    {
        public Type[] GetMapperProfiles()
        {
            return new Type[] { typeof(Profiles.DefaultProfile) };
        }

        public IServiceCollection RegisterService(IServiceCollection services)
        {
            services.AddControllers().AddControllersAsServices();
            
            // Model factory services
            return services;
        }

        public void OnStart(IServiceProvider serviceProvider)
        {
        }
    }
}
