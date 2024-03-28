
namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Служебная записка предписания ГЖИ"</summary>
    public class PrescriptionOfficialReportMap : BaseEntityMap<PrescriptionOfficialReport>
    {
        
        public PrescriptionOfficialReportMap() : 
                base("Приложения предписания ГЖИ", "GJI_PRESCRIPTION_OFFICIAL_REPORT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.ViolationDate, "Дата устранения нарушений").Column("VIOLATION_DATE");
            Property(x => x.ExtensionViolationDate, "Дата продления нарушений").Column("EXTENSION_DATE");
            Property(x => x.YesNo, "Нарушения устранены").Column("VIOLATION_REMOVED");
            Property(x => x.OfficialReportType, "Тип СЗ").Column("REPORT_TYPE");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            Property(x => x.DocumentNumber, "НОмер документа").Column("NUMBER").Length(300);
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            Reference(x => x.Prescription, "Предписание").Column("PRESCRIPTION_ID").NotNull().Fetch();
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
        }
    }
}
