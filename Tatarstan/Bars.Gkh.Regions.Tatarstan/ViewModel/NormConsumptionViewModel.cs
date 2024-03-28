namespace Bars.Gkh.Regions.Tatarstan.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Regions.Tatarstan.Entities.NormConsumption;

    public class NormConsumptionViewModel : BaseViewModel<NormConsumption>
    {
        public override IDataResult List(IDomainService<NormConsumption> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var muId = baseParams.Params.GetAsId("muId");
            var periodId = baseParams.Params.GetAsId("periodId");

            var data = domain.GetAll()
                .Where(x => x.Municipality.Id == muId)
                .Where(x => x.Period.Id == periodId)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.Municipality.Name,
                    Period = x.Period.Name,
                    x.Type,
                    x.State
                })
                .Filter(loadParams, this.Container)
                .Order(loadParams);

            var totalCount = data.Count();

            return new ListDataResult(data.Paging(loadParams).ToList(), totalCount);
        }
    }
}