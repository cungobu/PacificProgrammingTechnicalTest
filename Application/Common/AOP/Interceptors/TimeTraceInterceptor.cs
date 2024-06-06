using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;

namespace Common.AOP.Interceptors
{
    public class TimeTraceInterceptor<T> : LoggingInterceptor<T>
    {
        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            Exception exception = null;
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                this.Logger.LogDebug("{TargetMethod} - Start.", targetMethod.Name);
                var result = targetMethod?.Invoke(base._decorated, args);
                this.Logger.LogDebug("{TargetMethod} - Finish.", targetMethod.Name);
                return result;
            }
            catch (Exception ex)
            {
                exception = ex;
                throw new Common.Validation.ApplicationException(ex.InnerException);
            }
            finally
            {
                stopwatch.Stop();
                this._logMessageBuilder.BenchmarkTimespan = stopwatch.Elapsed;
                
                if (exception != null)
                {
                    this.Logger.LogError("{TargetMethod} - Error ocurred: {ErrorMessage}", targetMethod.Name, exception.Message, exception);
                }
                else
                {
                    this.Log(targetMethod);
                }

                this._logMessageBuilder.Reset();
            }
        }
    }
}
