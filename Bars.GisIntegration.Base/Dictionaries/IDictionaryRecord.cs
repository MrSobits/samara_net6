namespace Bars.GisIntegration.Base.Dictionaries
{
    /// <summary>
    /// Запись справочника
    /// </summary>
    public interface IDictionaryRecord
    {
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        long Id { get; }

        /// <summary>
        /// Идентификатор записи справочника внешней системы
        /// </summary>
        long ExternalId { get; }

        /// <summary>
        /// Наименование записи справочника внешней системы
        /// </summary>
        string ExternalName { get; }

        /// <summary>
        /// Код записи справочника в системе ГИС
        /// </summary>
        string GisCode { get; }

        /// <summary>
        /// Guid записи справочника в системе ГИС
        /// </summary>
        string GisGuid { get; }

        /// <summary>
        /// Наименование записи справочника в системе ГИС
        /// </summary>
        string GisName { get; }

        /// <summary>
        /// Запись справочника сопоставлена
        /// записи справочника внешней системы поставлена в соответствие запись справочника ГИС
        /// </summary>
        bool Compared { get; }
    }
}
