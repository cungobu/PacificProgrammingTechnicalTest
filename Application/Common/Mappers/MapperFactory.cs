using Microsoft.Extensions.DependencyInjection;

namespace Common.Mapper
{
    public class MapperFactory
    {
        private readonly IServiceCollection services;

        public MapperFactory(IServiceCollection services)
        {
            this.services = services;
            services.AddSingleton<IMapper, Mapper>();
        }

        public MapperFactory RegisterMapper(params Type[] types)
        {
            if (types != null && types.Length > 0)
            {
                services.AddAutoMapper(types);
            }

            return this;
        }
    }
}
