namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using Bars.B4;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using System.Linq;
    using Gkh.Entities.RealEstateType;

    public class RealEstateTypeRateViewModel : BaseViewModel<RealEstateTypeRate>
    {
        public override IDataResult List(IDomainService<RealEstateTypeRate> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            if (domainService.GetAll().Count(x => x.RealEstateType == null) == 0)
            {
                domainService.Save(new RealEstateTypeRate { RealEstateType = null });
            }
            
            var data = domainService.GetAll()
                .Filter(loadParams, this.Container)
                .Select(x => new
                {
                    x.Id,
                    RealEstateTypeName = ((long?)x.RealEstateType.Id) > 0 ? x.RealEstateType.Name : "Не определено",
                    x.NeedForFunding,
                    x.RateDeficit,
                    x.ReasonableRate,
                    x.SociallyAcceptableRate,
                    x.TotalArea
                });

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
