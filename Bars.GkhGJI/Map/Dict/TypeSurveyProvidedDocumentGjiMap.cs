/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Предоставляемый документ Типа обследования"
///     /// </summary>
///     public class TypeSurveyProvidedDocumentGjiMap : BaseEntityMap<TypeSurveyProvidedDocumentGji>
///     {
///         public TypeSurveyProvidedDocumentGjiMap()
///             : base("GJI_DICT_TYPE_SURVEY_DOC")
///         {
///             References(x => x.TypeSurvey, "TYPE_SURVEY_GJI_ID").Not.Nullable().Fetch.Join();
///             References(x => x.ProvidedDocGji, "PROVIDED_DOC_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Предоставляемый документ Типа обследования"</summary>
    public class TypeSurveyProvidedDocumentGjiMap : BaseEntityMap<TypeSurveyProvidedDocumentGji>
    {
        
        public TypeSurveyProvidedDocumentGjiMap() : 
                base("Предоставляемый документ Типа обследования", "GJI_DICT_TYPE_SURVEY_DOC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.TypeSurvey, "Тип обследования").Column("TYPE_SURVEY_GJI_ID").NotNull().Fetch();
            Reference(x => x.ProvidedDocGji, "Предоставляемый документ ГЖИ").Column("PROVIDED_DOC_ID").NotNull().Fetch();
        }
    }
}
