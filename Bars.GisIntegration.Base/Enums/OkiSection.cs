namespace Bars.GisIntegration.Base.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Секции ОКИ
    /// </summary>
    public enum OkiSection
    {
        [Display("Характеристика передачи (транспортировки) коммунальных ресурсов")]
        TransferSource = 1,
        [Display("Характеристика сетевого объекта")]
        NetOkiObject = 2,
        [Display("Производство коммунального ресурса")]
        CommunalSourceProduction = 3
    }
}
