namespace Bars.Esia.OAuth20.App.Entities
{
    using System;

    using Bars.B4.Utils;
    using Bars.Esia.OAuth20.App.Enums;

    /// <summary>
    /// Запрос к сервису авторизации
    /// </summary>
    public class AuthAppRequest
    {
        /// <summary>
        /// GUID сообщения
        /// </summary>
        public Guid MessageGuid { get; set; }

        /// <summary>
        /// Код операции
        /// </summary>
        public AuthAppOperationCode AuthAppOperationCode { get; set; }

        /// <summary>
        /// Параметры операции
        /// </summary>
        public DynamicDictionary Params { get; set; }

        public AuthAppRequest()
        {
            this.MessageGuid = Guid.NewGuid();
        }
    }
}