/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class BankAccountStatementLinkMap : BaseImportableEntityMap<BankAccountStatementLink>
///     {
///         public BankAccountStatementLinkMap() : base("REGOP_BANK_ACC_STMNT_LINK")
///         {
///             References(x => x.Document, "DOCUMENT_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Statement, "STATEMENT_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Ссылка на банковский документ"</summary>
    public class BankAccountStatementLinkMap : BaseImportableEntityMap<BankAccountStatementLink>
    {
        
        public BankAccountStatementLinkMap() : 
                base("Ссылка на банковский документ", "REGOP_BANK_ACC_STMNT_LINK")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Statement, "Банковская выписка").Column("STATEMENT_ID").NotNull().Fetch();
            Reference(x => x.Document, "Импортированный банковский документ").Column("DOCUMENT_ID").NotNull().Fetch();
        }
    }
}
