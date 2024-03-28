namespace Bars.Gkh.Gis.ViewModel.GisRealEstateType
{
    using System.Linq;
    using B4;
    using CommonParams;
    using Entities.RealEstate.GisRealEstateType;
    using Enum;

    public class GisRealEstateTypeCommonParamViewModel : BaseViewModel<GisRealEstateTypeCommonParam>
    {
        public override IDataResult List(IDomainService<GisRealEstateTypeCommonParam> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var realEstateTypeId = baseParams.Params.GetAs<long>("RealEstateTypeId");

            var commonDateParams = Container.ResolveAll<IGisCommonParam>().Where(x => x.CommonParamType == CommonParamType.Date)
                .Select(x => new
                {
                    x.Code
                })
                .ToDictionary(x => x.Code);

            var data = domainService.GetAll()
                .Where(x => x.RealEstateType.Id == realEstateTypeId)
                .Select(x => new
                {
                    x.Id,
                    x.CommonParamCode,
                    x.PrecisionValue,
                    x.Min,
                    x.Max
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }     
    }
}