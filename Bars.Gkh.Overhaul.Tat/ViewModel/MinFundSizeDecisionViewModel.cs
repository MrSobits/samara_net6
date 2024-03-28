namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Overhaul.Entities;
    using Overhaul.Enum;

    public class MinFundSizeDecisionViewModel : BaseViewModel<MinFundSizeDecision>
    {
        public override IDataResult Get(IDomainService<MinFundSizeDecision> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params.GetAs<long>("id"));

            var now = DateTime.Now.Date;

            var paymentSize = Container.Resolve<IDomainService<PaymentSizeMuRecord>>().GetAll()
                .Where(x => x.Municipality.Id == obj.RealityObject.Municipality.Id)
                .Where(x => x.PaymentSizeCr.TypeIndicator == TypeIndicator.MinSizePercentOfCostRestoration)
                .Where(x => !x.PaymentSizeCr.DateStartPeriod.HasValue || x.PaymentSizeCr.DateStartPeriod.Value <= now)
                .Where(x => !x.PaymentSizeCr.DateEndPeriod.HasValue || x.PaymentSizeCr.DateEndPeriod.Value >= now)
                .OrderByDescending(x => x.PaymentSizeCr.DateStartPeriod)
                .Select(x => (int?) x.PaymentSizeCr.PaymentSize)
                .FirstOrDefault()
                .GetValueOrDefault(0);

            return new BaseDataResult(new
            {
                obj.Id,
                obj.DateEnd,
                obj.DateStart,
                obj.MethodFormFund,
                obj.MoOrganizationForm,
                obj.MonthlyPayment,
                obj.OwnerMinFundSize,
                obj.PropertyOwnerDecisionType,
                obj.PropertyOwnerProtocol,
                RealityObject = obj.RealityObject.Id,
                SubjectMinFundSize = paymentSize
            });
        }
    }
}