/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class CompetitionDocumentMap : BaseImportableEntityMap<CompetitionDocument>
///     {
///         public CompetitionDocumentMap() : base("CR_COMPETITION_DOCUMENT")
///         {
///             References(x => x.Competition, "COMPETITION_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.File, "FILE_ID", ReferenceMapConfig.Fetch);
/// 
///             Map(x => x.DocumentDate, "DOCUMENT_DATE", true);
///             Map(x => x.DocumentName, "DOCUMENT_NAME", false, 300);
///             Map(x => x.DocumentNumber, "DOCUMENT_NUMBER", false, 100);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Документ конкурса"</summary>
    public class CompetitionDocumentMap : BaseImportableEntityMap<CompetitionDocument>
    {
        
        public CompetitionDocumentMap() : 
                base("Документ конкурса", "CR_COMPETITION_DOCUMENT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Competition, "Конкурс").Column("COMPETITION_ID").NotNull().Fetch();
            Property(x => x.DocumentName, "Наименование докуммента").Column("DOCUMENT_NAME").Length(300);
            Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER").Length(100);
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE").NotNull();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
        }
    }
}
