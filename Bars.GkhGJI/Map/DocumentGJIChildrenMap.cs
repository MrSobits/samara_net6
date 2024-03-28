/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Таблица подчиненности документов ГЖИ внутри проверки"
///     /// </summary>
///     public class DocumentGjiChildrenMap : BaseGkhEntityMap<DocumentGjiChildren>
///     {
///         public DocumentGjiChildrenMap() : base("GJI_DOCUMENT_CHILDREN")
///         {
///             References(x => x.Parent, "PARENT_ID").Not.Nullable().LazyLoad();
///             References(x => x.Children, "CHILDREN_ID").Not.Nullable().LazyLoad();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Таблица связи документов (Какой документ из какого был сформирован)"</summary>
    public class DocumentGjiChildrenMap : BaseEntityMap<DocumentGjiChildren>
    {
        
        public DocumentGjiChildrenMap() : 
                base("Таблица связи документов (Какой документ из какого был сформирован)", "GJI_DOCUMENT_CHILDREN")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.Parent, "Родительский документ").Column("PARENT_ID").NotNull();
            Reference(x => x.Children, "Дочерний документ").Column("CHILDREN_ID").NotNull();
        }
    }
}
