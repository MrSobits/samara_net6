namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.Overhaul;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Nso.ConfigSections;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.Gkh.Report;

    using Castle.Windsor;
    using Enum;
    using Gkh.Entities.RealEstateType;
    using Gkh.Utils;
    using Utils;

    public class SubsidyRecordService : ISubsidyRecordService
    {
        #region Property injection

        public IWindsorContainer Container { get; set; }

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

        public IDomainService<RealEstateTypeRate> TypeRateDomain { get; set; }

        public IDomainService<ShortProgramDifitsit> ShortProgramDifitsitDomain { get; set; }

        public IDomainService<ShortProgramRecord> ShortProgramRecordDomain { get; set; }

        #endregion Property injection

        private OverhaulNsoConfig Config { get; set; }

        /// <summary>
        /// В данном методе мы либо создаем судсидии если в периоде ДПКР они еще несозданы, либо удаляем субсидии если период был изменен
        /// но записи предположим существовали
        /// </summary>
        public IDataResult GetSubsidy(BaseParams baseParams)
        {
            var version = VersionDomain.GetAll().FirstOrDefault(x => x.IsMain);

            if (version == null)
            {
                throw new ValidationException("Не найдена основная версия!");
            }

            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var periodStart = config.ProgrammPeriodStart;
            var periodEnd = config.ProgrammPeriodEnd;

            // Получаем существующие записи субсидирования
            var subsidyRecordsDicts =
                SubsidyRecordDomain.GetAll()
                                   .AsEnumerable()
                                   .GroupBy(x => x.Id)
                                   .ToDictionary(x => x.Key, y => y.FirstOrDefault());

            // Получаем существующие записи для текущей версии (Поскольку часть полей будет там)
            var subsidyVersion = SubsidyRecordVersionDomain.GetAll().Where(x => x.Version.Id == version.Id).ToList();


            var listSubsidyToSave = new List<SubsidyRecord>();
            var listVersionToSave = new List<SubsidyRecordVersion>();

            // Далее проверяем изменились ли года в параметрах ДПКР
            var currentYear = periodStart;
            while (currentYear <= periodEnd)
            {
                var rec = subsidyRecordsDicts.Values.FirstOrDefault(x => x.SubsidyYear == currentYear);

                // Сначала проверяем существовала ли субсидия на этот год
                // Если нет то создаем новую и записываем в список на сохранение
                if (rec != null)
                {
                    // Если запись найдена по этому году то удаляем ее из списка чтобы неудалить потом
                    if (subsidyRecordsDicts.ContainsKey(rec.Id))
                    {
                        subsidyRecordsDicts.Remove(rec.Id);
                    }
                }
                else
                {
                    rec = new SubsidyRecord
                              {
                                  SubsidyYear = currentYear,
                                  PlanOwnerPercent = (currentYear == periodStart ? 70 : 95),
                                  NotReduceSizePercent = (currentYear == periodStart ? 30 : 10)
                              };

                    listSubsidyToSave.Add(rec);
                }

                // теперь проверяем существовала ли субсидия для этой версии
                // если не было версии то создаем версию для субсидии
                var versionRec = subsidyVersion.FirstOrDefault(x => x.SubsidyRecord.Id == rec.Id);

                // Если для субсидии нет версии, то тогда мы создаем 
                if (versionRec == null)
                {
                    versionRec = new SubsidyRecordVersion
                                     {
                                         Version = version,
                                         SubsidyRecord = rec
                                     };

                    listVersionToSave.Add(versionRec);
                }

                currentYear++;
            }

            var listDeleteIds = subsidyRecordsDicts.Keys.ToList();

            var listVersionDeleteIds = SubsidyRecordVersionDomain.GetAll()
                                          .Where(x => listDeleteIds.Contains(x.SubsidyRecord.Id))
                                          .Select(x => x.Id)
                                          .ToList();

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

                    foreach (var id in listVersionDeleteIds)
                    {
                        SubsidyRecordVersionDomain.Delete(id);
                    }

                    // Если в списке subsidyRecordsDicts остались какието записи значит, что периоды изменились
                    // но в субсидирвоании остались какието записи
                    foreach (var id in listDeleteIds)
                    {
                        SubsidyRecordDomain.Delete(id);
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
        ///   3. Считаем сумму Плановая Собираемость по формуле "СуммаЖилойПлощади * СоцТарифПоТипу * 12"
        ///   4. Считаем средства СобственниковНаКР и ОстатокЗаПредыдущийГод
        /// </summary>
        public IDataResult CalcOwnerCollection(BaseParams baseParams)
        {
            // в этом массиве будут измененные строки если такие требуеются
            // То перед выполнением действий требуется их сохранить, а уже потом расчитывать
            var modifiedRecords = baseParams.Params.GetAs<SubsidyRecord[]>("records");

            // 1. Сохраняем записи которые пришли измененные
            this.SaveSubsidyRecords(modifiedRecords);


            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var usePlanCollection = config.SubsidyConfig.UsePlanOwnerCollectionType;

            IDataResult result = null;

            switch (usePlanCollection)
            {
                case UsePlanOwnerCollectionType.LastYear:
                {
                    result = UseLastYearCollection();
                }
                break;

                case UsePlanOwnerCollectionType.CurrentYear:
                {
                    result = UseCurrentYearCollection();
                }
                break;

                case UsePlanOwnerCollectionType.NaoMethod:
                {
                    result = UseNaoMethodCollection();
                }
                break;
            }

            return result;
        }

        /// <summary>
        /// Метод подсчета собираемости по старому алгоритму для Новосибирска, данный подсчет учитывает предыдущие резервы
        /// </summary>
        private IDataResult UseLastYearCollection()
        {
            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var periodStart = config.ProgrammPeriodStart;
            var periodEnd = config.ProgrammPeriodEnd;
            var periodShort = config.ShortTermProgPeriod;
            
            var dictTypeRates = TypeRateDomain.GetAll()
                .Select(x => new
                {
                    x.Year,
                    x.TotalArea,
                    x.SociallyAcceptableRate
                })
                .AsEnumerable()
                .GroupBy(x => x.Year)
                .ToDictionary(x => x.Key, y => y.Select(x => x.TotalArea * x.SociallyAcceptableRate).Sum() * 12);

            var subsidyRecords =
                SubsidyRecordDomain.GetAll()
                    .Where(x => x.SubsidyYear >= periodStart && x.SubsidyYear <= periodEnd)
                    .OrderBy(x => x.SubsidyYear)
                    .ToList();

            // Производим вычисления
            foreach (var rec in subsidyRecords)
            {
                // Получаем запись предыдущего года
                var beforeRec = subsidyRecords.FirstOrDefault(x => x.SubsidyYear == rec.SubsidyYear - 1);

                // Остаток на 1 января предыдущего года
                var befBefRec = subsidyRecords.FirstOrDefault(x => x.SubsidyYear == rec.SubsidyYear - 2);
                var reserveBefBefRec = befBefRec != null ? befBefRec.Reserve : 0M;

                //если период попадает в период краткосрочной программы
                //то считаем в соответствии с тарифами на год
                if (rec.SubsidyYear < periodStart + periodShort && dictTypeRates.ContainsKey(rec.SubsidyYear))
                {
                    rec.PlanOwnerCollection = dictTypeRates[rec.SubsidyYear].ToDecimal();
                }
                else
                {
                    // Плановая собираемость текущего года
                    // Считаем сумму Плановая Собираемость по формуле "СуммаЖилойПлощади * СоцТарифПоТипу * 12"
                    rec.PlanOwnerCollection = dictTypeRates.Any() ? dictTypeRates.FirstOrDefault().Value.ToDecimal() : 0m;
                }

                // Поступившие средства текущего года (считается как ПлановаяСобираемость*ПроцентРискаСбора)
                rec.Available = ((rec.PlanOwnerCollection * rec.PlanOwnerPercent).ToDivisional());

                if (beforeRec != null)
                {
                    // Считаем Средства на КР 
                    rec.OwnerSumForCr = (beforeRec.Available) * (1 - rec.NotReduceSizePercent.ToDivisional()) + reserveBefBefRec;

                    // Считаем сумму резерва (Остатка в текущем году)
                    rec.Reserve = beforeRec.Reserve + beforeRec.Available - beforeRec.OwnerSumForCr;
                }
                else
                {
                    // Если нет предыдущего года то мы находимся в 1 году соответсвенно мы поулчаем просто Плановую собираемость
                    // Поскольку остатка нет то Плановую собираемость текущего года с учетом НеСнижаемогоРазмераРегФонда
                    rec.OwnerSumForCr = (rec.Available * (1 - rec.NotReduceSizePercent.ToDivisional()));

                    // Оставшуюся часть фиксируем как остаток первого года
                    rec.Reserve = rec.Available - rec.OwnerSumForCr;
                }
            }

            using (var tx = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    // Сохраняем субсидирвоание (Если еще небыло сохранено)
                    foreach (var rec in subsidyRecords)
                    {
                        SubsidyRecordDomain.Update(rec);
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
        /// Метод расчета по методу НАО
        /// </summary>
        /// <returns></returns>
        private IDataResult UseNaoMethodCollection()
        {
            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var periodStart = config.ProgrammPeriodStart;
            var periodEnd = config.ProgrammPeriodEnd;
            var periodShort = config.ShortTermProgPeriod;

            var dictTypeRates = TypeRateDomain.GetAll()
                .Select(x => new
                {
                    x.Year,
                    x.TotalArea,
                    x.SociallyAcceptableRate
                })
                .AsEnumerable()
                .GroupBy(x => x.Year)
                .ToDictionary(x => x.Key, y => y.Select(x => x.TotalArea * x.SociallyAcceptableRate).Sum() * 12);

            var subsidyRecords =
                SubsidyRecordDomain.GetAll()
                    .Where(x => x.SubsidyYear >= periodStart && x.SubsidyYear <= periodEnd)
                    .OrderBy(x => x.SubsidyYear)
                    .ToList();

            decimal useMeAsOwnerSumForCr = 0;

            var shortRecords = subsidyRecords.Where(x => x.SubsidyYear < periodStart + periodShort).ToList();

            //этот индекс означает какой номер года сейчас идет 1ый, 2ой, 3ий и так далее
            var indexYear = 0;

            // Производим вычисления
            foreach (var rec in shortRecords)
            {

                indexYear++;

                // Получаем запись предыдущего года
                var beforeRec = subsidyRecords.FirstOrDefault(x => x.SubsidyYear == rec.SubsidyYear - 1);

                // если период попадает в период краткосрочной программы
                // то считаем в соответствии с тарифами на год
                if (rec.SubsidyYear < periodStart + periodShort && dictTypeRates.ContainsKey(rec.SubsidyYear))
                {
                    rec.PlanOwnerCollection = dictTypeRates[rec.SubsidyYear].ToDecimal();
                }
                else
                {
                    // Плановая собираемость текущего года
                    // Считаем сумму Плановая Собираемость по формуле "СуммаЖилойПлощади * СоцТарифПоТипу * 12"
                    rec.PlanOwnerCollection =
                        dictTypeRates.Any()
                            ? dictTypeRates.FirstOrDefault().Value.ToDecimal()
                            : 0m;
                }

                switch (indexYear)
                {
                    // Если выясляем для Первого года программы 
                    case 1:
                    {
                        // Поступившие средства текущего года (считается как ПлановаяСобираемость*ПроцентРискаСбора) - Это сумма которую вообще можно израсходоват ьна капремонт
                        rec.Available = (rec.PlanOwnerCollection * rec.PlanOwnerPercent).ToDivisional();

                        // Теперь поулчаем сумму на КР для 1 года - тоесть это сумма с учетом процента на Резерв
                        rec.OwnerSumForCr = (rec.Available * (1 - rec.NotReduceSizePercent.ToDivisional()));

                        // в Резерв уходит разность между всеми Дозволенными деньгамии деньгами с учетом процента на резерв
                        rec.Reserve = rec.Available - rec.OwnerSumForCr;
                    }
                    break;

                    // Для второго года
                    case 2:
                    {
                        rec.Available = (rec.PlanOwnerCollection * rec.PlanOwnerPercent).ToDivisional();

                        // Для второго года мы в качестве Дозволенных средств на КР берм из 1го года - такая вот фигня
                        rec.OwnerSumForCr = (beforeRec.Available * (1 - rec.NotReduceSizePercent.ToDivisional()));

                        rec.Reserve = rec.Available - rec.OwnerSumForCr;
                    }
                    break;

                    // Для третьего года
                    case 3:
                    {
                        // Получаем запись еще более раннюю, например для 2016 года тут будет 2014
                        var before2Rec = subsidyRecords.FirstOrDefault(x => x.SubsidyYear == rec.SubsidyYear - 2);

                        // Вот тут как раз интересный момент, что сумма денег в 3ем году является Суммой текущей собираемости + резервы за 2014,2015
                        // То есть по данному методу, РЕзервы  не тратятся до 3го года тоест ьдо 2016 года, а потом в 2016 году они учитывабются сразу за 2 предыдущих года
                        rec.Available = (rec.PlanOwnerCollection * rec.PlanOwnerPercent).ToDivisional() + before2Rec.Reserve + beforeRec.Reserve;

                        rec.OwnerSumForCr = (rec.Available * (1 - rec.NotReduceSizePercent.ToDivisional()));

                        rec.Reserve = rec.Available - rec.OwnerSumForCr;
                    }
                    break;

                    // Для всех оставшихся годов
                    default:
                    {
                        // Все последующие года вычисляются также с учетом резерва ,но уже не 2х лет а только с предыдущим резервом
                        rec.Available = (rec.PlanOwnerCollection * rec.PlanOwnerPercent).ToDivisional() + beforeRec.Reserve;

                        rec.OwnerSumForCr = (rec.Available * (1 - rec.NotReduceSizePercent.ToDivisional()));

                        rec.Reserve = rec.Available - rec.OwnerSumForCr;
                    }
                    break;
                }

                // Записываем последнюю запись
                useMeAsOwnerSumForCr = rec.OwnerSumForCr;
                
            }

            // всем остальным присваиваем одну и туже собираемость прост оберем последнюю
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
                        SubsidyRecordDomain.Update(rec);
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
        /// Метод расчета собираемости относительно текущего года
        /// </summary>
        /// <returns></returns>
        private IDataResult UseCurrentYearCollection()
        {
            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var periodStart = config.ProgrammPeriodStart;
            var periodEnd = config.ProgrammPeriodEnd;
            var periodShort = config.ShortTermProgPeriod;

            var dictTypeRates = TypeRateDomain.GetAll()
                .Select(x => new
                {
                    x.Year,
                    x.TotalArea,
                    x.SociallyAcceptableRate
                })
                .AsEnumerable()
                .GroupBy(x => x.Year)
                .ToDictionary(x => x.Key, y => y.Select(x => x.TotalArea * x.SociallyAcceptableRate).Sum() * 12);

            var subsidyRecords =
                SubsidyRecordDomain.GetAll()
                    .Where(x => x.SubsidyYear >= periodStart && x.SubsidyYear <= periodEnd)
                    .OrderBy(x => x.SubsidyYear)
                    .ToList();

            decimal useMeAsOwnerSumForCr = 0;

            var shortRecords = subsidyRecords.Where(x => x.SubsidyYear < periodStart + periodShort).ToList();

            // Производим вычисления
            foreach (var rec in shortRecords)
            {
                // Получаем запись предыдущего года
                var beforeRec = subsidyRecords.FirstOrDefault(x => x.SubsidyYear == rec.SubsidyYear - 1);

                // если период попадает в период краткосрочной программы
                // то считаем в соответствии с тарифами на год
                if (rec.SubsidyYear < periodStart + periodShort && dictTypeRates.ContainsKey(rec.SubsidyYear))
                {
                    rec.PlanOwnerCollection = dictTypeRates[rec.SubsidyYear].ToDecimal();
                }
                else
                {
                    // Плановая собираемость текущего года
                    // Считаем сумму Плановая Собираемость по формуле "СуммаЖилойПлощади * СоцТарифПоТипу * 12"
                    rec.PlanOwnerCollection =
                        dictTypeRates.Any()
                            ? dictTypeRates.FirstOrDefault().Value.ToDecimal()
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
                rec.OwnerSumForCr = (rec.Available * (1 - rec.NotReduceSizePercent.ToDivisional()));

                // Оставшуюся часть фиксируем как остаток первого года
                rec.Reserve = rec.Available - rec.OwnerSumForCr;

                // Записываем последнюю запись
                useMeAsOwnerSumForCr = rec.OwnerSumForCr;
            }

            // всем остальным присваиваем одну и туже собираемость прост оберем последнюю
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
                        SubsidyRecordDomain.Update(rec);
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
        /// В этом методе мы расчитываем показатели
        /// То есть вычисляем Бюджеты и относительно бюджетов выстраиваем Скорректированную Очередь 
        /// Формируя тем самым Долгосрочную программу - DpkrCorrectionStage2
        /// А также после получения скорректированной очереди мы получаем из Долгосрочной программы записи Краткосрочной Программы
        /// И для них вычисляем Дифициты (ShortProgramDifitsit) а также сохраняем сами записи краткосрочной программы (ShortProgramRecord)
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса.</param>
        /// <returns>The <see cref="IDataResult"/>.</returns>
        public IDataResult CalcValues(BaseParams baseParams)
        {
            // в этом массиве будут измененные строки если такие требуеются
            // То перед выполнением действий требуется их сохранить, а уже потом расчитывать
            var modifiedRecords = baseParams.Params.GetAs<SubsidyRecord[]>("records");

            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var periodEnd = config.ProgrammPeriodEnd;
            var deleteWorkWithGreaterYear = config.SubsidyConfig.UseDeleteWorkWithGreaterYear == TypeUsage.Used;
            var useFixationPublishedYears = config.SubsidyConfig.UseFixationPublishedYears;

            // сохраняем измененные записи
            this.SaveSubsidyRecords(modifiedRecords);

            var session = Container.Resolve<ISessionProvider>().OpenStatelessSession();

            try
            {
                var version = VersionDomain.GetAll().FirstOrDefault(x => x.IsMain);

                if (version == null)
                {
                    throw new ValidationException("Не найдена основная версия!");
                }

                // 1 сначала удаляем существующие корректировки
                //   посколку грохаются корректироваки то необходимо вместе сними грохнуть и Дифициты по МО и Краткосрочную программу
                using (var tx = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        session.CreateSQLQuery("delete from ovrhl_short_prog_rec").ExecuteUpdate();
                        session.CreateSQLQuery("delete from ovhl_dpkr_correct_st2").ExecuteUpdate();
                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }

                // Подготавливаем данные для расчета
                var correction = this.GetThroughDpkrRows(version.Id);

                if (correction.Count == 0)
                {
                    const string Msg = "Расчет не был произведен, возможные причины:"
                                       + "<br>Не произведен расчет этапов долгосрочной программы,"
                                       + "<br>Неверный год окончания периода долгосрочной программы.";
                    return new BaseDataResult(false, Msg);
                }

                // Получаем записи субсидирования
                var subsidyRecords = SubsidyRecordDomain.GetAll()
                                       .OrderBy(x => x.SubsidyYear)
                                       .ToList();

                // получаем Года опубликованной программы по записям VersionStage.Id
                var dictPublishedRecords = new Dictionary<long, Dictionary<long, int>>();

                if (useFixationPublishedYears == TypeUsage.Used)
                {
                    dictPublishedRecords = PublishedRecordDomain.GetAll()
                                         .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
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
                                         .ToDictionary(x => x.Key, y => y.GroupBy(z => z.St2Id).ToDictionary(z => z.Key, x => x.Select(z => z.PublishedYear).FirstOrDefault()));
                }
                var publishedYears = dictPublishedRecords.ContainsKey(version.Id) ? dictPublishedRecords[version.Id] : new Dictionary<long, int>();
            
                // Запускаем метод который разрулит Какие бюджеты будут в субсидировании, Какие ООИ должны остатся
                // в своем году а какие должны перейти на следующий год
                this.CorrectionDpkr(correction, subsidyRecords, publishedYears);

                // Далее сохраняем полученные результаты
                // В этом списке будут записи корректировок на сохранение
                var correctionToSave = new List<DpkrCorrectionStage2>();

                foreach (var corr in correction.WhereIf(deleteWorkWithGreaterYear, x => x.PlanYear<=periodEnd))
                {
                    var newItem = new DpkrCorrectionStage2
                    {
                        RealityObject = new RealityObject
                        {
                            Id = corr.RealityObjectId,
                            Municipality = new Municipality { Id = corr.MunicipalityId }
                            // Такую фигню ставлю поскольку Надо для Дифицитов подсчитать по МО
                        },
                        PlanYear = corr.PlanYear,
                        Stage2 = new VersionRecordStage2
                        {
                            Id = corr.Stage2Id,
                            Sum = corr.Sum
                        }
                    };

                    correctionToSave.Add(newItem);         
                }
                
                // Получаем записи краткосрочной программы с уже подсчитанными значениями
                var shortRecordToSave = this.ShortProgramCalculation(correctionToSave, subsidyRecords);

                // Получаем дифициты по Годам и МО 
                var difitsitToSave = new List<ShortProgramDifitsit>();

                var dataDificit =
                    shortRecordToSave.Select(x => new
                    {
                        MunicipalityId = x.RealityObject.Municipality.Id,
                        x.Year,
                        SumDifitsit = x.Difitsit
                    })
                    .GroupBy(x => new { x.MunicipalityId, x.Year })
                    .ToDictionary(x => x.Key, y => y.Sum(x => x.SumDifitsit));

                // Получаем текущие дефициты
                var currentDeficit = ShortProgramDifitsitDomain.GetAll()
                        .Where(x => x.Version.Id == version.Id)
                        .AsEnumerable()
                        .GroupBy(x => x.Municipality.Id + "_" + x.Year)
                        .ToDictionary(x => x.Key, y => y.First());

                // для группировки создаем записи Дифицита
                foreach (var dif in dataDificit)
                {

                    ShortProgramDifitsit item;
                    var key = dif.Key.MunicipalityId + "_" + dif.Key.Year;
                    if (!currentDeficit.TryGetValue(key, out item))
                    {
                        item = new ShortProgramDifitsit
                                   {
                                       Municipality =
                                           new Municipality { Id = dif.Key.MunicipalityId },
                                       Version = version,
                                       Year = dif.Key.Year
                                   };
                    }

                    item.Difitsit = dif.Value;

                    difitsitToSave.Add(item);
                }

                // Получаем записи субсидирования
                var dictVersionRecords = SubsidyRecordVersionDomain.GetAll()
                    .Where(x => x.Version.Id == version.Id)
                    .AsEnumerable()
                    .GroupBy(x => x.SubsidyRecord.Id)
                    .ToDictionary(x => x.Key, y => y.First());

                using (var tr = session.BeginTransaction())
                {
                    try
                    {
                        // Сначала сохраняем Корректировки 
                        correctionToSave.ForEach(x => session.Insert(x));

                        // Теперь сохраняем записи Краткосрочной программы 
                        shortRecordToSave.ForEach(x => session.Insert(x));

                        // Теперь сохраняем дифициты краткосрочнйо программы
                        difitsitToSave.ForEach(x =>
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

                        // Поскольку в ходе вычислений БюджетаНаКр (BudgetCr) и ФактическойСуммыНаКР (CorrectionFinance)
                        // мы фиксировали значения не для Версии а в ТемповыеНеХранимыеПоля Субсидии
                        // То теперь необходимо их Сохранить в ту версию для которой производилось вычисление
                        foreach (var subsidy in subsidyRecords)
                        {
                            SubsidyRecordVersion currentVersion;

                            dictVersionRecords.TryGetValue(subsidy.Id, out currentVersion);

                            if (currentVersion != null)
                            {
                                currentVersion.BudgetCr = subsidy.BudgetCr;
                                currentVersion.CorrectionFinance = subsidy.CorrectionFinance;
                                currentVersion.BalanceAfterCr = subsidy.BalanceAfterCr;

                                session.Update(currentVersion);
                            }
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
            finally
            {
                Container.Resolve<ISessionProvider>().CloseCurrentSession();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// В этом методе по сформированной долгосрочной программе будем вычислять Краткосрочную программу
        /// И подсчитывать дифициты
        /// </summary>
        private List<ShortProgramRecord> ShortProgramCalculation(List<DpkrCorrectionStage2> correction, List<SubsidyRecord> subsidyRecords)
        {

            var result = new List<ShortProgramRecord>();

            var config = Container.GetGkhConfig<OverhaulNsoConfig>();

            // Начало периода
            var periodStart = config.ProgrammPeriodStart;

            // Количество лет краткосрочной программы
            var shortTermPeriod = config.ShortTermProgPeriod;

            var shortTerm = periodStart + shortTermPeriod;

            var data = correction.Where(x => x.PlanYear < shortTerm);

            var dictSubsidy = subsidyRecords.GroupBy(x => x.SubsidyYear).ToDictionary(x => x.Key, y => y.First());

            var dictSum = data.GroupBy(x => x.PlanYear).ToDictionary(x => x.Key, y => y.Select(x => x.Stage2.Sum).Sum());

            // для каждой записи отсеченной по году создаем запись краткосрочной программы
            foreach (var rec in data)
            {

                SubsidyRecord subsidy;

                dictSubsidy.TryGetValue(rec.PlanYear, out subsidy);

                if(subsidy == null)
                    continue;

                // Общая сумма на КР в году
                var sumInYear = dictSum[rec.PlanYear];
                var sumRecord = rec.Stage2.Sum;

                // Тут будут подсчитыватся показатели
                var shortR = new ShortProgramRecord { Stage2 = rec.Stage2, Year = rec.PlanYear, RealityObject = rec.RealityObject };

                if (sumInYear > 0)
                {
                    // Сначала подсчитываем сумму на КР из Средств собственников
                    shortR.OwnerSumForCr = ((sumRecord / sumInYear) * subsidy.OwnerSumForCr);

                    // проверяем привысилал ли Сумма Стоимсть работ
                    if (shortR.OwnerSumForCr > sumRecord)
                    {
                        // Если сумма получилась больше чем требовалось то ставим Стоимость по ООИ
                        shortR.OwnerSumForCr = sumRecord;
                    }

                    // Теперь подсчитываем сумму на КР из ГК ФСР
                    shortR.BudgetFcr = (((sumRecord - shortR.OwnerSumForCr) / (sumInYear - subsidy.OwnerSumForCr)) * subsidy.BudgetFcr);

                    // Проверяем превысила ли сумма стоисоть работ
                    if (shortR.BudgetFcr + shortR.OwnerSumForCr > sumRecord)
                    {
                        // Если сумма получилась больше чем требовалось то ставим Стоимость по ООИ
                        shortR.BudgetFcr = sumRecord - shortR.OwnerSumForCr;
                    }

                    // Теперь подсчитываем сумму на КР из Иных источников
                    shortR.BudgetOtherSource = (((sumRecord - shortR.OwnerSumForCr - shortR.BudgetFcr)
                          / (sumInYear - subsidy.OwnerSumForCr - subsidy.BudgetFcr)) * subsidy.BudgetOtherSource);

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
        private void CorrectionDpkr(List<DpkrCorrectionProxy> correction, List<SubsidyRecord> subsidyRecords, Dictionary<long, int> publishedYears)
        {
            this.Config = this.Container.GetGkhConfig<OverhaulNsoConfig>();
            var periodStart = this.Config.ProgrammPeriodStart;
            var periodEnd = this.Config.ProgrammPeriodEnd;
            var shortTermPeriod = this.Config.ShortTermProgPeriod;
            var typeCorrection = this.Config.SubsidyConfig.TypeCorrection;

            // Если способ корректировки = "C фиксацией года", то проставляем признак IsCalculated, чтобы данные записи не сдвигались от планового года
            if (typeCorrection == TypeCorrection.WithFixYear)
            {
                correction.Where(x => x.IsChangedYear).ForEach(x => x.IsCalculated = true);
            }

            // Расчитываем бюджеты исходя из того что
            // для годов краткосрочной программы бюджет скалдывается из Забитых В Ручную Бюджетов
            // А для остальных лет складывается из оставшейся суммы на КР деленная пропорционально оставшиммся годам (Но с учетом Фактических денег на КР в Краткосрочной программе)

            // 4. Получаем года Краткосрочной программы
            var shortTermYears = subsidyRecords.Where(x => x.SubsidyYear < periodStart + shortTermPeriod).ToList();

            foreach (var rec in shortTermYears)
            {
                rec.BudgetCr = rec.OwnerSumForCr + rec.BudgetFcr + rec.BudgetMunicipality + rec.BudgetOtherSource + rec.BudgetRegion;
            }

            // 5. Теперь расссматриваем только Года краткосрочной программы и получаем 
            //    какие ООИ будет делатся именно в Краткосрочной программе
            foreach (var item in shortTermYears)
            {
                // Обнуляем Фактическую стоимость
                item.CorrectionFinance = 0;

                // Для текущего года краткосрочной программы считаем корректировку 
                this.WalkThroughDpkr(correction, item, periodEnd,publishedYears);

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

                // Новосиб попросил не брать остатки предыдущего года
                /*
                // Поскольку при переходе на следующий год могли остаться остатки В бюджете 
                // то необходимо получить предудущий год и перекинуть сумму остатка на бюджет следующего года
                SubsidyRecord latterItem;

                dictSubsidy.TryGetValue(item.SubsidyYear - 1, out latterItem);

                if (latterItem != null && latterItem.BalanceAfterCR > 0)
                {
                    item.BudgetCr += latterItem.BalanceAfterCR;
                }

                // Для текущего года краткосрочной программы считаем корректировку 
                this.WalkThroughDpkr(correction, item, periodEnd);

                // После подсчета корректировку получаем Остаток на конец расчетного года
                item.BalanceAfterCR = item.BudgetCr - item.CorrectionFinance;
                */
            }

            // После считаем бюджет для Долгосрочной программы
            // 6 Получаем года Долгосрочной программы
            //   И формироуем Бюджеты долгосрочной программы путем Определения Оставшейся суммы на КР 
            //   с учетом Фактических денег на КР в Краткосрочнйо программе
            var longTermYears = subsidyRecords.Where(x => x.SubsidyYear >= periodStart + shortTermPeriod).ToList();
            
            if (longTermYears.Count > 0)
            {
                // считаем сколько денег осталось распеределить
                var sum = correction.Sum(x => x.Sum) - shortTermYears.Sum(x => x.BudgetCr);
                
                // получаем сумму на КР в долгосрочной программе
                var longSum = (sum / longTermYears.Count());

                // получаем остатки (Наппример копейки которые могли потерятся при округлении)
                // А если не округлять, то вообще ошибок не будет =)
                var mod = sum - longSum * longTermYears.Count();

                // Теперь расссматриваем только Года долгосрочной программы
                foreach (var item in longTermYears)
                {
                    // Обнуляем Фактическую стоимость
                    item.CorrectionFinance = 0;
                    
                    item.BudgetCr = longSum;

                    if (item.SubsidyYear == periodEnd)
                    {
                        // к последнему году прибавляем остатки от округления
                        item.BudgetCr += mod;
                    }
                    
                    if (item.SubsidyYear == periodEnd)
                    {
                        // получаем сумму на КР в последнем году
                        item.BudgetCr = correction.Where(x => x.PlanYear == periodEnd).Sum(x => x.Sum);
                    }
                    
                    // Расчитываем корректировку для Строки Субсидирвоания Долгосрочной программы
                    this.WalkThroughDpkr(correction, item, periodEnd,publishedYears);

                    // После корректировки высчитываем Остаток текущего расчетного года
                    item.BalanceAfterCr = item.BudgetCr - item.CorrectionFinance;

                    // Новосиб попросил непереносить остатки бюджетов на следующий год
                    /* 
                    // Поскольку при переходе на следующий год могли остаться остатки В бюджете 
                    // то необходимо получить предудущий год и перекинуть сумму остатка на бюджет следующего года
                    SubsidyRecord latterItem;

                    dictSubsidy.TryGetValue(item.SubsidyYear - 1, out latterItem);

                    if (latterItem != null && latterItem.BalanceAfterCR > 0)
                    {
                        item.BudgetCr += latterItem.BalanceAfterCR;
                    }

                    // Если год последний то Это означает что сколько требовалось сделать столько и должно быть в бюджете
                    // поскольку в результате округлений и перекидывания остатков произходит такая ситуация 
                    // что в поледнем году после проведения всех работ получается остаок средств
                    // то делаем костыль который в Бюджет последнего года положит именно ту сумму которую требуется на КР в последнем году
                    if (item.SubsidyYear == periodEnd)
                    {
                        // получаем сумму на КР в последнем году
                        item.BudgetCr = correction.Where(x => x.PlanYear == periodEnd).Sum(x => x.Sum);
                    }

                    // Расчитываем корректировку для Строки Субсидирвоания Долгосрочной программы
                    this.WalkThroughDpkr(correction, item, periodEnd);

                    // После корректировки высчитываем Остаток текущего расчетного года
                    item.BalanceAfterCR = item.BudgetCr - item.CorrectionFinance;
                    */
                }
            }
        }

        /// <summary>
        /// Данный метод получает Proxy записи для дальнейшей Работы с Корректировкой
        /// Почему прокси? Потому что данная корректировка будет постоянно менятся и чтобы не пихать все поля в Entity 
        /// будем менять Proxy класс и дальше его дорабатывать
        /// </summary>
        private List<DpkrCorrectionProxy> GetThroughDpkrRows(long versionId)
        {
            // Получаем записи версии 2 этапа
            // уже отсортированные по Индексу
            return VersionStage2Domain.GetAll()
                                   .Where(x => x.Stage3Version.ProgramVersion.Id == versionId)
                                   .Select(
                                       x =>
                                       new DpkrCorrectionProxy
                                       {
                                           Stage2Id = x.Id,
                                           MunicipalityId = x.Stage3Version.RealityObject.Municipality.Id,
                                           RealityObjectId = x.Stage3Version.RealityObject.Id,
                                           CeoId = x.CommonEstateObject.Id,
                                           PlanYear = x.Stage3Version.Year,
                                           Index = x.Stage3Version.IndexNumber,
                                           Sum = x.Sum,
                                           IsChangedYear =  x.Stage3Version.IsChangedYear
                                       })
                                    .OrderBy(x => x.Index)
                                    .ToList();

        }

        /// <summary>
        /// Данный метод отвечает за то что бы для Года в субсидировании посчитать Фактическую стоимость ООИ и выясняем
        ///   либо мы оставляем ООИ в этом году либо мы перекидываем ег она следующий год
        /// Это произходит Так:
        /// 1. В метод приходит субсидия (subsidy) и список ООИ (latterCorrection) который надо обработать
        /// 2. По признакам IsCalculated мы можем понять какие записи еще не обработаны
        ///    Соответсвенно чтобы получить сумму относящуюся к этому году мы берем записи по subsidy.SubsidyYear и !IsCalculated
        /// 3. Далее мы по Очереди берем с наименьшим IndexNumber и впихиваем в год, если деньги Бюджета уже исчерпаны то перекидываем в следующий год
        /// </summary>
        private void WalkThroughDpkr(List<DpkrCorrectionProxy> latterCorrection, SubsidyRecord subsidy,int periodEnd, Dictionary<long, int> publishedYears)
        {
            var deleteWorkWithGreaterYear = this.Config.SubsidyConfig.UseDeleteWorkWithGreaterYear == TypeUsage.Used;

            // Если способ корректировки = "C фиксацией года", то могут быть записи которые не сдвигаются, 
            //у них проставлен признак IsCalculated, поэтому увеличиваем накапливаемую сумму
            subsidy.CorrectionFinance += latterCorrection.Where(x => x.PlanYear == subsidy.SubsidyYear && x.IsCalculated).Sum(x => x.Sum);

            // Каждый раз получаем записи меньше либо равные году субсидирования 
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

            // Далее мы бежим по отсортированному списку и пытаемся ООИ впихнуть в Субсидирование
            // Если бюджет уже превышает, то перекидываем на следующий год. Учитываем что Сумма будет накопительной

            bool stop = false;

            // список Id которые после будут сдвинуты на +1 год (типа сдвиг срока эксплуатации)
            var listMovedCeo = new List<long>();

            foreach (var correctRecord in data)
            {
                    // Проверяем, если мы достигли предельного года в ДПКР 
                    // то фиксируем ООИ как рассмотренную и накапливаем сумму на последнем годе
                    if (subsidy.SubsidyYear >= periodEnd)
                    {
                        if (deleteWorkWithGreaterYear)
                        {
                            var key = string.Format("{0}_{1}", correctRecord.RealityObjectId, correctRecord.CeoId);

                            // поскольку надо подвинуть элемент, то надо по данному ООИ подвинуть все нижестоящие записи
                            if (dictNextCeo.ContainsKey(key))
                            {
                                listMovedCeo.AddRange(dictNextCeo[key]);
                            }
                            correctRecord.PlanYear++;

                        }
                        else
                        {
                            correctRecord.PlanYear = subsidy.SubsidyYear;
                            subsidy.CorrectionFinance += correctRecord.Sum;
                        }

                        correctRecord.IsCalculated = true;
                        continue;
                    }

                if (stop)
                {
                    // Если хотят очередь отрезать
                    // То все оставшиеся работы перекидываем на след. год
                    // Если не хватает денег, то тогда переставляем на след год
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
                else
                {
                    if (subsidy.CorrectionFinance + correctRecord.Sum <= subsidy.BudgetCr)
                    {
                        // Если с учетом накопления мы еще не привысили бюджет Этого года
                        // То считаем что запись будет относится к этому году
                        // и фиксируем накопительную сумму
                        subsidy.CorrectionFinance += correctRecord.Sum;
                        correctRecord.IsCalculated = true;
                    }
                    else
                    {
                            // Если попали сюда значит ООИ перекидываем на следующий год
                            // И он будет рассмотрен ктогда когда настанет черед следующего года
                            if (MoveToNextYear(subsidy, correctRecord, publishedYears))
                            {
                                var key = string.Format("{0}_{1}", correctRecord.RealityObjectId, correctRecord.CeoId);

                                // поскольку надо подвинуть элемент, то надо по данному ООИ подвинуть все нижестоящие записи
                                if (dictNextCeo.ContainsKey(key))
                                {
                                    listMovedCeo.AddRange(dictNextCeo[key]);
                                }
                            }
                        
                        // и отрезаем очередь
                        stop = true;
                    }
                }
            }

            if (this.Config.SubsidyConfig.UseLifetime == TypeUsage.Used && listMovedCeo.Any())
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
        /// В каждом из расчетов требуется сначала сохранить измененные записи Грида Субсидирования, а затем произвести какоето вычисление
        /// По этому этот метод сохраняет записи Субсидирования, которые были изменены
        /// </summary>
        private void SaveSubsidyRecords(SubsidyRecord[] data)
        {
            if (data != null && data.Any())
            {
                using (var tx = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        // Сохраняем все записи котоыре были изменены 
                        foreach (var record in data)
                            SubsidyRecordDomain.Update(record);

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

        private bool MoveToNextYear(SubsidyRecord subsidy, DpkrCorrectionProxy correctRecord, Dictionary<long, int> publishedYears)
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
                // Если превышает фиксированный год то необходимо работу оставить в этом году
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
        /// Идентификатор ООИ
        /// </summary>
        public virtual long CeoId { get; set; }

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
        /// Стоимость работ
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Идентификатор 2 этапа
        /// </summary>
        public virtual long Stage2Id { get; set; }

        /// <summary>
        /// Если запись прошла расчет то будет true , иначе false
        /// получается что если falseродолжаем обрабатывать запись
        /// </summary>
        public virtual bool IsCalculated { get; set;  }

        /// <summary>
        /// Изменялся ли плановый год
        /// </summary>
        public virtual bool IsChangedYear { get; set; }
    }
}