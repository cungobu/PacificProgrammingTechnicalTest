using Microsoft.Extensions.DependencyInjection;

namespace Common
{
    public interface IModule
    {
        IServiceCollection RegisterService(IServiceCollection services);
        Type[] GetMapperProfiles();
        void OnStart(IServiceProvider serviceProvider);
    }
}
