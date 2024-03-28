namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для гис ЕРП</summary>
    public class CourtPracticeDisputeHistoryMap : BaseEntityMap<CourtPracticeDisputeHistory>
    {
        
        public CourtPracticeDisputeHistoryMap() : 
                base("Административная практика", "GJI_CH_COURT_PRACTICE_HISTORY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.CourtMeetingResult, "CourtMeetingResult").Column("CM_RESULT");
            Property(x => x.CourtMeetingTime, "CourtMeetingTime").Column("CM_TIME");
            Property(x => x.CourtPracticeState, "CourtPracticeState").Column("CP_STATE");
            Property(x => x.CourtCosts, "CourtСosts").Column("IS_COST");
            Property(x => x.CourtCostsFact, "CourtСostsFact").Column("COST_FACT");
            Property(x => x.CourtCostsPlan, "CourtСostsPlan").Column("COST_PLAN");
            Property(x => x.DateCourtMeeting, "DateCourtMeeting").Column("CM_DATE").NotNull();          
            Property(x => x.Discription, "Discription").Column("COMMENT");      
            Property(x => x.InLaw, "InLaw").Column("IS_INLAW");
            Property(x => x.InLawDate, "InLawDate").Column("INLAW_DATE");
            Property(x => x.InterimMeasures, "InterimMeasures").Column("IS_MEASURES");
            Property(x => x.InterimMeasuresDate, "InterimMeasuresDate").Column("MEASURES_DATE");
            Property(x => x.PerformanceList, "PerformanceList").Column("PERF_LIST");
            Property(x => x.PerformanceProceeding, "PerformanceProceeding").Column("PERF_PROC");          
            Property(x => x.Dispute, "Dispute").Column("IS_DISPUTE");
            Property(x => x.PausedComment, "PausedComment").Column("PAUSED_COMMENT");          
            Reference(x => x.InstanceGji, "InstanceGji").Column("INSTANCE_ID");          
            Reference(x => x.JurInstitution, "JurInstitution").Column("JUR_INST_ID").NotNull();
            Reference(x => x.FileInfo, "FileInfo").Column("FILE_ID");
            Reference(x => x.CourtPractice, "CourtPractice").Column("CP_ID").Fetch();

        }
    }
}
