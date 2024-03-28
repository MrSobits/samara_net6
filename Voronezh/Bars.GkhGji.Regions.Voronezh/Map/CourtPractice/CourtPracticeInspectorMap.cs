namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для гис ЕРП</summary>
    public class CourtPracticeInspectorMap : BaseEntityMap<CourtPracticeInspector>
    {
        
        public CourtPracticeInspectorMap() : 
                base("Инспектор юрист", "GJI_VR_COURT_PRACTICE_INSPECTOR")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.LawyerInspector, "LawyerInspector").Column("LAWYER_INSPECTOR");
            Property(x => x.Discription, "Discription").Column("DESCRIPTION");

            Reference(x => x.CourtPractice, "CourtPractice").Column("CP_ID").NotNull().Fetch();
            Reference(x => x.Inspector, "Inspector").Column("INSPECTOR_ID").NotNull().Fetch();           
        }
    }
}
