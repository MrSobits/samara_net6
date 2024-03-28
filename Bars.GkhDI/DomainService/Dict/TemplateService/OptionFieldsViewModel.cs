namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using B4;

    using Entities;

    public class OptionFieldsViewModel : BaseViewModel<TemplateServiceOptionFields>
    {
        public override IDataResult List(IDomainService<TemplateServiceOptionFields> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var templateServiceId = baseParams.Params.GetAs<long>("templateServiceId");

            var data = domainService.GetAll()
                .Where(x => x.TemplateService.Id == templateServiceId)
                .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.IsHidden
                    })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}