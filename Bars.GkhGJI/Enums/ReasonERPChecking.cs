namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Основание для включения проверки в ЕРП
    /// </summary>
    public enum ReasonErpChecking
    {

        [Display("Не задано")]
        NotSet = 0,

        [Display("Заявление КО")]
        StatementKO = 10,

        [Display("Уведомление")]
        Notification = 20,

        [Display("Без уведомления")]
        WithoutNotice = 30,

        [Display("Требование прокуратуры")]
        TheProsecutorsRequest = 40
    }
}
