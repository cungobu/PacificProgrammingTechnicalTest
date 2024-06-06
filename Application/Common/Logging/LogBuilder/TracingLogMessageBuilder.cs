using System.Text;

namespace Common.Logging.LogBuilder
{
    public class TracingLogMessageBuilder : ILogMessageBuilder
    {
        public bool AppendUserInfoToLog { get; set; }
        public TimeSpan? BenchmarkTimespan { get; set; }

        protected const string LogMessageTemplate = "{{DateTime}} - {0}";

        public virtual string ToStringTemplate(string message)
        {
            message = message ?? string.Empty;

            var stringBuilder = new StringBuilder().AppendFormat(LogMessageTemplate, message);
            
            if(AppendUserInfoToLog)
            {
                stringBuilder.Append(" User: {user}.");
            }

            if(BenchmarkTimespan.HasValue)
            {
                stringBuilder.Append(" Completed in {consumedTime:0} miliseconds.");
            }

            return stringBuilder.ToString();
        }

        public virtual object[] ToArguments(object[] args)
        {
            var originalLength = (args?.Length).GetValueOrDefault();
            
            object[] newArgs = new object[originalLength + 1];
            newArgs[0] = DateTime.Now;

            if (AppendUserInfoToLog && BenchmarkTimespan.HasValue)
            {
                Array.Resize<object>(ref newArgs, newArgs.Length + 2);
                newArgs[newArgs.Length - 2] = "System";
                newArgs[newArgs.Length - 1] = BenchmarkTimespan.GetValueOrDefault().TotalMilliseconds;
            }
            else if (AppendUserInfoToLog)
            {
                Array.Resize<object>(ref newArgs, newArgs.Length + 1);
                newArgs[newArgs.Length - 1] = "System";
            }
            else if (BenchmarkTimespan.HasValue)
            {
                Array.Resize<object>(ref newArgs, newArgs.Length + 1);
                newArgs[newArgs.Length - 1] = BenchmarkTimespan.GetValueOrDefault().TotalMilliseconds;
            }

            if (args != null)
            {
                Array.Copy(args, 0, newArgs, 1, args.Length);
            }

            return newArgs;
        }

        public void Reset()
        {
            this.BenchmarkTimespan = null;
        }
    }
}
