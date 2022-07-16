using System.Runtime.CompilerServices;

namespace Api.Common
{
    public static class Exceptions
    {
        public static void ThrowIf(
            object? value,
            Func<object?, bool> expression, 
            string message,
            [CallerArgumentExpression("value")] string? paramName = null)
        {
            if (expression(value) == false)
            {
                throw new ArgumentException(message, paramName);
            }
        }
    }
}
