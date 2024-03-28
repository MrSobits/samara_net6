/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Nso.Map
/// {
///     using Bars.GkhGji.Regions.Nso.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Предписание"
///     /// </summary>
///     public class NsoPrescriptionMap : SubclassMap<NsoPrescription>
///     {
///         public NsoPrescriptionMap()
///         {
///             Table("GJI_NSO_PRESCRIPTION");
///             KeyColumn("ID");
///             Map(x => x.DocumentPlace, "DOCUMENT_PLACE").Length(1000);
/// 			Map(x => x.DocumentTime, "DOCUMENT_TIME");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Nso.Entities.NsoPrescription"</summary>
    public class NsoPrescriptionMap : JoinedSubClassMap<NsoPrescription>
    {
        
        public NsoPrescriptionMap() : 
                base("Bars.GkhGji.Regions.Nso.Entities.NsoPrescription", "GJI_NSO_PRESCRIPTION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DocumentPlace, "DocumentPlace").Column("DOCUMENT_PLACE").Length(1000);
            Property(x => x.DocumentTime, "DocumentTime").Column("DOCUMENT_TIME");
        }
    }
}
