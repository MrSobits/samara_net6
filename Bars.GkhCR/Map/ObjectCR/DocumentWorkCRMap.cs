/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using Bars.Gkh.Map;;
///     using Bars.GkhCr.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Документ работы объекта КР"
///     /// </summary>
///     public class DocumentWorkCrMap : BaseDocumentMap<DocumentWorkCr>
///     {
///         public DocumentWorkCrMap() : base("CR_OBJ_DOCUMENT_WORK")
///         {
///             Map(x => x.Description, "DESCRIPTION").Length(500);
/// 
///             References(x => x.ObjectCr, "OBJECT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Contragent, "CONTRAGENT_ID").Fetch.Join();
///             References(x => x.TypeWork, "TYPE_WORK_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Документ работы объекта КР"</summary>
    public class DocumentWorkCrMap : BaseImportableEntityMap<DocumentWorkCr>
    {
        
        public DocumentWorkCrMap() : 
                base("Документ работы объекта КР", "CR_OBJ_DOCUMENT_WORK")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentName, "DocumentName").Column("DOCUMENT_NAME").Length(300);
            Property(x => x.DocumentNum, "DocumentNum").Column("DOCUMENT_NUM").Length(50);
            Property(x => x.DateFrom, "DateFrom").Column("DATE_FROM");
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Reference(x => x.File, "File").Column("FILE_ID").Fetch();
            Reference(x => x.ObjectCr, "Объект капитального ремонта").Column("OBJECT_ID").NotNull().Fetch();
            Reference(x => x.Contragent, "Участник").Column("CONTRAGENT_ID").Fetch();
            Reference(x => x.TypeWork, "Вид работы").Column("TYPE_WORK_ID").Fetch();
            Property(x => x.UsedInExport, "Выводить документ на портал").Column("USED_IN_EXPORT");
        }
    }
}
