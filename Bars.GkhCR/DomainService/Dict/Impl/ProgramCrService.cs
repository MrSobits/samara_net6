namespace Bars.GkhCr.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    
    using Castle.Windsor;
    
    public class ProgramCrService : IProgramCrService
    {
        public IWindsorContainer Container { get; set; }
        public IFileService FileService { get; set; }
        public IUserIdentity user { get; set; }

        private T ToDerived<T>(T tBase, IEnumerable<PropertyInfo> properties)
                where T : class, IEntity, new()
        {
            var tDerived = new T();
            foreach (var propBase in properties.Where(x => x.CanWrite))
            {
                propBase.SetValue(tDerived, propBase.GetValue(tBase, null), null);
            }
            return tDerived;
        }

        public virtual IDataResult CopyProgram(BaseParams baseParams)
        {
            var programCrRepos = Container.Resolve<IRepository<ProgramCrChangeJournal>>();
            var typeWorkCrRepos = Container.Resolve<IRepository<TypeWorkCr>>();
            var programCrDomain = Container.Resolve<IRepository<ProgramCr>>();

            var oldProgramCrId = baseParams.Params.GetAs("programCrId", 0l);

            var program = programCrDomain.FirstOrDefault(x => x.Id == oldProgramCrId);

            if (program == null)
            {
                return new BaseDataResult(false, "Не удалось получить программу кап. ремонта");
            }

            try
            {
                // Определяем по какому алгоритму нужнокопирвоать программу КР
                // Если программа КР была создана на основе ДПКР то нужно запускать метод который реализован в Overhaul.Base модуле
                if (typeWorkCrRepos.GetAll().Any(x => x.ObjectCr.ProgramCr.Id == oldProgramCrId && x.IsDpkrCreated)
                    || programCrRepos.GetAll().Any(x => x.ProgramCr.Id == oldProgramCrId && x.TypeChange == TypeChangeProgramCr.FromDpkr))
                {
                    // Если программа которую хотят скопирвоать сформирвоана на основе ДПКР, то передаем метод копирования модулю Overhaul.Base 
                    // для того чтобы скопирвоались все связи с ДПКР в новую программу кап ремонта

                    var programCrByDpkrServcie = Container.Resolve<IProgramCrCopByDpkr>();

                    if (programCrByDpkrServcie == null)
                    {
                        return new BaseDataResult(false, "Не найдена реализация IProgramCrCopByDpkr");
                    }

                    try
                    {
                        return programCrByDpkrServcie.CopyProgramCr(baseParams);
                    }
                    finally
                    {
                        Container.Release(programCrByDpkrServcie);
                    }
                }
                else
                {
                    return BaseCopyProgram(baseParams);
                }
            }
            finally
            {
                Container.Release(programCrDomain);
                Container.Release(programCrRepos);
                Container.Release(typeWorkCrRepos);
            }
        }

        private IDataResult BaseCopyProgram(BaseParams baseParams)
        {
            var oldProgramCrId = baseParams.Params["programCrId"].ToInt();
            var name = baseParams.Params["Name"].ToStr();
            var code = baseParams.Params["Code"].ToStr();
            var period = baseParams.Params["Period"].ToInt();
            var visible = baseParams.Params["Visible"].To<TypeVisibilityProgramCr>();
            var type = baseParams.Params["Type"].To<TypeProgramCr>();
            var state = baseParams.Params["State"].To<TypeProgramStateCr>();
            var useInExport = baseParams.Params["UseInExport"].ToBool();
            var isAddHouse = baseParams.Params["IsAddHouse"].ToBool();
            var notValidLaw = baseParams.Params["NotValidLaw"].ToBool();
            var description = baseParams.Params["Description"].ToStr();
            var copyWithoutAttachments = baseParams.Params["CopyWithoutAttachments"].ToBool();

            // Используется Repository, что бы не вызывался интерцептор
            var objectCrService = this.Container.Resolve<IRepository<Entities.ObjectCr>>();
            var finSourceProgramCrService = this.Container.Resolve<IRepository<ProgramCrFinSource>>();
            var programCrService = this.Container.Resolve<IRepository<ProgramCr>>();
            var performedWorkActService = this.Container.Resolve<IRepository<PerformedWorkAct>>();
            var performedWorkActRecService = this.Container.Resolve<IRepository<PerformedWorkActRecord>>();
            var qualificationService = this.Container.Resolve<IRepository<Qualification>>();
            var voiceMemberService = this.Container.Resolve<IRepository<VoiceMember>>();
            var buildContractService = this.Container.Resolve<IRepository<BuildContract>>();
            var contractCrService = this.Container.Resolve<IRepository<ContractCr>>();
            var defectListService = this.Container.Resolve<IRepository<DefectList>>();
            var documentWorkCrService = this.Container.Resolve<IRepository<DocumentWorkCr>>();
            var financeSourceResourceService = this.Container.Resolve<IRepository<FinanceSourceResource>>();
            var monitoringSmrService = this.Container.Resolve<IRepository<MonitoringSmr>>();
            var personalAccountService = this.Container.Resolve<IRepository<PersonalAccount>>();
            var protocolCrService = this.Container.Resolve<IRepository<ProtocolCr>>();
            var typeWorkCrService = this.Container.Resolve<IRepository<TypeWorkCr>>();
            var programCrJournalService = this.Container.Resolve<IRepository<ProgramCrChangeJournal>>();

            try
            {
                var journalProperties = typeof(ProgramCrChangeJournal).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var finProperties = typeof(ProgramCrFinSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var objectCrProperties = typeof(Entities.ObjectCr).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var qualProperties = typeof(Qualification).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var buildContractProperties = typeof(BuildContract).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .WhereIf(copyWithoutAttachments, x => new[]{nameof(BuildContract.DocumentFile), nameof(BuildContract.ProtocolFile)}.All(y => y != x.Name));
                var contractCrProperties = typeof(ContractCr).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var defectListProperties = typeof(DefectList).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .WhereIf(copyWithoutAttachments, x => x.Name != nameof(DefectList.File));
                var documentWorkCrProperties = typeof(DocumentWorkCr).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .WhereIf(copyWithoutAttachments, x => x.Name != nameof(DocumentWorkCr.File));
                var financeSourceResourceProperties = typeof(FinanceSourceResource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var monitoringSmrProperties = typeof(MonitoringSmr).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var personalAccountProperties = typeof(PersonalAccount).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var protocolCrProperties = typeof(ProtocolCr).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .WhereIf(copyWithoutAttachments, x => x.Name != nameof(ProtocolCr.File));
                var typeWorkCrProperties = typeof(TypeWorkCr).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var performedWorkActProperties = typeof(PerformedWorkAct).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .WhereIf(copyWithoutAttachments, x => new[]{nameof(PerformedWorkAct.DocumentFile), nameof(PerformedWorkAct.CostFile), nameof(PerformedWorkAct.AdditionFile)}.All(y => y != x.Name));
                var performedWorkActRecProperties = typeof(PerformedWorkActRecord).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var voiceMemberProperties = typeof(VoiceMember).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                // Списки для сохранения
                var objectCrSaveList = new List<Entities.ObjectCr>();
                var performedWorkActSaveList = new List<PerformedWorkAct>();
                var performedWorkActRecordSaveList = new List<PerformedWorkActRecord>();
                var qualificationSaveList = new List<Qualification>();
                var voiceMemberSaveList = new List<VoiceMember>();
                var buildContractSaveList = new List<BuildContract>();
                var contractCrSaveList = new List<ContractCr>();
                var defectListSaveList = new List<DefectList>();
                var documentWorkCrSaveList = new List<DocumentWorkCr>();
                var financeSourceResourceSaveList = new List<FinanceSourceResource>();
                var monitoringSmrSaveList = new List<MonitoringSmr>();
                var personalAccountSaveList = new List<PersonalAccount>();
                var protocolCrSaveList = new List<ProtocolCr>();
                var typeWorkCrSaveList = new List<TypeWorkCr>();
                var programCrFinSourceSaveList = new List<ProgramCrFinSource>();

                var qualificationDict = qualificationService.GetAll().Where(x => x.ObjectCr.ProgramCr.Id == oldProgramCrId).GroupBy(x => x.ObjectCr.Id).ToDictionary(x => x.Key, y => y.ToList());
                var buildContractDict = buildContractService.GetAll().Where(x => x.ObjectCr.ProgramCr.Id == oldProgramCrId).GroupBy(x => x.ObjectCr.Id).ToDictionary(x => x.Key, y => y.ToList());
                var contractCrDict = contractCrService.GetAll().Where(x => x.ObjectCr.ProgramCr.Id == oldProgramCrId).GroupBy(x => x.ObjectCr.Id).ToDictionary(x => x.Key, y => y.ToList());
                var financeSourceResourceDict = financeSourceResourceService.GetAll().Where(x => x.ObjectCr.ProgramCr.Id == oldProgramCrId).GroupBy(x => x.ObjectCr.Id).ToDictionary(x => x.Key, y => y.ToList());
                var documentWorkCrDict = documentWorkCrService.GetAll().Where(x => x.ObjectCr.ProgramCr.Id == oldProgramCrId).GroupBy(x => x.ObjectCr.Id).ToDictionary(x => x.Key, y => y.ToList());
                var monitoringSmrDict = monitoringSmrService.GetAll().Where(x => x.ObjectCr.ProgramCr.Id == oldProgramCrId).GroupBy(x => x.ObjectCr.Id).ToDictionary(x => x.Key, y => y.ToList());
                var personalAccountDict = personalAccountService.GetAll().Where(x => x.ObjectCr.ProgramCr.Id == oldProgramCrId).GroupBy(x => x.ObjectCr.Id).ToDictionary(x => x.Key, y => y.ToList());
                var protocolCrDict = protocolCrService.GetAll().Where(x => x.ObjectCr.ProgramCr.Id == oldProgramCrId).GroupBy(x => x.ObjectCr.Id).ToDictionary(x => x.Key, y => y.ToList());
                var typeWorkCrDict = typeWorkCrService.GetAll().Where(x => x.ObjectCr.ProgramCr.Id == oldProgramCrId).GroupBy(x => x.ObjectCr.Id).ToDictionary(x => x.Key, y => y.ToList());
                var performedWorkActDict = performedWorkActService.GetAll().Where(x => x.ObjectCr.ProgramCr.Id == oldProgramCrId).GroupBy(x => x.ObjectCr.Id).ToDictionary(x => x.Key, y => y.ToList());
                var voiceMemberDict = voiceMemberService.GetAll().Where(x => x.Qualification.ObjectCr.ProgramCr.Id == oldProgramCrId).GroupBy(x => x.Qualification.Id).ToDictionary(x => x.Key, y => y.ToList());
                var performedWorkActRecDict = performedWorkActRecService.GetAll().Where(x => x.PerformedWorkAct.ObjectCr.ProgramCr.Id == oldProgramCrId).GroupBy(x => x.PerformedWorkAct.Id).ToDictionary(x => x.Key, y => y.ToList());

                using (var tr = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        // Программа КР
                        var newProgramCr = new ProgramCr
                        {
                            Id = 0,
                            Name = name,
                            Code = code,
                            Description = description,
                            Period = new Period { Id = period },
                            TypeVisibilityProgramCr = visible,
                            TypeProgramCr = type,
                            TypeProgramStateCr = state,
                            UsedInExport = useInExport,
                            MatchFl = notValidLaw,
                            NotAddHome = isAddHouse
                        };

                        var history = programCrJournalService.GetAll().FirstOrDefault(x => x.ProgramCr.Id == oldProgramCrId && x.TypeChange == TypeChangeProgramCr.FromDpkr);
                        var program = programCrService.GetAll().FirstOrDefault(x => x.Id == oldProgramCrId);
                        var defectListDict = defectListService.GetAll()
                            .Where(x => x.ObjectCr.ProgramCr.Id == oldProgramCrId
                                && x.DocumentDate >= newProgramCr.Period.DateStart
                                && (!newProgramCr.Period.DateEnd.HasValue || x.DocumentDate <= newProgramCr.Period.DateEnd))
                            .GroupBy(x => x.ObjectCr.Id).ToDictionary(x => x.Key, y => y.ToList());

                        ProgramCrChangeJournal newHistory;

                        if (history != null)
                        {
                            newHistory = this.ToDerived(history, journalProperties);
                            newHistory.Id = 0;
                            newHistory.ProgramCr = newProgramCr;
                            newHistory.UserName = user.Name;
                            newHistory.Description = string.Format("Скопирована с '{0}'", program.Name);
                        }
                        else
                        {
                            newHistory = new ProgramCrChangeJournal()
                            {
                                Id = 0,
                                TypeChange = TypeChangeProgramCr.Manually,
                                ProgramCr = newProgramCr,
                                UserName = user.Name,
                                Description = string.Format("Скопирована с '{0}'", program.Name)
                            };
                        }

                        // Фин. источники программы
                        var finSourceProgramCrList = finSourceProgramCrService.GetAll().Where(x => x.ProgramCr.Id == oldProgramCrId).ToList();

                        foreach (var item in finSourceProgramCrList)
                        {
                            var newFinSrc = this.ToDerived(item, finProperties);
                            newFinSrc.Id = 0;
                            newFinSrc.ProgramCr = newProgramCr;

                            programCrFinSourceSaveList.Add(newFinSrc);
                        }

                        // Объекты КР программы
                        var objectCrList = objectCrService.GetAll().Where(x => x.ProgramCr.Id == oldProgramCrId).ToList();

                        foreach (var objectCr in objectCrList)
                        {
                            var newObjectCr = this.ToDerived(objectCr, objectCrProperties);
                            newObjectCr.Id = 0;
                            newObjectCr.ProgramCr = newProgramCr;
                            objectCrSaveList.Add(newObjectCr);

                            // Квалификационный отбор объекта КР
                            if (qualificationDict.ContainsKey(objectCr.Id))
                            {
                                var qualificationList = qualificationDict[objectCr.Id];

                                foreach (var qualification in qualificationList)
                                {
                                    var newQualification = this.ToDerived(qualification, qualProperties);
                                    newQualification.Id = 0;
                                    newQualification.ObjectCr = newObjectCr;
                                    qualificationSaveList.Add(newQualification);

                                    if (voiceMemberDict.ContainsKey(qualification.Id))
                                    {
                                        var voiceMemberList = voiceMemberDict[qualification.Id];

                                        // Голос участника квалиф. отбора
                                        foreach (var voiceMember in voiceMemberList)
                                        {
                                            var newVoiceMember = this.ToDerived(voiceMember, voiceMemberProperties);
                                            newVoiceMember.Id = 0;
                                            newVoiceMember.Qualification = newQualification;
                                            voiceMemberSaveList.Add(newVoiceMember);
                                        }
                                    }
                                }
                            }

                            // Договор подряда КР
                            if (buildContractDict.ContainsKey(objectCr.Id))
                            {
                                var buildContractList = buildContractDict[objectCr.Id];

                                foreach (var buildContract in buildContractList)
                                {
                                    var newBuildContract = this.ToDerived(buildContract, buildContractProperties);
                                    newBuildContract.Id = 0;
                                    newBuildContract.ObjectCr = newObjectCr;
                                    newBuildContract.DocumentFile = FileService.ReCreateFile(newBuildContract.DocumentFile);
                                    newBuildContract.ProtocolFile = FileService.ReCreateFile(newBuildContract.ProtocolFile);
                                    buildContractSaveList.Add(newBuildContract);
                                }
                            }

                            // Договор КР
                            if (contractCrDict.ContainsKey(objectCr.Id))
                            {
                                var contractCrList = contractCrDict[objectCr.Id];

                                foreach (var contractCr in contractCrList)
                                {
                                    var newContractCr = this.ToDerived(contractCr, contractCrProperties);
                                    newContractCr.Id = 0;
                                    newContractCr.ObjectCr = newObjectCr;
                                    newContractCr.File = FileService.ReCreateFile(newContractCr.File);
                                    contractCrSaveList.Add(newContractCr);
                                }
                            }

                            // Дефектная ведомость
                            if (defectListDict.ContainsKey(objectCr.Id))
                            {
                                var defectList = defectListDict[objectCr.Id];

                                foreach (var defect in defectList)
                                {
                                    var newDefect = this.ToDerived(defect, defectListProperties);
                                    newDefect.Id = 0;
                                    newDefect.ObjectCr = newObjectCr;
                                    newDefect.File = FileService.ReCreateFile(newDefect.File);
                                    defectListSaveList.Add(newDefect);
                                }
                            }

                            // Документ работы объекта КР
                            if (documentWorkCrDict.ContainsKey(objectCr.Id))
                            {
                                var documentWorkCrList = documentWorkCrDict[objectCr.Id];

                                foreach (var documentWorkCr in documentWorkCrList)
                                {
                                    var newDocumentWorkCr = this.ToDerived(documentWorkCr, documentWorkCrProperties);
                                    newDocumentWorkCr.Id = 0;
                                    newDocumentWorkCr.ObjectCr = newObjectCr;
                                    documentWorkCrSaveList.Add(newDocumentWorkCr);
                                }
                            }

                            // Средства источника финансирования
                            if (financeSourceResourceDict.ContainsKey(objectCr.Id))
                            {
                                var financeSourceResourceList = financeSourceResourceDict[objectCr.Id];

                                foreach (var financeSourceResource in financeSourceResourceList)
                                {
                                    var newFinanceSourceResource = this.ToDerived(
                                        financeSourceResource, financeSourceResourceProperties);
                                    newFinanceSourceResource.Id = 0;
                                    newFinanceSourceResource.ObjectCr = newObjectCr;
                                    financeSourceResourceSaveList.Add(newFinanceSourceResource);
                                }
                            }

                            // Мониторинг СМР
                            if (monitoringSmrDict.ContainsKey(objectCr.Id))
                            {
                                var monitoringSmrList = monitoringSmrDict[objectCr.Id];

                                foreach (var monitoringSmr in monitoringSmrList)
                                {
                                    var newMonitoringSmr = this.ToDerived(monitoringSmr, monitoringSmrProperties);
                                    newMonitoringSmr.Id = 0;
                                    newMonitoringSmr.ObjectCr = newObjectCr;
                                    monitoringSmrSaveList.Add(newMonitoringSmr);
                                }
                            }

                            // Лицевой счет
                            if (personalAccountDict.ContainsKey(objectCr.Id))
                            {
                                var personalAccountList = personalAccountDict[objectCr.Id];

                                foreach (var personalAccount in personalAccountList)
                                {
                                    var newPersonalAccount = this.ToDerived(personalAccount, personalAccountProperties);
                                    newPersonalAccount.Id = 0;
                                    newPersonalAccount.ObjectCr = newObjectCr;
                                    personalAccountSaveList.Add(newPersonalAccount);
                                }
                            }

                            // Протокол(акт)
                            if (protocolCrDict.ContainsKey(objectCr.Id))
                            {
                                var protocolCrList = protocolCrDict[objectCr.Id];

                                foreach (var protocolCr in protocolCrList)
                                {
                                    var newProtocolCr = this.ToDerived(protocolCr, protocolCrProperties);
                                    newProtocolCr.Id = 0;
                                    newProtocolCr.File = FileService.ReCreateFile(newProtocolCr.File);
                                    newProtocolCr.ObjectCr = newObjectCr;
                                    protocolCrSaveList.Add(newProtocolCr);
                                }
                            }

                            // Вид работы КР
                            if (typeWorkCrDict.ContainsKey(objectCr.Id))
                            {
                                var typeWorkCrList = typeWorkCrDict[objectCr.Id];

                                foreach (var typeWorkCr in typeWorkCrList)
                                {
                                    var newTypeWorkCr = this.ToDerived(typeWorkCr, typeWorkCrProperties);
                                    newTypeWorkCr.Id = 0;
                                    newTypeWorkCr.ObjectCr = newObjectCr;
                                    typeWorkCrSaveList.Add(newTypeWorkCr);

                                    // Акты вып-х работ объекта КР
                                    if (performedWorkActDict.ContainsKey(objectCr.Id) && performedWorkActDict[objectCr.Id].Any())
                                    {
                                        var performedWorkActList = performedWorkActDict[objectCr.Id].Where(x => x.TypeWorkCr != null && x.TypeWorkCr.Id == typeWorkCr.Id);

                                        foreach (var performedWorkAct in performedWorkActList)
                                        {
                                            var newPerformedWorkAct = this.ToDerived(
                                                performedWorkAct, performedWorkActProperties);
                                            newPerformedWorkAct.Id = 0;
                                            newPerformedWorkAct.TypeWorkCr = newTypeWorkCr;
                                            newPerformedWorkAct.ObjectCr = newObjectCr;

                                            //Делаем ReCreateFile всех прикреплённых файлов, чтобы при дальнейшем акты ссылались на свои файлы
                                            newPerformedWorkAct.CostFile = FileService.ReCreateFile(performedWorkAct.CostFile);
                                            newPerformedWorkAct.AdditionFile = FileService.ReCreateFile(performedWorkAct.AdditionFile);
                                            newPerformedWorkAct.DocumentFile = FileService.ReCreateFile(performedWorkAct.DocumentFile);

                                            performedWorkActSaveList.Add(newPerformedWorkAct);

                                            // Записи акта вып-х работ объекта КР
                                            if (performedWorkActRecDict.ContainsKey(performedWorkAct.Id))
                                            {
                                                var performedWorkActRecList = performedWorkActRecDict[performedWorkAct.Id];

                                                foreach (var performedWorkActRec in performedWorkActRecList)
                                                {
                                                    var newPerformedWorkActRec = this.ToDerived(
                                                        performedWorkActRec, performedWorkActRecProperties);
                                                    newPerformedWorkActRec.Id = 0;
                                                    newPerformedWorkActRec.PerformedWorkAct = performedWorkAct;
                                                    performedWorkActRecordSaveList.Add(newPerformedWorkActRec);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // Тут идет типа сохранение

                        var dateCopy = DateTime.Now;

                        newProgramCr.ObjectCreateDate = dateCopy;
                        newProgramCr.ObjectEditDate = dateCopy;
                        newProgramCr.ObjectVersion = 0;
                        programCrService.Save(newProgramCr);

                        newHistory.ChangeDate = dateCopy;
                        newHistory.ObjectCreateDate = dateCopy;
                        newHistory.ObjectEditDate = dateCopy;
                        newHistory.ObjectVersion = 0;
                        programCrJournalService.Save(newHistory);

                        programCrFinSourceSaveList.ForEach(x =>
                        {
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;
                            x.ObjectVersion = 0;

                            finSourceProgramCrService.Save(x);
                        });

                        objectCrSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            objectCrService.Save(x);
                        });

                        typeWorkCrSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            typeWorkCrService.Save(x);
                        });

                        performedWorkActSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            performedWorkActService.Save(x);
                        });


                        performedWorkActRecordSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            performedWorkActRecService.Save(x);
                        });

                        qualificationSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            qualificationService.Save(x);
                        });

                        voiceMemberSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            voiceMemberService.Save(x);
                        });

                        buildContractSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            buildContractService.Save(x);
                        });

                        contractCrSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            contractCrService.Save(x);
                        });

                        defectListSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            defectListService.Save(x);
                        });

                        documentWorkCrSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            documentWorkCrService.Save(x);
                        });

                        financeSourceResourceSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            financeSourceResourceService.Save(x);
                        });

                        monitoringSmrSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            monitoringSmrService.Save(x);
                        });

                        personalAccountSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            personalAccountService.Save(x);
                        });

                        protocolCrSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            protocolCrService.Save(x);
                        });

                        tr.Commit();

                        return new BaseDataResult
                        {
                            Success = true
                        };
                    }
                    catch (Exception exc)
                    {
                        tr.Rollback();
                        return new BaseDataResult
                        {
                            Success = false,
                            Message = exc.Message
                        };
                    }
                }



            }
            finally
            {
                Container.Release(objectCrService);
                Container.Release(finSourceProgramCrService);
                Container.Release(programCrService);
                Container.Release(performedWorkActService);
                Container.Release(performedWorkActRecService);
                Container.Release(qualificationService);
                Container.Release(voiceMemberService);
                Container.Release(buildContractService);
                Container.Release(contractCrService);
                Container.Release(defectListService);
                Container.Release(documentWorkCrService);
                Container.Release(financeSourceResourceService);
                Container.Release(monitoringSmrService);
                Container.Release(personalAccountService);
                Container.Release(protocolCrService);
                Container.Release(typeWorkCrService);
                Container.Release(programCrJournalService);
            }

        }

        public IDataResult ListForQualification(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var currPeriods = this.Container.Resolve<IDomainService<Period>>()
                    .GetAll()
                    .Where(x => x.DateStart < DateTime.Now && (!x.DateEnd.HasValue || x.DateEnd > DateTime.Now))
                    .Select(x => x.Id)
                    .ToArray();

            var data = Container.Resolve<IDomainService<ProgramCr>>()
                          .GetAll()
                          .Where(x => x.TypeProgramStateCr == TypeProgramStateCr.Active && currPeriods.Contains(x.Period.Id))
                          .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data.Order(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }

        public IDataResult ListWithoutPaging(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var forObjCr = baseParams.Params.GetAs("forObjCr", false);

            var domainService = this.Container.Resolve<IDomainService<ProgramCr>>();

            var data = domainService.GetAll()
                 .WhereIf(forObjCr,
                    x => x.TypeVisibilityProgramCr != TypeVisibilityProgramCr.Hidden
                    && x.TypeVisibilityProgramCr != TypeVisibilityProgramCr.Print)
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Name)
                .Order(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }

        public IDataResult GetAonProgramsList(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var forObjCr = baseParams.Params.GetAs("forObjCr", false);

            var domainService = this.Container.Resolve<IDomainService<ProgramCr>>();

            var data = domainService.GetAll()
                .Where(x => x.TypeProgramStateCr == TypeProgramStateCr.Active ||
                            x.TypeProgramStateCr == TypeProgramStateCr.Open ||
                            x.TypeProgramStateCr == TypeProgramStateCr.New)
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public IDataResult RealityObjectList(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var onlyFromRegionProgram = baseParams.Params.GetAs("onlyFromRegionProgram", false);

            var programCrRoService = this.Container.Resolve<IProgramCrRealityObjectService>();
            var realityObjDomain = this.Container.Resolve<IDomainService<RealityObject>>();

            var realityObjInMain = programCrRoService.GetObjectsInMainProgram();
            var allRealityObject = realityObjDomain.GetAll();

            var data = onlyFromRegionProgram ? realityObjInMain : allRealityObject;

            var result = data.Select(
                x => new
                {
                    x.Id,
                    Municipality = x.Municipality.Name,
                    x.Address
                })
                .Filter(loadParams, this.Container);

            var totalCount = result.Count();

            return new ListDataResult(result.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        /// <inheritdoc />
        public IDataResult GjiNumberFill(BaseParams baseParams)
        {
            /*
             * Заполнение номера ГЖИ по шаблону: {0000}{11}{22}{3333}, где
             * 0 - год капремонта
             * 1 - код из зональной жилищной инспекции
             * 2 - код ГЖИ муниципального образования
             * 3 - порядковый номер объекта капремонта
             * Всего 12 символов
             */

            var programCrId = baseParams.Params.GetAsId("programCrId");

            var sessionProvider = this.Container.Resolve<ISessionProvider>();

            var objectCrDomain = this.Container.ResolveDomain<Entities.ObjectCr>();
            var programCrDomain = this.Container.ResolveDomain<ProgramCr>();
            var zonalInspMuDomain = this.Container.ResolveDomain<ZonalInspectionMunicipality>();

            using (this.Container.Using(objectCrDomain, programCrDomain, zonalInspMuDomain))
            {
                var programCrYearStr = programCrDomain.Get(programCrId).Period.DateStart.Year.ToString();

                var objectsQuery = objectCrDomain.GetAll()
                    .Where(x =>
                        x.ProgramCr.Id == programCrId && 
                        x.RealityObject.Municipality.CodeGji != null &&
                        x.RealityObject.Municipality.CodeGji != string.Empty);

                var codesDict = zonalInspMuDomain.GetAll()
                    .Where(x =>
                        objectsQuery.Any(y => y.RealityObject.Municipality.Id == x.Municipality.Id) &&
                        x.ZonalInspection.DepartmentCode != null &&
                        x.ZonalInspection.DepartmentCode != string.Empty)
                    .AsEnumerable()
                    .Select(x => new
                    {
                        MunicipalityId = x.Municipality.Id,
                        Codes = new
                        {
                            DepartmentCode = this.GetFormattedNum(x.ZonalInspection.DepartmentCode, 2),
                            CodeGji = this.GetFormattedNum(x.Municipality.CodeGji, 2)
                        }
                    })
                    .GroupBy(x => x.MunicipalityId)
                    .Where(x => x.Count() == 1)
                    .ToDictionary(x => x.Key,
                        y => y.First().Codes);

                objectsQuery = objectsQuery.Where(x => codesDict.Keys.Contains(x.RealityObject.Municipality.Id));

                // Последние объекты капремонта с заполненными по шаблону номерами ГЖИ
                var objectCrWithFillGjiNumsDict = objectsQuery
                    .Where(x => x.GjiNum != null && x.GjiNum.Length == 12)
                    .Select(x => new
                    {
                        MunicipalityId = x.RealityObject.Municipality.Id,
                        x.GjiNum
                    })
                    .OrderBy(x => x.GjiNum)
                    .AsEnumerable()
                    .Where(x =>
                    {
                        // Год капремонта
                        var existProgramCrYear = x.GjiNum.Substring(0, 4);
                        // Код из ЗЖИ
                        var existDepartmentCode = x.GjiNum.Substring(4, 2);
                        // Код ГЖИ из МО
                        var existCodeGji = x.GjiNum.Substring(6, 2);
                        // Порядковый номер объекта капремонта
                        var existIndex = x.GjiNum.Substring(8, 4);
                        var codes = codesDict[x.MunicipalityId];

                        // Учитываются только те номера ГЖИ, которые
                        // заполнены по текущему году капремонта, кодам из МО и ЗЖИ
                        // порядковый номер объекта можно правильно инкрементировать
                        return programCrYearStr == existProgramCrYear && int.TryParse(existIndex, out var index) &&
                            codes.DepartmentCode == existDepartmentCode && codes.CodeGji == existCodeGji;
                    })
                    .GroupBy(x => x.MunicipalityId)
                    .ToDictionary(x => x.Key, y => y.Last().GjiNum);

                var groupedObjectCr = objectsQuery
                    .Where(x => x.GjiNum == null || x.GjiNum == string.Empty)
                    .OrderBy(x => x.Id)
                    .AsEnumerable()
                    .GroupBy(x => x.RealityObject.Municipality.Id);

                var objectCrToSave = new List<Entities.ObjectCr>();
                var session = sessionProvider.GetCurrentSession();

                foreach (var groupObjectCr in groupedObjectCr)
                {
                    var objectCrIndex = 1;

                    if (objectCrWithFillGjiNumsDict.TryGetValue(groupObjectCr.Key, out var lastGjiNum))
                    {
                        var indexStr = lastGjiNum.Substring(8, 4);
                        objectCrIndex += int.Parse(indexStr);
                    }

                    var codes = codesDict[groupObjectCr.Key];

                    foreach (var objectCr in groupObjectCr)
                    {
                        objectCr.GjiNum =
                            $"{this.GetFormattedNum(programCrYearStr, 4)}" +
                            $"{codes.DepartmentCode, 2}" +
                            $"{codes.CodeGji, 2}" +
                            $"{this.GetFormattedNum(objectCrIndex, 4)}";

                        objectCrToSave.Add(objectCr);
                        session.Evict(objectCr);

                        objectCrIndex++;
                    }
                }

                if (objectCrToSave.Any())
                {
                    TransactionHelper.InsertInManyTransactions(this.Container, objectCrToSave, useStatelessSession: true);
                }

                return new BaseDataResult();
            }
        }

        /// <summary>
        /// Получить номер для формата
        /// </summary>
        /// <remarks>
        /// Недостающие символы заменяются "0", а лишние справа отбрасываются
        /// </remarks>
        private string GetFormattedNum(object num, int numLength)
        {
            if (!(num is string result))
            {
                result = num.ToString();
            }

            if (result.Length < numLength)
            {
                var stringBuilder = new StringBuilder();

                for (var i = 0; i < numLength - result.Length; i++)
                {
                    stringBuilder.Append("0");
                }

                stringBuilder.Append(result);

                result = stringBuilder.ToString();
            }
            else if (result.Length > numLength)
            {
                result = result.Substring(0, numLength);
            }

            return result;
        }
    }
}
