namespace Bars.Gkh.Overhaul.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Entities;
    using Gkh.Entities.CommonEstateObject;

    public class StructuralElementGroupViewModel : BaseViewModel<StructuralElementGroup>
    {
        public override IDataResult List(IDomainService<StructuralElementGroup> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);
            var cmnEstateObj = loadParam.Filter.GetAs<long>("commonestateobject");

            var queryable = domainService.GetAll()
                .WhereIf(cmnEstateObj > 0, x => x.CommonEstateObject.Id == cmnEstateObj)
                .Filter(loadParam, Container);

            return new ListDataResult(queryable.Order(loadParam).ToList(), queryable.Count());
        }
    }
}