namespace Bars.Gkh.Overhaul.Nso.ViewModel
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.Gkh.Overhaul.Nso.Enum;
    using Overhaul.Entities;

    public class ContributionCollectionViewModel : BaseViewModel<ContributionCollection>
    {
        public override IDataResult List(IDomainService<ContributionCollection> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var roId = baseParams.Params.GetAs<long>("roId");

            var muId = Container.Resolve<IDomainService<RealityObject>>().GetAll()
                .Where(x => x.Id == roId)
                .Select(x => x.Municipality.Id)
                .FirstOrDefault();

            var tempData = domainService.GetAll()
                .WhereIf(roId > 0, x => x.LongTermPrObject.RealityObject.Id == roId)
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Date)
                .Filter(loadParams, Container)
                .ToList();

            var listPaymentSizeMu =
                Container.Resolve<IDomainService<PaymentSizeMuRecord>>().GetAll()
                    .Where(x => x.Municipality.Id == muId)
                    .Where(x => !x.PaymentSizeCr.DateEndPeriod.HasValue || x.PaymentSizeCr.DateEndPeriod >= DateTime.Now.Date)
                    .Select(x => x.PaymentSizeCr.PaymentSize)
                    .Distinct()
                    .ToList();

            var listMinAmount =
                Container.Resolve<IDomainService<MinAmountDecision>>().GetAll()
                    .Where(x => x.RealityObject.Id == roId)
                    .Where(x => x.PropertyOwnerDecisionType == PropertyOwnerDecisionType.SetMinAmount)
                    .Where(x => !x.PaymentDateEnd.HasValue || x.PaymentDateEnd >= DateTime.Now.Date)
                    .Select(x => x.SizeOfPaymentOwners)
                    .Distinct()
                    .ToList();

            if ((tempData.Count == 0) || (listPaymentSizeMu.Count == 0) || (listMinAmount.Count == 0))
            {
                return new ListDataResult();
            }

            var paymentSize = listPaymentSizeMu.Count == 1 ? listPaymentSizeMu.First() : listPaymentSizeMu.Max();

            var sizeOfPaymentOwners = listMinAmount.Count == 1 ? listMinAmount.First() : listMinAmount.Max();

            var result = tempData
                .Select(x =>
                {
                    var sumMinContributions = paymentSize*x.AreaOwnerAccount;
                    var sumSetContributions = sizeOfPaymentOwners*x.AreaOwnerAccount;
                    var differenceSumContributions = sumSetContributions - sumMinContributions;

                    return new
                    {
                        x.Id,
                        x.Date,
                        x.PersonalAccount,
                        MinContributions = paymentSize,
                        x.AreaOwnerAccount,
                        SumMinContributions = sumMinContributions,
                        SetContributions = sizeOfPaymentOwners,
                        SumSetContributions = sumSetContributions,
                        DifferenceSumContributions = differenceSumContributions
                    };
                })
                .ToList();

            return new ListDataResult(result, result.Count);
        }
    }
}