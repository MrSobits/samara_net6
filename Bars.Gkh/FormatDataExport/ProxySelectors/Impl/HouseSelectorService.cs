namespace Bars.Gkh.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.Gkh.DomainService.TechPassport;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors.SystemSelectors;
    
    /// <summary>
    /// Селектор для Дом
    /// </summary>
    public class HouseSelectorService : BaseProxySelectorService<HouseProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, HouseProxy> GetCache()
        {
            return this.GetProxies(this.FilterService.GetFiltredQuery<RealityObject>()).Distinct(x=>x.Id)
                .ToDictionary(x => x.Id);
        }

        /// <inheritdoc />
        protected override ICollection<HouseProxy> GetAdditionalCache()
        {
            var realityObjectRepository = this.Container.ResolveRepository<RealityObject>();

            using (this.Container.Using(realityObjectRepository))
            {
                var query = realityObjectRepository.GetAll()
                    .WhereContainsBulked(x => x.Id, this.AdditionalIds);

                return this.GetProxies(query).ToList();
            }
        }

        protected virtual IEnumerable<HouseProxy> GetProxies(IQueryable<RealityObject> roQuery)
        {
            var municipalityFiasOktmoRepository = this.Container.ResolveRepository<MunicipalityFiasOktmo>();
            var tehPassportCacheService = this.Container.Resolve<ITehPassportCacheService>();

            using (this.Container.Using(
                municipalityFiasOktmoRepository,
                tehPassportCacheService))
            {
                var tpBasementCache = tehPassportCacheService.GetCacheByRealityObjects("Form_1", 27, 1)
                    .ToDictionary(x => x.Key, x => x.Value.ToInt());

                var tpLifeCycleStageCache = tehPassportCacheService.GetCacheByRealityObjects("Form_1_1", 1, 1)
                    .ToDictionary(x => x.Key, x => x.Value.ToInt());

                var countPersAccountCache = tehPassportCacheService.GetCacheByRealityObjects("Form_1", 14, 1)
                    .ToDictionary(x => x.Key, x => x.Value.ToInt());

                var reconstructionsYearCache = tehPassportCacheService.GetCacheByRealityObjects("Form_1", 3, 1)
                    .ToDictionary(x => x.Key, x => x.Value.ToInt());

                var timeZone = this.SelectParams.GetAs("TimeZone", TimeZoneType.EuropeMoscow);

                var manOrgDict = this.ProxySelectorFactory.GetSelector<ActualManOrgByRealityObject>()
                    .ProxyListCache
                    .ToDictionary(x => x.Key,
                        x => new
                        {
                            ContragentId = x.Value.Contragent.ExportId,
                            x.Value.TypeManagement
                        });

                var placeOktmoDict = municipalityFiasOktmoRepository.GetAll()
                    .Select(x => new
                    {
                        x.Municipality.Id,
                        x.FiasGuid,
                        x.Oktmo
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key,
                        x => x.Select(y => new
                            {
                                y.FiasGuid,
                                y.Oktmo
                            })
                            .GroupBy(y => y.FiasGuid, y => y.Oktmo)
                            .ToDictionary(y => y.Key, y => y.FirstOrDefault()));

                return this.FilterByEditDate(roQuery)
                    .Select(x => new
                    {
                        x.Id,
                        MuId = x.Municipality.Id,
                        City = x.Municipality.Name,
                        Settlement = x.FiasAddress.PlaceName,
                        x.FiasAddress.PlaceGuidId,
                        x.FiasAddress.StreetName,
                        x.FiasAddress.House,
                        x.FiasAddress.Building,
                        x.FiasAddress.Housing,
                        x.FiasAddress.Letter,
                        x.MaximumFloors,
                        x.BuildYear,
                        x.AreaMkd,
                        x.AreaCommonUsage,
                        x.FiasAddress.StreetGuidId,
                        x.FiasAddress.HouseGuid,
                        x.CadastralHouseNumber,
                        x.PhysicalWear,
                        x.TypeHouse,
                        x.AreaLiving,
                        x.DateCommissioning,
                        x.Floors,
                        x.IsCulturalHeritage,
                        x.Municipality.Oktmo,
                        x.ConditionHouse,
                        x.AccountFormationVariant
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var yearReconstruction = reconstructionsYearCache.Get(x.Id);
                        
                        return new HouseProxy
                        {
                            Id = x.Id,
                            City = x.City,
                            Settlement = x.Settlement,
                            Street = x.StreetName,
                            House = x.House,
                            Building = x.Building,
                            Housing = x.Housing,
                            Letter = x.Letter,
                            HouseGuid = x.HouseGuid.ToStr(),
                            OktmoCode = placeOktmoDict.Get(x.MuId)?.Get(x.PlaceGuidId) ?? x.Oktmo,
                            TimeZone = timeZone.ToInt().ToString(),
                            IsNumberExists = string.IsNullOrEmpty(x.CadastralHouseNumber) ? 2 : 1,
                            CadastralHouseNumber = x.CadastralHouseNumber,
                            TypeHouse = this.GetTypeHouse(x.TypeHouse),
                            AreaMkd = x.AreaMkd,
                            ConditionHouse = this.GetCondition(x.ConditionHouse),
                            CommissioningYear = this.GetDateFromYear(x.DateCommissioning?.Year),
                            MaximumFloors = x.MaximumFloors,
                            MinimumFloors = x.Floors,
                            UndergroundFloorCount = tpBasementCache.Get(x.Id),
                            IsCulturalHeritage = x.IsCulturalHeritage ? 1 : 2,
                            AreaCommonUsage = x.AreaCommonUsage,
                            BuildYear = this.GetDateFromYear(x.BuildYear),
                            AreaLiving = x.AreaLiving,
                            AccountFormationVariant = this.GetMethodFormFundCr(x.AccountFormationVariant),
                            TypeManagement = this.GetTypeContract(manOrgDict.Get(x.Id)?.TypeManagement),
                            LifeCycleStage = this.GetLifeCycle(tpLifeCycleStageCache.Get(x.Id)),
                            ContragentId = manOrgDict.Get(x.Id)?.ContragentId,
                            NoInstallationPu = 1,
                            PersonalAccountCount = countPersAccountCache.Get(x.Id),
                            ReconstructionYear = this.GetDateFromYear(yearReconstruction),
                        };
                    });
            }
        }

        protected int? GetTypeHouse(TypeHouse? typeHouse)
        {
            switch (typeHouse)
            {
                case TypeHouse.ManyApartments:
                    return 1;
                case TypeHouse.Individual:
                case TypeHouse.SocialBehavior:
                    return 2;
                case TypeHouse.BlockedBuilding:
                    return 3;
                default:
                    return null;
            }
        }

        protected string GetLifeCycle(int? typeHouse)
        {
            switch (typeHouse)
            {
                case 1:
                    return "Строящийся";
                case 2:
                    return "Эксплуатируемый";
                case 3:
                    return "Выведен из эксплуатации";
                case 4:
                    return "Снесенный";
                default:
                    return null;
            }
        }

        protected int? GetCondition(ConditionHouse? conditionHouse)
        {
            switch (conditionHouse)
            {
                case ConditionHouse.Emergency:
                case ConditionHouse.Resettlement:
                    return 1;
                case ConditionHouse.Serviceable:
                    return 2;
                case ConditionHouse.Dilapidated:
                case ConditionHouse.Razed:
                    return 3;
                default:
                    return null;
            }
        }

        protected int? GetTypeContract(TypeManagementManOrg? typeValue)
        {
            switch (typeValue)
            {
                case TypeManagementManOrg.TSJ:
                    return 2; // ТСЖ
                case TypeManagementManOrg.JSK:
                    return 3; // ЖСК
                case TypeManagementManOrg.Other:
                    return 4; // Иной кооператив
                case TypeManagementManOrg.UK:
                    return 5; // УО
                default:
                    return 6; // Не выбран
            }
        }

        protected int? GetMethodFormFundCr(CrFundFormationType? methodFormFundCr)
        {
            switch (methodFormFundCr)
            {
                case CrFundFormationType.SpecialAccount:
                    return 3; 
                case CrFundFormationType.RegOpAccount:
                    return 4; 
                default:
                    return null;
            }
        }

        protected DateTime? GetDateFromYear(int? year)
        {
            return year != null && year >= 1 && year <= 9999 ? new DateTime(year.Value, 1, 1) : default(DateTime?);
        }
    }
}