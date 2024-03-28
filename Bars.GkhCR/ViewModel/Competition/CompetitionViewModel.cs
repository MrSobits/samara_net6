namespace Bars.GkhCr.DomainService
{
    using System.Linq;

    using B4;
    using Entities;
    using Gkh.Domain;

    public class CompetitionViewModel : BaseViewModel<Competition>
    {
        public override IDataResult List(IDomainService<Competition> domainService, BaseParams baseParams)
        {
            var service = Container.Resolve<ICompetitionService>();

            try
            {
                var totalCount = 0;
                var list = service.GetList(baseParams, true, ref totalCount);
                return new ListDataResult(list, totalCount);
            }
            finally 
            {
                Container.Release(service);
            }
        }

        public override IDataResult Get(IDomainService<Competition> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            var obj =
                domainService.GetAll()
                    .Where(x => x.Id == id)
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        x.NotifDate,
                        x.NotifNumber,
                        x.ReviewDate,
                        x.ReviewPlace,
                        x.File,
                        ReviewTime = x.ReviewTime.HasValue
                            ? x.ReviewTime.Value.ToString("HH:mm")
                            : "",
                        x.ExecutionDate,
                        x.ExecutionPlace,
                        ExecutionTime =
                            x.ExecutionTime.HasValue
                                ? x.ExecutionTime.Value.ToString("HH:mm")
                                : ""
                    })
                    .FirstOrDefault();

            return new BaseDataResult(obj);
        }
    }
}