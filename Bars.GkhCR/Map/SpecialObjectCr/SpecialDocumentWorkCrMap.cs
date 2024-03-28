namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Документ работы объекта КР"</summary>
    public class SpecialDocumentWorkCrMap : BaseImportableEntityMap<SpecialDocumentWorkCr>
    {
        public SpecialDocumentWorkCrMap() : 
                base("Документ работы объекта КР", "CR_SPECIAL_OBJ_DOCUMENT_WORK")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.DocumentName, "DocumentName").Column("DOCUMENT_NAME").Length(300);
            this.Property(x => x.DocumentNum, "DocumentNum").Column("DOCUMENT_NUM").Length(50);
            this.Property(x => x.DateFrom, "DateFrom").Column("DATE_FROM");
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            this.Property(x => x.UsedInExport, "Выводить документ на портал").Column("USED_IN_EXPORT");

            this.Reference(x => x.File, "File").Column("FILE_ID").Fetch();
            this.Reference(x => x.ObjectCr, "Объект капитального ремонта").Column("OBJECT_ID").NotNull().Fetch();
            this.Reference(x => x.Contragent, "Участник").Column("CONTRAGENT_ID").Fetch();
            this.Reference(x => x.TypeWork, "Вид работы").Column("TYPE_WORK_ID").Fetch();
        }
    }
}
