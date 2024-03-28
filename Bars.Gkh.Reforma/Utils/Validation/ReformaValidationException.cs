namespace Bars.Gkh.Reforma.Utils.Validation
{
    using System;

    using Bars.B4;

    /// <summary>
    /// Исключение проверки интеграции Реформы
    /// </summary>
    internal class ReformaValidationException : ValidationException
    {
        /// <inheritdoc />
        public ReformaValidationException(string message)
            : base(message)
        {
        }

        /// <inheritdoc />
        public ReformaValidationException(string message, Exception exception)
            : base(message, exception)
        {
        }

        /// <inheritdoc />
        public ReformaValidationException(string message, Type entityType, string code)
            : base(message, entityType, code)
        {
        }
    }
}