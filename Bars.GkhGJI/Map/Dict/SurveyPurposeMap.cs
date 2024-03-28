/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности Цели проверки.
///     /// </summary>
///     public class SurveyPurposeMap : BaseEntityMap<SurveyPurpose>
///     {
///         /// <summary>
///         /// Конструктор.
///         /// </summary>
///         public SurveyPurposeMap() : base("GJI_DICT_SURVEY_PURP")
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
    
    
    /// <summary>Маппинг для "Справочники - ГЖИ - Цели проверки"</summary>
    public class SurveyPurposeMap : BaseEntityMap<SurveyPurpose>
    {
        
        public SurveyPurposeMap() : 
                base("Справочники - ГЖИ - Цели проверки", "GJI_DICT_SURVEY_PURP")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Код.").Column("CODE").Length(300);
            Property(x => x.Name, "Наименование.").Column("NAME").Length(2000).NotNull();
        }
    }
}
