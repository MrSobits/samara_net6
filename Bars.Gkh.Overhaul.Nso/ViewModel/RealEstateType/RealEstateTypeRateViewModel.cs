namespace Bars.Gkh.Overhaul.Nso.ViewModel
{
    using Bars.B4;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using System.Linq;
    using Gkh.Entities.RealEstateType;

    public class RealEstateTypeRateViewModel : BaseViewModel<RealEstateTypeRate>
    {
        public override IDataResult List(IDomainService<RealEstateTypeRate> domainService, BaseParams baseParams)
        {
            //var loadParams = GetLoadParam(baseParams);
            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Year,
                    RealEstateTypeName = x.RealEstateType.Name ?? "Не определено",
                    x.NeedForFunding,
                    x.RateDeficit,
                    x.ReasonableRate,
                    x.SociallyAcceptableRate,
                    x.TotalArea,
                    RetId = (long?) x.RealEstateType.Id
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.RetId);

            return new ListDataResult(data.ToList(), data.Count());
        }
    }
}
