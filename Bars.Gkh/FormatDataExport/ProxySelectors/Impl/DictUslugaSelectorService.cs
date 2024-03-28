namespace Bars.Gkh.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Entities.Dicts.ContractService;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Utils;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    /// <summary>
    /// Сервис получения <see cref="DictUslugaProxy"/>
    /// </summary>
    public class DictUslugaSelectorService : BaseProxySelectorService<DictUslugaProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, DictUslugaProxy> GetCache()
        {
            return this.GetProxy(false)
                .ToDictionary(x => x.Id);
        }

        protected override ICollection<DictUslugaProxy> GetAdditionalCache()
        {
            return this.GetProxy(true);
        }

        private ICollection<DictUslugaProxy> GetProxy(bool isFiltred)
        {
            var addContractServiceRepository = this.Container.ResolveRepository<AdditionalContractService>();
            var agrContractServiceRepository = this.Container.ResolveRepository<AgreementContractService>();
            var comContractServiceRepository = this.Container.ResolveRepository<CommunalContractService>();
            var templateServiceRepository = this.Container.ResolveRepository<TemplateService>();

            var drsoContractServiceRepository = this.Container.ResolveRepository<ServiceDictionary>();

            using (this.Container.Using(addContractServiceRepository,
                agrContractServiceRepository,
                comContractServiceRepository,
                drsoContractServiceRepository,
                templateServiceRepository))
            {
                var addContractServices = addContractServiceRepository.GetAll()
                    .WhereIfContainsBulked(isFiltred, x => x.ExportId, this.AdditionalIds)
                    .Select(x => new DictUslugaProxy
                    {
                        Id = x.ExportId,
                        Name = x.Name,
                        Type = 0, // прочие
                        ServiceType = 1,
                        IsNotSortOrder = 1,
                        OkeiCode = x.UnitMeasure.OkeiCode,
                        AnotherUnit = x.UnitMeasure.Name
                    })
                    .ToList();

                var agrContractServices = agrContractServiceRepository.GetAll()
                    .WhereIfContainsBulked(isFiltred, x => x.ExportId, this.AdditionalIds)
                    .Select(x => new DictUslugaProxy
                    {
                        Id = x.ExportId,
                        Name = x.Name,
                        Type = 2, // жилищная
                        ServiceType = 1,
                        IsNotSortOrder = 1,
                        HousingServiceType = this.GetHousingServiceType(x.Name),
                        OkeiCode = x.UnitMeasure.OkeiCode,
                        AnotherUnit = x.UnitMeasure.Name
                    })
                    .ToList();

                var comContractServices = comContractServiceRepository.GetAll()
                    .WhereIfContainsBulked(isFiltred, x => x.ExportId, this.AdditionalIds)
                    .Select(x => new DictUslugaProxy
                    {
                        Id = x.ExportId,
                        Name = x.Name,
                        Type = 1, // коммунальная
                        ServiceType = x.IsHouseNeeds ? 2 : 1,
                        IsNotSortOrder = 1,
                        CommunalServiceType = this.GetCommunalServiceType(x.Code),
                        ElectricMeteringType = x.CommunalResource == TypeCommunalResource.ElectricalEnergy
                            ? this.GetElectricMeteringType()
                            : null,
                        OkeiCode = x.UnitMeasure.OkeiCode,
                        AnotherUnit = x.UnitMeasure.Name,
                        CommunalResourceType = this.GetCommunalResourceType(x.CommunalResource),
                        SortOrder = this.GetSortOrder(x.CommunalResource)
                    })
                    .ToList();

                var drsoServices = drsoContractServiceRepository.GetAll()
                    .WhereIfContainsBulked(isFiltred, x => x.ExportId, this.AdditionalIds)
                    .Select(x => new
                    {
                        x.Id,
                        x.ExportId,
                        x.Name,
                        x.TypeCommunalResource,
                        x.UnitMeasure.OkeiCode,
                        AnotherUnit = x.UnitMeasure.Name,
                        x.TypeService,
                        x.IsProvidedForAllHouseNeeds
                    })
                    .AsEnumerable()
                    .Select(x => new DictUslugaProxy
                    {
                        Id = x.ExportId,
                        Name = x.Name,
                        Type = this.GetTypeService(x.TypeService),
                        ServiceType = x.IsProvidedForAllHouseNeeds ? 2 : 1,
                        CommunalServiceType = x.TypeService == TypeServiceGis.Communal
                            ? this.GetCommunalServiceType(x.TypeCommunalResource)
                            : null,
                        HousingServiceType = x.TypeService == TypeServiceGis.Housing
                            ? this.GetHousingServiceType(x.Name)
                            : null,
                        ElectricMeteringType = x.TypeCommunalResource == TypeCommunalResource.ElectricalEnergy
                            ? this.GetElectricMeteringType()
                            : null,
                        OkeiCode = x.OkeiCode,
                        AnotherUnit = x.AnotherUnit,
                        CommunalResourceType = x.TypeService == TypeServiceGis.Communal
                            ? this.GetCommunalResourceType(x.TypeCommunalResource)
                            : null,
                        IsNotSortOrder = 1,
                        DrsoServiceId = x.Id
                    })
                    .ToList();

                var templateServices = templateServiceRepository.GetAll()
                    .WhereIfContainsBulked(isFiltred, x => x.ExportId, this.AdditionalIds)
                    .Select(x => new
                    {
                        Id = x.ExportId,
                        x.Name,
                        Type = (TypeGroupServiceDi?) x.TypeGroupServiceDi,
                        x.UnitMeasure.OkeiCode,
                        AnotherUnit = x.UnitMeasure.Name,
                        IsNotSortOrder = 1,
                        TemplateServiceId = x.Id,
                        x.CommunalResourceType,
                        x.HousingResourceType,
                        x.Code
                    })
                    .AsEnumerable()
                    .Select(x => new DictUslugaProxy
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Type = this.GetTypeService(x.Type),
                        ServiceType = 1,
                        CommunalServiceType = x.Type == TypeGroupServiceDi.Communal
                            ? this.GetCommunalTemplateServiceType(x.Code)
                            : null,
                        HousingServiceType = x.Type == TypeGroupServiceDi.Housing
                            ? this.GetHousingServiceType(x.HousingResourceType)
                            : null,
                        ElectricMeteringType = x.CommunalResourceType == TypeCommunalResource.ElectricalEnergy
                            ? this.GetElectricMeteringType()
                            : null,
                        OkeiCode = x.OkeiCode,
                        AnotherUnit = x.AnotherUnit,
                        CommunalResourceType = x.Type == TypeGroupServiceDi.Communal
                            ? this.GetCommunalResourceType(x.CommunalResourceType)
                            : null,
                        IsNotSortOrder = 1,
                        TemplateServiceId = x.TemplateServiceId
                    })
                    .ToList();

                return drsoServices
                    .Union(addContractServices)
                    .Union(agrContractServices)
                    .Union(comContractServices)
                    .Union(templateServices)
                    .ToList();
            }
        }

        private int? GetCommunalServiceType(TypeCommunalResource? communalResource)
        {
            // Приложение 59
            switch (communalResource)
            {
                case TypeCommunalResource.ColdWater:
                    return 1;
                case TypeCommunalResource.HotWater:
                    return 2;
                case TypeCommunalResource.WasteHomeWaters:
                    return 3;
                case TypeCommunalResource.ElectricalEnergy:
                    return 4;
                case TypeCommunalResource.Gas:
                    return 5;
                case TypeCommunalResource.ThermalEnergy:
                    return 6;
                default:
                    return null;
            }
        }
        private int? GetCommunalTemplateServiceType(string communalResource)
        {
            // Приложение 59
            switch (communalResource)
            {
                case "17":
                case "33":
                    return 1;
                case "18":
                    return 2;
                case "19":
                    return 3;
                case "20":
                    return 4;
                case "21":
                    return 5;
                case "22":
                    return 6;
                default:
                    return null;
            }
        }
        private int? GetCommunalServiceType(string communalResource)
        {
            // Приложение 59
            switch (communalResource)
            {
                case "1":
                case "99":
                    return 1;
                case "2":
                    return 2;
                case "3":
                    return 3;
                case "4":
                    return 4;
                case "5":
                    return 5;
                case "6":
                    return 6;
                default:
                    return null;
            }
        }
        private int? GetHousingServiceType(string serviceName)
        {
            // Приложение 60
            switch (serviceName.ToLower())
            {
                case "капитальный ремонт жилого здания":
                    return 2;
                default:
                    return 3;
            }
        }
        private int? GetHousingServiceType(TypeHousingResource? resource)
        {
            // Приложение 60
            switch (resource)
            {
                case TypeHousingResource.RealityObjectPay:
                    return 1;
                case TypeHousingResource.OverhaulPay:
                    return 2;
                case TypeHousingResource.Maintenance:
                    return 3;
                case TypeHousingResource.SocialRentals:
                    return 4;
                case TypeHousingResource.CommercialRentals:
                    return 5;
                case TypeHousingResource.Rent:
                    return 6;
                case TypeHousingResource.MandatoryWork:
                    return 7;
                case TypeHousingResource.AdditionalWork:
                    return 8;
                default:
                    return null;
            }
        }

        private int? GetElectricMeteringType()
        {
            // Приложение 24
            return 1; // тип порешали 25.07.2017 18:50
        }

        private int? GetCommunalResourceType(TypeCommunalResource? communalResource)
        {
            // Приложение 21
            switch (communalResource)
            {
                case TypeCommunalResource.ColdWater:
                    return 1;
                case TypeCommunalResource.HotWater:
                    return 2;
                case TypeCommunalResource.ElectricalEnergy:
                    return 3;
                case TypeCommunalResource.Gas:
                    return 4;
                case TypeCommunalResource.ThermalEnergy:
                    return 5;
                case TypeCommunalResource.HomeGasInBulbs:
                    return 6;
                case TypeCommunalResource.SolidFuel:
                    return 7;
                case TypeCommunalResource.WasteHomeWaters:
                    return 8;
                default:
                    return null;
            }
        }

        private int? GetSortOrder(TypeCommunalResource communalResource)
        {
            switch (communalResource)
            {
                case TypeCommunalResource.ThermalEnergy:
                    return 1;
                case TypeCommunalResource.HotWater:
                    return 2;
                case TypeCommunalResource.ColdWater:
                    return 3;
                case TypeCommunalResource.WasteHomeWaters:
                    return 4;
                case TypeCommunalResource.ElectricalEnergy:
                    return 5;
                case TypeCommunalResource.Gas:
                    return 6;
                default:
                    return null;
            }
        }

        private int GetTypeService(TypeGroupServiceDi? templateType)
        {
            switch (templateType)
            {
                case TypeGroupServiceDi.Communal:
                    return 1;
                case TypeGroupServiceDi.Housing:
                    return 2;
                case TypeGroupServiceDi.Other:
                    return 0;
            }

            return 0;
        }

        private int GetTypeService(TypeServiceGis? serviceType)
        {
            switch (serviceType)
            {
                case TypeServiceGis.Communal:
                    return 1;
                case TypeServiceGis.Housing:
                    return 2;
                case TypeServiceGis.Other:
                    return 0;
            }

            return 0;
        }
    }
}