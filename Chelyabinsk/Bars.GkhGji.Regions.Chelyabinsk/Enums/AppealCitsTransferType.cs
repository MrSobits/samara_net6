namespace Bars.GkhGji.Regions.Chelyabinsk.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип операции трансфера обращений
    /// </summary>
    public enum AppealCitsTransferType
    {
        /// <summary>
        /// Экспорт сведений о приеме в работу обращения
        /// </summary>
        [Display("Экспорт сведений о приеме в работу обращения")]
        ExportInfoAcceptWork,

        /// <summary>
        /// Экспорт сведений о завершении работы по обращению
        /// </summary>
        [Display("Экспорт сведений о завершении работы по обращению")]
        ExportInfoCompletionOfWork,

        /// <summary>
        /// Экспорт сведений об отмене обращения
        /// </summary>
        [Display("Экспорт сведений об отмене обращения")]
        ExportInfoCitizensAppealCancel,
    }
}
