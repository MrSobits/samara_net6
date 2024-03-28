namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Domain;
    using Bars.Gkh.Overhaul.Tat.ConfigSections;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Utils;
    using Castle.Windsor;
    using Enum;

    /// <summary>
    /// Домен сервис для субсидирования
    /// </summary>
    public partial class SubsidyMunicipalityService : ISubsidyMunicipalityService
    {
        #region property injection

		/// <summary>
		/// Контейнер
		/// </summary>
        public IWindsorContainer Container { get; set; }

		/// <summary>
		/// Домен сервис для Субсидия для муниципального образования
		/// </summary>
		public IDomainService<SubsidyMunicipality> SubsidyMuService { get; set; }

		/// <summary>
		/// Домен сервис для Запись субсидии для муниципального образования
		/// </summary>
		public IDomainService<SubsidyMunicipalityRecord> SubsidyRecordService { get; set; }

		/// <summary>
		/// Домен сервис для  Сущность, содержащая данные, необходимые при учете корректировки ДПКР
		/// </summary>
		public IDomainService<DpkrCorrectionStage2> DpkrService { get; set; }

		/// <summary>
		/// Домен сервис для Версия программы
		/// </summary>
		public IDomainService<ProgramVersion> VersionDomain { get; set; }

		/// <summary>
		/// Домен сервис для Версия ДПКР
		/// </summary>
		public IDomainService<SubsidyRecordVersionData> SubsidyRecordVersionDomain { get; set; }

		/// <summary>
		/// Домен сервис для Версионирование второго этапа
		/// </summary>
		public IDomainService<VersionRecordStage2> VersionStage2Domain { get; set; }

		/// <summary>
		/// Домен сервис для Запись краткосрочной программы
		/// </summary>
		public IDomainService<ShortProgramRecord> ShortRecDomain { get; set; }

        #endregion property injection

        /// <summary>
        /// Получение субсидии по МО 
        /// </summary>
        public IDataResult GetSubsidy(BaseParams baseParams)
        {
            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");

            ProgramVersion version;
            if (!TryGetVersion(municipalityId, out version))
            {
                return new BaseDataResult(false, "Для выбранного муниципального образования отсутствует основная версия");
            }

            var config = Container.GetGkhConfig<OverhaulTatConfig>();
            var periodStart = config.ProgrammPeriodStart;
            var periodEnd = config.ProgrammPeriodEnd;

            SubsidyMunicipality subsidyMu;
            if (!TryGetSubsidy(municipalityId, out subsidyMu))
            {
                subsidyMu =
                    new SubsidyMunicipality
                        { Municipality = Container.Resolve<IDomainService<Municipality>>().Load(municipalityId) };

                SubsidyMuService.Save(subsidyMu);
            }

            // Получаем существующие записи субсидирования
            var dictSubsidyRecords =
                SubsidyRecordService.GetAll()
                    .Where(x => x.SubsidyMunicipality.Municipality.Id == municipalityId)
                    .ToDictionary(x => x.SubsidyYear);

            // Получаем существующие записи для текущей версии (Поскольку часть полей будет там)
            var dictSubsidyVersion =
                SubsidyRecordVersionDomain.GetAll()
                    .Where(x => x.Version.Id == version.Id)
                    .ToDictionary(x => x.SubsidyRecordUnversioned.Id, y => y.Id);

            var listSubsidyToSave = new List<SubsidyMunicipalityRecord>();
            var listVersionToSave = new List<SubsidyRecordVersionData>();
            
            // Далее проверяем изменились ли года в параметрах ДПКР
            for (int currentYear = periodStart; currentYear <= periodEnd; currentYear++)
            {
                SubsidyMunicipalityRecord rec;

                // Сначала проверяем существовала ли субсидия на этот год
                // Если нет то создаем новую и записываем в список на сохранение
                if (dictSubsidyRecords.ContainsKey(currentYear))
                {
                    rec = dictSubsidyRecords[currentYear];
                    dictSubsidyRecords.Remove(currentYear);
                }
                else
                {
                    rec = new SubsidyMunicipalityRecord
                    {
                        SubsidyYear = currentYear,
                        SubsidyMunicipality = subsidyMu
                    };

                    listSubsidyToSave.Add(rec);
                }

                // теперь проверяем существовала ли субсидия для этой версии
                // если не было версии то создаем версию для субсидии
                // Если для субсидии нет версии, то тогда мы создаем 
                if (!dictSubsidyVersion.ContainsKey(rec.Id))
                {
                    var versionRec = new SubsidyRecordVersionData
                    {
                        Version = version,
                        SubsidyRecordUnversioned = rec
                    };

                    listVersionToSave.Add(versionRec);
                }   
            }

            // Если есть необходимость удалять субсидирование (например если поменялись года Начала или Окончания ДПКР в настройках)
            // То следом за удаляемыми записями субсидирования удаляем и все версионные записи
            var deleteIds = dictSubsidyRecords.Values.Select(x => x.Id).Distinct().ToList();

            var listVersionsForDelete = SubsidyRecordVersionDomain.GetAll()
                .Where(x => deleteIds.Contains(x.SubsidyRecordUnversioned.Id))
                .Select(x => x.Id)
                .ToList();

            using (var tx = OpenTransaction())
            {
                try
                {
                    // Сохраняем субсидирвоание (Если еще небыло сохранено)
                    SaveOrUpdate(listSubsidyToSave, SubsidyRecordService);

                    // Сохраняем версии субсидирвоание (Если еще небыло сохранено)
                    SaveOrUpdate(listVersionToSave, SubsidyRecordVersionDomain);

                    foreach (var id in listVersionsForDelete)
                    {
                        SubsidyRecordVersionDomain.Delete(id);
                    }

                    // Если в списке subsidyRecordsDicts остались какието записи значит, что периоды изменились
                    // но в субсидирвоании остались какието записи
                    foreach (var id in deleteIds)
                    {
                        SubsidyRecordService.Delete(id);
                    }

                    tx.Commit();
                }
                catch (ValidationException e)
                {
                    tx.Rollback();
                    return new BaseDataResult(false, e.Message);
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }

            return new BaseDataResult(new {versionId = version.Id});
        }

        /// <summary>
        /// Метод расчета показателей
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult CalcValues(BaseParams baseParams)
        {
            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");

            ProgramVersion version;

            if (!TryGetVersion(municipalityId, out version))
            {
                return new BaseDataResult(false, "Для выбранного муниципального образования отсутствует основная версия");
            }

            if (version.State.FinalState)
            {
                return new BaseDataResult(false, "Нельзя менять версию с конечным статусом");
            }

            SubsidyMunicipality subsidyMu;

            if (!TryGetSubsidy(municipalityId, out subsidyMu))
            {
                return new BaseDataResult(false, "Не удалось получить субсидирование");
            }

            var modifiedRecords = baseParams.Params.GetAs<SubsidyMunicipalityRecord[]>("records");

            foreach (var item in modifiedRecords)
            {
                item.SubsidyMunicipality = subsidyMu;
            }

            SaveOrUpdate(modifiedRecords, SubsidyRecordService);
            
            /*
             Сначала пользователь забивает бюджеты там где ячейки дают забивать для РТ будет только 1 год = 2014
             Пользователь забил бюджеты далее он нажимает Расчитать показатели 
             
             NeedBudgetForCr - Потребность на год расчитывается так
                  Берем 2 этап версии (Накладываем фильтр по МО и по Версии) далее берем тольк онужный год например по 2014 году и получаем сумму денег
                  напрмиер получим так
                  2014 год - потребность 1 255 000 руб
                  2015 год - потребность 2 340 500 руб
              
             Deficit - Остаток расчитывается так
                  Зная бюджеты и Потребность мы просто делаем такую легкую формулу
             
                  Остаток на год = "Бюджет на год" - "Потребность на КР на год"        
            */

            var existSubsidyRecData =
                SubsidyRecordVersionDomain.GetAll()
                    .Where(x => x.Version.Id == version.Id)
                    .ToDictionary(x => x.SubsidyRecordUnversioned.Id);

            var stage3Records =
                Container.Resolve<IDomainService<VersionRecord>>().GetAll()
                    .Where(x => x.ProgramVersion.Id == version.Id)
                    .GroupBy(x => x.CorrectYear)
                    .ToDictionary(x => x.Key, y => y.Sum(x => x.Sum));

            var listForSave = new List<SubsidyRecordVersionData>();

            foreach (var record in GetSubsidyMuRecords(subsidyMu.Id))
            {
                SubsidyRecordVersionData newRecord;

                if (existSubsidyRecData.ContainsKey(record.Id))
                {
                    newRecord = existSubsidyRecData[record.Id];
                }
                else
                {
                    newRecord = new SubsidyRecordVersionData
                    {
                        SubsidyRecordUnversioned = record,
                        Version = version
                    };
                }

                newRecord.NeedFinance = stage3Records.Get(record.SubsidyYear).RoundDecimal(2);

                newRecord.Deficit = (record.BudgetRegion + record.BudgetMunicipality + record.BudgetFcr + record.OwnerSource) - newRecord.NeedFinance;

                listForSave.Add(newRecord);
            }

            SaveOrUpdate(listForSave, SubsidyRecordVersionDomain);

            return new BaseDataResult();
        }

        /// <summary>
        /// Корректировка ДПКР
        /// </summary>
        public IDataResult CorrectDpkr(BaseParams baseParams)
        {
            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");

            ProgramVersion version;
            if (!TryGetVersion(municipalityId, out version))
            {
                return new BaseDataResult(false, "Для выбранного муниципального образования отсутствует основная версия");
            }

            if (version.State.FinalState)
            {
                return new BaseDataResult(false, "Нельзя менять версию с конечным статусом");
            }

            SubsidyMunicipality subsidyMu;
            if (!TryGetSubsidy(municipalityId, out subsidyMu))
            {
                return new BaseDataResult(false, "Не удалось получить субсидирование");
            }

            var modifiedRecords = baseParams.Params.GetAs<SubsidyMunicipalityRecord[]>("records");

            foreach (var item in modifiedRecords)
            {
                item.SubsidyMunicipality = subsidyMu;
            }

            // 1. Сохраняем записи которые пришли измененные
            SaveOrUpdate(modifiedRecords, SubsidyRecordService);

            var config = Container.GetGkhConfig<OverhaulTatConfig>();
            var actualizeYear = config.ActualizePeriodStart;
            var actualizeEnd = config.ActualizePeriodEnd;
            var endYear = config.ProgrammPeriodEnd;
            var checkFixingFeatureCorrectionYear = config.CheckFixingFeatureCorrectionYear;

            //Такую фигню делаю для того чтобы непоставили в наастройках 2014 так как прошедшие года недолжны изменится так как они уже прошли и изменений вних недолжно произойти
            if (actualizeYear < 2015)
            {
                return new BaseDataResult(false, "Года начала актуализации должен быть >= 2015");
            }

            var correctionYears = DpkrService.GetAll()
                .Where(x => x.Stage2.Stage3Version.ProgramVersion.Id == version.Id)
                .Select(x => new
                {
                    x.Stage2.Id,
                    x.PlanYear
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.PlanYear).FirstOrDefault());

            // записи второго этапа, по которым считается корректировка
            var stage2Correction = VersionStage2Domain.GetAll()
                .Where(x => x.Stage3Version.ProgramVersion.Id == version.Id)
                .Select(x => new DpkrCorrectionProxy
                {
                    Id = x.Id,
                    stage3Id = x.Stage3Version.Id,
                    FixedYear = x.Stage3Version.FixedYear,
                    Year = x.Stage3Version.Year,
                    IndexNumber = x.Stage3Version.IndexNumber,
                    Sum = x.Stage3Version.Sum, // тут берем именно сумму по 3 этапу потмоу как записи могут быт ьсгруппирвоаны и они должны целой суммой списатся с бюджета или сдвинутся одновременно
                    RoId = x.Stage3Version.RealityObject.Id,
                    Point = x.Stage3Version.Point
                })
                .ToArray()
                .Select(x => new DpkrCorrectionProxy
                {
                    Id = x.Id,
                    stage3Id = x.stage3Id,
                    FixedYear = x.FixedYear,
                    Year = (x.FixedYear || checkFixingFeatureCorrectionYear) && correctionYears.ContainsKey(x.Id) ? correctionYears[x.Id] : x.Year,
                    IndexNumber = x.IndexNumber,
                    Sum = x.Sum,
                    RoId = x.RoId,
                    Point = x.Point
                })
                .ToList();

            var versionDataToSave = new List<SubsidyRecordVersionData>();

            var versionDataRecords = SubsidyRecordVersionDomain.GetAll()
                .Where(x => x.Version.Id == version.Id)
                .Where(x => x.SubsidyRecordUnversioned.SubsidyMunicipality.Municipality.Id == municipalityId)
                .ToDictionary(x => x.SubsidyRecordUnversioned.Id);

            var result = new LinkedList<DpkrCorrectionProxy>();
            var stage3Success = new Dictionary<long, int>();

            //Необходимо работать только с теми записями, которые начинаются с периода актуализации, тоест ьгода 2014 изменят ьнельзя совсем, так как они уже прошли
            foreach (var subsidy in GetSubsidyMuRecords(subsidyMu.Id).OrderBy(x => x.SubsidyYear))
            {
                var year = subsidy.SubsidyYear;

                //все деньги, которые наскребли в текущем году
                var sum = GetSumSubsidy(subsidy);

                var needFinance = 0m;

                //сортировка на случай если обезьянки ошиблись с ручной корректировкой номера или года
                foreach (var correction in stage2Correction
                    .OrderBy(x => x.Year)
                    .ThenByDescending(x => x.FixedYear)
                    .ThenBy(x => x.IndexNumber)
                    .ThenByDescending(x => x.Point))
                {
                    if (checkFixingFeatureCorrectionYear)
                    {
                        if (correction.Year == year)
                        {
                            result.AddLast(correction);

                            stage2Correction.Remove(correction);

                            if (!stage3Success.ContainsKey(correction.stage3Id))
                            {
                                stage3Success.Add(correction.stage3Id, year);
                                needFinance += correction.Sum;

                            }
                            continue;
                        }

                        break;
                    }

                    // если дошли до записи через год, значит в текущем году работы и/или суммы полностью забиты
                    //неоходимо брать только те записи которые начинаются относителньо периода актуализации
                    //тоесть прошлае года 2014 недолжны пересчитатся или изменится 
                    if (year < actualizeYear && correction.Year > year)
                    {
                        break; // если год субсидрования 2014 или старый то нельзя в него поднимать работы с последующих годов нужно оставить все как есть 
                    }

                    if (correction.Year > year + 1)
                    {
                        break;
                    }

                    if (stage3Success.ContainsKey(correction.stage3Id))
                    {
                        // поскольку сумма уже была учтена по 3 этапу значит прос тоработам присваиваем текущий год 

                        var yearCorrectSt3 = stage3Success[correction.stage3Id];
                        correction.Year = yearCorrectSt3;
                        result.AddLast(correction);

                        stage2Correction.Remove(correction);

                    }
                    else if (correction.FixedYear && subsidy.SubsidyYear == correction.Year)
                    {
                        // если работа не вмещается в текущий год, то перекидываем ее на следующий год
                        // если текущий год - последний, то перекидывать некуда, оставляем тут

                        // Данную запись у которой ест ьпризнак фиксации года фиксируем только в своем году
                        // Для того, чтобы эти запеси непереносились в другие года и несдвигались в года выше
                        
	                    result.AddLast(correction);

                        stage2Correction.Remove(correction);

                        if (!stage3Success.ContainsKey(correction.stage3Id))
                        {
                            stage3Success.Add(correction.stage3Id, year);
                            needFinance += correction.Sum;
                        }
                    }
                    else if (!correction.FixedYear)
                    {
						//при включении настройки "Проверка признака фиксации скорректированного года"
						//в краткосрочную программу включать только работы с признаком "Фиксация скорректированного года"

						// иначе если запись без фиксации года, то необходимо расчитывать по юджетам чтобы не привысить 
                        if (year < endYear && needFinance + correction.Sum > sum)
                        {
                            correction.Year = year + 1;
                            continue;
                        }
                        
                        correction.Year = year;

                        result.AddLast(correction);

                        stage2Correction.Remove(correction);

                        if (!stage3Success.ContainsKey(correction.stage3Id))
                        {
                            stage3Success.Add(correction.stage3Id, year);
                            needFinance += correction.Sum;
                        }
                    }
                }

                var versionRecord = versionDataRecords[subsidy.Id];

                versionRecord.CorrDeficit = sum - needFinance;
                versionRecord.CorrNeedFinance = needFinance;

                versionDataToSave.Add(versionRecord);
            }

            var correctionToSave = new LinkedList<DpkrCorrectionStage2>();

            // записи второго этапа, для обновления типа корректировки
            var dictSt2 = VersionStage2Domain.GetAll()
                .Where(x => x.Stage3Version.ProgramVersion.Id == version.Id)
                .AsEnumerable()
                .ToDictionary(x => x.Id);

            // Поскольку корректировка будет менять ТипКорректировки у версии то надо их сохранять
            var stage2ToSave = new LinkedList<VersionRecordStage2>();

            // идентификаторы второго этапа версии, которые есть в краткосрочке
            var setStageInShortYears = ShortRecDomain.GetAll()
// ReSharper disable once ImplicitlyCapturedClosure
                .Where(x => x.Stage1.Stage2Version.Stage3Version.ProgramVersion.Id == version.Id)
                .Select(x => new {st2Id = x.Stage1.Stage2Version.Id, x.ShortProgramObject.Year})
                .AsEnumerable()
                .GroupBy(x => x.st2Id)
                .ToDictionary(x => x.Key, y => y.Select(z => z.Year).First());

            foreach (var correction in result)
            {
                var stage2 = dictSt2[correction.Id];
                stage2.TypeCorrection = TypeCorrection.Done; // ставим, что запись прошла через корректировку
                stage2ToSave.AddLast(stage2);

                var corrStage = new DpkrCorrectionStage2
                {
                    Stage2 = stage2,
                    PlanYear = correction.Year,
                    RealityObject = new RealityObject { Id = correction.RoId },
                    TypeResult = TypeResultCorrectionDpkr.New
                };

                // После того как получили Корректировку необходимо проставить ТипРезультатаКорректировки
                CorrectTypeResult(corrStage, setStageInShortYears, actualizeYear, actualizeEnd);

                correctionToSave.AddLast(corrStage);
            }

            InTransaction(session =>
            {
                //удаляем тольк оте записи, которые больше периода актуализации
                // так как 2014 год и все прошлые года недолжны удалятся или изменятся
                session.CreateSQLQuery(
                    string.Format(@"delete from ovrhl_dpkr_correct_st2 where st2_version_id in (
	                                    select st2.id 
	                                    from ovrhl_stage2_version st2
	                                    inner join ovrhl_version_rec st3 on st3.id = st2.st3_version_id and st3.version_id = {0})",
                        version.Id
                        ))
                    .ExecuteUpdate();

                versionDataToSave.ForEach(x =>
                {
                    if (x.Id > 0)
                        session.Update(x);
                    else
                        session.Insert(x);
                });

                correctionToSave.ForEach(x =>
                {
                    if (x.Id > 0)
                        session.Update(x);
                    else
                        session.Insert(x);
                });

                stage2ToSave.ForEach(x =>
                {
                    if (x.Id > 0)
                        session.Update(x);
                    else
                        session.Insert(x);
                });
            });

            return new BaseDataResult();
        }
    }

    internal class DpkrCorrectionProxy
    {
        public long Id { get; set; }

        public long stage3Id { get; set; }

        public bool FixedYear { get; set; }

        public int Year { get; set; }

        public decimal Sum { get; set; }

        public int IndexNumber { get; set; }

        public long RoId { get; set; }

        public decimal Point { get; set; }
    }

    public static class BudgetDecimalExtensions
    {
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
}