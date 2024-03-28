/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map
/// {
///     using Bars.B4.DataAccess;
///     using Entities;
/// 
///     class AccBankStatementMap : BaseEntityMap<AccBankStatement>
///     {
///         public AccBankStatementMap()
///             : base("OVRHL_ACC_BANK_STATEMENT")
///         {
///             Map(x => x.Number, "DOC_NUMBER").Length(50);
///             Map(x => x.DocumentDate, "DOC_DATE").Not.Nullable();
///             Map(x => x.BalanceIncome, "BALANCE_INCOME");
///             Map(x => x.BalanceOut, "BALANCE_OUT");
///             Map(x => x.LastOperationDate, "LAST_OPERATION_DATE");
/// 
///             References(x => x.BankAccount, "ACCOUNT_ID").Fetch.Join();
///             References(x => x.State, "STATE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.AccBankStatement"</summary>
    public class AccBankStatementMap : BaseEntityMap<AccBankStatement>
    {
        
        public AccBankStatementMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.AccBankStatement", "OVRHL_ACC_BANK_STATEMENT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Number, "Number").Column("DOC_NUMBER").Length(50);
            Property(x => x.DocumentDate, "DocumentDate").Column("DOC_DATE").NotNull();
            Property(x => x.BalanceIncome, "BalanceIncome").Column("BALANCE_INCOME");
            Property(x => x.BalanceOut, "BalanceOut").Column("BALANCE_OUT");
            Property(x => x.LastOperationDate, "LastOperationDate").Column("LAST_OPERATION_DATE");
            Reference(x => x.BankAccount, "BankAccount").Column("ACCOUNT_ID").Fetch();
            Reference(x => x.State, "State").Column("STATE_ID").Fetch();
        }
    }
}
