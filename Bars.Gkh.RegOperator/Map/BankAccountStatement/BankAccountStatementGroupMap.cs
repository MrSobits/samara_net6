/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.RegOperator.Entities;
/// 
///     public class BankAccountStatementGroupMap : BaseImportableEntityMap<BankAccountStatementGroup>
///     {
///         public BankAccountStatementGroupMap()
///             : base("REGOP_BANKACC_STMNT_GROUP")
///         {
///             Map(x => x.ImportDate, "IMPORT_DATE");
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.UserLogin, "USER_LOGIN");
///             Map(x => x.Sum, "SUM");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Соответсвует файлу, в котором находится информация по банковским выпискам"</summary>
    public class BankAccountStatementGroupMap : BaseImportableEntityMap<BankAccountStatementGroup>
    {
        
        public BankAccountStatementGroupMap() : 
                base("Соответсвует файлу, в котором находится информация по банковским выпискам", "REGOP_BANKACC_STMNT_GROUP")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ImportDate, "ImportDate").Column("IMPORT_DATE");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.UserLogin, "UserLogin").Column("USER_LOGIN");
            Property(x => x.Sum, "Sum").Column("SUM");
        }
    }
}
