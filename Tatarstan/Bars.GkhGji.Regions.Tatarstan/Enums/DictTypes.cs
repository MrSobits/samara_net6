namespace Bars.GkhGji.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Типы справочника
    /// </summary>
    public enum DictTypes
    {
        /// <summary>
        /// Тип субъекта
        /// </summary>
        [Display("Тип субъекта")]
        SubjectType,

        /// <summary>
        /// Форма КНМ
        /// </summary>
        [Display("Форма КНМ")]
        KnmForm,

        /// <summary>
        /// Тип КНМ
        /// </summary>
        [Display("Тип КНМ")]
        KnmType,

        /// <summary>
        /// Статус КНМ
        /// </summary>
        [Display("Статус проверки")]
        CheckStatus,

        /// <summary>
        /// Единица измерения срока проведения проверки
        /// </summary>
        [Display("Единица измерения")]
        MeasureUnitType
    }
}
