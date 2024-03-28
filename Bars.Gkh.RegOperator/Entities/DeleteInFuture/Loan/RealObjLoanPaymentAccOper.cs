namespace Bars.Gkh.RegOperator.Entities.Loan
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// ����� �������� �� ����� ���� � ����� ����� ����
    /// </summary>
    public class RealObjLoanPaymentAccOper : BaseImportableEntity
    {
        /// <summary>
        /// ���� ����
        /// </summary>
        public virtual RealityObjectLoan RealityObjectLoan { get; set; }

        /// <summary>
        /// �������� �� ����� ����� ����
        /// </summary>
        public virtual RealityObjectPaymentAccountOperation PayAccOperation { get; set; }
    }
}