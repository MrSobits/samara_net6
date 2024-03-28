/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Дом акта обследования"
///     /// </summary>
///     public class ActSurveyRealityObjectMap : BaseEntityMap<ActSurveyRealityObject>
///     {
///         public ActSurveyRealityObjectMap()
///             : base("GJI_ACTSURVEY_ROBJECT")
///         {
///             References(x => x.ActSurvey, "ACTSURVEY_ID").Not.Nullable().Fetch.Join();
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Дом акта обследования Данная таблица хранит всебе все дома акта обследования"</summary>
    public class ActSurveyRealityObjectMap : BaseEntityMap<ActSurveyRealityObject>
    {
        
        public ActSurveyRealityObjectMap() : 
                base("Дом акта обследования Данная таблица хранит всебе все дома акта обследования", "GJI_ACTSURVEY_ROBJECT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ActSurvey, "Акт обследования").Column("ACTSURVEY_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull().Fetch();
        }
    }
}
