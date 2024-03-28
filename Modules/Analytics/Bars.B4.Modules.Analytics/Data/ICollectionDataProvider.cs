namespace Bars.B4.Modules.Analytics.Data
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Filters;

    /// <summary>
    /// Интерфейс поставщика, предоставляющий данные в виде коллекции.
    /// </summary>
    public interface ICollectionDataProvider<out T> : IDataProvider where T : class
    {
        /// <summary>
        /// Метод, возвращающий данные, к которым применены фильтры, переданные в качестве аргументов.
        /// </summary>
        /// <param name="systemFilter">Системный фмльтр.</param>
        /// <param name="dataFilter">Фильтр.</param>
        /// <param name="baseParams">Объект, содержащий параметры запроса.</param>
        /// <returns>Отфильтрованные данные.</returns>
        IQueryable<T> GetCollectionData(SystemFilter systemFilter, DataFilter dataFilter, BaseParams baseParams);

    }
}
