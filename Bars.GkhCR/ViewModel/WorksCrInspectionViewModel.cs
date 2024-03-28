namespace Bars.GkhCr.ViewModel
{
    using System.Linq;
    using B4;
    using Entities;
    using Gkh.Domain;

    public class WorksCrInspectionViewModel : BaseViewModel<WorksCrInspection>
    {
        public override IDataResult List(IDomainService<WorksCrInspection> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var twId = baseParams.Params.GetAsId("twId");

            var query = domainService.GetAll()
                .Where(x => x.TypeWork.Id == twId)
                .Select(x => new
                {
                    x.Id,
                    x.FactDate,
                    x.PlanDate,
                    Official = x.Official.Fio
                })
                .Filter(loadParams, Container);

            return new ListDataResult(query.Order(loadParams).Paging(loadParams).ToList(), query.Count());
        }
    }
}