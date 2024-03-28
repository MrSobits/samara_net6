namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;
    using Entities;

    public class ControlDateViewModel : BaseViewModel<ControlDate>
    {
        public override IDataResult List(IDomainService<ControlDate> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var programCrId = baseParams.Params.GetAs<long>("programCrId", 0);

            var data = domainService.GetAll()
                .Where(x => x.ProgramCr.Id == programCrId)
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

        public override IDataResult Get(IDomainService<ControlDate> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id", 0);
            var value = domainService.GetAll().FirstOrDefault(x => x.Id == id);

            return  value != null ? new BaseDataResult(new
                                          {
                                              value.Id,
                                              ProgramName = value.ProgramCr.Name,
                                              value.Work,
                                              value.Date
                                          }) : new BaseDataResult();
        }
    }
}