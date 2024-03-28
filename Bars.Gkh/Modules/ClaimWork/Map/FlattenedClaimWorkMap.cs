namespace Bars.Gkh.Modules.ClaimWork.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Modules.ClaimWork.Entities;

    /// <summary>Маппинг для "Основание претензионно исковой работы"</summary>
    // ReSharper disable once UnusedMember.Global
    public class FlattenedClaimWorkMap : BaseEntityMap<FlattenedClaimWork>
    {
        public FlattenedClaimWorkMap()
            :
            base("Архивные и долевые ПИР", "FLATTENED_CLAIM_WORK")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Num, "Num").Column("Num");
            this.Property(x => x.Share, "Share").Column("Share");
            this.Property(x => x.DebtorFullname, "DebtorFullname").Column("DebtorFullname");
            this.Property(x => x.DebtorRoomAddress, "DebtorRoomAddress").Column("DebtorRoomAddress");
            this.Property(x => x.DebtorRoomType, "DebtorRoomType").Column("DebtorRoomType");
            this.Property(x => x.DebtorRoomNumber, "DebtorRoomNumber").Column("DebtorRoomNumber");
            this.Property(x => x.DebtorDebtPeriod, "DebtorDebtPeriod").Column("DebtorDebtPeriod");
            this.Property(x => x.DebtorDebtAmount, "DebtorDebtAmount").Column("DebtorDebtAmount");
            this.Property(x => x.DebtorDutyAmount, "DebtorDutyAmount").Column("DebtorDutyAmount");
            this.Property(x => x.DebtorDebtPaymentDate, "DebtorDebtPaymentDate").Column("DebtorDebtPaymentDate");
            this.Property(x => x.DebtorDutyPaymentAssignment, "DebtorDutyPaymentAssignment").Column("DebtorDutyPaymentAssignment");
            this.Property(x => x.DebtorClaimDeliveryDate, "DebtorClaimDeliveryDate").Column("DebtorClaimDeliveryDate");
            this.Property(x => x.DebtorPaymentsAfterCourtOrder, "DebtorPaymentsAfterCourtOrder").Column("DebtorPaymentsAfterCourtOrder");
            this.Property(x => x.DebtorJurInstType, "DebtorJurInstType").Column("DebtorJurInstType");
            this.Property(x => x.DebtorJurInstName, "DebtorJurInstName").Column("DebtorJurInstName");
            this.Property(x => x.CourtClaimNum, "CourtClaimNum").Column("CourtClaimNum");
            this.Property(x => x.CourtClaimDate, "CourtClaimDate").Column("CourtClaimDate");
            this.Property(x => x.CourtClaimConsiderationResult, "CourtClaimConsiderationResult").Column("CourtClaimConsiderationResult");
            this.Property(x => x.CourtClaimCancellationDate, "CourtClaimCancellationDate").Column("CourtClaimCancellationDate");
            this.Property(x => x.CourtClaimRospName, "CourtClaimRospName").Column("CourtClaimRospName");
            this.Property(x => x.CourtClaimRospDate, "CourtClaimRospDate").Column("CourtClaimRospDate");
            this.Property(x => x.CourtClaimEnforcementProcNum, "CourtClaimEnforcementProcNum").Column("CourtClaimEnforcementProcNum");
            this.Property(x => x.CourtClaimEnforcementProcDate, "CourtClaimEnforcementProcDate").Column("CourtClaimEnforcementProcDate");
            this.Property(x => x.CourtClaimPaymentAssignmentNum, "CourtClaimPaymentAssignmentNum").Column("CourtClaimPaymentAssignmentNum");
            this.Property(x => x.CourtClaimPaymentAssignmentDate, "CourtClaimPaymentAssignmentDate").Column("CourtClaimPaymentAssignmentDate");
            this.Property(x => x.CourtClaimRospDebtExact, "CourtClaimRospDebtExact").Column("CourtClaimRospDebtExact");
            this.Property(x => x.CourtClaimRospDutyExact, "CourtClaimRospDutyExact").Column("CourtClaimRospDutyExact");
            this.Property(x => x.CourtClaimEnforcementProcActEndNum, "CourtClaimEnforcementProcActEndNum").Column("CourtClaimEnforcementProcActEndNum");
            this.Property(x => x.CourtClaimDeterminationTurnDate, "CourtClaimDeterminationTurnDate").Column("CourtClaimDeterminationTurnDate");
            this.Property(x => x.FkrRospName, "FkrRospName").Column("FkrRospName");
            this.Property(x => x.FkrEnforcementProcDecisionNum, "FkrEnforcementProcDecisionNum").Column("FkrEnforcementProcDecisionNum");
            this.Property(x => x.FkrEnforcementProcDate, "FkrEnforcementProcDate").Column("FkrEnforcementProcDate");
            this.Property(x => x.FkrPaymentAssignementNum, "FkrPaymentAssignementNum").Column("FkrPaymentAssignementNum");
            this.Property(x => x.FkrPaymentAssignmentDate, "FkrPaymentAssignmentDate").Column("FkrPaymentAssignmentDate");
            this.Property(x => x.FkrDebtExact, "FkrDebtExact").Column("FkrDebtExact");
            this.Property(x => x.FkrDutyExact, "FkrDutyExact").Column("FkrDutyExact");
            this.Property(x => x.FkrEnforcementProcActEndNum, "FkrEnforcementProcActEndNum").Column("FkrEnforcementProcActEndNum");
            this.Property(x => x.LawsuitCourtDeliveryDate, "LawsuitCourtDeliveryDate").Column("LawsuitCourtDeliveryDate");
            this.Property(x => x.LawsuitDocNum, "LawsuitDocNum").Column("LawsuitDocNum");
            this.Property(x => x.LawsuitConsiderationDate, "LawsuitConsiderationDate").Column("LawsuitConsiderationDate");
            //this.Property(x => x.LawsuitConsiderationResult, "FkrConsiderationResult").Column("FkrConsiderationResult");
            this.Property(x => x.LawsuitDebtExact, "LawsuitDebtExact").Column("LawsuitDebtExact");
            this.Property(x => x.LawsuitDutyExact, "LawsuitDutyExact").Column("LawsuitDutyExact");
            this.Property(x => x.ListListNum, "ListListNum").Column("ListListNum");
            this.Property(x => x.ListListRopsDate, "ListListRopsDate").Column("ListListRopsDate");
            this.Property(x => x.ListRospName, "ListRospName").Column("ListRospName");
            this.Property(x => x.ListRospDate, "ListRospDate").Column("ListRospDate");
            this.Property(x => x.ListEnfProcDecisionNum, "ListEnfProcDecisionNum").Column("ListEnfProcDecisionNum");
            this.Property(x => x.ListEnfProcDate, "ListEnfProcDate").Column("ListEnfProcDate");
            this.Property(x => x.ListPaymentAssignmentNum, "ListPaymentAssignmentNum").Column("ListPaymentAssignmentNum");
            this.Property(x => x.ListPaymentAssignmentDate, "ListPaymentAssignmentDate").Column("ListPaymentAssignmentDate");
            this.Property(x => x.ListRospDebtExacted, "ListRospDebtExacted").Column("ListRospDebtExacted");
            this.Property(x => x.ListRospDutyExacted, "ListRospDutyExacted").Column("ListRospDutyExacted");
            this.Property(x => x.ListEnfProcActEndNum, "ListEnfProcActEndNum").Column("ListEnfProcActEndNum");
            this.Property(x => x.Note, "Note").Column("Note");
            this.Property(x => x.RloiId, "RloiId").Column("RloiId");
            this.Property(x => x.Archived, "Archived").Column("Archived");
            this.Property(x => x.ZVSPCourtDecision, "ZVSPCourtDecision").Column("ZVSPCourtDecision");
            this.Property(x => x.ZVSPPenaltyAmmount, "ZVSPPenaltyAmmount").Column("ZVSPPenaltyAmmount");
            this.Property(x => x.LawsuitDeterminationMotionless, "LawsuitDeterminationMotionless").Column("LawsuitDeterminationMotionless");
            this.Property(x => x.LawsuitDeterminationMotionlessDate, "LawsuitDeterminationMotionlessDate").Column("LawsuitDeterminationMotionlessDate");
            this.Property(x => x.LawsuitDeterminationDenail, "LawsuitDeterminationDenail").Column("LawsuitDeterminationDenail");
            this.Property(x => x.LawsuitDeterminationDenailDate, "LawsuitDeterminationDenailDate").Column("LawsuitDeterminationDenailDate");
            this.Property(x => x.LawsuitDeterminationJurDirected, "LawsuitDeterminationJurDirected").Column("LawsuitDeterminationJurDirected");
            this.Property(x => x.LawsuitDeterminationJurDirectedDate, "LawsuitDeterminationJurDirectedDate").Column("LawsuitDeterminationJurDirectedDate");
            this.Property(x => x.LawsuitDeterminationReturn, "LawsuitDeterminationReturn").Column("LawsuitDeterminationReturn");
            this.Property(x => x.LawsuitDeterminationReturnDate, "LawsuitDeterminationReturnDate").Column("LawsuitDeterminationReturnDate");
            this.Property(x => x.DebtorPenaltyAmount, "Пени").Column("DebtorPenaltyAmount");
            this.Property(x => x.LawsuitDeterminationMotionFix, "Устранение недостатков").Column("LawsuitDeterminationMotionFix");
            this.Property(x => x.LawsuitDocType, "тип Документа").Column("LawsuitDocType");
            this.Property(x => x.LawsuitDistanceDecisionCancel, "Отмена заочного решения").Column("LawsuitDistanceDecisionCancel");
            this.Property(x => x.InstallmentPlan, "Рассрочка").Column("InstallmentPlan");
            this.Property(x => x.LawsuitResultConsideration, "Результат рассмотрения иска").Column("LawsuitResultConsideration");
        }
    }
}