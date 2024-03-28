namespace Bars.Gkh.Gis.Enum
{
    using B4.Utils;

    public enum TypeImportFormat
    {
        /*
         * устаревшие режимы
        [Display("РСО")]
        Rso = 10,

        [Display("СЗ")]
        Sz = 20,
         */

        [Display("ГИС")]
        IncGis = 30,

        [Display("ПГУ")]
        IncPgu = 40,

        [Display("Данные СЗ для отчетов")]
        SzDataForMinStroyReport = 50,

        [Display("Данные ЖКХ для отчетов")]
        GkhDataForMinStroyReport = 60,

        [Display("Выгрузка показаний ПУ из ПГУ РТ")]
        UnloadCounterValuesFromPgmuRt = 70
    }
}
