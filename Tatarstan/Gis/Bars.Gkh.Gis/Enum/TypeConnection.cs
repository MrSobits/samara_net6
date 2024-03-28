namespace Bars.Gkh.Gis.Enum
{
    using B4.Utils;

    public enum ConnectionType
    {
        [Display("Инкрементальные данные ПО ЕРЦ Казани")]
        GisConnStringIncKzn,

        [Display("Инкрементальные данные ПО ЕРЦ РТ")]
        GisConnStringIncRt,

        [Display("Инкрементальные данные ПГМУ РТ ")]
        GisConnStringPgu,

        [Display("Инкрементальные данные для отчетов МСА РТ")]
        GisConnStringReports,

        [Display("Инкрементальные данные в формате 3.0")]
        GisConnStringInc
    }
}