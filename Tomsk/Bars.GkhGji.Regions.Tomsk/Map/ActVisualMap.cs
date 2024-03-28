/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map
/// {
///     using Entities;
///     using FluentNHibernate.Mapping;
/// 
///     public class ActVisualMap : SubclassMap<ActVisual>
///     {
///         public ActVisualMap()
///         {
///             Table("GJI_ACTVISUAL");
///             KeyColumn("ID");
///             
///             Map(x => x.Conclusion, "CONCLUSION").Length(2000);
///             Map(x => x.InspectionResult, "INSPECTION_RESULT").Length(2000);
///             Map(x => x.Flat, "FLAT").Length(10);
///             Map(x => x.Hour, "HOUR");
///             Map(x => x.Minute, "MINUTE");
/// 
///             References(x => x.RealityObject, "RO_ID").Fetch.Join();
///             References(x => x.FrameVerification, "FRAME_VERIFICATION_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.ActVisual"</summary>
    public class ActVisualMap : JoinedSubClassMap<ActVisual>
    {
        
        public ActVisualMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.ActVisual", "GJI_ACTVISUAL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Conclusion, "Conclusion").Column("CONCLUSION").Length(2000);
            Property(x => x.InspectionResult, "InspectionResult").Column("INSPECTION_RESULT").Length(2000);
            Property(x => x.Flat, "Flat").Column("FLAT").Length(10);
            Property(x => x.Hour, "Hour").Column("HOUR");
            Property(x => x.Minute, "Minute").Column("MINUTE");
            Reference(x => x.RealityObject, "RealityObject").Column("RO_ID").Fetch();
            Reference(x => x.FrameVerification, "FrameVerification").Column("FRAME_VERIFICATION_ID").Fetch();
        }
    }
}
