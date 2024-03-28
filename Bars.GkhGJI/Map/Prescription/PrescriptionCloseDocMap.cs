/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
/// 
///     public class PrescriptionCloseDocMap : BaseEntityMap<PrescriptionCloseDoc>
///     {
///         public PrescriptionCloseDocMap()
///             : base("GJI_PRESCR_CLOSE_DOC")
///         {
///             Map(x => x.Date, "DOC_DATE");
///             Map(x => x.DocType, "DOC_TYPE");
///             Map(x => x.Name, "NAME");
///             References(x => x.File, "FILE_ID");
///             References(x => x.Prescription, "PRESCR_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.PrescriptionCloseDoc"</summary>
    public class PrescriptionCloseDocMap : BaseEntityMap<PrescriptionCloseDoc>
    {
        
        public PrescriptionCloseDocMap() : 
                base("Bars.GkhGji.Entities.PrescriptionCloseDoc", "GJI_PRESCR_CLOSE_DOC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Date, "Дата предоставления").Column("DOC_DATE");
            Property(x => x.DocType, "Тип документа").Column("DOC_TYPE");
            Property(x => x.Name, "Наименование").Column("NAME");
            Reference(x => x.File, "Файл").Column("FILE_ID");
            Reference(x => x.Prescription, "Предписание").Column("PRESCR_ID");
        }
    }
}
