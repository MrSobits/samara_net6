namespace Bars.GkhRf.DomainService
{
    using System;
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;

    public class TransferRfViewModel : BaseViewModel<TransferRf>
    {
        public override IDataResult List(IDomainService<TransferRf> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var withArchiveRecs = loadParams.Filter.GetAs<bool>("withArchiveRecs");

            // Не знаю, кто сделал эту фигню, но почему-то раньше запрашивалось через Filter, а по факту передается через Params. Возможно, что где-то в регионах именно через Filter передается 
            // Поэтому осталвяю и тот, и другой варианты

            if (baseParams.Params.ContainsKey("withArchiveRecs"))
            {
                withArchiveRecs = baseParams.Params.GetAs("withArchiveRecs", false);
            }

            // зашит 01.01.2014, т.к. договора, заключенные до 2014, заключены по старому бизнес процессу
            // до того как официально начал действовать регоператор
            var firstDayCurrentYear = new DateTime(2014, 1, 1);

            var data = Container.Resolve<IDomainService<ViewTransferRf>>().GetAll()
                .WhereIf(!withArchiveRecs, x => x.DocumentDate >= firstDayCurrentYear)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNum,
                    x.DocumentDate,
                    x.MunicipalityName,
                    x.ManagingOrganizationName,
                    x.ContractRfObjectsCount
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.MunicipalityName)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}