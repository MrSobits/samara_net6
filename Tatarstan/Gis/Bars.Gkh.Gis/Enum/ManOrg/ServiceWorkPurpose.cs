namespace Bars.Gkh.Gis.Enum.ManOrg
{
    using B4.Utils;

    /// <summary>
    /// Назначение работ
    /// </summary>
    public enum ServiceWorkPurpose
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        Default = 0,

        /// <summary>
        /// Обслуживание
        /// </summary>
        [Display("Обслуживание")]
        Service = 1,

        /// <summary>
        /// Текущий ремонт
        /// </summary>
        [Display("Текущий ремонт")]
        CurrentRepairs = 2,

        /// <summary>
        /// Аварийные работы
        /// </summary>
        [Display("Аварийные работы")]
        EmergencyWork = 3,

        /// <summary>
        /// По обращению граждан
        /// </summary>
        [Display("По обращению граждан")]
        RequestOfCitizens = 4,

        /// <summary>
        /// По ограничениям поставки
        /// </summary>
        [Display("По ограничениям поставки")]
        SupplyConstraints = 5,

        /// <summary>
        /// Иной вид работы
        /// </summary>
        [Display("Иной вид работы")]
        Other = 6
    }
}
