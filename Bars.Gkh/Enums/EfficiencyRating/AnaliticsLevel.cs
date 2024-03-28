namespace Bars.Gkh.Enums.EfficiencyRating
{
    using Bars.B4.Utils;

    /// <summary>
    /// Уровень детализации
    /// </summary>
    public enum AnaliticsLevel
    {
        /// <summary>
        /// По муниципальным районам
        /// </summary>
        [Display("По муниципальным районам")]
        ByMunicipality = 10,

        /// <summary>
        /// По управляющим организациям
        /// </summary>
        [Display("По управляющим организациям")]
        ByManagingOrganization = 20
    }
}