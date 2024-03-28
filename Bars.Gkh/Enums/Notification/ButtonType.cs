namespace Bars.Gkh.Enums.Notification
{
    using System;

    using Bars.B4.Utils;

    /// <summary>
    /// Тип кнопок
    /// </summary>
    [Flags]
    public enum ButtonType
    {
        /// <summary>
        /// Ок
        /// </summary>
        [Display("Ок")]
        Ok = 0x01,

        /// <summary>
        /// Принимаю
        /// </summary>
        [Display("Принимаю")]
        Accept = 0x02,

        /// <summary>
        /// Не принимаю
        /// </summary>
        [Display("Не принимаю")]
        Decline = 0x04,

        /// <summary>
        /// Ознакомлен
        /// </summary>
        [Display("Ознакомлен")]
        Familiarize = 0x08
    }
}