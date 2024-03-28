namespace Bars.GkhDi.PercentCalculationProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    using NHibernate.Linq;

    /// <summary>
    /// Реализация калькулятора Раскрытия информации по форме 988
    /// </summary>
    public partial class PercentCalculation988 : BasePercentCalculation
    {
        /// <summary>
        /// Домен-сервис для объекта недвижимости в периоде раскрытия информации
        /// </summary>
        public IDomainService<DisclosureInfoRealityObj> DisclosureInfoRealityObjDomainService { get; set; }

        /// <summary>
        /// Указывает, допустима ли данная реализация калькулятора раскрытия информации для указанного периода
        /// </summary>
        /// <param name="periodDi">Период раскрытия информации</param>
        /// <returns>Маркер</returns>
        public override bool CheckByPeriod(PeriodDi periodDi)
        {
            return periodDi.DateStart >= new DateTime(2015, 1, 1);
        }

        /// <summary>
        /// Запуск основных расчётов процентов раскрытия информации по УО
        /// </summary>
        /// <param name="disInfos">Перечисление УО, которые участвуют в подсчёте процентов</param>
        /// <param name="diQuery">Запрос УО, которые участвуют в подсчёте процентов</param>
        /// <returns>Проценты по УО</returns>
        protected override Dictionary<long, PercCalcResult> CalculateManOrgsInfo(IEnumerable<DisclosureInfo> disInfos, IQueryable<DisclosureInfo> diQuery)
        {
            var dictMoPerc = new Dictionary<long, PercCalcResult>();

            this.PrepareManOrgCacheCache(diQuery);

            foreach (var disclosureInfo in disInfos)
            {
                var diFullPercent = new DisclosureInfoPercent
                {
                    Code = "ManOrgInfoPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                    CalcDate = DateTime.Now.Date,
                    ActualVersion = 1,
                    DisclosureInfo = disclosureInfo
                };

                // Общие сведения об УО
                var diPercent = this.CalculateGeneralDataPercent(disclosureInfo);
                this.DisInfoPercents.Add(diPercent);
                this.PercentAlgoritm.AddElement(diFullPercent, diPercent);

                // Сведения о расторгнутых договорах
                diPercent = this.CalculateTerminateContractPercent(disclosureInfo);
                if (diPercent != null)
                {
                    this.DisInfoPercents.Add(diPercent);
                    this.PercentAlgoritm.AddElement(diFullPercent, diPercent);
                }

                // Членство в объединениях
                diPercent = this.CalculateMembershipUnionsPercent(disclosureInfo);
                if (diPercent != null)
                {
                    this.DisInfoPercents.Add(diPercent);
                    this.PercentAlgoritm.AddElement(diFullPercent, diPercent);
                }

                // Финансовая деятельность
                diPercent = this.CalculateFinActivityPercent(disclosureInfo);
                this.DisInfoPercents.Add(diPercent);
                this.PercentAlgoritm.AddElement(diFullPercent, diPercent);

                // Сведения о фондах
                diPercent = this.CalculateFundsInfoPercent(disclosureInfo);
                if (diPercent != null)
                {
                    this.DisInfoPercents.Add(diPercent);
                    this.PercentAlgoritm.AddElement(diFullPercent, diPercent);
                }

                // Административная ответственность
                diPercent = this.CalculateAdminResponsibilityPercent(disclosureInfo);
                this.DisInfoPercents.Add(diPercent);
                this.PercentAlgoritm.AddElement(diFullPercent, diPercent);

                // Лицензии
                diPercent = this.CalculateLicensePercent(disclosureInfo);
                this.DisInfoPercents.Add(diPercent);
                this.PercentAlgoritm.AddElement(diFullPercent, diPercent);

                // Сведения о договорах
                diPercent = this.CalculateInfoOnContrPercent(disclosureInfo);
                if (diPercent != null)
                {
                    this.DisInfoPercents.Add(diPercent);
                    this.PercentAlgoritm.AddElement(diFullPercent, diPercent);
                }

                // Документы
                diPercent = this.CalculateDocumentsPercent(disclosureInfo);
                if (diPercent != null)
                {
                    this.DisInfoPercents.Add(diPercent);
                    this.PercentAlgoritm.AddElement(diFullPercent, diPercent);
                }

                this.PercentAlgoritm.CalcPercent(diFullPercent);
                this.DisInfoPercents.Add(diFullPercent);

                dictMoPerc.Add(
                    disclosureInfo.Id,
                    new PercCalcResult
                    {
                        Percent = diFullPercent.Percent.ToDecimal(),
                        CompletePositionCount = diFullPercent.CompletePositionsCount,
                        PositionCount = diFullPercent.PositionsCount
                    });
            }

            return dictMoPerc;
        }

        /// <summary>
        /// Запуск основных расчётов процентов раскрытия информации по домам
        /// </summary>
        protected override void CalculateRealObj()
        {
            var diRoQuery = this.DisInfoRoDomain.GetAll()
                .Where(x => x.PeriodDi.Id == this.period.Id)
                .Where(x => this.moRoQuery.Any(y => y.RealityObject.Id == x.RealityObject.Id && y.ManOrgContract.ManagingOrganization.Id == x.ManagingOrganization.Id));

            var diRealObjs = this.GetRealityObjects(diRoQuery).DistinctBy(x => x.RoId);

            var diRoIds = diRealObjs.Select(x => x.Id).ToArray();
            this.PrepareRealityObjectCache(this.DisclosureInfoRealityObjDomainService.GetAll().WhereContains(x => x.Id, diRoIds));

            foreach (var realObj in diRealObjs)
            {
                var realityObjectPercent = new DiRealObjPercent
                {
                    Code = "DiRealObjPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                    CalcDate = DateTime.Now.Date,
                    ActualVersion = 1,
                    DiRealityObject = new DisclosureInfoRealityObj { Id = realObj.Id }
                };

                // Общие сведения и сведения о фонде капитального ремонта
                var diPercent = this.CalculateRealityObjectGeneralDataPercent(realObj);
                this.RealObjPercents.Add(diPercent);
                this.PercentAlgoritm.AddElement(realityObjectPercent, diPercent);

                // Конструктивные элементы
                diPercent = this.CalculateStructElementsPercent(realObj);
                this.RealObjPercents.Add(diPercent);
                this.PercentAlgoritm.AddElement(realityObjectPercent, diPercent);

                // Инженерные системы
                diPercent = this.CalculateEngineerSystemsPercent(realObj);
                this.RealObjPercents.Add(diPercent);
                this.PercentAlgoritm.AddElement(realityObjectPercent, diPercent);

                // Лифты
                diPercent = this.CalculateLiftsPercent(realObj);
                this.RealObjPercents.Add(diPercent);
                this.PercentAlgoritm.AddElement(realityObjectPercent, diPercent);

                // Приборы учёта
                diPercent = this.CalculateRealtyObjectDevicesPercent(realObj);
                this.RealObjPercents.Add(diPercent);
                this.PercentAlgoritm.AddElement(realityObjectPercent, diPercent);

                // Услуги (коммунальные, жилищные)
                diPercent = this.CalculateServicesPercent(realObj);
                this.RealObjPercents.Add(diPercent);
                this.PercentAlgoritm.AddElement(realityObjectPercent, diPercent);

                // План работ по содержанию и ремонту
                diPercent = this.CalculatePlanWorkServiceRepairPercent(realObj);
                if (diPercent.IsNotNull())
                {
                    this.RealObjPercents.Add(diPercent);
                    this.PercentAlgoritm.AddElement(realityObjectPercent, diPercent);
                }

                // Сведения об использовании мест общего пользования
                diPercent = this.CalculatePlaceGeneralUsePercent(realObj);
                this.RealObjPercents.Add(diPercent);
                this.PercentAlgoritm.AddElement(realityObjectPercent, diPercent);

                // Документы
                diPercent = this.CalculateDocumentsDiRealObjPercent(realObj);
                this.RealObjPercents.Add(diPercent);
                this.PercentAlgoritm.AddElement(realityObjectPercent, diPercent);

                // Управление домом
                diPercent = this.CalculateHouseManagingPercent(realObj);
                this.RealObjPercents.Add(diPercent);
                this.PercentAlgoritm.AddElement(realityObjectPercent, diPercent);

                // План мер по снижению расходов
                diPercent = this.CalculatePlanReductionExpensePercent(realObj);
                if (diPercent.IsNotNull())
                {
                    this.RealObjPercents.Add(diPercent);
                    this.PercentAlgoritm.AddElement(realityObjectPercent, diPercent);
                }

                // Сведения о случаях снижения платы
                diPercent = this.CalculateInfoAboutReductionPaymentPercent(realObj);
                if (diPercent.IsNotNull())
                {
                    this.RealObjPercents.Add(diPercent);
                    this.PercentAlgoritm.AddElement(realityObjectPercent, diPercent);
                }

                // Сведения об оплатах коммунальных услуг
                diPercent = this.CalculateCommunalServicesPaymentPercent(realObj);
                if (diPercent.IsNotNull())
                {
                    this.RealObjPercents.Add(diPercent);
                    this.PercentAlgoritm.AddElement(realityObjectPercent, diPercent);
                }

                // Сведения об использовании нежилых помещений
                diPercent = this.CalculateNonResidentialPlacementPercent(realObj);
                if (diPercent.IsNotNull())
                {
                    this.RealObjPercents.Add(diPercent);
                    this.PercentAlgoritm.AddElement(realityObjectPercent, diPercent);
                }

                // Финансовые показатели
                diPercent = this.CalculateFinActivityHousePercent(realObj);
                this.RealObjPercents.Add(diPercent);
                this.PercentAlgoritm.AddElement(realityObjectPercent, diPercent);

                this.PercentAlgoritm.CalcPercent(realityObjectPercent);
                this.RealObjPercents.Add(realityObjectPercent);

                this.DictRoPerc.Add(
                    realObj.RoId, 
                    new PercCalcResult
                    {
                        Percent = realityObjectPercent.Percent.ToDecimal(),
                        CompletePositionCount = realityObjectPercent.CompletePositionsCount,
                        PositionCount = realityObjectPercent.PositionsCount
                    });
            }
        }
       
        protected DisclosureInfoRealityObj GetRealityObjectProxy(long? id)
        {
            if (id.HasValue)
            {
                return new DisclosureInfoRealityObj() { Id = id.Value };
            }
            else
            {
                return null;
            }
        }

        private List<DisclosureInfoRealityObjProxy> GetRealityObjects(IQueryable<DisclosureInfoRealityObj> diRoQuery)
        {
            return diRoQuery
                .Fetch(x => x.RealityObject)
                .Select(
                    x => new DisclosureInfoRealityObjProxy
                    {
                        Id = x.Id,
                        RoId = x.RealityObject.Id,
                        FiasAddress = x.RealityObject.FiasAddress,
                        TypeProject = x.RealityObject.TypeProject.Name,
                        NumberNonResidentialPremises = x.RealityObject.NumberNonResidentialPremises,
                        AreaLiving = x.RealityObject.AreaLiving,
                        AreaMkd = x.RealityObject.AreaMkd,
                        AreaNotLivingPremises = x.RealityObject.AreaNotLivingPremises,
                        AreaOfAllNotLivingPremises = x.RealityObject.AreaNotLivingFunctional,
                        BuildYear = x.RealityObject.BuildYear,
                        DateCommissioning = x.RealityObject.DateCommissioning,
                        Floors = x.RealityObject.Floors,
                        MaximumFloors = x.RealityObject.MaximumFloors,
                        NumberApartments = x.RealityObject.NumberApartments,
                        NumberEntrances = x.RealityObject.NumberEntrances,
                        NumberLifts = x.RealityObject.NumberLifts,
                        TypeHouse = x.RealityObject.TypeHouse,
                        RoofingMaterial = x.RealityObject.RoofingMaterial.Name,
                        PlaceGeneralUse = x.PlaceGeneralUse,
                        AccountFormationVariant = x.RealityObject.AccountFormationVariant,
                        ReductionPayment = x.ReductionPayment,
                        NonResidentialPlacement = x.NonResidentialPlacement,

                        AdvancePayments = x.AdvancePayments,
                        CarryOverFunds = x.CarryOverFunds,
                        CashBalanceAdvancePayments = x.CashBalanceAdvancePayments,
                        CashBalanceAll = x.CashBalanceAll,
                        CashBalanceCarryOverFunds = x.CashBalanceCarryOverFunds,
                        CashBalanceDebt = x.CashBalanceDebt,
                        ChargeForMaintenanceAndRepairsAll = x.ChargeForMaintenanceAndRepairsAll,
                        ChargeForMaintenanceAndRepairsMaintanance = x.ChargeForMaintenanceAndRepairsMaintanance,
                        ChargeForMaintenanceAndRepairsManagement = x.ChargeForMaintenanceAndRepairsManagement,
                        ChargeForMaintenanceAndRepairsRepairs = x.ChargeForMaintenanceAndRepairsRepairs,
                        ComServApprovedPretensionCount = x.ComServApprovedPretensionCount,
                        ComServEndAdvancePay = x.ComServEndAdvancePay,
                        ComServEndCarryOverFunds = x.ComServEndCarryOverFunds,
                        ComServEndDebt = x.ComServEndDebt,
                        ComServNoApprovedPretensionCount = x.ComServNoApprovedPretensionCount,
                        ComServPretensionRecalcSum = x.ComServPretensionRecalcSum,
                        ComServReceivedPretensionCount = x.ComServReceivedPretensionCount,
                        ComServStartAdvancePay = x.ComServStartAdvancePay,
                        ComServStartCarryOverFunds = x.ComServStartCarryOverFunds,
                        ComServStartDebt = x.ComServStartDebt,
                        Debt = x.Debt,
                        ReceivedCashAll = x.ReceivedCashAll,
                        ReceivedCashAsGrant = x.ReceivedCashAsGrant,
                        ReceivedCashFromOtherTypeOfPayments = x.ReceivedCashFromOtherTypeOfPayments,
                        ReceivedCashFromOwners = x.ReceivedCashFromOwners,
                        ReceivedCashFromOwnersTargeted = x.ReceivedCashFromOwnersTargeted,
                        ReceivedCashFromUsingCommonProperty = x.ReceivedCashFromUsingCommonProperty,

                        ApprovedPretensionCount = x.ApprovedPretensionCount,
                        NoApprovedPretensionCount = x.NoApprovedPretensionCount,
                        PretensionRecalcSum = x.PretensionRecalcSum,
                        ReceivedPretensionCount = x.ReceivedPretensionCount,

                        ReceiveSumByClaimWork = x.ReceiveSumByClaimWork,
                        SentPetitionCount = x.SentPetitionCount,
                        SentPretensionCount = x.SentPretensionCount,
                        ConditionHouse = x.RealityObject.ConditionHouse
                    }).ToList();
        }

    }
}
