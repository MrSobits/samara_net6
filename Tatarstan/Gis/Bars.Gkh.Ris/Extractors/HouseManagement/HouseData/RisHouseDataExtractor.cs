namespace Bars.Gkh.Ris.Extractors.HouseManagement.HouseData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Enums;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh.Ris.Enums.HouseManagement;

    /// <summary>
    /// Экстрактор данных по домам
    /// </summary>
    public class RisHouseDataExtractor : BaseDataExtractor<RisHouse, RealityObject>
    {
        private IDictionary houseStateDict;
        // private Dictionary<long, nsiRef> projectTypeDict;
        private IDictionary olsonTzDict;
        private IDictionary overhaulFormingKindDict;
        private IDictionary houseManagementTypeDict;
        private Dictionary<long, TehPassInfoProxy> tehPassDict;
        private Dictionary<long, CrFundFormationDecisionType> decisionTypesByRoId;
        private Dictionary<long, ManOrgBaseContract> manOrgContractsByRoId;
        private Dictionary<string, string> oktmoByFiasId;

        /// <summary>
        /// Менеджер справочников
        /// </summary>
        public IDictionaryManager DictionaryManager { get; set; }

        /// <summary>
        /// Получить сущности сторонней системы - дома
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы - дома</returns>
        public override List<RealityObject> GetExternalEntities(DynamicDictionary parameters)
        {
            if (parameters.ContainsKey("uoId"))
            {
                return this.GetRealityObjectListByUO(parameters);
            }

            if (parameters.ContainsKey("omsId"))
            {
                return this.GetRealityObjectListByOMS(parameters);
            }

            if (parameters.ContainsKey("rsoId"))
            {
                return this.GetRealityObjectListByRSO(parameters);
            }

            return this.GetRealityObjectListDefault(parameters);
        }

        /// <summary>
        /// Конвертировать тип дома
        /// </summary>
        /// <param name="houseType">Тип дома в сторонней системе</param>
        /// <returns>Тип дома Ris</returns>
        public HouseType ConvertHouseType(TypeHouse houseType)
        {
            switch (houseType)
            {
                case TypeHouse.BlockedBuilding:
                    return HouseType.Blocks;
                case TypeHouse.ManyApartments:
                    return HouseType.Apartment;
                default:
                    return HouseType.Living;
            }
        }

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// Заполнить словари
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        protected override void BeforeExtractHandle(DynamicDictionary parameters)
        {
            this.houseStateDict = this.DictionaryManager.GetDictionary("HouseStateDictionary");
            // this.projectTypeDict = this.GetDictionary("Тип проекта здания");
            this.olsonTzDict = this.DictionaryManager.GetDictionary("OlsonTZDictionary");
            this.overhaulFormingKindDict = this.DictionaryManager.GetDictionary("OverhaulFormingKindDictionary");
            this.houseManagementTypeDict = this.DictionaryManager.GetDictionary("HouseManagementTypeDictionary");

            var crFundFormationDecisionDomain = this.Container.ResolveDomain<CrFundFormationDecision>();
            var manOrgContractRealityObjectContract = this.Container.ResolveDomain<ManOrgContractRealityObject>();
            var fiasDomain = this.Container.ResolveDomain<Fias>();

            try
            {
                this.decisionTypesByRoId = crFundFormationDecisionDomain.GetAll()
                    .Where(x => x.Protocol != null && x.Protocol.RealityObject != null)
                    .Select(x => new
                    {
                        x.Decision,
                        x.Protocol.RealityObject.Id,
                        x.Protocol.DateStart
                    })
                    .ToArray()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => x.Where(y => y.DateStart == x.Max(z => z.DateStart)).Select(t => t.Decision).FirstOrDefault());

                this.manOrgContractsByRoId = manOrgContractRealityObjectContract.GetAll()
                    .Where(x => x.RealityObject != null && x.ManOrgContract != null)
                    .Where(x => x.ManOrgContract.StartDate <= DateTime.Now && x.ManOrgContract.EndDate >= DateTime.Now)
                    .Select(
                        x => new
                        {
                            x.RealityObject.Id,
                            x.ManOrgContract
                        })
                    .ToArray()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => x.Select(z => z.ManOrgContract).FirstOrDefault());

                this.oktmoByFiasId = fiasDomain.GetAll()
                    .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                    .Select(
                        x => new
                        {
                            x.AOGuid,
                            x.OKTMO
                        })
                    .ToArray()
                    .GroupBy(x => x.AOGuid)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.OKTMO).FirstOrDefault());
            }
            finally
            {
                this.Container.Release(crFundFormationDecisionDomain);
                this.Container.Release(manOrgContractRealityObjectContract);
                this.Container.Release(fiasDomain);
            }
        }

        /// <summary>
        /// Выполнить обработку перед подготовкой Ris сущностей
        /// Получить данных из тех паспортов для выбранных домов
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        /// <param name="realityObjects">Выбранные дома</param>
        protected override void BeforePrepareRisEntitiesHandle(DynamicDictionary parameters, List<RealityObject> realityObjects)
        {
            var realityObjectIds = realityObjects.Select(x => x.Id).ToArray();
            var techPassValueDomain = this.Container.ResolveDomain<TehPassportValue>();

            try
            {
                this.tehPassDict =
                    techPassValueDomain.GetAll()
                    .Where(x => realityObjectIds.Contains(x.TehPassport.RealityObject.Id))
                    .Where(x =>
                        (x.FormCode == "Form_6_1_1" && x.CellCode == "2:1") || (x.FormCode == "Form_6_1_1" && x.CellCode == "1:1") ||
                        (x.FormCode == "Form_2" && x.CellCode == "3:3") || (x.FormCode == "Form_1_3_3" && x.CellCode == "2:1"))
                    .ToArray()
                    .GroupBy(x => x.TehPassport.RealityObject.Id)
                    .ToDictionary(
                        x => x.Key,
                        x =>
                            new TehPassInfoProxy
                            {
                                EnergyDateValue = x.FirstOrDefault(y => y.FormCode == "Form_6_1_1" && y.CellCode == "2:1"),
                                EnergyCategoryValue = x.FirstOrDefault(y => y.FormCode == "Form_6_1_1" && y.CellCode == "1:1"),
                                BuiltUpAreaValue = x.FirstOrDefault(y => y.FormCode == "Form_2" && y.CellCode == "3:3"),
                                UndergroundAreaValue = x.FirstOrDefault(y => y.FormCode == "Form_1_3_3" && y.CellCode == "2:1")
                            });
            }
            finally
            {
                this.Container.Release(techPassValueDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="realityObject">Сущность внешней системы</param>
        /// <param name="risHouse">Ris сущность</param>
        protected override void UpdateRisEntity(RealityObject realityObject, RisHouse risHouse)
        {
            var state = this.houseStateDict.GetDictionaryRecord((int)realityObject.ConditionHouse);
            // var projectType = this.projectTypeDict?.Get(realityObject.ConditionHouse.ToLong());
            var olsonTz = this.olsonTzDict.GetDictionaryRecord(0);
            var tehPass = this.tehPassDict?.Get(realityObject.Id);
            var decisionType = this.decisionTypesByRoId?.Get(realityObject.Id);
            var overhaulFormingKind = decisionType != null ? this.overhaulFormingKindDict.GetDictionaryRecord((long)decisionType) : null;
            var houseManagementTypeEnum = this.GetHouseManagementTypeEnum(realityObject.Id);
            var houseManagementType = houseManagementTypeEnum != null ? this.houseManagementTypeDict.GetDictionaryRecord((long)houseManagementTypeEnum) : null;

            risHouse.HouseType = this.ConvertHouseType(realityObject.TypeHouse);
            risHouse.ExternalSystemEntityId = realityObject.Id;
            risHouse.ExternalSystemName = "gkh";
            risHouse.FiasHouseGuid = realityObject.HouseGuid;
            risHouse.CadastralNumber = realityObject.CadastralHouseNumber;
            risHouse.TotalSquare = realityObject.AreaMkd;
            // risHouse.ResidentialSquare = realityObject.AreaLiving.GetValueOrDefault();
            // risHouse.NonResidentialSquare = this.GetNonResidentialSquare(realityObject);
            risHouse.StateCode = state?.GisCode;
            risHouse.StateGuid = state?.GisGuid;
            //risHouse.ProjectSeries = realityObject.TypeProject?.Name;
            // risHouse.ProjectTypeCode = projectType?.Code;
            //risHouse.ProjectTypeGuid = projectType?.GUID;
            //risHouse.BuildingYear = realityObject.BuildYear.HasValue ? (short?)realityObject.BuildYear.Value : null;
            risHouse.UsedYear = (short?)realityObject.DateCommissioning?.Year;
            risHouse.FloorCount = realityObject.MaximumFloors.ToString();
            // risHouse.TotalWear = realityObject.PhysicalWear;
            // risHouse.EnergyDate = this.tehPassDict.ContainsKey(realityObject.Id)
            //    ? this.tehPassDict[realityObject.Id].EnergyDate
            //    : null;
            // узнать как сопоставлять и сделать здесь получение исходя из TehPassDict[x.Id].EnergyCategory
            //risHouse.EnergyCategoryCode = "2"; //?
            //risHouse.EnergyCategoryGuid = "c8c8e97a-b76a-46a6-8e21-f83482e64eeb"; //?
            risHouse.OktmoCode = this.oktmoByFiasId?.Get(realityObject.FiasAddress.PlaceGuidId); // this.oktmoByFiasId?.Get(realityObject.FiasAddress.PlaceGuidId);  //realityObject.FiasAddress Municipality?.Oktmo?.ToString() ?? "";
            risHouse.OlsonTZCode = olsonTz?.GisCode;
            risHouse.OlsonTZGuid = olsonTz?.GisGuid;
            risHouse.CulturalHeritage = false;
            //risHouse.BuiltUpArea = this.tehPassDict.ContainsKey(realityObject.Id)
            //    ? this.tehPassDict[realityObject.Id].BuiltUpArea
            //    : null;
            risHouse.MinFloorCount = realityObject.Floors;
            risHouse.UndergroundFloorCount = tehPass?.UndergroundArea.GetValueOrDefault() > 0 ? "1" : "0";
            //risHouse.OverhaulYear = realityObject.DateLastOverhaul.HasValue
            //    ? (short?)realityObject.DateLastOverhaul?.Year
            //    : null;
            risHouse.OverhaulFormingKindCode = overhaulFormingKind?.GisCode;
            risHouse.OverhaulFormingKindGuid = overhaulFormingKind?.GisGuid;
            risHouse.HouseManagementTypeCode = houseManagementType?.GisCode;
            risHouse.HouseManagementTypeGuid = houseManagementType?.GisGuid;
        }

        /// <summary>
        /// Получить способ управления домом по действующему ДУ
        /// </summary>
        /// <param name="realityObjectId">Идентификатор дома</param>
        /// <returns>Способ управления домом</returns>
        private HouseManagementType? GetHouseManagementTypeEnum(long realityObjectId)
        {
            HouseManagementType? result = null;
            var manOrgContract = this.manOrgContractsByRoId?.Get(realityObjectId);

            if (manOrgContract != null)
            {
                if (manOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.JskTsj
                    && manOrgContract.ManagingOrganization?.Contragent?.OrganizationForm?.Code == "85")
                {
                    result = HouseManagementType.AnotherCooperative;
                }
                else if (manOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag)
                {
                    result = HouseManagementType.LocalControl;
                }
            }
            else
            {
                result = HouseManagementType.NotSet;
            }

            return result;
        }

        /// <summary>
        /// Получить список домов по UO
        /// </summary>
        /// <param name="parameters">Параметры запроса</param>
        /// <returns>Список домов</returns>
        private List<RealityObject> GetRealityObjectListByUO(DynamicDictionary parameters)
        {
            long[] selectedIds;
            var selectedHouses = parameters.GetAs("selectedHouses", string.Empty);

            if (selectedHouses.ToUpper() == "ALL")
            {
                selectedIds = null; // выбраны все, фильтрацию не накладываем
            }
            else
            {
                selectedIds = selectedHouses.ToLongArray();
            }

            var contragentId = parameters.GetAs<long>("uoId");
            var result = new List<RealityObject>();
            var manOrgContractRealityObjectDomain = this.Container.ResolveDomain<ManOrgContractRealityObject>();
            var manOrgDomain = this.Container.ResolveRepository<ManagingOrganization>();

            try
            {
                var manorg = manOrgDomain.GetAll()
                    .Where(x => x.ActivityGroundsTermination == GroundsTermination.NotSet)
                    .FirstOrDefault(x => x.Contragent.Id == contragentId);

                if (manorg == null)
                {
                    return result;
                }

                return
                    manOrgContractRealityObjectDomain.GetAll()
                        .WhereIf(selectedIds != null, x => selectedIds.Contains(x.RealityObject.Id))
                        .Where(x => x.RealityObject.TypeHouse != TypeHouse.NotSet)
                        .Where(x => x.ManOrgContract.ManagingOrganization.Id == manorg.Id)
                        .Where(x => x.ManOrgContract.ManagingOrganization.ActivityGroundsTermination == GroundsTermination.NotSet)
                        .Where(x => x.ManOrgContract.ManagingOrganization.TypeManagement != TypeManagementManOrg.Other)
                        .Where(x => x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgOwners
                            || x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.JskTsj)
                        .Where(x => x.ManOrgContract.TerminateReason == null || x.ManOrgContract.TerminateReason == string.Empty)
                        .Where(x => !x.ManOrgContract.StartDate.HasValue || x.ManOrgContract.StartDate.Value <= DateTime.Now)
                        .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate.Value >= DateTime.Now)
                        .Select(x => x.RealityObject)
                        .ToList();
            }
            finally
            {
                this.Container.Release(manOrgContractRealityObjectDomain);
                this.Container.Release(manOrgDomain);
            }
        }

        /// <summary>
        /// Получить список домов по OMS
        /// </summary>
        /// <param name="parameters">Параметры запроса</param>
        /// <returns>Список домов</returns>
        private List<RealityObject> GetRealityObjectListByOMS(DynamicDictionary parameters)
        {
            long[] selectedIds;
            var selectedHouses = parameters.GetAs("selectedHouses", string.Empty);

            if (selectedHouses.ToUpper() == "ALL")
            {
                selectedIds = null; // выбраны все, фильтрацию не накладываем
            }
            else
            {
                selectedIds = selectedHouses.ToLongArray();
            }

            var contragentId = parameters.GetAs<long>("omsId");
            var result = new List<RealityObject>();
            var realityObjectDomain = this.Container.ResolveDomain<RealityObject>();
            var localGovernmentMunicipality = this.Container.ResolveDomain<LocalGovernmentMunicipality>();
            var manOrgContractRealityObjectDomain = this.Container.ResolveDomain<ManOrgContractRealityObject>();
            var moContractDirectManagServ = this.Container.ResolveDomain<RealityObjectDirectManagContract>();
            var localGovernmentDomain = this.Container.ResolveDomain<LocalGovernment>();

            try
            {
                var localGovernment = localGovernmentDomain.GetAll().FirstOrDefault(x => x.Contragent.Id == contragentId);

                if (localGovernment == null)
                {
                    return result;
                }

                var municipalityIds = localGovernmentMunicipality
                    .GetAll()
                    .Where(x => x.LocalGovernment.Id == localGovernment.Id)
                    .Select(x => x.Municipality.Id)
                    .ToArray();

                return
                    manOrgContractRealityObjectDomain.GetAll()
                        .WhereIf(selectedIds != null, x => selectedIds.Contains(x.RealityObject.Id))
                        .Where(x => x.RealityObject.Municipality != null && municipalityIds.Contains(x.RealityObject.Municipality.Id))
                        .Where(x => x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag)
                        .Where(x => x.ManOrgContract.TerminateReason == null || x.ManOrgContract.TerminateReason == string.Empty)
                        .Where(
                            x =>
                                (!x.ManOrgContract.StartDate.HasValue || x.ManOrgContract.StartDate.Value <= DateTime.Now)
                                    && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate.Value >= DateTime.Now))
                        .Where(x => moContractDirectManagServ.GetAll().Any(y => y.Id == x.ManOrgContract.Id && !y.IsServiceContract))
                        .Select(x => x.RealityObject)
                        .ToList();

            }
            finally
            {
                this.Container.Release(realityObjectDomain);
                this.Container.Release(localGovernmentMunicipality);
                this.Container.Release(manOrgContractRealityObjectDomain);
                this.Container.Release(moContractDirectManagServ);
                this.Container.Release(localGovernmentDomain);
            }
        }

        /// <summary>
        /// Получить список домов по RSO
        /// </summary>
        /// <param name="parameters">Параметры запроса</param>
        /// <returns>Список домов</returns>
        private List<RealityObject> GetRealityObjectListByRSO(DynamicDictionary parameters)
        {
            long[] selectedIds;
            var selectedHouses = parameters.GetAs("selectedHouses", string.Empty);

            if (selectedHouses.ToUpper() == "ALL")
            {
                selectedIds = null; // выбраны все, фильтрацию не накладываем
            }
            else
            {
                selectedIds = selectedHouses.ToLongArray();
            }

            var contragentId = parameters.GetAs<long>("rsoId");
            var result = new List<RealityObject>();
            var realObjPublicServiceOrgDomain = this.Container.ResolveDomain<PublicServiceOrgContract>();
            var publicServiceOrgDomain = this.Container.ResolveDomain<PublicServiceOrg>();

            try
            {
                //Поставщик ресурсов
                var publicServiceOrg = publicServiceOrgDomain.GetAll().FirstOrDefault(x => x.Contragent.Id == contragentId);

                if (publicServiceOrg != null)
                {
                    result.AddRange(
                        realObjPublicServiceOrgDomain.GetAll()
                            .WhereIf(selectedIds != null, x => selectedIds.Contains(x.RealityObject.Id))
                            .Where(x => x.PublicServiceOrg.Id == publicServiceOrg.Id)
                            .Where(x => (!x.DateStart.HasValue || x.DateStart.Value <= DateTime.Now)
                                && (!x.DateEnd.HasValue || x.DateEnd >= DateTime.Now))
                            .Select(x => x.RealityObject)
                            .ToList());
                }
            }
            finally
            {
                this.Container.Release(realObjPublicServiceOrgDomain);
                this.Container.Release(publicServiceOrgDomain);
            }

            return result;
        }

        /// <summary>
        /// Получить список домов
        /// </summary>
        /// <param name="parameters">Параметры запроса</param>
        /// <returns>Список домов</returns>
        private List<RealityObject> GetRealityObjectListDefault(DynamicDictionary parameters)
        {
            long[] selectedIds;
            var selectedHouses = parameters.GetAs("selectedHouses", string.Empty);

            if (selectedHouses.ToUpper() == "ALL")
            {
                selectedIds = null; // выбраны все, фильтрацию не накладываем
            }
            else
            {
                selectedIds = selectedHouses.ToLongArray();
            }

            var realityObjectDomain = this.Container.ResolveDomain<RealityObject>();

            try
            {
                return realityObjectDomain.GetAll()
                    .WhereIf(selectedIds != null, x => selectedIds.Contains(x.Id))
                    .ToList();
            }
            finally
            {
                this.Container.Release(realityObjectDomain);
            }
        }

        /// <summary>
        /// Получить нежилую площадь
        /// </summary>
        /// <param name="house">Дом</param>
        /// <returns>Нежилая площадь</returns>
        private decimal GetNonResidentialSquare(RealityObject house)
        {
            var totalSquare = house.AreaLivingNotLivingMkd;
            var livingSquare = house.AreaLiving;

            if (totalSquare.HasValue && livingSquare.HasValue)
            {
                var totalSquareValue = totalSquare.Value;
                var livingSquareValue = livingSquare.Value;

                if (totalSquareValue > livingSquareValue)
                {
                    return totalSquareValue - livingSquareValue;
                }
            }

            return 0m;
        }

        /// <summary>
        /// Служебный класс для паспорта объекта
        /// </summary>
        private class TehPassInfoProxy
        {
            public TehPassportValue EnergyDateValue { get; set; }
            public TehPassportValue EnergyCategoryValue { get; set; }
            public TehPassportValue BuiltUpAreaValue { get; set; }
            public TehPassportValue UndergroundAreaValue { get; set; }

            public DateTime? EnergyDate
            {
                get
                {
                    return this.EnergyDateValue != null ? this.EnergyDateValue.Value.ToDateTime() :
                    (DateTime?)null;
                }
            }
            public int? EnergyCategory
            {
                get
                {
                    return this.EnergyCategoryValue != null
                        ? this.EnergyCategoryValue.Value.ToInt()
                        : (int?)null;
                }
            }
            public decimal? BuiltUpArea
            {
                get
                {
                    return this.BuiltUpAreaValue != null
                        ? this.BuiltUpAreaValue.Value.ToDecimal()
                        : (decimal?)null;
                }
            }
            public decimal? UndergroundArea
            {
                get
                {
                    return this.UndergroundAreaValue != null
                        ? this.UndergroundAreaValue.Value.ToDecimal()
                        : (decimal?)null;
                }
            }
        }
    }
}
