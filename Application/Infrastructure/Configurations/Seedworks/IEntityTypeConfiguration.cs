using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Seedworks
{
    internal interface IEntityTypeConfiguration
    {
        void Configure(EntityTypeBuilder builder);
    }
}
