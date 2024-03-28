namespace Bars.Gkh.Overhaul.Tat.Enum
{
    using Bars.B4.Utils;

    /// <summary>
    /// тип актуализации
    /// </summary>

    public enum VersionActualizeType
    {
        [Display("Добавление новых записей")]
        ActualizeNewRecords = 10,

        [Display("Добавление новых записей по ООИ \"Лифты\"")]
        ActualizeLiftNewRecords = 11,

        [Display("Актуализация стоимости")]
        ActualizeSum = 20,

        [Display("Актуализация года")]
        ActualizeYear = 30,

        [Display("Удаление лишних записей")]
        ActualizeDeletedEntries = 40,

        [Display("Группировка ООИ")]
        ActualizeGroup = 50,

        [Display("Расчет очередности")]
        ActualizeOrder = 60,

        [Display("Актуализация из КПКР")]
        ActualizeFromShortCr = 70

    }
}