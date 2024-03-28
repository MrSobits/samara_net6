namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Учет платежных документов по начислениям и оплатам на КР
    /// </summary>
    public class ConfirmContribution : BaseEntity
    {
        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }
    }
}