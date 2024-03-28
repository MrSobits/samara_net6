namespace Bars.Gkh.ClaimWork.ViewModel
{
    using System;
    using System.Linq;
    using B4;
    using B4.Utils;
    using Modules.ClaimWork.Entities;

    public class PetitionViewModel : BaseViewModel<Petition>
    {
        public override IDataResult List(IDomainService<Petition> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var dateStart = baseParams.Params.GetAs<DateTime?>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime?>("dateEnd");
            var roId = baseParams.Params.GetAs<long>("address");

            var data = domain.GetAll()
                .WhereIf(dateStart.HasValue, x => x.DocumentDate.Value.Date >= dateStart.Value.Date)
                .WhereIf(dateEnd.HasValue, x => x.DocumentDate.Value.Date <= dateEnd.Value.Date)
                .WhereIf(roId != 0, x => x.ClaimWork.RealityObject.Id == roId)
                .Select(x => new
                {
                    x.Id,
                    x.ClaimWork,
                    x.ClaimWork.ClaimWorkTypeBase,
                    x.DocumentDate,
                    x.DateOfRewiew,
                    x.BidNumber,
                    x.BidDate,
                    x.ClaimWork.BaseInfo,
                    Municipality = x.ClaimWork.RealityObject.Municipality.Name,
                    x.ClaimWork.RealityObject.Address
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат выполнения запроса
        /// </returns>
        public override IDataResult Get(IDomainService<Petition> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            var obj = domainService.Get(id);

            if (obj != null)
            {
                return new BaseDataResult(
                    new
                    {
                        obj.Id,
                        obj.ClaimWork,
                        obj.ClaimWork.ClaimWorkTypeBase,
                        obj.DocumentType,
                        obj.DocumentDate,
                        obj.DocumentNumber,
                        obj.DocumentNum,
                        obj.State,
                        obj.ConsiderationDate,
                        obj.ConsiderationNumber,
                        obj.WhoConsidered,
                        obj.ResultConsideration,
                        obj.LawsuitDocType,
                        obj.DateEnd,
                        obj.BidDate,
                        obj.BidNumber,
                        obj.File,
                        obj.DebtBaseTariffSum,
                        obj.DebtDecisionTariffSum,
                        obj.DebtSum,
                        obj.PenaltyDebt,
                        obj.Duty,
                        obj.Costs,
                        obj.JurInstitution,
                        obj.JuridicalSectorMu,
                        obj.DateOfAdoption,
                        obj.DateOfRewiew,
                        obj.DebtSumApproved,
                        obj.Suspended,
                        obj.DeterminationNumber,
                        obj.DeterminationDate,
                        obj.PenaltyDebtApproved,
                        obj.CbSize,
                        obj.CbDebtSum,
                        obj.CbPenaltyDebt,
                        obj.CbFactInitiated,
                        obj.CbDateInitiated,
                        obj.CbStationSsp,
                        obj.CbDateSsp,
                        obj.CbDocumentType,
                        obj.CbSumRepayment,
                        obj.CbDateDocument,
                        obj.CbNumberDocument,
                        obj.CbFile,
                        obj.CbSumStep,
                        obj.CbIsStopped,
                        obj.CbDateStopped,
                        obj.CbReasonStoppedType,
                        obj.CbReasonStoppedDescription,
                        obj.PetitionType,
                        obj.ClaimWork.BaseInfo,
                        obj.ClaimWork.RealityObject?.Municipality,
                        obj.ClaimWork.RealityObject?.Address,
                        obj.DutyPostponement,
                        obj.DebtStartDate,
                        obj.DebtEndDate,
                        obj.DebtCalcMethod,
                        obj.Description,
                        obj.JudgeName,
                        obj.NumberCourtBuisness,
                        obj.IsDeterminationCancel,
                        obj.DateDeterminationCancel,
                        obj.IsDeterminationOfTurning,
                        obj.DateDeterminationOfTurning,
                        obj.IsDeterminationRenouncement,
                        obj.DateDeterminationRenouncement,
                        obj.IsDeterminationReturn,
                        obj.DateDeterminationReturn,
                        obj.FKRAmountCollected,
                        obj.DateJudicalOrder,
                        obj.PayDocDate,
                        obj.PayDocNumber,
                        obj.DutyPayed,
                        obj.MoneyLess,
                        obj.IsMotionless,
                        obj.DateMotionless,
                        obj.IsErrorFix,
                        obj.DateErrorFix,
                        obj.IsLimitationOfActions,
                        obj.DateLimitationOfActions,
                        obj.IsLawsuitDistanceDecisionCancel,
                        obj.DateLawsuitDistanceDecisionCancel,
                        obj.RedirectDate,
                        obj.LawsuitDistanceDecisionCancelComment,
                        obj.DutyDebtApproved,
                        obj.IsDenied,
                        obj.DeniedDate,
                        obj.IsDeniedAdmission,
                        obj.DeniedAdmissionDate,
                        obj.IsDirectedByJuridiction,
                        obj.DirectedByJuridictionDate,
                        obj.DirectedToDebtor
                    });
            }
            return new BaseDataResult();
        }
    }
}