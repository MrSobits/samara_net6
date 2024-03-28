namespace Bars.Gkh.ViewModel.EfficiencyRating
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Entities.EfficiencyRating;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Представление для <see cref="ManagingOrganizationEfficiencyRating"/>
    /// </summary>
    public class ManagingOrganizationEfficiencyRatingViewModel : BaseViewModel<ManagingOrganizationEfficiencyRating>
    {
        /// <summary>
        /// Домен-сервис <see cref="ManagingOrganization"/>
        /// </summary>
        public IDomainService<ManagingOrganization> ManaingOrganizationDomainService { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="EfficiencyRatingPeriod"/>
        /// </summary>
        public IDomainService<EfficiencyRatingPeriod> EfficiencyRatingPeriodDomainService { get; set; }

        /// <summary>Получить список</summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат получения списка</returns>
        public override IDataResult List(IDomainService<ManagingOrganizationEfficiencyRating> domainService, BaseParams baseParams)
        {
            var periodId = baseParams.Params.GetAsId("periodId");

            var periodIdsAll = baseParams.Params.ContainsKey("periodIds") && baseParams.Params.GetAs("periodIds", string.Empty).ToLower() == "all";
            var periodIds = baseParams.Params.ContainsKey("periodIds") && !periodIdsAll ? baseParams.Params.GetAs<long[]>("periodIds") : null;

            var municipalityIdsmunicipalityIds = baseParams.Params.GetAs<long[]>("municipalityIds");
            var loadParams = baseParams.GetLoadParam();

            IQueryable<ManagingOrganizationEfficiencyRating> query = null;
            if (periodIdsAll || periodIds.IsNotEmpty())
            {
                if (periodIdsAll)
                {
                    periodIds = this.EfficiencyRatingPeriodDomainService.GetAll().Select(x => x.Id).ToArray();
                }

                query = domainService.GetAll().WhereContains(x => x.Period.Id, periodIds);
            }

            var dataQuery = this.ManaingOrganizationDomainService.GetAll()
                .Where(x => x.ActivityGroundsTermination == GroundsTermination.NotSet)
                .WhereIf(municipalityIdsmunicipalityIds.IsNotEmpty(), x => municipalityIdsmunicipalityIds.Contains(x.Contragent.Municipality.Id))
                .WhereIf(query != null, x => query.Count(y => y.ManagingOrganization.Id == x.Id) == periodIds.Length)
                .Select(x => new
                {
                    x.Id,
                    x.Contragent.Municipality,
                    ManagingOrganization = x.Contragent.Name,
                    x.Contragent.Inn,
                    x.Contragent.Kpp,
                    x.Contragent.Ogrn
                })
                .Filter(loadParams, this.Container);

            var count = dataQuery.Count();

            var dataList = dataQuery.Order(loadParams).Paging(loadParams).ToList();
            var manorgIds = dataList.Select(x => x.Id).ToArray();

            var efList = domainService.GetAll()
                .Where(x => manorgIds.Contains(x.ManagingOrganization.Id))
                .Where(x => x.Period.Id == periodId)
                .Select(x => new { x.Id, ManorgId = x.ManagingOrganization.Id, x.Dynamics, x.Rating, x.Period })
                .ToDictionary(x => x.ManorgId);

            var result = dataList
                .Select(x =>
                {
                    var efData = efList.Get(x.Id);
                    return new
                    {
                        x.Id,
                        ObjectId = efData?.Id,
                        efData?.Period,

                        x.Municipality,
                        x.ManagingOrganization,
                        x.Inn,
                        x.Kpp,
                        x.Ogrn,

                        efData?.Dynamics,
                        efData?.Rating
                    };
                })
                .ToList();

            return new ListDataResult(result, count);
        }
    }
}