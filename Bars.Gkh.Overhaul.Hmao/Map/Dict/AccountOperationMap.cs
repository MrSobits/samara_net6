/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map
/// {
///     using Bars.B4.DataAccess;
///     using Enum;
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Операции"
///     /// </summary>
///     public class AccountOperationMap : BaseImportableEntityMap<AccountOperation>
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

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "Операция по счету"</summary>
    public class AccountOperationMap : BaseImportableEntityMap<AccountOperation>
    {
        
        public AccountOperationMap() : 
                base("Операция по счету", "OVRHL_DICT_ACCTOPERATION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").NotNull();
            Property(x => x.Code, "Код").Column("CODE").NotNull();
            Property(x => x.Type, "Тип").Column("TYPE").NotNull();
        }
    }
}
