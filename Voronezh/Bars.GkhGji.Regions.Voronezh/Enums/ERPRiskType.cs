namespace Bars.GkhGji.Regions.Voronezh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    ///Простой тип - коды категорий риска
    /// </summary>
    public enum ERPRiskType
    {
        /// <summary>
        /// Чрезвычайно высокий риск (1 класс)
        /// </summary>
        [Display("Чрезвычайно высокий риск (1 класс)")]
        CLASS_I = 10,

        /// <summary>
        /// Высокий риск (2 класс)
        /// </summary>
        [Display("Высокий риск (2 класс)")]
        CLASS_II = 20,

        /// <summary>
        /// Значительный риск (3 класс)
        /// </summary>
        [Display("Значительный риск (3 класс)")]
        CLASS_III = 30,

        /// <summary>
        /// Средний риск (4 класс)
        /// </summary>
        [Display("Средний риск (4 класс)")]
        CLASS_IV = 40,

        /// <summary>
        /// Умеренный риск (5 класс)
        /// </summary>
        [Display("Умеренный риск (5 класс)")]
        CLASS_V = 50,

        /// <summary>
        /// Низкий риск (6 класс)
        /// </summary>
        [Display("Низкий риск (6 класс)")]
        CLASS_VI = 60
    }
}