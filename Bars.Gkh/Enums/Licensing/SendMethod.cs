namespace Bars.Gkh.Enums.Licensing
{
    using Bars.B4.Utils;

    /// <summary>
    /// Способ отправки
    /// </summary>
    public enum SendMethod
    {
        /// <summary>
        /// Почтой
        /// </summary>
        [Display("Почтой")]
        Post = 10,

        /// <summary>
        /// Наручно
        /// </summary>
        [Display("Наручно")]
        Manually = 20,

        /// <summary>
        /// Через МФЦ
        /// </summary>
        [Display("Через МФЦ")]
        Mfc = 30,

        /// <summary>
        /// Через законного представителя
        /// </summary>
        [Display("Через законного представителя")]
        Representative = 40,

        /// <summary>
        /// Через портал гос. услуг
        /// </summary>
        [Display("Через портал гос. услуг")]
        Portal = 50
    }
}