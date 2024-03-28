/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.Loan;
/// 
///     public class RealityObjectLoanPaymentMap : BaseImportableEntityMap<RealityObjectLoanPayment>
///     {
///         public RealityObjectLoanPaymentMap()
///             : base("REGOP_RO_LOAN_PAYMENT")
///         {
///             Map(x => x.OperationGuid, "OP_GUID", false, 40);
///             References(x => x.Loan, "LOAN_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.IncomeOperation, "INCOME_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.OutcomeOperation, "OUTCOME_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map.Loan
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.Loan;
    
    
    /// <summary>Маппинг для "Оплата займа"</summary>
    public class RealityObjectLoanPaymentMap : BaseImportableEntityMap<RealityObjectLoanPayment>
    {
        
        public RealityObjectLoanPaymentMap() : 
                base("Оплата займа", "REGOP_RO_LOAN_PAYMENT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Loan, "Займ").Column("LOAN_ID").NotNull().Fetch();
            Reference(x => x.IncomeOperation, "Операция прихода на счет").Column("INCOME_ID").NotNull().Fetch();
            Reference(x => x.OutcomeOperation, "Операция оплаты со счета").Column("OUTCOME_ID").NotNull().Fetch();
            Property(x => x.OperationGuid, "Гуид, связывающий источник операции с текущей оплатой займа. Пример источника - к" +
                    "акое-либо распределение").Column("OP_GUID").Length(40);
        }
    }
}
