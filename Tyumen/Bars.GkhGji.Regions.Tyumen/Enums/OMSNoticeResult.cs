namespace Bars.GkhGji.Regions.Tyumen.Enums
{
    using Bars.B4.Utils;

    public enum OMSNoticeResult
    {
        [Display("Не задано")]
        NotSet = 0,

        [Display("Приказ об исключении")]
        ExclusionOrder = 10,

        [Display("Протокол собрания о продлении(вето)")]
        Veto = 20,

        [Display("Другая УК")]
        OtherMO = 30,

        [Display("Ошибка")]
        Error = 40
    }
}
