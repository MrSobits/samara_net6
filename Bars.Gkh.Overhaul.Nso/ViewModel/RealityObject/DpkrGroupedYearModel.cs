namespace Bars.Gkh.Overhaul.Nso.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class DpkrGroupedYearModel : BaseViewModel<DpkrGroupedYear>
    {
        public override IDataResult List(IDomainService<DpkrGroupedYear> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var data =
                domainService.GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        Municipality = x.RealityObject.Municipality.Name,
                        RealityObject = x.RealityObject.Address,
                        x.Year,
                        x.Sum
                    })
                    .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams), data.Count());
        }
    }
}