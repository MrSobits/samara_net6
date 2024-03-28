namespace Bars.Gkh.FormatDataExport.ProxySelectors.SystemSelectors
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.IoC;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    using NHibernate.Linq;

    /// <summary>
    /// Сервис получения актуальных управляющих организаций по домам
    /// </summary>
    public class ActualManOrgByRealityObjectSelectorService : BaseProxySelectorService<ActualManOrgByRealityObject>
    {
        /// <inheritdoc />
        protected override bool CanGetFullData()
        {
            return true;
        }

        /// <inheritdoc />
        protected override IDictionary<long, ActualManOrgByRealityObject> GetCache()
        {
            var realityObjectManOrgService = this.Container.Resolve<IRealityObjectManOrgService>();

            using (this.Container.Using(realityObjectManOrgService))
            {
                var roQuery = this.FilterService.GetFiltredQuery<RealityObject>();

                return realityObjectManOrgService.GetActualManagingOrganizationsQuery(this.FilterService.ExportDate)
                    .Where(x => roQuery.Any(y => y.Id == x.RealityObject.Id))
                    .Fetch(x => x.ManOrgContract)
                    .ThenFetch(x => x.ManagingOrganization)
                    .ThenFetch(x => x.Contragent)
                    .Select(x => new
                    {
                        x.RealityObject.Id,
                        x.ManOrgContract.StartDate,
                        x.ManOrgContract.ManagingOrganization,
                        x.ManOrgContract.ManagingOrganization.Contragent
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key,
                        x => ActualManOrgByRealityObject.FromBase(x.OrderByDescending(y => y.StartDate)
                            .FirstOrDefault()?.ManagingOrganization));
            }
        }
    }

    public class ActualManOrgByRealityObject : ManagingOrganization
    {
        public static ActualManOrgByRealityObject FromBase(ManagingOrganization mo)
        {
            var result = new ActualManOrgByRealityObject();

            foreach (var prop in mo.GetType().GetProperties())
                typeof(ActualManOrgByRealityObject)
                    .GetProperty(prop.Name)
                    .SetValue(result, prop.GetValue(mo, null), null);

            return result;
        }
    }
}