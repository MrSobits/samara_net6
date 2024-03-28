 namespace Bars.GkhDi.Regions.Tatarstan.PercentCalculationProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;
    using Bars.GkhDi.PercentCalculationProvider;
    using Bars.GkhDi.Regions.Tatarstan.Entities;
    using Bars.Gkh.Entities;
    using Gkh.Utils;

    /// <summary>
    /// Класс расчётов процентов раскрытия информации по РТ
    /// </summary>
    public class PercentCalculation : GkhDi.PercentCalculationProvider.PercentCalculation
    {

        #region property injections
        /// <summary>
        /// Сервис объектов недвижимости в период раскрытия информации
        /// </summary>
        public virtual IDomainService<DisclosureInfoRealityObj> DisInfoRoDomain { get; set; }
        /// <summary>
        /// Сервис услуг ремонта
        /// </summary>
        public virtual IDomainService<RepairService> RepairServiceDomain { get; set; }
        /// <summary>
        /// Сервис поставщиков услуг
        /// </summary>
        public virtual IDomainService<ProviderService> ProviderServiceDomain { get; set; }
        /// <summary>
        /// Сервис ППР списка работ
        /// </summary>
        public virtual IDomainService<WorkRepairList> WorkRepairListDomain { get; set; }
        /// <summary>
        /// Сервис тарифов потребителей
        /// </summary>
        public virtual IDomainService<TariffForConsumers> TariffForConsumersDomain { get; set; }
        /// <summary>
        /// Сервис работ по ТО
        /// </summary>
        public virtual IDomainService<WorkRepairTechServ> WorkRepairTechServDomain { get; set; }
        /// <summary>
        /// Сервис детализаций работ по РТ
        /// </summary>
        public virtual IDomainService<WorkRepairDetailTat> WorkRepairDetailTatService { get; set; }
        /// <summary>
        /// Сервис работ по плану работ
        /// </summary>
        public virtual IDomainService<PlanWorkServiceRepairWorks> PlanWorkServiceRepairWorksDomain { get; set; }
        /// <summary>
        /// Сервис планов работ по содержанию и ремонту
        /// </summary>
        public virtual IDomainService<PlanWorkServiceRepair> PlanWorkServiceRepairDomain { get; set; }
        /// <summary>
        /// Сервис связей названий мер с планом
        /// </summary>
        public virtual IDomainService<PlanReduceMeasureName> PlanReduceMeasureNameDomain { get; set; }
        /// <summary>
        /// Сервис планов мер по снижению расходов
        /// </summary>
        public virtual IDomainService<PlanReductionExpense> PlanReductionExpenseDomain { get; set; }
        /// <summary>
        /// Сервис работ по плану по снижению расходов
        /// </summary>
        public virtual IDomainService<PlanReductionExpenseWorks> PlanReductionExpenseWorksDomain { get; set; }
        /// <summary>
        /// Сервис сведений по снижению плат
        /// </summary>
        public virtual IDomainService<InfoAboutReductionPayment> InfoAboutReductionPaymentDomain { get; set; }
        /// <summary>
        /// Сервис сведений об использовании мест общего пользования
        /// </summary>
        public virtual IDomainService<InfoAboutUseCommonFacilities> InfoAboutUseCommonFacilitiesDomain { get; set; }
        /// <summary>
        /// Сервис сведений об использовании нежелых помещений
        /// </summary>
        public virtual IDomainService<NonResidentialPlacement> NonResidentialPlacementDomain { get; set; }
        /// <summary>
        /// Сервис сведений об УО объекта недвижимости
        /// </summary>
        public virtual IDomainService<DocumentsRealityObj> DocumentsRealityObjDomain { get; set; }
        /// <summary>
        /// Сервис базовых услуг
        /// </summary>
        public virtual IDomainService<BaseService> BaseServiceDomain { get; set; }
        /// <summary>
        /// Сервис значений тех. паспорта
        /// </summary>
        public virtual IDomainService<TehPassportValue> TehPassportValueDomain { get; set; }
        /// <summary>
        /// Сервис тарифов для РСО
        /// </summary>
        public virtual IDomainService<TariffForRso> TariffForRsoDomain { get; set; }
        #endregion

        /// <summary>
        /// Запуск основных расчётов процентов раскрытия информации по домам
        /// </summary>
        protected override void CalculateRealObj()
        {
            var diRoQuery = DisInfoRoDomain.GetAll().Where(x =>
                moRoQuery.Any(y => y.RealityObject.Id == x.RealityObject.Id && y.ManOrgContract.ManagingOrganization.Id == x.ManagingOrganization.Id) &&
                x.PeriodDi.Id == period.Id);

            var diRealObjs = diRoQuery
                        .Select(x => new
                        {
                            x.Id,
                            x.ReductionPayment,
                            RealObjId = x.RealityObject.Id,
                            x.PlaceGeneralUse,
                            x.NonResidentialPlacement,
                            x.RealityObject.NumberLifts, 
                            x.PeriodDi
                        }).ToList();

            var planReductionExpensePercentDict = this.GetPlanReductionExpensePercentDict(diRoQuery);

            var infoAboutReductionPaymentDiIds = InfoAboutReductionPaymentDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                .Select(x => x.DisclosureInfoRealityObj.Id)
                .Distinct()
                .ToArray();

            var planWorkServicePercentDict = this.GetPlanWorkServicePercentDict(diRoQuery);

            var nonResidentialPlacementDiIds = NonResidentialPlacementDomain
                .GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id)
                                && ((x.DateStart.HasValue && (x.DateStart >= period.DateStart)
                                        && x.DateStart.HasValue && (period.DateEnd >= x.DateStart))
                                        || ((x.DateStart.HasValue && (period.DateStart >= x.DateStart) || !x.DateStart.HasValue)
                                            && (x.DateEnd.HasValue && (x.DateEnd >= period.DateStart) || !x.DateEnd.HasValue))))
                .Select(x => x.DisclosureInfoRealityObj.Id)
                .Distinct()
                .ToDictionary(x => x);

            var infoAboutUseCommonFacilitiesDiIds = InfoAboutUseCommonFacilitiesDomain
                .GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id)
                               && ((x.DateStart.HasValue && (x.DateStart >= period.DateStart)
                                        && x.DateStart.HasValue && (period.DateEnd >= x.DateStart))
                                        || ((x.DateStart.HasValue && (period.DateStart >= x.DateStart) || !x.DateStart.HasValue)
                                            && (x.DateEnd.HasValue && (x.DateEnd >= period.DateStart) || !x.DateEnd.HasValue))))
                .Select(x => x.DisclosureInfoRealityObj.Id)
                .Distinct()
                .ToDictionary(x => x);

            var documentRealObjs = DocumentsRealityObjDomain.GetAll()
                 .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                 .Select(x => new
                 {
                     DisclosureInfoRealityObjId = x.DisclosureInfoRealityObj.Id,
                     FileActState = ((long?)x.FileActState.Id) != null,
                     FileCatalogRepair = ((long?)x.FileCatalogRepair.Id) != null,
                     FileReportPlanRepair = ((long?)x.FileReportPlanRepair.Id) != null
                 })
                 .AsEnumerable()
                 .GroupBy(x => x.DisclosureInfoRealityObjId)
                 .ToDictionary(
                     x => x.Key,
                     y => y.Select(z => new { z.FileActState, z.FileCatalogRepair, z.FileReportPlanRepair }).FirstOrDefault());

            var services = BaseServiceDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                .Where(x => x.TemplateService.IsMandatory &&
                    (x.TemplateService.ActualYearStart.HasValue && x.DisclosureInfoRealityObj.PeriodDi.DateStart.Value.Year >= x.TemplateService.ActualYearStart.Value ||
                        !x.TemplateService.ActualYearStart.HasValue)
                    && (x.TemplateService.ActualYearEnd.HasValue && x.DisclosureInfoRealityObj.PeriodDi.DateStart.Value.Year <= x.TemplateService.ActualYearEnd.Value ||
                        !x.TemplateService.ActualYearEnd.HasValue))
                .Select(x => new
                {
                    DisclosureInfoRealityObjId = x.DisclosureInfoRealityObj.Id,
                    x.Id,
                    x.TemplateService.Code,
                    x.TemplateService.KindServiceDi,
                    RealObjId = x.DisclosureInfoRealityObj.RealityObject.Id,
                    AnyProvider = ProviderServiceDomain.GetAll().Any(y => y.BaseService.Id == x.Id)
                })
                .AsEnumerable()
                .GroupBy(x => x.DisclosureInfoRealityObjId)
                .ToDictionary(
                    x => x.Key,
                    y => y.Select(x => new ServiceProxy
                        {
                            Id = x.Id,
                            Code = x.Code.ToInt(),
                            KindServiceDi = x.KindServiceDi,
                            DiRealObjId = x.DisclosureInfoRealityObjId,
                            RealObjId = x.RealObjId,
                            HasProvider = x.AnyProvider
                        }));

            var serv7tpDone = TehPassportValueDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.RealityObject.Id == x.TehPassport.RealityObject.Id)
                            && x.FormCode == "Form_3_7" && x.CellCode == "1:3" && x.Value != "0")
                .Select(x => x.TehPassport.RealityObject.Id).Distinct()
                .ToHashSet();

            var serv7ToInclude = HousingServiceDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                .Where(x => x.TemplateService.Code == "7")
                .Where(x => !(x.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedWithoutMo && x.Provider != null)
                         && !(x.TypeOfProvisionService == TypeOfProvisionServiceDi.OwnersRefused && x.Protocol != null))
                .Select(x => x.Id)
                .ToHashSet();

            // Списки на исключение из подсчета
            var housingServices = GetHousingServicesToExcludeDict(diRoQuery);
            var communalServices = GetCommunalServicesToExcludeDict(diRoQuery);
            var repairServices = GetRepairServicesToExcludeDict(diRoQuery);
            var capRepairServices = GetCapRepairServicesToExcludeDict(diRoQuery);

            var serv7RealObjs = TehPassportValueDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.RealityObject.Id == x.TehPassport.RealityObject.Id)
                                    && x.FormCode == "Form_3_7" && x.CellCode == "1:3" && x.Value == "0")
                .Select(x => x.TehPassport.RealityObject.Id).Distinct()
                .ToDictionary(x => x);

            var tariffForConsumers = TariffForConsumersDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.BaseService.DisclosureInfoRealityObj.Id))
                .Select(x => x.BaseService.Id)
                .Distinct()
                .ToDictionary(x => x);

            var tariffForRsoItemsDiIds = TariffForRsoDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.BaseService.DisclosureInfoRealityObj.Id))
                .Select(x => x.BaseService.Id)
                .Distinct()
                .ToDictionary(x => x);

            var repairServiceIsDisclosedDict = GetRepairServiceIsDisclosedDict(diRoQuery);

            foreach (var diRealityObj in diRealObjs)
            {
                var PositionsCount = 0;
                var CompletePositionsCount = 0;
                var tempPositionsCount = 0;
                var tempCompletePositionsCount = 0;
                decimal? percent;

                #region План мер по снижению расходов

                percent = planReductionExpensePercentDict.ContainsKey(diRealityObj.Id) ? planReductionExpensePercentDict[diRealityObj.Id] : 0;
                tempPositionsCount = 1;
                tempCompletePositionsCount = percent == 100 ? 1 : 0;

                RealObjPercents.Add(new DiRealObjPercent
                {
                    Code = "PlanReductionExpensePercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = tempPositionsCount,
                    CompletePositionsCount = tempCompletePositionsCount,
                    ActualVersion = 1,
                    DiRealityObject = new DisclosureInfoRealityObj { Id = diRealityObj.Id }
                });
                PositionsCount += tempPositionsCount;
                CompletePositionsCount += tempCompletePositionsCount;

                #endregion План мер по снижению расходов

                #region Сведения о случаях снижения платы
                percent = 0;
                tempPositionsCount = 1;
                if (diRealityObj.ReductionPayment == YesNoNotSet.No)
                {
                    percent = null;
                    tempPositionsCount = 0;
                }

                if (diRealityObj.ReductionPayment == YesNoNotSet.Yes)
                {
                    var hasInfoAboutReductionPayment = infoAboutReductionPaymentDiIds.Contains(diRealityObj.Id);

                    percent = hasInfoAboutReductionPayment ? 100 : 0;
                }

                tempCompletePositionsCount = percent == 100 ? 1 : 0;

                RealObjPercents.Add(new DiRealObjPercent
                {
                    Code = "InfoAboutReductionPaymentPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = tempPositionsCount,
                    CompletePositionsCount = tempCompletePositionsCount,
                    ActualVersion = 1,
                    DiRealityObject = new DisclosureInfoRealityObj { Id = diRealityObj.Id }
                });

                PositionsCount += tempPositionsCount;
                CompletePositionsCount += tempCompletePositionsCount;

                #endregion Сведения о случаях снижения платы

                #region Документы

                tempPositionsCount = 2;
                tempCompletePositionsCount = 0;

                if (documentRealObjs.ContainsKey(diRealityObj.Id) && documentRealObjs[diRealityObj.Id].FileActState)
                {
                    tempCompletePositionsCount++;
                }

                if (documentRealObjs.ContainsKey(diRealityObj.Id) && documentRealObjs[diRealityObj.Id].FileCatalogRepair)
                {
                    tempCompletePositionsCount++;
                }

                percent = (decimal.Divide(tempCompletePositionsCount, tempPositionsCount) * 100).RoundDecimal(2);

                RealObjPercents.Add(new DiRealObjPercent
                {
                    Code = "DocumentsDiRealObjPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = tempPositionsCount,
                    CompletePositionsCount = tempCompletePositionsCount,
                    ActualVersion = 1,
                    DiRealityObject = new DisclosureInfoRealityObj { Id = diRealityObj.Id }
                });

                PositionsCount += tempPositionsCount;
                CompletePositionsCount += tempCompletePositionsCount;
                #endregion Документы

                #region План работ по содержанию и ремонту

                percent = planWorkServicePercentDict[diRealityObj.Id];
                tempPositionsCount = percent.HasValue ? 1 : 0;
                tempCompletePositionsCount = percent == 100 ? 1 : 0;

                RealObjPercents.Add(new DiRealObjPercent
                {
                    Code = "PlanWorkServiceRepairPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = tempPositionsCount,
                    CompletePositionsCount = tempCompletePositionsCount,
                    ActualVersion = 1,
                    DiRealityObject = new DisclosureInfoRealityObj { Id = diRealityObj.Id }
                });

                PositionsCount += tempPositionsCount;
                CompletePositionsCount += tempCompletePositionsCount;
                #endregion План работ по содержанию и ремонту

                #region Сведения об использовании нежилых помещений

                tempPositionsCount = 1;
                tempCompletePositionsCount = 0;

                if (diRealityObj.NonResidentialPlacement == YesNoNotSet.No)
                {
                    tempPositionsCount = 0;
                }
                else
                {
                    if (diRealityObj.NonResidentialPlacement == YesNoNotSet.Yes && nonResidentialPlacementDiIds.ContainsKey(diRealityObj.Id))
                    {
                        tempCompletePositionsCount = 1;
                    }
                }

                percent = tempPositionsCount != 0 ? (decimal.Divide(tempCompletePositionsCount, tempPositionsCount) * 100).RoundDecimal(2) : (decimal?)null;

                RealObjPercents.Add(new DiRealObjPercent
                {
                    Code = "NonResidentialPlacementPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = tempPositionsCount,
                    CompletePositionsCount = tempCompletePositionsCount,
                    ActualVersion = 1,
                    DiRealityObject = new DisclosureInfoRealityObj { Id = diRealityObj.Id }
                });

                PositionsCount += tempPositionsCount;
                CompletePositionsCount += tempCompletePositionsCount;

                #endregion Сведения об использовании нежилых помещений

                #region Сведения об использовании мест общего пользования

                tempPositionsCount = 1;
                tempCompletePositionsCount = 0;

                if (diRealityObj.PlaceGeneralUse == YesNoNotSet.No)
                {
                    tempPositionsCount--;
                }
                else
                {
                    if (diRealityObj.PlaceGeneralUse == YesNoNotSet.Yes && infoAboutUseCommonFacilitiesDiIds.ContainsKey(diRealityObj.Id))
                    {
                        tempCompletePositionsCount++;
                    }
                }

                percent = tempPositionsCount != 0 ? (decimal.Divide(tempCompletePositionsCount, tempPositionsCount) * 100).RoundDecimal(2) : (decimal?)null;

                RealObjPercents.Add(new DiRealObjPercent
                {
                    Code = "PlaceGeneralUsePercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = tempPositionsCount,
                    CompletePositionsCount = tempCompletePositionsCount,
                    ActualVersion = 1,
                    DiRealityObject = new DisclosureInfoRealityObj { Id = diRealityObj.Id }
                });

                PositionsCount += tempPositionsCount;
                CompletePositionsCount += tempCompletePositionsCount;
                #endregion Сведения об использовании мест общего пользования

                #region Услуги

                TempCompleteCodes.Clear();
                TempNullCodes.Clear();

                tempPositionsCount = this.TemplateServiceDomain.GetAll()
                    .Count(x => x.IsMandatory &&
                        (x.ActualYearStart.HasValue && diRealityObj.PeriodDi.DateStart.Value.Year >= x.ActualYearStart.Value || !x.ActualYearStart.HasValue)
                        && (x.ActualYearEnd.HasValue && diRealityObj.PeriodDi.DateStart.Value.Year <= x.ActualYearEnd.Value || !x.ActualYearEnd.HasValue));

                tempCompletePositionsCount = 0;
                percent = 0;     

                if (diRealityObj.NumberLifts == 0)
                {
                    TempNullCodes.Add(27);
                    TempNullCodes.Add(28);
                }

                if (services.ContainsKey(diRealityObj.Id))
                {
                    var servs = CheckRequiredServices(services[diRealityObj.Id], housingServices, communalServices, repairServices, capRepairServices, serv7RealObjs, diRealityObj.NumberLifts);

                    if (servs.All(x => x.Code != 7))
                    {
                        TempNullCodes.Add(7);
                    }

                    foreach (var service in servs)
                    {
                        var servPositionsCount = 1;
                        var servCompletePositionsCount = 1;
                        decimal servPercent;

                        servPositionsCount++;
                        if (service.HasProvider)
                        {
                            servCompletePositionsCount++;
                        }

                        servPositionsCount++;
                        if (tariffForConsumers.ContainsKey(service.Id))
                        {
                            servCompletePositionsCount++;
                        }

                        if (service.Code == 7)
                        {
                            servPositionsCount++;

                            if (serv7ToInclude.Contains(service.Id) && serv7tpDone.Contains(service.RealObjId))
                            {
                                servCompletePositionsCount++;
                            }
                        }

                        if (service.KindServiceDi == KindServiceDi.Communal)
                        {
                            servPositionsCount++;
                            if (tariffForRsoItemsDiIds.ContainsKey(service.Id))
                            {
                                servCompletePositionsCount++;
                            }
                        }

                        if (service.KindServiceDi == KindServiceDi.Repair)
                        {
                            servPositionsCount++;
                            if (repairServiceIsDisclosedDict.ContainsKey(service.Id))
                            {
                                servCompletePositionsCount++;
                            }
                        }

                        if (service.KindServiceDi == KindServiceDi.Housing
                            || service.KindServiceDi == KindServiceDi.Managing
                            || service.KindServiceDi == KindServiceDi.CapitalRepair)
                        {
                            servPercent = servCompletePositionsCount == servPositionsCount ? 100 : 50;
                        }
                        else
                        {
                            servPercent = 30 + (70m * servCompletePositionsCount/servPositionsCount);
                        }

                        if (servPercent == 100)
                        {
                            TempCompleteCodes.Add(service.Code);
                        }

                        ServicePercents.Add(
                            new ServicePercent
                            {
                                Code = "ServicePercent",
                                TypeEntityPercCalc = TypeEntityPercCalc.Service,
                                Percent = servPercent,
                                CalcDate = DateTime.Now.Date,
                                PositionsCount = servPositionsCount,
                                CompletePositionsCount = servCompletePositionsCount,
                                ActualVersion = 1,
                                Service = new BaseService { Id = service.Id }
                            });
                    }
                }

                var nullCodes = TempNullCodes.Where(x => !TempCompleteCodes.Contains(x)).ToList();
                tempPositionsCount -= nullCodes.Count();
                tempCompletePositionsCount += TempCompleteCodes.Count();

                percent = (decimal.Divide(tempCompletePositionsCount, tempPositionsCount) * 100).RoundDecimal(2);

                RealObjPercents.Add(new DiRealObjPercent
                {
                    Code = "ServicesPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = tempPositionsCount,
                    CompletePositionsCount = tempCompletePositionsCount,
                    ActualVersion = 1,
                    DiRealityObject = new DisclosureInfoRealityObj { Id = diRealityObj.Id }
                });

                PositionsCount += tempPositionsCount;
                CompletePositionsCount += tempCompletePositionsCount;

                #endregion Услуги

                percent = (decimal.Divide(CompletePositionsCount, PositionsCount) * 100).RoundDecimal(2);
                RealObjPercents.Add(new DiRealObjPercent
                {
                    Code = "DiRealObjPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.RealObj,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = PositionsCount,
                    CompletePositionsCount = CompletePositionsCount,
                    ActualVersion = 1,
                    DiRealityObject = new DisclosureInfoRealityObj { Id = diRealityObj.Id }
                });

                DictRoPerc.Add(
                    diRealityObj.RealObjId, new PercCalcResult
                    {
                        Percent = percent.ToDecimal(),
                        CompletePositionCount = CompletePositionsCount,
                        PositionCount = PositionsCount
                    });
            }
        }


        /// <summary>
        /// Получение информации по планам мер на снижение расходов
        /// </summary>
        /// <param name="diRoQuery">Список домов для проверки</param>
        /// <returns>Словарь объектов управления</returns>
        protected override Dictionary<long, decimal> GetPlanReductionExpensePercentDict(IQueryable<DisclosureInfoRealityObj> diRoQuery)
        {
            /*по каждому дому должна быть хотя бы 1 запись и в ней должна быть хотя бы 1 запись меры с заполненными полями:
            - наименование
            - срок выполнения
            - предполагаемое снижение
            */

            var planReductionPlanReduceMeasureNameQuery = PlanReduceMeasureNameDomain.GetAll()
                .Where(x => x.MeasuresReduceCosts != null)
                .Select(x => x.PlanReductionExpenseWorks.Id);

            var planReductionExpenseWorksQuery = PlanReductionExpenseWorksDomain.GetAll()
                .Where(x => (x.Name != null && x.Name != "") || planReductionPlanReduceMeasureNameQuery.Contains(x.Id))
                .Where(x => x.DateComplete.HasValue)
                .Where(x => x.PlannedReductionExpense.HasValue);

            var percentDict = PlanReductionExpenseDomain.GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                .Where(x => planReductionExpenseWorksQuery.Any(y => y.PlanReductionExpense.Id == x.Id))
                .Select(x => x.DisclosureInfoRealityObj.Id)
                .Distinct()
                .ToDictionary(x => x, x => 100m);

            return percentDict;
        }

        /// <summary>
        /// Учёт УО с отсутствующими услугами на ремонт
        /// </summary>
        /// <param name="diRoQuery">Список домов для проверки</param>
        /// <returns>Словарь УО с процентами</returns>
        protected new Dictionary<long, decimal?> GetPlanWorkServicePercentDict(IQueryable<DisclosureInfoRealityObj> diRoQuery)
        {
            /* План работ по содержанию и ремонту
                100% Если выполняются все условия:
                1. По каждой услуге с типом="ремонт" и типом предоставления="услуга предоставляется через УО" есть запись плана работ 
                2. Есть записи по ТО и указана плановая сумма, дата начала и окончания
                3. раздел "ППР":
                3.1 в разделе "ППР" в комбобоксе "наличие ППР"="нет"
                    или
                3.2 в разделе "ППР" в комбобоксе "наличие ППР"="да":
                        3.2.1 по каждой записи указана плановая сумма, дата начала и окончания    
             
            * 
            * Процент не расчитывается, если нет услуг с типом="ремонт" и типом предоставления="услуга предоставляется через УО",
            * но есть услуги с типом="ремонт" и типом предоставления != "услуга предоставляется через УО".
            * То есть, если у УО нет услуг по ремонту, то и нечего раскрывать
            */

            var planWorkServiceRepairWorksQuery = PlanWorkServiceRepairWorksDomain.GetAll();

            var planWorkServiceRepairQuery = PlanWorkServiceRepairDomain.GetAll();

            var repairServiceQuery = RepairServiceDomain.GetAll()
                .Where(x => x.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedMo);

            var repairServiceProvidedWithoutMoQuery = RepairServiceDomain.GetAll()
                .Where(x => x.TypeOfProvisionService != TypeOfProvisionServiceDi.ServiceProvidedMo);

            var percentDict = diRoQuery
                .Select(x => new
                {
                    x.Id,
                    anyServiceProvidedWithoutMo = repairServiceProvidedWithoutMoQuery.Any(y => y.DisclosureInfoRealityObj.Id == x.Id),
                    anyServiceProvidedMo = repairServiceQuery.Any(y => y.DisclosureInfoRealityObj.Id == x.Id),
                    allDiscoveredServiceProvidedMo = repairServiceQuery.Where(y => y.DisclosureInfoRealityObj.Id == x.Id)
                        .All(v => v.SumWorkTo.HasValue 
                            && v.DateStart.HasValue
                            && v.DateEnd.HasValue
                            && planWorkServiceRepairQuery.Any(y => y.BaseService.Id == v.Id)
                            && WorkRepairTechServDomain.GetAll().Any(y => y.BaseService.Id == v.Id)
                            && (v.ScheduledPreventiveMaintanance == YesNoNotSet.No
                                || (v.ScheduledPreventiveMaintanance == YesNoNotSet.Yes 
                                    && planWorkServiceRepairWorksQuery.Any(y => y.PlanWorkServiceRepair.BaseService.Id == v.Id)
                                    && planWorkServiceRepairWorksQuery
                                        .Where(y => y.PlanWorkServiceRepair.BaseService.Id == v.Id)
                                        .All(y => y.DateStart.HasValue && y.DateEnd.HasValue && y.Cost.HasValue))))
                })
                .ToDictionary(
                    x => x.Id, 
                    x => x.anyServiceProvidedMo
                        ? (decimal?)(x.allDiscoveredServiceProvidedMo ? 100 : 0)
                        : (x.anyServiceProvidedWithoutMo ? null : (decimal?)0));

            return percentDict;
        }


        /// <summary>
        /// Получение информации по услугам ремонта
        /// </summary>
        /// <param name="diRoQuery">Список домов для проверки</param>
        /// <returns>Список услуг</returns>
        protected override Dictionary<long, long> GetRepairServiceIsDisclosedDict(IQueryable<DisclosureInfoRealityObj> diRoQuery)
        {
            /*100% = если выполнено:
            1. в таблице поставщик есть хотя бы 1 запись
            2. в разделе "тариф" есть хотя бы 1 запись
            3. в разделе "Работы по ТО" есть хотя бы 1 запись И заполнена плановая сумма И дата начала  И дата окончания
            4. в разделе "ППР" в комбобоксе "наличие ППР"="нет"
                    или
               в разделе "ППР" в комбобоксе "наличие ППР"="да" 
                         *  И {есть хотя бы 1 запись работы
                         *  И (по ней заполнена дата начала, плановая стоимость 
                         *  И (есть хотя бы 1 запись детализации с указанием ед.измерения и планового объема) ).}

                если в разделе "ППР" несколько записей, то по по всем должны быть заполнены дата начала, детализация с указанием ед.измерения и план. объема.*/

            var workRepairListQuery = WorkRepairListDomain.GetAll();

            var serviceIsDisclosed = RepairServiceDomain
                 .GetAll()
                 .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                 .Where(x => x.TypeOfProvisionService != TypeOfProvisionServiceDi.ServiceNotAvailable)
                 .Where(x => ProviderServiceDomain.GetAll().Any(y => y.BaseService.Id == x.Id)) // Хотя бы 1 поставщик
                 .Where(x => TariffForConsumersDomain.GetAll().Any(y => y.BaseService.Id == x.Id)) // Хотя бы 1 тариф
                 .Where(x => WorkRepairTechServDomain.GetAll().Any(y => y.BaseService.Id == x.Id)) // в разделе "Работы по ТО" есть хотя бы 1 запись
                 .Where(x => x.SumWorkTo.HasValue && x.DateStart.HasValue && x.DateEnd.HasValue) // заполнена плановая сумма И дата начала  И дата окончания
                 .Where(x => x.ScheduledPreventiveMaintanance == YesNoNotSet.No
                     || (x.ScheduledPreventiveMaintanance == YesNoNotSet.Yes 
                         && workRepairListQuery.Any(y => y.BaseService.Id == x.Id)
                         && workRepairListQuery.Where(y => y.BaseService.Id == x.Id)
                         .All(y => y.DateStart.HasValue && y.PlannedCost.HasValue
                             && WorkRepairDetailTatService.GetAll()
                                .Where(z => z.BaseService.Id == x.Id)
                                .Where(z => z.WorkPpr.GroupWorkPpr.Id == y.GroupWorkPpr.Id)
                                .Where(z => z.UnitMeasure != null)
                                .Any(z => z.PlannedVolume.HasValue))))
                 .Select(x => x.Id)
                 .Distinct()
                 .ToDictionary(x => x);

            return serviceIsDisclosed;
        }


        /// <summary>
        /// Получение информации для исключения
        /// Список на исключение из подсчета:
        ///    тип предоставления услуги = Услуга не предоставляется,
        ///    или Услуга предоставляется без участия УО + есть поставщик(и)
        /// </summary>
        /// <param name="diRoQuery">Список домов для проверки</param>
        /// <returns>Словарь исключенных домов</returns>
        protected override Dictionary<long, long> GetRepairServicesToExcludeDict(IQueryable<DisclosureInfoRealityObj> diRoQuery)
        {
            // Список на исключение из подсчета:
            //    тип предоставления услуги = Услуга не предоставляется,
            //    или Услуга предоставляется без участия УО + есть поставщик(и)

            var repairServicesToExcludeDict = RepairServiceDomain
                .GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                .Where(x => x.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceNotAvailable
                    || (x.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedWithoutMo
                    && ProviderServiceDomain.GetAll().Any(y => y.BaseService.Id == x.Id)))
                .Select(x => x.Id)
                .Distinct()
                .ToDictionary(x => x);

            return repairServicesToExcludeDict;
        }
    }
}