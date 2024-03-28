namespace Bars.Gkh.Overhaul.Hmao.Enum
{
    using B4.Utils;

    /// <summary>
    /// тип актуализации
    /// </summary>
    public enum VersionActualizeType
    {
        [Display("Добавление новых записей")]
        ActualizeNewRecords = 10,

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
        ActualizeFromShortCr = 70,

        [Display("Актуализация изменения года")]
        ActualizeChangeYear = 80,

        [Display("Импорт сведений о сроках проведения КР")]
        ImportPublishYearImport = 90,

        [Display("Публикация программы")]
        CreateDpkrForPublish = 100,

        [Display("Разделение работ")]
        SplitWork = 110
    }
}