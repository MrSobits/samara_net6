/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Saha.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Saha.Entities;
/// 
///     public class InspectionGjiViolWordingMap : BaseEntityMap<InspectionGjiViolWording>
///     {
///         public InspectionGjiViolWordingMap()
///             : base("GJI_SAHA_INSP_VIOL_WORD")
///         {
///             Map(x => x.Wording, "WORDING");
/// 
///             References(x => x.InspectionViolation, "INSPECTION_VIOL_ID", ReferenceMapConfig.NotNullAndFetch);
///         } 
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Saha.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Saha.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Saha.Entities.InspectionGjiViolWording"</summary>
    public class InspectionGjiViolWordingMap : BaseEntityMap<InspectionGjiViolWording>
    {
        
        public InspectionGjiViolWordingMap() : 
                base("Bars.GkhGji.Regions.Saha.Entities.InspectionGjiViolWording", "GJI_SAHA_INSP_VIOL_WORD")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.InspectionViolation, "InspectionViolation").Column("INSPECTION_VIOL_ID").NotNull().Fetch();
            Property(x => x.Wording, "Wording").Column("WORDING").Length(250);
        }
    }
}
