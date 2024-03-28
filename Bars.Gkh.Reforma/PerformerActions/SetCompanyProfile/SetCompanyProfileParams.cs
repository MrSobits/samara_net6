namespace Bars.Gkh.Reforma.PerformerActions.SetCompanyProfile
{
    public class SetCompanyProfileParams
    {
        /// <summary>
        ///     УО
        /// </summary>
        public long ManagingOrganizationId { get; set; }

        /// <summary>
        ///     Период раскрытия (ЖКХ)
        /// </summary>
        public long PeriodId { get; set; }

        /// <summary>
        ///     Идентификатор периода в Реформе
        /// </summary>
        public int PeriodExternalId { get; set; }
    }
}