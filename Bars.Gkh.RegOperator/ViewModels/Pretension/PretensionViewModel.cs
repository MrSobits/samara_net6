namespace Bars.Gkh.RegOperator.ViewModels.Pretension
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.ClaimWork.Entities;
    using Bars.Gkh.ConfigSections.ClaimWork.Debtor;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Enums.ClaimWork;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    public class PretensionViewModel : BaseViewModel<PretensionClw>
    {
        /// <inheritdoc />
        public override IDataResult Get(IDomainService<PretensionClw> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var record = domainService.Get(id);

            if (record.IsNull())
            {
                return BaseDataResult.Error("Не удалось получить притензию");
            }

            var individualPaymentPlannedPeriodConfigValue = this.Container.GetGkhConfig<DebtorClaimWorkConfig>().Individual.Pretension.PaymentPlannedPeriod;
            var legalPaymentPlannedPeriodConfigValue = this.Container.GetGkhConfig<DebtorClaimWorkConfig>().Legal.Pretension.PaymentPlannedPeriod;
            var debtor = record.ClaimWork as DebtorClaimWork;

            var result = new
            {
                record.Id,
                record.ClaimWork,
                record.DocumentType,
                record.DocumentDate,
                record.DocumentNumber,
                record.State,
                record.DateReview,
                record.DebtBaseTariffSum,
                record.DebtDecisionTariffSum,
                record.Sum,
                record.Penalty,
                record.SumPenaltyCalc,
                record.File,
                record.SendDate,
                record.RequirementSatisfaction,
                PaymentPlannedPeriod = record.DocumentDate.HasValue && debtor != null
                    ? debtor.DebtorType == DebtorType.Individual
                        ? (DateTime?)record.DocumentDate.Value.AddDays(individualPaymentPlannedPeriodConfigValue)
                        : (DateTime?)record.DocumentDate.Value.AddDays(legalPaymentPlannedPeriodConfigValue)
                    : null,
                record.NumberPretension
            };
            return new BaseDataResult(result);
        }

        /// <inheritdoc />
        public override IDataResult List(IDomainService<PretensionClw> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var dateStart = baseParams.Params.GetAs<DateTime?>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime?>("dateEnd");
            var roId = baseParams.Params.GetAs<long>("address");

            var individualPaymentPlannedPeriodConfigValue = this.Container.GetGkhConfig<DebtorClaimWorkConfig>().Individual.Pretension.PaymentPlannedPeriod;
            var legalPaymentPlannedPeriodConfigValue = this.Container.GetGkhConfig<DebtorClaimWorkConfig>().Legal.Pretension.PaymentPlannedPeriod;

            var data = domain.GetAll()
                .Where(x => x.ClaimWork.ClaimWorkTypeBase == ClaimWorkTypeBase.Debtor)
                .WhereIf(dateStart.HasValue, x => x.DocumentDate.Value.Date >= dateStart.Value.Date)
                .WhereIf(dateEnd.HasValue, x => x.DocumentDate.Value.Date <= dateEnd.Value.Date)
                .WhereIf(roId != 0, x => x.ClaimWork.RealityObject.Id == roId)
                .Select(x => new
                {
                    x.Id,
                    x.ClaimWork,
                    x.ClaimWork.ClaimWorkTypeBase,
                    x.DocumentDate,
                    x.DateReview,
                    x.DebtBaseTariffSum,
                    x.DebtDecisionTariffSum,
                    x.Sum,
                    x.Penalty,
                    x.ClaimWork.BaseInfo,
                    Municipality = x.ClaimWork.RealityObject.Municipality.Name,
                    x.ClaimWork.RealityObject.Address,
                    PaymentPlannedPeriod = x.DocumentDate != null ? ((DebtorClaimWork)x.ClaimWork).DebtorType == DebtorType.Individual ?
                                             (DateTime?)x.DocumentDate.ToDateTime().AddDays(individualPaymentPlannedPeriodConfigValue)
                                             : (DateTime?)x.DocumentDate.ToDateTime().AddDays(legalPaymentPlannedPeriodConfigValue) : null,
                    x.NumberPretension
                })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

    }
}
