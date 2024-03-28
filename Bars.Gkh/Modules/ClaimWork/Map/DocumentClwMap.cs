/// <mapping-converter-backup>
/// namespace Bars.Gkh.Modules.ClaimWork.Map
/// {
///     using Entities;
///     using B4.DataAccess.ByCode;
/// 
///     public class DocumentClwMap : BaseEntityMap<DocumentClw>
///     {
///         public DocumentClwMap()
///             : base("CLW_DOCUMENT")
///         {
///             Map(x => x.DocumentType, "TYPE_DOCUMENT", true, (object)10);
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentNumber, "DOCUMENT_NUMBER");
///             Map(x => x.DocumentNum, "DOCUMENT_NUM");
/// 
///             References(x => x.ClaimWork, "CLAIMWORK_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.State, "STATE_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Modules.ClaimWork.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    
    
    /// <summary>Маппинг для "Базовый документ ПиР"</summary>
    public class DocumentClwMap : BaseEntityMap<DocumentClw>
    {
        
        public DocumentClwMap() : 
                base("Базовый документ ПиР", "CLW_DOCUMENT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ClaimWork, "Основание ПИР").Column("CLAIMWORK_ID").NotNull().Fetch();
            Property(x => x.DocumentType, "Тип документа ПиР").Column("TYPE_DOCUMENT").DefaultValue(ClaimWorkDocumentType.Notification).NotNull();
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER").Length(250);
            Property(x => x.DocumentNum, "Номер документа (Целая часть)").Column("DOCUMENT_NUM");
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}
