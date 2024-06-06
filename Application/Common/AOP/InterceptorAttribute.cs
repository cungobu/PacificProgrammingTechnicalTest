namespace Common.AOP
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class InterceptorAttribute : Attribute
    {
        public InterceptorAttribute(Type interceptorType)
        {
            InterceptorType = interceptorType;
        }

        public Type InterceptorType { get; }
    }
}
