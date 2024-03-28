namespace Bars.GkhGji.Regions.Tatarstan.Map.PreventiveAction
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    public class VisitSheetAnnexMap : BaseEntityMap<VisitSheetAnnex>
    {
        public VisitSheetAnnexMap() : 
            base("Лист визита. Приложение", "GJI_VISIT_SHEET_ANNEX")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.VisitSheet, "Лист визита").Column("VISIT_SHEET_ID");
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.Name, "Наименование").Column("NAME");
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION");
            this.Reference(x => x.File, "Файл").Column("FILE_ID");
        }
    }
}