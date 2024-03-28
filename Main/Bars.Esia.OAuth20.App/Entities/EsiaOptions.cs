namespace Bars.Esia.OAuth20.App.Entities
{
    using System;

    using Bars.Esia.OAuth20.App.Enums;

    /// <summary>
    /// Параметры ЕСИА
    /// </summary>
    public class EsiaOptions
    {
        /// <summary>
        /// Набор символов в UUID, используемый для защиты от перехвата
        /// </summary>
        public Guid State { get; set; }

        /// <summary>
        /// Тип доступа
        /// </summary>
        public AccessType AccessType { get; set; }

        /// <summary>
        /// Время ожидания для выполнения HTTP-запроса
        /// </summary>
        public TimeSpan RequestTimeout { get; set; }

        /// <summary>
        /// Макс. число байт буффера при чтении ответа на HTTP-запрос
        /// </summary>
        public int MaxResponseContentBufferSize { get; set; }

        /// <summary>
        /// Отепечаток сертификата
        /// </summary>
        public string CertificateThumbPrint { get; set; }

        /// <summary>
        /// Идентификатор ИС
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Область доступа
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// Адрес, на который должна вернуть ЕСИА после авторизации
        /// </summary>
        public string CallbackUri { get; set; }

        /// <summary>
        /// Адрес для получения кода доступа из ЕСИА
        /// </summary>
        public string RedirectUri { get; set; }

        /// <summary>
        /// Адрес для получения маркера доступа из ЕСИА
        /// </summary>
        public string TokenUri { get; set; }

        /// <summary>
        /// Адрес для получения данных из ЕСИА
        /// </summary>
        public string RestUri { get; set; }

        /// <summary>
        /// Тип запроса
        /// </summary>
        public string RequestType { get; set; }

        #region Ссылки на колекции в ЕСИА
        /// <summary>
        /// Пользователи, зарегистрированных в ЕСИА
        /// </summary>
        public string PrnsRef { get; set; }

        /// <summary>
        /// Контактные данные пользователей
        /// </summary>
        public string CttsRef { get; set; }

        /// <summary>
        /// Адреса пользователей
        /// </summary>
        public string AddrsRef { get; set; }

        /// <summary>
        /// Документы пользователя
        /// </summary>
        public string DocsRef { get; set; }

        /// <summary>
        /// Организации пользователей
        /// </summary>
        public string OrgsRef { get; set; }
        #endregion
    }
}