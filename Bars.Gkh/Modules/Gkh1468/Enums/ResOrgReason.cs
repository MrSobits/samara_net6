namespace Bars.Gkh.Modules.Gkh1468.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Основание договора с жилым домом
    /// </summary>
    public enum ResOrgReason
    {
        /// <summary>
        /// Договор управления
        /// </summary>
        [Display("Договор управления")]
        ManagementContract = 10,

        /// <summary>
        /// Устав
        /// </summary>
        [Display("Устав")]
        Charter = 20,

        /// <summary>
        /// Решение собрания собственников
        /// </summary>
        [Display("Решение собрания собственников")]
        OwnerMeetingDecision = 30,

        /// <summary>
        /// Открытый конкурс
        /// </summary>
        [Display("Открытый конкурс")]
        OpenCompetition = 40
    }
}
