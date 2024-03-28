namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using B4;
    using Entities;
    using System.Linq;
    using Gkh.Entities.Dicts;
    using Gkh.Entities.RealEstateType;
    using Overhaul.Entities;

    public class RealEstateTypeViewModel : BaseViewModel<RealEstateType>
    {
        public IDomainService<RealEstateTypeCommonParam> CommonParamService { get; set; }

        public override IDataResult List(IDomainService<RealEstateType> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    IndCount = CommonParamService.GetAll().Count(y => y.RealEstateType.Id == x.Id)
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}