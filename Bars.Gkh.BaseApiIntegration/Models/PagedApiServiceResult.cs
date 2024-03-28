namespace Bars.Gkh.BaseApiIntegration.Models
{
    using System;

    /// <summary>
    /// Постраничный результат API-сервиса
    /// </summary>
    public class PagedApiServiceResult
    {
        /// <summary>
        /// Данные
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Guid следующей страницы
        /// </summary>
        public Guid? NextPageGuid { get; set; }
    }
}