/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhDi.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Дом группа домов"
///     /// </summary>
///     public class RealityObjGroupMap : BaseImportableEntityMap<RealityObjGroup>
///     {
///         public RealityObjGroupMap() : base("DI_DISINFO_RO_GROUP")
///         {
///             References(x => x.RealityObject, "REALITY_OBJ_ID").Not.Nullable().Fetch.Join();
///             References(x => x.GroupDi, "GROUP_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.RealityObjGroup"</summary>
    public class RealityObjGroupMap : BaseImportableEntityMap<RealityObjGroup>
    {
        
        public RealityObjGroupMap() : 
                base("Bars.GkhDi.Entities.RealityObjGroup", "DI_DISINFO_RO_GROUP")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJ_ID").NotNull().Fetch();
            Reference(x => x.GroupDi, "GroupDi").Column("GROUP_ID").NotNull().Fetch();
        }
    }
}
