using Common.AOP.Interceptors;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;

namespace Common.AOP
{
    public class LoggingInterceptor<TDecorated> : GenericInterceptor<TDecorated>
    {
        private const string MessageLogTemplate = "{Namespace}.{MethodName} is called at {CallerNamespace}.{CallerMethodName} ({CallerFileName} at line #{CallerFileLineNumber}).";
        private const string MessageLogTemplateWithoutStackTrace = "{Namespace}.{MethodName} is called.";

        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            try
            {
                this.Logger.LogDebug("{TargetMethod} - Start.", targetMethod.Name);
                var result = targetMethod?.Invoke(base._decorated, args);
                this.Logger.LogDebug("{TargetMethod} - Finish.", targetMethod.Name);
                return result;
            }
            catch (Exception ex)
            {
                this.Logger.LogError("{TargetMethod} - Error ocurred: {ErrorMessage}", targetMethod.Name, ex.Message, ex);
                throw new Common.Validation.ApplicationException(ex.InnerException);
            }
            finally
            {
                Log(targetMethod);
            }
        }

        protected virtual void Log(MethodInfo? targetMethod)
        {
            StackFrame frame = GetCallerStackFrame();
            var methodClass = base._decorated.GetType();

            if (frame != null)
            {
                MethodBase callerMethod = frame.GetMethod();
                this.Logger.LogInformation(MessageLogTemplate, methodClass.FullName, targetMethod.Name, callerMethod.DeclaringType.FullName, callerMethod.Name, frame.GetFileName(), frame.GetFileLineNumber());
            }
            else
            {
                this.Logger.LogInformation(MessageLogTemplateWithoutStackTrace, methodClass.FullName, targetMethod.Name);
            }

            this._logMessageBuilder.Reset();
        }

        private static StackFrame GetCallerStackFrame()
        {
            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace(true);
            
            StackFrame frame = default(StackFrame);
            
            for (int i = 0; i < stackTrace.FrameCount; i++)
            {
                var currentFrame = stackTrace.GetFrame(i);
                var callerClass = currentFrame.GetMethod().DeclaringType;
                
                if (currentFrame.GetFileName() != null 
                    && callerClass.Name != null 
                    && !callerClass.Name.Contains("generatedProxy")
                    && !callerClass.Name.Contains("Interceptor"))
                {
                    frame = currentFrame;
                    break;
                }
            }

            return frame;
        }

        public static TDecorated Create(TDecorated decorated)
        {
            object proxy = Create<TDecorated, LoggingInterceptor<TDecorated>>();
            ((LoggingInterceptor<TDecorated>)proxy).SetParameters(decorated);

            return (TDecorated)proxy;
        }

        private void SetParameters(TDecorated decorated)
        {
            _decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
        }
    }
}
