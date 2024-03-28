namespace Bars.Gkh.Overhaul.Nso.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class RealityObjectStructuralElementInProgrammViewModel : BaseViewModel<RealityObjectStructuralElementInProgramm>
    {
        public override IDataResult List(IDomainService<RealityObjectStructuralElementInProgramm> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    Municipality = x.StructuralElement.RealityObject.Municipality.Name,
                    RealityObject = x.StructuralElement.RealityObject.Address,
                    CommonEstateObject = x.StructuralElement.StructuralElement.Group.CommonEstateObject.Name,
                    StructuralElement = x.StructuralElement.StructuralElement.Name,
                    UnitMeasure = x.StructuralElement.StructuralElement.UnitMeasure.Name,
                    x.StructuralElement.Volume,
                    x.Year,
                    x.Sum,
                    x.ServiceCost
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}