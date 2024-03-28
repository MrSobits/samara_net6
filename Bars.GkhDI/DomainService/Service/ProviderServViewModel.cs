namespace Bars.GkhDi.DomainService
{
    using System;
    using System.Linq;
    using B4;

    using Entities;

    public class ProviderServViewModel : BaseViewModel<ProviderService>
    {
        public override IDataResult List(IDomainService<ProviderService> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var baseServiceId = baseParams.Params.GetAs<long>("baseServiceId");

            var data = domainService.GetAll()
                .Where(x => x.BaseService.Id == baseServiceId)
                .Select(x => new
                {
                    x.Id,
                    x.Provider.Name,
                    DateStartContract = x.DateStartContract <= DateTime.MinValue ? null : x.DateStartContract,
                    x.Description,
                    IsActive = x.IsActive ? "Да" : "Нет"
                })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}