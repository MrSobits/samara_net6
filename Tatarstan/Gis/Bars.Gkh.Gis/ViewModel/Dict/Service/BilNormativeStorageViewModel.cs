namespace Bars.Gkh.Gis.ViewModel.Dict.Service
{
    using System;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Entities.Kp50;
    using Gkh.Domain;

    public class BilNormativStorageViewModel : BaseViewModel<BilNormativStorage>
    {
        public override IDataResult List(IDomainService<BilNormativStorage> domainService, BaseParams baseParams)
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
                    .WhereIf(monthDate.HasValue, x => x.NormativeStartDate.HasValue && x.NormativeStartDate <= monthDate.Value.AddMonths(1).AddDays(-1)
                    && (!x.NormativeEndDate.HasValue || x.NormativeEndDate >= monthDate.Value))
                    .ToList()
                    .Select(x => new
                    {
                        x.Id,
                        Municipality = x.BilService.Schema.LocalSchemaPrefix.IsEmpty()
                            ? x.BilService.Schema.Description
                            : x.BilService.Schema.Description.IsEmpty()
                                ? string.Format("({0})", x.BilService.Schema.LocalSchemaPrefix)
                                : string.Format("{0} ({1})", x.BilService.Schema.Description, x.BilService.Schema.LocalSchemaPrefix),
                        Name = x.NormativeName ?? string.Empty,
                        Value = x.NormativeValue,
                        Measure = x.BilService.MeasureName ?? string.Empty,
                        Description = x.NormativeDescription ?? string.Empty
                    })
                    .AsQueryable()
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                    .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Name)
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