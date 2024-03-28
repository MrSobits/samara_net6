/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Инспектируемая часть акта обследования ГЖИ"
///     /// </summary>
///     public class ActSurveyInspectedPartMap : BaseEntityMap<ActSurveyInspectedPart>
///     {
///         public ActSurveyInspectedPartMap()
///             : base("GJI_ACTSURVEY_INSPECTPART")
///         {
///             Map(x => x.Character, "CHARACTER").Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(500);
/// 
///             References(x => x.ActSurvey, "ACTSURVEY_ID").Not.Nullable().Fetch.Join();
///             References(x => x.InspectedPart, "INSPECTIONPART_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Инспектируемая часть в акте обследования"</summary>
    public class ActSurveyInspectedPartMap : BaseEntityMap<ActSurveyInspectedPart>
    {
        
        public ActSurveyInspectedPartMap() : 
                base("Инспектируемая часть в акте обследования", "GJI_ACTSURVEY_INSPECTPART")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Character, "Характер и местоположение").Column("CHARACTER").Length(300);
            Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(500);
            Reference(x => x.ActSurvey, "Акт обследования").Column("ACTSURVEY_ID").NotNull().Fetch();
            Reference(x => x.InspectedPart, "Инспектируемая часть").Column("INSPECTIONPART_ID").NotNull().Fetch();
        }
    }
}
