namespace Bars.Gkh.Reforma.PerformerActions.GetCompanyProfile
{
    public class GetCompanyProfileParams
    {
        /// <summary>
        ///     ИНН УО
        /// </summary>
        public string Inn { get; set; }

        /// <summary>
        ///     Идентификатор периода в Реформе
        /// </summary>
        public int PeriodExternalId { get; set; }
    }
}