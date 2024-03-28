/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
///     using Bars.Gkh.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Отзывы заказчиков о подрядчиках"
///     /// </summary>
///     public class BuilderFeedbackMap : BaseGkhEntityMap<BuilderFeedback>
///     {
///         public BuilderFeedbackMap()
///             : base("GKH_BUILDER_FEEDBACK")
///         {
///             Map(x => x.TypeAssessment, "ASSESSMENT").Not.Nullable().CustomType<TypeAssessment>();
///             Map(x => x.TypeAuthor, "AUTHOR").Not.Nullable().CustomType<TypeAuthor>();
///             Map(x => x.Content, "CONTENT").Length(500);
///             Map(x => x.DocumentName, "DOCUMENT_NAME").Length(100);
///             Map(x => x.FeedbackDate, "FEEDBACK_DATE");
///             Map(x => x.OrganizationName, "ORGANIZATION_NAME").Length(50);
///             
///             References(x => x.Builder, "BUILDER_ID").Not.Nullable().Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Отзывы заказчиков о подрядчиках"</summary>
    public class BuilderFeedbackMap : BaseImportableEntityMap<BuilderFeedback>
    {
        
        public BuilderFeedbackMap() : 
                base("Отзывы заказчиков о подрядчиках", "GKH_BUILDER_FEEDBACK")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.TypeAssessment, "Оценка").Column("ASSESSMENT").NotNull();
            Property(x => x.TypeAuthor, "Автор").Column("AUTHOR").NotNull();
            Property(x => x.Content, "Содержание").Column("CONTENT").Length(500);
            Property(x => x.DocumentName, "Название документа").Column("DOCUMENT_NAME").Length(100);
            Property(x => x.FeedbackDate, "Дата отзыва").Column("FEEDBACK_DATE");
            Property(x => x.OrganizationName, "Наименование организации").Column("ORGANIZATION_NAME").Length(50);
            Reference(x => x.Builder, "Подрядчик").Column("BUILDER_ID").NotNull().Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
        }
    }
}
