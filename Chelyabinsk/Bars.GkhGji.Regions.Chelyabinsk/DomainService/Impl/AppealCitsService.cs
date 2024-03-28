namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;

    public class AppealCitsService : BaseChelyabinsk.DomainService.Impl.AppealCitsService
    {
        public IDomainService<AppealCits> AppealCitsDomain { get; set; }

        public IDomainService<AppealCitsAnswer> AppealCitsAnswerDomain { get; set; }
        public IDomainService<AppealCitsExecutant> AppealCitsExecutantDomain { get; set; }

        protected override IQueryable<ViewAppealCitizensBaseChelyabinsk> ApplyFilters(IQueryable<ViewAppealCitizensBaseChelyabinsk> query, BaseParams baseParams)
        {
            var isShowOnlyFromEais = baseParams.Params.GetAs("showOnlyFromEais", false);
            if (isShowOnlyFromEais)
            {
                query = query.Where(x => x.AppealCits.AppealUid.HasValue);
            }
            return base.ApplyFilters(query, baseParams);
        }
    }
}
