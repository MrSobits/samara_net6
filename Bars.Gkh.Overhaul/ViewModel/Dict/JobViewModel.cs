namespace Bars.Gkh.Overhaul.ViewModel
{
    using System.Linq;

    using B4;
    using Entities;

    public class JobViewModel : BaseViewModel<Job>
    {
        public override IDataResult List(IDomainService<Job> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            var queryable = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    Work = x.Work.Name,
                    UnitMeasure = x.UnitMeasure.Name
                })
                .Filter(loadParam, Container);

            return new ListDataResult(queryable.Order(loadParam).Paging(loadParam).ToList(), queryable.Count());
        }
    }
}