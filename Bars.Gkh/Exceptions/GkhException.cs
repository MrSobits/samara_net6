namespace Bars.Gkh.Exceptions
{
    using System;

    /// <summary>
    /// Базовый класс для приложения
    /// </summary>
    [Serializable]
    public class GkhException : Exception
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public GkhException() : this("Ошибка выполнения операции")
        {
            
        }

        /// <summary>
        /// .ctor
        /// </summary>
        public GkhException(string message) : this(message, null)
        {
            
        }

        /// <summary>
        /// .ctor
        /// </summary>
        public GkhException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}