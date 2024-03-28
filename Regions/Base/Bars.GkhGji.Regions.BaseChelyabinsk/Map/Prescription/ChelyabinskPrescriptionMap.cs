/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Chelyabinsk.Map
/// {
///     using Bars.GkhGji.Regions.Chelyabinsk.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Предписание"
///     /// </summary>
///     public class ChelyabinskPrescriptionMap : SubclassMap<ChelyabinskPrescription>
///     {
///         public ChelyabinskPrescriptionMap()
///         {
///             Table("GJI_NSO_PRESCRIPTION");
///             KeyColumn("ID");
///             Map(x => x.DocumentPlace, "DOCUMENT_PLACE").Length(1000);
/// 			Map(x => x.DocumentTime, "DOCUMENT_TIME");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.Prescription
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Prescription;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.ChelyabinskPrescription"</summary>
    public class ChelyabinskPrescriptionMap : JoinedSubClassMap<ChelyabinskPrescription>
    {
        
        public ChelyabinskPrescriptionMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.ChelyabinskPrescription", "GJI_NSO_PRESCRIPTION")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.DocumentPlace, "DocumentPlace").Column("DOCUMENT_PLACE").Length(1000);
            this.Property(x => x.DocumentTime, "DocumentTime").Column("DOCUMENT_TIME");
        }
    }
}
