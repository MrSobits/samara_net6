/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.GkhDi.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     public class DiRealObjPercentMap : SubclassMap<DiRealObjPercent>
///     {
///         public DiRealObjPercentMap()
///         {
///             Table("DI_PERC_REAL_OBJ");
///             KeyColumn("ID");
///             References(x => x.DiRealityObject, "REAL_OBJ_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.DiRealObjPercent"</summary>
    public class DiRealObjPercentMap : JoinedSubClassMap<DiRealObjPercent>
    {
        
        public DiRealObjPercentMap() : 
                base("Bars.GkhDi.Entities.DiRealObjPercent", "DI_PERC_REAL_OBJ")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.DiRealityObject, "DiRealityObject").Column("REAL_OBJ_ID").NotNull().Fetch();
        }
    }
}
