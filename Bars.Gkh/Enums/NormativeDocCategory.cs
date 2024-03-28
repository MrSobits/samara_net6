namespace Bars.Gkh.Enums
{
    using B4.Utils;

    /// <summary>
    ///     Категория нормативно правового документа
    /// </summary>
    public enum NormativeDocCategory
    {
        /// <summary>
        ///     Жилищный надзор
        /// </summary>
        [Display("Жилищный надзор")]
        HousingSupervision = 10,

        /// <summary>
        ///     Жилищный фонд
        /// </summary>
        [Display("Жилищный фонд")]
        HousingFund = 20,

        /// <summary>
        ///     Капитальный ремонт
        /// </summary>
        [Display("Капитальный ремонт")]
        Overhaul = 30
    }
}