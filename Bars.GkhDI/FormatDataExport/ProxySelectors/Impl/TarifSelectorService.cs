namespace Bars.GkhDi.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Utils;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    public class TarifSelectorService : BaseProxySelectorService<TarifProxy>
    {
        public const string CommunalDocumentName = "Нормативно-правовой акт, устанавливающий норматив потребления коммунальной услуги";
        public const string HousingDocumentName = "Нормативно-правовой акт, устанавливающий норматив потребления жилищной услуги";
        public const string CrDocumentName = "Нормативно-правовой акт, устанавливающий норматив потребления услуги кап.ремонта";

        /// <inheritdoc />
        protected override IDictionary<long, TarifProxy> GetCache()
        {
            var baseServiceRepository = this.Container.ResolveRepository<BaseService>();
            var tariffForRsoRepository = this.Container.ResolveRepository<TariffForRso>();
            var tariffForConsumersRepository = this.Container.ResolveRepository<TariffForConsumers>();
            var consumptionNormsNpaRepository = this.Container.ResolveRepository<ConsumptionNormsNpa>();
            var fiasRepository = this.Container.ResolveRepository<Fias>();

            using (this.Container.Using(baseServiceRepository,
                tariffForRsoRepository,
                tariffForConsumersRepository,
                consumptionNormsNpaRepository,
                fiasRepository))
            {
                var roQuery = this.FilterService.GetFiltredQuery<RealityObject>();

                var regionCode = fiasRepository.GetAll()
                    .Select(x => x.CodeRegion)
                    .First(x => x != null)
                    .ToInt();

                var numberDict = tariffForRsoRepository.GetAll()
                    .Select(x => new
                    {
                        x.BaseService.Id,
                        x.NumberNormativeLegalAct,
                        x.DateStart
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.DateStart).FirstOrDefault()?.NumberNormativeLegalAct);

                var communalDateDict = consumptionNormsNpaRepository.GetAll()
                    .Select(x => new
                    {
                        x.BaseService.Id,
                        x.NpaDate
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id, x => x.NpaDate)
                    .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y).FirstOrDefault());

                var dateDict = tariffForConsumersRepository.GetAll()
                    .Select(x => new
                    {
                        x.BaseService.Id,
                        x.DateStart,
                        x.DateEnd,
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.DateStart).FirstOrDefault());

                return baseServiceRepository.GetAll()
                    .Where(x => roQuery.Any(r => r.Id == x.DisclosureInfoRealityObj.RealityObject.Id))
                    .Select(x => new
                    {
                        x.Id,
                        KindServiceDi = (KindServiceDi?) x.TemplateService.KindServiceDi,
                        ProviderId = (long?) x.Provider.ExportId,
                        x.Provider.Oktmo,
                        TemplateServiceName = x.TemplateService.Name,
                        x.TariffForConsumers
                    })
                    .AsEnumerable()
                    .Select(x => new TarifProxy
                    {
                        Id = x.Id,
                        DocumentName = this.GetDocumentName(x.KindServiceDi),
                        DocumentNumber = numberDict.Get(x.Id),
                        DocumentDate = x.KindServiceDi == KindServiceDi.Communal
                            ? communalDateDict.Get(x.Id)
                            : dateDict.Get(x.Id)?.DateStart,
                        StartDate = dateDict.Get(x.Id)?.DateStart,
                        EndDate = dateDict.Get(x.Id)?.DateEnd,
                        RegionCode = regionCode,
                        Type = this.GetTarifType(x.KindServiceDi),

                        RsoContragentId = x.ProviderId,

                        Oktmo = x.Oktmo,

                        CommunalServiceType = this.GetCommunalServiceType(x.TemplateServiceName),
                        TarifCost = x.TariffForConsumers,

                        CommunalResourceType = this.GetCommunalResourceType(x.TemplateServiceName)
                    })
                    .ToDictionary(x => x.Id);
            }
        }

        private int? GetTarifType(KindServiceDi? serviceType)
        {
            switch (serviceType)
            {
                case KindServiceDi.Communal:
                    return 1;
                case KindServiceDi.Housing:
                    return 5;
                case KindServiceDi.CapitalRepair:
                    return 3;
                default:
                    return null;
            }
        }

        private string GetDocumentName(KindServiceDi? serviceType)
        {
            switch (serviceType)
            {
                case KindServiceDi.Communal:
                    return TarifSelectorService.CommunalDocumentName;
                case KindServiceDi.Housing:
                    return TarifSelectorService.HousingDocumentName;
                case KindServiceDi.CapitalRepair:
                    return TarifSelectorService.CrDocumentName;
                default:
                    return string.Empty;
            }
        }

        private int? GetCommunalServiceType(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                switch (name.ToLower())
                {
                    case "холодное водоснабжение":
                        return 1;
                    case "горячее водоснабжение":
                        return 2;
                    case "водоотведение":
                        return 3;
                    case "электроснабжение":
                        return 4;
                    case "газоснабжение":
                        return 5;
                    case "отопление":
                        return 6;
                    case "обращение с твердыми отходами":
                        return 7;
                }
            }

            return null;
        }

        private int? GetCommunalResourceType(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                switch (name.ToLower())
                {
                    case "холодная вода":
                        return 1;
                    case "горячая вода":
                        return 2;
                    case "электрическая энергия":
                        return 3;
                    case "газ":
                        return 4;
                    case "тепловая энергия":
                        return 5;
                    case "бытовой газ в баллонах":
                        return 6;
                    case "твердое топливо":
                        return 7;
                    case "сточные бытовые воды":
                        return 8;
                }
            }

            return null;
        }
    }
}