namespace Bars.GkhGji.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Основание КНМ без взаимодействия с контролируемыми лицами
    /// </summary>
    public enum TypeBaseAction
    {
        /// <summary>
        /// Ежегодный план
        /// </summary>
        [Display("Ежегодный план")]
        Plan = 1,

        /// <summary>
        /// Задание
        /// </summary>
        [Display("Задание")]
        Task,

        /// <summary>
        /// Обращение гражданина
        /// </summary>
        [Display("Обращение гражданина")]
        Appeal
    }
}
