namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using B4;

    using Entities;

    public class TariffForRsoViewModel : BaseViewModel<TariffForRso>
    {
        public override IDataResult List(IDomainService<TariffForRso> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var baseServiceId = baseParams.Params.GetAs<long>("baseServiceId");

            var data = domainService.GetAll()
                .Where(x => x.BaseService.Id == baseServiceId)
                .Select(x => new
                {
                    x.Id,
                    x.DateStart,
                    x.DateEnd,
                    x.NumberNormativeLegalAct,
                    x.Cost,
                    x.DateNormativeLegalAct,
                    x.OrganizationSetTariff
                })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}