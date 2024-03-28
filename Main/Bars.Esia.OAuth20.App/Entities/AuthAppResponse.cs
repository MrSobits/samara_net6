namespace Bars.Esia.OAuth20.App.Entities
{
    using System;

    /// <summary>
    /// Ответ сервиса авторизации
    /// </summary>
    public class AuthAppResponse
    {
        /// <summary>
        /// GUID сообщения
        /// </summary>
        public Guid MessageGuid { get; set; }

        /// <summary>
        /// Успешность выполнения
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Результат выполнения
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Перечень ошибок
        /// </summary>
        public object Errors { get; set; }
    }
}