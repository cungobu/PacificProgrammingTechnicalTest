using Common.AOP.Interceptors;
using System.Reflection;

namespace Common.AOP
{
    public static class InterceptorExtension
    {
        public static readonly Type[] DefaultInterceptors = new Type[] 
        {
            typeof(TimeTraceInterceptor<>)
        };

        public static object Decorate(this object decorated, Type decoratedInterfaceType, bool includedOptionalInterceptor = true, IServiceProvider serviceProvider = null)
        {
            if (decorated != null)
            {
                List<Type> interceptorTypes = new List<Type>();
                interceptorTypes.AddRange(DefaultInterceptors);
                
                if (includedOptionalInterceptor)
                {
                    var optionalInterceptors = decorated.GetType().GetCustomAttributes<InterceptorAttribute>(true);
                    interceptorTypes.AddRange(optionalInterceptors.Select(c=>c.InterceptorType));

                    optionalInterceptors = decoratedInterfaceType.GetCustomAttributes<InterceptorAttribute>(true);
                    interceptorTypes.AddRange(optionalInterceptors.Select(c => c.InterceptorType));
                }

                foreach(var interceptorType in interceptorTypes.ToHashSet())
                {
                    decorated = InterceptorDecorator.Decorate(decorated, decoratedInterfaceType, interceptorType, serviceProvider);
                }
            }
            return decorated;
        }

        public static T Decorate<T>(this T decorated, bool includedOptionalInterceptor = true, IServiceProvider serviceProvider = null)
        {
            Type decoratedInterfaceType = typeof(T);
            return (T)Decorate(decorated, decoratedInterfaceType, includedOptionalInterceptor, serviceProvider);            
        }
    }
}
