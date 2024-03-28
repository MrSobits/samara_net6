/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhDi.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Связь деятельности УО и деятельности УО в доме"
///     /// </summary>
///     public class DisclosureInfoRelationMap : BaseImportableEntityMap<DisclosureInfoRelation>
///     {
///         public DisclosureInfoRelationMap() : base("DI_DISINFO_RELATION")
///         {
///             References(x => x.DisclosureInfo, "DISINFO_ID").Not.Nullable().Fetch.Join();
///             References(x => x.DisclosureInfoRealityObj, "DISINFO_RO_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; 
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.DisclosureInfoRelation"</summary>
    public class DisclosureInfoRelationMap : BaseImportableEntityMap<DisclosureInfoRelation>
    {
        
        public DisclosureInfoRelationMap() : 
                base("Bars.GkhDi.Entities.DisclosureInfoRelation", "DI_DISINFO_RELATION")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.DisclosureInfo, "DisclosureInfo").Column("DISINFO_ID").NotNull().Fetch();
            Reference(x => x.DisclosureInfoRealityObj, "DisclosureInfoRealityObj").Column("DISINFO_RO_ID").NotNull().Fetch();
        }
    }
}
