namespace Bars.Gkh.Overhaul.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Entities;
    using Gkh.Entities.CommonEstateObject;

    public class StructuralElementGroupAttributeViewModel : BaseViewModel<StructuralElementGroupAttribute>
    {
        public override IDataResult List(IDomainService<StructuralElementGroupAttribute> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);
            var group = loadParam.Filter.GetAs<long>("group");

            var queryable = domainService.GetAll()
                .WhereIf(group > 0, x => x.Group.Id == group)
                .Filter(loadParam, Container);

            return new ListDataResult(queryable.Order(loadParam).ToList(), queryable.Count());
        }
    }
}