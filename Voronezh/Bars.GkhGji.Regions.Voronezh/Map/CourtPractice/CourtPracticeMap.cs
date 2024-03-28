namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для гис ЕРП</summary>
    public class CourtPracticeMap : BaseEntityMap<CourtPractice>
    {
        
        public CourtPracticeMap() : 
                base("Административная практика", "GJI_VR_COURT_PRACTICE")
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
            Property(x => x.DefendantAddress, "DefendantAddress").Column("DEF_ADDRESS");
            Property(x => x.DefendantFio, "DefendantFio").Column("DEF_FIO");
            Property(x => x.DifferentAddress, "DifferentAddress").Column("DIFF_ADDRESS");
            Property(x => x.DifferentFIo, "DifferentFIo").Column("DIFF_FIO");
            Property(x => x.Discription, "Discription").Column("COMMENT");
            Property(x => x.DisputeCategory, "DisputeCategory").Column("DISP_CATEGORY");
            Property(x => x.DisputeType, "DisputeType").Column("DISP_TYPE");
            Property(x => x.DocumentNumber, "DocumentNumber").Column("DOC_NUMBER");        
            Property(x => x.InLaw, "InLaw").Column("IS_INLAW");
            Property(x => x.InLawDate, "InLawDate").Column("INLAW_DATE");
            Property(x => x.InterimMeasures, "InterimMeasures").Column("IS_MEASURES");
            Property(x => x.InterimMeasuresDate, "InterimMeasuresDate").Column("MEASURES_DATE");
            Property(x => x.PerformanceList, "PerformanceList").Column("PERF_LIST");
            Property(x => x.PerformanceProceeding, "PerformanceProceeding").Column("PERF_PROC");
            Property(x => x.PlaintiffAddress, "PlaintiffAddress").Column("PLANT_ADDRESS");
            Property(x => x.PlaintiffFio, "PlaintiffFio").Column("PLANT_FIO");
            Property(x => x.Dispute, "Dispute").Column("IS_DISPUTE");
            Property(x => x.PausedComment, "PausedComment").Column("PAUSED_COMMENT");

            Reference(x => x.InstanceGji, "InstanceGji").Column("INSTANCE_ID");
            Reference(x => x.DocumentGji, "DocumentGji").Column("DOCUMENT_ID");
            Reference(x => x.TypeFactViolation, "TypeFactViolation").Column("TYPE_FACT_ID");
            Reference(x => x.ContragentDefendant, "ContragentDefendant").Column("CONTRAGENT_D_ID");
            Reference(x => x.JurInstitution, "JurInstitution").Column("JUR_INST_ID").NotNull();
            Reference(x => x.FileInfo, "FileInfo").Column("FILE_ID");
            Reference(x => x.State, "State").Column("STATE_ID");
            Reference(x => x.ContragentPlaintiff, "ContragentPlaintiff").Column("CONTRAGENT_P_ID");
            Reference(x => x.DifferentContragent, "DifferentContragent").Column("CONTRAGENT_DIFF_ID");
            Reference(x => x.MKDLicRequest, "MKDLicRequest").Column("REQUEST_ID");
        }
    }
}
