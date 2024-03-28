namespace Bars.Gkh.Gis.ViewModel.Dict.Service
{
    using System;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Entities.Kp50;
    using Gkh.Domain;

    public class BilTarifStorageViewModel : BaseViewModel<BilTarifStorage>
    {
        public override IDataResult List(IDomainService<BilTarifStorage> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var serviceId = loadParams.Filter.GetAsId("serviceId");
            var monthDate = loadParams.Filter.GetAs<DateTime?>("monthDate");

            var bilServiceDictDomain = Container.ResolveDomain<BilServiceDictionary>();

            try
            {
                var data = domainService.GetAll()
                    .Where(x => bilServiceDictDomain.GetAll()
                        .Where(y => y.Service.Id == serviceId)
                        .Any(y => y.Id == x.BilService.Id))
                    .WhereIf(monthDate.HasValue, x => x.TarifStartDate.HasValue && x.TarifStartDate<=monthDate.Value.AddMonths(1).AddDays(-1)
                    && (!x.TarifEndDate.HasValue || x.TarifEndDate >= monthDate.Value))
                    .Select(x => new
                    {
                        x.Id,
                        Municipality = x.RealityObject.Municipality.Name,
                        x.RealityObject.Address,
                        x.BilService.ServiceName,
                        x.BilService.ServiceTypeName,
                        x.SupplierName,
                        x.TarifValue,
                        x.TarifTypeName,
                        x.FormulaName,
                        x.LsCount
                    })
                    .AsQueryable()
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                    .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Address)
                    .OrderThenIf(loadParams.Order.Length == 0, true, x => x.ServiceName)
                    .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                Container.Release(bilServiceDictDomain);
            }
        }
    }
}