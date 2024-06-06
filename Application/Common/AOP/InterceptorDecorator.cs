using System.Reflection;

namespace Common.AOP
{
    public class InterceptorDecorator
    {
        public static TDecorated Decorate<TDecorated, TInterceptor>(TDecorated decorated, IServiceProvider serviceProvider = null)
            where TInterceptor : DispatchProxy, IInterceptor<TDecorated>
        {
            var decoratedInstance = DispatchProxy.Create<TDecorated, TInterceptor>();
            SetupInterceptor(decoratedInstance, decorated, serviceProvider);
            return decoratedInstance;
        }

        public static object Decorate(object decorated, Type decoratedInterfaceType, Type interceptorType, IServiceProvider serviceProvider = null)
        {
            var interceptorConcreteType = interceptorType.IsGenericTypeDefinition ? interceptorType.MakeGenericType(decoratedInterfaceType) : interceptorType;
            var decoratedInstance = DispatchProxy.Create(decoratedInterfaceType, interceptorConcreteType);
            SetupInterceptor(decoratedInstance, decorated, serviceProvider);
            return decoratedInstance;
        }

        private static void SetupInterceptor(object decoratedInstance, object decorated, IServiceProvider serviceProvider = null)
        {
            var interceptor = decoratedInstance as IInterceptor;
            if (interceptor != null)
            {
                interceptor.SetInstance(decorated);
                interceptor.SetServiceContainer(serviceProvider);
            }
        }
    }
}
