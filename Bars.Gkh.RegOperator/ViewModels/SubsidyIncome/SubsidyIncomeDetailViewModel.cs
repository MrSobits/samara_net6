namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Entities;
    using Gkh.Domain;
    using B4.Utils;
    using Distribution;
    using Enums;

    public class SubsidyIncomeDetailViewModel : BaseViewModel<SubsidyIncomeDetail>
    {
        public override IDataResult List(IDomainService<SubsidyIncomeDetail> domainService, BaseParams baseParams)
        {
            var payAccDomain = Container.ResolveDomain<RealityObjectPaymentAccount>();
            var distrs = Container.ResolveAll<IDistribution>();

            try
            {
                var loadParam = GetLoadParam(baseParams);

                var subsidyIncomeId = baseParams.Params.GetAsId("subsidyIncomeId");

                var query = domainService.GetAll()
                    .Where(x => x.SubsidyIncome.Id == subsidyIncomeId);

                var payAccNums = payAccDomain.GetAll()
                    .Where(x => query.Any(y => y.RealityObject.Id == x.RealityObject.Id))
                    .Select(x => new
                    {
                        x.RealityObject.Id,
                        x.AccountNumber
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.AccountNumber).First());

                var distrsInfo = distrs
                    .Where(x => x.Code.Contains("Subsidy"))
                    .Select(x => new
                    {
                        x.Name,
                        x.Code
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Code)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Name).First());

                var data = domainService.GetAll()
                    .Where(x => x.SubsidyIncome.Id == subsidyIncomeId)
                    .Select(x => new
                    {
                        x.Id,
                        x.RealObjId,
                        x.RealObjAddress,
                        Municipality = x.RealityObject.Municipality.Name,
                        RoId = (long?) x.RealityObject.Id,
                        x.RealityObject.Address,
                        x.RealityObject,
                        x.TypeSubsidyDistr,
                        x.DateReceipt,
                        x.SubsidyIncome,
                        x.Sum,
                        x.IsConfirmed,
                        x.SubsidyIncome.DistributeState
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.RealObjId,
                        x.RealObjAddress,
                        x.Municipality,
                        x.Address,
                        x.RealityObject,
                        x.TypeSubsidyDistr,
                        x.DateReceipt,
                        x.SubsidyIncome,
                        x.Sum,
                        PayAccNum = payAccNums.Get(x.RoId ?? 0),
                        SubsidyDistrName = distrsInfo.Get(x.TypeSubsidyDistr),
                        IsDefined = x.RealityObject != null,
                        x.IsConfirmed,
                        ConfirmStatus = x.SubsidyIncome.DistributeState == DistributionState.Deleted ? "Удален" :
                                      x.IsConfirmed ? "Подтвержден" : "Не подтвержден"
                    })
                    .AsQueryable()
                    .Filter(loadParam, Container);

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToArray(), data.Count());
            }
            finally
            {
                Container.Release(payAccDomain);
                Container.Release(distrs);
            }
        }
    }
}
