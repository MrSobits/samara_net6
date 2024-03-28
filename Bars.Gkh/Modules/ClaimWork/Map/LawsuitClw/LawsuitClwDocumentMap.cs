/// <mapping-converter-backup>
/// namespace Bars.Gkh.ClaimWork.Map
/// {
///     using Bars.Gkh.Modules.ClaimWork.Entities;
///     using B4.DataAccess.ByCode;
/// 
///     public class LawsuitClwDocumentMap : BaseEntityMap<LawsuitClwDocument>
///     {
///         public LawsuitClwDocumentMap()
///             : base("CLW_LAWSUIT_DOC")
///         {
///             Map(x => x.DocDate, "DOC_DATE");
///             Map(x => x.DocName, "DOC_NAME", false, 500);
///             Map(x => x.DocNumber, "DOC_NUMBER", false, 100);
///             Map(x => x.Description, "DESCRIPTION", false, 2000);
/// 
///             References(x => x.DocumentClw, "DOCUMENT_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.File, "FILE_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Modules.ClaimWork.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    
    
    /// <summary>Маппинг для "Документ Искового зявления"</summary>
    public class LawsuitClwDocumentMap : BaseEntityMap<LawsuitClwDocument>
    {
        
        public LawsuitClwDocumentMap() : 
                base("Документ Искового зявления", "CLW_LAWSUIT_DOC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.DocumentClw, "ссылка на докуент").Column("DOCUMENT_ID").NotNull().Fetch();
            Property(x => x.DocName, "Наименвоание документа").Column("DOC_NAME").Length(500);
            Property(x => x.DocDate, "Дата документа").Column("DOC_DATE");
            Property(x => x.DocNumber, "Номер документа").Column("DOC_NUMBER").Length(100);
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
        }
    }
}
