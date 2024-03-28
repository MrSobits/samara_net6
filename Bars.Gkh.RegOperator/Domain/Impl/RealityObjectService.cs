namespace Bars.Gkh.RegOperator.Domain.Impl
{
    using System;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Decisions.Nso.Domain;
    using Decisions.Nso.Entities;
    using Entities;
    using Enums;
    using Gkh.Domain;
    using Gkh.Entities;
    using Gkh.Enums;
    using Gkh.Enums.Decisions;
    using Gkh.Utils;

    using Bars.Gkh.DomainService;
    using System.Collections.Generic;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Сервис для работы с МКД
    /// </summary>
    public class RealityObjectService : Gkh.DomainService.RealityObjectService
    {
        /// <inheritdoc />
        public override IDataResult ListView(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            
            var query = this.GetRealityObjectsQuery(baseParams);
            return new ListDataResult(query.Paging(loadParam).ToList(), query.Count());
        }

        public override List<long> GetAvaliableRoIds(List<long> contragentList)
        {
            var bkDomain = this.Container.Resolve<IDomainService<BuildContract>>();

            var list = bkDomain.GetAll()
                .Where(x => contragentList.Contains(x.Builder.Contragent.Id))
                .Select(x => x.ObjectCr.RealityObject.Id).Distinct().ToList();          
            return list;
        }

        /// <inheritdoc />
        public override IDataResult ListByLocalityGuid(BaseParams baseParams)
        {
            var roDomain = this.Container.Resolve<IDomainService<RealityObject>>();
            var roDecisionService = this.Container.Resolve<IRealityObjectDecisionsService>();
            var accOwnerDecDomain = this.Container.ResolveDomain<AccountOwnerDecision>();

            try
            {
                var loadParams = baseParams.GetLoadParam();

                var localityGuid = baseParams.Params.GetAs<string>("localityGuid");
                var muId = baseParams.Params.GetAs<long>("muId");
                var settlementId = baseParams.Params.GetAs<long>("settlementId");
                var fundForm = baseParams.Params.GetAs<FundFormationType?>("fundForm");
                var dateEnd = baseParams.Params.GetAs<DateTime?>("dateEnd");

                var filterRoQuery = roDomain.GetAll()
                    .WhereIf(!string.IsNullOrEmpty(localityGuid), x => x.FiasAddress.PlaceGuidId == localityGuid)
                    .WhereIf(muId > 0, x => x.Municipality.Id == muId)
                    .WhereIf(settlementId > 0, x => x.MoSettlement.Id == settlementId);

                long[] roIds = null;

                if (fundForm != null)
                {
                    var filterRoIds = filterRoQuery.Select(x => x.Id).ToArray();

                    var roDecisionQuery = roDecisionService.GetRobjectsFundFormation(((IQueryable<long>)null))
                        .WhereIf(filterRoIds.Any(), x => filterRoIds.Contains(x.Key))
                        .WhereIf(dateEnd.HasValue, x => x.Value.Any(y => !dateEnd.HasValue || y.Item1 <= dateEnd.Value));

                    var accOwnerDecisDict = accOwnerDecDomain.GetAll()
                        .Where(x => filterRoQuery.Any(y => y.Id == x.Protocol.RealityObject.Id))
                        .WhereIf(dateEnd.HasValue, x => x.StartDate <= dateEnd)
                        .OrderByDescending(x => x.StartDate)
                        .Select(x => new
                        {
                            x.Protocol.RealityObject.Id,
                            x.DecisionType
                        })
                        .ToList()
                        .GroupBy(x => x.DecisionType)
                        .ToDictionary(x => x.Key, x => x.Select(y => y.Id));

                    switch (fundForm)
                    {
                        case FundFormationType.Regop:
                            roIds = roDecisionQuery
                                .Where(x => x.Value.OrderByDescending(y => y.Item1)
                                    .First(y => !dateEnd.HasValue || y.Item1 <= dateEnd.Value)
                                    .Item2 == CrFundFormationDecisionType.RegOpAccount)
                                .Select(x => x.Key)
                                .ToArray();
                            break;

                        case FundFormationType.SpecRegop:
                            roIds = roDecisionQuery
                                .Where(x => x.Value.OrderByDescending(y => y.Item1)
                                    .First(y => !dateEnd.HasValue || y.Item1 <= dateEnd.Value)
                                    .Item2 == CrFundFormationDecisionType.SpecialAccount)
                                .Select(x => x.Key)
                                .Intersect(accOwnerDecisDict.ContainsKey(AccountOwnerDecisionType.RegOp)
                                    ? accOwnerDecisDict[AccountOwnerDecisionType.RegOp]
                                    : new long[]{})
                                .ToArray();
                            break;

                        case FundFormationType.Special:
                            roIds = roDecisionQuery
                                .Where(x => x.Value.OrderByDescending(y => y.Item1)
                                    .First(y => !dateEnd.HasValue || y.Item1 <= dateEnd.Value)
                                    .Item2 == CrFundFormationDecisionType.SpecialAccount)
                                .Select(x => x.Key)
                                .Intersect(accOwnerDecisDict.ContainsKey(AccountOwnerDecisionType.Custom)
                                    ? accOwnerDecisDict[AccountOwnerDecisionType.Custom].ToArray()
                                    : new long[] { })
                                .ToArray();
                            break;
                    }
                }

                var data = filterRoQuery
                    .WhereIf(roIds != null, x => roIds.Contains(x.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.Address,
                        Municipality = x.Municipality.Name
                    })
                    .Filter(loadParams, this.Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(roDomain);
                this.Container.Release(roDecisionService);
            }
        }

        /// <inheritdoc />
        public override IDataResult ListByMoSettlement(BaseParams baseParams)
        {
            var roDomain = this.Container.Resolve<IDomainService<RealityObject>>();
            var roDecisionService = this.Container.Resolve<IRealityObjectDecisionsService>();

            try
            {

                var loadParams = baseParams.GetLoadParam();

                var roIds = baseParams.Params.GetAs("Id", string.Empty).ToLongArray();

                var settlementIds = baseParams.Params.GetAs("settlementId", string.Empty).ToLongArray();

                var isDecisionRegOp = baseParams.Params.GetAs("isDecisionRegOp", false); // Если еободимо достать дома по решениям на счете рег опа

                var roQuery =
                    roDomain.GetAll()
                            .WhereIf(roIds.Any() && roIds.First() != 0, x => roIds.Contains(x.Id))
                            .WhereIf(
                                settlementIds.Any() && settlementIds.First() != 0,
                                x =>
                                settlementIds.Contains(x.Municipality.Id)
                                || settlementIds.Contains(x.MoSettlement.Id)
                                || (x.MoSettlement.ParentMo != null
                                    && settlementIds.Contains(x.MoSettlement.ParentMo.Id)));

                if (isDecisionRegOp)
                {
                    var decisionsDict = roDecisionService.GetActualDecisionForCollection<CrFundFormationDecision>(roQuery, true);

                    var regopDecRoIds = decisionsDict.Where(x => x.Value.Decision == CrFundFormationDecisionType.RegOpAccount)
                        .Select(x => x.Key.Id)
                        .ToDictionary(x => x);

                    var roDecIds = regopDecRoIds.Keys.ToList();

                    roQuery = roQuery.WhereIf(roDecIds.Count > 0, x => roDecIds.Contains(x.Id));
                }

                var data = roQuery
                    .Select(x => new { x.Id, Name = x.Address })
                    .Filter(loadParams, this.Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                this.Container.Release(roDomain);
                this.Container.Release(roDecisionService);
            }
        }

        /// <summary>
        /// Получение листа жилых домов
        /// </summary>
        /// <param name="baseParams">Базовые параметры </param>
        /// <returns>Лист жилых домов</returns>
        public override IQueryable<RealityObjectProxy> GetRealityObjectsQuery(BaseParams baseParams)
        {
            var roDomain = this.Container.ResolveDomain<RealityObject>();
            var delAgentsRoDomain = this.Container.ResolveDomain<DeliveryAgentRealObj>();

            try
            {
                var loadParam = baseParams.GetLoadParam();

                // Если true показываем снесенные дома
                var filterShowDemolished = baseParams.Params.GetAs<bool>("filterShowDemolished");

                // Если true показываем индивидуальные дома
                var filterShowIndividual = baseParams.Params.GetAs<bool>("filterShowIndividual");

                // Если true показываем дома блокированной застройки
                var filterShowBlockedBuilding = baseParams.Params.GetAs<bool>("filterShowBlockedBuilding");

                // Если true показываем только те дома, которые не привязаны ни к одному агенту доставки
                var filterShowWoDeliveryAgent = baseParams.Params.GetAs("filterShowWoDeliveryAgent", false);

                var deliveryAgentSelectChange = baseParams.Params.GetAs<long>("deliveryAgentSelectChange");

                var moIds = baseParams.Params.GetAs<string[]>("moIds");

                var deliveryAgentRoIds =
                    delAgentsRoDomain.GetAll()
                        .Where(x => x.RealityObject != null)
                        .Where(x => x.DeliveryAgent.Id == deliveryAgentSelectChange)
                        .Select(x => x.RealityObject.Id)
                        .ToArray();

                //Данный костыль добавляю потому как Пользователи при фильтрации иногда забывают (или не хотят) вбивать пробелы
                //Но при этом хотят чтобы их значения находились
                //соответственно я получаю Фильтр по Адресу и проверяю наличие данных как по Переданному фильтру, так и с удалением пробелов
                var complexFilter = loadParam.FindInComplexFilter("Address");

                string filterForAddress = null;

                if (complexFilter != null)
                {
                    filterForAddress = complexFilter.Value.ToStr();
                    loadParam.SetComplexFilterNull("Address");
                }

                var query =
                    roDomain.GetAll()
                        .WhereIf(deliveryAgentRoIds.Any(), x => deliveryAgentRoIds.Contains(x.Id))
                        .WhereIf(
                            filterShowWoDeliveryAgent,
                            x => !delAgentsRoDomain.GetAll().Where(y => y.DeliveryAgent != null).Any(y => y.RealityObject.Id == x.Id))
                        .WhereIf(!filterShowBlockedBuilding, x => x.TypeHouse != TypeHouse.BlockedBuilding)
                        .WhereIf(!filterShowIndividual, x => x.TypeHouse != TypeHouse.Individual)
                        .WhereIf(!filterShowDemolished, x => x.ConditionHouse != ConditionHouse.Razed)
                        .WhereIf(
                            !filterForAddress.IsEmpty(),
                            x =>
                                x.Address != null
                                && (x.Address.ToUpper().Contains(filterForAddress.ToUpper())
                                    || x.Address.Replace(" ", "").ToUpper().Contains(filterForAddress.Replace(" ", "").ToUpper())))
                        .WhereIf(moIds != null && moIds.Any(), x => moIds.Contains(x.FiasAddress.PlaceName))
                        .Select(x => new RealityObjectProxy
                        {
                            Id = x.Id,
                            ExternalId = x.ExternalId,
                            Municipality = x.Municipality.ParentMo == null ? x.Municipality.Name : x.Municipality.ParentMo.Name,
                            Settlement =
                                x.MoSettlement != null ? x.MoSettlement.Name : (x.Municipality.ParentMo != null ? x.Municipality.Name : ""),
                            PlaceName = x.FiasAddress.PlaceName,
                            Address = x.Address,
                            FiasHauseGuid = x.FiasAddress.HouseGuid.ToString(),
                            FullAddress = x.FiasAddress.AddressName,
                            TypeHouse = x.TypeHouse,
                            ConditionHouse = x.ConditionHouse,
                            IsCulturalHeritage = x.IsCulturalHeritage,
                            DateDemolition = x.DateDemolition,
                            Floors = x.MaximumFloors,
                            NumberEntrances = x.NumberEntrances,
                            NumberLiving = x.NumberLiving,
                            NumberApartments = x.NumberApartments,
                            AreaLivingNotLivingMkd = x.AreaLivingNotLivingMkd,
                            AreaMkd = x.AreaMkd,
                            AreaLiving = x.AreaLiving,
                            PhysicalWear = x.PhysicalWear,
                            NumberLifts = x.NumberLifts,
                            HeatingSystem = x.HeatingSystem,
                            WallMaterialName = x.WallMaterial.Name,
                            RoofingMaterialName = x.RoofingMaterial.Name,
                            TypeRoof = x.TypeRoof,
                            DateLastOverhaul = x.DateLastOverhaul,
                            DateCommissioning = x.DateCommissioning,
                            CapitalGroup = x.CapitalGroup.Name,
                            ManOrgNames = x.ManOrgs,
                            TypeContracts = x.TypesContract,
                            CodeErc = x.CodeErc,
                            IsInsuredObject = x.IsInsuredObject,
                            GkhCode = x.GkhCode,
                            IsBuildSocialMortgage = x.IsBuildSocialMortgage,
                            State = x.State,
                            IsRepairInadvisable = (bool?)x.IsRepairInadvisable,
                            IsNotInvolvedCr = x.IsNotInvolvedCr,
                            IsInvolvedCrTo2 = x.IsInvolvedCrTo2,
                            District = x.District,
                            TotalBuildingVolume = x.TotalBuildingVolume,
                            BuildYear = x.BuildYear,
                            PrivatizationDateFirstApartment = x.PrivatizationDateFirstApartment,
                            AccountFormationVariant = x.AccountFormationVariant,
                            HouseGuid = x.HouseGuid,
                            HasVidecam = x.HasVidecam,
                            Inn = x.InnManOrgs,
                            StartControlDate = x.StartControlDate,
                            ObjectConstruction = x.ObjectConstruction
                        })
                        .Filter(loadParam, this.Container)
                        .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                        .OrderThenIf(loadParam.Order.Length == 0, true, x => x.Address)
                        .Order(loadParam);

                return query;
            }
            finally
            {
                this.Container.Release(roDomain);
                this.Container.Release(delAgentsRoDomain);
            }
        }
    }
}