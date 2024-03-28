namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус файла 
    /// </summary>
    
    public enum MessageCheck
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Отправлено
        /// </summary>
        [Display("Отправлено")]
        Sent = 10,

        /// <summary>
        /// Получено 
        /// </summary>
        [Display("Получено")]
        Recd = 20,

    }
}
