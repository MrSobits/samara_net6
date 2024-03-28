namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using Bars.B4;
    using System.Linq;
    using Gkh.Entities.RealEstateType;
    using Overhaul.Entities;

    public class RealEstateTypeCommonParamViewModel : BaseViewModel<RealEstateTypeCommonParam>
    {
        public override IDataResult List(IDomainService<RealEstateTypeCommonParam> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var realEstateTypeId = baseParams.Params.GetAs<long>("RealEstateTypeId");

            var data = domainService.GetAll()
                .Where(x => x.RealEstateType.Id == realEstateTypeId).
                Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
