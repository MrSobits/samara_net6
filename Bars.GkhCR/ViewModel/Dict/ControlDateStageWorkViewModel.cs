namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;
    using Entities;

    public class ControlDateStageWorkViewModel : BaseViewModel<ControlDateStageWork>
    {
        public override IDataResult List(IDomainService<ControlDateStageWork> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var controlDateId = baseParams.Params.GetAs<long>("controlDateId", 0);

            var data = domainService.GetAll()
                .Where(x => x.ControlDate.Id == controlDateId)
                .Select(x => new
                    {
                        x.Id,
                        StageWork = x.StageWork.Name
                    })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}