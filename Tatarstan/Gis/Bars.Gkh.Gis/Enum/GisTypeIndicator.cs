namespace Bars.Gkh.Gis.Enum
{
    using B4.Utils;

    /// <summary>
    /// Перечисление для индикаторов
    /// пока перечисление, потом из базы
    /// </summary>
    public enum GisTypeIndicator
    {
        [Display("Объем")]
        Volume = 10,

        [Display("Начислено")]
        Charge = 20,

        [Display("Оплачено")]
        Payment = 30,

        [Display("Коэффициент одн")]
        CoefOdn = 40,

        [Display("Распределенный объем")]
        DistibutedVolume = 50,

        [Display("Нераспределенный объем")]
        NotDistibutedVolume = 60,

        [Display("Суммарное начисление")]
        SummaryCharge = 70,

        [Display("Суммарная оплата")]
        SummaryPayment = 80
    }
}
