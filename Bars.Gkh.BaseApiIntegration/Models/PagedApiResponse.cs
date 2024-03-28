namespace Bars.Gkh.BaseApiIntegration.Models
{
    using System;

    /// <summary>
    /// Постраничный ответ с удобным приведением
    /// </summary>
    public class PagedApiResponse : PagedApiResponse<object>
    {
        
    }

    /// <summary>
    /// Постраничный ответ с указанием типа данных
    /// </summary>
    /// <typeparam name="T">Тип данных</typeparam>
    public class PagedApiResponse<T> : BaseApiResponse<T>
    {
        /// <summary>
        /// Guid следующей страницы
        /// </summary>
        public Guid? NextPageGuid { get; set; }
    }
}