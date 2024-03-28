namespace Bars.Gkh.RegOperator.Modules.ClaimWork.Map
{
    using Entity;
    using B4.Modules.Mapping.Mappers;

    /// <summary>Маппинг для "Bars.Gkh.RegOperator.Modules.ClaimWork.Entity.ViewDebtor"</summary>
    public class ViewDebtorMap : PersistentObjectMap<ViewDebtor>
    {

        /// <summary>
        /// Маппинг для View "Реестр неплательщиков"
        /// </summary>
        public ViewDebtorMap() :
                base("Bars.Gkh.RegOperator.Modules.ClaimWork.Entity.ViewDebtor", "VIEW_CLW_DEBTOR")
        {
        }

        /// <summary>
        /// Маппинг для View "Реестр неплательщиков"
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.ClwId, "ClwId").Column("CLW_ID");
            this.Property(x => x.DocumentType, "DocumentType").Column("DOCUMENT_TYPE");
            this.Property(x => x.OwnerType, "OwnerType").Column("OWNER_TYPE");
            this.Property(x => x.OwnerName, "OwnerName").Column("OWNER_NAME").Length(250);
            this.Property(x => x.RoId, "RoId").Column("RO_ID");
            this.Property(x => x.Address, "Address").Column("ADDRESS").Length(250);
            this.Property(x => x.RoomId, "RoomId").Column("ROOM_ID");
            this.Property(x => x.RoomAddress, "RoomAddress").Column("ROOM_ADDRESS").Length(250);
            this.Property(x => x.MuId, "MuId").Column("MU_ID");
            this.Property(x => x.Municipality, "Municipality").Column("MUNICIPALITY").Length(250);
            this.Property(x => x.StlId, "StlId").Column("STL_ID");
            this.Property(x => x.Settlement, "Settlement").Column("SETTLEMENT").Length(250);
            this.Property(x => x.JurSectorNumber, "JurSectorNumber").Column("JURSECTOR_NUMBER").Length(250);
            this.Property(x => x.LawsuitDocDate, "LawsuitDocDate").Column("LS_DOC_DATE");
            this.Property(x => x.LawsuitDocNumber, "LawsuitDocNumber").Column("LS_DOC_NUMBER").Length(250);
            this.Property(x => x.WhoConsidered, "WhoConsidered").Column("WHO_CONSIDERED");
            this.Property(x => x.DateOfAdoption, "DateOfAdoption").Column("DATE_OF_ADOPTION");
            this.Property(x => x.DateOfReview, "DateOfReview").Column("DATE_OF_REWIEW");
            this.Property(x => x.ResultConsideration, "ResultConsideration").Column("RESULT_CONSIDERATION");
            this.Property(x => x.DebtSumApproved, "DebtSumApproved").Column("DEBT_SUM_APPROV");
            this.Property(x => x.PenaltyDebtApproved, "PenaltyDebtApproved").Column("PENALTY_DEBT_APPROV");
            this.Property(x => x.LawsuitDocType, "LawsuitDocType").Column("LAW_TYPE_DOCUMENT");
            this.Property(x => x.DateConsideration, "DateConsideration").Column("DATE_CONSIDERAT");
            this.Property(x => x.NumberConsideration, "NumberConsideration").Column("NUM_CONSIDERAT").Length(250);
            this.Property(x => x.CbDebtSum, "CbDebtSum").Column("CB_DEBT_SUM");
            this.Property(x => x.CbPenaltyDebtSum, "CbPenaltyDebtSum").Column("CB_PENALTY_DEBT_SUM");
            this.Property(x => x.PretensionDocDate, "PretensionDocDate").Column("PRET_DOC_DATE");
            this.Property(x => x.PretensionDocNumber, "PretensionDocNumber").Column("PRET_DOC_NUMBER").Length(250);
            this.Property(x => x.PretensionDebtSum, "PretensionDebtSum").Column("PRET_SUM");
            this.Property(x => x.PretensionPenaltyDebtSum, "PretensionPenaltyDebtSum").Column("PRET_PENALTY_SUM");
            this.Property(x => x.RepaidBeforeDecision, "RepaymentType").Column("REPAID_BEFORE_DEC");
            this.Property(x => x.RepaidBeforeExecutionProceedings, "RepaymentType").Column("REPAID_BEFORE_EX_PROC");
            this.Property(x => x.Objection, "Objection").Column("Objection").NotNull();
        }
    }
}
