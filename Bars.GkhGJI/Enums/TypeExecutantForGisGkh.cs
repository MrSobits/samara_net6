namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип исполнителя документа для ГИС ЖКХ
    /// </summary>
    public enum TypeExecutantForGisGkh
    {
        [Display("Не установлено")]
        NotSet = 10,

        [Display("ФЛ")]
        FL = 20,

        [Display("ЮЛ")]
        UL = 30,

        [Display("ДЛ")]
        DL = 40
    }
}