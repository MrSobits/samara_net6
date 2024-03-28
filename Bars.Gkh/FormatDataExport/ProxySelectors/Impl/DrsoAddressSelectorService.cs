namespace Bars.Gkh.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    /// <summary>
    /// Сервис получения <see cref="DrsoObjectQualityProxy"/>
    /// </summary>
    public class DrsoAddressSelectorService : BaseProxySelectorService<DrsoAddressProxy>
    {
        protected override IDictionary<long, DrsoAddressProxy> GetCache()
        {
            var publicServiceOrgContractRealObjRepository = this.Container.Resolve<IRepository<PublicServiceOrgContractRealObj>>();

            using (this.Container.Using(publicServiceOrgContractRealObjRepository))
            {
                return this.FilterService
                    .FilterByContragent(publicServiceOrgContractRealObjRepository.GetAll(),
                        x => x.RsoContract.PublicServiceOrg.Contragent)
                    .Select(x => new
                    {
                        x.Id,
                        DrsoId = x.RsoContract.Id,
                        RealityObjectId = x.RealityObject.Id,
                    })
                    .AsEnumerable()
                    .Select(x => new DrsoAddressProxy
                    {
                        Id = x.Id,
                        DrsoId = x.DrsoId,
                        DomId = x.RealityObjectId,
                    })
                    .ToDictionary(x => x.Id);
            }
        }
    }
}