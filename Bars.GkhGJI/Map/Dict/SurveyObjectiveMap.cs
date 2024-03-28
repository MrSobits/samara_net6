/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности Задачи проверки.
///     /// </summary>
///     public class SurveyObjectiveMap : BaseEntityMap<SurveyObjective>
///     {
///         /// <summary>
///         /// Конструктор.
///         /// </summary>
///         public SurveyObjectiveMap() : base("GJI_DICT_SURVEY_OBJ")
///         {
///             this.Map(x => x.Code, "CODE", false, 300);
///             this.Map(x => x.Name, "NAME", true, 2000);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Справочники - ГЖИ - Задачи проверки"</summary>
    public class SurveyObjectiveMap : BaseEntityMap<SurveyObjective>
    {
        
        public SurveyObjectiveMap() : 
                base("Справочники - ГЖИ - Задачи проверки", "GJI_DICT_SURVEY_OBJ")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Код.").Column("CODE").Length(300);
            Property(x => x.Name, "Наименование.").Column("NAME").Length(2000).NotNull();
        }
    }
}
