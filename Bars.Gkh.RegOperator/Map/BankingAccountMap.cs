/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class BankingAccountMap : BaseImportableEntityMap<BankingAccount>
///     {
///         public BankingAccountMap() : base("REGOP_BANK_ACCOUNT")
///         {
///             Map(x => x.BankAccountNum, "CACCOUNT_NUM", false, 50);
///             Map(x => x.CurrentBalance, "CCURR_BALANCE", true);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Расчетный счет"</summary>
    public class BankingAccountMap : BaseImportableEntityMap<BankingAccount>
    {
        
        public BankingAccountMap() : 
                base("Расчетный счет", "REGOP_BANK_ACCOUNT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.BankAccountNum, "Номер счета").Column("CACCOUNT_NUM").Length(50);
            Property(x => x.CurrentBalance, "Состояние счета").Column("CCURR_BALANCE").NotNull();
        }
    }
}
