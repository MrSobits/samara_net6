namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    
    using Entities;
    using Gkh.Utils;

    public class DefaultPlanCollectionInfoViewModel : BaseViewModel<DefaultPlanCollectionInfo>
    {
        public override IDataResult List(IDomainService<DefaultPlanCollectionInfo> domainService, BaseParams baseParams)
        {
            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var periodStart = config.ProgrammPeriodStart;
            var shortPeriod = config.ShortTermProgPeriod;

            if (periodStart == 0 || shortPeriod == 0)
            {
                return new BaseDataResult(false, "Не задан период");
            }

            var data = domainService.GetAll().Where(x => x.Year >= periodStart && periodStart + shortPeriod > x.Year).OrderBy(x => x.Year).ToList();

            return new ListDataResult(data, data.Count());
        }
    }
}