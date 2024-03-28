namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Caching.LinqExtensions;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.ConfigSections.RegOperator.Enums;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Dto;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Tasks.PaymentDocuments;
    using Bars.Gkh.Utils;
    using Castle.Windsor;
    using Newtonsoft.Json;
    using System.Diagnostics;
    using System.ComponentModel;
    using Bars.Gkh.RegOperator.DomainModelServices.Impl;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Enums.Decisions;

    /// <summary>
    /// Класс предназначен для получения иерархии документов на оплату
    /// </summary>
    public class PaymentDocPreviewHelper
    {
        private readonly IWindsorContainer container;
        private readonly PaymentDocumentConfigContainer config;
        private const string NoDeliveryAgent = "Без агента доставки";
        private readonly OrgFormGroup orgFormGroup;
        private readonly GroupingForLegalWithOneOpenRoom groupingForLegalWithOneOpenRoom;

        private string PhisicalString
        {
            get { return this.OptimizePath ? "ФЛ" : "Физические лица"; }
        }

        private string LegalString
        {
            get { return this.OptimizePath ? "ЮЛ" : "Юридические лица"; }
        }

        private string LegalStringOF
        {
            get { return this.OptimizePath ? "ЮЛ. Организационная-правовая форма" : "Юридические лица. Организационная-правовая форма"; }
        }

        /// <summary>
        /// Использовать сокращения в путях. Используется при выгрузке документов, но не при просмотре.
        /// </summary>
        public bool OptimizePath { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        public PaymentDocPreviewHelper(IWindsorContainer container)
        {
            this.container = container;
            this.config = this.container.GetGkhConfig<RegOperatorConfig>().PaymentDocumentConfigContainer;
            this.orgFormGroup = this.config.PaymentDocumentConfigLegal.OrgFormGroup;
            this.groupingForLegalWithOneOpenRoom = this.config.PaymentDocumentConfigLegal.GroupingForLegalWithOneOpenRoom;
        }

        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Строит дерево иерархии документов
        /// </summary>
        /// <param name="accountIds">Список счетов</param>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Дерево счетов</returns>
        /// <example>
        /// Вывод примерно такой 
        /// -Муниципальный район
        /// ----Улица
        /// ------- 1-20 (Группировка по кол-ву ЛС в одном файле)
        /// ---------- ЛС
        /// </example>
        // TODO: метод должен принимать на вход IQueryable<BillingPersonalAccountDto>
        internal TreeNode GetPreview(IEnumerable<long> accountIds, BaseParams baseParams)
        {
            int count;
            return this.GetPreview(accountIds, baseParams, out count);
        }

        internal TreeNode GetPreview(IEnumerable<long> accountIds, BaseParams baseParams, out int count)
        {
            var accountSvc = container.Resolve<IPersonalAccountFilterService>();
            var accountDomain = container.ResolveDomain<BasePersonalAccount>();

            using (container.Using(accountSvc, accountDomain))
            {
                //формируем запрос на выборку BasePersonalAccount по выбранным ЛС
                var accountsQuery = accountDomain.GetAll().WhereIf(accountIds.Any(), x => accountIds.Contains(x.Id));

                //применяем настройки печати
                var accountsDtoQuery = accountSvc.GetQueryableByFiltersAndBillingAddress(baseParams, accountsQuery);

                var root = TreeNode.CreateRoot("root", null);

                //добавляем ветки физиков
                count = this.CreatePhysicalNodes(root, accountsDtoQuery);

                //добавляем ветки юриков
                count += this.CreateLegalsNodes(root, accountsDtoQuery);

                return root;
            }
        }

        /// <summary>
        /// Класс, который содержит в себе вытащенные из параметра запроса
        /// </summary>
        private class BaseParamsAccountFilter
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="baseParams">Параметры запроса</param>
            public BaseParamsAccountFilter(BaseParams baseParams)
            {
                this.LoadParam = baseParams.GetLoadParam();

                this.OwnerIdSpecified = baseParams.Params.ContainsKey("ownerId");
                this.OwnerId = baseParams.Params.GetAsId("ownerId");
                this.PeriodId = baseParams.Params.GetAsId("periodId");
                this.FilterByPeriod = baseParams.Params.GetAs<bool>("filterByPeriod");
                this.FromOwner = baseParams.Params.GetAs<bool>("fromOwner", ignoreCase: true);

                if (baseParams.Params.ContainsKey("crFoundType") &&
                    baseParams.Params.GetAs<string>("crFoundType").IsNotEmpty())
                {
                    this.CrFundTypeList =
                        baseParams.Params.GetAs<object>("crFoundType")
                            .CastAs<List<object>>()
                            .Cast<DynamicDictionary>()
                            .Select(
                                decType =>
                                    (CrFundFormationType)
                                    Enum.Parse(typeof(CrFundFormationType), decType.GetAs<string>("Id")))
                            .ToList();
                }

                this.ShowAll = this.LoadParam.Filter.GetAs<bool>("showAll", ignoreCase: true) || baseParams.Params.GetAs<bool>("showAll", ignoreCase: true);

                this.CashPaymentCenterId = baseParams.Params.GetAsId("cashPaymentCenterId");

                this.PrivilegedCategory = new PrivilegedCategoryFilter(baseParams);

                this.RoIdSpecified = baseParams.Params.ContainsKey("roId");
                this.RoId = baseParams.Params.GetAsId("roId");

                this.BankAccNumSpecified = baseParams.Params.GetAs<bool>("bankAccNum");
                this.RoomId = baseParams.Params.GetAsId("RoomId");

                if (baseParams.Params.ContainsKey("crOwnerTypeValues") && baseParams.Params.GetAs<string>("crOwnerTypeValues").IsNotEmpty())
                {
                    this.CrOwnerTypeList =
                        baseParams.Params.GetAs<object>("crOwnerTypeValues")
                            .CastAs<List<object>>()
                            .Cast<DynamicDictionary>()
                            .Select(decType => (CrOwnerFilterType)Enum.Parse(typeof(CrOwnerFilterType), decType.GetAs<string>("Id")))
                            .ToList();
                }

                if (baseParams.Params.ContainsKey("showAllGroups"))
                {
                    this.ShowInAllGroups = baseParams.Params.GetAs<bool>("showAllGroups");
                }

                if (baseParams.Params.ContainsKey("groupIds") && baseParams.Params.GetAs<string>("groupIds").IsNotEmpty())
                {
                    this.GroupIds = baseParams.Params.GetAs<List<long>>("groupIds");
                }

                this.DeliveryAgentIds = baseParams.Params.GetAs<long[]>("deliveryAgentIds");
                this.DeliveryAgentShowAll = baseParams.Params.GetAs<bool>("deliveryAgentShowAll");
                this.HasNotDeliveryAgent = this.DeliveryAgentIds.IsNotEmpty() && this.DeliveryAgentIds.Contains(-1);
                if (this.HasNotDeliveryAgent)
                {
                    this.DeliveryAgentIds = this.DeliveryAgentIds.Where(x => x != -1).ToArray();
                }


                this.OwnershipTypeValues = baseParams.Params.GetAs<RoomOwnershipType[]>("ownershipTypeValues");

                this.HasChargesValues = baseParams.Params.GetAs<AccountFilterHasCharges[]>("hasChargesValues");
                if (this.HasChargesValues.IsNotEmpty())
                {
                    this.HasBaseTariffCharge = this.HasChargesValues.Contains(AccountFilterHasCharges.BaseTariffCharge);
                    this.HasDecisionTariffCharge = this.HasChargesValues.Contains(AccountFilterHasCharges.DecisionTariffCharge);
                    this.HasPenaltyCharge = this.HasChargesValues.Contains(AccountFilterHasCharges.PenaltyCharge);
                    this.HasPenaltyZeroCharge = this.HasChargesValues.Contains(AccountFilterHasCharges.PenaltyZeroCharge);
                    this.HasOverpayment = this.HasChargesValues.Contains(AccountFilterHasCharges.Overpayment);
                    this.HasBaseTariffZeroCharge = this.HasChargesValues.Contains(AccountFilterHasCharges.BaseTariffZeroCharge);
                    this.HasBaseTariffOverpayment = this.HasChargesValues.Contains(AccountFilterHasCharges.BaseTariffOverpayment);
                    this.HasDecisionTariffOverpayment = this.HasChargesValues.Contains(AccountFilterHasCharges.DecisionTariffOverpayment);
                    this.HasPenaltyOverpayment = this.HasChargesValues.Contains(AccountFilterHasCharges.PenaltyOverpayment);
                    this.HasBaseTariffDebt = this.HasChargesValues.Contains(AccountFilterHasCharges.BaseTariffDebt);
                    this.HasDecisionDebt = this.HasChargesValues.Contains(AccountFilterHasCharges.DecisionDebt);
                    this.HasPenaltyDebt = this.HasChargesValues.Contains(AccountFilterHasCharges.PenaltyDebt);
                }

                this.AccountRegistryMode = baseParams.Params.GetAs<AccountRegistryMode>("mode");
            }

            public bool FilterByPeriod { get; private set; }

            public bool OwnerIdSpecified { get; private set; }

            public long RoId { get; private set; }

            public long PeriodId { get; private set; }

            public bool RoIdSpecified { get; private set; }

            public bool BankAccNumSpecified { get; private set; }

            public LoadParam LoadParam { get; set; }

            public long OwnerId { get; private set; }

            public bool FromOwner { get; private set; }

            public long CashPaymentCenterId { get; private set; }

            public bool ShowAll { get; private set; }

            public PrivilegedCategoryFilter PrivilegedCategory { get; private set; }

            public List<CrFundFormationType> CrFundTypeList { get; private set; }

            public long RoomId { get; private set; }

            public List<CrOwnerFilterType> CrOwnerTypeList { get; private set; }

            public List<long> GroupIds { get; private set; }

            public bool ShowInAllGroups { get; private set; }

            public long[] DeliveryAgentIds { get; private set; }

            public bool DeliveryAgentShowAll { get; private set; }

            public bool HasNotDeliveryAgent { get; private set; }

            public RoomOwnershipType[] OwnershipTypeValues { get; private set; }

            public AccountFilterHasCharges[] HasChargesValues { get; private set; }

            public bool HasBaseTariffCharge { get; private set; }

            public bool HasDecisionTariffCharge { get; private set; }

            public bool HasPenaltyCharge { get; private set; }

            public bool HasPenaltyZeroCharge { get; private set; }

            public bool HasOverpayment { get; private set; }

            public bool HasBaseTariffZeroCharge { get; private set; }

            public bool HasBaseTariffOverpayment { get; private set; }

            public bool HasDecisionTariffOverpayment { get; private set; }

            public bool HasPenaltyOverpayment { get; private set; }

            public bool HasBaseTariffDebt { get; private set; }

            public bool HasDecisionDebt { get; private set; }

            public bool HasPenaltyDebt { get; private set; }

            public AccountRegistryMode AccountRegistryMode { get; private set; }
        }

        private int CreatePhysicalNodes(TreeNode node, IQueryable<BillingPersonalAccountDto> baseQuery)
        {
            var deliveryAgentRoDmn = this.container.ResolveDomain<DeliveryAgentRealObj>();
            var individualAccountOwnerDmn = this.container.ResolveDomain<IndividualAccountOwner>();
            var fiasRepo = this.container.ResolveRepository<Fias>();
            var groRepo = this.container.ResolveRepository<RealityObject>();
            var fiasAdrRepo = this.container.ResolveRepository<FiasAddress>();
            var ownershipHistoryDomain = this.container.ResolveDomain<AccountOwnershipHistory>();

            var result = 0;
            using (this.container.Using(deliveryAgentRoDmn, fiasRepo, ownershipHistoryDomain))
            {
                //var filtered = baseQuery.Where(this.CreateExpressionForIndividual(this.groupingForLegalWithOneOpenRoom)).ToList();
                var filtered = baseQuery.ToList();

                if (groupingForLegalWithOneOpenRoom == GroupingForLegalWithOneOpenRoom.IndividualFolder)
                {
                    filtered = filtered.Where(x => x.OwnerType == PersonalAccountOwnerType.Individual || x.HasOnlyOneRoomWithOpenState).ToList();
                }
                else
                {
                    filtered = filtered.Where(x => x.OwnerType == PersonalAccountOwnerType.Individual).ToList();
                }

                //var filtered = baseQuery.Where(this.CreateExpressionForIndividual(this.groupingForLegalWithOneOpenRoom)).ToArray();

                result = filtered.Count();
                Stopwatch sw1 = new Stopwatch();
                Stopwatch sw2 = new Stopwatch();
                Stopwatch sw3 = new Stopwatch();
                Stopwatch sw4 = new Stopwatch();
                Stopwatch sw5 = new Stopwatch();


                try
                {
                    sw1.Start();
                    
                    //var periodRepo = this.container.ResolveRepository<Period>();

                    //var printedperiod = periodRepo.GetAll()
                    //    .Where(x => x.DateEnd != null)
                    //    .OrderByDescending(x => x.Id).FirstOrDefault();

                    #region материализация истории принадлежности ЛС

                    var persAccOwnerDict2 = ownershipHistoryDomain.GetAll()
                        .Where(x => baseQuery.Any(y => y.Id == x.PersonalAccount.Id && y.OwnerId == x.AccountOwner.Id))
                        .Select(x => new
                        {
                            x.PersonalAccount.Id,
                            hisId = x.Id,
                            x.Date,
                            AccountOwner = (x.AccountOwner as IndividualAccountOwner)
                        })
                        .Where(x => x.AccountOwner.RealityObject != null || x.AccountOwner.FiasFactAddress != null)
                        .Select(x => new
                        {
                            AccountId = x.Id,
                            x.Date,
                            x.hisId,
                            RealityObjectID = x.AccountOwner.RealityObject != null ? x.AccountOwner.RealityObject.Id : 0,
                            FiasFactAddressId = x.AccountOwner.FiasFactAddress != null ? x.AccountOwner.FiasFactAddress.Id : 0
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.AccountId).ToDictionary(
                            x => x.Key,
                            x => x.OrderByDescending(y => y.Date).ThenByDescending(y => y.hisId)
                                .Select(
                                    y => new
                                    {
                                        y.RealityObjectID,
                                        y.FiasFactAddressId
                                    })
                                .First());
                    sw1.Stop();
                    var listIdsRo = persAccOwnerDict2.Select(x => x.Value.RealityObjectID).ToList();
                    var listIdsF = persAccOwnerDict2.Select(x => x.Value.FiasFactAddressId).ToList();

                    sw4.Start();

                    //Dictionary<long, RealityObject> groDict 
                    var roDict = groRepo.GetAll()
                        .Where(x => listIdsRo.Contains(x.Id))
                        .Select(x => new
                        {
                            x.Id,
                            RealityObject = new RealityObject
                            {
                                Id = x.Id,
                                Municipality = x.Municipality,
                                MoSettlement = x.MoSettlement
                            }

                        })
                        .ToDictionary(
                            x => x.Id,
                            x => x.RealityObject);

                    var fiasFA = fiasAdrRepo.GetAll()
                        .Where(x => listIdsF.Contains(x.Id))
                        .ToDictionary(
                            x => x.Id,
                            x => fiasAdrRepo.Get(x.Id));
                    sw4.Stop();

                    sw2.Start();
                    filtered.ForEach(
                        x =>
                        {
                            x.IndividualFactAddressRo = persAccOwnerDict2.ContainsKey(x.Id) && roDict.ContainsKey(persAccOwnerDict2[x.Id].RealityObjectID)
                                ? roDict[persAccOwnerDict2[x.Id].RealityObjectID]
                                : null;
                            x.IndividualFactAddress = persAccOwnerDict2.ContainsKey(x.Id) && fiasFA.ContainsKey(persAccOwnerDict2[x.Id].FiasFactAddressId)
                                ? fiasFA[persAccOwnerDict2[x.Id].FiasFactAddressId]
                                : null;

                            x.ApplyAddressParts();
                            x.ApplyIndividialFactRoParts();
                            x.ApplyIndividialFactFiasParts();
                        });

                    sw2.Stop();
                    #endregion

                    /*
                                        #region trash
                                        //Nh не дает получить эти данные сразу (fetch) в подзапросе основного запроса, по крайнем мере пока такой способ не найден
                                        //поэтому получаем данные scope'ом и присоединяем после, чтобы не стрелять в базу на каждой итерации
                                        var persAccOwnerDict = ownershipHistoryDomain.GetAll()
                                            .Where(x => baseQuery.Any(y => y.Id == x.PersonalAccount.Id))
                                            .Select(x => new
                                            {
                                                x.PersonalAccount.Id,
                                                x.Date,
                                                AccountOwner = (x.AccountOwner as IndividualAccountOwner)
                                            })
                                            .Select(x => new
                                            {
                                                AccountId = x.Id,
                                                x.Date,
                                                RealityObject = x.AccountOwner.RealityObject != null ? new RealityObject
                                                {
                                                    Id = x.AccountOwner.RealityObject.Id,
                                                    Municipality = x.AccountOwner.RealityObject.Municipality,
                                                    MoSettlement = x.AccountOwner.RealityObject.MoSettlement
                                                } : null,
                                                x.AccountOwner.FiasFactAddress
                                            })
                                            .AsEnumerable()
                                            .GroupBy(x => x.AccountId)
                                            .ToDictionary(
                                                x => x.Key,
                                                x => x.OrderByDescending(y => y.Date)
                                                    .Select(
                                                        y => new
                                                        {
                                                            y.RealityObject,
                                                            y.FiasFactAddress
                                                        })
                                                    .First());

                                        filtered.ForEach(
                                            x =>
                                            {
                                                x.IndividualFactAddressRo = persAccOwnerDict.ContainsKey(x.Id) ? persAccOwnerDict[x.Id].RealityObject : null;
                                                x.IndividualFactAddress = persAccOwnerDict.ContainsKey(x.Id) ? persAccOwnerDict[x.Id].FiasFactAddress : null;

                                                x.ApplyAddressParts();
                                                x.ApplyIndividialFactRoParts();
                                                x.ApplyIndividialFactFiasParts();
                                            });
                                        #endregion

                                        */
                    sw3.Start();
                    var grouped = filtered.GroupBy(x => x.BillingAddressType);
                    var individualFactAddressRoIds = filtered.Select(a => a.IndividualFactAddressRoId).ToArray();

                    var fiasFactAddresGuids = filtered
                        .Where(a => a.IndividualFactAddressRaionGuid != null)
                        .Where(a => a.IndividualFactAddressRaionGuid != "")
                        .Select(a => a.IndividualFactAddressRaionGuid)
                        .ToArray();

                    var fiasRaionDict = fiasRepo.GetAll()
                        .Where(x => x.AOLevel == FiasLevelEnum.Raion)
                        .Where(x => fiasFactAddresGuids.Contains(x.AOGuid))
                        .Select(x => new
                        {
                            x.AOGuid,
                            Name = x.OffName + " " + x.ShortName
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.AOGuid)
                        .ToDictionary(x => x.Key, x => x.First().Name);

                    var deliveryAgentByRo = deliveryAgentRoDmn.GetAll()
                        .Where(x => baseQuery.Any(a => a.RoId == x.RealityObject.Id) || individualFactAddressRoIds.Contains(x.RealityObject.Id))
                        .Select(x => new { RoId = x.RealityObject.Id, DeliveryAgentName = x.DeliveryAgent.Contragent.ShortName })
                        .AsEnumerable()
                        .GroupBy(x => x.RoId)
                        .ToDictionary(x => x.Key, y => y.First().DeliveryAgentName);

                    foreach (var group in grouped)
                    {
                        var billingAddressType = group.Key;
                        var nodeName = this.GetNodeName(billingAddressType, true, false);
                        var child = node.AddChild(nodeName, nodeName);
                        child.IsPhysical = true;
                        var groupers = this.GetGroupers(billingAddressType);
                        this.CreateGroup(child, groupers, group, deliveryAgentByRo, fiasRaionDict);
                    }

                    sw3.Stop();
                }

                catch (Exception e)
                {
                    var v = e.ToString();
                    return result;

                }
            }

            return result;
        }

        private Expression<Func<BillingPersonalAccountDto, bool>> CreateExpressionForIndividual(GroupingForLegalWithOneOpenRoom groupingForLegalWithOneOpenRoom)
        {
            switch (groupingForLegalWithOneOpenRoom)
            {
                case GroupingForLegalWithOneOpenRoom.LegalFolder:
                    return dto => dto.OwnerType == PersonalAccountOwnerType.Individual;
                case GroupingForLegalWithOneOpenRoom.IndividualFolder:
                    return
                        dto =>
                            dto.OwnerType == PersonalAccountOwnerType.Individual
                            || (dto.OwnerType == PersonalAccountOwnerType.Legal && dto.HasOnlyOneRoomWithOpenState);
            }

            return dto => true;
        }

        private int CreateLegalsNodes(TreeNode node, IQueryable<BillingPersonalAccountDto> baseQuery)
        {
            var result = 0;
            var legalAccDmn = this.container.ResolveDomain<LegalAccountOwner>();
            var calcaccService = this.container.Resolve<ICalcAccountService>();
            var calcaccByRo = calcaccService.GetRobjectsAccounts(baseQuery.Select(x => x.RoId));
            var allowAccWithoutCharge = this.container.GetGkhConfig<RegOperatorConfig>().PaymentDocumentConfigContainer.PaymentDocumentConfigLegal.AllowAccWithoutCharge;

            var stateCodeNoActive = "4";

            var filtered = baseQuery
                .Where(x=> x.OwnerType == PersonalAccountOwnerType.Legal)
                .Where(this.CreateExpressionForLegal(this.groupingForLegalWithOneOpenRoom))
                .Where(this.CreateChargeExpressionForLegal(allowAccWithoutCharge))
                .Where(x => x.State.Code != stateCodeNoActive) 
                //.Select(x=> new BillingPersonalAccountDto
                //{
                //    AccountFormationVariant = x.AccountFormationVariant,
                //    AccuralByOwnersDecision = x.AccuralByOwnersDecision,
                //    AccountOwner = x.AccountOwner,
                //    Address = x.Address,
                //    AddressOutsideSubject = x.AddressOutsideSubject,
                //    Area = x.Area,
                //    AreaMkd = x.AreaMkd,
                //    AreaShare = x.AreaShare,
                //    BillingAddressType = x.BillingAddressType,
                //    Building = x.Building,
                //    CloseDate = x.CloseDate,
                //    CreditedWithPenalty = x.CreditedWithPenalty,
                //    Email = x.Email,
                //    HasCharges = x.HasCharges,
                //    HasNonZeroCharges = x.HasNonZeroCharges,
                //    HasOnlyOneRoomWithOpenState = x.HasOnlyOneRoomWithOpenState,
                //    HouseNum = x.HouseNum,
                //    Housing = x.Housing,
                //    LegalFactAddress = new FiasAddress { }

                //})
                .Where(x => x.State.Code != stateCodeNoActive)
                .ToArray();

            result = filtered.Length;
            filtered.ForEach(x => x.ApplyAddressParts());

            // список айдишек разрешенных ОргПравФорм(из настроек)
            var orgFormIds = this.config.PaymentDocumentConfigLegal
                .OrganizationForms
                .Select(x => x.Id)
                .ToArray();

            // лист OwnerId у которых (Contragent!=null && orgFormsSelectedIds.contains(Contragent.OrganizationForm.Id))

            // подумать как фильтрануть только по filtered записям
            HashSet<long> ownerIdsApprovedOrgForms;
            if (this.orgFormGroup == OrgFormGroup.NoGroup)
            {
                ownerIdsApprovedOrgForms = new HashSet<long>();
            }
            else
            {
                ownerIdsApprovedOrgForms = legalAccDmn.GetAll()
                    .Where(x => orgFormIds.Contains(x.Contragent.OrganizationForm.Id))
                    .Select(x => x.Id)
                    .ToHashSet();
            }

            var grouped = filtered.GroupBy(x => new
            {
                x.BillingAddressType,
                IsOrgFormApproved = ownerIdsApprovedOrgForms.Contains(x.OwnerId)
            });

            foreach (var group in grouped)
            {
                var billingAddressType = group.Key.BillingAddressType;

                // group.Key.IsOrgFormApproved - тру в случае если ОргПравФорма входит в список в настройках
                // orgFormGroup == OrgFormGroup.FolderGroup || orgFormGroup == OrgFormGroup.FolderAndFormsGroup - в настройках выбран пункт 2 или 3
                // если все тру то получаем "ЮЛ. ОрганизационноПФорма" иначе "ЮЛ"
                var nodeName = this.GetNodeName(billingAddressType,
                    false,
                    group.Key.IsOrgFormApproved && (this.orgFormGroup == OrgFormGroup.FolderGroup || this.orgFormGroup == OrgFormGroup.FolderAndFormsGroup));

                var child = node.AddChild(nodeName, nodeName);

                var data =
                    group.Select(x => new { Account = x, CalcAccount = calcaccByRo.Get(x.RoId).Return(a => a.AccountNumber) })
                        .Where(x => x.CalcAccount.IsNotEmpty())
                        .Join(
                            legalAccDmn.GetAll()
                                .Select(
                                    l =>
                                        new
                                        {
                                            l.Id,
                                            ContragentId = l.Contragent.Id,
                                            ContragentShortName = l.Contragent.ShortName,
                                            OrgFormName = l.Contragent.OrganizationForm.Name
                                        }),
                            x => x.Account.OwnerId,
                            x => x.Id,
                            (d, o) => new { d.Account, o.ContragentId, ContragentName = o.ContragentShortName, o.OrgFormName, d.CalcAccount })
                        .ToList();

                var data1 = data.GroupBy(x => x.OrgFormName);

                foreach (var byOrgFormName in data1.OrderBy(x => x.Key))
                {
                    TreeNode orgForm = null;

                    if (byOrgFormName.Key != null && group.Key.IsOrgFormApproved && this.orgFormGroup == OrgFormGroup.FolderAndFormsGroup)
                    {
                        orgForm = child.AddChild(byOrgFormName.Key, byOrgFormName.Key);
                        orgForm.Id = byOrgFormName.Key;
                        orgForm.IsPhysical = false; // указываю явно, чтобы сделать на это акцент
                    }

                    foreach (var byContragent in byOrgFormName.GroupBy(x => new { x.ContragentName, x.ContragentId }))
                    {
                        var name = this.GetLegalName(byContragent.Key.ContragentName, byContragent.Key.ContragentId);

                        var contragentNode = orgForm == null ? child.AddChild(name, name) : orgForm.AddChild(name, name);
                        contragentNode.Id = name;
                        contragentNode.IsPhysical = false;

                        foreach (var byCalcAccount in byContragent.GroupBy(x => x.CalcAccount).OrderBy(x => x.Key))
                        {
                            var calcNode = contragentNode.AddChild(byCalcAccount.Key, byCalcAccount.Key);
                            calcNode.Id = byCalcAccount.Key;
                            calcNode.IconCls = "icon-page-white-acrobat";

                            foreach (var account in byCalcAccount)
                            {
                                this.AddAccountNode(calcNode, account.Account);
                            }
                        }
                    }
                }
            }

            return result;
        }

        private Expression<Func<BillingPersonalAccountDto, bool>> CreateExpressionForLegal(GroupingForLegalWithOneOpenRoom groupingForLegalWithOneOpenRoom)
        {
            switch (groupingForLegalWithOneOpenRoom)
            {
                case GroupingForLegalWithOneOpenRoom.LegalFolder:
                    return dto => dto.OwnerType == PersonalAccountOwnerType.Legal;
                case GroupingForLegalWithOneOpenRoom.IndividualFolder:
                    return dto => dto.OwnerType == PersonalAccountOwnerType.Legal && dto.HasOnlyOneRoomWithOpenState == false;
            }

            return dto => true;
        }

        private Expression<Func<BillingPersonalAccountDto, bool>> CreateChargeExpressionForLegal(AllowAccWithoutCharge allowAccWithoutCharge)
        {
            var parameterDto = Expression.Parameter(typeof(BillingPersonalAccountDto), "dto");
            Expression propertyHasCharges = Expression.Property(parameterDto, "HasNonZeroCharges");
            Expression propertySaldoOut = Expression.Property(parameterDto, "SaldoOut");
            Expression constantZero = Expression.Constant(0m);
            Expression constantTrue = Expression.Constant(true);

            Expression expression = null;

            //dto => dto.HasNonZeroCharges == true
            Expression baseExpression = Expression.Equal(propertyHasCharges, constantTrue);
            switch (allowAccWithoutCharge)
            {
                case AllowAccWithoutCharge.DontAllow:
                    expression = baseExpression;
                    break;
                case AllowAccWithoutCharge.WithPositiveSaldo:
                    //dto => dto.HasNonZeroCharges == true || dto.SaldoOut > 0
                    var positiveExpression = Expression.GreaterThan(propertySaldoOut, constantZero);
                    expression = Expression.OrElse(baseExpression, positiveExpression);
                    break;
                case AllowAccWithoutCharge.WithNegativeSaldo:
                    //dto => dto.HasNonZeroCharges == true || dto.SaldoOut < 0
                    var negativeExpression = Expression.LessThan(propertySaldoOut, constantZero);
                    expression = Expression.OrElse(baseExpression, negativeExpression);
                    break;
                case AllowAccWithoutCharge.WithNonZeroSaldo:
                    //dto => dto.HasNonZeroCharges == true || dto.SaldoOut <> 0
                    var nonZeroExpression = Expression.NotEqual(propertySaldoOut, constantZero);
                    expression = Expression.OrElse(baseExpression, nonZeroExpression);
                    break;
            }

            var result = Expression.Lambda<Func<BillingPersonalAccountDto, bool>>(expression, parameterDto);
            return result;
        }

        private string GetNodeName(AddressType billingAddressType, bool isPhysical, bool isOrgFormApproved)
        {
            switch (billingAddressType)
            {
                case AddressType.FactAddress:
                    if (isPhysical)
                    {
                        return this.config.PaymentDocumentConfigIndividual.GroupingOptions.AddressGroupingType
                            == AddressGroupingType.FactAddressInSpecialFolder ? "Физ. лица фактический адрес" : this.PhisicalString;
                    }

                    return "Юр.лица фактический адрес";
                case AddressType.AddressOutsideSubject:
                    return isPhysical
                        ? "Физ. лица фактический адрес"
                        : "Юр.лица фактический адрес";
                case AddressType.Email:
                    return isPhysical
                        ? "Физ. лица электронная почта"
                        : "Юр. лица электронная почта";
                default:
                    return isPhysical
                        ? this.PhisicalString
                        : (isOrgFormApproved ? this.LegalStringOF : this.LegalString);
            }
        }

        private TreeNode AddAccountNode(TreeNode node, BillingPersonalAccountDto account)
        {
            var child = node.AddChild(account.PersonalAccountNum, null);
            child.Id = account.Id.ToString();
            child.Value = account;

            return child;
        }

        private string GetLegalName(string name, long id)
        {
            name = name ?? string.Empty;
            var idStr = id.ToString();

            if (this.OptimizePath)
            {
                var maxNameLength = 100 - (idStr.Length + 1); // Зарезервировать место для суффикса "_<ID>"
                name = name.Substring(0, Math.Min(maxNameLength, name.Length));
            }

            name = string.Format("{0}_{1}", name, idStr);

            return name;
        }

        private string GetPhisicalName(string name)
        {
            name = name ?? string.Empty;
            return this.OptimizePath ? name.Substring(0, Math.Min(35, name.Length)) : name;
        }

        private IEnumerable<GroupingElement> GetGroupers(AddressType billingAddressType)
        {
            List<GroupingElement> result;

            if (billingAddressType == AddressType.Email)
            {
                result = new List<GroupingElement>
                {
                    new GroupingElement {GroupingType = GroupingType.Email}
                };
            }
            else if (billingAddressType == AddressType.AddressOutsideSubject)
            {
                result = new List<GroupingElement>
                {
                    new GroupingElement {GroupingType = GroupingType.AddressOutsideSubject}
                };
            }
            else if (this.config.PaymentDocumentConfigIndividual.GroupingOptions.GroupingElements.IsNotEmpty())
            {
                result = this.config.PaymentDocumentConfigIndividual.GroupingOptions.GroupingElements
                    .Where(x => x.IsUsed == YesNo.Yes)
                    .OrderBy(x => x.Order)
                    .ToList();
            }
            else
            {
                result = new List<GroupingElement>
                {
                    new GroupingElement {GroupingType = GroupingType.Settlement},
                    new GroupingElement {GroupingType = GroupingType.Locality},
                    new GroupingElement {GroupingType = GroupingType.Street}
                };
            }

            result.Add(new GroupingElement { GroupingType = GroupingType.ByAccountCount });

            return result;
        }

        private void CreateGroup(TreeNode node, IEnumerable<GroupingElement> groupers, IEnumerable<BillingPersonalAccountDto> dtos, Dictionary<long, string> delAgentByRo, Dictionary<string, string> fiasRaionDict)
        {
            if (groupers.IsEmpty())
            {
                dtos.ForEach(x => this.AddAccountNode(node, x));
            }
            else
            {
                var groupOption = groupers.First();
                if (groupOption.GroupingType != GroupingType.ByAccountCount)
                {
                    var groupExpr = this.CreateGrouper(groupOption, delAgentByRo, fiasRaionDict);
                    var group = dtos.GroupBy(groupExpr);

                    foreach (var g in group.OrderBy(x => x.Key))
                    {
                        var name = this.GetPhisicalName(g.Key.ToString());
                        var localRoot = node.AddChild(name, name);
                        localRoot.Id = g.Key;
                        localRoot.IsPhysical = true;

                        this.CreateGroup(localRoot, groupers.Skip(1), g.Select(x => x), delAgentByRo, fiasRaionDict);
                    }
                }
                else
                {
                    // Мы на последнем уровне группировки
                    dtos = this.Order(dtos);
                    if (this.config.PaymentDocumentConfigIndividual.PhysicalAccountsPerDocument > 0)
                    {
                        int current = 0;
                        foreach (var section in dtos.Split(this.config.PaymentDocumentConfigIndividual.PhysicalAccountsPerDocument))
                        {
                            var array = section.ToArray();
                            this.ShuffleItemsIfNeed(this.config, array);

                            var groupRoot = node.AddChild("{0}-{1}".FormatUsing(current + 1, current + section.Count()), null);
                            groupRoot.IconCls = "icon-page-white-acrobat";
                            this.CreateGroup(groupRoot, null, array, delAgentByRo, fiasRaionDict);

                            current += section.Count();
                        }
                    }
                    else
                    {
                        var array = dtos.ToArray();
                        this.ShuffleItemsIfNeed(this.config, array);
                        this.CreateGroup(node, null, array, delAgentByRo, fiasRaionDict);
                    }
                }
            }
        }

        private Func<BillingPersonalAccountDto, object> CreateGrouper(GroupingElement grouper, Dictionary<long, string> delAgentByRo, Dictionary<string, string> fiasRaionDict)
        {
            var pe = Expression.Parameter(typeof(BillingPersonalAccountDto), "x");

            var roFieldName = this.GetFieldName(YesNo.No, grouper.GroupingType);
            var fieldName = this.GetFieldName(grouper.UseFactAddress, grouper.GroupingType);

            Expression resultExpr;
            if (fieldName == "DeliveryAgent")
            {
                Expression<Func<long, string>> l = id => this.Get(delAgentByRo, id);
                Expression<Func<long, long, FiasAddress, string>> l2 = (id1, id2, address) => this.Get(delAgentByRo, id1, id2, address);

                Expression getFromDictExpr;

                if (grouper.UseFactAddress == YesNo.Yes)
                {
                    getFromDictExpr = Expression.Call(
                        Expression.Constant(this),
                        l2.Body.As<MethodCallExpression>().Method,
                        Expression.Constant(delAgentByRo),
                        Expression.PropertyOrField(pe, "RoId"),
                        Expression.PropertyOrField(pe, "IndividualFactAddressRoId"),
                        Expression.PropertyOrField(pe, "IndividualFactAddress")
                    );
                }
                else
                {
                    getFromDictExpr = Expression.Call(
                        Expression.Constant(this),
                        l.Body.As<MethodCallExpression>().Method,
                        Expression.Constant(delAgentByRo),
                        Expression.PropertyOrField(pe, "RoId"));
                }

                resultExpr = Expression.Convert(
                    Expression.Coalesce(getFromDictExpr, Expression.Constant(PaymentDocPreviewHelper.NoDeliveryAgent)),
                    typeof(object));
            }
            else if (fieldName == "IndividualFactAddressRoMunicipality")
            {
                Expression<Func<string, string>> l = aoguid => this.GetRaion(fiasRaionDict, aoguid);
                var propertyOrField = Expression.Convert(
                    Expression.Coalesce(Expression.PropertyOrField(pe, fieldName), Expression.Call(
                        Expression.Constant(this),
                        l.Body.As<MethodCallExpression>().Method,
                        Expression.Constant(fiasRaionDict),
                        Expression.Coalesce(Expression.PropertyOrField(pe, "IndividualFactAddressRaionGuid"), Expression.Constant(string.Empty)))),
                    typeof(object));

                resultExpr = Expression.Convert(
                    Expression.Coalesce(propertyOrField, Expression.PropertyOrField(pe, roFieldName)),
                    typeof(object));

                resultExpr = Expression.Convert(
                    Expression.Coalesce(resultExpr, Expression.Constant("Нет данных по имени группировки")),
                    typeof(object));
            }
            else
            {
                var propertyOrField = Expression.Convert(
                    Expression.Coalesce(Expression.PropertyOrField(pe, fieldName), Expression.PropertyOrField(pe, roFieldName)),
                    typeof(object));

                resultExpr = Expression.Convert(
                    Expression.Coalesce(propertyOrField, Expression.Constant("Нет данных по имени группировки")),
                    typeof(object));
            }

            return Expression.Lambda<Func<BillingPersonalAccountDto, object>>(resultExpr, pe).Compile();
        }

        private string GetFieldName(YesNo useFactAddress, GroupingType groupingType)
        {
            if (useFactAddress == YesNo.Yes)
            {
                switch (groupingType)
                {
                    case GroupingType.Municipality:
                        return "IndividualFactAddressRoMunicipality";
                    case GroupingType.Settlement:
                        return "IndividualFactAddressRoSettlement";
                    case GroupingType.Street:
                        return "IndividualFactAddressStreetName";
                    case GroupingType.Locality:
                        return "IndividualFactAddressPlaceName";
                    case GroupingType.DeliveryAgent:
                        return "DeliveryAgent";
                    case GroupingType.PostCode:
                        return "IndividualFactAddressPostCode";
                    case GroupingType.CrFundFormationType:
                        return "CrFundFormationType";
                    default:
                        return groupingType.ToString();
                }
            }

            switch (groupingType)
            {
                case GroupingType.Municipality:
                    return "Municipality";
                case GroupingType.Settlement:
                    return "Settlement";
                case GroupingType.Street:
                    return "StreetName";
                case GroupingType.Locality:
                    return "PlaceName";
                case GroupingType.DeliveryAgent:
                    return "DeliveryAgent";
                case GroupingType.PostCode:
                    return "PostCode";
                case GroupingType.CrFundFormationType:
                    return "CrFundFormationType";
                default:
                    return groupingType.ToString();
            }
        }

        private string Get(Dictionary<long, string> dict, long key)
        {
            return dict.Get(key);
        }

        private string Get(Dictionary<long, string> dict, long roId, long factRoId, FiasAddress addres)
        {
            return dict.Get(factRoId == 0 && addres == null ? roId : factRoId);
        }

        private string GetRaion(Dictionary<string, string> dict, string key)
        {
            return dict.Get(key);
        }

        private IEnumerable<BillingPersonalAccountDto> Order(IEnumerable<BillingPersonalAccountDto> dtos)
        {
            dtos = dtos.OrderBy(x => x.Municipality)
                .ThenBy(x => x.Settlement)
                .ThenBy(x => x.PlaceName);

            if (this.config.PaymentDocumentConfigIndividual.SortOptions.UseIndexForSorting == YesNo.Yes)
            {
                dtos = dtos.CastAs<IOrderedEnumerable<BillingPersonalAccountDto>>()
                    .ThenBy(x => x.PostCode.Or("0"), new NumericComparer());
            }

            dtos = dtos.CastAs<IOrderedEnumerable<BillingPersonalAccountDto>>()
                .ThenBy(x => x.StreetName)
                .ThenBy(x => x.HouseNum, new NumericComparer())
                .ThenBy(x => x.Letter)
                .ThenBy(x => x.Housing, new NumericComparer())
                .ThenBy(x => x.Building, new NumericComparer())
                .ThenBy(x => x.RoomNum, new NumericComparer());

            return dtos;
        }

        private void ShuffleItemsIfNeed(PaymentDocumentConfigContainer options, BillingPersonalAccountDto[] items)
        {
            if (options.PaymentDocumentConfigIndividual.SortOptions.DocumentsPerSheet == TwoDocumentsPerSheet.OrderedByHalfSheet
                && options.PaymentDocumentConfigCommon.PaperFormat == PaperFormat.A5)
            {
                this.ShuffleItems(items);
            }
        }

        private void ShuffleItems(BillingPersonalAccountDto[] items)
        {
            // Пример для [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]
            // Получаем [1, 6, 2, 7, 3, 8, 4, 9, 5, 10, 11]
            var length = items.Length;
            var halfLength = length / 2;

            var arr1 = new BillingPersonalAccountDto[halfLength];
            var arr2 = new BillingPersonalAccountDto[halfLength];

            Array.Copy(items, 0, arr1, 0, halfLength);
            Array.Copy(items, halfLength, arr2, 0, halfLength);

            for (int i = 0, j = 0; i < halfLength; ++i, j += 2)
            {
                items[j] = arr1[i];
                items[j + 1] = arr2[i];
            }
        }
    }

    /// <summary>
    /// Вспомогательный класс для построения дерева документов на оплату
    /// </summary>
    internal class TreeNode
    {
        private string relativePath;


        private Dictionary<string, TreeNode> keyToChildrenMap;

        private TreeNode(string name, string path)
        {
            this.keyToChildrenMap = new Dictionary<string, TreeNode>();
            this.relativePath = path;
            this.Name = name;
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        [JsonProperty("id")]
        public object Id { get; set; }

        /// <summary>
        /// Родитель
        /// </summary>
        [JsonIgnore]
        public TreeNode Parent { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [JsonProperty("text")]
        public string Name { get; private set; }

        /// <summary>
        /// Дочерние узлы
        /// </summary>
        [JsonProperty("children")]
        public List<TreeNode> Children { get; private set; }

        /// <summary>
        /// Счёт
        /// </summary>
        [JsonIgnore]
        public BillingPersonalAccountDto Value { get; set; }

        /// <summary>
        /// Индикатор конечного узла
        /// </summary>
        [JsonProperty("leaf")]
        public bool IsLeaf
        {
            get { return this.Children.IsEmpty(); }
        }

        /// <summary>
        /// Класс иконки
        /// </summary>
        [JsonProperty("iconCls")]
        public string IconCls { get; set; }

        /// <summary>
        /// Путь
        /// </summary>
        public string Path
        {
            get
            {
                if (this.Parent != null && this.Parent.Parent != null)
                {
                    return this.relativePath.IsEmpty()
                        ? this.Parent.Path
                        : this.Parent.Path + System.IO.Path.DirectorySeparatorChar + this.Name.GetCorrectPath();
                }

                return this.Name.GetCorrectPath();
            }
        }

        /// <summary>
        /// Индикатор физлица
        /// </summary>
        public bool IsPhysical { get; set; }

        /// <summary>
        /// Адрес помещения
        /// </summary>
        public string RoomAddress { get { return this.Value.Return(x => x.RoomAddress); } }

        /// <summary>
        /// Почтовый индекс
        /// </summary>
        public string PostCode { get { return this.Value.Return(x => x.PostCode); } }

        /// <summary>
        /// способ формирования фонда кр на текущий момент
        /// </summary>
        public CrFundFormationType AccountFormationVariant { get; set; }

        /// <summary>
        /// Создание корневого узла дерева
        /// </summary>
        /// <param name="name">Наименование</param>
        /// <param name="path">Путь</param>
        /// <returns></returns>
        public static TreeNode CreateRoot(string name, string path)
        {
            return new TreeNode(name, path);
        }

        /// <summary>
        /// Добавить дочерний узел
        /// </summary>
        /// <param name="name">Наименование</param>
        /// <param name="path">Путь</param>
        /// <returns>Дочерний узел</returns>
        public TreeNode AddChild(string name, string path)
        {
            TreeNode result;
            this.Children = this.Children ?? new List<TreeNode>();

            var key = this.GetKey(name, path);
            if (!this.keyToChildrenMap.TryGetValue(key, out result))
            {
                result = new TreeNode(name, path) { Parent = this };
                this.keyToChildrenMap[key] = result;
                this.Children.Add(result);
            }

            return result;
        }

        /// <summary>
        /// Возвращает наименование с учётом родительских узлов
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this.Parent != null)
            {
                return this.Parent.Name + "/" + this.Name;
            }

            return this.Name;
        }

        private string GetKey(string name, string path)
        {
            return name + path;
        }
    }
}