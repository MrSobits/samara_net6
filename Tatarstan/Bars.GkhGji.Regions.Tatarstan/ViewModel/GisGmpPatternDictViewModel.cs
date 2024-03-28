namespace Bars.GkhGji.Regions.Tatarstan.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    public class GisGmpPatternDictViewModel : BaseViewModel<GisGmpPatternDict>
    {
        public override IDataResult List(IDomainService<GisGmpPatternDict> domainService, BaseParams baseParams)
        {
            var useRelevanceFilter = baseParams.Params.GetAs<bool>("relevanceFilter");

            return domainService.GetAll()
                .WhereIf(useRelevanceFilter, w => w.Relevance)
                .Select(x => new
                    {
                        x.Id,
                        x.PatternName,
                        x.PatternCode,
                        x.Relevance
                    })
                .ToListDataResult(this.GetLoadParam(baseParams), this.Container);
        }
    }
}