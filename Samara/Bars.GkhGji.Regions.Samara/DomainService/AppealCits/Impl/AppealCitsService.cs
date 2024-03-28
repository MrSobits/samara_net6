namespace Bars.GkhGji.Regions.Samara.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Samara.Entities;

    /// <summary>
    /// Поскольку в Самарае вместо 1го проверяющег оможет быть много проверяющих то значит добавляем и фильтрацию по Оператору для реестра обращений
    /// </summary>
    public class AppealCitsService : Bars.GkhGji.DomainService.AppealCitsService<ViewAppealCitizens>
    {
        public override IQueryable<ViewAppealCitizens> AddUserFilters(IQueryable<ViewAppealCitizens> query)
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            var serviceAppCitTester = Container.Resolve<IDomainService<AppealCitsTester>>();

            try
            {
                var municipalityList = userManager.GetMunicipalityIds();
                var inspectorsList = userManager.GetInspectorIds();

                return
                    query.WhereIf(
                        municipalityList.Count > 0,
                        x => municipalityList.Contains((long)x.MunicipalityId) || !x.MunicipalityId.HasValue)
                         .WhereIf(
                             inspectorsList.Count > 0,
                             x =>
                             inspectorsList.Contains(x.Executant.Id)
                             || inspectorsList.Contains(x.AppealCits.Surety.Id)
                             || serviceAppCitTester.GetAll().Any(y => y.AppealCits.Id == x.Id && inspectorsList.Contains(y.Tester.Id)));
            }
            finally
            {
                Container.Release(userManager);
                Container.Release(serviceAppCitTester);
            }
        }
    }
}