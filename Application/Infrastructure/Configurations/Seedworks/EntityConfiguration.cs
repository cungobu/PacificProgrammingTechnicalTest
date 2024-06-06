using Infrastructure.Configurations.Seedworks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Infrastructure.Configurations
{
    public class EntityConfiguration<T> : IEntityTypeConfiguration<T> where T : class
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            PropertyTypeConverterConfiguration<T> standardPropertyConverterConfiguration = new PropertyTypeConverterConfiguration<T>();
            standardPropertyConverterConfiguration.Configure(builder);

            var entityType = typeof(T);
            builder.ToTable(entityType.Name);

            //var primaryKeyProperty = entityType.GetProperties().FirstOrDefault(c => c.GetCustomAttribute<KeyAttribute>() != null);
            //if(primaryKeyProperty != null)
            //{
            //    builder.HasKey(primaryKeyProperty.Name);
            //}

            var supportedConfigurationTypes = EntityTypeConfigurationExtension.GetSupportedConfigurations(entityType);
            foreach(var configurationType in supportedConfigurationTypes)
            {
                IEntityTypeConfiguration configuration = Activator.CreateInstance(configurationType) as IEntityTypeConfiguration;
                configuration.Configure(builder);
            }
        }
    }
}
