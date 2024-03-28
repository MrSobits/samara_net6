namespace Bars.Gkh.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh.Modules.Gkh1468.Enums;

    /// <summary>
    /// Сервис получения <see cref="DrsoObjectProxy"/>
    /// </summary>
    public class DrsoObjectSelectorService : BaseProxySelectorService<DrsoObjectProxy>
    {
        protected override IDictionary<long, DrsoObjectProxy> GetCache()
        {
            var publicOrgServiceRepository = this.Container.Resolve<IRepository<PublicServiceOrgContractService>>();

            using (this.Container.Using(publicOrgServiceRepository))
            {
                return this.FilterService.FilterByContragent(publicOrgServiceRepository.GetAll(),
                        x => x.ResOrgContract.PublicServiceOrg.Contragent)
                    .Select(x => new
                    {
                        DrsoServiceId = (long?) x.Service.ExportId,
                        x.Id,
                        ResOrgContractId = (long?) x.ResOrgContract.Id,
                        x.CommunalResource,
                        x.SchemeConnectionType,
                        x.StartDate,
                        x.EndDate,
                        x.PlanVolume,
                        x.UnitMeasure.OkeiCode
                    })
                    .AsEnumerable()
                    .Select(x => new DrsoObjectProxy
                    {
                        Id = x.Id,
                        ResOrgContractId = x.ResOrgContractId,
                        DictUslugaId = x.DrsoServiceId,
                        CommunalResource = this.GetCommunalResource(x.CommunalResource),
                        SchemeConnectionType = this.GetSchemeConnectionType(x.SchemeConnectionType),
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        PlanVolume = x.PlanVolume,
                        OkeiCode = x.OkeiCode
                    })
                    .ToDictionary(x => x.Id);
            }
        }

        private int? GetCommunalResource(CommunalResource communalResource)
        {
            switch (communalResource?.Name)
            {
                case "Питьевая вода":
                    return 1;

                case "Техническая вода":
                    return 2;

                case "Горячая вода ":
                    return 3;

                case "Тепловая энергия":
                    return 4;

                case "Теплоноситель":
                    return 5;

                case "Поддерживаемая мощность":
                    return 6;

                case "Сточные воды":
                    return 7;

                case "Электрическая энергия":
                    return 8;

                case "Природный газ (метан)":
                    return 9;

                case "Сжиженный газ (пропан-бутан)":
                    return 10;

                case "Топливо твердое":
                    return 10;

                case "Топливо печное бытовое":
                    return 12;

                case "Керосин":
                    return 13;

                default:
                    return null;
            }
        }

        private int? GetSchemeConnectionType(SchemeConnectionType? schemeConnectionType)
        {
            switch (schemeConnectionType)
            {
                case SchemeConnectionType.Dependent:
                    return 1;
                case SchemeConnectionType.Independent:
                    return 2;
                default:
                    return null;
            }
        }
    }
}