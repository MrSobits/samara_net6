/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.B4.Utils;
///     using Bars.Gkh.Overhaul.Nso.Entities;
/// 
///     public class AccrualsAccountOperationMap : BaseEntityMap<AccrualsAccountOperation>
///     {
///          public AccrualsAccountOperationMap()
///            : base("OVRHL_ACCOUNT_ACCR_OPERATION")
///            {
///                Map(x => x.AccrualDate, "ACCRUAL_DATE").IsNotNull();
///                Map(x => x.TotalIncome, "TOTAL_DEBIT");
///                Map(x => x.TotalOut, "TOTAL_CREDIT");
///                Map(x => x.OpeningBalance, "OPENING_BALANCE");
///                Map(x => x.ClosingBalance, "CLOSING_BALANCE");
/// 
///                References(x => x.Account, "ACCOUNT_ID");
///            }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.AccrualsAccountOperation"</summary>
    public class AccrualsAccountOperationMap : BaseEntityMap<AccrualsAccountOperation>
    {
        
        public AccrualsAccountOperationMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.AccrualsAccountOperation", "OVRHL_ACCOUNT_ACCR_OPERATION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.AccrualDate, "AccrualDate").Column("ACCRUAL_DATE");
            Property(x => x.TotalIncome, "TotalIncome").Column("TOTAL_DEBIT");
            Property(x => x.TotalOut, "TotalOut").Column("TOTAL_CREDIT");
            Property(x => x.OpeningBalance, "OpeningBalance").Column("OPENING_BALANCE");
            Property(x => x.ClosingBalance, "ClosingBalance").Column("CLOSING_BALANCE");
            Reference(x => x.Account, "Account").Column("ACCOUNT_ID");
        }
    }
}
