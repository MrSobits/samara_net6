namespace Bars.GkhCr.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhCr.Entities;

    public class StageWorkCrViewModel : BaseViewModel<StageWorkCr>
    {
        public override IDataResult List(IDomainService<StageWorkCr> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                    {
                        x.Id,
                        x.Code,
                        x.Name
                    })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}