namespace Common.Helpers
{
    using System;

    /// <summary>
    /// Utility class for throwing exception when matching a specific condition.
    /// </summary>
    public static class ThrowError
    {
        /// <summary>
        /// Throw exception if matched
        /// </summary>
        /// <typeparam name="TException">The exception type</typeparam>
        /// <param name="condition">The condition</param>
        /// <param name="errorMessage">Error message</param>
        public static void ThrowIfMatch<TException>(bool condition, string errorMessage)
            where TException : Exception
        {
            if (condition)
            {
                TException exception = (TException)Activator.CreateInstance(typeof(TException), errorMessage);
                throw exception;
            }
        }

        /// <summary>
        /// Throw exception if matched.
        /// </summary>
        /// <param name="condition">The condition</param>
        /// <param name="errorMessage">Error message</param>
        public static void ThrowIfMatch(bool condition, string errorMessage)
        {
            ThrowIfMatch<ArgumentException>(condition, errorMessage);
        }

        /// <summary>
        /// Throw exception if the object is null
        /// </summary>
        /// <param name="obj">The object to be checked</param>
        /// <param name="parameterName">Name of the object</param>
        /// <param name="errorMessage">Error message</param>
        public static void ThrowIfNull(object? obj, string parameterName, string errorMessage = null)
        {
            errorMessage = errorMessage ?? $"Parameter '{parameterName}' is null";
            ThrowIfMatch<ArgumentNullException>(obj == null, errorMessage);
        }
    }
}
