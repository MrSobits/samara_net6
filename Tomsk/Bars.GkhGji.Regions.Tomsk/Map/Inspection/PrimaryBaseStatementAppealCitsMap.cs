/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Regions.Tomsk.Entities.Inspection;
/// 
///     class PrimaryBaseStatementAppealCitsMap : BaseEntityMap<PrimaryBaseStatementAppealCits>
///     {
///         public PrimaryBaseStatementAppealCitsMap()
///             : base("GJI_PRIM_BASESTAT_APPCIT")
///         {
///             this.References(x => x.BaseStatementAppealCits, "BASESTAT_APPCIT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map.Inspection
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities.Inspection;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.Inspection.PrimaryBaseStatementAppealCits"</summary>
    public class PrimaryBaseStatementAppealCitsMap : BaseEntityMap<PrimaryBaseStatementAppealCits>
    {
        
        public PrimaryBaseStatementAppealCitsMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.Inspection.PrimaryBaseStatementAppealCits", "GJI_PRIM_BASESTAT_APPCIT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.BaseStatementAppealCits, "BaseStatementAppealCits").Column("BASESTAT_APPCIT_ID").NotNull().Fetch();
        }
    }
}
