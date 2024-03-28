/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Chelyabinsk.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class PrescriptionBaseDocumentMap : BaseEntityMap<PrescriptionBaseDocument>
///     {
///         public PrescriptionBaseDocumentMap()
///             : base("GJI_PRESCR_BASE_DOC")
///         {
/// 
///             Map(x => x.DateDoc, "DATE_DOC");
///             Map(x => x.NumDoc, "NUM_DOC", false, 300);
/// 
///             References(x => x.KindBaseDocument, "KIND_BASE_DOC_ID", ReferenceMapConfig.NotNull);
///             References(x => x.Prescription, "PRESCRIPTION_ID", ReferenceMapConfig.NotNull);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.Prescription
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Prescription;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.PrescriptionBaseDocument"</summary>
    public class PrescriptionBaseDocumentMap : BaseEntityMap<PrescriptionBaseDocument>
    {
        
        public PrescriptionBaseDocumentMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.PrescriptionBaseDocument", "GJI_PRESCR_BASE_DOC")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.Prescription, "Prescription").Column("PRESCRIPTION_ID").NotNull();
            this.Reference(x => x.KindBaseDocument, "KindBaseDocument").Column("KIND_BASE_DOC_ID").NotNull();
            this.Property(x => x.DateDoc, "DateDoc").Column("DATE_DOC");
            this.Property(x => x.NumDoc, "NumDoc").Column("NUM_DOC").Length(300);
        }
    }
}
