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
    public class DrsoObjectQualitySelectorService : BaseProxySelectorService<DrsoObjectQualityProxy>
    {
        protected override IDictionary<long, DrsoObjectQualityProxy> GetCache()
        {
            var publicOrgServiceQualityLevelRepository = this.Container.Resolve<IRepository<PublicOrgServiceQualityLevel>>();

            using (this.Container.Using(publicOrgServiceQualityLevelRepository))
            {
                return this.FilterService
                    .FilterByContragent(publicOrgServiceQualityLevelRepository.GetAll(),
                        x => x.ServiceOrg.ResOrgContract.PublicServiceOrg.Contragent)
                    .Select(x => new
                    {
                        x.Id,
                        DrsoId = x.ServiceOrg.ResOrgContract.Id,
                        QualityName = x.Name.ToLower(),
                        QualityValue = x.Value,
                        x.UnitMeasure.OkeiCode
                    })
                    .AsEnumerable()
                    .Select(x => new DrsoObjectQualityProxy
                    {
                        Id = x.Id,
                        DrsoId = x.DrsoId,
                        QualityType = x.QualityName == "питьевая вода"
                            ? 8
                            : x.QualityName == "горячая вода"
                                ? 9
                                : x.QualityName == "тепловая энергия"
                                    ? 4
                                    : (int?) null,
                        QualityValue = x.QualityValue,
                        OkeiCode = x.OkeiCode,
                        QualityName = x.QualityName
                    })
                    .ToDictionary(x => x.Id);
            }
        }
    }
}