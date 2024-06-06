namespace Common.AOP
{
    public interface IInterceptor<T> : IInterceptor
    {
        void SetInstance(T instance);
    }

    public interface IInterceptor
    {
        void SetInstance(object instance);
        void SetServiceContainer(IServiceProvider serviceProvider);
    }
}
