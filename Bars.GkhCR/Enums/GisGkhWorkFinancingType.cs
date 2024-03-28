namespace Bars.GkhCr.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Источник финансирования для ГИС ЖКХ 
    /// </summary>
    public enum GisGkhWorkFinancingType
    {
        /// <summary>
        /// Фонд
        /// </summary>
        [Display("Фонд содействия")]
        Fund = 10,

        /// <summary>
        /// Региональный бюджет
        /// </summary>
        [Display("Региональный бюджет")]
        RegionBudget = 20,

        /// <summary>
        /// Муниципальный бюджет
        /// </summary>
        [Display("Муниципальный бюджет")]
        MunicipalBudget = 30,

        /// <summary>
        /// Средства собственников
        /// </summary>
        [Display("Средства собственников")]
        Owners = 40
    }
}