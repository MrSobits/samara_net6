namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для гис ЕРП</summary>
    public class CourtPracticeRealityObjectMap : BaseEntityMap<CourtPracticeRealityObject>
    {
        
        public CourtPracticeRealityObjectMap() : 
                base("Место возникновения проблемы", "GJI_VR_COURT_PRACTICE_RO")
        {
        }
        
        protected override void Map()
        {
          
            Reference(x => x.CourtPractice, "ContragentDefendant").Column("CP_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "RealityObject").Column("RO_ID").NotNull().Fetch();           
        }
    }
}
