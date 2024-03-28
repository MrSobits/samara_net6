/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map
/// {
///     using Bars.B4.DataAccess;
///     using Entities;
/// 
///     class AccBankStatementMap : BaseImportableEntityMap<AccBankStatement>
///     {
///         public AccBankStatementMap()
///             : base("OVRHL_ACC_BANK_STATEMENT")
///         {
///             Map(x => x.Number, "DOC_NUMBER").Length(50);
///             Map(x => x.DocumentDate, "DOC_DATE");
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

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "Банковская выписка"</summary>
    public class AccBankStatementMap : BaseImportableEntityMap<AccBankStatement>
    {
        
        public AccBankStatementMap() : 
                base("Банковская выписка", "OVRHL_ACC_BANK_STATEMENT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Number, "Номер счета").Column("DOC_NUMBER").Length(50);
            Property(x => x.DocumentDate, "Дата выписки").Column("DOC_DATE");
            Property(x => x.BalanceIncome, "Входящий остаток").Column("BALANCE_INCOME");
            Property(x => x.BalanceOut, "Исходящий остаток").Column("BALANCE_OUT");
            Property(x => x.LastOperationDate, "Дата последней операции по счету").Column("LAST_OPERATION_DATE");
            Reference(x => x.BankAccount, "Счет").Column("ACCOUNT_ID").Fetch();
            Reference(x => x.State, "State").Column("STATE_ID").Fetch();
        }
    }
}
