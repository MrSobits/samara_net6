namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид КНД
    /// </summary>
    public enum TypeDecisionAnswer
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Жилищный надзор
        /// </summary>
        [Display("По жалобе на постановление")]
        Resolution = 10,

        /// <summary>
        /// Лицензионный контроль
        /// </summary>
        [Display("По жалобе на определение об отказе")]
        Definition = 20
    }
}