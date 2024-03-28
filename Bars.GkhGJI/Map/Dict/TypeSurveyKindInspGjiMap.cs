/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг сущности "Вид обследования"
///     /// </summary>
///     public class TypeSurveyKindInspGjiMap : BaseEntityMap<TypeSurveyKindInspGji>
///     {
///         public TypeSurveyKindInspGjiMap() : base("GJI_DICT_KIND_INSPECTION")
///         {
///             Map(x => x.Code, "CODE").Length(100);
///             References(x => x.TypeSurvey, "TYPE_SURVEY_GJI_ID").Not.Nullable().Fetch.Join();
///             References(x => x.KindCheck, "KIND_CHECK_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Вид проверок"</summary>
    public class TypeSurveyKindInspGjiMap : BaseEntityMap<TypeSurveyKindInspGji>
    {
        
        public TypeSurveyKindInspGjiMap() : 
                base("Вид проверок", "GJI_DICT_KIND_INSPECTION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Код").Column("CODE").Length(100);
            Reference(x => x.TypeSurvey, "Тип обследования").Column("TYPE_SURVEY_GJI_ID").NotNull().Fetch();
            Reference(x => x.KindCheck, "Вид обследования").Column("KIND_CHECK_ID").NotNull().Fetch();
        }
    }
}
