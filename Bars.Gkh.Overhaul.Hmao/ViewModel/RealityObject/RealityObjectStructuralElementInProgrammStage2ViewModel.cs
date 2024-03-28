namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using System.Linq;

    using B4;
    using Entities;

    public class RealityObjectStructuralElementInProgrammStage2ViewModel : BaseViewModel<RealityObjectStructuralElementInProgrammStage2>
    {
        public override IDataResult List(IDomainService<RealityObjectStructuralElementInProgrammStage2> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    Municipality = x.RealityObject.Municipality.Name,
                    RealityObject = x.RealityObject.Address,
                    CommonEstateObject = x.CommonEstateObject.Name,
                    x.StructuralElements,
                    x.Year,
                    x.Sum
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}