namespace Bars.Gkh.Overhaul.Regions.Saha.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Regions.Saha.Entities.RealEstateType;
    using Bars.Gkh.Utils;

    public class SahaRealEstateTypeRateViewModel : BaseViewModel<SahaRealEstateTypeRate>
    {
        public override IDataResult List(IDomainService<SahaRealEstateTypeRate> domainService, BaseParams baseParams)
        {
            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var periodStart = config.ProgrammPeriodStart;
            var shortTermPeriod = config.ShortTermProgPeriod;

            var data = domainService.GetAll()
                .Where(x => x.Year < periodStart + shortTermPeriod)
                .Select(x => new
                {
                    x.Id,
                    x.Year,
                    RealEstateTypeName = x.RealEstateType.Name ?? "Не определено",
                    x.NeedForFunding,
                    x.RateDeficit,
                    x.RateDeficitNotLivingArea,
                    x.ReasonableRate,
                    x.ReasonableRateNotLivingArea,
                    x.SociallyAcceptableRate,
                    x.SociallyAcceptableRateNotLivingArea,
                    x.TotalArea,
                    x.TotalNotLivingArea,
                    RetId = (long?)x.RealEstateType.Id
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.RetId);

            return new ListDataResult(data.ToList(), data.Count());
        }
    }
}