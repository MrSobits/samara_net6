namespace Bars.Gkh.ConfigSections.Overhaul.Enums
{
    using B4.Utils;

    /// <summary>
    /// Способ корректировки записей актуализации
    /// </summary>
    public enum TypeCorrectionActualizeRecs
    {
        [Display("Не используется")]
        NotUsed = 0,

        [Display("Фиксация за предыдущей записью")]
        FixAfterPreviosRecord = 10
    }
}
