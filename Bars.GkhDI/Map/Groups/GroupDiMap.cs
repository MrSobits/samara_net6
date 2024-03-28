/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhDi.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Группа домов"
///     /// </summary>
///     public class GroupDiMap : BaseImportableEntityMap<GroupDi>
///     {
///         public GroupDiMap() : base("DI_DISINFO_GROUP")
///         {
///             Map(x => x.Name, "NAME");
/// 
///             References(x => x.DisclosureInfo, "DISINFO_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.GroupDi"</summary>
    public class GroupDiMap : BaseImportableEntityMap<GroupDi>
    {
        
        public GroupDiMap() : 
                base("Bars.GkhDi.Entities.GroupDi", "DI_DISINFO_GROUP")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Name").Column("NAME");
            Reference(x => x.DisclosureInfo, "DisclosureInfo").Column("DISINFO_ID").NotNull().Fetch();
        }
    }
}
