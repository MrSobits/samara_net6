namespace Bars.Gkh.Gis.ViewModel.GisRealEstateType
{
    using System.Linq;
    using B4;
    using Entities.RealEstate.GisRealEstateType;

    public class GisRealEstateTypeIndicatorViewModel : BaseViewModel<GisRealEstateTypeIndicator>
    {
        public IDomainService<GisRealEstateTypeCommonParam> CommonParamService { get; set; }

        public override IDataResult List(IDomainService<GisRealEstateTypeIndicator> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var realEstateTypeId = baseParams.Params.GetAs<int>("RealEstateTypeId");

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.RealEstateIndicator,
                    x.RealEstateType,
                    x.PrecisionValue,
                    x.Max,
                    x.Min
                })
                .Filter(loadParams, Container)
                .Where(x => realEstateTypeId == 0 || x.RealEstateType.Id == realEstateTypeId);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}