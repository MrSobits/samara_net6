namespace Bars.Gkh.Overhaul.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Utils;

    public class StructuralElementFeatureViolViewModel : BaseViewModel<StructuralElementFeatureViol>
    {
        public override IDataResult List(IDomainService<StructuralElementFeatureViol> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var element = loadParam.Filter.GetAs<long>("element");

            return domainService.GetAll()
                .WhereIf(element > 0, x => x.StructuralElement.Id == element)
                .Select(x => new
                {
                    x.Id,
                    StructuralElement = x.StructuralElement.Id,
                    x.FeatureViol.Name,
                    x.FeatureViol.Code,
                    x.FeatureViol.GkhReformCode
                })
                .ToListDataResult(loadParam);
        }
    }
}