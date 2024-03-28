namespace Bars.Gkh.Repair.ViewModel
{
    using System.Linq;
    using B4;
    using Bars.Gkh.Repair.Entities.RepairControlDate;

    public class RepairControlDateViewModel : BaseViewModel<RepairControlDate>
    {
        public override IDataResult List(IDomainService<RepairControlDate> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var repairPogramId = baseParams.Params.GetAs("repairPogramId", 0);

            var data = domainService.GetAll()
                .Where(x => x.RepairProgram.Id == repairPogramId)
                .Select(x => new
                {
                    x.Id,
                    WorkName = x.Work.Name,
                    x.Date

                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<RepairControlDate> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs("id", 0);
            var value = domainService.GetAll().FirstOrDefault(x => x.Id == id);

            return value != null ? new BaseDataResult(new
            {
                value.Id,
                ProgramName = value.RepairProgram.Name,
                value.Work,
                value.Date
            }) : new BaseDataResult();
        }

    }
}
