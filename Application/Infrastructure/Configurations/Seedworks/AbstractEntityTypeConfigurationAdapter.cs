using Common.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Seedworks
{
    public abstract class AbstractEntityTypeConfigurationAdapter<T> : IEntityTypeConfiguration<T>, IEntityTypeConfiguration where T : class
    {
        public abstract void Configure(EntityTypeBuilder<T> builder);

        void IEntityTypeConfiguration.Configure(EntityTypeBuilder builder)
        {
            var genericBuilder = builder as EntityTypeBuilder<T>;
            ThrowError.ThrowIfNull(genericBuilder, nameof(builder), $"Type mismatched. Entity Builder is not {typeof(EntityTypeBuilder<T>).Name}");
            this.Configure(genericBuilder);
        }
    }
}
