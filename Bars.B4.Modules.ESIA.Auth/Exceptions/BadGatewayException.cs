namespace Bars.B4.Modules.ESIA.Auth.Exceptions
{
    using System;

    /// <summary>
    /// Исключения для создания HTTP ответа с кодом 502
    /// </summary>
    public class BadGatewayException : Exception
    {
        public BadGatewayException(string message)
            : base(message)
        {
        }
    }
}