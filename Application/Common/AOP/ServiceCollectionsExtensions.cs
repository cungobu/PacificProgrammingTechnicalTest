namespace Common.AOP
{
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;

    public static class ServiceCollectionsExtensions
    {
        public static IServiceCollection DecorateBy<TInterface, TProxy>(this IServiceCollection services)
            where TInterface : class
            where TProxy : DispatchProxy
        {
            return DecorateBy(services, typeof(TInterface), typeof(TProxy));
        }

        public static IServiceCollection DecorateBy(this IServiceCollection services, Type serviceType, Type interceptorType)
        {
            MethodInfo createMethod;
            try
            {
                createMethod = interceptorType
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .First(info => !info.IsGenericMethod && info.ReturnType == serviceType);
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException($"Looks like there is no static method in {interceptorType} " +
                                                    $"which creates instance of {serviceType} (note that this method should not be generic)", e);
            }

            var argInfos = createMethod.GetParameters();

            // Save all descriptors that needs to be decorated into a list.
            var descriptorsToDecorate = services
                .Where(s => s.ServiceType == serviceType)
                .ToList();

            if (descriptorsToDecorate.Count == 0)
            {
                throw new InvalidOperationException($"Attempted to Decorate services of type {serviceType}, " +
                                                    "but no such services are present in ServiceCollection");
            }

            foreach (var descriptor in descriptorsToDecorate)
            {
                var decorated = ServiceDescriptor.Describe(
                    serviceType,
                    sp =>
                    {
                        var decoratorInstance = createMethod.Invoke(null,
                            argInfos.Select(
                                    info => info.ParameterType == (descriptor.ServiceType ?? descriptor.ImplementationType)
                                        ? sp.CreateInstance(descriptor)
                                        : sp.GetRequiredService(info.ParameterType))
                                .ToArray());

                        return decoratorInstance;
                    },
                    descriptor.Lifetime);

                services.Remove(descriptor);
                services.Add(decorated);
            }

            return services;
        }

        public static IServiceCollection Decorate(this IServiceCollection services, Type serviceType)
        {
            // Save all descriptors that needs to be decorated into a list.
            var descriptorsToDecorate = services.Where(s => s.ServiceType == serviceType).ToList();
            
            if (descriptorsToDecorate.Count() == 0)
            {
                throw new InvalidOperationException($"Attempted to Decorate services of type {serviceType}, " +
                                                    "but no such services are present in ServiceCollection");
            }

            DecorateServices(services, descriptorsToDecorate);

            return services;
        }

        public static void DecorateServices(this IServiceCollection services, IEnumerable<ServiceDescriptor> descriptorsToDecorate)
        {
            foreach (var descriptor in descriptorsToDecorate.ToArray())
            {
                var decorated = ServiceDescriptor.Describe(
                    descriptor.ServiceType,
                    sp =>
                    {
                        var serviceInstanceType = descriptor.ServiceType ?? descriptor.ImplementationType;
                        var serviceInstance = sp.CreateInstance(descriptor);
                        var decoratorInstance = serviceInstance.Decorate(serviceInstanceType, true, sp);
                        return decoratorInstance;
                    },
                    descriptor.Lifetime);

                services.Remove(descriptor);
                services.Add(decorated);
            }
        }

        public static IServiceCollection Decorate<TInterface>(this IServiceCollection services)
        {
            Type serviceType = typeof(TInterface);
            return Decorate(services, serviceType);
        }        

        private static object CreateInstance(this IServiceProvider services, ServiceDescriptor descriptor)
        {
            if (descriptor.ImplementationInstance != null)
            {
                return descriptor.ImplementationInstance;
            }

            if (descriptor.ImplementationFactory != null)
            {
                return descriptor.ImplementationFactory(services);
            }

            return ActivatorUtilities.GetServiceOrCreateInstance(services, descriptor.ImplementationType);
        }
    }

}