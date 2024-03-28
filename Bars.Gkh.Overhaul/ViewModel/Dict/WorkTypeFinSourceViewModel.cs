namespace Bars.Gkh.Overhaul.ViewModel
{
    using System.Linq;

    using B4;
    using Entities;

    public class WorkTypeFinSourceViewModel : BaseViewModel<WorkTypeFinSource>
    {
        public override IDataResult List(IDomainService<WorkTypeFinSource> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var workId = baseParams.Params.GetAs<long>("workId");

            var data = domainService.GetAll()
                .Where(x => x.Work.Id == workId)
                .Select(x => new
                    {
                        x.Id,
                        x.TypeFinSource
                    })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}