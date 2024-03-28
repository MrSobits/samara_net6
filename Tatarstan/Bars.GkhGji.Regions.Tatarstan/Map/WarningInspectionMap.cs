namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using Bars.GkhGji.Entities;

    public class WarningInspectionMap : JoinedSubClassMap<WarningInspection>
    {
        
        public WarningInspectionMap() : base("Основание предостережения ГЖИ", "GJI_WARNING_INSPECTION")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Date, "Дата предостережения").Column("DATE").NotNull();
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.DocumentName, "Наименование документа").Column("DOCUMENT_NAME");
            this.Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER");
            this.Property(x => x.SourceFormType, "Форма поступления").Column("SOURCE_FORM_TYPE").NotNull();
            this.Property(x => x.InspectionBasis, "Основание создания проверки").Column("INSPECTION_BASIS");
            this.Property(x => x.Inn, "ИНН физ./долж. лица").Column("INN");
            this.Reference(x => x.File, "Файл").Column("FILE_ID");
            this.Reference(x => x.AppealCits, "Обращение гражданина").Column("APPEAL_CITS_ID");
            this.Reference(x => x.WarningInspectionControlType, "Вид контроля").Column("CONTROL_TYPE_ID");
        }
    }
}