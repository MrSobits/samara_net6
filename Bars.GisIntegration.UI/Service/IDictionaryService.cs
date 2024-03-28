namespace Bars.GisIntegration.UI.Service
{
    using Bars.B4;

    /// <summary>
    /// Интерфейс сервиса справочников
    /// </summary>
    public interface IDictionaryService
    {
        /// <summary>
        /// Получить параметры выполнения подписывания и отправки данных
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Параметры выполнения подписывания и отправки данных</returns>
        IDataResult GetUpdateStatesParams(BaseParams baseParams);

        /// <summary>
        /// Получить параметры выполнения сопоставления справочника
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие
        /// код справочника</param>
        /// <returns>Параметры сопоставления справочника</returns>
        IDataResult GetCompareDictionaryParams(BaseParams baseParams);
        
        /// <summary>
        /// Получить параметры выполнения сопоставления записей справочника
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие
        /// код справочника</param>
        /// <returns>Параметры сопоставления справочника</returns>
        IDataResult GetCompareDictionaryRecordsParams(BaseParams baseParams);

        /// <summary>
        /// Получить пакеты с запросами списка справочников
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Список пакетов</returns>
        IDataResult GetDictionaryListPackages(BaseParams baseParams);
        
        /// <summary>
        /// Получить пакеты с запросами записей справочника
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Список пакетов</returns>
        IDataResult GetDictionaryRecordsPackages(BaseParams baseParams);

        /// <summary>
        /// Обновить статусы справочников
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие идентификаторы пакетов</param>
        /// <returns>Результат выполнения операции</returns>
        IDataResult UpdateStates(BaseParams baseParams);

        /// <summary>
        /// Сопоставить справочник
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие
        /// код справочника
        /// реестровый номер справочника ГИС
        /// группу справочника ГИС</param>
        /// <returns>Результат выполнения операции</returns>
        IDataResult CompareDictionary(BaseParams baseParams);

        /// <summary>
        /// Получить результаты предварительного сопоставления записей справочника
        /// </summary>
        /// <param name="baseParams">Параметры: код справочника и идентификаторы пакетов</param>
        /// <returns>Результаты предварительного сопоставления записей</returns>
        IDataResult GetRecordComparisonResult(BaseParams baseParams);

        /// <summary>
        /// Сохранить результат сопоставления записей справочника
        /// </summary>
        /// <param name="baseParams">Параметры: список сопоставленных записей</param>
        /// <returns></returns>
        IDataResult PersistRecordComparison(BaseParams baseParams);
    }
}
