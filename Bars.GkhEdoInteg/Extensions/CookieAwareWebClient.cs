namespace Bars.GkhEdoInteg.Extensions
{
    using System;
    using System.Net;

    /// <summary>
    /// Реализация <see cref="T:System.Net.WebClient" /> с куки сессией
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class CookieAwareWebClient : WebClient
    {
        /// <summary>
        /// Временя ожидания в миллисекундах
        /// </summary>
        private readonly int requestTimeout;

        /// <summary>
        /// Контейнер куки запроса
        /// </summary>
        public CookieContainer CookieContainer { get; } = new CookieContainer();

        /// <inheritdoc />
        public CookieAwareWebClient(int requestTimeout)
        {
            this.requestTimeout = requestTimeout;
        }

        /// <summary>
        /// Переопределение метода получение веб-запроса
        /// </summary>
        /// <param name="address">Uri-адрес</param>
        /// <returns>Веб-запрос</returns>
        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            if (request is HttpWebRequest webRequest)
            {
                webRequest.Timeout = this.requestTimeout;
                webRequest.CookieContainer = this.CookieContainer;
            }
            
            return request;
        }
    }
}
