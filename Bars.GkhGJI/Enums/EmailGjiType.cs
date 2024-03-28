namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    public enum EmailGjiType
    {
        [Display("Обращение граждан")]
        Appeal = 0,

        [Display("Служебный документ")]
        Service = 10,

        [Display("Входящий документ")]
        Incoming = 20,

        [Display("Иное")]
        NotSet = 30
    }
}
