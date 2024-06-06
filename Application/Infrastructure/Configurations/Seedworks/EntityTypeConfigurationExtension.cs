using System.Data;

namespace Infrastructure.Configurations.Seedworks
{
    public static class EntityTypeConfigurationExtension
    {
        public static Type[] GetSupportedConfigurations(Type entityType)
        {
            var interfaceType = typeof(IEntityTypeConfiguration);
            var configurations = interfaceType.Assembly.GetTypes().Where(c => c.IsAssignableTo(interfaceType) && c.IsClass && !c.IsAbstract).ToArray();
            return configurations.Where(c => c.SupportFor(entityType)).Select(c => c.MakeGenericType(entityType)).ToArray();
        }

        public static bool SupportFor(this Type configurationType, Type entityType)
        {
            var arguments = configurationType.GetGenericArguments();
            
            foreach (var argument in arguments)
            {
                var constraints = argument.GetGenericParameterConstraints();
                foreach(var constraint in constraints)
                {
                    if(constraint.IsAssignableFrom(entityType))
                    {
                        return true;
                    }
                    
                    if(constraint.IsParentOf(entityType))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool IsParentOf(this Type genericType, Type entityType)
        {
            if (genericType.IsAbstract && genericType.IsGenericType)
            {
                var genericTypeDefinition = genericType.GetGenericTypeDefinition();

                if (entityType.IsGenericType && genericTypeDefinition.IsAssignableFrom(entityType.GetGenericTypeDefinition()))
                {
                    return true;
                }
                else
                {
                    entityType = entityType.BaseType;
                    if (entityType != null)
                    {
                        var result = genericType.IsParentOf(entityType);
                        return result;
                    }
                }
            }

            return false;
        }
    }
}
