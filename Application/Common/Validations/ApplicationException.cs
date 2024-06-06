namespace Common.Validation
{
    public class ApplicationException : Exception
    {
        public ApplicationException(Exception innerException)
            : base(innerException.Message, innerException)
        {
        }

        public override string? StackTrace => InnerException.StackTrace;
        public override string Message => InnerException.Message;
    }
}
