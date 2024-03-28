/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг сущности "Правовое основание проверки"
///     /// </summary>
///     public class TypeSurveyInspFoundationGjiMap : BaseEntityMap<TypeSurveyInspFoundationGji>
///     {
///         public TypeSurveyInspFoundationGjiMap() : base("GJI_DICT_LEGFOUND_INSPECT")
///         {
///             Map(x => x.Name, "NAME").Length(600);
///             Map(x => x.Code, "CODE").Length(100);
/// 
///             References(x => x.NormativeDoc, "NORM_DOC_ID").Not.Nullable().Fetch.Join();
///             References(x => x.TypeSurvey, "TYPE_SURVEY_GJI_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Правовое основание проведения проверки"</summary>
    public class TypeSurveyInspFoundationGjiMap : BaseEntityMap<TypeSurveyInspFoundationGji>
    {
        
        public TypeSurveyInspFoundationGjiMap() : 
                base("Правовое основание проведения проверки", "GJI_DICT_LEGFOUND_INSPECT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").Length(600);
            Property(x => x.Code, "Код").Column("CODE").Length(100);
            Reference(x => x.NormativeDoc, "Нормативный документ").Column("NORM_DOC_ID").NotNull().Fetch();
            Reference(x => x.TypeSurvey, "Тип обследования").Column("TYPE_SURVEY_GJI_ID").NotNull().Fetch();
        }
    }
}
