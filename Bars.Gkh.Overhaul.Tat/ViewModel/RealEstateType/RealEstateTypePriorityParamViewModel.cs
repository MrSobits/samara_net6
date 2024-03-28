namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using System.Linq;
    using B4;

    using Bars.Gkh.Overhaul.Tat.Entities;
    using Gkh.Entities.RealEstateType;

    public class RealEstateTypePriorityParamViewModel : BaseViewModel<RealEstateTypePriorityParam>
    {
        public override IDataResult List(IDomainService<RealEstateTypePriorityParam> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var realEstateTypeId = baseParams.Params.GetAs<long>("RealEstateTypeId");

            var data = domainService.GetAll()
                .Where(x => x.RealEstateType.Id == realEstateTypeId)
                .Filter(loadParams, Container)
                .Select(x => new
                {
                    x.Id,
                    RealEstateType = x.RealEstateType.Id,
                    x.Code,
                    x.Weight
                });

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}