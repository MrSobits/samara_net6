namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using Bars.B4;
    using System.Linq;

    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    
    using Gkh.Entities.RealEstateType;
    using Gkh.Utils;

    public class RealEstateTypeRateViewModel : BaseViewModel<RealEstateTypeRate>
    {
        public override IDataResult List(IDomainService<RealEstateTypeRate> domainService, BaseParams baseParams)
        {
            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var periodStart = config.ProgrammPeriodStart;
            var shortTermPeriod = config.ShortTermProgPeriod;

            //var loadParams = GetLoadParam(baseParams);
            var data = domainService.GetAll()
                .Where(x => x.Year < periodStart + shortTermPeriod)
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
