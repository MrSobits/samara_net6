namespace Bars.Gkh.Overhaul.Tat.DomainService
{
    using System.Data;
    using System.Reflection;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.GkhCr.DomainService;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Microsoft.Extensions.Logging;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Castle.Windsor;
    using NHibernate.Linq;

    public class ProgramCrCopByDpkr : IProgramCrCopByDpkr
    {
        public IWindsorContainer Container { get; set; }
        public IFileService FileService { get; set; }
        public IUserIdentity user { get; set; }

        private T ToDerived<T>(T tBase, IEnumerable<PropertyInfo> propertyes)
                where T : class, IEntity, new()
        {
            var tDerived = new T();
            foreach (var propBase in propertyes)
            {
                try
                {
                    propBase.SetValue(tDerived, propBase.GetValue(tBase, null), null);
                }
                catch
                {
                    // ignored
                }
            }
            return tDerived;
        }

        private IEnumerable<PropertyInfo> GetPropertiesForCopy(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.CanRead && x.CanWrite);
        }

        // Метод копирования программы с учетом того что она создана из ДПКР
        public IDataResult CopyProgramCr(BaseParams baseParams)
        {
            var logger = this.Container.Resolve<ILogger>();

            logger.LogDebug("Копирование программы | инициализация параметров");
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

            logger.LogDebug("Копирование программы | получение сервисов/репозиториев");
            // Используется Repository, что бы не вызывался интерцептор
            var objectCrService = this.Container.Resolve<IRepository<ObjectCr>>();
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
            var typeWorkCrStage1Service = this.Container.Resolve<IRepository<TypeWorkCrVersionStage1>>();
            var programCrJournalService = this.Container.Resolve<IRepository<ProgramCrChangeJournal>>();

            try
            {
                logger.LogDebug("Копирование программы | получение списка свойств");
                var journalProperies = this.GetPropertiesForCopy(typeof(ProgramCrChangeJournal));
                var finProperies = this.GetPropertiesForCopy(typeof(ProgramCrFinSource));
                var objectCrProperies = this.GetPropertiesForCopy(typeof(ObjectCr));
                var qualProperies = this.GetPropertiesForCopy(typeof(Qualification));
                var buildContractProperies = this.GetPropertiesForCopy(typeof(BuildContract))
                    .WhereIf(copyWithoutAttachments, x => new[]{nameof(BuildContract.DocumentFile), nameof(BuildContract.ProtocolFile)}.All(y => y != x.Name));
                var contractCrProperies = this.GetPropertiesForCopy(typeof(ContractCr));
                var defectListProperies = this.GetPropertiesForCopy(typeof(DefectList))
                    .WhereIf(copyWithoutAttachments, x => x.Name != nameof(DefectList.File));
                var documentWorkCrProperies = this.GetPropertiesForCopy(typeof(DocumentWorkCr))
                    .WhereIf(copyWithoutAttachments, x => x.Name != nameof(DocumentWorkCr.File));
                var financeSourceResourceProperies = this.GetPropertiesForCopy(typeof(FinanceSourceResource));
                var monitoringSmrProperies = this.GetPropertiesForCopy(typeof(MonitoringSmr));
                var personalAccountProperies = this.GetPropertiesForCopy(typeof(PersonalAccount));
                var protocolCrProperies = this.GetPropertiesForCopy(typeof(ProtocolCr))
                    .WhereIf(copyWithoutAttachments, x => x.Name != nameof(ProtocolCr.File));
                var typeWorkCrProperies = this.GetPropertiesForCopy(typeof(TypeWorkCr));
                var performedWorkActProperies = this.GetPropertiesForCopy(typeof(PerformedWorkAct))
                    .WhereIf(copyWithoutAttachments, x => new[]{nameof(PerformedWorkAct.DocumentFile), nameof(PerformedWorkAct.CostFile), nameof(PerformedWorkAct.AdditionFile)}.All(y => y != x.Name));
                var performedWorkActRecProperies = this.GetPropertiesForCopy(typeof(PerformedWorkActRecord));
                var referenceProperies = this.GetPropertiesForCopy(typeof(TypeWorkCrVersionStage1));
                var voiceMemberProperies = this.GetPropertiesForCopy(typeof(VoiceMember));

                // Списки для сохранения
                var objectCrSaveList = new List<ObjectCr>();
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
                var typeWorkStage1SaveList = new List<TypeWorkCrVersionStage1>();

                logger.LogDebug("Копирование программы | кэширование данных");
                var qualificationDict = qualificationService.GetAll().Where(x => x.ObjectCr.ProgramCr.Id == oldProgramCrId).GroupBy(x => x.ObjectCr.Id).ToDictionary(x => x.Key, y => y.ToList());
                var buildContractDict = buildContractService.GetAll().Where(x => x.ObjectCr.ProgramCr.Id == oldProgramCrId).Fetch(x => x.DocumentFile).Fetch(x => x.ProtocolFile).ToList().GroupBy(x => x.ObjectCr.Id).ToDictionary(x => x.Key, y => y.ToList());
                var contractCrDict = contractCrService.GetAll().Where(x => x.ObjectCr.ProgramCr.Id == oldProgramCrId).Fetch(x => x.File).ToList().GroupBy(x => x.ObjectCr.Id).ToDictionary(x => x.Key, y => y.ToList());
                var financeSourceResourceDict = financeSourceResourceService.GetAll().Where(x => x.ObjectCr.ProgramCr.Id == oldProgramCrId).GroupBy(x => x.ObjectCr.Id).ToDictionary(x => x.Key, y => y.ToList());
                var documentWorkCrDict = documentWorkCrService.GetAll().Where(x => x.ObjectCr.ProgramCr.Id == oldProgramCrId).GroupBy(x => x.ObjectCr.Id).ToDictionary(x => x.Key, y => y.ToList());
                var monitoringSmrDict = monitoringSmrService.GetAll().Where(x => x.ObjectCr.ProgramCr.Id == oldProgramCrId).GroupBy(x => x.ObjectCr.Id).ToDictionary(x => x.Key, y => y.ToList());
                var personalAccountDict = personalAccountService.GetAll().Where(x => x.ObjectCr.ProgramCr.Id == oldProgramCrId).GroupBy(x => x.ObjectCr.Id).ToDictionary(x => x.Key, y => y.ToList());
                var protocolCrDict = protocolCrService.GetAll().Where(x => x.ObjectCr.ProgramCr.Id == oldProgramCrId).Fetch(x => x.File).ToList().GroupBy(x => x.ObjectCr.Id).ToDictionary(x => x.Key, y => y.ToList());

                var typeWorkCrDict = typeWorkCrService.GetAll().Where(x => x.ObjectCr.ProgramCr.Id == oldProgramCrId)
                    .Select(x => new TypeWorkCr
                    {
                        Id = x.Id,
                        ExternalId = x.ExternalId,
                        ObjectCr = x.ObjectCr,
                        FinanceSource = x.FinanceSource,
                        Work = x.Work,
                        StageWorkCr = x.StageWorkCr,
                        HasPsd = x.HasPsd,
                        Volume = x.Volume,
                        SumMaterialsRequirement = x.SumMaterialsRequirement,
                        Sum = x.Sum,
                        Description = x.Description,
                        DateStartWork = x.DateStartWork,
                        DateEndWork = x.DateEndWork,
                        VolumeOfCompletion = x.VolumeOfCompletion,
                        ManufacturerName = x.ManufacturerName,
                        PercentOfCompletion = x.PercentOfCompletion,
                        CostSum = x.CostSum,
                        CountWorker = x.CountWorker,
                        AdditionalDate = x.AdditionalDate,
                        YearRepair = x.YearRepair,
                        IsActive = x.IsActive,
                        IsDpkrCreated = x.IsDpkrCreated,
                        State = x.State
                    }).ToList().GroupBy(x => x.ObjectCr.Id).ToDictionary(x => x.Key, y => y.ToList());
                var typeWorkCrStage1Dict = typeWorkCrStage1Service.GetAll().Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == oldProgramCrId).GroupBy(x => x.TypeWorkCr.Id).ToDictionary(x => x.Key, y => y.ToList());
                var performedWorkActDict = performedWorkActService.GetAll().Where(x => x.ObjectCr.ProgramCr.Id == oldProgramCrId).Fetch(x => x.AdditionFile).Fetch(x => x.CostFile).Fetch(x => x.DocumentFile).ToList().GroupBy(x => x.ObjectCr.Id).ToDictionary(x => x.Key, y => y.ToList());
                var voiceMemberDict = voiceMemberService.GetAll().Where(x => x.Qualification.ObjectCr.ProgramCr.Id == oldProgramCrId).GroupBy(x => x.Qualification.Id).ToDictionary(x => x.Key, y => y.ToList());
                
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

                        logger.LogDebug("Копирование программы | история");
                        var history = programCrJournalService.GetAll().FirstOrDefault(x => x.ProgramCr.Id == oldProgramCrId && x.TypeChange == TypeChangeProgramCr.FromDpkr);
                        var program = programCrService.GetAll().FirstOrDefault(x => x.Id == oldProgramCrId);
                        var defectListDict = defectListService.GetAll()
                            .Where(x => x.ObjectCr.ProgramCr.Id == oldProgramCrId
                                && x.DocumentDate >= newProgramCr.Period.DateStart
                                && (!newProgramCr.Period.DateEnd.HasValue || x.DocumentDate <= newProgramCr.Period.DateEnd))
                            .Fetch(x => x.File).ToList()
                            .GroupBy(x => x.ObjectCr.Id)
                            .ToDictionary(x => x.Key, y => y.ToList());

                        ProgramCrChangeJournal newHistory;

                        if (history != null)
                        {
                            newHistory = this.ToDerived(history, journalProperies);
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
                                TypeChange = TypeChangeProgramCr.FromDpkr,
                                ProgramCr = newProgramCr,
                                UserName = user.Name,
                                Description = string.Format("Скопирована с '{0}'", program.Name)
                            };
                        }

                        // Фин. источники программы
                        logger.LogDebug("Копирование программы | Фин. источники программы");
                        var finSourceProgramCrList = finSourceProgramCrService.GetAll().Where(x => x.ProgramCr.Id == oldProgramCrId).ToList();

                        foreach (var item in finSourceProgramCrList)
                        {
                            var newFinSrc = this.ToDerived(item, finProperies);
                            newFinSrc.Id = 0;
                            newFinSrc.ProgramCr = newProgramCr;

                            programCrFinSourceSaveList.Add(newFinSrc);
                        }

                        logger.LogDebug("Копирование программы | Фин. источники программы, скопировано {0}", programCrFinSourceSaveList.Count);

                        // Объекты КР программы
                        logger.LogDebug("Копирование программы | Объекты КР программы");
                        var objectCrList = objectCrService.GetAll().Where(x => x.ProgramCr.Id == oldProgramCrId).ToList();
                        foreach (var objectCr in objectCrList)
                        {
                            var newObjectCr = this.ToDerived(objectCr, objectCrProperies);
                            newObjectCr.Id = 0;
                            newObjectCr.ProgramCr = newProgramCr;
                            objectCrSaveList.Add(newObjectCr);

                            // Квалификационный отбор объекта КР
                            if (qualificationDict.ContainsKey(objectCr.Id))
                            {
                                var qualificationList = qualificationDict[objectCr.Id];

                                foreach (var qualification in qualificationList)
                                {
                                    var newQualification = this.ToDerived(qualification, qualProperies);
                                    newQualification.Id = 0;
                                    newQualification.ObjectCr = newObjectCr;
                                    qualificationSaveList.Add(newQualification);

                                    if (voiceMemberDict.ContainsKey(qualification.Id))
                                    {
                                        var voiceMemberList = voiceMemberDict[qualification.Id];

                                        // Голос участника квалиф. отбора
                                        foreach (var voiceMember in voiceMemberList)
                                        {
                                            var newVoiceMember = this.ToDerived(voiceMember, voiceMemberProperies);
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
                                    var newBuildContract = this.ToDerived(buildContract, buildContractProperies);
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
                                    var newContractCr = this.ToDerived(contractCr, contractCrProperies);
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
                                    var newDefect = this.ToDerived(defect, defectListProperies);
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
                                    var newDocumentWorkCr = this.ToDerived(documentWorkCr, documentWorkCrProperies);
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
                                        financeSourceResource, financeSourceResourceProperies);
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
                                    var newMonitoringSmr = this.ToDerived(monitoringSmr, monitoringSmrProperies);
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
                                    var newPersonalAccount = this.ToDerived(personalAccount, personalAccountProperies);
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
                                    var newProtocolCr = this.ToDerived(protocolCr, protocolCrProperies);
                                    newProtocolCr.Id = 0;
                                    newProtocolCr.File = FileService.ReCreateFile(newProtocolCr.File);
                                    newProtocolCr.ObjectCr = newObjectCr;
                                    protocolCrSaveList.Add(newProtocolCr);
                                }
                            }

                            // Вид работы КР
                            if (typeWorkCrDict.ContainsKey(objectCr.Id))
                            {
                                // Делаем частичтное кэширование ибо 5+кк строк слишком быстро съедают память
                                var performedWorkActRecDict = performedWorkActRecService.GetAll()
                                    .Where(x => x.PerformedWorkAct.ObjectCr.Id == objectCr.Id)
                                    .GroupBy(x => x.PerformedWorkAct.Id)
                                    .ToDictionary(x => x.Key, y => y.ToList());

                                var typeWorkCrList = typeWorkCrDict[objectCr.Id];

                                foreach (var typeWorkCr in typeWorkCrList)
                                {
                                    var newTypeWorkCr = this.ToDerived(typeWorkCr, typeWorkCrProperies);
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
                                                performedWorkAct, performedWorkActProperies);
                                            newPerformedWorkAct.Id = 0;
                                            newPerformedWorkAct.TypeWorkCr = newTypeWorkCr;
                                            newPerformedWorkAct.ObjectCr = newObjectCr;
                                            performedWorkActSaveList.Add(newPerformedWorkAct);

                                            // Записи акта вып-х работ объекта КР
                                            if (performedWorkActRecDict.ContainsKey(performedWorkAct.Id))
                                            {
                                                var performedWorkActRecList = performedWorkActRecDict[performedWorkAct.Id];

                                                foreach (var performedWorkActRec in performedWorkActRecList)
                                                {
                                                    var newPerformedWorkActRec = this.ToDerived(
                                                        performedWorkActRec, performedWorkActRecProperies);
                                                    newPerformedWorkActRec.Id = 0;
                                                    newPerformedWorkActRec.PerformedWorkAct = performedWorkAct;
                                                    performedWorkActRecordSaveList.Add(newPerformedWorkActRec);
                                                }
                                            }
                                        }
                                    }

                                    // Тут как раз делается то ради чего был скопирвоан весь этот метод 
                                    if (typeWorkCrStage1Dict.ContainsKey(typeWorkCr.Id))
                                    {
                                        var referenceList = typeWorkCrStage1Dict[typeWorkCr.Id];

                                        foreach (var typeWorkStage1 in referenceList)
                                        {
                                            var newTypeWorkStage1 = this.ToDerived(typeWorkStage1, referenceProperies);
                                            newTypeWorkStage1.Id = 0;
                                            newTypeWorkStage1.TypeWorkCr = newTypeWorkCr;
                                            typeWorkStage1SaveList.Add(newTypeWorkStage1);
                                        }
                                    }
                                }
                            }
                        }
                        logger.LogDebug("Копирование программы | Объекты КР программы, скопировано {0}", objectCrSaveList.Count);
                        // Тут идет типа сохранение

                        var dateCopy = DateTime.Now;

                        newProgramCr.ObjectCreateDate = dateCopy;
                        newProgramCr.ObjectEditDate = dateCopy;
                        newProgramCr.ObjectVersion = 0;
                        logger.LogDebug("Копирование программы | сохранение программы");
                        programCrService.Save(newProgramCr);

                        history.ChangeDate = dateCopy;
                        history.ObjectCreateDate = dateCopy;
                        history.ObjectEditDate = dateCopy;
                        history.ObjectVersion = 0;
                        logger.LogDebug("Копирование программы | сохранение истории");
                        programCrJournalService.Save(newHistory);

                        logger.LogDebug("Копирование программы | сохранение programCrFinSourceSaveList");
                        programCrFinSourceSaveList.ForEach(x =>
                        {
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;
                            x.ObjectVersion = 0;

                            finSourceProgramCrService.Save(x);
                        });
                        logger.LogDebug("Копирование программы | сохранение programCrFinSourceSaveList ({0})", programCrFinSourceSaveList.Count);

                        logger.LogDebug("Копирование программы | сохранение objectCrSaveList");
                        objectCrSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            objectCrService.Save(x);
                        });
                        logger.LogDebug("Копирование программы | сохранение objectCrSaveList ({0})", objectCrSaveList.Count);

                        logger.LogDebug("Копирование программы | сохранение typeWorkCrSaveList");
                        typeWorkCrSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            typeWorkCrService.Save(x);
                        });
                        logger.LogDebug("Копирование программы | сохранение typeWorkCrSaveList ({0})", typeWorkCrSaveList.Count);

                        logger.LogDebug("Копирование программы | сохранение typeWorkStage1SaveList");
                        typeWorkStage1SaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            typeWorkCrStage1Service.Save(x);
                        });
                        logger.LogDebug("Копирование программы | сохранение typeWorkStage1SaveList ({0})", typeWorkStage1SaveList.Count);

                        logger.LogDebug("Копирование программы | сохранение performedWorkActSaveList");
                        performedWorkActSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            performedWorkActService.Save(x);
                        });
                        logger.LogDebug("Копирование программы | сохранение performedWorkActSaveList ({0})", performedWorkActSaveList.Count);

                        logger.LogDebug("Копирование программы | сохранение performedWorkActRecordSaveList");
                        performedWorkActRecordSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            performedWorkActRecService.Save(x);
                        });
                        logger.LogDebug("Копирование программы | сохранение performedWorkActRecordSaveList ({0})", performedWorkActRecordSaveList.Count);

                        logger.LogDebug("Копирование программы | сохранение qualificationSaveList");
                        qualificationSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            qualificationService.Save(x);
                        });
                        logger.LogDebug("Копирование программы | сохранение qualificationSaveList ({0})", qualificationSaveList.Count);

                        logger.LogDebug("Копирование программы | сохранение voiceMemberSaveList");
                        voiceMemberSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            voiceMemberService.Save(x);
                        });
                        logger.LogDebug("Копирование программы | сохранение voiceMemberSaveList ({0})", voiceMemberSaveList.Count);

                        logger.LogDebug("Копирование программы | сохранение buildContractSaveList");
                        buildContractSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            buildContractService.Save(x);
                        });
                        logger.LogDebug("Копирование программы | сохранение buildContractSaveList ({0})", buildContractSaveList.Count);

                        logger.LogDebug("Копирование программы | сохранение contractCrSaveList");
                        contractCrSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            contractCrService.Save(x);
                        });
                        logger.LogDebug("Копирование программы | сохранение contractCrSaveList ({0})", contractCrSaveList.Count);

                        logger.LogDebug("Копирование программы | сохранение defectListSaveList");
                        defectListSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            defectListService.Save(x);
                        });
                        logger.LogDebug("Копирование программы | сохранение defectListSaveList ({0})", defectListSaveList.Count);

                        logger.LogDebug("Копирование программы | сохранение documentWorkCrSaveList");
                        documentWorkCrSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            documentWorkCrService.Save(x);
                        });
                        logger.LogDebug("Копирование программы | сохранение documentWorkCrSaveList ({0})", documentWorkCrSaveList.Count);

                        logger.LogDebug("Копирование программы | сохранение financeSourceResourceSaveList");
                        financeSourceResourceSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            financeSourceResourceService.Save(x);
                        });
                        logger.LogDebug("Копирование программы | сохранение financeSourceResourceSaveList ({0})", financeSourceResourceSaveList.Count);

                        logger.LogDebug("Копирование программы | сохранение monitoringSmrSaveList");
                        monitoringSmrSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            monitoringSmrService.Save(x);
                        });
                        logger.LogDebug("Копирование программы | сохранение monitoringSmrSaveList ({0})", monitoringSmrSaveList.Count);

                        logger.LogDebug("Копирование программы | сохранение personalAccountSaveList");
                        personalAccountSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            personalAccountService.Save(x);
                        });
                        logger.LogDebug("Копирование программы | сохранение personalAccountSaveList ({0})", personalAccountSaveList.Count);

                        logger.LogDebug("Копирование программы | сохранение protocolCrSaveList");
                        protocolCrSaveList.ForEach(x =>
                        {
                            x.ObjectVersion = 0;
                            x.ObjectCreateDate = dateCopy;
                            x.ObjectEditDate = dateCopy;

                            protocolCrService.Save(x);
                        });
                        logger.LogDebug("Копирование программы | сохранение protocolCrSaveList ({0})", protocolCrSaveList.Count);

                        logger.LogDebug("Копирование программы | Commit");
                        tr.Commit();
                        logger.LogDebug("Копирование программы | Завершено успешно");

                        return new BaseDataResult
                        {
                            Success = true
                        };
                    }
                    catch (Exception exc)
                    {
                        logger.LogDebug("Копирование программы | Rollback", exc);
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
                logger.LogDebug("Копирование программы | освобождаем ресурсы");
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
                Container.Release(typeWorkCrStage1Service);
                Container.Release(programCrJournalService);
            }
        }
    }
}
