/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Nnovgorod.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Regions.Nnovgorod.Entities;
/// 
///     public class DisposalControlMeasuresMap : BaseEntityMap<DisposalControlMeasures>
///     {
///         public DisposalControlMeasuresMap()
///             : base("GJI_NNOV_DISP_CON_MEASURE")
///         {
///             References(x => x.Disposal, "DISPOSAL_ID").Not.Nullable().Fetch.Join();
/// 
///             Map(x => x.ControlMeasuresName, "CONTROL_MEASURES_NAME").Length(2000);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Nnovgorod.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Nnovgorod.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Nnovgorod.Entities.DisposalControlMeasures"</summary>
    public class DisposalControlMeasuresMap : BaseEntityMap<DisposalControlMeasures>
    {
        
        public DisposalControlMeasuresMap() : 
                base("Bars.GkhGji.Regions.Nnovgorod.Entities.DisposalControlMeasures", "GJI_NNOV_DISP_CON_MEASURE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ControlMeasuresName, "ControlMeasuresName").Column("CONTROL_MEASURES_NAME").Length(2000);
            Reference(x => x.Disposal, "Disposal").Column("DISPOSAL_ID").NotNull().Fetch();
        }
    }
}
