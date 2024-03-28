/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
///     using Bars.Gkh.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Документы подрядчиков"
///     /// </summary>
///     public class BuilderDocumentMap : BaseGkhEntityMap<BuilderDocument>
///     {
///         public BuilderDocumentMap()
///             : base("GKH_BUILDER_DOCUMENT")
///         {
///             Map(x => x.Description, "DESCRIPTION").Length(500);
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentName, "DOCUMENT_NAME").Length(100);
///             Map(x => x.DocumentNum, "DOCUMENT_NUM").Length(50);
///             Map(x => x.DocumentExist, "DOCUMENT_EXIST").Not.Nullable().CustomType<YesNoNotSet>();
///             Map(x => x.TypeDocument, "DOCUMENT_TYPE").Not.Nullable().CustomType<TypeDocument>();
/// 
///             References(x => x.Builder, "BUILDER_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Contragent, "CONTRAGENT_ID").Fetch.Join();
///             References(x => x.Period, "PERIOD_ID").Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Документы подрядчиков"</summary>
    public class BuilderDocumentMap : BaseImportableEntityMap<BuilderDocument>
    {
        
        public BuilderDocumentMap() : 
                base("Документы подрядчиков", "GKH_BUILDER_DOCUMENT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DocumentName, "Наименование документа").Column("DOCUMENT_NAME").Length(100);
            Property(x => x.DocumentNum, "номер документа").Column("DOCUMENT_NUM").Length(50);
            Property(x => x.DocumentExist, "Наличие документа").Column("DOCUMENT_EXIST").NotNull();
            Property(x => x.TypeDocument, "Тип документа").Column("DOCUMENT_TYPE").NotNull();
            Reference(x => x.Builder, "Подрядчик").Column("BUILDER_ID").NotNull().Fetch();
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").Fetch();
            Reference(x => x.Period, "Период").Column("PERIOD_ID").Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Reference(x => x.BuilderDocumentType, "Тип документа из справочника").Column("DOCUMENT_TYPE_ID").Fetch();
        }
    }
}
