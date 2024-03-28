namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.Gkh.Report;

    using Castle.Windsor;
    using Overhaul.Entities;
    using Bars.Gkh.Overhaul.Enum;
    using Bars.Gkh.Overhaul.Nso.ConfigSections;

    using Gkh.Utils;
    using Utils;

    public class SubsidyMunicipalityService : ISubsidyMunicipalityService
    {
        #region property injection

        public IWindsorContainer Container { get; set; }

        public IDomainService<SubsidyMunicipality> SubsidyMuService { get; set; }

        public IDomainService<SubsidyMunicipalityRecord> SubsidyRecordService { get; set; }

        public IDomainService<DpkrCorrectionStage2> DpkrService { get; set; }

        public IDomainService<StructuralElementWork> StructElJobService { get; set; }

        public IDomainService<WorkTypeFinSource> WorkFinSourceService { get; set; }

        public IDomainService<RealityObjectStructuralElement> RoStructElService { get; set; }

        public IDomainService<WorkPrice> WorkPriceService { get; set; }

        public IDomainService<VersionRecord> VersionRecordDomain { get; set; }

        public IDomainService<ProgramVersion> VersionDomain { get; set; }

        public IDomainService<SubsidyRecordVersionData> SubsidyRecordVersionDomain { get; set; }

        public IDomainService<RealityObject> RoDomain { get; set; }

        public IDomainService<VersionRecordStage2> VersionStage2Domain { get; set; }

        public IDomainService<VersionRecordStage1> VersionStage1Domain { get; set; } 

        #endregion property injection

        public IDataResult GetSubsidy(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("municipalityId");
            var versionId = baseParams.Params.GetAs<long>("versionId");

            if( id <= 0 )
                throw new ValidationException(string.Format("Передан не верный идентификатор МО {0}", id));

            var domainSubsidy = Container.Resolve<IDomainService<SubsidyMunicipality>>();
            var domainMunicipality = Container.Resolve<IDomainService<Municipality>>();

            var subsidy = domainSubsidy.GetAll().FirstOrDefault(x => x.Municipality.Id == id);

            if (subsidy == null)
            {
                subsidy = new SubsidyMunicipality() { Municipality = domainMunicipality.Load(id) };
                domainSubsidy.Save(subsidy);
            }

            CreateVersionedSubsidyRecordsIfNotExist(subsidy.Id, versionId);
            
            Container.Release(domainSubsidy);
            Container.Release(domainMunicipality);

            return new BaseDataResult(subsidy);
        }

        /// <summary>
        /// Данный метод расчитывает Потребность в финансировании
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult CalcFinanceNeedBefore(BaseParams baseParams)
        {
            var subsidyId = baseParams.Params.GetAs<long>("subsidyId");
            var versionId = baseParams.Params.GetAs<long>("versionId");

            if (subsidyId <= 0)
                throw new ValidationException(string.Format("Передан неверный идентификатор Субсидии {0}", subsidyId));

            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var periodStart = config.ProgrammPeriodStart;

            //Субсидия
            var subsidy = SubsidyMuService.Load(subsidyId);
            
            //Словарь Сумм программы по Году
            var programmData = this.VersionRecordDomain.GetAll()
                                            .Where(x => x.ProgramVersion.Id == versionId)
                                            .Where(x => x.RealityObject.Municipality.Id == subsidy.Municipality.Id)
                                            .Select(x => new { x.Year, S = x.Sum })
                                            .AsEnumerable()
                                            .GroupBy(x => x.Year)
                                            .ToDictionary(x => x.Key, y => y.Sum(x => x.S));

            var data = SubsidyRecordVersionDomain.GetAll()
                                    .Where(
                                           x =>
                                               x.SubsidyRecordUnversioned.SubsidyMunicipality.Id == subsidyId
                                               && x.Version.Id == versionId)
                                    .ToList();

            int n; // порядковый номер года в периоде ДПКР
            //Заполняем потребность в средствах собственниках
            foreach (var rec in data.OrderBy(x => x.SubsidyRecordUnversioned.SubsidyYear))
            {
                var yearDiff = (rec.SubsidyRecordUnversioned.SubsidyYear - periodStart);

                n = yearDiff;

                if (programmData.ContainsKey(rec.SubsidyRecordUnversioned.SubsidyYear))
                {
                    if (!subsidy.ConsiderInflation)
                    {
                        rec.FinanceNeedBefore = programmData[rec.SubsidyRecordUnversioned.SubsidyYear];
                    }
                    else
                    {
                        rec.FinanceNeedBefore = programmData[rec.SubsidyRecordUnversioned.SubsidyYear]
                                                * (decimal)Math.Pow((double)(1 + subsidy.CoefAvgInflationPerYear.ToDivisional()), n);
                    }
                }
                else
                {
                    rec.FinanceNeedBefore = 0;
                }

                SubsidyRecordVersionDomain.Update(rec);
            }
            
            return new BaseDataResult();
        }

        /// <summary>
        /// Метод расчета показателей
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult CalcValues(BaseParams baseParams)
        {
            var subsidyId = baseParams.Params.GetAs<long>("subsidyId");
            var verionId = baseParams.Params.GetAs<long>("versionId");

            if (subsidyId == 0)
                throw new ValidationException("Передан неверный идентификатор Субсидии");
            if (verionId == 0)
                throw new ValidationException("Не выбрана версия ДПКР");

            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var periodStart = config.ProgrammPeriodStart;
            
            var subsidy = SubsidyMuService.Get(subsidyId);
            
            /*сумма значений поля "в т.ч. жилых всего" жилых домов с типом Многоквартирный*/
            var sumRO = RoDomain.GetAll()
                        .Where(x => x.Municipality.Id == subsidy.Municipality.Id && x.TypeHouse == TypeHouse.ManyApartments)
                        .Sum(x => x.AreaLiving)
                        .ToDecimal();

            var subsidyRecords = SubsidyRecordService.GetAll()
                                    .Where(x => x.SubsidyMunicipality.Id == subsidyId)
                                    .OrderBy(x => x.SubsidyYear)
                                    .ToList();
            var subsidyRecordIds = subsidyRecords.Select(x => x.Id).ToList();

            var versionData = SubsidyRecordVersionDomain.GetAll()
                                                        .Where(
                                                               x =>
                                                                   subsidyRecordIds.Contains(x.SubsidyRecordUnversioned.Id)
                                                                   && x.Version.Id == verionId)
                                                        .ToList();

            /* Осуществили вычисление показателей */
            subsidy.CalculationCompleted = true;

            SubsidyMuService.Update(subsidy);

            var startTarifDenominator =
                versionData.Sum(
                                x =>
                                    (decimal)
                                        Math.Pow((double)(1 + subsidy.CoefGrowthTarif.ToDivisional()),
                                            x.SubsidyRecordUnversioned.SubsidyYear - periodStart) * 12 * sumRO);

            // Начальный рекомендуемый тариф
            var startRecomendTarif = startTarifDenominator != 0
                ? (versionData.Sum(x => x.FinanceNeedBefore)
                   - (versionData.Sum(x => x.SubsidyRecordUnversioned.BudgetFund)
                      + versionData.Sum(x => x.SubsidyRecordUnversioned.BudgetRegion)
                      + versionData.Sum(x => x.SubsidyRecordUnversioned.BudgetMunicipality)
                      + versionData.Sum(x => x.SubsidyRecordUnversioned.OtherSource))) / startTarifDenominator
                : 0;

            int n = 1, iteration = 0 /*шаг цикла*/;
            //заполняем поля Расчетная собираемость, Прогнозируемая собираемость, Дефицит
            foreach (var rec in subsidyRecords.OrderBy(x => x.SubsidyYear))
            {
                var versionRec = versionData.FirstOrDefault(x => x.SubsidyRecordUnversioned.Id == rec.Id);

                ++iteration;
                // Порядковый номер дома в периоде ДПКР - 1
                var yearDiff = (rec.SubsidyYear - periodStart);

                n = yearDiff;

                var powCoef = (decimal)Math.Pow((double)(1 + subsidy.CoefGrowthTarif.ToDivisional()), n);

                //Установленный тариф. В первый год устанавливается из значения начального тарифа
                rec.EstablishedTarif = iteration == 1 ? subsidy.StartTarif : (subsidy.StartTarif * powCoef);
                
                //Расчетная собираемость - это установленный тариф * СуммуПлощадейДомов
                rec.CalculatedCollection = rec.EstablishedTarif * sumRO * 12;

                //Лимит средств собственников - это Расчетная собираемост * Суммарный рисковый коэффициент"
                rec.OwnersLimit = rec.CalculatedCollection * subsidy.CoefSumRisk;

                if (versionRec != null)
                {
                    //Дифицит До  - Потребность в финансках До - Сумма(Бюджеты + Иные источники). Считаем для версии ДПКР
                    versionRec.DeficitBefore = versionRec.FinanceNeedBefore
                                               - (rec.OwnersLimit + rec.BudgetFund + rec.BudgetRegion
                                                  + rec.BudgetMunicipality + rec.OtherSource);

                    versionRec.RecommendedTarif = iteration == 1 ? startRecomendTarif : startRecomendTarif * powCoef;

                    versionRec.RecommendedTarifCollection = versionRec.RecommendedTarif * sumRO;

                    SubsidyRecordVersionDomain.Update(versionRec);
                }

                /*Рассчитывание долей*/
                var fundsSum = rec.OwnersLimit.ZeroIfBelowZero() + rec.BudgetFund + rec.BudgetRegion + rec.BudgetMunicipality + rec.OtherSource;

                rec.ShareBudgetFund = fundsSum != 0 ? (rec.BudgetFund / fundsSum) : 0;
                rec.ShareBudgetRegion = fundsSum != 0 ? (rec.BudgetRegion / fundsSum) : 0;
                rec.ShareBudgetMunicipality = fundsSum != 0 ? (rec.BudgetMunicipality / fundsSum) : 0;
                rec.ShareOtherSource = fundsSum != 0 ? (rec.OtherSource / fundsSum) : 0;
                rec.ShareOwnerFounds = fundsSum != 0 ? (rec.OwnersLimit / fundsSum) : 0;

                SubsidyRecordService.Update(rec);
            }
            
            return new BaseDataResult();
        }

        /// <summary>
        /// Корректировка ДПКР
        /// </summary>
        public IDataResult CorrectDpkr(BaseParams baseParams)
        {
            var subsidyId = baseParams.Params.GetAs<long>("subsidyId");
            if (subsidyId == 0)
            {
                throw new ArgumentException("Не выбрано муниципальное образование!");
            }

            var subsidy = SubsidyMuService.Get(subsidyId);
            if (subsidy == null)
            {
                throw new ArgumentException("Не выбрано муниципальное образование!");
            }

            var versionId = baseParams.Params.GetAs<long>("versionId");
            
            if (versionId <= 0)
            {
                throw new ArgumentException("Не выбрана версия!");
            }

            /* Удаляем данные по корректировкам для текущего МО */
            Container.UsingForResolved<IDataTransaction>(
            (c, tr) =>
            {
                try
                {
                    var ids = DpkrService.GetAll().Where(x => x.RealityObject.Municipality.Id == subsidy.Municipality.Id && x.Stage2.Stage3Version.ProgramVersion.Id == versionId).Select(x => x.Id).ToArray();
                    ids.ForEach(x => DpkrService.Delete(x));
                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            });

            // Получаем записи, котоыре необходимо скорректировать 
            var correctionList = GetThroughDpkrRows(subsidy, versionId);

            // получаем субсидии, которые потом в дальнейшем будем использовать при расчете
            // этот же массив будет служить для списания денжных средств из лимитов бюджетов
            var subsidyRecordList = SubsidyRecordService.GetAll()
                .Where(x => x.SubsidyMunicipality.Id == subsidy.Id)
                .OrderBy(x => x.SubsidyYear)
                .ToList();

            // Получаем стоимости работ в каждом году
            var workPriceDict = WorkPriceService.GetAll()
                                    .AsEnumerable()
                                    .GroupBy(x => x.Year)
                                    .ToDictionary(x => x.Key, y => y.ToList());

            // В этом словаре будет содержатся кредитная история дома
            // Тоесть во время дальнейших вычислений необходимо понимать брал ли дом Займ
            // Например если у дома был займ то какаято работа уже может переместится в другой год
            // после того как дом отдаст займ за другие работы 
            var creditHistory = new Dictionary<long, RealityObjectCreditHistory>();

            // запускаем рекурсивный метод, который будет вычислять в каком году будет ремонтирвоатся работа
            foreach (var subsidyRecord in subsidyRecordList)
            {
                this.WalkThroughDpkr(correctionList, subsidyRecord, creditHistory, workPriceDict);
            }

            if (correctionList.Count == 0)
            {
                return new BaseDataResult(
                    false,
                    "Расчет не был произведен, возможные причины:"
                    + "<br>Не произведен расчет 3 этапа долгосрочной программы,"
                    + "<br>Неверный год окончания периода долгосрочной программы.");
            }


            var correctionToSave = new List<DpkrCorrectionStage2>();

            foreach (var corr in correctionList)
            {
                var newItem = new DpkrCorrectionStage2()
                {
                    RealityObject = new RealityObject { Id = corr.RealityObjectId },
                    PlanYear = corr.PlanYear,
                    YearCollection = corr.YearCollection,
                    OwnersMoneyBalance = corr.OwnersMoneyBalance,
                    HasCredit = corr.HasCredit,
                    FundBudgetNeed = corr.BudgetFundNeed,
                    MunicipalityBudgetNeed = corr.BudgetMunicipalityNeed,
                    RegionBudgetNeed = corr.BudgetRegionNeed,
                    OtherSourcesBudgetNeed = corr.OtherSourceNeed,
                    OwnersMoneyNeed = corr.OwnersMoneyNeed,
                    BudgetFundBalance = corr.BudgetFundBalance,
                    BudgetMunicipalityBalance = corr.BudgetMunicipalityBalance,
                    BudgetRegionBalance = corr.BudgetRegionBalance,
                    OtherSourceBalance = corr.OtherSourceBalance,
                    Stage2 = new VersionRecordStage2 { Id = corr.Stage2Id },
                    IsOwnerMoneyBalanceCalculated = corr.IsOwnerMoneyBalanceCalculated,
                };

                correctionToSave.Add(newItem);
            }

            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    correctionToSave.ForEach(DpkrService.Save);

                    var subsidyRecords = SubsidyRecordService.GetAll()
                                           .Where(x => x.SubsidyMunicipality.Id == subsidyId)
                                           .ToList();

                    var recordIds = subsidyRecords.Select(x => x.Id)
                                                  .ToList();
                    var versionRecords = SubsidyRecordVersionDomain.GetAll()
                                                                   .Where(
                                                                          x =>
                                                                              recordIds.Contains(
                                                                              x.SubsidyRecordUnversioned.Id)
                                                                              && x.Version.Id == versionId)
                                                                   .ToList();

                    foreach (var item in subsidyRecords)
                    {
                        var versionRecord = versionRecords.FirstOrDefault(x => x.SubsidyRecordUnversioned.Id == item.Id);

                        // получаем стоимость всех работ в этом году
                        var costJobs = correctionList.Where(x => x.PlanYear == item.SubsidyYear).Sum(x => x.CostJobs);

                        if (versionRecord != null)
                        {
                            versionRecord.DeficitAfter = (item.OwnersLimit + item.BudgetFund + item.BudgetRegion
                                                 + item.BudgetMunicipality + item.OtherSource) - costJobs;
                            versionRecord.FinanceNeedAfter = costJobs;

                            SubsidyRecordVersionDomain.Update(versionRecord);
                        }
                    }

                    // Сохраняем состояние что была произведена корректирвока ДПКР
                    subsidy.DpkrCorrected = true;
                    SubsidyMuService.Update(subsidy);

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    throw ex;
                }
            }
            
            return new BaseDataResult();
        }

        public bool CheckCalculation(BaseParams baseParams)
        {
            var subsidyId = baseParams.Params.GetAs<long>("subsidyId");
            if (subsidyId == 0)
            {
                return false;
            }

            var subMun = SubsidyMuService.Get(subsidyId);
            if (subMun == null)
            {
                return false;
            }

            return subMun.CalculationCompleted;
        }

        private List<DpkrCorrectionStage2Proxy> GetThroughDpkrRows(SubsidyMunicipality subsidy, long versionId)
        {
            // В этом массиве в итоге будет храниться список Корректировок
            var correctData = new List<DpkrCorrectionStage2Proxy>();

            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var year = config.ProgrammPeriodEnd;

            // Получаем идентификаторы структурных элементов, которые содержатся в 1 этапе
            // для домов относящихся к переданному МО
            var str1Ids = VersionStage1Domain.GetAll()
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                .Where(x => x.StructuralElement.RealityObject.Municipality.Id == subsidy.Municipality.Id)
                .Where(x => x.Year <= year)
                .Select(x => x.StructuralElement.StructuralElement.Id)
                .Distinct()
                .ToList();

            // Источники финансирования по видам работ
            // Он необходим для того, чтобы понимать к какому истонику относится вид работы.
            // В дальнейшем будем Понимать Какая работа к каому Источнику относится 
            var workFinanceSources = WorkFinSourceService.GetAll()
                .Select(x => new { WorkId = x.Work.Id, x.TypeFinSource })
                .ToList();

            // Получаем словарь  < КЭДома , Объем >
            // Для того чтобы потом подсчитывать по дому для необходимый объем 
            var volumeDict = RoStructElService.GetAll()
                                 .Where(x => x.RealityObject.Municipality.Id == subsidy.Municipality.Id)
                                 .Select(x => new { x.Id, x.Volume })
                                 .AsEnumerable()
                                 .GroupBy(x => x.Id)
                                 .ToDictionary(x => x.Key, y => y.Sum(x => x.Volume));

            // Получаем словарь вид работы - конструктивные элементы
            // Но при этом нам нужны только те структурные элементы 
            // котоыре участвуют в 1 Этапе
            var strElementsJobs = StructElJobService.GetAll()
                    .Select(x => new { WorkTypeId = x.Job.Work.Id, JobId = x.Job.Id, StructElId = x.StructuralElement.Id })
                    .AsEnumerable()
                    .Where(x => str1Ids.Contains(x.StructElId))
                    .GroupBy(x => x.StructElId)
                    .ToDictionary(x => x.Key, y => y.ToList());

            // Получаем для вида работ список
            var stage1Dict = VersionStage1Domain.GetAll()
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                .Where(x => x.Year <= year && x.StructuralElement.RealityObject.Municipality.Id == subsidy.Municipality.Id)
                .Select(x => new
                {
                    x.Id,
                    RoStrId = x.StructuralElement.Id,
                    RoId = x.StructuralElement.RealityObject.Id,
                    StrId = x.StructuralElement.StructuralElement.Id,
                    Stage2Id = x.Stage2Version.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.Stage2Id)
                .ToDictionary(x => x.Key, y => y.Select(x => new { x.Id, x.RoStrId, x.StrId }));

            // Получаем для вида работ список
            var stage2DataWorkSection = VersionStage2Domain.GetAll()
                .Where(x => x.Stage3Version.ProgramVersion.Id == versionId)
                .Where(x => x.Stage3Version.Year <= year && x.Stage3Version.RealityObject.Municipality.Id == subsidy.Municipality.Id)
                .Select(x => new
                {
                    x.Id,
                    RoId = x.Stage3Version.RealityObject.Id,
                    CeoId = x.CommonEstateObject.Id,
                    x.Stage3Version.RealityObject.AreaLiving,
                    x.CommonEstateObjectWeight,
                    x.Stage3Version.Year,
                    x.Stage3Version.IndexNumber
                })
                .ToList();

            //проходим по записям 2го этапа 
            foreach (var item in stage2DataWorkSection)
            {
                //Если для 2го этапа ненашли записи 1го этапа то преходим к следующей строке
                if (!stage1Dict.ContainsKey(item.Id))
                    continue;

                var st1Records = stage1Dict[item.Id];
                var listJobs = new List<CommonEstateObjectJobProxy>();

                foreach (var st1 in st1Records)
                {
                    // По КЭ находим работы, если нет таких то выходим из цикла
                    if (!strElementsJobs.ContainsKey(st1.StrId))
                        continue;

                    var vol = 0M;
                    if (volumeDict.ContainsKey(st1.RoStrId))
                    {
                        vol = volumeDict[st1.RoStrId];
                    }

                    // Для каждой работы структурного элемента
                    // получаем из Какого источника он будет финансироватся 
                    foreach (var job in strElementsJobs[st1.StrId])
                    {
                        var jobProxy = new CommonEstateObjectJobProxy
                        {
                            RoStructuralElementId = st1.RoStrId,
                            StructuralElementId = st1.StrId,
                            WorkTypeId = job.WorkTypeId,
                            JobId = job.JobId,
                            Volume = vol,
                            IsFinancedFromFundBudget = workFinanceSources.Any(x => x.WorkId == job.WorkTypeId && x.TypeFinSource == TypeFinSource.Fund),
                            IsFinancedFromRegionBudget = workFinanceSources.Any(x => x.WorkId == job.WorkTypeId && x.TypeFinSource == TypeFinSource.Region),
                            IsFinancedFromMunicipalityBudget = workFinanceSources.Any(x => x.WorkId == job.WorkTypeId && x.TypeFinSource == TypeFinSource.Municipality),
                            IsFinancedFromOtherSources = workFinanceSources.Any(x => x.WorkId == job.WorkTypeId && x.TypeFinSource == TypeFinSource.Other),
                            IsFinancedFromOwnersMoney = workFinanceSources.Any(x => x.WorkId == job.WorkTypeId && x.TypeFinSource == TypeFinSource.Owners)
                        };

                        listJobs.Add(jobProxy);
                    }

                }

                // Теперь для каждой записи 1 этапа создаем начальную запись
                var cd = new DpkrCorrectionStage2Proxy
                {
                    Stage2Id = item.Id,
                    RealityObjectId = item.RoId,
                    CeoId = item.CeoId,
                    AreaLiving = item.AreaLiving.HasValue ? item.AreaLiving.Value : 0,
                    PlanYear = item.Year,
                    FirstPlanYear = item.Year,
                    CommonEstateObjectWeight = item.CommonEstateObjectWeight,
                    Jobs = listJobs
                };

                correctData.Add(cd);
            }

            return correctData;
        }

        /// <summary>
        /// В данном методе для Строки Субсидирования выбираются все ООИ и рассматривается 
        /// может ли ООИ отремонтироватся в текущем году, также в этом методе в начале каждого года идет пересчет Кредитной истории дома
        /// </summary>
        private void WalkThroughDpkr(List<DpkrCorrectionStage2Proxy> latterCorrection,
                                                            SubsidyMunicipalityRecord subsidyRecord,
                                                            Dictionary<long, RealityObjectCreditHistory> creditHistory,
                                                            Dictionary<int, List<WorkPrice>> workPriceDict)
        {
            /* 
               Если в списке для обхода нет записей,
               которые требуеют вычислений, то обрываем реурсию
            */
            if (latterCorrection.All(x => x.IsOwnerMoneyBalanceCalculated))
            {
                return;
            }

            // Сначала для всех оставшихся домов обновляем кредитную историю     
            var roInfo = latterCorrection.Where(x => !x.IsOwnerMoneyBalanceCalculated)
                                .Select(x => new { x.RealityObjectId, x.AreaLiving })
                                .GroupBy(x => x.RealityObjectId)
                                .ToDictionary(x => x.Key, y => y.Select(x => x.AreaLiving).FirstOrDefault());

            foreach (var ro in roInfo)
            {
                /*
                  Берем кредитную истоию дома, если еще займов небыло,
                  то просто создаем пустую запись с начальными значениями
               */
                RealityObjectCreditHistory credit = null;
                if (!creditHistory.TryGetValue(ro.Key, out credit))
                {
                    /*
                        Поскольку займов еще небыло, то начинаем кредитную историю заполняя ее начльными значениями
                    */
                    credit = new RealityObjectCreditHistory()
                    {
                        RealityObject = ro.Key,
                        AreaLiving = ro.Value,
                        Balance = ro.Value * subsidyRecord.EstablishedTarif * 12,
                        CollectionYear = ro.Value * subsidyRecord.EstablishedTarif * 12,
                        CreditYear = subsidyRecord.SubsidyYear,
                        RepaymentYear = subsidyRecord.SubsidyYear
                    };

                    creditHistory.Add(ro.Key, credit);
                }
                else
                {
                    /*
                      Поскольку кредитная история уже была, то значит что мы перешли в следующий год
                      и необходимо обновить значения
                     */

                    // к Накопленной сумме прибавляем 
                    credit.Balance += credit.CollectionYear;
                }
            }

            /*
               Сначала получаем Плановый год 
            */
            var data = latterCorrection.Where(x => !x.IsOwnerMoneyBalanceCalculated && x.PlanYear == subsidyRecord.SubsidyYear)
                                .OrderBy(x => x.Index)
                                .ThenByDescending(x => x.CommonEstateObjectWeight)
                                .ToList();

            /*
                 Теперь по отсортированной выборке мы проходим по всем объектам ООИ в выбранном году
                 И выясняем какие ООИ попадут в этот год а какие перейдут дальше
            */
            foreach (var correctRecord in data)
            {
                // Получаем кредитную историю по Дому для рассматриваемого ООИ
                var credit = creditHistory[correctRecord.RealityObjectId];

                var workPriceInYear = new List<WorkPrice>();

                if (workPriceDict.ContainsKey(subsidyRecord.SubsidyYear))
                    workPriceInYear = workPriceDict[subsidyRecord.SubsidyYear];

                CalculateCorrections(correctRecord, credit, subsidyRecord, workPriceInYear);
            }

        }

        private void CalculateCorrections(
            DpkrCorrectionStage2Proxy correctRecord,
            RealityObjectCreditHistory credit,
            SubsidyMunicipalityRecord subsidyRecord,
            List<WorkPrice> workPriceInYear)
        {

            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var endYear = config.ProgrammPeriodEnd;

            /*
             проверяем брал ли дом зам, если год полного погашения займа > чем рассматриваемый год
             то такое ООИ мы переносим наследующий год до тех пор пока Дом полностью не расплатится с кредитом
             * Если собираемость в год у дома = 0 рублей то тоже перекидываем на след год
            */
            if (credit.RepaymentYear > subsidyRecord.SubsidyYear || credit.CollectionYear <= 0)
            {
                // поскольку выяснилось что дом расплатится с кредитом не в этом году а в будующем
                // то перебрасываем ООИ на следующий год и прекращаем расчет
                this.ShiftYear(correctRecord, endYear);
                return;
            }

            /*
               Вычисляем сколько необходимо денег на все работы по данному ООИ
            */

            var costJobs = 0M; // сколько необходимо средств на все работы по ООИ
            var fundBudgetNeed = 0M; // сколько необходимо из бюджета Фонда
            var regionBudgetNeed = 0M; // сколько необходимо из бюджета региона
            var municipalityBudgetNeed = 0M; // сколько необходимо из бюджета МО
            var otherSourcesNeed = 0M; // сколько необходимо из иных источников
            var ownersMoneyNeed = 0M; // сколько необходимо из Средств собственников

            // теперь проходим по каждой работе и распеределяем деньги
            foreach (var job in correctRecord.Jobs)
            {
                // получаем стоимость работы относительно Года расчета
                var cost = 0M;

                var price = workPriceInYear.FirstOrDefault(x => x.Job.Id == job.JobId);
                if (price != null)
                {
                    cost = price.NormativeCost * job.Volume;
                }

                // Если стоимость неопределена то дальше нет смысла продолжать
                if (cost == 0) continue;

                // Прибавляем текущую стоимость к общей стоимости
                costJobs += cost;

                // высчитываем общий знаменатель относительно того что Вид работы может финансироваться из разных источников
                var commonDenominator = subsidyRecord.ShareBudgetFund * job.IsFinancedFromFundBudget.ToInt()
                                        + subsidyRecord.ShareBudgetRegion * job.IsFinancedFromRegionBudget.ToInt()
                                        + subsidyRecord.ShareBudgetMunicipality
                                          * job.IsFinancedFromMunicipalityBudget.ToInt()
                                        + subsidyRecord.ShareOtherSource * job.IsFinancedFromOtherSources.ToInt()
                                        + subsidyRecord.ShareOwnerFounds * job.IsFinancedFromOwnersMoney.ToInt();

                fundBudgetNeed += commonDenominator != 0 && job.IsFinancedFromFundBudget
                                      ? (cost * (subsidyRecord.ShareBudgetFund / commonDenominator))
                                      : 0;
                regionBudgetNeed += commonDenominator != 0 && job.IsFinancedFromRegionBudget
                                        ? (cost * (subsidyRecord.ShareBudgetRegion / commonDenominator))
                                        : 0;
                municipalityBudgetNeed += commonDenominator != 0 && job.IsFinancedFromMunicipalityBudget
                                              ? (cost * (subsidyRecord.ShareBudgetMunicipality / commonDenominator))
                                              : 0;
                otherSourcesNeed += commonDenominator != 0 && job.IsFinancedFromOtherSources
                                        ? (cost * (subsidyRecord.ShareOtherSource / commonDenominator))
                                        : 0;
                ownersMoneyNeed += commonDenominator != 0 && job.IsFinancedFromOwnersMoney
                                       ? (cost * (subsidyRecord.ShareOwnerFounds / commonDenominator))
                                       : 0;
            }

            /*
             после того как выяснили сколько денег необходимо из разных бюджетов мы должны понять 
             Хватит ли денег в Бюджетах чтобы осуществить выполнение Работ в этой году 
            */

            if (fundBudgetNeed > subsidyRecord.BudgetFund || regionBudgetNeed > subsidyRecord.BudgetRegion
                || municipalityBudgetNeed > subsidyRecord.BudgetMunicipality
                || otherSourcesNeed > subsidyRecord.OtherSource || ownersMoneyNeed > subsidyRecord.OwnersLimit)
            {

                // Значит по какомуто бюджету нехватает денег на выполнение этой работы
                // следовательно необходимо перекинуть год на следующий и рассмотреть ООИ в следующем году
                this.ShiftYear(correctRecord, endYear);
                return;
            }

            if (costJobs > 0
                && (fundBudgetNeed + regionBudgetNeed + municipalityBudgetNeed + otherSourcesNeed + ownersMoneyNeed)
                <= 0)
            {
                // Значит незабиты или неправилньо забиты лимиты в субсидирвоании и переносим оои на след год
                this.ShiftYear(correctRecord, endYear);
                return;
            }

            /*
              Если денег по бюджетам хватает, то проверяем относительно баланса дома сможет ли дом отремонтировать своими средставми
            */
            if (credit.Balance < ownersMoneyNeed)
            {
                // Если денег у дома на текущий момент недостаточно чтобы выполнить эти работы
                // проверяем с учетом займа сможет ли дом расплатится 

                // Получаем сумму займа которая требуется на выполнение этих работ
                var loan = ownersMoneyNeed - credit.Balance;

                // Получаем срок займа в течении которого дом расплатиться за этот кредит
                // Займ делим на собираемость в год => получаем количество лет
                var loanYears = 1 + (int)(loan / credit.CollectionYear);

                // Если Количество лет займа больше чем возможный срок кредита 
                // То нельзя дому давать займ и перекидываем в следующий год
                if (loanYears > subsidyRecord.SubsidyMunicipality.DateReturnLoan)
                {
                    // Значит что дом невсилах расплатится в Максимально возможный срок кредита
                    // и переносим на следующий год
                    this.ShiftYear(correctRecord, endYear);
                    return;
                }

                // Если мы дошли досюда, значит мы поняли что дому необходимо дать Займ
                credit.CreditYear = correctRecord.PlanYear; // Выставляем год взятия Займа
                credit.RepaymentYear = correctRecord.PlanYear + loanYears; // Выставляем год расчета по кредиту
                correctRecord.HasCredit = true;
            }

            // Если мы дошли до сюда, значит что ООИ будет ремонтироватся в этом году 
            // А следовательно необходимо списать средства с баланса дома 
            // И из бюджетных лимитов мы вычитаем соответсвующие суммы
            credit.Balance -= ownersMoneyNeed; // Списываем деньги с Баланса дома Поскольку мы поняли что работа будет

            // Далее списываем деньги из бюджетов
            subsidyRecord.BudgetFund -= fundBudgetNeed;
            subsidyRecord.BudgetMunicipality -= municipalityBudgetNeed;
            subsidyRecord.BudgetRegion -= regionBudgetNeed;
            subsidyRecord.OtherSource -= otherSourcesNeed;
            subsidyRecord.OwnersLimit -= ownersMoneyNeed;

            // Далее фиксируем что запись пройшла расчет и будет ремонтироваться в Плановом году
            correctRecord.IsOwnerMoneyBalanceCalculated = true;

            // Фиксируем текущее состояние лимитов котрое получилось врезультате списания Денег на Работы текущего ООИ
            correctRecord.BudgetFundBalance = subsidyRecord.BudgetFund;
            correctRecord.BudgetRegionBalance = subsidyRecord.BudgetRegion;
            correctRecord.BudgetMunicipalityBalance = subsidyRecord.BudgetMunicipality;
            correctRecord.OtherSourceBalance = subsidyRecord.OtherSource;
            correctRecord.OwnerLimitBalance = subsidyRecord.OwnersLimit;

            // Далее фиксируем расчитанные показатели
            correctRecord.YearCollection = credit.CollectionYear;
            correctRecord.CostJobs = costJobs;
            correctRecord.OwnersMoneyBalance = credit.Balance;
            correctRecord.BudgetFundNeed = fundBudgetNeed;
            correctRecord.BudgetRegionNeed = regionBudgetNeed;
            correctRecord.BudgetMunicipalityNeed = municipalityBudgetNeed;
            correctRecord.OtherSourceNeed = otherSourcesNeed;
            correctRecord.OwnersMoneyNeed = ownersMoneyNeed;
        }

        /// <summary>
        /// в этом методе мы перекидваем ООИ на следующий год рассмотрения
        /// Если мы достигли максимального года, то завершаем работу с данным ООИ
        /// </summary>
        private void ShiftYear(DpkrCorrectionStage2Proxy correctRecord, int endYear)
        {
            correctRecord.PlanYear++;

            correctRecord.IsOwnerMoneyBalanceCalculated = correctRecord.PlanYear > endYear;
        }

        private void CreateVersionedSubsidyRecordsIfNotExist(long id, long versionId)
        {
            var alreadyrecords = SubsidyRecordService.GetAll()
                                                     .Where(x => x.SubsidyMunicipality.Id == id)
                                                     .Select(x => x.Id)
                                                     .ToList();

            if (!SubsidyRecordVersionDomain.GetAll()
                                          .Any(x => x.Version.Id == versionId && alreadyrecords.Contains(x.SubsidyRecordUnversioned.Id)))
            {
                using (var tr = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        foreach (var subsidyRecordId in alreadyrecords)
                        {
                            SubsidyRecordVersionDomain.Save(new SubsidyRecordVersionData
                            {
                                Version = new ProgramVersion { Id = versionId },
                                SubsidyRecordUnversioned = new SubsidyMunicipalityRecord { Id = subsidyRecordId }
                            });
                        }

                        tr.Commit();
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Получение словаря значений капитального ремонта
        /// </summary>
        public Stream PrintReport(BaseParams baseParams)
        {
            var stream = new MemoryStream();

            var report = Container.Resolve<IGkhBaseReport>("SubsidyList");

            var reportProvider = Container.Resolve<IGkhReportProvider>();

            //собираем сборку отчета и формируем reportParams
            var reportParams = new ReportParams();
            report.BaseParams = baseParams;
            report.SetUserParams(new UserParamsValues());
            report.PrepareReport(reportParams);

            //получаем Генератор отчета
            var generatorName = report.GetReportGenerator();

            var generator = Container.Resolve<IReportGenerator>(generatorName);

            reportProvider.GenerateReport(report, stream, generator, reportParams);

            return stream;
        }
    }

    /// <summary>
    /// Информация по кредиту дома
    /// Здесь будет информация по текущему займу дома
    /// В котором можно узнать сколько Дом взял в займы денег, 
    /// в какой год полностью расчитается с займом
    /// </summary>
    public class RealityObjectCreditHistory
    {
        /// <summary>
        /// Дом, который берет займ 
        /// </summary>
        public long RealityObject { get; set; }

        /// <summary>
        /// Жилая площадь дома
        /// </summary>
        public decimal AreaLiving { get; set; }

        /// <summary>
        /// Накопления
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// Собираемость собственников в Год
        /// </summary>
        public decimal CollectionYear { get; set; }

        /// <summary>
        /// Год займа
        /// </summary>
        public int CreditYear { get; set; }

        /// <summary>
        /// Год возврата займа
        /// </summary>
        public int RepaymentYear { get; set; }
    }

    /// <summary>
    /// Сущность, содержащая данные, необходимые при учете корректировки ДПКР
    /// Лимит займа, Дефицит ...
    /// </summary>
    public class DpkrCorrectionStage2Proxy : BaseEntity
    {
        /// <summary>
        /// Id Объекта недвижимости
        /// </summary>
        public virtual long RealityObjectId { get; set; }

        /// <summary>
        /// Идентификатор ООИ
        /// </summary>
        public virtual long CeoId { get; set; }

        /// <summary>
        /// Скорректированный год который получился врезультате субсидирования
        /// </summary>
        public virtual int PlanYear { get; set; }

        /// <summary>
        /// Собираемость к году
        /// </summary>
        public virtual decimal YearCollection { get; set; }

        /// <summary>
        /// Остаток средств собственников
        /// </summary>
        public virtual decimal OwnersMoneyBalance { get; set; }

        /// <summary>
        /// Есть непогашенный кредит
        /// </summary>
        public virtual bool HasCredit { get; set; }

        /// <summary>
        /// Остаток в Бюджете фонда
        /// </summary>
        public virtual decimal BudgetFundBalance { get; set; }

        /// <summary>
        /// Остаток в Бюджете региона
        /// </summary>
        public virtual decimal BudgetRegionBalance { get; set; }

        /// <summary>
        /// Остаток в  Бюджете МО
        /// </summary>
        public virtual decimal BudgetMunicipalityBalance { get; set; }

        /// <summary>
        /// Остаток в  Дриугих источников
        /// </summary>
        public virtual decimal OtherSourceBalance { get; set; }

        /// <summary>
        /// Остаток в фонде собственников
        /// </summary>
        public virtual decimal OwnerLimitBalance { get; set; }

        /// <summary>
        /// Деньги из бюджета фонда
        /// </summary>
        public virtual decimal BudgetFundNeed { get; set; }

        /// <summary>
        /// Деньги из бюджета региона
        /// </summary>
        public virtual decimal BudgetRegionNeed { get; set; }

        /// <summary>
        /// Деньги из бюджета МО
        /// </summary>
        public virtual decimal BudgetMunicipalityNeed { get; set; }

        /// <summary>
        /// Деньги из Дриугих источники
        /// </summary>
        public virtual decimal OtherSourceNeed { get; set; }

        /// <summary>
        /// Потребность в финансировании собственников
        /// </summary>
        public virtual decimal OwnersMoneyNeed { get; set; }

        /// <summary>
        /// Признак того, что строка поучавствовала в расчете баланса собственников и ее считать заново не надо
        /// </summary>
        public virtual bool IsOwnerMoneyBalanceCalculated { get; set; }

        /// <summary>
        /// стоимость работ в том году в котором хотят ремонтирвоать эту услугу
        /// </summary>
        public virtual decimal CostJobs { get; set; }

        /// <summary>
        /// Год который был до субсидирования по результату 3 го этапа
        /// </summary>
        public virtual int FirstPlanYear { get; set; }

        /// <summary>
        /// Жилая площадь дома
        /// </summary>
        public virtual decimal AreaLiving { get; set; }

        /// <summary>
        /// Идекс очередности из 3 этапа
        /// </summary>
        public virtual int Index { get; set; }

        /// <summary>
        /// Идентификатор 2 этапа
        /// </summary>
        public virtual long Stage2Id { get; set; }

        /// <summary>
        /// Вес ООИ
        /// </summary>
        public virtual int CommonEstateObjectWeight { get; set; }

        /// <summary>
        /// Работы которые необходимо произвести в рамках ООИ
        /// </summary>
        public List<CommonEstateObjectJobProxy> Jobs { get; set; }
    }

    /// <summary>
    /// Работа ООИ
    /// </summary>
    public class CommonEstateObjectJobProxy
    {
        /// <summary>
        /// Конструктивный элемент в доме
        /// </summary>
        public long RoStructuralElementId { get; set; }

        /// <summary>
        /// Конструктивный элемент
        /// </summary>
        public long StructuralElementId { get; set; }

        /// <summary>
        /// Объем
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// Вид работы
        /// </summary>
        public long WorkTypeId { get; set; }

        /// <summary>
        /// Работа
        /// </summary>
        public long JobId { get; set; }

        /// <summary>
        /// Финансируется из бюджета фонда
        /// </summary>
        public virtual bool IsFinancedFromFundBudget { get; set; }

        /// <summary>
        /// Финансируется из бюджета региона
        /// </summary>
        public virtual bool IsFinancedFromRegionBudget { get; set; }

        /// <summary>
        /// Финансируется из бюджета МО
        /// </summary>
        public virtual bool IsFinancedFromMunicipalityBudget { get; set; }

        /// <summary>
        /// Финансируется из иных источников
        /// </summary>
        public virtual bool IsFinancedFromOtherSources { get; set; }

        /// <summary>
        /// Финансируется из средств собственников
        /// </summary>
        public virtual bool IsFinancedFromOwnersMoney { get; set; }

        /// <summary>
        /// Сколько денег нужно из бюджета Фонда
        /// </summary>
        public virtual decimal FundBudgetNeed { get; set; }

        /// <summary>
        /// Сколько денег нужно из бюджета региона
        /// </summary>
        public virtual decimal RegionBudgetNeed { get; set; }

        /// <summary>
        /// Сколько денег нужно из бюджета МО
        /// </summary>
        public virtual decimal MunicipalityBudgetNeed { get; set; }

        /// <summary>
        /// Сколько денег нужно из иных источников
        /// </summary>
        public virtual decimal OtherSourcesNeed { get; set; }

        /// <summary>
        /// Сколько денег нужно из Средств собственников
        /// </summary>
        public virtual decimal OwnersMoneyNeed { get; set; }

        /// <summary>
        /// Сколько денег нужно на выполнение всей работы
        /// </summary>
        public virtual decimal Cost { get; set; }
    }
}