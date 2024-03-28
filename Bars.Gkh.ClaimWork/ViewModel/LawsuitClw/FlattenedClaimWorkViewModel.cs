namespace Bars.Gkh.ClaimWork.ViewModel
{
    using System.Linq;

    using B4;

    using Bars.B4.Utils;

    using Modules.ClaimWork.Entities;

    public class FlattenedClaimWorkViewModel : BaseViewModel<FlattenedClaimWork>
    {
        public override IDataResult List(IDomainService<FlattenedClaimWork> domain, BaseParams baseParams)
        {
            LoadParam loadParams = this.GetLoadParam(baseParams);
            var archived = loadParams.Filter.GetValue("Archived");

            var tmpdata = domain.GetAll();
            if (archived is bool)
            {
                tmpdata = tmpdata.Where(x => x.Archived == (bool)archived);
            }

            var data = tmpdata.Select(
                    x => new
                    {
                        x.Id,
                        x.Num,
                        x.Share,
                        x.DebtorFullname,
                        x.DebtorRoomAddress,
                        x.DebtorRoomType,
                        x.DebtorRoomNumber,
                        x.DebtorDebtPeriod,
                        x.DebtorDebtAmount,
                        x.DebtorDutyAmount,
                        x.DebtorDebtPaymentDate,
                        x.DebtorDutyPaymentAssignment,
                        x.DebtorClaimDeliveryDate,
                        x.DebtorJurInstType,
                        x.DebtorJurInstName,
                        x.CourtClaimNum,
                        x.CourtClaimDate,
                        x.CourtClaimConsiderationResult,
                        x.ZVSPCourtDecision,
                        x.CourtClaimCancellationDate,
                        x.CourtClaimRospName,
                        x.CourtClaimRospDate,
                        x.CourtClaimEnforcementProcNum,
                        x.CourtClaimEnforcementProcDate,
                        x.CourtClaimPaymentAssignmentNum,
                        x.CourtClaimPaymentAssignmentDate,
                        x.CourtClaimRospDebtExact,
                        x.CourtClaimRospDutyExact,
                        x.CourtClaimEnforcementProcActEndNum,
                        x.CourtClaimDeterminationTurnDate,
                        x.FkrRospName,
                        x.FkrEnforcementProcDecisionNum,
                        x.FkrEnforcementProcDate,
                        x.FkrPaymentAssignementNum,
                        x.FkrPaymentAssignmentDate,
                        x.FkrDebtExact,
                        x.FkrDutyExact,
                        x.FkrEnforcementProcActEndNum,
                        x.LawsuitCourtDeliveryDate,
                        x.LawsuitDocNum,
                        x.LawsuitConsiderationDate,
                        //x.LawsuitConsiderationResult,
                        x.LawsuitDebtExact,
                        x.LawsuitDutyExact,
                        x.ListListNum,
                        x.ListListRopsDate,
                        x.ListRospName,
                        x.ListRospDate,
                        x.ListEnfProcDecisionNum,
                        x.ListEnfProcDate,
                        x.ListPaymentAssignmentNum,
                        x.ListPaymentAssignmentDate,
                        x.ListRospDebtExacted,
                        x.ListRospDutyExacted,
                        x.ListEnfProcActEndNum,
                        x.RloiId,
                        x.Archived,
                        x.Note                        
                    })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат выполнения запроса
        /// </returns>
        public override IDataResult Get(IDomainService<FlattenedClaimWork> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            FlattenedClaimWork obj = domainService.Get(id);

            if (obj != null)
            {
                return new BaseDataResult(
                    new
                    {
                        obj.Id,
                        obj.Num,
                        obj.DebtorFullname,
                        obj.DebtorRoomAddress,
                        obj.DebtorRoomType,
                        obj.DebtorRoomNumber,
                        obj.DebtorDebtPeriod,
                        obj.DebtorDebtAmount,
                        obj.DebtorDutyAmount,
                        obj.DebtorDebtPaymentDate,
                        obj.DebtorDutyPaymentAssignment,
                        obj.DebtorClaimDeliveryDate,
                        obj.DebtorJurInstType,
                        obj.DebtorJurInstName,
                        obj.CourtClaimNum,
                        obj.CourtClaimDate,
                        obj.CourtClaimConsiderationResult,
                        obj.ZVSPCourtDecision,
                        obj.CourtClaimCancellationDate,
                        obj.CourtClaimRospName,
                        obj.CourtClaimRospDate,
                        obj.CourtClaimEnforcementProcNum,
                        obj.CourtClaimEnforcementProcDate,
                        obj.CourtClaimPaymentAssignmentNum,
                        obj.CourtClaimPaymentAssignmentDate,
                        obj.CourtClaimRospDebtExact,
                        obj.CourtClaimRospDutyExact,
                        obj.CourtClaimEnforcementProcActEndNum,
                        obj.CourtClaimDeterminationTurnDate,
                        obj.FkrRospName,
                        obj.FkrEnforcementProcDecisionNum,
                        obj.FkrEnforcementProcDate,
                        obj.FkrPaymentAssignementNum,
                        obj.FkrPaymentAssignmentDate,
                        obj.FkrDebtExact,
                        obj.FkrDutyExact,
                        obj.FkrEnforcementProcActEndNum,
                        obj.LawsuitCourtDeliveryDate,
                        obj.LawsuitDocNum,
                        obj.LawsuitConsiderationDate,
                        //FkrConsiderationResult = obj.LawsuitConsiderationResult,
                        obj.LawsuitDebtExact,
                        obj.LawsuitDutyExact,
                        obj.ListListNum,
                        obj.ListListRopsDate,
                        obj.ListRospName,
                        obj.ListRospDate,
                        obj.ListEnfProcDecisionNum,
                        obj.ListEnfProcDate,
                        obj.ListPaymentAssignmentNum,
                        obj.ListPaymentAssignmentDate,
                        obj.ListRospDebtExacted,
                        obj.ListRospDutyExacted,
                        obj.ListEnfProcActEndNum,
                        obj.RloiId,
                        obj.Archived,
                        obj.Note,
                        obj.Share,
                        obj.ZVSPPenaltyAmmount,
                        obj.LawsuitDeterminationDenail,
                        obj.LawsuitDeterminationDenailDate,
                        obj.LawsuitDeterminationJurDirected,
                        obj.LawsuitDeterminationJurDirectedDate,
                        obj.LawsuitDeterminationMotionless,
                        obj.LawsuitDeterminationMotionlessDate,
                        obj.LawsuitDeterminationReturn,
                        obj.LawsuitDeterminationReturnDate,
                        obj.LawsuitResultConsideration,
                        obj.InstallmentPlan,
                        obj.LawsuitDeterminationMotionFix,
                        obj.DebtorPenaltyAmount,
                        obj.LawsuitDocType,
                        obj.LawsuitDistanceDecisionCancel
                    });
            }

            return new BaseDataResult();
        }
    }
}