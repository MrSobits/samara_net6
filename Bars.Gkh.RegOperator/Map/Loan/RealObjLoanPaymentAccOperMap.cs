/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.Loan
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.Loan;
/// 
///     public class RealObjLoanPaymentAccOperMap : BaseImportableEntityMap<RealObjLoanPaymentAccOper>
///     {
///         public RealObjLoanPaymentAccOperMap()
///             : base("REGOP_LOAN_PAY_ACC")
///         {
///             References(x => x.RealityObjectLoan, "LOAN_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.PayAccOperation, "PAY_ACC_OP_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map.Loan
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.Loan;
    
    
    /// <summary>Маппинг для "Ñâÿçü îïåðàöèè ïî çàéìó äîìà è ñ÷åòó îïëàò äîìà"</summary>
    public class RealObjLoanPaymentAccOperMap : BaseImportableEntityMap<RealObjLoanPaymentAccOper>
    {
        
        public RealObjLoanPaymentAccOperMap() : 
                base("Ñâÿçü îïåðàöèè ïî çàéìó äîìà è ñ÷åòó îïëàò äîìà", "REGOP_LOAN_PAY_ACC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObjectLoan, "Çàéì äîìà").Column("LOAN_ID").NotNull().Fetch();
            Reference(x => x.PayAccOperation, "Îïåðàöèÿ ïî ñ÷åòó îïëàò äîìà").Column("PAY_ACC_OP_ID").NotNull().Fetch();
        }
    }
}
