namespace Bars.Gkh.Gis.Enum.ManOrg
{
    using B4.Utils;

    /// <summary>
    /// Тип работ
    /// </summary>
    public enum ServiceWorkType
    {
        /// <summary>
        /// Не задан
        /// </summary>
        [Display("Не задан")]
        Default = 0,

        /// <summary>
        /// Работа
        /// </summary>
        [Display("Работа")]
        Work = 1,

        /// <summary>
        /// Услуга
        /// </summary>
        [Display("Услуга")]
        Service = 2
    }
}
