namespace Bars.GkhDi.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип сущности в расчете процентов
    /// </summary>
    public enum TypeEntityPercCalc
    {
        [Display("Раскрытие информации")] 
        DisclosureInfo = 10,

        [Display("Жилой дом")] 
        RealObj = 20,

        [Display("Услуга")]
        Service = 30
    }
}
