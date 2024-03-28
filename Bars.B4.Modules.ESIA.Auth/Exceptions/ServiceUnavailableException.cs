namespace Bars.B4.Modules.ESIA.Auth.Exceptions
{
    using System;

    /// <summary>
    /// Исключения для создания HTTP ответа с кодом 503
    /// </summary>
    public class ServiceUnavailableException : Exception
    {
        public ServiceUnavailableException(string message)
            : base(message)
        {
        }
    }
}