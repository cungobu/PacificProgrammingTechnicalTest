using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Common.Logging.LogBuilder;
using Common.Logging;

namespace Common.AOP.Interceptors
{
    public abstract class GenericInterceptor<T> : DispatchProxy, IInterceptor<T>
    {
        protected TracingLogMessageBuilder _logMessageBuilder = new TracingLogMessageBuilder();
        protected T _decorated;
        protected IServiceProvider _serviceProvider;
        protected ILogger<T> Logger => GetLogger();

        public void SetInstance(T instance)
        {
            _decorated = instance ?? throw new ArgumentNullException(nameof(instance));
        }

        public void SetInstance(object instance)
        {
            if(instance == null) throw new ArgumentNullException(nameof(instance));
            if (!(instance is T)) throw new InvalidCastException(nameof(instance));
            SetInstance((T)instance);
        }

        public void SetServiceContainer(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        private ILogger<T> GetLogger()
        {
            var logger = this._serviceProvider.GetRequiredService<ILogger<T>>();
            _logMessageBuilder.AppendUserInfoToLog = true;
            logger.SetLogMessageBuilder(this._logMessageBuilder);
            return logger;
        }
    }
}
