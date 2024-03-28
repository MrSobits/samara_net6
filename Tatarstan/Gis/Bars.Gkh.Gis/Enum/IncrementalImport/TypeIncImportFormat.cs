namespace Bars.Gkh.Gis.Enum.IncrementalImport
{
    using Bars.B4.Utils;

    /// <summary>
    /// Перечисление форматов инкрементальной загрузки из биллинга
    /// т.к. в таблице биллинга нет кода результата, а есть только его название
    /// то решил завести отдельный enum
    /// </summary>
    public enum TypeIncImportFormat
    {
        [Display("ПГУ")]
        PGU = 40,
        
        [Display("Данные ЖКХ для отчетов")]
        GkhDataForMinStroyReport = 60,

        [Display("Данные СЗ для отчетов")]
        SzDataForMinStroyReport = 50
    }
}
