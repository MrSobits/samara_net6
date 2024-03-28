namespace Bars.Gkh.Regions.Tatarstan.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    public class ConstructionObjectContractViewModel : BaseViewModel<ConstructionObjectContract>
    {
        public override IDataResult List(IDomainService<ConstructionObjectContract> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var objectId = baseParams.Params.GetAsId("objectId");

            var data =
                domain.GetAll()
                      .Where(x => x.ConstructionObject.Id == objectId)
                      .Select(x => new { x.Id, x.State, x.Name, x.Date, x.Number, Contragent = x.Contragent.Name, x.Sum })
                      .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}