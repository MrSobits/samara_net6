namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    public class ViewFormatDataExportInspectionMap : BaseEntityMap<ViewFormatDataExportInspection>
    {
        
        public ViewFormatDataExportInspectionMap() : 
                base("Bars.GkhGji.Entities.ViewActCheck", "view_format_data_export_inspection")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.IsPlanned, "Проверка плановая").Column("IS_PLANNED");
            this.Property(x => x.TypeBase, "Тип основания проверки").Column("TYPE_BASE");
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER");
            this.Property(x => x.CheckDate, "Дата проверки").Column("CHECK_DATE");
            this.Property(x => x.ContragentName, "Наименование субъекта проверки").Column("CONTRAGENT_NAME");
            this.Property(x => x.MunicipalityName, "Наименование муниципального образования").Column("MUNICIPALITY_NAME");

            this.Reference(x => x.Inspection, "Проверка").Column("ID");
            this.Reference(x => x.Disposal, "Распоряжение").Column("DISPOSAL_ID");
            this.Reference(x => x.ActCheck, "Акт проверки").Column("ACTCHECK_ID");
            this.Reference(x => x.Contragent, "Субъект проверки").Column("CONTRAGENT_ID");
        }
    }
}
