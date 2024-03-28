namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    ///Простой тип - коды категорий риска
    /// </summary>
    public enum ProfVisitResult
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Информирование проверяемого лица
        /// </summary>
        [Display("Информирование проверяемого лица")]
        Inform = 10,

        /// <summary>
        /// Угроза вреда(ущерба)
        /// </summary>
        [Display("Угроза вреда(ущерба)")]
        HasViolations = 20,

        /// <summary>
        /// Нарушений не выявлено
        /// </summary>
        [Display("Нарушений не выявлено")]
        HasNoViolations = 30,
    }
}