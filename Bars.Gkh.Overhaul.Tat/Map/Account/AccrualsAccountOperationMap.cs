/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Overhaul.Tat.Entities;
/// 
///     public class AccrualsAccountOperationMap : BaseEntityMap<AccrualsAccountOperation>
///     {
///         public AccrualsAccountOperationMap()
///             : base("OVRHL_ACCOUNT_ACCR_OPERATION")
///         {
///             Map(x => x.AccrualDate, "ACCRUAL_DATE").Not.Nullable();
///             Map(x => x.TotalDebit, "TOTAL_DEBIT");
///             Map(x => x.TotalCredit, "TOTAL_CREDIT");
///             Map(x => x.OpeningBalance, "OPENING_BALANCE");
///             Map(x => x.ClosingBalance, "CLOSING_BALANCE");
/// 
///             References(x => x.Account, "ACCOUNT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.AccrualsAccountOperation"</summary>
    public class AccrualsAccountOperationMap : BaseEntityMap<AccrualsAccountOperation>
    {
        
        public AccrualsAccountOperationMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.AccrualsAccountOperation", "OVRHL_ACCOUNT_ACCR_OPERATION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.AccrualDate, "AccrualDate").Column("ACCRUAL_DATE").NotNull();
            Property(x => x.TotalDebit, "TotalDebit").Column("TOTAL_DEBIT");
            Property(x => x.TotalCredit, "TotalCredit").Column("TOTAL_CREDIT");
            Property(x => x.OpeningBalance, "OpeningBalance").Column("OPENING_BALANCE");
            Property(x => x.ClosingBalance, "ClosingBalance").Column("CLOSING_BALANCE");
            Reference(x => x.Account, "Account").Column("ACCOUNT_ID").NotNull().Fetch();
        }
    }
}
