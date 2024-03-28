/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Акт обследования"
///     /// </summary>
///     public class ActSurveyMap : SubclassMap<ActSurvey>
///     {
///         public ActSurveyMap()
///         {
///             Table("GJI_ACTSURVEY");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
///             Map(x => x.Area, "AREA");
///             Map(x => x.Flat, "FLAT").Length(10);
///             Map(x => x.Reason, "REASON").Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(500);
///             Map(x => x.FactSurveyed, "FACT_SURVEYED").Not.Nullable().CustomType<SurveyResult>();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Акт обследования"</summary>
    public class ActSurveyMap : JoinedSubClassMap<ActSurvey>
    {
        
        public ActSurveyMap() : 
                base("Акт обследования", "GJI_ACTSURVEY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Area, "Обследованная площадь").Column("AREA");
            Property(x => x.Flat, "Квартира").Column("FLAT").Length(10);
            Property(x => x.Reason, "Причина").Column("REASON").Length(300);
            Property(x => x.Description, "Выводы по результату").Column("DESCRIPTION").Length(500);
            Property(x => x.FactSurveyed, "Факт обследования").Column("FACT_SURVEYED").NotNull();
        }
    }
}
