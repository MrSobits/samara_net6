/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Smol.Map
/// {
///     using Bars.GkhGji.Regions.Smolensk.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "протокол смоленска"
///     /// </summary>
///     public class ActSurveySmolMap : SubclassMap<ActSurveySmol>
///     {
///         public ActSurveySmolMap()
///         {
///             Table("GJI_ACTSURVEY_SMOL");
///             KeyColumn("ID");
///             Map(x => x.DateNotification, "DATE_NOTIFICATION");
///             Map(x => x.NumberNotification, "NUMBER_NOTIFICATION").Length(100);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Smolensk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Smolensk.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Smolensk.Entities.ActSurveySmol"</summary>
    public class ActSurveySmolMap : JoinedSubClassMap<ActSurveySmol>
    {
        
        public ActSurveySmolMap() : 
                base("Bars.GkhGji.Regions.Smolensk.Entities.ActSurveySmol", "GJI_ACTSURVEY_SMOL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DateNotification, "DateNotification").Column("DATE_NOTIFICATION");
            Property(x => x.NumberNotification, "NumberNotification").Column("NUMBER_NOTIFICATION").Length(100);
        }
    }
}
