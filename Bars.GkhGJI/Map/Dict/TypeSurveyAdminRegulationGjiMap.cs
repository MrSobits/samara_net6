/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг сущности "Вид обследования"
///     /// </summary>
///     public class TypeSurveyAdminRegulationGjiMap : BaseEntityMap<TypeSurveyAdminRegulationGji>
///     {
///         public TypeSurveyAdminRegulationGjiMap() : base("GJI_DICT_ADM_REG")
///         {
///             References(x => x.TypeSurvey, "TYPE_SURVEY_GJI_ID").Not.Nullable().Fetch.Join();
///             References(x => x.NormativeDoc, "NORM_DOC_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Административные регламенты"</summary>
    public class TypeSurveyAdminRegulationGjiMap : BaseEntityMap<TypeSurveyAdminRegulationGji>
    {
        
        public TypeSurveyAdminRegulationGjiMap() : 
                base("Административные регламенты", "GJI_DICT_ADM_REG")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.TypeSurvey, "Тип обследования").Column("TYPE_SURVEY_GJI_ID").NotNull().Fetch();
            Reference(x => x.NormativeDoc, "Нормативный документ").Column("NORM_DOC_ID").NotNull().Fetch();
        }
    }
}
