using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

namespace Infrastructure.Configurations.Seedworks
{
    public class PropertyTypeConverterConfiguration<T> : AbstractEntityTypeConfigurationAdapter<T> where T : class
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            foreach (var property in typeof(T).GetProperties())
            {
                var propertyType = GetUnderlyingPropertyType(property);

                if (property.PropertyType.IsAssignableTo(typeof(string)))
                {
                    builder.Property(property.Name).IsRequired(false);
                }
            }
        }

        private static Type GetUnderlyingPropertyType(PropertyInfo property)
        {
            var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            return type;
        }
    }
}
