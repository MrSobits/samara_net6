namespace Bars.Gkh.BaseApiIntegration.Models
{
    /// <summary>
    /// Базовый ответ с удобным приведением
    /// </summary>
    public class BaseApiResponse : BaseApiResponse<object>
    {
    }

    /// <summary>
    /// Базовый ответ с указанием типа данных
    /// </summary>
    /// <typeparam name="T">Тип данных</typeparam>
    public class BaseApiResponse<T>
    {
        /// <summary>
        /// Данные
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Ошибка
        /// </summary>
        public CustomError Error { get; set; }
    }
}