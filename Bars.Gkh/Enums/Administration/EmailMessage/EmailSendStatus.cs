using Bars.B4.Utils;

namespace Bars.Gkh.Enums.Administration.EmailMessage
{
    /// <summary>
    /// Cтатус выполнения
    /// </summary>
    public enum EmailSendStatus
    {
        /// <summary>
        /// Успешно
        /// </summary>
        [Display("Успешно")]
        Success = 10,

        /// <summary>
        /// Ошибка
        /// </summary>
        [Display("Ошибка")]
        Error = 20
    }
}