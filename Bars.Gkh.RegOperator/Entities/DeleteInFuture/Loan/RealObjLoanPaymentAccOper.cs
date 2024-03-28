namespace Bars.Gkh.RegOperator.Entities.Loan
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Связь операции по займу дома и счету оплат дома
    /// </summary>
    public class RealObjLoanPaymentAccOper : BaseImportableEntity
    {
        /// <summary>
        /// Займ дома
        /// </summary>
        public virtual RealityObjectLoan RealityObjectLoan { get; set; }

        /// <summary>
        /// Операция по счету оплат дома
        /// </summary>
        public virtual RealityObjectPaymentAccountOperation PayAccOperation { get; set; }
    }
}