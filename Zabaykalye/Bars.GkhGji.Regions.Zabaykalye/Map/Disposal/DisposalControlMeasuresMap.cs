/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Zabaykalye.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Zabaykalye.Entities;
/// 
///     public class DisposalControlMeasuresMap : BaseEntityMap<DisposalControlMeasures>
///     {
///         public DisposalControlMeasuresMap()
///             : base("GJI_ZBKL_DISP_CON_MEASURE")
///         {
///             References(x => x.Disposal, "DISPOSAL_ID", ReferenceMapConfig.NotNullAndFetch);
/// 
///             References(x => x.ControlActivity, "CONTROL_MEASURES_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Zabaykalye.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Zabaykalye.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Zabaykalye.Entities.DisposalControlMeasures"</summary>
    public class DisposalControlMeasuresMap : BaseEntityMap<DisposalControlMeasures>
    {
        
        public DisposalControlMeasuresMap() : 
                base("Bars.GkhGji.Regions.Zabaykalye.Entities.DisposalControlMeasures", "GJI_ZBKL_DISP_CON_MEASURE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Disposal, "Disposal").Column("DISPOSAL_ID").NotNull().Fetch();
            Reference(x => x.ControlActivity, "ControlActivity").Column("CONTROL_MEASURES_ID").NotNull().Fetch();
        }
    }
}
