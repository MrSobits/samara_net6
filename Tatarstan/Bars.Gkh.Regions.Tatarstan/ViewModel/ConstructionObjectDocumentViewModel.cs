namespace Bars.Gkh.Regions.Tatarstan.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    public class ConstructionObjectDocumentViewModel : BaseViewModel<ConstructionObjectDocument>
    {
        public override IDataResult List(IDomainService<ConstructionObjectDocument> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var objectId = baseParams.Params.GetAsId("objectId");

            var data =
                domain.GetAll()
                      .Where(x => x.ConstructionObject.Id == objectId)
                      .Select(x => new { x.Id, x.Type, x.Name, x.Date, Contragent = x.Contragent.Name, x.File })
                      .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}