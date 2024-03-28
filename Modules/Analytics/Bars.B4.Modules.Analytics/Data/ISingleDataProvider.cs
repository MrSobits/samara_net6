namespace Bars.B4.Modules.Analytics.Data
{
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Filters;

    /// <summary>
    /// Интерфейс поставщика данных, предоставляющий данные в виде объекта
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISingleDataProvider<out T> : IDataProvider where T : class
    {
        /// <summary>
        /// Метод получение данных, реализуется в наследниках.
        /// </summary>
        /// <param name="systemFilter">Системный фильтр.</param>
        /// <param name="dataFilter">Фильтр.</param>
        /// <param name="baseParams">Объект, содержащий параметры запроса.</param>
        /// <returns>Отфильтрованные данные.</returns>
        T GetSingleData(SystemFilter systemFilter, DataFilter dataFilter, BaseParams baseParams);
    }
}
