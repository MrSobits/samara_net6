namespace Bars.GisIntegration.UI.ViewModel
{
    using Bars.B4;

    /// <summary>
    /// Интерфейс View - модели справочников
    /// </summary>
    public interface IDictionaryViewModel
    {
        /// <summary>
        /// Получить список справочников
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции, 
        /// содержащий список справочников</returns>
        IDataResult List(BaseParams baseParams);

        /// <summary>
        /// Получить список справочников ГИС ЖКХ
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие 
        /// идентификаторы пакетов для получения списка справочников</param>
        /// <returns>Список справочников ГИС ЖКХ</returns>
        IDataResult GisDictionariesList(BaseParams baseParams);

        /// <summary>
        /// Получить список записей справочника
        /// </summary>
        /// <param name="baseParams">Параметры: код справочника</param>
        /// <returns></returns>
        IDataResult ListRecords(BaseParams baseParams);
    }
}
