namespace Bars.GisIntegration.Base.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Состояние справочника
    /// </summary>
    public enum DictionaryState
    {
        /// <summary>
        /// Не сопоставлен
        /// </summary>
        [Display("Не сопоставлен")]
        NotCompared = 10,

        /// <summary>
        /// Справочник ГИС был удален - требуется сопоставление справочника 
        /// </summary>
        [Display("Справочник ГИС был удален")]
        GisDictionaryDeleted = 15,

        /// <summary>
        /// Справочник ГИС был изменен - требуется сопоставление записей
        /// </summary>
        [Display("Справочник ГИС был изменен")]
        GisDictionaryChanged = 20,

        /// <summary>
        /// Записи не сопоставлены
        /// </summary>
        [Display("Записи не сопоставлены")]
        RecordsNotCompared = 30,

        /// <summary>
        /// Записи не сопоставлены
        /// </summary>
        [Display("Записи частично сопоставлены")]
        RecordsPartiallyCompared = 40,

        /// <summary>
        /// Сопоставлен
        /// </summary>
        [Display("Сопоставлен")]
        Compared = 50
    }
}
