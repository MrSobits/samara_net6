namespace Bars.Gkh.RegOperator.Map.RealityAccount
{
    using B4.DataAccess.ByCode;
    using Entities;

    public class RoPayAccOperationLoanMap : BaseEntityMap<RoPayAccOperationLoan>
    {
        public RoPayAccOperationLoanMap()
            : base("REGOP_RO_PACC_OP_LOAN")
        {
            References(x => x.Loan, "LOAN_ID", ReferenceMapConfig.NotNullAndFetch);
            References(x => x.Operation, "OPERATION_ID", ReferenceMapConfig.NotNullAndFetch);
        }
    }
}