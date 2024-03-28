/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tula.Map
/// {
///     using Bars.Gkh.Enums;
/// 
///     using Bars.GkhGji.Regions.Tula.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Акт обследования"
///     /// </summary>
///     public class TulaActSurveyMap : SubclassMap<TulaActSurvey>
///     {
///         public TulaActSurveyMap()
///         {
///             Table("TULA_GJI_ACTSURVEY");
///             KeyColumn("ID");
/// 
///             Map(x => x.ConclusionIssued, "CON_ISSUED").Not.Nullable().CustomType<YesNo>();
///             Map(x => x.DateOf, "DATE_OF");
///             Map(x => x.TimeEnd, "TIME_END");
///             Map(x => x.TimeStart, "TIME_START");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tula.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tula.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tula.Entities.TulaActSurvey"</summary>
    public class TulaActSurveyMap : JoinedSubClassMap<TulaActSurvey>
    {
        
        public TulaActSurveyMap() : 
                base("Bars.GkhGji.Regions.Tula.Entities.TulaActSurvey", "TULA_GJI_ACTSURVEY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ConclusionIssued, "ConclusionIssued").Column("CON_ISSUED").NotNull();
            Property(x => x.DateOf, "DateOf").Column("DATE_OF");
            Property(x => x.TimeEnd, "TimeEnd").Column("TIME_END");
            Property(x => x.TimeStart, "TimeStart").Column("TIME_START");
        }
    }
}
