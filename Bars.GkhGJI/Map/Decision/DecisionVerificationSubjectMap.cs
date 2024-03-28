namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.Disposal.DisposalVerificationSubject"</summary>
    public class DecisionVerificationSubjectMap : BaseEntityMap<DecisionVerificationSubject>
    {
        
        public DecisionVerificationSubjectMap() : 
                base("Bars.GkhGji.Entities.DisposalVerificationSubject", "GJI_DECISION_VERIFSUBJ")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.Decision, "Disposal").Column("DECISION_ID").NotNull().Fetch();
            this.Reference(x => x.SurveySubject, "SurveySubject").Column("SURVEY_SUBJECT_ID").Fetch();
        }
    }
}
