namespace Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService.TechPassport;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors.SystemSelectors;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.Utils;

    using MainSelectorNamespace = Gkh.FormatDataExport.ProxySelectors.Impl;

    /// <summary>
    /// Селектор для Дом
    /// </summary>
    public class HouseSelectorService : MainSelectorNamespace.HouseSelectorService
    {
        /// <inheritdoc />
        protected override IEnumerable<HouseProxy> GetProxies(IQueryable<RealityObject> roQuery)
        {
            var municipalityFiasOktmoRepository = this.Container.Resolve<IRepository<MunicipalityFiasOktmo>>();
            var tehPassportCacheService = this.Container.Resolve<ITehPassportCacheService>();
            var calcAccountService = this.Container.Resolve<ICalcAccountService>();

            using (this.Container.Using(municipalityFiasOktmoRepository,
                tehPassportCacheService,
                calcAccountService))
            {
                var timeZone = this.SelectParams.GetAs("TimeZone", TimeZoneType.EuropeMoscow);

                var manOrgDict = this.ProxySelectorFactory.GetSelector<ActualManOrgByRealityObject>()
                    .ProxyListCache
                    .ToDictionary(x => x.Key,
                        x => new
                        {
                            ContragentId = x.Value.Contragent.ExportId,
                            x.Value.TypeManagement
                        });

                var tpBasementCache = tehPassportCacheService.GetCacheByRealityObjects("Form_1", 27, 1)
                    .ToDictionary(x => x.Key, x => x.Value.ToInt());

                var tpLifeCycleStageCache = tehPassportCacheService.GetCacheByRealityObjects("Form_1_1", 1, 1)
                    .ToDictionary(x => x.Key, x => x.Value.ToInt());

                var countPersAccountCache = tehPassportCacheService.GetCacheByRealityObjects("Form_1", 14, 1)
                    .ToDictionary(x => x.Key, x => x.Value.ToInt());

                var reconstructionsYearCache = tehPassportCacheService.GetCacheByRealityObjects("Form_1", 3, 1)
                    .ToDictionary(x => x.Key, x => x.Value.ToInt());

                var placeOktmoDict = municipalityFiasOktmoRepository.GetAll()
                    .WhereNotNull(x => x.Municipality)
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

                return roQuery
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
                            PersonalAccountCount = countPersAccountCache.Get(x.Id),
                            AreaLiving = x.AreaLiving,
                            AccountFormationVariant = this.GetMethodFormFundCr(x.AccountFormationVariant),
                            TypeManagement = this.GetTypeContract(manOrgDict.Get(x.Id)?.TypeManagement),
                            LifeCycleStage = this.GetLifeCycle(tpLifeCycleStageCache.Get(x.Id)),
                            ReconstructionYear = this.GetDateFromYear(yearReconstruction),
                            ContragentId = manOrgDict.Get(x.Id)?.ContragentId,
                            NoInstallationPu = 2
                        };
                    });
            }
        }
    }
}