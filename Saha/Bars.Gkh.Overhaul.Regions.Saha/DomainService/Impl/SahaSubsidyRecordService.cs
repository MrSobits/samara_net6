namespace Bars.Gkh.Overhaul.Regions.Saha.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.DomainService.Dict.RealEstateType;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.RealEstateType;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.DomainService;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Regions.Saha.Entities.RealEstateType;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using NHibernate;

    public class SahaSubsidyRecordService : ISubsidyRecordService
    {
        #region Property injection

        public IWindsorContainer Container { get; set; }
        public IHmaoRealEstateTypeService RealEstateService { get; set; }
        public IDomainService<SubsidyRecord> SubsidyRecordDomain { get; set; }
        public IDomainService<SubsidyRecordVersion> SubsidyRecordVersionDomain { get; set; }
        public IDomainService<DpkrCorrectionStage2> DpkrCorrectionServiceDomain { get; set; }
        public IDomainService<VersionRecord> VersionRecordDomain { get; set; }
        public IDomainService<ProgramVersion> VersionDomain { get; set; }
        public IDomainService<RealityObject> RoDomain { get; set; }
        public IDomainService<EmergencyObject> EmergencyDomain { get; set; }
        public IDomainService<VersionRecordStage2> VersionStage2Domain { get; set; }
        public IDomainService<VersionRecordStage1> VersionStage1Domain { get; set; }
        public IDomainService<PublishedProgramRecord> PublishedRecordDomain { get; set; }
        public IDomainService<DpkrCorrectionStage2> DpkrService { get; set; }
        public IDomainService<SahaRealEstateTypeRate> TypeRateDomain { get; set; }
        public IDomainService<RealEstateTypeRealityObject> TypeRealityObjectDomain { get; set; }
        public IDomainService<ShortProgramDifitsit> ShortProgramDifitsitDomain { get; set; }
        public IDomainService<ShortProgramRecord> ShortProgramRecordDomain { get; set; }
        public IDomainService<Municipality> MunicipalityDomain { get; set; }
        public IDomainService<ShareFinancingCeo> ShareFinCeoDomain { get; set; }

        #endregion Property injection

        /// <summary>
        /// В данном методе мы либо создаем судсидии если в периоде ДПКР они еще несозданы, либо удаляем субсидии если период был изменен
        /// но записи предположим существовали
        /// </summary>
        public IDataResult GetSubsidy(BaseParams baseParams)
        {
            var moId = baseParams.Params.GetAs<long>("mo_id");

            if (moId <= 0)
            {
                throw new ValidationException(string.Format("Невозможно определить муниципальное образование по id: {0}!", moId));
            }

            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var periodStart = config.ProgrammPeriodStart;
            var periodEnd = config.ProgrammPeriodEnd;

            return GetSubsidyByMu(moId, periodStart, periodEnd);
        }

        /// <summary>
        /// В этом методе мы рассчитываем собираемость
        /// последовательность действий:
        ///   1. Сохраняем записи которые пришли измененные
        ///   2. Получаем Социальные тарифы по типам домов 
        ///   3. Считаем сумму Плановая Собираемость по формуле "СуммаПлощади * СоцТарифПоТипу * 12"
        ///   4. Считаем средства СобственниковНаКР и ОстатокЗаПредыдущийГод
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult UpdateSaldoBallance(BaseParams baseParams)
        {
            var moId = baseParams.Params.GetAs<long>("muId");

            if (moId <= 0)
            {
                throw new ValidationException("Не удалось получить муниципальное образование");
            }

            decimal saldoBallance = 0;

            SubsidyRecordVersionDomain.GetAll()
              .Where(x => x.SubsidyRecord.Municiaplity.Id == moId && x.Version.IsMain)
              .OrderBy(x => x.SubsidyYear).ForEach(x =>
              {
                  x.SaldoBallance = saldoBallance + x.OwnerSumForCr - x.CorrectionFinance - x.AdditionalExpences;
                  saldoBallance = saldoBallance + x.OwnerSumForCr - x.CorrectionFinance - x.AdditionalExpences;
                  SubsidyRecordVersionDomain.Update(x);
              });
            return new BaseDataResult();
        }

        public IDataResult GetSubsidyByMu(long muId, int periodStart, int periodEnd)
        {
            var municipality = MunicipalityDomain.Load(muId);

            var version = VersionDomain.GetAll().FirstOrDefault(x => x.IsMain && x.Municipality.Id == muId);

            if (version == null)
            {
                return new BaseDataResult(false, "Не найдена основная версия!");
            }

            // Получаем существующие записи субсидирования по МО
            var subsidyRecordsDicts =
                SubsidyRecordDomain.GetAll()
                    .Where(x => x.Municiaplity.Id == muId)
                    .Select(x => new { x.Id, x.SubsidyYear })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => x.FirstOrDefault());

            // Получаем существующие записи для текущей версии (Поскольку часть полей будет там) и МО
            var subsidyVersion = SubsidyRecordVersionDomain.GetAll()
                .Where(x => x.Version.Id == version.Id)
                .Where(x => SubsidyRecordDomain.GetAll()
                    .Where(y => y.Municiaplity.Id == muId)
                    .Any(y => y.Id == x.SubsidyRecord.Id))
                .ToList();

            var listSubsidyToSave = new List<SubsidyRecord>();
            var listVersionToSave = new List<SubsidyRecordVersion>();

            // Далее проверяем изменились ли года в параметрах ДПКР
            var currentYear = periodStart;
            while (currentYear <= periodEnd)
            {
                var rec = subsidyRecordsDicts.Values.FirstOrDefault(x => x.SubsidyYear == currentYear);

                SubsidyRecord tmpRecord;

                // Сначала проверяем существовала ли субсидия на этот год
                // Если нет то создаем новую и записываем в список на сохранение
                if (rec != null)
                {
                    tmpRecord = new SubsidyRecord { Id = rec.Id };
                    // Если запись найдена по этому году то удаляем ее из списка чтобы неудалить потом
                    if (subsidyRecordsDicts.ContainsKey(rec.Id))
                    {
                        subsidyRecordsDicts.Remove(rec.Id);
                    }
                }
                else
                {
                    tmpRecord = new SubsidyRecord
                    {
                        SubsidyYear = currentYear,
                        PlanOwnerPercent = 0,
                        NotReduceSizePercent = 0,
                        Municiaplity = municipality,
                        DateCalcOwnerCollection = DateTime.Now
                    };

                    listSubsidyToSave.Add(tmpRecord);
                }

                // теперь проверяем существовала ли субсидия для этой версии
                // если не было версии то создаем версию для субсидии
                var versionRec = subsidyVersion.FirstOrDefault(x => x.SubsidyRecord.Id == tmpRecord.Id);

                // Если для субсидии нет версии, то тогда мы создаем 
                if (versionRec == null)
                {
                    versionRec =
                        new SubsidyRecordVersion
                        {
                            Version = version,
                            SubsidyRecord = tmpRecord,
                            SubsidyYear = currentYear,
                            PlanOwnerPercent = 0,
                            NotReduceSizePercent = 0,
                            DateCalcOwnerCollection = DateTime.Now
                        };

                    listVersionToSave.Add(versionRec);
                }

                currentYear++;
            }

            using (var tx = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    // Сохраняем субсидирвоание (Если еще небыло сохранено)
                    foreach (var item in listSubsidyToSave)
                    {
                        SubsidyRecordDomain.Save(item);
                    }

                    // Сохраняем версии субсидирвоание (Если еще небыло сохранено)
                    foreach (var item in listVersionToSave)
                    {
                        SubsidyRecordVersionDomain.Save(item);
                    }

                    // Если в списке subsidyRecordsDicts остались какието записи значит, что периоды изменились
                    // но в субсидирвоании остались какието записи
                    foreach (var keyValue in subsidyRecordsDicts)
                    {
                        SubsidyRecordDomain.Delete(keyValue.Key);
                    }

                    tx.Commit();

                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    tx.Rollback();
                    return new BaseDataResult { Success = false, Message = e.Message };
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// В этом методе мы расчитываем собираемость
        /// последовтельность действий:
        ///   1. Сохраняем записи которые пришли измененные
        ///   2. Получаем Социальные тарифы по типам домов 
        ///   3. Считаем сумму Плановая Собираемость по формуле "СуммаПлощади * СоцТарифПоТипу * 12"
        ///   4. Считаем средства СобственниковНаКР и ОстатокЗаПредыдущийГод
        /// </summary>
        public IDataResult CalcOwnerCollection(BaseParams baseParams)
        {
            var moId = baseParams.Params.GetAs<long>("mo_id");
            var isMass = baseParams.Params.GetAs<bool>("isMass");
            if (moId <= 0 && !isMass)
            {
                throw new ValidationException("Не удалось получить муниципальное образование");
            }

            var versionId = VersionDomain.GetAll().FirstOrDefault(x => x.IsMain && x.Municipality.Id == moId).Return(x => x.Id);

            // в этом массиве будут измененные строки если такие требуеются
            // То перед выполнением действий требуется их сохранить, а уже потом расчитывать
            var modifiedRecords = baseParams.Params.GetAs<SubsidyRecordVersion[]>("records");

            // 1. Сохраняем записи которые пришли измененные
            UpdateSubsidyRecords(modifiedRecords);

            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var periodStart = config.ProgrammPeriodStart;
            var periodEnd = config.ProgrammPeriodEnd;
            var periodShort = config.ShortTermProgPeriod;

            var gkhParams = Container.Resolve<IGkhParams>().GetParams();

            var moLevel = (MoLevel)gkhParams.GetAs<int>("MoLevel");

            var municipalities = Container.Resolve<IRepository<Municipality>>().GetAll()
                .WhereIf(!isMass, x => x.Id == moId)
                .WhereIf(isMass, x => VersionDomain.GetAll().Any(y => y.Municipality.Id == x.Id && y.IsMain))
                .AsEnumerable()
                .Where(x => x.Level.ToMoLevel(Container) == moLevel)
                .ToList();

            // убрано по задаче 30620,
            // обновляем данные колонок "Неснижаемый размер регионального фонда, %", "Плановая собираемость, %" из вкладки "Плановые показатели собираемости", если они не забиты
            // UpdateVersionSubsidyInfo(municipalities);
            foreach (var municipality in municipalities)
            {
                try
                {
                    GetSubsidyByMu(municipality.Id, periodStart, periodEnd);
                }
                catch
                {
                    // ignored
#warning тихий эксепшн, чтобы если какое то мо не расчиталось - продолжилось дальше
                }

                var dictOwnerCollection = GetOwnerCollection(municipality);

                var subsidyRecords =
                    SubsidyRecordVersionDomain.GetAll()
                        .Where(x => x.SubsidyYear <= periodEnd)
                        .Where(x => x.SubsidyYear >= periodStart)
                        .Where(x => x.Version.Id == versionId)
                        .OrderBy(x => x.SubsidyYear)
                        .ToList();

                /* Если год субсидирования является годом окончания краткосрочной программы,
                * возьмем значение средства собственников на кап ремонт в качестве значения для тех, 
                * кто не попадает в краткосрочку
                */
                decimal useMeAsOwnerSumForCr = 0;

                var shortRecords = subsidyRecords.Where(x => x.SubsidyYear < periodStart + periodShort).ToList();

                // вычисляем только для годов краткосрочки
                foreach (var rec in shortRecords)
                {
                    // Получаем запись предыдущего года
                    var beforeRec = subsidyRecords.FirstOrDefault(x => x.SubsidyYear == rec.SubsidyYear - 1);

                    // если период попадает в период краткосрочной программы
                    // то считаем в соответствии с тарифами на год
                    if (rec.SubsidyYear < periodStart + periodShort && dictOwnerCollection.ContainsKey(rec.SubsidyYear))
                    {
                        rec.PlanOwnerCollection = dictOwnerCollection[rec.SubsidyYear].ToDecimal();
                    }
                    else
                    {
                        // Плановая собираемость текущего года
                        // Считаем сумму Плановая Собираемость по формуле "СуммаЖилойПлощади * СоцТарифПоТипу * 12"
                        rec.PlanOwnerCollection =
                            dictOwnerCollection.Any()
                                ? dictOwnerCollection.FirstOrDefault().Value.ToDecimal()
                                : 0m;
                    }

                    // Поступившие средства текущего года (считается как ПлановаяСобираемость*ПроцентРискаСбора)
                    rec.Available = (rec.PlanOwnerCollection * rec.PlanOwnerPercent).ToDivisional();

                    if (beforeRec != null)
                    {
                        // если есть предыдущий год то берем его резерв и прибавляем к сумме на КР этого года
                        rec.Available += beforeRec.Reserve;
                    }

                    // Если нет предыдущего года то мы находимся в 1 году соответсвенно мы поулчаем просто Плановую собираемость
                    // Поскольку остатка нет то Плановую собираемость текущего года с учетом НеСнижаемогоРазмераРегФонда
                    rec.OwnerSumForCr = rec.Available * (1 - rec.NotReduceSizePercent.ToDivisional());

                    // Оставшуюся часть фиксируем как остаток первого года
                    rec.Reserve = rec.Available - rec.OwnerSumForCr;

                    // Записываем последнюю запись
                    useMeAsOwnerSumForCr = rec.OwnerSumForCr;
                }

                subsidyRecords
                    .Where(x => x.SubsidyYear >= periodStart + periodShort)
                    .ForEach(x => x.OwnerSumForCr = useMeAsOwnerSumForCr);

                using (var tx = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        // Сохраняем субсидирвоание (Если еще небыло сохранено)
                        foreach (var rec in subsidyRecords)
                        {
                            // Фиксируем дату расета 
                            rec.DateCalcOwnerCollection = DateTime.Now;

                            SubsidyRecordVersionDomain.Update(rec);
                        }

                        tx.Commit();
                    }
                    catch (ValidationException e)
                    {
                        tx.Rollback();
                        return new BaseDataResult { Success = false, Message = e.Message };
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// В этом методе мы расчитываем показатели
        /// То есть вычисляем Бюджеты и относителньо бюджетов выстраиваем Скорректированную ОЧередь 
        /// Формируя тем самым Долгосрочную программу - DpkrCorrectionStage2
        /// А также после получения скорректированной очереди мы получаем из Долгосрочной программы записи Краткосрочной Программы
        /// И для Них ВЫчисляем Дифициты (ShortProgramDifitsit) а также сохраняем сами записи краткосрочной программы (ShortProgramRecord)
        /// </summary>
        public IDataResult CalcValues(BaseParams baseParams)
        {
            var moId = baseParams.Params.GetAs<long>("mo_id");
            var isMass = baseParams.Params.GetAs<bool>("isMass");
            if (moId <= 0 && !isMass)
            {
                throw new ValidationException(
                    string.Format("Невозможно определить муниципальное образование по id: {0}!", moId));
            }

            // в этом массиве будут измененные строки если такие требуеются
            // То перед выполнением действий требуется их сохранить, а уже потом расчитывать
            var modifiedRecords = baseParams.Params.GetAs<SubsidyRecordVersion[]>("records");

            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var periodStartYear = config.ProgrammPeriodStart;
            var periodEndYear = config.ProgrammPeriodEnd;
            var groupByRoPeriod = config.GroupByRoPeriod;
            var useRealObjCollection = config.SubsidyConfig.UseRealObjCollection;
            var useFixationPublishedYears = config.SubsidyConfig.UseFixationPublishedYears;

            if (groupByRoPeriod == 0 && ShareFinCeoDomain.GetAll().Any()
                && ShareFinCeoDomain.GetAll().Sum(x => x.Share) < 100)
            {
                throw new ValidationException("Доли финансирования должны составлять 100%");
            }

            var session = Container.Resolve<ISessionProvider>().OpenStatelessSession();

            try
            {
                // Получаем основные версии либо по 1 МО либо по всем МО
                var versions = VersionDomain.GetAll()
                    .Where(x => x.IsMain)
                    .WhereIf(moId != 0, x => x.Municipality.Id == moId)
                    .GroupBy(x => x.Municipality.Id)
                    .ToDictionary(x => x.Key, y => y.First());

                if (!versions.Any())
                {
                    throw new ValidationException(
                        "Не найдена основная версия для выбранного муниципального образования!");
                }

                // Получаем записи субсидирования
                var subsidyRecordsDict = SubsidyRecordVersionDomain.GetAll()
                        .WhereIf(!isMass, x => x.Version.Municipality.Id == moId)
                        .Where(x => x.Version.IsMain)
                        .AsEnumerable()
                        .GroupBy(x => x.Version.Id)
                        .ToDictionary(x => x.Key, y => y.OrderBy(x => x.SubsidyYear).ToList());

                if (modifiedRecords.Any())
                {
                    var subsidyRecs = subsidyRecordsDict.First().Return(x => x.Value);
                    foreach (var record in modifiedRecords)
                    {
                        var rec = subsidyRecs.First(x => x.SubsidyYear == record.SubsidyYear);

                        rec.SubsidyYear = record.SubsidyYear;
                        rec.BudgetRegion = record.BudgetRegion;
                        rec.BudgetMunicipality = record.BudgetMunicipality;
                        rec.BudgetFcr = record.BudgetFcr;
                        rec.BudgetOtherSource = record.BudgetOtherSource;
                        rec.PlanOwnerCollection = record.PlanOwnerCollection;
                        rec.PlanOwnerPercent = record.PlanOwnerPercent;
                        rec.NotReduceSizePercent = record.NotReduceSizePercent;
                        rec.OwnerSumForCr = record.OwnerSumForCr;
                        rec.DateCalcOwnerCollection = record.DateCalcOwnerCollection;
                    }
                }

                // удаляем данные перед началом корректировки
                DeleteBeforeCorrection(session, isMass, moId);

                var rowsList = this.GetQueryableDpkCorrectiomRows(moId, periodEndYear).AsEnumerable();

                // Получаем записи корректировки отсортированные по Индексу
                var dictCorrections = rowsList.GroupBy(x => x.VersionId)
                                        .ToDictionary(x => x.Key, y => y.OrderBy(x => x.Index).ToList());

                // получаем Года опубликованной программы по записям VersionStage.Id
                var dictPublishedRecords = new Dictionary<long, Dictionary<long, int>>();

                if (useFixationPublishedYears == TypeUsage.Used)
                {
                    dictPublishedRecords = PublishedRecordDomain.GetAll()
                        .Where(x => x.Stage2 != null)
                        .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                        .WhereIf(
                            moId > 0,
                            x => x.PublishedProgram.ProgramVersion.Municipality.Id == moId)
                        .Select(
                            x =>
                                new
                                {
                                    VersionId = x.PublishedProgram.ProgramVersion.Id,
                                    St2Id = x.Stage2.Id,
                                    x.PublishedYear
                                })
                        .AsEnumerable()
                        .GroupBy(x => x.VersionId)
                        .ToDictionary(
                            x => x.Key,
                            y => y.GroupBy(z => z.St2Id).ToDictionary(z => z.Key, x => x.Select(z => z.PublishedYear).FirstOrDefault()));
                }


                // Получаем записи корректировки отсортированные по году и весу
                var dictForOwnerCollections = rowsList
                                    .GroupBy(x => x.VersionId)
                                    .ToDictionary(
                                        x => x.Key,
                                        y => y.GroupBy(z => z.RealityObjectId)
                                            .ToDictionary(
                                                x => x.Key,
                                                z => z.OrderBy(x => x.PlanYear)
                                                    .ThenByDescending(x => x.CeoWeight)
                                                    .ToList()));

                if (dictCorrections.Count == 0)
                {
                    return new BaseDataResult(
                        false,
                        "Расчет не был произведен, возможные причины:"
                        + "<br>Не произведен расчет этапов долгосрочной программы,"
                        + "<br>Неверный год окончания периода долгосрочной программы.");
                }

                var correctionToSave = new List<DpkrCorrectionStage2>();
                var shortRecordToSave = new List<ShortProgramRecord>();
                var subsidyRecordToSave = new List<SubsidyRecordVersion>();

                foreach (var version in versions)
                {
                    if (!dictCorrections.ContainsKey(version.Value.Id) || !subsidyRecordsDict.ContainsKey(version.Value.Id))
                    {
                        continue;
                    }

                    // получаем первоначальные данные по которымдальше будет проходить корректировка
                    var correction = dictCorrections[version.Value.Id];
                    var subsidyRecords = subsidyRecordsDict[version.Value.Id];
                    var publishedYears = dictPublishedRecords.ContainsKey(version.Value.Id) ? dictPublishedRecords[version.Value.Id] : new Dictionary<long, int>();

                    var versionCorrectionToSave = new List<DpkrCorrectionStage2>();
                    var versionShortRecordToSave = new List<ShortProgramRecord>();
                    var versionSubsidyRecordToSave = new List<SubsidyRecordVersion>();

                    // Необходимо исключит ьконструктивные элементы которые не попадают в Собираемость по Дому
                    // в случае если
                    // 1. Используется Учет собираемсоти
                    // 2. ПериодГруппировкиПоДомам = 0
                    // 3. И отсутсвуют доли финансирования в справочнике
                    if (useRealObjCollection == TypeUsage.Used && groupByRoPeriod == 0 && !ShareFinCeoDomain.GetAll().Any())
                    {
                        var listExcludeId = new List<long>(); // В этом списке будут Id котоыре над оисключить

                        // 1. Получаем собираемость за весь период по домам 
                        var roQuery = this.GetRoQueryForSubsidy(version.Value.Municipality.Id);
                        var dictCollections = RealEstateService.GetCollectionByPeriod(roQuery, periodStartYear, periodEndYear);

                        // 2. Формируем список работ 2 этапа отсортированные по Году и ВесуООИ
                        var dictCeoWeight = dictForOwnerCollections[version.Value.Id];
                        foreach (var kvp in dictCeoWeight)
                        {
                            var roId = kvp.Key;

                            // получаем сумму собираемости по дому
                            var sumCollection = dictCollections.ContainsKey(roId) ? dictCollections[roId] : 0M;

                            // Проходим по работам отсортированным по Году и Весу
                            // и пытаемся сразуже определить работы, котоыре не войдут в ДПКР по собираемсоти
                            foreach (var row in kvp.Value)
                            {
                                if (sumCollection >= row.Sum)
                                {
                                    // Если умещается в собираемость, то списываем сумму (Как будто эта работа останется)
                                    sumCollection -= row.Sum;
                                }
                                else
                                {
                                    // Иначе помечаем работу как Исключаемую
                                    listExcludeId.Add(row.Stage2Id);
                                }
                            }
                        }

                        // 3 ТЕперь пытаемся исключить те записи которые предполагается удалить
                        if (listExcludeId.Any())
                        {
                            var dictTemp = correction.GroupBy(x => x.Stage2Id).ToDictionary(x => x.Key, y => y.FirstOrDefault());

                            foreach (var id in listExcludeId)
                            {
                                if (dictTemp.ContainsKey(id))
                                {
                                    // удаляем его из списка
                                    correction.Remove(dictTemp[id]);
                                }
                            }
                        }
                    }

                    // Запускаем метод, который разрулит Какие бюджеты будут в субсидировании, Какие ООИ должны остатся
                    // в своем году а какие должны перейти на следующий год
                    CorrectionDpkr(correction, subsidyRecords, publishedYears);

                    // после корректировки какието записи могли быть вытеснены за пределы программы 
                    // поскольку денег ни в каком году на них не хватило
                    // их просто не берем дальше, берем только те которые попали в период ДПКР
                    var finalCorrections = correction.Where(x => x.PlanYear >= periodStartYear
                                                                && x.PlanYear <= periodEndYear
                                                                && x.IsCalculated);

                    foreach (var corr in finalCorrections)
                    {
                        var newItem =
                            new DpkrCorrectionStage2
                            {
                                RealityObject = new RealityObject { Id = corr.RealityObjectId },
                                PlanYear = corr.PlanYear,
                                Stage2 = new VersionRecordStage2
                                {
                                    Id = corr.Stage2Id,
                                    Sum = corr.Sum
                                }
                            };

                        versionCorrectionToSave.Add(newItem);
                    }

                    // Получаем записи краткосрочной программы с уже подсчитанными значениями
                    versionShortRecordToSave.AddRange(ShortProgramCalculation(versionCorrectionToSave, subsidyRecords));

                    // Поскольку в ходе вычислений БюджетаНаКр (BudgetCr) и ФактическойСуммыНаКР (CorrectionFinance)
                    // мы фиксировали значения не для Версии а в ТемповыеНеХранимыеПоля Субсидии
                    // То теперь необходимо их Сохранить в ту версию для которой производилось вычисление
                    foreach (var subsidy in subsidyRecords)
                    {
                        versionSubsidyRecordToSave.Add(subsidy);
                    }

                    // переносим в общий список
                    correctionToSave.AddRange(versionCorrectionToSave);
                    shortRecordToSave.AddRange(versionShortRecordToSave);
                    subsidyRecordToSave.AddRange(versionSubsidyRecordToSave);
                }

                using (var tr = session.BeginTransaction())
                {
                    try
                    {
                        subsidyRecordToSave.ForEach(x =>
                        {
                            if (x.Id > 0)
                            {
                                session.Update(x);
                            }
                            else
                            {
                                session.Insert(x);
                            }
                        });

                        // Сначала сохраняем Корректировки 
                        correctionToSave.ForEach(x => session.Insert(x));

                        // Теперь сохраняем записи Краткосрочной программы 
                        shortRecordToSave.ForEach(x => session.Insert(x));

                        tr.Commit();
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
            finally
            {
                Container.Resolve<ISessionProvider>().CloseCurrentSession();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Метод очистки перед проведением корректировки
        /// </summary>
        private static void DeleteBeforeCorrection(IStatelessSession session, bool isMass, long moId = 0)
        {
            // удаляем существующие корректировки
            // посколку грохаются корректироваки,то необходимо вместе сними 
            // грохнуть и Дифициты по МО и Краткосрочную программу
            using (var tr = session.BeginTransaction())
            {
                try
                {
                    if (isMass)
                    {
                        session.CreateSQLQuery(@"delete from ovrhl_short_prog_rec where stage2_id in ( 
                                                select t.id from ovrhl_stage2_version t 
                                                join ovrhl_version_rec st3 on st3.id = t.st3_version_id 
                                                 join ovrhl_prg_version v on v.id = st3.version_id and v.is_main").ExecuteUpdate();

                        session.CreateSQLQuery(@"delete from ovhl_dpkr_correct_st2 where st2_version_id in (
                                                select st2.id from ovrhl_stage2_version st2
                                                join ovrhl_version_rec st3 on st3.id = st2.st3_version_id
                                                join ovrhl_prg_version v on v.id = st3.version_id and v.is_main").ExecuteUpdate();
                    }
                    else
                    {
                        // удаляем записи краткосрочной программы
                        session.CreateSQLQuery(string.Format(@"delete from ovrhl_short_prog_rec where stage2_id in ( 
                                                select t.id from ovrhl_stage2_version t 
                                                join ovrhl_version_rec st3 on st3.id = t.st3_version_id 
                                                 join ovrhl_prg_version v on v.id = st3.version_id and v.is_main and v.mu_id = {0})", moId)).ExecuteUpdate();

                        // удаляем Корректировки по версии
                        session.CreateSQLQuery(string.Format(@"delete from ovhl_dpkr_correct_st2 where st2_version_id in (
                                                select st2.id from ovrhl_stage2_version st2
                                                join ovrhl_version_rec st3 on st3.id = st2.st3_version_id
                                                join ovrhl_prg_version v on v.id = st3.version_id and v.is_main and v.mu_id = {0})", moId)).ExecuteUpdate();
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

        /// <summary>
        /// В этом методе по сформированной долгосрочной программе будем вычислять Краткосрочную программу
        /// И подсчитывать дифициты
        /// </summary>
        /// <param name="correction">Данные, полученные после вычисления корректировок</param>
        /// <param name="subsidyRecords">Записи субсидирования, с фильтром по МО</param>
        private IEnumerable<ShortProgramRecord> ShortProgramCalculation(IEnumerable<DpkrCorrectionStage2> correction, IEnumerable<SubsidyRecordVersion> subsidyRecords)
        {
            var result = new List<ShortProgramRecord>();

            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var periodStart = config.ProgrammPeriodStart;
            var shortTermPeriod = config.ShortTermProgPeriod;

            var shortTerm = periodStart + shortTermPeriod;

            var data = correction.Where(x => x.PlanYear < shortTerm);

            var dictSubsidy = subsidyRecords.GroupBy(x => x.SubsidyYear).ToDictionary(x => x.Key, y => y.First());

            var dictSum = data.GroupBy(x => x.PlanYear).ToDictionary(x => x.Key, y => y.Select(x => x.Stage2.Sum).Sum());

            // для каждой записи отсеченной по году создаем запись краткосрочной программы
            foreach (var rec in data)
            {
                SubsidyRecordVersion subsidy;

                dictSubsidy.TryGetValue(rec.PlanYear, out subsidy);

                if (subsidy == null)
                    continue;

                // Общая сумма на КР в году
                var sumInYear = dictSum[rec.PlanYear];
                var sumRecord = rec.Stage2.Sum;

                // Тут будут подсчитыватся показатели
                var shortR = new ShortProgramRecord { Stage2 = rec.Stage2, Year = rec.PlanYear, RealityObject = rec.RealityObject };

                if (sumInYear > 0)
                {
                    // Сначала подсчитываем сумму на КР из Средств собственников
                    shortR.OwnerSumForCr = ((sumRecord / sumInYear) * subsidy.OwnerSumForCr).RoundDecimal(2);

                    // проверяем привысилал ли Сумма Стоимсть работ
                    if (shortR.OwnerSumForCr > sumRecord)
                    {
                        // Если сумма получилась больше чем требовалось то ставим Стоимость по ООИ
                        shortR.OwnerSumForCr = sumRecord;
                    }

                    // Теперь подсчитываем сумму на КР из ГК ФСР
                    shortR.BudgetFcr = (((sumRecord - shortR.OwnerSumForCr) / (sumInYear - subsidy.OwnerSumForCr)) * subsidy.BudgetFcr)
                            .RoundDecimal(2);

                    // Проверяем превысила ли сумма стоисоть работ
                    if (shortR.BudgetFcr + shortR.OwnerSumForCr > sumRecord)
                    {
                        // Если сумма получилась больше чем требовалось то ставим Стоимость по ООИ
                        shortR.BudgetFcr = sumRecord - shortR.OwnerSumForCr;
                    }

                    // Теперь подсчитываем сумму на КР из Иных источников
                    shortR.BudgetOtherSource = (((sumRecord - shortR.OwnerSumForCr - shortR.BudgetFcr)
                          / (sumInYear - subsidy.OwnerSumForCr - subsidy.BudgetFcr)) * subsidy.BudgetOtherSource).RoundDecimal(2);

                    // Проверяем превысила ли сумма стоисоть работ
                    if (shortR.BudgetOtherSource + shortR.BudgetFcr + shortR.OwnerSumForCr > sumRecord)
                    {
                        // Если сумма получилась больше чем требовалось то ставим Стоимость по ООИ
                        shortR.BudgetOtherSource = sumRecord - shortR.OwnerSumForCr - shortR.BudgetFcr;
                    }

                    // После подсчета показателей мы поулчаем Дефицит по Данному ООИ
                    shortR.Difitsit = sumRecord - shortR.OwnerSumForCr - shortR.BudgetFcr
                                      - shortR.BudgetOtherSource;
                }

                result.Add(shortR);
            }

            return result;
        }

        /// <summary>
        /// Этот метод отвечает за поулчение бюджетов и корректировку дпкр и получение фактических сумм для каждого года Субсидирования
        /// </summary>
        /// <param name="correction">Список прокси объектов с данными предпоследнего этапа, отфильтрованные по МО</param>
        /// <param name="subsidyRecords">Записи субсидирования, отфильтрованные по МО</param>
        private void CorrectionDpkr(List<DpkrCorrectionProxy> correction, List<SubsidyRecordVersion> subsidyRecords, Dictionary<long, int> publishedYears)
        {
            // Окончание периода
            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var periodStart = config.ProgrammPeriodStart;
            var shortTerm = config.ShortTermProgPeriod;
            var periodEnd = config.ProgrammPeriodEnd;
            var typeCorrection = config.SubsidyConfig.TypeCorrection;
            var correctionDict = new Dictionary<int, decimal>();

            // Если способ корректировки = "C фиксацией года", то проставляем признак IsCalculated, чтобы данные записи не сдвигались от планового года
            if (typeCorrection == TypeCorrection.WithFixYear)
            {
                correctionDict = correction.Where(x => x.IsManuallyCorrect).GroupBy(x => x.PlanYear).ToDictionary(x => x.Key, x => x.SafeSum(y => y.Sum));
                correction.Where(x => x.IsManuallyCorrect).ForEach(x => x.IsCalculated = true);
            }

            // Теперь проходим по субсидированию и начинаем распиливать деньги на работы
            foreach (var item in subsidyRecords)
            {
                item.BudgetCr = item.OwnerSumForCr;

                // Бюджеты учитываем только для годов Краткосрочки, для последующих годов учитываем тольк особираемость собственников на КР
                if (item.SubsidyYear < periodStart + shortTerm)
                {
                    item.BudgetCr += item.BudgetFcr + item.BudgetMunicipality + item.BudgetOtherSource + item.BudgetRegion;
                }

                // стоимость работ по записям с фиксированным годом
                var fixedYearCorrectionFinance = correctionDict.Get(item.SubsidyYear);

                // Обнуляем Фактическую стоимость
                item.CorrectionFinance = fixedYearCorrectionFinance;

                var lastYear = subsidyRecords.FirstOrDefault(x => x.SubsidyYear == item.SubsidyYear - 1);

                // переносим остатки предыдущего года на тот год который рассматривается 
                if (lastYear != null && lastYear.BalanceAfterCr > 0)
                {
                    item.BudgetCr = item.BudgetCr + lastYear.BalanceAfterCr;
                }

                // Теперь считаем корректировку. То есть пытаемся определить работы которые останутся 
                // в этом году, а какие уйдут в другой год
                WalkThroughDpkr(correction, item, publishedYears);

                if (item.SubsidyYear == periodEnd)
                {
                    // Если год субсидирования был последний, то тогда необходимо проверить чтобы по дому хотябы одна работа была в дпкр
                    // берем все работы вытолкнутые из дпкр и проверяем есть ли дом в периоде ДПКР если нет то работу 1 по дому 
                    // возвращаем в последний год программы
                    var outRecords =
                        correction.Where(x => x.PlanYear > periodEnd)
                            .Where(x => !correction.Any(y => y.PlanYear <= periodEnd && x.RealityObjectId == y.RealityObjectId))
                            .GroupBy(x => x.RealityObjectId)
                            .ToDictionary(x => x.Key, y => y.OrderBy(z => z.Index).FirstOrDefault())
                            .Values.ToList();

                    if (outRecords.Any())
                    {
                        foreach (var rec in outRecords)
                        {
                            rec.PlanYear = periodEnd;
                            rec.IsCalculated = true;
                            item.CorrectionFinance += rec.Sum;
                        }
                    }
                }

                // После подсчета корректировку получаем Остаток на конец расчетного года
                item.BalanceAfterCr = item.BudgetCr - item.CorrectionFinance;

                // [#42695] «в том случае, если год записи был зафиксирован, но в рамках 
                // этого года на нее не хватает средств, значение поля "Остаток средств
                // после проведения капитального ремонта" должно быть = 0»
                // если фиксированные записи вызвали дефицит, то просто считаем, что дефицита нет
                // планирование бюджета 80 lvl
                if (fixedYearCorrectionFinance > 0M && item.BalanceAfterCr < 0M)
                {
                    item.BalanceAfterCr = Math.Min(0M, item.BalanceAfterCr + fixedYearCorrectionFinance);
                }
            }
        }

        private IEnumerable<DpkrCorrectionProxy> GetQueryableDpkCorrectiomRows(long municipalityId, int periodEndYear)
        {
            // Получаем записи версии 2 этапа
            // уже отсортированные по Индексу
            return VersionStage2Domain.GetAll()
                                   .Where(x => x.Stage3Version.ProgramVersion.IsMain)
                                   .Where(x => x.Stage3Version.Year <= periodEndYear)
                                   .WhereIf(
                                       municipalityId > 0,
                                       x => x.Stage3Version.ProgramVersion.Municipality.Id == municipalityId)
                                   .Select(
                                       x =>
                                       new DpkrCorrectionProxy
                                       {
                                           Stage2Id = x.Id,
                                           MunicipalityId = x.Stage3Version.RealityObject.Municipality.Id,
                                           RealityObjectId = x.Stage3Version.RealityObject.Id,
                                           PlanYear = x.Stage3Version.Year,
                                           Index = x.Stage3Version.IndexNumber,
                                           Sum = x.Sum,
                                           SumStage3 = x.Stage3Version.Sum,
                                           Stage3Id = x.Stage3Version.Id,
                                           CeoId = x.CommonEstateObject.Id,
                                           CeoWeight = x.CommonEstateObject.Weight,
                                           VersionId = x.Stage3Version.ProgramVersion.Id,
                                           IsChangedYear = x.Stage3Version.IsChangedYear,
                                           IsManuallyCorrect = x.Stage3Version.IsManuallyCorrect
                                       });
        }


        /// <summary>
        /// Метод корректировки если в 3м этапе было схлопывание по дома и ООИ поместилисмь через запятую в одну запись по дому
        /// Крыша, Фасад, Подъезд
        /// </summary>
        private void WalkThroughDpkr(IEnumerable<DpkrCorrectionProxy> latterCorrection, SubsidyRecordVersion subsidy, Dictionary<long, int> publishedYears)
        {
            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var groupByRoPeriod = config.GroupByRoPeriod;
            var periodEnd = config.ProgrammPeriodEnd;

            if (groupByRoPeriod > 0)
            {
                // Запускаем корректировку для ДПКР расчитанных с параметром группировки > 0
                // читать описание метода
                MethodCorrectionWithGrouppingPeriod(latterCorrection, subsidy, periodEnd, publishedYears);
            }
            else if (groupByRoPeriod == 0 && ShareFinCeoDomain.GetAll().Any())
            {
                // Если группировка по Периоду = 0 и забиты Доли то запускаем метод отрезания очереди по долям
                var shareFinancing =
                    ShareFinCeoDomain.GetAll()
                        .OrderBy(x => x.CommonEstateObject.Weight)
                        .AsEnumerable()
                        .Select(x => new ShareFinancingCeoProxy
                        {
                            CeoId = x.CommonEstateObject.Id,
                            Share = x.Share,
                            Sum = (subsidy.BudgetCr * (x.Share / 100)).RoundDecimal(2)
                        })
                        .ToList();

                // Запускаем корректировку для тех у кого ДПКР расчитан по периоду группировки = 0 и есть доли финансирования
                // читать описание метода
                MethodCorrectionWithShareFinansing(latterCorrection, subsidy, shareFinancing, shareFinancing[0], publishedYears);
            }
            else
            {
                // Запускаем стандартную корректировку
                // читать описание метода
                MethodCorrectionStandart(latterCorrection, subsidy, publishedYears);
            }
        }

        private static bool MoveToNextYear(SubsidyRecordVersion subsidy, DpkrCorrectionProxy correctRecord, Dictionary<long, int> publishedYears)
        {
            // Если запись находится в опубликованной программе то необходимо проверить чтобы 
            // Не привысила год
            var fixedYear = 9999;

            var result = true;

            if (publishedYears.ContainsKey(correctRecord.Stage2Id))
            {
                fixedYear = publishedYears[correctRecord.Stage2Id];
            }

            if (correctRecord.PlanYear >= fixedYear)
            {
                // Если превышает фикстированный год то необходимо работу оставить в этом году
                subsidy.CorrectionFinance += correctRecord.Sum;
                correctRecord.IsCalculated = true;
                result = false;
            }
            else
            {
                correctRecord.PlanYear++;
            }

            return result;
        }

        /// <summary>
        /// Метод обычной корректирвоки, когда просто проверяется может ли ООИ быть выполнен в 
        /// тоесть например такие работы
        /// 1 работа = 10 тыщ
        /// 2 работа = 1 млн
        /// 3 работа = 5 тыщ
        /// 4 работа = 6 тыщ
        /// И например остаток в бюджете 900 тыщ, то работы 2,3,4 переходят на след год
        /// и работа 3 и 4 будет рассмотрена толко после того когда работа 2 найдет свое место
        /// </summary>
        private static void MethodCorrectionStandart(IEnumerable<DpkrCorrectionProxy> latterCorrection,
                                                SubsidyRecordVersion subsidy,
                                                Dictionary<long, int> publishedYears)
        {
            var data = latterCorrection.Where(x => x.PlanYear == subsidy.SubsidyYear && !x.IsCalculated);

            // Формируем словарь по Дом + ООИ чтобы потом сдвигать именно так 
            var dictNextCeo =
                latterCorrection.Where(x => x.PlanYear > subsidy.SubsidyYear)
                                .GroupBy(x => string.Format("{0}_{1}", x.RealityObjectId, x.CeoId))
                                .ToDictionary(x => x.Key, y => y.Select(z => z.Stage2Id).Distinct().ToList());
            var dictNextRecord =
                latterCorrection.Where(x => x.PlanYear > subsidy.SubsidyYear)
                                .GroupBy(x => x.Stage2Id)
                                .ToDictionary(x => x.Key, y => y.FirstOrDefault());

            bool stop = false;

            // список Id которые после будут сдвинуты на +1 год (типа сдвиг срока эксплуатации)
            var listMovedCeo = new List<long>();

            foreach (var correctRecord in data)
            {
                if (correctRecord.IsCalculated)
                    continue;

                if (!stop)
                {
                    if (subsidy.CorrectionFinance + correctRecord.Sum <= subsidy.BudgetCr)
                    {
                        subsidy.CorrectionFinance += correctRecord.Sum;
                        correctRecord.IsCalculated = true;
                    }
                    else
                    {
                        stop = true;
                    }
                }

                if (stop)
                {
                    if (MoveToNextYear(subsidy, correctRecord, publishedYears))
                    {
                        var key = string.Format("{0}_{1}", correctRecord.RealityObjectId, correctRecord.CeoId);

                        // поскольку надо подвинуть элемент, то надо по данному ООИ подвинуть все нижестоящие записи
                        if (dictNextCeo.ContainsKey(key))
                        {
                            listMovedCeo.AddRange(dictNextCeo[key]);
                        }
                    }
                }
            }

            if (listMovedCeo.Any())
            {
                foreach (var id in listMovedCeo.Distinct().ToList())
                {
                    if (dictNextRecord.ContainsKey(id))
                    {
                        dictNextRecord[id].PlanYear++;
                    }
                }
            }
        }

        /// <summary>
        /// Метод отрезания очереди для тех у кого расчет производился с параметром группировки по дому, 
        /// он такой же как MethodCorrectionStandart, но переносятся не одна работа вся группа работ которая попала в Stage3
        /// </summary>
        private static void MethodCorrectionWithGrouppingPeriod(IEnumerable<DpkrCorrectionProxy> latterCorrection,
                                                        SubsidyRecordVersion subsidy,
                                                        int periodEnd,
                                                        Dictionary<long, int> publishedYears)
        {
            // Каждый раз получаем записи меньше либо равные году субсидирования 
            var data = latterCorrection.Where(x => x.PlanYear == subsidy.SubsidyYear && !x.IsCalculated);

            var dataStage3 = data.GroupBy(x => x.Stage3Id).ToDictionary(x => x.Key, y => y.ToList());

            // Далее мы бежим по отсортированному списку и пытаемся ООИ впихнуть в Субсидирование
            // Если бюджет уже превышает, то перекидываем на следующий год. Учитываем что Сумма будет накопительной
            var stop = false;

            foreach (var correctRecord in data)
            {
                if (correctRecord.IsCalculated)
                    continue;

                // Проверяем, если мы достигли предельного года в ДПКР 
                // то фиксируем ООИ как рассмотренную и накапливаем сумму на последнем годе
                if (subsidy.SubsidyYear >= periodEnd)
                {
                    correctRecord.PlanYear = subsidy.SubsidyYear;
                    correctRecord.IsCalculated = true;
                    subsidy.CorrectionFinance += correctRecord.Sum;
                    continue;
                }

                if (!stop)
                {
                    if (subsidy.CorrectionFinance + correctRecord.SumStage3 <= subsidy.BudgetCr)
                    {
                        subsidy.CorrectionFinance += correctRecord.SumStage3;

                        // Если денег хватает то ставим всем записям ставим что они расчитаны
                        if (dataStage3.ContainsKey(correctRecord.Stage3Id))
                        {
                            foreach (var item in dataStage3[correctRecord.Stage3Id])
                            {
                                item.IsCalculated = true;
                            }
                        }
                    }
                    else
                    {
                        // и отрезаем очередь
                        stop = true;
                    }
                }

                if (stop)
                {
                    // Если не хватает денег, то тогда переставляем на след год
                    if (dataStage3.ContainsKey(correctRecord.Stage3Id))
                    {
                        foreach (var item in dataStage3[correctRecord.Stage3Id])
                        {
                            MoveToNextYear(subsidy, item, publishedYears);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Метод отрезания очереди с учетом долей финансирвоания работ. В этом методе:
        ///     1. сначала выстраивается очередь приоритетов ООИ,
        ///     2. затем определяется доля каждого ООИ в общем бюджете на КР
        ///     3. затем уже определяется вмещается ли нужный ООИ в бюджет, есди нет то переносится на след год
        /// </summary>
        private static void MethodCorrectionWithShareFinansing(IEnumerable<DpkrCorrectionProxy> latterCorrection,
                                                        SubsidyRecordVersion subsidy,
                                                        IList<ShareFinancingCeoProxy> shareFinancing,
                                                        ShareFinancingCeoProxy share,
                                                        Dictionary<long, int> publishedYears)
        {
            // Получаем данные по текщему ООИ 
            // Каждый раз получаем записи меньше либо равные году субсидирования 
            var data = latterCorrection.Where(x => x.PlanYear == subsidy.SubsidyYear && !x.IsCalculated && x.CeoId == share.CeoId);

            // Формируем словарь по Дом + ООИ чтобы потом сдвигать именно так 
            var dictNextCeo =
                latterCorrection.Where(x => x.PlanYear > subsidy.SubsidyYear && x.CeoId == share.CeoId)
                                .GroupBy(x => string.Format("{0}_{1}", x.RealityObjectId, x.CeoId))
                                .ToDictionary(x => x.Key, y => y.Select(z => z.Stage2Id).Distinct().ToList());

            var dictNextRecord =
                latterCorrection.Where(x => x.PlanYear > subsidy.SubsidyYear && x.CeoId == share.CeoId)
                                .GroupBy(x => x.Stage2Id)
                                .ToDictionary(x => x.Key, y => y.FirstOrDefault());

            // список Id которые после будут сдвинуты на +1 год (типа сдвиг срока эксплуатации)
            var listMovedCeo = new List<long>();

            foreach (var correctRecord in data)
            {
                if (correctRecord.Sum <= share.Sum)
                {
                    // Если работа подходит под имеющуюся сумму, то тогда фиксируем ее 
                    subsidy.CorrectionFinance += correctRecord.Sum;

                    // и уменьшаем оставшуюся долю
                    share.Sum = share.Sum - correctRecord.Sum;

                    correctRecord.PlanYear = subsidy.SubsidyYear;

                    correctRecord.IsCalculated = true;
                }
                else
                {
                    // Иначе перекидываем данную работу на след год
                    if (MoveToNextYear(subsidy, correctRecord, publishedYears))
                    {
                        var key = string.Format("{0}_{1}", correctRecord.RealityObjectId, correctRecord.CeoId);

                        // поскольку надо подвинуть элемент, то надо по данному ООИ подвинуть все нижестоящие записи
                        if (dictNextCeo.ContainsKey(key))
                        {
                            listMovedCeo.AddRange(dictNextCeo[key]);
                        }
                    }
                }
            }

            if (listMovedCeo.Any())
            {
                foreach (var id in listMovedCeo.Distinct().ToList())
                {
                    if (dictNextRecord.ContainsKey(id))
                    {
                        dictNextRecord[id].PlanYear++;
                    }
                }
            }

            // Получаем остаток на данном ООИ 
            var delta = share.Sum;

            // Обнуляем его деньги после прохода 
            share.Sum = 0;

            // Поулчаем текущий индекс в массиве
            var index = shareFinancing.IndexOf(share);

            // Если есть в массиве следующие ООИ то передаем расчет им
            if (index + 1 < shareFinancing.Count())
            {
                var newShare = shareFinancing[index + 1];

                // Предполагаем что у нового ООИ будет теперь сумма с учетом остатка
                newShare.Sum = newShare.Sum + delta;

                // И запускаем рекурсию на выполнение
                MethodCorrectionWithShareFinansing(latterCorrection, subsidy, shareFinancing, newShare, publishedYears);
            }
        }

        /// <summary>
        /// В каждом из расчетов требуется сначала сохранить измененные записи Грида Субсидирования, а затем произвести какоето вычисление
        /// По этому этот метод сохраняет записи Субсидирования, которые были изменены
        /// </summary>
        private void UpdateSubsidyRecords(SubsidyRecordVersion[] data)
        {
            if (data != null && data.Any())
            {
                using (var tx = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        // Сохраняем все записи котоыре были изменены 
                        foreach (var record in data)
                        {
                            var rec = SubsidyRecordVersionDomain.Load(record.Id);

                            rec.SubsidyYear = record.SubsidyYear;
                            rec.BudgetRegion = record.BudgetRegion;
                            rec.BudgetMunicipality = record.BudgetMunicipality;
                            rec.BudgetFcr = record.BudgetFcr;
                            rec.BudgetOtherSource = record.BudgetOtherSource;
                            rec.PlanOwnerCollection = record.PlanOwnerCollection;
                            rec.PlanOwnerPercent = record.PlanOwnerPercent;
                            rec.NotReduceSizePercent = record.NotReduceSizePercent;
                            rec.OwnerSumForCr = record.OwnerSumForCr;
                            rec.DateCalcOwnerCollection = record.DateCalcOwnerCollection;

                            SubsidyRecordVersionDomain.Update(rec);
                        }

                        tx.Commit();
                    }
                    catch (ValidationException)
                    {
                        tx.Rollback();
                    }
                    catch
                    {
                        tx.Rollback();
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

            // собираем сборку отчета и формируем reportParams
            var reportParams = new ReportParams();
            report.BaseParams = baseParams;
            report.SetUserParams(new UserParamsValues());
            report.PrepareReport(reportParams);

            // получаем Генератор отчета
            var generatorName = report.GetReportGenerator();

            var generator = Container.Resolve<IReportGenerator>(generatorName);

            reportProvider.GenerateReport(report, stream, generator, reportParams);

            return stream;
        }

        private IQueryable<RealityObject> GetRoQueryForSubsidy(long muId)
        {
            return Container.Resolve<IRepository<RealityObject>>().GetAll()
                    .Where(x => VersionRecordDomain.GetAll()
                        .Where(y => y.ProgramVersion.IsMain && y.RealityObject.Id == x.Id)
                        .Any(y => y.ProgramVersion.Municipality.Id == muId))
                    .Where(x => x.ConditionHouse == ConditionHouse.Serviceable
                                || x.ConditionHouse == ConditionHouse.Dilapidated)
                    .Where(x =>
                        !EmergencyDomain.GetAll().Any(e => e.RealityObject.Id == x.Id)
                        || EmergencyDomain.GetAll()
                            .Where(e =>
                                e.ConditionHouse == ConditionHouse.Serviceable
                                || e.ConditionHouse == ConditionHouse.Dilapidated)
                            .Any(e => e.RealityObject.Id == x.Id))
                    .Where(x => !x.IsNotInvolvedCr);
        }

        private Dictionary<int, decimal> GetOwnerCollection(Municipality municipality)
        {
            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var rateCalcArea = config.RateCalcTypeArea;

            var muId = municipality.Id;

            var roQuery = GetRoQueryForSubsidy(muId);

            var roRealEstTypesDict = Container.Resolve<IRealEstateTypeService>().GetRealEstateTypes(roQuery);

            var typeRateByRealEstType =
                TypeRateDomain.GetAll()
                    .Where(x => x.SociallyAcceptableRate.HasValue)
                    .Select(x => new { Id = (long?)x.RealEstateType.Id, x.Year, x.SociallyAcceptableRate })
                    .ToList();

            var roAreas = roQuery
                .Select(x => new
                {
                    x.Id,
                    x.AreaLiving,
                    x.AreaMkd,
                    x.AreaLivingNotLivingMkd
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.AreaLiving,
                    x.AreaMkd,
                    x.AreaLivingNotLivingMkd,
                    Tarifs =
                        roRealEstTypesDict.ContainsKey(x.Id)
                            ? typeRateByRealEstType.Where(y => roRealEstTypesDict[x.Id].Contains(y.Id.ToLong())).ToList()
                            : typeRateByRealEstType.Where(y => !y.Id.HasValue).ToList()
                })
                .ToList();

            return TypeRateDomain.GetAll()
                .Select(x => new
                {
                    x.Year,
                    x.SociallyAcceptableRate,
                    x.SociallyAcceptableRateNotLivingArea
                })
                .AsEnumerable()
                .GroupBy(x => x.Year)
                .ToDictionary(x => x.Key, y =>
                {
                    switch (rateCalcArea)
                    {
                        case RateCalcTypeArea.AreaLiving:
                            {
                                return roAreas.Where(x => x.Tarifs.Any())
                                              .Select(x => x.AreaLiving.ToDecimal() * x.Tarifs.Where(z => z.Year == y.Key).Max(z => z.SociallyAcceptableRate).ToDecimal() * 12)
                                              .Sum()
                                     + roAreas.Where(x => x.Tarifs.Any() && (x.AreaLivingNotLivingMkd.ToDecimal() - x.AreaLiving.ToDecimal()) > 0)
                                              .Select(x => (x.AreaLivingNotLivingMkd.ToDecimal() - x.AreaLiving.ToDecimal()) * x.Tarifs.Where(z => z.Year == y.Key).Max(z => z.SociallyAcceptableRate).ToDecimal() * 12)
                                              .Sum();
                            }
                        case RateCalcTypeArea.AreaLivingNotLiving:
                            {
                                return roAreas.Where(x => x.Tarifs.Any())
                                                  .Select(x => x.AreaLivingNotLivingMkd.ToDecimal() * x.Tarifs.Where(z => z.Year == y.Key).Max(z => z.SociallyAcceptableRate).ToDecimal() * 12)
                                                  .Sum();
                            }
                        case RateCalcTypeArea.AreaMkd:
                            {
                                return roAreas.Where(x => x.Tarifs.Any())
                                                   .Select(x => x.AreaMkd.ToDecimal() * x.Tarifs.Where(z => z.Year == y.Key).Max(z => z.SociallyAcceptableRate).ToDecimal() * 12)
                                                   .Sum();
                            }
                    }

                    return 0m;
                });
        }
    }

    #region Utils

    public static class BudgetDecimalExtensions
    {
        /// <summary>
        /// Возвращает 0, если значение меньше 0
        /// </summary>
        public static decimal ZeroIfBelowZero(this decimal value)
        {
            if (value < 0)
            {
                return 0M;
            }

            return value;
        }

        /// <summary>
        /// Перевод в проценты / 100
        /// </summary>
        public static decimal ToDivisional(this decimal val)
        {
            if (val > 1)
            {
                return val / 100;
            }

            return val;
        }
    }

    #endregion

    #region Proxy classes

    /// <summary>
    /// Сущность, содержащая данные, необходимые при учете корректировки ДПКР
    /// Лимит займа, Дефицит ...
    /// </summary>
    public class DpkrCorrectionProxy : BaseEntity
    {
        /// <summary>
        /// Id Объекта недвижимости
        /// </summary>
        public virtual long RealityObjectId { get; set; }

        /// <summary>
        /// Id Муниципального образования
        /// </summary>
        public virtual long MunicipalityId { get; set; }

        /// <summary>
        /// Скорректированный год который получился врезультате субсидирования
        /// </summary>
        public virtual int PlanYear { get; set; }

        /// <summary>
        /// Идекс очередности из 3 этапа
        /// </summary>
        public virtual int Index { get; set; }

        /// <summary>
        /// Стоимость 3 этапа
        /// </summary>
        public virtual decimal SumStage3 { get; set; }

        /// <summary>
        /// Стоимость работ
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Идентификатор 2 этапа
        /// </summary>
        public virtual long Stage2Id { get; set; }

        /// <summary>
        /// Идентификатор 3 этапа
        /// </summary>
        public virtual long Stage3Id { get; set; }

        /// <summary>
        /// Идентификатор ООИ
        /// </summary>
        public virtual long CeoId { get; set; }

        /// <summary>
        /// Вес ООИ
        /// </summary>
        public virtual int CeoWeight { get; set; }

        /// <summary>
        /// Если запись прошла расчет то будет true , иначе false
        /// получается что если falseродолжаем обрабатывать запись
        /// </summary>
        public virtual bool IsCalculated { get; set; }

        /// <summary>
        /// Идентификатор Версии
        /// </summary>
        public virtual long VersionId { get; set; }

        /// <summary>
        /// Изменялся ли плановый год
        /// </summary>
        public virtual bool IsChangedYear { get; set; }

        /// <summary>
        /// Изменялся ли плановый год
        /// </summary>
        public virtual bool IsManuallyCorrect { get; set; }
    }

    /// <summary>
    /// Прокси сущность для Доли финансирования работ
    /// </summary>
    public class ShareFinancingCeoProxy : BaseEntity
    {
        /// <summary>
        /// Id ООИ
        /// </summary>
        public virtual long CeoId { get; set; }

        /// <summary>
        /// Доля
        /// </summary>
        public virtual decimal Share { get; set; }

        /// <summary>
        /// Сумма на КР
        /// </summary>
        public virtual decimal Sum { get; set; }
    }

    #endregion
}