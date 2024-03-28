namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using System.Linq;
    using B4;

    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    
    using Entities;
    using Gkh.Utils;

    public class ShortProgramDeficitViewModel : BaseViewModel<ShortProgramDifitsit>
    {
        public override IDataResult List(IDomainService<ShortProgramDifitsit> domainService, BaseParams baseParams)
        {
            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var startYear = config.ProgrammPeriodStart;
            var shortProgramYear = config.ShortTermProgPeriod;

            var data = domainService.GetAll()
                .Where(x => x.Version.IsMain)
                .Select(x => new
                {
                    VersionId = x.Version.Id,
                    x.Municipality.Id,
                    x.Municipality.Name,
                    Deficit = x.Difitsit,
                    x.Year,
                    x.BudgetRegionShare
                })
                .AsEnumerable()
                .GroupBy(x => new {x.Id, x.Name, x.VersionId})
                .Select(x => new
                {
                    x.Key.Id,
                    Municipality = x.Key.Name,
                    x.Key.VersionId,
                    Data =
                        x.Select(y =>
                            new
                            {
                                y.Deficit,
                                y.Year,
                                Share = y.BudgetRegionShare
                            }).ToList()
                })
                .ToList();

            return new BaseDataResult(new {data = data.ToList(), startYear, shortProgramYear});
        }
    }
}
