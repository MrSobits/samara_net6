namespace Bars.B4.Modules.Analytics.Domain
{
    using System.Linq;
    using Bars.B4.Modules.Analytics.Data;

    /// <summary>
    /// Сервис для работы с поставщиками данных.
    /// </summary>
    public interface IDataProviderService
    {
        /// <summary>
        /// Получение всех поставщиков.
        /// </summary>
        /// <returns>Список поставщиков.</returns>
        IQueryable<IDataProvider> GetAll(bool noHidden = true);

        /// <summary>
        /// Получение поставщика данных по ключу.
        /// </summary>
        /// <param name="key">Уникальный ключ поставщика данных.</param>
        /// <returns>Поставщик данных.</returns>
        IDataProvider Get(string key);
    }
}
