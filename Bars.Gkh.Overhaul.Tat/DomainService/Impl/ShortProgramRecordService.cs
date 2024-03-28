namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.DomainService.BaseParams;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Tat.ConfigSections;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Сервис работы с КПКР
    /// </summary>
    public class ShortProgramRecordService : IShortProgramRecordService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="ShortProgramRecord"/>
        /// </summary>
        public IDomainService<ShortProgramRecord> DomainService { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="State"/>
        /// </summary>
        public IDomainService<State> StateDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="VersionRecord"/>
        /// </summary>
        public IDomainService<VersionRecord> Stage3Domain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="VersionRecordStage2"/>
        /// </summary>
        public IDomainService<VersionRecordStage2> Stage2Domain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="VersionRecordStage1"/>
        /// </summary>
        public IDomainService<VersionRecordStage1> Stage1Domain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="StructuralElementWork"/>
        /// </summary>
        public IDomainService<StructuralElementWork> StrElWorksDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="ProgramVersion"/>
        /// </summary>
        public IDomainService<ProgramVersion> VersionDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="ShortProgramRecord"/>
        /// </summary>
        public IDomainService<ShortProgramRecord> ShortRecordDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="ShortProgramRealityObject"/>
        /// </summary>
        public IDomainService<ShortProgramRealityObject> ShortObjectDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="DpkrCorrectionStage2"/>
        /// </summary>
        public IDomainService<DpkrCorrectionStage2> CorrectionDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="Work"/>
        /// </summary>
        public IDomainService<Work> WorksDomain { get; set; }

        /// <inheritdoc />
        public IDataResult CreateShortProgram(BaseParams baseParams)
        {
            var municipalityId = baseParams.Params.GetAs<long>("municipality_id");
            var year = baseParams.Params.GetAs<int>("year");

            if (municipalityId == 0)
            {
                return new BaseDataResult(false, "Не выбрано МО");
            }

            if (year <= 0)
            {
                return new BaseDataResult(false, "Не выбран год");
            }

            var version = this.VersionDomain.GetAll().FirstOrDefault(x => x.Municipality.Id == municipalityId && x.IsMain);

            if (version == null)
            {
                return new BaseDataResult(false, "Не указана основная версия");
            }

            if (version.State.FinalState)
            {
                return new BaseDataResult(false, "Нельзя менять версию с конечным статусом");
            }

            var services = this.WorksDomain.GetAll()
                .Where(x => x.TypeWork == TypeWork.Service && (x.Code == "1020" || x.Code == "1019" || x.Code == "1018"))
                .AsEnumerable()
                .GroupBy(x => x.Code)
                .ToDictionary(x => x.Key, y => y.First());

            if (!services.ContainsKey(this.serviceTehnadzor))
            {
                return new BaseDataResult(false, "В справочнике отсутствует услуга 'Технадзор'");
            }

            if (!services.ContainsKey(this.servicePsdExpertiza))
            {
                return new BaseDataResult(false, "В справочнике отсутствует услуга 'ПСД экспертиза'");
            }

            if (!services.ContainsKey(this.servicePsdRazrabotka))
            {
                return new BaseDataResult(false, "В справочнике отсутствует услуга 'ПСД разработка'");
            }

            // Проверяем Существует ли нужный статус и если нет то создаем новый
            var firstState = this.StateDomain.GetAll().FirstOrDefault(x => x.TypeId == "ovrhl_short_prog_object" && x.StartState);

            if (firstState == null)
            {
                firstState = new State
                {
                    Name = "Черновик",
                    Code = "Черновик",
                    StartState = true,
                    TypeId = "ovrhl_short_prog_object"
                };

                this.StateDomain.Save(firstState);
            }

            // Проверяем
            // Если для версии уже существовала краткосрочка то тогда запускаем один алгоритм
            // Иначе запускаем другой алгоритм
            if (this.ShortObjectDomain.GetAll().Any(x => x.ProgramVersion.Id == version.Id && x.Year == year))
            {
                return this.UpdateShortProgram(version, services, firstState, year);
            }

            return this.CreateShortProgram(version, services, firstState, year);
        }

        /// <inheritdoc />
        public IDataResult ListWork(BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var works = this.Container.Resolve<IDomainService<Work>>().GetAll()
                .Where(x => x.WithinShortProgram)
                .Select(x => new { x.Id, x.Code, x.Name })
                .Filter(loadParams, this.Container);

            return new ListDataResult(works.Order(loadParams).Paging(loadParams).ToList(), works.Count());
        }

        /// <inheritdoc />
        public IDataResult AddWorks(BaseParams baseParams)
        {
            var objectIds = baseParams.Params.GetAs<long[]>("objectIds");
            var roId = baseParams.Params.GetAs<long>("roId");

            using (var tr = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var existRec = this.DomainService.GetAll()
                        .Where(x => x.ShortProgramObject.Id == roId)
                        .Select(x => x.Work.Id)
                        .Distinct()
                        .ToDictionary(x => x);

                    foreach (var id in objectIds)
                    {
                        if (existRec.ContainsKey(id))
                        {
                            continue;
                        }

                        var newRec = new ShortProgramRecord
                        {
                            Cost = 0,
                            Volume = 0,
                            ShortProgramObject = new ShortProgramRealityObject { Id = roId },
                            Work = new Work { Id = id },
                            TypeDpkrRecord = TypeDpkrRecord.UserRecord
                        };

                        this.DomainService.Save(newRec);
                    }

                    tr.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    tr.Rollback();
                    return new BaseDataResult(false, e.Message);
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }
        }

        /// <inheritdoc />
        public IDataResult ActualizeVersion(BaseParams baseParams)
        {
            var municipalityId = baseParams.Params.GetAs<long>("municipality_id");
            var year = baseParams.Params.GetAs<int>("year");

            var config = this.Container.GetGkhConfig<OverhaulTatConfig>();
            var startYear = config.ActualizePeriodStart;
            var endYear = config.ActualizePeriodEnd;

            if (municipalityId == 0)
            {
                return new BaseDataResult(false, "Не выбрано МО");
            }

            if (startYear == 0 || endYear == 0)
            {
                return new BaseDataResult(false, "Не указан период актуализации");
            }

            var version = this.VersionDomain.GetAll().FirstOrDefault(x => x.Municipality.Id == municipalityId && x.IsMain);

            if (version == null)
            {
                return new BaseDataResult(false, "Не указана основная версия");
            }

            if (version.State.FinalState)
            {
                return new BaseDataResult(false, "Нельзя менять версию с конечным статусом");
            }

            if (this.ShortObjectDomain.GetAll()
                .Where(x => x.ProgramVersion.Id == version.Id)
                .WhereIf(year > 0, x => x.Year == year)
                .WhereIf(year <= 0, x => x.Year >= startYear && x.Year <= endYear)
                .Any(x => !x.State.FinalState))
            {
                return new BaseDataResult(false, "Не все дома краткосрочной программы имеют конечный статус");
            }

            var checkCorrection = this.CorrectionDomain.GetAll()
                .Where(x => x.Stage2.Stage3Version.ProgramVersion.Id == version.Id)
                .WhereIf(year > 0, x => x.PlanYear == year)
                .WhereIf(year <= 0, x => x.PlanYear >= startYear && x.PlanYear <= endYear)
                .Any();

            if (checkCorrection)
            {
                // Если есть записи по краткосрочному периоду в результате корректировке 
                // но в краткосрочке нет ниодной записи, то нельзя актуализировать
                if (!this.ShortRecordDomain.GetAll()
                    .Where(x => x.ShortProgramObject.ProgramVersion.Id == version.Id && x.Work.TypeWork == TypeWork.Work)
                    .WhereIf(year > 0, x => x.ShortProgramObject.Year == year)
                    .WhereIf(year <= 0, x => x.ShortProgramObject.Year >= startYear && x.ShortProgramObject.Year <= endYear)
                    .Any())
                {
                    if (year > 0)
                    {
                        return new BaseDataResult(false, "Необходимо сформировать краткосрочную программу за " + year + " год");
                    }
                    else
                    {
                        return new BaseDataResult(false,
                            "Необходимо сформировать краткосрочную программу за период актуализации с " + startYear + " по " + endYear + " гг");
                    }
                }
            }

            // Тут будет версии на сохранение
            var stage1ToSave = new List<VersionRecordStage1>();
            var stage2ToSave = new List<VersionRecordStage2>();
            var stage3ToSave = new List<VersionRecord>();

            // Тут будут записи на удаление Весрии
            // Они могут удалятся только в том случае если пользователь добавли ручные записи затем актуализирвоал и они попали
            // в версию затем он удалил эти записи из краткосрочки и опят ьнажал актуализироват ьсоответсвенно записи удаленные должны удалится из версии
            var stage1ToDelete = new List<long>();
            var stage2ToDelete = new List<long>();
            var stage3ToDelete = new List<long>();
            var corrToDelete = new List<long>();

            // В этом массиве будут записи краткосрочки на сохранение
            var listToSave = new List<ShortProgramRecord>();

            var shortRecords = new List<ShortRecordProxy>();

            // Поулчаем объекты краткосрочки за выбранный период
            var shortObjectsToSave = this.ShortObjectDomain.GetAll().Where(x => x.ProgramVersion.Id == version.Id)
                .WhereIf(year > 0, x => x.Year == year)
                .WhereIf(year <= 0, x => x.Year >= startYear && x.Year <= endYear)
                .ToList();

            if (shortObjectsToSave.Count > 0)
            {
                // 1 Сначала Запускаем метод который актуализирует объекты краткосрочки
                // Это означает что необходимо получить итоговые стоимости всех услуг и общие стоимости
                this.ActualizationShortObjects(version, shortObjectsToSave, shortRecords);

                // 2 Теперь необходимо Актуализировать полученные данные с Версионными
                // Тоест ьпоскольку зменились суммы их необходимо втом же виде перенести и в версию
                this.ActualizationToVersion(version, shortRecords, stage1ToSave, stage2ToSave, stage3ToSave);
            }

            // Все записи краткосрочки
            var records = this.ShortRecordDomain.GetAll()
                .Where(x => x.ShortProgramObject.ProgramVersion.Id == version.Id)
                .Where(x => x.Work.TypeWork == TypeWork.Work)
                .AsEnumerable()
                .ToDictionary(x => x.Id);

            var st1ForDelete = this.Stage1Domain.GetAll()
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion == version)
                .Where(x => x.Stage2Version.TypeDpkrRecord == TypeDpkrRecord.UserRecord)
                .Where(x =>
                    !this.ShortRecordDomain.GetAll()
                        .Where(y => y.ShortProgramObject.ProgramVersion == version)
                        .Any(y => y.Stage1.Stage2Version == x.Stage2Version))
                .Select(x => new
                {
                    x.Id,
                    st2Id = x.Stage2Version.Id,
                    st3Id = x.Stage2Version.Stage3Version.Id
                })
                .ToList();

            var correctionForDelete = this.CorrectionDomain.GetAll()
                .Where(x => x.Stage2.Stage3Version.ProgramVersion == version)
                .Where(x => x.Stage2.TypeDpkrRecord == TypeDpkrRecord.UserRecord)
                .Where(x =>
                    !this.ShortRecordDomain.GetAll()
                        .Where(y => y.ShortProgramObject.ProgramVersion == version)
                        .Any(y => y.Stage1.Stage2Version == x.Stage2))
                .Select(x => new { x.Id, st2Id = x.Stage2.Id })
                .AsEnumerable()
                .GroupBy(x => x.st2Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Id).First());

            foreach (var rec in shortRecords)
            {
                if (!records.ContainsKey(rec.Id))
                {
                    continue;
                }

                var recUpdate = records[rec.Id];

                recUpdate.ServiceCost = rec.ServiceCost;
                recUpdate.TotalCost = rec.TotalCost;

                if (rec.Stage1 != null)
                {
                    recUpdate.Stage1 = rec.Stage1;
                }

                listToSave.Add(recUpdate);
            }

            foreach (var delete in st1ForDelete)
            {
                if (!stage1ToDelete.Contains(delete.Id))
                {
                    stage1ToDelete.Add(delete.Id);
                }

                if (!stage2ToDelete.Contains(delete.st2Id))
                {
                    stage2ToDelete.Add(delete.st2Id);

                    // Поскольку планируется удалить запись 2 этапа то возможно на нее уже была расчитана корректировка
                    // следовательно удаляем и корректировку
                    if (correctionForDelete.ContainsKey(delete.st2Id) && !corrToDelete.Contains(correctionForDelete[delete.st2Id]))
                    {
                        corrToDelete.Add(correctionForDelete[delete.st2Id]);
                    }
                }

                if (!stage3ToDelete.Contains(delete.st3Id))
                {
                    stage3ToDelete.Add(delete.st3Id);
                }
            }

            // Получаем список 3 этапа именно для тех записей где надо обновить Скорректированный год
            var st3ForUpdate = this.GetStage3ForUpdateCorrectionYear(version);

            // Поскольку унас на сохранение должен пойти тольк о2 Список № этапа то объекдиняем полученные данные
            var dictSt3Updates = stage3ToSave.Where(x => x.Id > 0).ToDictionary(x => x.Id);
            foreach (var st3 in st3ForUpdate)
            {
                if (dictSt3Updates.ContainsKey(st3.Id))
                {
                    dictSt3Updates[st3.Id].CorrectYear = st3.CorrectYear;
                }
                else
                {
                    // иначе добавляем запись на сохранение
                    stage3ToSave.Add(st3);
                }
            }

            var session = this.Container.Resolve<ISessionProvider>().OpenStatelessSession();
            var unProxy = this.Container.Resolve<IUnProxy>();

            try
            {
                using (var transaction = session.BeginTransaction(IsolationLevel.Serializable))
                {
                    // Теперь отправляем полученные данные на сохранение
                    try
                    {
                        // Поскольку записи версии могли быть созданы то либ осоздаем либо обновляем
                        stage3ToSave.ForEach(x =>
                        {
                            if (x.Id > 0)
                                session.Update(unProxy.GetUnProxyObject(x));
                            else
                                session.Insert(unProxy.GetUnProxyObject(x));
                        });
                        stage2ToSave.ForEach(x =>
                        {
                            if (x.Id > 0)
                                session.Update(unProxy.GetUnProxyObject(x));
                            else
                                session.Insert(unProxy.GetUnProxyObject(x));
                        });
                        stage1ToSave.ForEach(x =>
                        {
                            if (x.Id > 0)
                                session.Update(unProxy.GetUnProxyObject(x));
                            else
                                session.Insert(unProxy.GetUnProxyObject(x));
                        });

                        // Записи краткосрочки только Обновляем они тут не создавались
                        shortObjectsToSave.ForEach(x => session.Update(unProxy.GetUnProxyObject(x)));
                        listToSave.ForEach(x => session.Update(unProxy.GetUnProxyObject(x)));

                        // если есть записи на удаление то удаляем
                        foreach (var id in stage1ToDelete)
                        {
                            session.CreateSQLQuery(@"DELETE FROM OVRHL_TYPE_WORK_CR_ST1 where ST1_ID = :id")
                                .SetParameter("id", id)
                                .ExecuteUpdate();
                        }

                        foreach (var id in stage1ToDelete)
                        {
                            session.CreateSQLQuery(@"DELETE FROM ovrhl_stage1_version where id = :id")
                                .SetParameter("id", id)
                                .ExecuteUpdate();
                        }

                        foreach (var id in corrToDelete)
                        {
                            session.CreateSQLQuery(@"DELETE FROM ovrhl_dpkr_correct_st2 where id = :id")
                                .SetParameter("id", id)
                                .ExecuteUpdate();
                        }

                        foreach (var id in stage2ToDelete)
                        {
                            session.CreateSQLQuery(@"DELETE FROM ovrhl_stage2_version where id = :id")
                                .SetParameter("id", id)
                                .ExecuteUpdate();
                        }

                        foreach (var id in stage3ToDelete)
                        {
                            session.CreateSQLQuery(@"DELETE FROM ovrhl_version_rec where id = :id")
                                .SetParameter("id", id)
                                .ExecuteUpdate();
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            finally
            {
                this.Container.Release(unProxy);
                this.Container.Resolve<ISessionProvider>().CloseCurrentSession();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            return new BaseDataResult(true, "Актуализация выполнена успешно");
        }

        /// <inheritdoc />
        public IDataResult ListForMassStateChange(BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var muId = baseParams.Params.GetAs<long>("muId");
            var stateId = baseParams.Params.GetAs<long>("stateId");
            var year = baseParams.Params.GetAs<int>("year");

            var works = this.ShortObjectDomain.GetAll()
                .Where(x => x.State.Id == stateId && x.RealityObject.Municipality.Id == muId && x.ProgramVersion.IsMain && x.Year == year)
                .Select(x => new { x.Id, State = x.State.Name, x.RealityObject.Address })
                .OrderBy(x => x.Address)
                .Filter(loadParams, this.Container);

            return new ListDataResult(works.Order(loadParams).Paging(loadParams).ToList(), works.Count());
        }

        /// <inheritdoc />
        public IDataResult MassStateChange(BaseParams baseParams)
        {
            var objectIds = baseParams.Params.GetAs<long[]>("objectIds");
            var nextStateId = baseParams.Params.GetAs<long>("nextStateId");
            var newState = this.StateDomain.Load(nextStateId);

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var objectId in objectIds)
                    {
                        var shortProgRealObj = this.ShortObjectDomain.Load(objectId);
                        shortProgRealObj.State = newState;
                        this.ShortObjectDomain.Update(shortProgRealObj);
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult GetYears(BaseParams baseParams)
        {
            var currentYear = DateTime.Now.Year;

            var years = this.ShortObjectDomain.GetAll()
                .Select(x => x.Year)
                .Distinct()
                .ToList()
                .Select(x => new { Id = x, Name = x, Default = x == currentYear })
                .ToList();

            return new ListDataResult(years, years.Count);
        }

        /// <inheritdoc />
        public void CalculationCosts(Dictionary<string, decimal> dictServices, List<ShortRecordProxy> works)
        {
            // получаем только работы котоыре расчитанные (то есть не добавлены в ручную)
            // Данное условие больше ненужно ,но если захоятт обратно возобновить то это надо так делать
            // var dataNoUsers = works.Where(x => x.Cost > 0 && x.TypeDpkrRecord != TypeDpkrRecord.UserRecord).ToList();
            var worksWithCosts = works.Where(x => x.Cost > 0).ToList();

            foreach (var kvp in dictServices)
            {
                if (kvp.Value <= 0)
                {
                    continue;
                }

                // Поскольку Сказали чт овсе услуги должны расчитыватся на все работы тогда делаем на все работы любую услугу
                this.Calculation(kvp.Value, worksWithCosts);
            }
        }

        /// <inheritdoc />
        private IDataResult CreateShortProgram(ProgramVersion version, Dictionary<string, Work> services, State firstState, int year)
        {
            // Получаем Результаты корректировки
            var currentCorrection = this.CorrectionDomain.GetAll()
                .Where(x => x.Stage2.Stage3Version.ProgramVersion.Id == version.Id)
                .Where(x => x.PlanYear == year)
                .AsEnumerable()
                .GroupBy(x => x.Stage2.Id)
                .ToDictionary(x => x.Key, y => y.FirstOrDefault());

            // Получаем работы по конструктивным элементам
            var jobs = this.StrElWorksDomain.GetAll()
                .Select(x => new WorkProxy
                {
                    Id = x.Id,
                    WorkId = x.Job.Work.Id,
                    strElId = x.StructuralElement.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.strElId)
                .ToDictionary(x => x.Key, y => y.First());

            var dictStage1 = this.Stage1Domain.GetAll()
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == version.Id)

                // .Where(x => x.StructuralElement != null) Блять ненадо эту хуйню здесь вставлять если незнаете как работает лучше несутей суда свои руки
                // В первом этапе могут быть записи добавленные вручную которых нет в Жилых домах, соответсвенно они просто живут в ДПКР и больше нигде для этого завели поле StrElement которое полюбому должно быть
                .Where(x => x.StrElement != null)
                .Select(x => new Stage1Proxy
                {
                    Id = x.Id,
                    st2Id = x.Stage2Version.Id,
                    Volume = x.Volume,
                    strElId = x.StrElement.Id,
                    RoId = x.RealityObject.Id,
                    Sum = x.Sum,
                    SumService = x.SumService
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => y.GroupBy(x => x.st2Id).ToDictionary(x => x.Key, x => x.ToList()));

            var dictCorrections = this.CorrectionDomain.GetAll()
                .Where(x => x.Stage2.Stage3Version.ProgramVersion.Id == version.Id)
                .Where(x => x.PlanYear == year)
                .Select(x => new
                {
                    st2Id = x.Stage2.Id,
                    RoId = x.RealityObject.Id,
                    x.Stage2.TypeDpkrRecord
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => y.Select(x => new { x.st2Id, x.TypeDpkrRecord }).Distinct().ToList());

            // Получаем список существующих объектов
            // Если в этом списке уже есть дом то мы его просто пролетаем и ничего с ним не делаем
            var currObject = this.ShortObjectDomain.GetAll()
                .Where(x => x.ProgramVersion.Id == version.Id && x.Year == year)
                .AsEnumerable()
                .GroupBy(x => x.RealityObject.Id)
                .ToDictionary(x => x.Key, y => y.FirstOrDefault());

            var listRoToSave = new List<ShortProgramRealityObject>();
            var listRecordsToSave = new List<ShortProgramRecord>();
            var listCorrectionToSave = new List<DpkrCorrectionStage2>();

            // проходим по группе домов и создаем на них записи Краткосрочки
            foreach (var corr in dictCorrections)
            {
                // Если дом уже есть в списке существующих то мы по нему ничего неделам
                // поскольку пользователи уже могли вбить нужные им суммы в работы и мы неможем взять и грохнуть дома а потом создат ьновые
                if (currObject.ContainsKey(corr.Key))
                {
                    continue;
                }

                if (!dictStage1.ContainsKey(corr.Key))
                {
                    continue;
                }

                var st1dict = dictStage1[corr.Key];

                // создаем запись объекта 
                var ro = new ShortProgramRealityObject
                {
                    Year = year,
                    State = new State { Id = firstState.Id },
                    RealityObject = new RealityObject { Id = corr.Key },
                    ProgramVersion = new ProgramVersion { Id = version.Id },
                    TypeActuality = TypeActuality.Actual // выставляем тип = запись актуализирована с версией
                };

                listRoToSave.Add(ro);

                // Создаем услугу Технадзор
                var serv1020 = new ShortProgramRecord
                {
                    ShortProgramObject = ro,
                    Work = new Work { Id = services[this.serviceTehnadzor].Id },
                    Volume = 0,
                    Cost = 0,
                    TypeDpkrRecord = TypeDpkrRecord.UserRecord // выставляем тип = запись добавлена в ручную
                };

                listRecordsToSave.Add(serv1020);

                // Создаем услугу ПСД экспертиза
                var serv1019 = new ShortProgramRecord
                {
                    ShortProgramObject = ro,
                    Work = new Work { Id = services[this.servicePsdExpertiza].Id },
                    Volume = 0,
                    Cost = 0,
                    TypeDpkrRecord = TypeDpkrRecord.UserRecord // выставляем тип = запись добавлена в ручную
                };

                listRecordsToSave.Add(serv1019);

                // Создаем услугу ПСД разработка
                var serv1018 = new ShortProgramRecord
                {
                    ShortProgramObject = ro,
                    Work = new Work { Id = services[this.servicePsdRazrabotka].Id },
                    Volume = 0,
                    Cost = 0,
                    TypeDpkrRecord = TypeDpkrRecord.UserRecord // выставляем тип = запись добавлена в ручную
                };

                listRecordsToSave.Add(serv1018);

                // теперь проходим по списку St2 и формируем работы
                foreach (var corrValue in corr.Value)
                {
                    if (!st1dict.ContainsKey(corrValue.st2Id))
                        continue;

                    var st1 = st1dict[corrValue.st2Id];

                    foreach (var item in st1)
                    {
                        if (!jobs.ContainsKey(item.strElId))
                            continue;

                        var job = jobs[item.strElId];

                        var record = new ShortProgramRecord
                        {
                            ShortProgramObject = ro,
                            Stage1 = new VersionRecordStage1 { Id = item.Id },
                            Work = new Work { Id = job.WorkId },
                            Volume = item.Volume,
                            Cost = item.Sum,
                            ServiceCost = item.SumService,
                            TotalCost = item.Sum + item.SumService,
                            TypeDpkrRecord = corrValue.TypeDpkrRecord // Внимание тут м выставляем именно тот тип который был в Stage2
                        };

                        // После чего расчитываем процент услуг от стоимости работ
                        this.DealPercents(serv1020, serv1019, serv1018, record);

                        listRecordsToSave.Add(record);

                        DpkrCorrectionStage2 correction;

                        currentCorrection.TryGetValue(corrValue.st2Id, out correction);

                        if (correction != null)
                        {
                            correction.TypeResult = TypeResultCorrectionDpkr.InShortTerm;
                            listCorrectionToSave.Add(correction);
                        }
                    }
                }
            }

            var session = this.Container.Resolve<ISessionProvider>().OpenStatelessSession();

            try
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        //Удаление старых версий скорректированных объектов ООИ
                        if (listCorrectionToSave.Any())
                        {
                            var correctionSt3VersionIds = string.Join(", ",
                                listCorrectionToSave
                                    .Select(x => x.Stage2.Stage3Version.Id)
                                    .Distinct());

                            session.CreateSQLQuery(
                                string.Format(@"delete from ovrhl_dpkr_correct_st2 
	                                            where st2_version_id in (select st2.id 
	                                                from ovrhl_stage2_version st2
	                                                where st2.st3_version_id in ({0}));",
                                correctionSt3VersionIds))
                                .ExecuteUpdate();
                        }

                        listRoToSave.ForEach(x => session.Insert(x));

                        listRecordsToSave.ForEach(x => session.Insert(x));

                        listCorrectionToSave.ForEach(x => session.Insert(x));

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            finally
            {
                this.Container.Resolve<ISessionProvider>().CloseCurrentSession();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            return new BaseDataResult(true, "Краткосрочная программа сформирована успешно<br>");
        }

        /// <inheritdoc />
        private IDataResult UpdateShortProgram(ProgramVersion version, Dictionary<string, Work> services, State firstState, int year)
        {
            // получаем дома из существующей краткосрочной программы на котоыре могла повлиять корректировка 
            // Тоесть в случае если либ оудаляется какойто ООИ из краткосрочки или добавляется
            // Либо если дом неактуальный

            // получаем те дома в которые добавляется работа
            var changedObjectsIds = this.ShortObjectDomain.GetAll()
                .Where(x => x.ProgramVersion.Id == version.Id && x.Year == year)
                .Where(
                    x =>
                        this.CorrectionDomain.GetAll()
                            .Where(y => y.PlanYear == year)
                            .Where(y => y.Stage2.Stage3Version.ProgramVersion.Id == version.Id)
                            .Where(y => y.RealityObject.Id == x.RealityObject.Id)
                            .Any(y => y.TypeResult == TypeResultCorrectionDpkr.AddInShortTerm)
                        ||
                        x.TypeActuality == TypeActuality.NoActual)
                .Select(x => new { x.Id, x.RealityObject.Address })
                .ToList();

            // поулчаем те дома из которых удаляется запись
            this.ShortRecordDomain.GetAll()
                .Where(x => x.ShortProgramObject.ProgramVersion.Id == version.Id && x.ShortProgramObject.Year == year)
                .Where(x => x.Stage1 != null)
                .Where(
                    x => this.CorrectionDomain.GetAll()
                        .Where(y => y.Stage2.Id == x.Stage1.Stage2Version.Id)
                        .Any(y => y.TypeResult == TypeResultCorrectionDpkr.RemoveFromShortTerm))
                .Select(x => new { x.ShortProgramObject.Id, x.ShortProgramObject.RealityObject.Address })
                .ToList()
                .ForEach(x =>
                {
                    if (!changedObjectsIds.Any(y => y.Id == x.Id))
                    {
                        changedObjectsIds.Add(x);
                    }
                });

            var listIds = changedObjectsIds.Select(x => x.Id).Distinct().ToList();

            var listNames = changedObjectsIds.Select(x => x.Address).Distinct().ToList();

            // Получаем корректировки которые уже при удалении записей домов бдут иметь неверные типы РезультатаПроверки
            // сначала получаем записи добавляемые в краткосрочку
            var corrections =
                this.CorrectionDomain.GetAll()
                    .Where(y => y.Stage2.Stage3Version.ProgramVersion.Id == version.Id && year == year)
                    .Where(y => y.TypeResult == TypeResultCorrectionDpkr.AddInShortTerm)
                    .ToList();

            // теперь поулчаем записи удаляемые из краткосрочки текущего года
            corrections.AddRange(this.CorrectionDomain.GetAll()
                .Where(y => this.ShortRecordDomain.GetAll().Any(x => x.Stage1.Stage2Version.Id == y.Stage2.Id && x.ShortProgramObject.Year == year))
                .Where(y => y.TypeResult == TypeResultCorrectionDpkr.RemoveFromShortTerm)
                .ToList());

            // Им ыставляем новые признак
            foreach (var item in corrections)
            {
                item.TypeResult = TypeResultCorrectionDpkr.New;
            }

            // теперь полностью удаляем эти объекты на которые могла повлият ькорректировка 
            var session = this.Container.Resolve<ISessionProvider>().OpenStatelessSession();

            if (listIds.Any())
            {
                var unProxy = this.Container.Resolve<IUnProxy>();

                try
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        try
                        {
                            foreach (var id in listIds)
                            {
                                session.CreateSQLQuery(string.Format("delete from OVRHL_SHORT_PROG_DEFECT where SHORT_OBJ_ID = {0}", id)).ExecuteUpdate();

                                session.CreateSQLQuery(string.Format("delete from OVRHL_SHORT_PROG_PROTOCOL where SHORT_OBJ_ID = {0}", id)).ExecuteUpdate();

                                session.CreateSQLQuery(string.Format("delete from ovrhl_short_prog_rec where short_prog_obj_id = {0}", id)).ExecuteUpdate();

                                session.CreateSQLQuery(string.Format("delete from ovrhl_short_prog_obj where id = {0}", id)).ExecuteUpdate();
                            }

                            corrections.ForEach(x => session.Update(unProxy.GetUnProxyObject(x)));

                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                finally
                {
                    this.Container.Release(unProxy);
                    this.Container.Resolve<ISessionProvider>().CloseCurrentSession();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }

            // теперь создаем дома только для тех, которые не создано но должны создатся
            this.CreateShortProgram(version, services, firstState, year);

            // После выполнения формируем стору результата
            var message = "Краткосрочная программа сформирована успешно";

            if (listNames.Any())
            {
                message +=
                    "<br><br>Состав работ в следующих домах был изменен, либо <br>в результате корректировки следующие дома были удалены из краткосрочной программы<br><br>";
                foreach (var address in listNames)
                {
                    message += address + "<br>";
                }
            }

            return new BaseDataResult(true, message);
        }

        /// <inheritdoc />
        private void ActualizationShortObjects(ProgramVersion version, List<ShortProgramRealityObject> shortObjects, List<ShortRecordProxy> shortRecords)
        {
            // тут берем услуги по данному дому По дому 
            var dictServices = this.ShortRecordDomain.GetAll()
                .Where(x => x.ShortProgramObject.ProgramVersion.Id == version.Id)
                .Where(x => x.Work.TypeWork == TypeWork.Service)
                .Select(x => new { x.Work.Code, x.Cost, ObjId = x.ShortProgramObject.Id })
                .AsEnumerable()
                .GroupBy(x => x.ObjId)
                .ToDictionary(x => x.Key, y => y.GroupBy(x => x.Code).ToDictionary(x => x.Key, z => z.Select(x => x.Cost).First()));

            var dictShortRecords = this.ShortRecordDomain.GetAll()
                .Where(x => x.ShortProgramObject.ProgramVersion.Id == version.Id)
                .Where(x => x.Work.TypeWork == TypeWork.Work)
                .Select(x => new
                {
                    x.Id,
                    x.Cost,
                    ShortObjectId = x.ShortProgramObject.Id,
                    shortYear = x.ShortProgramObject.Year,
                    stage1Id = x.Stage1 != null ? x.Stage1.Id : 0L,
                    x.Volume,
                    WorkId = x.Work.Id,
                    x.TypeDpkrRecord
                })
                .AsEnumerable()
                .GroupBy(x => x.ShortObjectId)
                .ToDictionary(
                    x => x.Key,
                    y => y.Select(
                            x => new ShortRecordProxy
                            {
                                // Такой замут делаю просто чтобы обнулить несколько полей, нехочется делать отдельные обработки перед выполнением вычисления
                                Id = x.Id,
                                Cost = x.Cost,
                                ShortProgramObjectId = x.ShortObjectId,
                                Stage1Id = x.stage1Id,
                                ShortYearStart = x.shortYear,
                                TotalCost = x.Cost,
                                ServiceCost = 0M,
                                Volume = x.Volume,
                                WorkId = x.WorkId,
                                TypeDpkrRecord = x.TypeDpkrRecord
                            })
                        .ToList());

            foreach (var item in shortObjects)
            {
                // сразу же выставляем признак что Дом прошел через Актуализацию
                item.TypeActuality = TypeActuality.Actual;

                // Далее получаем его записи в краткосрочке
                if (!dictServices.ContainsKey(item.Id) || !dictShortRecords.ContainsKey(item.Id))
                {
                    continue;
                }

                var services = dictServices[item.Id];
                var records = dictShortRecords[item.Id];

                // Запускае метод расчета стоимостей услуг и общих стоимостей на работу
                this.CalculationCosts(services, records);

                // поскольку мы расчитали список, то его и добавляем на сохранение
                shortRecords.AddRange(records);
            }
        }

        /// <summary>
        /// Метод предназначен для актуализации полученных стоимостей в записи Версий
        /// соответсвенно необходимо обновить Stage1, Stage2, Stage3
        /// при этом поскольку в Краткосроке могли быть какие то ручные добавленные записи то необходимо для них Создавать новые записи
        /// в Stage1, Stage2, Stage3
        /// </summary>
        private void ActualizationToVersion(
            ProgramVersion version,
            List<ShortRecordProxy> shortRecords,
            List<VersionRecordStage1> stage1,
            List<VersionRecordStage2> stage2,
            List<VersionRecord> stage3)
        {
            // Превращаем массив records в словарик для быстрого доступа к элементам
            var infoDict = this.ShortRecordDomain.GetAll()
                .Where(x => x.ShortProgramObject.ProgramVersion.Id == version.Id)
                .Where(x => x.Work.TypeWork == TypeWork.Work)
                .Select(x => new { x.Id, WorkId = x.Work.Id, RoId = x.ShortProgramObject.RealityObject.Id })
                .AsEnumerable()
                .ToDictionary(x => x.Id);

            // получаем по WorkId Сруктурный Элемент и ООИ в котором он находится 
            // страшная жесть но сказали что на одну работу должен быть 1 КЭ и 1 ООИ
            var infoStructElementsDict = this.StrElWorksDomain.GetAll()
                .Select(x => new
                {
                    WorkId = x.Job.Work.Id,
                    StrElId = x.StructuralElement.Id,
                    CeoId = x.StructuralElement.Group.CommonEstateObject.Id,
                    CeoName = x.StructuralElement.Group.CommonEstateObject.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.WorkId)
                .ToDictionary(x => x.Key, y => y.First());

            // Записи 1 этапа
            var dictStage1 = this.Stage1Domain.GetAll()
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == version.Id)
                .AsEnumerable()
                .ToDictionary(x => x.Id);

            // Записи 2 этапа
            var dictStage2 = this.Stage2Domain.GetAll()
                .Where(x => x.Stage3Version.ProgramVersion.Id == version.Id)
                .AsEnumerable()
                .ToDictionary(x => x.Id);

            // Записи 3 этапа
            var dictStage3 = this.Stage3Domain.GetAll().Where(x => x.ProgramVersion.Id == version.Id).AsEnumerable()
                .ToDictionary(x => x.Id);

            // формируем словарь Минимальных Индексов по Дому котоыре участвуют в краткосрочной программе
            var dictIndexes = this.Stage2Domain.GetAll()
                .Where(x => x.Stage3Version.ProgramVersion.Id == version.Id)
                .Where(x => this.ShortObjectDomain.GetAll().Any(y => y.ProgramVersion.Id == version.Id))
                .Select(x => new { x.Stage3Version.IndexNumber, RoId = x.Stage3Version.RealityObject.Id })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => y.Min(x => x.IndexNumber));

            // формируем словарь Максимальных Баллов по Дому котоыре участвуют в краткосрочной программе
            var dictPoints = this.Stage2Domain.GetAll()
                .Where(x => x.Stage3Version.ProgramVersion.Id == version.Id)
                .Where(x => this.ShortObjectDomain.GetAll().Any(y => y.ProgramVersion.Id == version.Id))
                .Select(x => new { x.Stage3Version.Point, RoId = x.Stage3Version.RealityObject.Id })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => y.Max(x => x.Point));

            // Сначала обновляем суммы у тех записей работ у которых уже есть stage1
            var recordsIn = shortRecords.Where(x => x.Stage1Id > 0).ToList();

            foreach (var rec in recordsIn)
            {
                if (!dictStage1.ContainsKey(rec.Stage1Id))
                {
                    continue;
                }

                var st1 = dictStage1[rec.Stage1Id];

                // Проставляем новые значения в St1
                st1.Sum = rec.Cost;
                st1.SumService = rec.ServiceCost;
                st1.Volume = rec.Volume;

                // Поскольку мы его обновлили то добавляем на сохранение
                stage1.Add(st1);
            }

            // Из полученного списка на сохранение 1 этапа получаем 
            var dictForStage2 = stage1.GroupBy(x => x.Stage2Version.Id).ToDictionary(x => x.Key, y => y.Sum(x => x.Sum + x.SumService));

            foreach (var st2Id in dictForStage2)
            {
                if (!dictStage2.ContainsKey(st2Id.Key))
                {
                    continue;
                }

                // получаем запись второго этапа
                var st2 = dictStage2[st2Id.Key];

                // обнновляем сумму
                st2.Sum = st2Id.Value;
                st2.TypeCorrection = TypeCorrection.NotDone;

                // Поскольку мы его обновлили то добавляем на сохранение
                stage2.Add(st2);
            }

            // Из полученного списка на сохранение 2 этапа получаем 
            var dictForStage3 = stage2.GroupBy(x => x.Stage3Version.Id).ToDictionary(x => x.Key, y => y.Sum(x => x.Sum));

            foreach (var st3Id in dictForStage3)
            {
                if (!dictStage3.ContainsKey(st3Id.Key))
                {
                    continue;
                }

                // получаем запись 3 этапа
                var st3 = dictStage3[st3Id.Key];

                // обнновляем сумму
                st3.Sum = st3Id.Value;

                // Поскольку мы его обновлили то добавляем на сохранение
                stage3.Add(st3);
            }

            // Далее для тех работ у которых нет связи с версией мы должны создать цепочку stage1 -> Stage2 -> Stage3
            var recordsOut = shortRecords.Where(x => x.Stage1Id == 0).ToList();

            foreach (var rec in recordsOut)
            {
                if (!infoDict.ContainsKey(rec.Id) || !infoStructElementsDict.ContainsKey(rec.WorkId))
                    continue;

                var infoStructElement = infoStructElementsDict[rec.WorkId];
                var info = infoDict[rec.Id];

                // Поскольку записи не было вообще
                // То создаем всю цепочку последовательно
                var st1 = new VersionRecordStage1
                {
                    RealityObject = new RealityObject { Id = info.RoId },
                    StrElement = new StructuralElement { Id = infoStructElement.StrElId },
                    Sum = rec.Cost,
                    SumService = rec.ServiceCost,
                    Volume = rec.Volume,
                    Year = rec.ShortYearStart,
                    TypeDpkrRecord = TypeDpkrRecord.UserRecord
                };

                rec.Stage1 = st1;

                stage1.Add(st1);

                var st2 = new VersionRecordStage2
                {
                    Sum = st1.Sum + st1.SumService,
                    CommonEstateObject =
                        new CommonEstateObject { Id = infoStructElement.CeoId },
                    TypeDpkrRecord = TypeDpkrRecord.UserRecord,
                    TypeCorrection = TypeCorrection.NotDone
                };

                // проставляем ссылку на 2 этап
                st1.Stage2Version = st2;

                stage2.Add(st2);

                var st3 = new VersionRecord
                {
                    ProgramVersion = new ProgramVersion { Id = version.Id },
                    RealityObject = new RealityObject { Id = info.RoId },
                    Year = rec.ShortYearStart,
                    CorrectYear = rec.ShortYearStart,
                    CommonEstateObjects = infoStructElement.CeoName,
                    FixedYear = true,
                    Sum = st2.Sum,
                    IndexNumber = 0,
                    Point = 0,
                    TypeDpkrRecord = TypeDpkrRecord.UserRecord
                };

                st2.Stage3Version = st3;

                stage3.Add(st3);

                // Теперь осталось посчитать тольк онужный IndexNumper и Point
                // для 3 этапа
                if (dictIndexes.ContainsKey(info.RoId) && dictPoints.ContainsKey(info.RoId))
                {
                    st3.IndexNumber = dictIndexes[info.RoId];
                    dictPoints[info.RoId] = dictPoints[info.RoId] + 1;
                    st3.Point = dictPoints[info.RoId];
                }
            }
        }

        private List<VersionRecord> GetStage3ForUpdateCorrectionYear(ProgramVersion version)
        {
            var result = new List<VersionRecord>();

            // получаем Новые года для Stage3 то есть получаем именно
            // те записи Stage3 которые должны измениться и получаем тот год который должен оказатьсяв версии
            var correctionRecords = this.CorrectionDomain.GetAll()
                .Where(x => x.Stage2.Stage3Version.ProgramVersion.Id == version.Id)
                .Where(x => x.Stage2.Stage3Version.CorrectYear != x.PlanYear)
                .Select(x => new { st3Id = x.Stage2.Stage3Version.Id, x.PlanYear })
                .AsEnumerable()
                .GroupBy(x => x.st3Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.PlanYear).First());

            var updateData = this.Stage3Domain.GetAll()
                .Where(x => x.ProgramVersion.Id == version.Id)
                .Where(x => this.CorrectionDomain.GetAll()
                    .Where(y => y.Stage2.Stage3Version.ProgramVersion.Id == version.Id)
                    .Where(y => y.Stage2.Stage3Version.Id == x.Id)
                    .Any(y => y.Stage2.Stage3Version.CorrectYear != y.PlanYear))
                .ToList();

            foreach (var rec in updateData)
            {
                if (!correctionRecords.ContainsKey(rec.Id))
                {
                    continue;
                }

                rec.CorrectYear = correctionRecords[rec.Id];
                result.Add(rec);
            }

            return result;
        }

        private void Calculation(decimal sumService, List<ShortRecordProxy> works)
        {
            var cnt = works.Count();

            if (cnt == 0)
                return;

            // Поулчаем сумму по переданным работам
            var sumWorks = works.Sum(x => x.Cost);

            // Накопительные переменная для того чтобы где то не потерять копейки
            // фообщем чтобы непотерять ошибкиокруглений я хочу последнюю запись не расчитывать исходя 
            // из процентов а прост овычесть СуммаВсего-СуммаПредыдущихЗАписей
            var temp = 0M;
            int i = 0;

            foreach (var work in works)
            {
                i++;

                if (work.Cost <= 0)
                {
                    continue;
                }

                var currentSum = 0m;

                if (i == cnt)
                {
                    // Если работа последняя то избегаем ошибок округления
                    currentSum = sumService - temp;
                }
                else
                {
                    // вычисляем сумму услуги исходя из доли каждой работы в общей стоимости
                    currentSum = (sumService * (work.Cost / sumWorks)).RoundDecimal(2);
                }

                work.ServiceCost += currentSum;
                work.TotalCost += currentSum;

                // Для того чтобы не было ошибок округления накапливаем Стоимость услуги
                temp += currentSum;
            }
        }

        /// <summary>
        /// Метод который делит общую стоимость работ на услуги ПСД экспертиза, ПСД разработка, Технадзор
        /// </summary>
        private void DealPercents(
            ShortProgramRecord serv1020,
            ShortProgramRecord serv1019,
            ShortProgramRecord serv1018,
            ShortProgramRecord record)
        {
            if (serv1020 == null || serv1019 == null || serv1018 == null || record == null)
            {
                return;
            }

            if (record.ServiceCost > 0)
            {
                var costPSD = (record.ServiceCost * 0.375m).RoundDecimal(2);

                serv1019.Cost += costPSD;
                serv1018.Cost += costPSD;

                // Поидее стоимость услуги технадзор должно считаться Стоимость*0.25 (тоесть 25 % от стоимости услуги)
                // но чтобы не было оишбок при округлении просто вычитаю стоимость предыдущих услуг
                // Не пугайтесь тут нет ошибок просто сумма услуг всех складывается из = Стоимость*0.25 + Стоимость*0.375 + Стоимость*0.375
                // Соответсвенно вычитая из общей стоимости 2 раза costPSD мы и получим стоимость Технадзора без ошибок на округление
                serv1020.Cost += record.ServiceCost - costPSD - costPSD;

                if (serv1020.Cost < 0)
                {
                    serv1020.Cost = 0;
                }
            }
        }

        private LoadParam GetLoadParam(BaseParams baseParams)
        {
            return baseParams.Params.Read<LoadParam>((LoadParam)null).Execute(new Func<DynamicDictionary, LoadParam>(Converter.ToLoadParam));
        }

        private class WorkProxy
        {
            public long Id { get; set; }

            public long WorkId { get; set; }

            public long strElId { get; set; }
        }

        private class Stage1Proxy
        {
            public long Id { get; set; }

            public long st2Id { get; set; }

            public decimal Volume { get; set; }

            public long strElId { get; set; }

            public long RoId { get; set; }

            public decimal Sum { get; set; }

            public decimal SumService { get; set; }
        }

        #region

        // объявляем Код для услуги ПСД расзработка
        private readonly string servicePsdRazrabotka = "1018";

        // Объявляем Код для услуги ПСД экспертиза
        private readonly string servicePsdExpertiza = "1019";

        // Объявляем Код для Услуги Технадзор
        private readonly string serviceTehnadzor = "1020";
        #endregion
    }
}