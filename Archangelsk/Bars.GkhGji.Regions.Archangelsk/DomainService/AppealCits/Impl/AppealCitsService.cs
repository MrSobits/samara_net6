namespace Bars.GkhGji.Regions.Archangelsk.DomainService
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DomainService.BaseParams;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Archangelsk.Entities;

    public class AppealCitsService : Bars.GkhGji.DomainService.AppealCitsService
    {
        public override IQueryable<ViewAppealCitizens> AddUserFilters(IQueryable<ViewAppealCitizens> query)
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            var serviceAppealCitsExecutant = Container.Resolve<IDomainService<AppealCitsExecutant>>();

            try
            {
                var municipalityList = userManager.GetMunicipalityIds();
                var inspectorsList = userManager.GetInspectorIds();

                return query.WhereIf(
                        municipalityList.Count > 0,
                        x => municipalityList.Contains((long)x.MunicipalityId) || !x.MunicipalityId.HasValue)
                         .WhereIf(
                             inspectorsList.Count > 0,
                             x =>
                             serviceAppealCitsExecutant.GetAll()
                                                       .Any(
                                                           y =>
                                                           inspectorsList.Contains(y.Author.Id)
                                                           || inspectorsList.Contains(y.Executant.Id)));
            }
            finally
            {

                Container.Release(userManager);
                Container.Release(serviceAppealCitsExecutant);
            }
        }
    }
}