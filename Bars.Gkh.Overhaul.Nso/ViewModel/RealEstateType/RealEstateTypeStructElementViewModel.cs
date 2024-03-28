namespace Bars.Gkh.Overhaul.Nso.ViewModel
{
    using System.Linq;
    using B4;
    using Gkh.Entities.RealEstateType;

    public class RealEstateTypeStructElementViewModel : BaseViewModel<RealEstateTypeStructElement>
    {
        public override IDataResult List(IDomainService<RealEstateTypeStructElement> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var realEstateTypeId = baseParams.Params.GetAs<long>("RealEstateTypeId");

            var data = domainService.GetAll()
                .Where(x => x.RealEstateType.Id == realEstateTypeId)
                .Filter(loadParams, this.Container)
                .Select(x => new
                {
                    x.Id,
                    x.RealEstateType,
                    x.StructuralElement,
                    x.Exists
                });

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
