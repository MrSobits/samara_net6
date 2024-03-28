namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class ActIsolatedPeriodViewModel : BaseViewModel<ActIsolatedPeriod>
    {
        public override IDataResult List(IDomainService<ActIsolatedPeriod> domainService, BaseParams baseParams)
        {
            var loadParam = this.GetLoadParam(baseParams);
            var documentId = baseParams.Params.GetAs<long>("documentId");

            var data = domainService.GetAll()
                .Where(x => x.ActIsolated.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.DateCheck,
                    x.DateStart,
                    x.DateEnd
                })
                .Filter(loadParam, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }
        
        public override IDataResult Get(IDomainService<ActIsolatedPeriod> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var obj = domainService.GetAll().FirstOrDefault(x => x.Id == id);

            return obj != null ? new BaseDataResult(
                new
                    {
                        obj.Id,
                        obj.ActIsolated,
                        obj.DateCheck,
                        obj.DateStart,
                        obj.DateEnd,
                        TimeStart = obj.DateStart.HasValue ? obj.DateStart.Value.ToString("HH:mm") : string.Empty,
                        TimeEnd = obj.DateEnd.HasValue ? obj.DateEnd.Value.ToString("HH:mm") : string.Empty,
                    }) : new BaseDataResult();
        }
    }
}