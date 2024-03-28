namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Bars.GkhCr.Localizers;
    using Castle.Windsor;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;

    /// <summary>
    /// Домен сервис для ДПКР РТ
    /// </summary>
    public class TatDpkrService : IDpkrService
    {

        public IWindsorContainer Container { get; set; }

        public IDomainService<ObjectCr> objectCrDomain { get; set; }

        public IDomainService<TypeWorkCr> typeWorkCrDomain { get; set; }

        public IDomainService<ProgramCr> programCrDomain { get; set; }

        public IDomainService<ShortProgramRecord> shortRecordDomain { get; set; }

        public IDomainService<ShortProgramDefectList> shortDefectListDomain { get; set; }

        public IDomainService<ShortProgramProtocol> shortProtocolDomain { get; set; }

        public IDomainService<ProgramVersion> progVersionDomain { get; set; }

        public IDomainService<FinanceSource> finSourceDomain { get; set; }

        public IDomainService<MonitoringSmr> monitoringSmrDomain { get; set; }

        public IDomainService<VersionRecordStage1> stage1Domain { get; set; }

        public IDomainService<State> stateDomain { get; set; }

        public IGkhUserManager UserManager { get; set; }

        public IDomainService<ProgramCrChangeJournal> ProgChangeJournalDomain { get; set; }

        public IDomainService<ProgramCr> ProgramDomain { get; set; }

        public IFileService FileService { get; set; }

        /// <summary>
        /// Данный метод по Краткосрочной программе ДПКР создает Объекты КР в выбранной программе
        /// переносятся работы, дефектные ведомости и протоколы
        /// </summary>
        public IDataResult CreateProgramCrByDpkr(BaseParams baseParams)
        {
            var programCrId = baseParams.Params.GetAs<long>("programCrId");

            var user = UserManager.GetActiveOperator();

            // получаем все основные утвержденные версии
            var currentVersions = progVersionDomain.GetAll()
                                 .Where(x => x.IsMain && x.Municipality != null && x.State.FinalState)
                                 .AsEnumerable()
                                 .GroupBy(x => x.Municipality.Id)
                                 .ToDictionary(x => x.Key, y => y.FirstOrDefault());

            if (!currentVersions.Any())
            {
                return new BaseDataResult(false, "Ни по одному МО не найдена основаная и утвержденная версия.");
            }

            if (programCrId < 1)
            {
                return new BaseDataResult(false, "Передан неверный идентификатор Программы КР");
            }

            var programCr = Container.Resolve<IDomainService<ProgramCr>>().Get(programCrId);

            var period = programCr.Period;

            var currentObjectCrMunicipalityIds =
                objectCrDomain.GetAll()
                              .Where(x => x.ProgramCr.Id == programCrId)
                              .Select(x => x.RealityObject.Municipality.Id)
                              .Distinct()
                              .ToList();

            var noExistIds = currentVersions.Keys.Where(x => !currentObjectCrMunicipalityIds.Contains(x)).ToList();

            if (!noExistIds.Any())
            {
                return new BaseDataResult(false, "По всем МО уже было проведено формирование программы из региональной программы");
            }
            
            // получаем те версии по которым еще непроводилась выгрузка из краткосрочки
            var versionIds = currentVersions.Values
                .Where(x => noExistIds.Contains(x.Municipality.Id))
                .Select(x => x.Id).Distinct().ToList();

            var queryShortRecords = shortRecordDomain.GetAll()
                .Where(x => versionIds.Contains(x.ShortProgramObject.ProgramVersion.Id))
                .Where(x => x.ShortProgramObject.Year == period.DateStart.Year);

            // проверяем есть ли вообще записи
            if (!queryShortRecords.Any())
            {
                return new BaseDataResult(false, "Не найдены записи");
            }

            // проверяем есть ли среди нужных нам записей те у которых не конечнй статус Объекта краткосрочки
            if (queryShortRecords.Any(x => !x.ShortProgramObject.State.FinalState))
            {

                // если такие имеются, то выводим Названия МО в которых есть такие записи
                var names =
                    queryShortRecords.Where(x => !x.ShortProgramObject.State.FinalState)
                                     .Select(x => x.ShortProgramObject.RealityObject.Municipality.Name)
                                     .Distinct()
                                     .ToList()
                                     .AggregateWithSeparator(", ");

                return new BaseDataResult(false, string.Format("В следующих МО не все дома в конечном статусе: {0}", names));
            }

            // получаем работы по краткосрочнйо программе в дпкр
            var dataWorks = queryShortRecords
                .Select(
                    x =>
                        new
                        {
                            x.Id,
                            year = x.ShortProgramObject.Year,
                            Stage1Id = x.Stage1 != null ? (long?)x.Stage1.Id : null,
                            roId = x.ShortProgramObject.RealityObject.Id,
                            workId = x.Work.Id,
                            sum = x.Cost,
                            volume = x.Volume,
                            stateName = x.ShortProgramObject.State.Name
                        })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(x => x.Key, y => y.ToList());
            
            // Получаю источник финансирвоания по коду (нужен для объекта КР)
            var finSource = this.finSourceDomain.GetAll().FirstOrDefault(x => x.Code == "1");

            var stateObjectCrByName = stateDomain.GetAll()
                .Where(x => x.TypeId == "cr_object")
                .AsEnumerable()
                .ToDictionary(x => x.Name.Trim().ToUpper());

            // Получаю начальный статус для Мониторинга смр
            var firstStateSmr = stateDomain.GetAll().FirstOrDefault(x => x.StartState && x.TypeId == "cr_obj_monitoring_cmp");

            // получаю словарь статусов для Дефектной ведомости чтобы сопоставлять по Name
            var dictStatesDefectList =
                stateDomain.GetAll()
                           .Where(x => x.TypeId == "cr_obj_defect_list")
                           .AsEnumerable()
                           .GroupBy(x => x.Name)
                           .ToDictionary(x => x.Key, y => y.FirstOrDefault());

            var listObjectCrToSave = new List<ObjectCr>();
            var listWorkTypeCrToSave = new List<TypeWorkCr>();
            var listSmrToSave = new List<MonitoringSmr>();
            var listDefectToSave = new List<DefectList>();
            var listProtocolToSave = new List<ProtocolCr>();
            var listTypeWorkStage1ToSave = new List<TypeWorkCrVersionStage1>();

            // Получаем дефектные ведомости по краткосрочке
            var queryDefectList = shortDefectListDomain.GetAll()
               .Where(x => versionIds.Contains(x.ShortObject.ProgramVersion.Id)
                   && x.DocumentDate >= period.DateStart
                   && (!period.DateEnd.HasValue || x.DocumentDate <= period.DateEnd));

            var dictDefect = queryDefectList.Select(
                    x =>
                    new
                    {
                        x.Id,
                        roId = x.ShortObject.RealityObject.Id,
                        StateName = x.State.Name,
                        workId = x.Work.Id,
                        x.DocumentDate,
                        x.DocumentName,
                        x.File,
                        x.Sum
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.roId)
                    .ToDictionary(x => x.Key, y => y.ToList());


            // Получаем протоколы по краткосрочке
            var queryProtocols = shortProtocolDomain.GetAll()
               .Where(x => versionIds.Contains(x.ShortObject.ProgramVersion.Id));

            var dictProtocols =
                queryProtocols.Select(
                    x =>
                    new
                    {
                        x.Id,
                        roId = x.ShortObject.RealityObject.Id,
                        ContragentId = (long?)x.Contragent.Id,
                        x.DocumentName,
                        x.DocumentNum,
                        x.CountAccept,
                        x.CountVote,
                        x.CountVoteGeneral,
                        x.Description,
                        x.DateFrom,
                        x.GradeOccupant,
                        x.GradeClient,
                        x.File,
                        x.SumActVerificationOfCosts
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.roId)
                    .ToDictionary(x => x.Key, y => y.ToList());

            foreach (var kvp in dataWorks)
            {
                var stateName = kvp.Value.First().stateName.Trim().ToUpper();

                var newObjectCr = new ObjectCr(new RealityObject { Id = kvp.Key }, new ProgramCr { Id = programCrId })
                {
                    State = stateObjectCrByName.ContainsKey(stateName) ? stateObjectCrByName[stateName] : null
                };

                listObjectCrToSave.Add(newObjectCr);

                var smr = new MonitoringSmr { ObjectCr = newObjectCr, State = firstStateSmr };

                listSmrToSave.Add(smr);

                // Необходимо чтобы в КР ушли работы схлопнутые по виду работ
                // Поэтому делаю общий словарь чтобы неполучилось задвоения
                var currentWorks = new Dictionary<long, TypeWorkCr>();

                // теперь для каждой полученной записи создаем запись работы по объекту КР
                foreach (var item in kvp.Value)
                {

                    if (!currentWorks.ContainsKey(item.workId))
                    {
                        var newWork = new TypeWorkCr
                                          {
                                              ObjectCr = newObjectCr,
                                              Work = new Work { Id = item.workId },
                                              Sum = 0,
                                              CostSum = 0,
                                              Volume = 0,
                                              FinanceSource = finSource,
                                              YearRepair = item.year,
                                              IsDpkrCreated = true,
                                              IsActive = true
                                          };

                        currentWorks.Add(item.workId, newWork);

                        listWorkTypeCrToSave.Add(newWork);
                    }

                    var work = currentWorks[item.workId];

                    work.Sum += item.sum;
                    work.Volume += item.volume;

                    if (item.Stage1Id.HasValue)
                    {
                        var newTypeWorkSt1 = new TypeWorkCrVersionStage1
                                                 {
                                                     TypeWorkCr = work,
                                                     Stage1Version = new VersionRecordStage1 { Id = item.Stage1Id.Value},
                                                     Sum = item.sum,
                                                     Volume = item.volume
                                                 };

                        listTypeWorkStage1ToSave.Add(newTypeWorkSt1);
                    }
                }

                if (dictDefect.ContainsKey(kvp.Key))
                {
                    foreach (var defect in dictDefect[kvp.Key])
                    {
                        var newDefectLsit = new DefectList
                        {
                            ObjectCr = newObjectCr,
                            State = dictStatesDefectList.ContainsKey(defect.StateName) ? dictStatesDefectList[defect.StateName] : null,
                            Work = new Work { Id = defect.workId},
                            DocumentDate = defect.DocumentDate,
                            DocumentName = defect.DocumentName,
                            File = this.FileService.ReCreateFile(defect.File),
                            Sum = defect.Sum
                        };

                        listDefectToSave.Add(newDefectLsit);
                    }
                }

                if (dictProtocols.ContainsKey(kvp.Key))
                {
                    foreach (var prot in dictProtocols[kvp.Key])
                    {
						var allGlossaryItems = TypeDocumentCrLocalizer.GetAllGlossaryItems();
						var item = TypeDocumentCrLocalizer.GetItemFromCacheByKey(allGlossaryItems, TypeDocumentCrLocalizer.ProtocolNeedCrKey);

                        var newProtocol = new ProtocolCr
                        {
                            ObjectCr = newObjectCr,
                            Contragent = prot.ContragentId > 0 ? new Contragent { Id = prot.ContragentId.ToInt() } : null,
                            DocumentName = prot.DocumentName,
                            DocumentNum = prot.DocumentNum,
                            CountAccept = prot.CountAccept,
                            CountVote = prot.CountVote,
                            CountVoteGeneral = prot.CountVoteGeneral,
                            Description = prot.Description,
                            DateFrom = prot.DateFrom,
                            GradeOccupant = prot.GradeOccupant,
                            GradeClient = prot.GradeClient,
                            File = prot.File,
                            SumActVerificationOfCosts = prot.SumActVerificationOfCosts,
                            TypeDocumentCr = item
                        };

                        listProtocolToSave.Add(newProtocol);
                    }
                }

            }

            var session = Container.Resolve<ISessionProvider>().OpenStatelessSession();

            try
            {
                using (var tr = session.BeginTransaction())
                {
                    try
                    {
                        listObjectCrToSave.ForEach(x => session.Insert(x));
                        listSmrToSave.ForEach(x => session.Insert(x));
                        listWorkTypeCrToSave.ForEach(x => session.Insert(x));
                        listDefectToSave.ForEach(x => session.Insert(x));
                        listProtocolToSave.ForEach(x => session.Insert(x));
                        listTypeWorkStage1ToSave.ForEach(x => session.Insert(x));


                        ProgChangeJournalDomain.Save(new ProgramCrChangeJournal
                        {
                            ProgramCr = ProgramDomain.Load(programCrId),
                            ChangeDate = DateTime.Now,
                            TypeChange = TypeChangeProgramCr.FromDpkr,
                            UserName = user != null ? user.Name : "Администратор",
                            MuCount = versionIds.Count
                        });

                        tr.Commit();
                    }
                    catch (Exception)
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception exc)
            {
                return new BaseDataResult(false, string.Format("Ошибка: {0}", exc.Message));
            }        

            return new BaseDataResult(true, "Программа сформирована на основе региональной программы успешно");
        }

        public IEnumerable<RealityObjectDpkrInfo> GetOvrhlYears(BaseParams baseParams)
        {
            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");

            var currentYear = DateTime.Now.Date.Year;

            return Container.Resolve<IDomainService<VersionRecord>>().GetAll()
                .Where(x => x.ProgramVersion.IsMain)
                .Where(x => x.RealityObject.Municipality.Id == municipalityId)
                .Where(x => x.Year >= currentYear)
                .Select(x => new RealityObjectDpkrInfo
                {
                    RealityObject = x.RealityObject,
                    Year = x.Year,
                    Sum = x.Sum
                })
                .OrderBy(x => x.Year)
                .AsEnumerable()
                .GroupBy(x => x.RealityObject.Id)
                .Select(x => x.First());
        }
    }
}