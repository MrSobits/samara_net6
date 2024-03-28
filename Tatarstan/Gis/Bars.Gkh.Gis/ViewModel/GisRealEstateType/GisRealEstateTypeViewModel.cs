namespace Bars.Gkh.Gis.ViewModel.GisRealEstateType
{
    using System.Linq;
    using B4;
    using Entities.RealEstate.GisRealEstateType;

    public class GisRealEstateTypeViewModel : BaseViewModel<GisRealEstateType>
    {
        public IDomainService<GisRealEstateTypeCommonParam> CommonParamService { get; set; }

        public override IDataResult List(IDomainService<GisRealEstateType> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Group,
                    IndCount = CommonParamService.GetAll().Count(y => y.RealEstateType.Id == x.Id)
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}