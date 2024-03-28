namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    public class HeatSeasonDomainService : BaseDomainService<HeatSeason>
    {
        public override IQueryable<HeatSeason> GetAll()
        {
            var userManager = Container.Resolve<IGkhUserManager>();

            var contragentList = userManager.GetContragentIds();
            var municipalityList = userManager.GetMunicipalityIds();

            if (contragentList.Count > 0)
            {
                // тут надо наложить фильтрацию на Контаргентов через управляющие компании
                var realObjListQuery = Container.Resolve<IDomainService<ViewRealityObjectManOrgContract>>().GetAll()
                         .Where(x => (!x.StartDate.HasValue || x.StartDate.Value <= DateTime.Now) && (!x.EndDate.HasValue || x.EndDate.Value >= DateTime.Now))
                         .WhereIf(contragentList.Count > 0, x => contragentList.Contains(x.ManagingOrganization.Contragent.Id))
                         .WhereIf(municipalityList.Count > 0, x => municipalityList.Contains(x.ManagingOrganization.Contragent.Municipality.Id))
                         .Select(x => x.RealityObject.Id);

                return base.GetAll()
                        .WhereIf(realObjListQuery.Count() > 0, x => realObjListQuery.Contains(x.RealityObject.Id))
                        .WhereIf(municipalityList.Count > 0, x => municipalityList.Contains(x.RealityObject.Municipality.Id));
            }

            return base.GetAll()
                .WhereIf(municipalityList.Count > 0, x => municipalityList.Contains(x.RealityObject.Municipality.Id));
        }
    }
}