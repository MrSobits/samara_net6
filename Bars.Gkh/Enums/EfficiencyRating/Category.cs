namespace Bars.Gkh.Enums.EfficiencyRating
{
    using Bars.B4.Utils;

    /// <summary>
    /// Категория графика
    /// </summary>
    public enum Category
    {
        /// <summary>
        /// Изменение значений показателей УК по годам
        /// </summary>
        [Display("Изменение значений показателей УК по годам")]
        RatingValue = 10,

        /// <summary>
        /// Изменение значений показателей УК по разделам
        /// </summary>
        [Display("Изменение значений показателей УК по разделам")]
        FactorValue = 20
    }
}