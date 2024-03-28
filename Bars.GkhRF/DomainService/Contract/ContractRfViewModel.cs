namespace Bars.GkhRf.DomainService
{
    using System;
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;

    public class ContractRfViewModel : BaseViewModel<ContractRf>
    {
        public override IDataResult List(IDomainService<ContractRf> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            // зашит 01.01.2014, т.к. договора, заключенные до 2014, заключены по старому бизнес процессу
            // до того как официально начал действовать регоператор
            var firstDayCurrentYear = new DateTime(2014, 1, 1);
            var withArchiveRecs = loadParams.Filter.Get("withArchiveRecs", false);

            var data = Container.Resolve<IDomainService<ViewContractRf>>().GetAll()
                .WhereIf(!withArchiveRecs, x => x.DocumentDate >= firstDayCurrentYear)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNum,
                    x.DocumentDate,
                    x.MunicipalityName,
                    x.ManagingOrganizationName,
                    x.ContractRfObjectsCount,
                    x.SumAreaMkd,
                    x.SumAreaLivingOwned
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            data = data
                .OrderIf(loadParams.Order.Length == 0, true, x => x.MunicipalityName);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}