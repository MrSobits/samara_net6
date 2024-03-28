namespace Bars.GisIntegration.Base.Dictionaries
{
    using System;
    using System.Collections.Generic;

    using Bars.GisIntegration.Base.Dictionaries.Impl;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Package.Impl;

    /// <summary>
    /// Интерфейс менеджера справочников
    /// </summary>
    public interface IDictionaryManager
    {
        /// <summary>
        /// Подписывать запросы к сервису общей НСИ
        /// </summary>
        bool SignCommonNsiRequests { get; }

        /// <summary>
        /// Получить все справочники
        /// </summary>
        /// <returns>Список справочников</returns>
        List<IDictionary> GetAllDictionaries();

        /// <summary>
        /// Получить справочник
        /// </summary>
        /// <param name="dictionaryCode">Код справочника</param>
        /// <returns>Справочник</returns>
        IDictionary GetDictionary(string dictionaryCode);

        /// <summary>
        /// Обновить статусы справочников
        /// </summary>
        /// <param name="packageIds">Идентификаторы пакетов</param>
        void UpdateStates(Guid[] packageIds);

        /// <summary>
        /// Получить пакеты с запросами списка справочников
        /// </summary>
        /// <returns>Список пакетов</returns>
        List<TempPackageInfo> GetDictionaryListPackages();

        /// <summary>
        /// Получить пакеты с запросами записей справочников
        /// </summary>
        /// <param name="dictionaryCode">Код справочника</param>
        /// <returns>Список пакетов</returns>
        List<TempPackageInfo> GetDictionaryRecordsPackages(string dictionaryCode);

        /// <summary>
        /// Получить список справочников ГИС
        /// </summary>
        /// <param name="packageIds">Идентификаторы пакетов</param>
        /// <returns>Список справочников ГИС</returns>
        List<GisDictionary> GetGisDictionariesList(Guid[] packageIds);

        /// <summary>
        /// Выполнить предварительное сопоставление записей и вернуть результат
        /// </summary>
        /// <param name="dictionaryCode">Код справочника</param>
        /// <param name="packageIds">Идентификаторы пакетов</param>
        /// <returns>Результат предварительного сопоставления</returns>
        RecordComparisonResult PerformRecordComparison(string dictionaryCode, Guid[] packageIds);

        /// <summary>
        /// Сохранить список сопоставленных записей справочника
        /// </summary>
        /// <param name="dictionaryCode">Код справочника</param>
        /// <param name="records">Список сопоставленных записей</param>
        void PersistRecordComparison(string dictionaryCode, List<RecordComparisonProxy> records);

        /// <summary>
        /// Сопоставить справочник
        /// </summary>
        /// <param name="dictionaryCode">Код справочника</param>
        /// <param name="gisDictionaryGroup">Группа справочников ГИС</param>
        /// <param name="gisDictionaryRegisryRegistryNumber">Номер справочника ГИС</param>
        void CompareDictionary(string dictionaryCode, DictionaryGroup gisDictionaryGroup, string gisDictionaryRegisryRegistryNumber);
    }

    public class RecordComparisonResult
    {
        public List<GisRecordProxy> GisRecords { get; set; }

        public List<RecordComparisonProxy> Records { get; set; }
    }
}
