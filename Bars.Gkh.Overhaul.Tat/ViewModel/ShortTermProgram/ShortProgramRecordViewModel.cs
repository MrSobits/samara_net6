namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using System.Linq;
    using B4;
    using DataResult;
    using Entities;

    public class ShortProgramRecordViewModel : BaseViewModel<ShortProgramRecord>
    {
        public override IDataResult List(IDomainService<ShortProgramRecord> domainService, BaseParams baseParams)
        {
            var objectId = baseParams.Params.GetAs<long>("objectId");

            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Where(x => x.ShortProgramObject.Id == objectId)
                .Select(x => new
                {
                    x.Id,
                    x.Work.TypeWork,
                    WorkName = x.Work.Name,
                    x.Volume,
                    x.Cost
                })
                .Filter(loadParams, Container);

            var sum = data.Any() ? data.Sum(x => x.Cost) : 0;

            return new ListSummaryResult(data.OrderBy(x => x.TypeWork).Order(loadParams).Paging(loadParams).ToList(),
                data.Count(),
                new {Cost = sum});
        }

        public override IDataResult Get(IDomainService<ShortProgramRecord> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            var obj = domainService.GetAll()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    ShortProgramObject = x.ShortProgramObject.Id,
                    Stage1 = (long?)x.Stage1.Id,
                    Work = x.Work.Id,
                    WorkName = x.Work.Name,
                    x.Work.TypeWork,
                    x.Cost,
                    x.ServiceCost,
                    x.Volume,
                    x.TotalCost,
                    x.TypeDpkrRecord
                })
                .FirstOrDefault();

            return new BaseDataResult(obj);
        }
    }
}