namespace Bars.Gkh.Reforma.Enums
{
    using Bars.B4.Utils;

    public enum RequestStatus {
        /// <summary>
        /// Заявка подтверждена
        /// </summary>
        [Display("Заявка подтверждена")]
        approved = 1,

        /// <summary>
        /// Заявка отклонена
        /// </summary>
        [Display("Заявка отклонена")]
        declined = 2,

        /// <summary>
        /// В ожидании рассмотрения
        /// </summary>
        [Display("В ожидании рассмотрения")]
        pending = 3
    }
}