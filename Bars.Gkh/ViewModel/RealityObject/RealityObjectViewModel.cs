namespace Bars.Gkh.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Entities.RealEstateType;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Модель отображения Жилого дома
    /// </summary>
    public class RealityObjectViewModel : BaseViewModel<Entities.RealityObject>
    {
        /// <summary>
        /// Вернуть список
        /// </summary>
        /// <param name="domain">Домен-сервис</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат</returns>
        public override IDataResult List(IDomainService<Entities.RealityObject> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            /* baseParams метода:
              contragentId и typeJurOrg == 10 - то получаем по контрагенту УО и его дома
              dateInspection- дата проверки, чтобы получить дома управляющей организации, в период управления которыми входит дата проверки
              dateStart - дата начала (для получения договоров входящих в интервал)
              dateEnd - дата окончания (для получения договоров входящих в интервал)
              manOrgId - получаем все текущие дома управляющей организации, которые на данный момент находятся в управлении
              realityObjIds - пришла строка с идентификаторами домов которыенеобходимо получить
              existsRoIds - пришла строка с идентификаторами домов которые необходимо исключить
              onlyEmergency - только аварийные
             */

            var onlyEmergency = baseParams.Params.GetAs("onlyEmergency", false);
            var dateInspection = baseParams.Params.GetAs("dateInspection", DateTime.MinValue);
            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);

            var contragentId = baseParams.Params.GetAs("contragentId", 0L);
            var manOrgId = baseParams.Params.GetAs("manOrgId", 0L);
            var typeJurOrg = baseParams.Params.GetAs("typeJurOrg", -1);

            var municipalityId = baseParams.Params.GetAs("municipalityId", 0L);
            var municipalities = baseParams.Params.GetAs("municipalities", string.Empty);
            var settlementId = baseParams.Params.GetAsId("settlementId");

            var ids = baseParams.Params.GetAs("realityObjIds", string.Empty);
            var existsRoIds = baseParams.Params.GetAs<string>("existsRoIds").ToLongArray();

            //идентификатор двора, дома с которым нужно загружать.
            var outdoorId = baseParams.Params.GetAsId("outdoorId");

            var needOnlyWithoutOutdoor = baseParams.Params.GetAs<bool>("needOnlyWithoutOutdoor");

            List<long> checkedMunicipalities = null;

            if (!string.IsNullOrEmpty(municipalities))
            {
                var inspIds = municipalities.Split(';').Select(x => x.ToLong()).ToList();

                checkedMunicipalities = this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                    .Where(x => inspIds.Contains(x.Id))
                    .Select(x => x.Id)
                    .Distinct()
                    .ToList();
            }

            if (ids == "-1")
            {
                return new ListDataResult();
            }

            if (string.IsNullOrEmpty(ids))
            {
                ids = baseParams.Params.ContainsKey("Id") ? baseParams.Params["Id"].ToStr() : string.Empty;
            }

            if (contragentId > 0)
            {
                switch (typeJurOrg)
                {
                    case 0: // если тип юр лица 0, значит нужно получить все дома
                        {
                            // этот пункт нужен для гжи: постановлений прокуратуры, т.к. дома должны выбираться в зависимости от типа исполнителя
                        }

                        break;
                    case 10: // управляющая организация
                        {
                            manOrgId = this.Container.Resolve<IDomainService<ManagingOrganization>>().GetAll()
                                .Where(x => x.Contragent.Id == contragentId)
                                .Select(x => x.Id)
                                .FirstOrDefault();
                        }

                        break;
                    case 20: // Поставщик коммунальных услуг
                        {
                            // Если тип Поставщик коммунальных услуг то нужно получить все дома
                            break;
                        }
                }
            }

            var realityObjIds = this.GetFilterRealtyObjectIds(dateInspection, dateStart, dateEnd, manOrgId, ids);

            var data = domain.GetAll()
                .WhereIf(municipalityId > 0, x => x.Municipality.Id == municipalityId)
                .WhereIf(settlementId > 0, x => x.MoSettlement.Id == settlementId)
                .WhereIf(onlyEmergency, x => x.ConditionHouse == ConditionHouse.Emergency)
                .WhereIf(realityObjIds != null, x => realityObjIds.Contains(x.Id))
                .WhereIf(existsRoIds.Any(), x => !existsRoIds.Contains(x.Id))
                .WhereIf(checkedMunicipalities != null, x => checkedMunicipalities.Contains(x.Municipality.Id))
                .WhereIf(outdoorId != default(long), x => x.Outdoor.Id == outdoorId)
                .WhereIf(needOnlyWithoutOutdoor, x => x.Outdoor == null)
                .Select(
                    x => new
                    {
                        x.Id,
                        Municipality = x.Municipality.Name,
                        Settlement = x.MoSettlement.Name,
                        FullAddress = x.FiasAddress.AddressName,
                        x.Address,
                        x.AreaLiving,
                        x.DateLastOverhaul,
                        x.HeatingSystem,
                        x.AreaMkd,
                        x.TypeHouse,
                        x.Floors,
                        x.NumberEntrances
                    })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Address)
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }

        /// <summary>
        /// Метод получения списка домов с фильтрацией по параметрам
        /// </summary>
        /// <param name="dateInspection">дата проверки</param>
        /// <param name="dateStart">дата начала</param>
        /// <param name="dateEnd">дата окончания</param>
        /// <param name="manOrgId">упр орг ид</param>
        /// <param name="ids">ид</param>
        /// <returns></returns>
        private IEnumerable<long> GetFilterRealtyObjectIds(DateTime dateInspection, DateTime dateStart, DateTime dateEnd, long manOrgId, string ids)
        {
            if (manOrgId > 0)
            {
                var contractIds = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll().
                    Where(x => x.ManOrgContract.ManagingOrganization.Id == manOrgId)
                    .WhereIf(
                        dateInspection != DateTime.MinValue,
                        x =>
                            (x.ManOrgContract.StartDate <= dateInspection && x.ManOrgContract.EndDate >= dateInspection)
                                || (x.ManOrgContract.StartDate <= dateInspection && x.ManOrgContract.EndDate == null)
                                || (x.ManOrgContract.StartDate == null && x.ManOrgContract.EndDate >= dateInspection)
                                || (x.ManOrgContract.StartDate == null && x.ManOrgContract.EndDate == null))
                    .WhereIf(
                        dateStart != DateTime.MinValue && dateEnd != DateTime.MinValue,
                        x =>
                            ((x.ManOrgContract.EndDate <= dateEnd && x.ManOrgContract.EndDate >= dateStart)
                                || (x.ManOrgContract.StartDate <= dateEnd && x.ManOrgContract.StartDate >= dateStart)))
                    .Select(x => x.RealityObject.Id)
                    .ToArray();
            }

            if (ids.Contains(','))
            {
                return ids.Split(',').Select(id => id.ToLong()).ToArray();
            }

            var rid = ids.ToLong();
            return rid > 0 ? new[] { rid } : null;
        }

        /// <summary>
        /// Получить Жилой дом
        /// </summary>
        /// <param name="domainService">Домен-сервис</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат</returns>
        public override IDataResult Get(IDomainService<Entities.RealityObject> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId("Id");

            var rec = domainService.Get(id);

            if (rec == null)
            {
                return new BaseDataResult(null);
            }

            IRealityObjectRealEstateTypeService realObjRealEstTypeService = null;
            if (this.Container.Kernel.HasComponent(typeof(IRealityObjectRealEstateTypeService)))
            {
                realObjRealEstTypeService = this.Container.Resolve<IRealityObjectRealEstateTypeService>();
            }

            BaseRealObjOverhaulDataObject overhaulDataObject = null;
            if (this.Container.Kernel.HasComponent("RealObjOverhaulDataObject"))
            {
                overhaulDataObject = this.Container.Resolve<BaseRealObjOverhaulDataObject>("RealObjOverhaulDataObject");
                overhaulDataObject.Init(rec);
            }

            var typeRoDomain = this.Container.Resolve<IDomainService<RealEstateTypeRealityObject>>();
            var typeDomain = this.Container.Resolve<IDomainService<RealEstateType>>();

            try
            {
                var typeIds = typeRoDomain.GetAll()
                    .Where(y => y.RealityObject.Id == id)
                    .Select(x => x.RealEstateType.Id);

                var realEstateTypeNames = typeDomain.GetAll()
                    .Where(x => typeIds.Contains(x.Id))
                    .AsEnumerable()
                    .AggregateWithSeparator(x => x.Name, ",");

                var isAutoRealEstType = false;
                var roTypes = string.Empty;

                if (realObjRealEstTypeService != null)
                {
                    var res = realObjRealEstTypeService.GetAutoRealEstateType(rec);
                    isAutoRealEstType = res.Success;
                    roTypes = res.Data.ToStr();
                }

                return new BaseDataResult(
                        new
                        {
                            rec.Id,
                            rec.ExternalId,
                            FiasAddress = rec.FiasAddress.GetFiasProxy(this.Container),
                            FiasHauseGuid = rec.FiasAddress.HouseGuid,
                            rec.Address,
                            rec.AreaLiving,
                            rec.AreaLivingOwned,
                            rec.AreaOwned,
                            rec.AreaMunicipalOwned,
                            rec.AreaGovernmentOwned,
                            rec.AreaFederalOwned,
                            rec.Iscluttered,
                            rec.HasVidecam,
                            rec.Longitude,
                            rec.Latitude,
                            rec.AreaCommercialOwned,
                            rec.AreaNotLivingFunctional,
                            rec.AreaLivingNotLivingMkd,
                            rec.AreaMkd,
                            rec.AreaCommonUsage,
                            rec.AreaCleaning,
                            rec.DateLastOverhaul,
                            rec.DateCommissioning,
                            rec.CapitalGroup,
                            rec.CodeErc,
                            rec.DateDemolition,
                            rec.MaximumFloors,
                            rec.RoofingMaterial,
                            rec.WallMaterial,
                            rec.IsInsuredObject,
                            rec.Notation,
                            rec.SeriesHome,
                            rec.TypeProject,
                            rec.HeatingSystem,
                            rec.ConditionHouse,
                            rec.TypeHouse,
                            rec.TypeRoof,
                            rec.FederalNum,
                            rec.PhysicalWear,
                            rec.TypeOwnership,
                            rec.Floors,
                            rec.NumberApartments,
                            rec.NumberEntrances,
                            rec.NumberLifts,
                            rec.NumberLiving,
                            rec.Description,
                            rec.GkhCode,
                            rec.AddressCode,
                            rec.WebCameraUrl,
                            rec.DateTechInspection,
                            rec.ResidentsEvicted,
                            rec.IsBuildSocialMortgage,
                            rec.TotalBuildingVolume,
                            rec.CadastreNumber,
                            rec.CadastralHouseNumber,
                            rec.NecessaryConductCr,
                            rec.FloorHeight,
                            rec.PercentDebt,
                            rec.PrivatizationDateFirstApartment,
                            rec.HasPrivatizedFlats,
                            rec.BuildYear,
                            rec.State,
                            rec.EnergyPassport,
                            rec.MethodFormFundCr,
                            rec.HasJudgmentCommonProp,
                            rec.IsRepairInadvisable,
                            rec.IsNotInvolvedCr,
                            rec.ProjectDocs,
                            rec.ConfirmWorkDocs,
                            IsAutoRealEstType = isAutoRealEstType,
                            AutoRealEstType = roTypes,
                            rec.RealEstateType,
                            rec.UnomCode,
                            rec.District,
                            overhaulDataObject?.PublishDate,
                            UnpublishDate = overhaulDataObject?.UnpublishDate ?? rec.UnpublishDate,
                            rec.VtscpCode,
                            rec.Points,
                            rec.NumberNonResidentialPremises,
                            rec.AreaNotLivingPremises,
                            RealEstateTypeNames = realEstateTypeNames,
                            rec.ObjectConstruction,
                            rec.BuiltOnResettlementProgram,
                            rec.IsCulturalHeritage,
                            rec.IsInvolvedCrTo2,
                            rec.LatestTechnicalMonitoring,
                            rec.CulturalHeritageAssignmentDate,
                            rec.CentralHeatingStation,
                            rec.NumberInCtp,
                            rec.PriorityCtp,
                            AddressCtp = rec.CentralHeatingStation?.Address.AddressName,
                            rec.Municipality,
                            Settlement = rec.MoSettlement,
                            rec.MonumentDocumentNumber,
                            rec.MonumentFile,
                            rec.MonumentDepartmentName,
                            rec.DateCommissioningLastSection,
                            rec.TechPassportFile,
                            rec.IsSubProgram,
                            rec.IncludeToSubProgram,
                            rec.ASSberbankClient,
                            rec.HasCharges185FZ,
                            rec.ExportedToPortal,
                            rec.FileInfo,
                            rec.IsIncludedRegisterCHO,
                            rec.IsIncludedListIdentifiedCHO,
                            rec.IsDeterminedSubjectProtectionCHO,
                            overhaulDataObject?.DocumentNumber,
                            overhaulDataObject?.DocumentDate,
                            overhaulDataObject?.ObligationDate
                        });
            }
            finally
            {
                this.Container.Release(realObjRealEstTypeService);
                this.Container.Release(overhaulDataObject);
                this.Container.Release(typeRoDomain);
                this.Container.Release(typeDomain);
            }
        }
    }
}