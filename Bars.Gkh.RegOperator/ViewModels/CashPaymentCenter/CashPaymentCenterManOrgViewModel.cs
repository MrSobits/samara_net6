namespace Bars.Gkh.RegOperator.ViewModel
{
    using System.Linq;
    using B4.DataAccess;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities;
    using Gkh.Entities;

    public class CashPaymentCenterManOrgViewModel : BaseViewModel<CashPaymentCenterManOrg>
    {
        public override IDataResult Get(IDomainService<CashPaymentCenterManOrg> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            return new BaseDataResult(
                domainService.GetAll()
                    .Where(x => x.Id == id)
                    .Select(x => new
                    {
                        x.Id,
                        x.CashPaymentCenter,
                        x.NumberContract,
                        x.DateContract,
                        x.DateStart,
                        x.DateEnd,
                        ManOrg = new ManagingOrganization
                        {
                            Id = x.ManOrg.Id,
                            ContragentName = x.ManOrg.Contragent.Name
                        }
                    })
                    .FirstOrDefault());
        }

        public override IDataResult List(IDomainService<CashPaymentCenterManOrg> domain, BaseParams baseParams)
        {
            var cashPaymCenterManOrgRoDomain = Container.ResolveDomain<CashPaymentCenterManOrgRo>();

            try
            {
                var loadParams = GetLoadParam(baseParams);
                var cashPaymentCenterId = loadParams.Filter.GetAs<long>("cashPaymentCenterId");

                if (cashPaymentCenterId == 0)
                {
                    return new BaseDataResult(false, "Не удалось получить РКЦ");
                }

                var data = domain.GetAll()
                     .Where(x => x.CashPaymentCenter.Id == cashPaymentCenterId)
                     .Select(x => new
                     {
                         x.Id,
                         Municipality = x.ManOrg.Contragent.Municipality.Name,
                         x.NumberContract,
                         x.DateContract,
                         ManOrg = x.ManOrg.Id,
                         ManOrgName = x.ManOrg.Contragent.Name,
                         x.ManOrg.Contragent.Inn,
                         HousesCount = cashPaymCenterManOrgRoDomain.GetAll()
                            .Where(y => y.CashPaymentCenterManOrg.Id == x.Id)
                            .Select(y => y.RealityObject.Id)
                            .Distinct()
                            .Count()

                     })
                     .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                     .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                Container.Release(cashPaymCenterManOrgRoDomain);
            }
        }
    }
}