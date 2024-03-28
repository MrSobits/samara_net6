/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Связь документов ГЖИ по типу"
///     /// </summary>
///     public class DocumentGjiReferenceMap : BaseGkhEntityMap<DocumentGjiReference>
///     {
///         public DocumentGjiReferenceMap()
///             : base("GJI_DOCUMENT_REFERENCE")
///         {
///             Map(x => x.TypeReference, "TYPE_REFERENCE").Not.Nullable().CustomType<TypeDocumentReferenceGji>();
/// 
///             References(x => x.Document1, "DOCUMENT1_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Document2, "DOCUMENT2_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Таблица связи документов (Для того чтобы не делать жесткие ссылки между документами ГЖИ) Данная таблица необходима для того чтобы ставить ссылки между доментами по типам В дальнейшем если в одной связке унас будет не 2 документа на несколько то мы будем расширять данную сущность"</summary>
    public class DocumentGjiReferenceMap : BaseEntityMap<DocumentGjiReference>
    {
        
        public DocumentGjiReferenceMap() : 
                base(@"Таблица связи документов (Для того чтобы не делать жесткие ссылки между документами ГЖИ) Данная таблица необходима для того чтобы ставить ссылки между доментами по типам В дальнейшем если в одной связке унас будет не 2 документа на несколько то мы будем расширять данную сущность", "GJI_DOCUMENT_REFERENCE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.TypeReference, "Тип связи документов ГЖИ").Column("TYPE_REFERENCE").NotNull();
            Reference(x => x.Document1, "1й документ").Column("DOCUMENT1_ID").NotNull().Fetch();
            Reference(x => x.Document2, "2й документ").Column("DOCUMENT2_ID").NotNull().Fetch();
        }
    }
}
