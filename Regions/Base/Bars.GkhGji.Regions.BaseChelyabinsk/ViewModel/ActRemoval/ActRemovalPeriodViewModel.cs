namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.ActRemoval
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;

    public class ActRemovalPeriodViewModel : BaseViewModel<ActRemovalPeriod>
    {
        public override IDataResult List(IDomainService<ActRemovalPeriod> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("documentId")
                                 ? baseParams.Params["documentId"].ToLong()
                                 : 0;

            var data = domainService.GetAll()
                .Where(x => x.ActRemoval.Id == documentId)
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

        public override IDataResult Get(IDomainService<ActRemovalPeriod> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var obj = domainService.GetAll().FirstOrDefault(x => x.Id == id);

            return obj != null ? new BaseDataResult(
                new
                    {
                        obj.Id,
                        obj.ActRemoval,
                        obj.DateCheck,
                        obj.DateStart,
                        obj.DateEnd,
                        TimeStart = obj.DateStart.HasValue ? obj.DateStart.Value.ToShortTimeString() : string.Empty,
                        TimeEnd = obj.DateEnd.HasValue ? obj.DateEnd.Value.ToShortTimeString() : string.Empty,
                    }) : new BaseDataResult();
        }
    }
}