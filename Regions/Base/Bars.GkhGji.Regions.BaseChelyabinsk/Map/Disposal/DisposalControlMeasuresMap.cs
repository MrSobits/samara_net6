/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Chelyabinsk.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Chelyabinsk.Entities;
/// 
///     public class DisposalControlMeasuresMap : BaseEntityMap<DisposalControlMeasures>
///     {
///         public DisposalControlMeasuresMap()
///             : base("GJI_DISP_CON_MEASURE")
///         {
///             References(x => x.Disposal, "DISPOSAL_ID", ReferenceMapConfig.NotNullAndFetch);
/// 
///             References(x => x.ControlActivity, "CONTROL_MEASURES_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.DisposalControlMeasures"</summary>
    public class DisposalControlMeasuresMap : BaseEntityMap<DisposalControlMeasures>
    {
        
        public DisposalControlMeasuresMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.DisposalControlMeasures", "GJI_DISP_CON_MEASURE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Disposal, "Disposal").Column("DISPOSAL_ID").NotNull().Fetch();
            Reference(x => x.ControlActivity, "ControlActivity").Column("CONTROL_MEASURES_ID").NotNull().Fetch();
            Property(x => x.Description, "Description").Column("DESCRIPTION");
            Property(x => x.DateStart, "DateStart").Column("DATE_START");
            Property(x => x.DateEnd, "DateEnd").Column("DATE_END");
        }
    }
}
