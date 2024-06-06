using System.Text;

namespace Common.Logging.LogBuilder
{
    public class SimpleLogMessageBuilder : ILogMessageBuilder
    {
        protected const string LogMessageTemplate = "{{DateTime}} - {0}";

        public virtual string ToStringTemplate(string message)
        {
            message = message ?? string.Empty;
            return new StringBuilder().AppendFormat(LogMessageTemplate, message).ToString();
        }

        public virtual object[] ToArguments(object[] args)
        {
            var originalLength = (args?.Length).GetValueOrDefault();

            object[] newArgs = new object[originalLength + 1];
            newArgs[0] = DateTime.Now;

            if (args != null)
            {
                Array.Copy(args, 0, newArgs, 1, args.Length);
            }

            return newArgs;
        }
    }
}
