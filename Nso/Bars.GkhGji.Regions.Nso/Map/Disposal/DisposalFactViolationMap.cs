/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Nso.Map.Disposal
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Nso.Entities.Disposal;
/// 
///     public class DisposalFactViolationMap : BaseEntityMap<DisposalFactViolation>
///     {
///         public DisposalFactViolationMap()
///             : base("GJI_NSO_DISP_FACT_VIOL")
///         {
/// 
///             References(x => x.Disposal, "DISPOSAL_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.TypeFactViolation, "FACT_VIOL_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Nso.Map.Disposal
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Nso.Entities.Disposal;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Nso.Entities.Disposal.DisposalFactViolation"</summary>
    public class DisposalFactViolationMap : BaseEntityMap<DisposalFactViolation>
    {
        
        public DisposalFactViolationMap() : 
                base("Bars.GkhGji.Regions.Nso.Entities.Disposal.DisposalFactViolation", "GJI_NSO_DISP_FACT_VIOL")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Disposal, "Disposal").Column("DISPOSAL_ID").NotNull().Fetch();
            Reference(x => x.TypeFactViolation, "TypeFactViolation").Column("FACT_VIOL_ID").NotNull().Fetch();
        }
    }
}
