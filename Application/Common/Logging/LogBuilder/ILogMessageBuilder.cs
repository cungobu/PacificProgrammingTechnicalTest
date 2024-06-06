namespace Common.Logging.LogBuilder
{
    public interface ILogMessageBuilder
    {
        string ToStringTemplate(string message);
        object[] ToArguments(object[] args);
    }
}
