/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Overhaul.Tat.Entities;
///     using Bars.Gkh.Overhaul.Tat.Enum;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Операции"
///     /// </summary>
///     public class AccountOperationMap : BaseEntityMap<AccountOperation>
///     {
///         public AccountOperationMap()
///             : base("OVRHL_DICT_ACCTOPERATION")
///         {
///             Map(x => x.Name, "NAME").Not.Nullable();
///             Map(x => x.Code, "CODE").Not.Nullable();
///             Map(x => x.Type, "TYPE").Not.Nullable().CustomType<AccountOperationType>();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.AccountOperation"</summary>
    public class AccountOperationMap : BaseEntityMap<AccountOperation>
    {
        
        public AccountOperationMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.AccountOperation", "OVRHL_DICT_ACCTOPERATION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Name").Column("NAME").NotNull();
            Property(x => x.Code, "Code").Column("CODE").NotNull();
            Property(x => x.Type, "Type").Column("TYPE").NotNull();
        }
    }
}
