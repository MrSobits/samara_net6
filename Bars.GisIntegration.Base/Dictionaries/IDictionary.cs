namespace Bars.GisIntegration.Base.Dictionaries
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Справочник внешней системы
    /// </summary>
    public interface IDictionary
    {
        /// <summary>
        /// Идентификатор справочника
        /// </summary>
        long Id { get; }

        /// <summary>
        /// Наименование
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Код
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Реестровый номер справочника в ГИС
        /// </summary>
        string GisRegistryNumber { get; }

        /// <summary>
        /// Состояние справочника
        /// </summary>
        DictionaryState State { get; }

        /// <summary>
        /// Дата последнего сопоставления записей справочника
        /// </summary>
        DateTime? LastRecordsCompareDate { get; }

        /// <summary>
        /// Группа справочника
        /// </summary>
        DictionaryGroup? Group { get; }

        /// <summary>
        /// Получить записи справочника
        /// </summary>
        /// <returns></returns>
        List<IDictionaryRecord> GetDictionaryRecords();

        /// <summary>
        /// Получить запись справочника
        /// </summary>
        /// <param name="externalId">Идентификатор записи справочника внешней системы</param>
        /// <returns>Запись справочника</returns>
        IDictionaryRecord GetDictionaryRecord(long externalId);

        /// <summary>
        /// Обновить статус справочника
        /// </summary>
        /// <param name="newState">Новый статус</param>
        void UpdateState(DictionaryState newState);

        /// <summary>
        /// Сопоставить справочник
        /// Текущему справочнику поставить в соответствие справочник ГИС
        /// </summary>
        /// <param name="dictionaryGroup">Группа справочника</param>
        /// <param name="gisRegistryNumber">Реестровый номер справочника в ГИС</param>
        void CompareDictionary(DictionaryGroup dictionaryGroup, string gisRegistryNumber);

        /// <summary>
        /// Выполнить предварительное сопоставление с записями ГИС ЖКХ
        /// </summary>
        /// <param name="gisRecords">Записи справочника ГИС ЖКХ</param>
        /// <returns>Результат предварительного сопоставления</returns>
        List<RecordComparisonProxy> PerformRecordComparison(List<GisRecordProxy> gisRecords);

        /// <summary>
        /// Сохранить результаты сопоставления с записями ГИС ЖКХ
        /// </summary>
        /// <param name="records">Список сопоставленных записей</param>
        void PersistRecordComparison(List<RecordComparisonProxy> records);
    }

    public class GisRecordProxy
    {
        public string Code { get; set; }

        public string Guid { get; set; }

        public string Name { get; set; }
    }

    public class ExternalEntityProxy
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public ExternalEntityProxy()
        {
            
        }

        public ExternalEntityProxy(long id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public ExternalEntityProxy(Enum enumValue)
        {
            this.Id = enumValue.GetHashCode();
            this.Name = enumValue.GetDisplayName();
        }
    }

    public class ExternalEntityProxyList : List<ExternalEntityProxy>
    {
        public static ExternalEntityProxyList FromEnum<T>() where T : struct, IConvertible
        {
            var result = new ExternalEntityProxyList();
            var values = Enum.GetValues(typeof(T));
            foreach (Enum value in values)
            {
                result.Add(new ExternalEntityProxy(value));
            }

            return result;
        }
    }

    public class RecordComparisonProxy
    {
        public ExternalEntityProxy ExternalEntity { get; set; }

        public GisRecordProxy GisRecord { get; set; }
    }
}
