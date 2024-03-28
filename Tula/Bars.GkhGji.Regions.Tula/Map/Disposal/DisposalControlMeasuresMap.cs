/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tula.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Tula.Entities;
/// 
///     public class DisposalControlMeasuresMap : BaseEntityMap<DisposalControlMeasures>
///     {
///         public DisposalControlMeasuresMap()
///             : base("GJI_TULA_DISP_CON_MEASURE")
///         {
///             References(x => x.Disposal, "DISPOSAL_ID", ReferenceMapConfig.NotNullAndFetch);
/// 
///             References(x => x.ControlActivity, "CONTROL_MEASURES_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tula.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tula.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tula.Entities.DisposalControlMeasures"</summary>
    public class DisposalControlMeasuresMap : BaseEntityMap<DisposalControlMeasures>
    {
        
        public DisposalControlMeasuresMap() : 
                base("Bars.GkhGji.Regions.Tula.Entities.DisposalControlMeasures", "GJI_TULA_DISP_CON_MEASURE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Disposal, "Disposal").Column("DISPOSAL_ID").NotNull().Fetch();
            Reference(x => x.ControlActivity, "ControlActivity").Column("CONTROL_MEASURES_ID").NotNull().Fetch();
        }
    }
}
