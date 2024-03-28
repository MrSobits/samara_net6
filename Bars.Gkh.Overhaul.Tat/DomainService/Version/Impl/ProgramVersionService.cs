namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using Converter = Bars.B4.DomainService.BaseParams.Converter;

    public class ProgramVersionService : IProgramVersionService
    {
        public IWindsorContainer Container { get; set; }
        public IDomainService<ProgramVersion> ProgramVersionDomain { get; set; }
        public IDomainService<State> StateDomain { get; set; }
        public IAuthorizationService AuthorizationService { get; set; }
        public IUserIdentity UserIdentity { get; set; }
        public IFileService FileService { get; set; }

        private static T ToDerived<T>(T tBase, IEnumerable<PropertyInfo> propertyes)
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

        public virtual IDataResult CopyProgram(BaseParams baseParams)
        {
            const string permissionName = "Ovrhl.ProgramVersion.Copy";
            var oldProgramVersionId = baseParams.Params.GetAs("versionId", 0L);
            var programVersion = ProgramVersionDomain.FirstOrDefault(x => x.Id == oldProgramVersionId);
            var newProgramVersion = new ProgramVersion();

            if (programVersion == null)
            {
                return new BaseDataResult(false, "Не удалось получить версию программы");
            }

            if (!AuthorizationService.Grant(UserIdentity, permissionName))
            {
                return new BaseDataResult(false, "Копирование версии запрещено правами доступа");
            }

            var name = baseParams.Params["Name"].ToStr();
            var versionDate = baseParams.Params["VersionDate"].ToDateTime();

            var programVersionRepo = Container.Resolve<IRepository<ProgramVersion>>();
            var subsidyRecordVersionRepo = Container.Resolve<IRepository<SubsidyRecordVersion>>();
            var subsidyRecordVersionDataRepo = Container.Resolve<IRepository<SubsidyRecordVersionData>>();
            var dpkrCorrectionStage2Repo = Container.Resolve<IRepository<DpkrCorrectionStage2>>();
            var typeWorkCrVersionStage1Repo = Container.Resolve<IRepository<TypeWorkCrVersionStage1>>();
            var versionRecordStage1Repo = Container.Resolve<IRepository<VersionRecordStage1>>();
            var versionRecordStage2Repo = Container.Resolve<IRepository<VersionRecordStage2>>();
            var versionRecordRepo = Container.Resolve<IRepository<VersionRecord>>();
            var versionParamRepo = Container.Resolve<IRepository<VersionParam>>();
            var publishedProgramRepo = Container.Resolve<IRepository<PublishedProgram>>();
            var publishedProgramRecordRepo = Container.Resolve<IRepository<PublishedProgramRecord>>();
            var shortProgramRoRepo = Container.Resolve<IRepository<ShortProgramRealityObject>>();
            var shortProgramRecordRepo = Container.Resolve<IRepository<ShortProgramRecord>>();
            var shortProgramProtRepo = Container.Resolve<IRepository<ShortProgramProtocol>>();
            var shortProgramDefectListRepo = Container.Resolve<IRepository<ShortProgramDefectList>>();

            try
            {
                var subsidyRecordVersionProperies =
                    typeof(SubsidyRecordVersion).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var subsidyRecordVersionDataProperies =
                    typeof(SubsidyRecordVersionData).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var dpkrCorrectionStage2Properies =
                    typeof(DpkrCorrectionStage2).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var typeWorkCrVersionStage1Properies =
                    typeof(TypeWorkCrVersionStage1).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var versionRecordStage1Properies =
                    typeof(VersionRecordStage1).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var versionRecordStage2Properies =
                    typeof(VersionRecordStage2).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var versionRecordProperies =
                    typeof(VersionRecord).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var versionParamProperies =
                    typeof(VersionParam).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var publishedProgramProperies =
                    typeof(PublishedProgram).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var publishedProgramRecordProperies =
                    typeof(PublishedProgramRecord).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var shortProgramRoProperties =
                    typeof(ShortProgramRealityObject).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var shortProgramRecordProperties =
                    typeof(ShortProgramRecord).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var shortProgramProtProperties =
                    typeof(ShortProgramProtocol).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var shortProgramDefectListProperties =
                    typeof(ShortProgramDefectList).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                // Списки для сохранения
                var subsidyRecordVersionSaveList = new List<SubsidyRecordVersion>();
                var subsidyRecordVersionDataSaveList = new List<SubsidyRecordVersionData>();
                var dpkrCorrectionStage2SaveList = new List<DpkrCorrectionStage2>();
                var typeWorkCrVersionStage1SaveList = new List<TypeWorkCrVersionStage1>();
                var versionRecordStage1SaveList = new List<VersionRecordStage1>();
                var versionRecordStage2SaveList = new List<VersionRecordStage2>();
                var versionRecordSaveList = new List<VersionRecord>();
                var versionParamSaveList = new List<VersionParam>();
                var publishedProgramSaveList = new List<PublishedProgram>();
                var publishedProgramRecordSaveList = new List<PublishedProgramRecord>();
                var shortProgramRoSaveList = new List<ShortProgramRealityObject>();
                var shortProgramRecordSaveList = new List<ShortProgramRecord>();
                var shortProgramProtSaveList = new List<ShortProgramProtocol>();
                var shortProgramDefectListSaveList = new List<ShortProgramDefectList>();

                var versionRecordStage2Dict =
                    versionRecordStage2Repo.GetAll()
                        .Where(x => x.Stage3Version.ProgramVersion.Id == oldProgramVersionId)
                        .GroupBy(x => x.Stage3Version.Id)
                        .ToDictionary(x => x.Key, y => y.ToList());
                var publishedProgramRecordDict =
                    publishedProgramRecordRepo.GetAll()
                        .Where(x => x.Stage2.Stage3Version.ProgramVersion.Id == oldProgramVersionId)
                        .Where(x => x.PublishedProgram.ProgramVersion.Id == oldProgramVersionId)
                        .GroupBy(x => x.Stage2.Id)
                        .ToDictionary(x => x.Key, y => y.ToList());
                var dpkrCorrectionStage2Dict =
                    dpkrCorrectionStage2Repo.GetAll()
                        .Where(x => x.Stage2.Stage3Version.ProgramVersion.Id == oldProgramVersionId)
                        .GroupBy(x => x.Stage2.Id)
                        .ToDictionary(x => x.Key, y => y.ToList());
                var versionRecordStage1Dict =
                    versionRecordStage1Repo.GetAll()
                        .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == oldProgramVersionId)
                        .GroupBy(x => x.Stage2Version.Id)
                        .ToDictionary(x => x.Key, y => y.ToList());
                var typeWorkCrVersionStage1Dict =
                    typeWorkCrVersionStage1Repo.GetAll()
                        .Where(
                            x => x.Stage1Version.Stage2Version.Stage3Version.ProgramVersion.Id == oldProgramVersionId)
                        .GroupBy(x => x.Stage1Version.Id)
                        .ToDictionary(x => x.Key, y => y.ToList());
                var shortProgramRecordDict =
                    shortProgramRecordRepo.GetAll()
                        .Where(x => x.ShortProgramObject.ProgramVersion.Id == oldProgramVersionId)
                        .GroupBy(x => x.ShortProgramObject.Id)
                        .ToDictionary(x => x.Key, y => y.ToList());
                var shortProgramProtDict =
                    shortProgramProtRepo.GetAll()
                        .Where(x => x.ShortObject.ProgramVersion.Id == oldProgramVersionId)
                        .GroupBy(x => x.ShortObject.Id)
                        .ToDictionary(x => x.Key, y => y.ToList());
                var shortProgramDefectListDict =
                    shortProgramDefectListRepo.GetAll()
                        .Where(x => x.ShortObject.ProgramVersion.Id == oldProgramVersionId)
                        .GroupBy(x => x.ShortObject.Id)
                        .ToDictionary(x => x.Key, y => y.ToList());

                // Проверяем существует ли начальный статус и, если нет, то создаем новый
                var firstState =
                    StateDomain.GetAll().FirstOrDefault(x => x.TypeId == "ovrhl_program_version" && x.StartState);

                if (firstState == null)
                {
                    firstState = new State
                                     {
                                         Name = "Черновик",
                                         Code = "Черновик",
                                         StartState = true,
                                         TypeId = "ovrhl_program_version"
                                     };

                    StateDomain.Save(firstState);
                }

                // Версия программы
                newProgramVersion = new ProgramVersion
                {
                    Id = 0,
                    Name = name,
                    Municipality = programVersion.Municipality,
                    VersionDate = versionDate,
                    IsMain = false,
                    State = firstState
                };

                // PublishedProgram
                var oldNewPublishProgramDict = new Dictionary<long, PublishedProgram>();
                var oldNewVersionRecordStage1Dict = new Dictionary<long, VersionRecordStage1>();

                var publishedProgramList =
                    publishedProgramRepo.GetAll().Where(x => x.ProgramVersion.Id == oldProgramVersionId).ToList();

                foreach (var publishedProgram in publishedProgramList)
                {
                    var newPublishedProgram = ToDerived(publishedProgram, publishedProgramProperies);
                    newPublishedProgram.Id = 0;
                    newPublishedProgram.ProgramVersion = newProgramVersion;

                    publishedProgramSaveList.Add(newPublishedProgram);
                    oldNewPublishProgramDict.Add(publishedProgram.Id, newPublishedProgram);
                }

                // SubsidyRecordVersion
                var subsidyRecordVersionList =
                    subsidyRecordVersionRepo.GetAll().Where(x => x.Version.Id == oldProgramVersionId).ToList();
                foreach (var item in subsidyRecordVersionList)
                {
                    var newSubRecVer = ToDerived(item, subsidyRecordVersionProperies);
                    newSubRecVer.Id = 0;
                    newSubRecVer.Version = newProgramVersion;

                    subsidyRecordVersionSaveList.Add(newSubRecVer);
                }

                // SubsidyRecordVersionData
                var subsidyRecordVersionDataList =
                    subsidyRecordVersionDataRepo.GetAll().Where(x => x.Version.Id == oldProgramVersionId).ToList();
                foreach (var item in subsidyRecordVersionDataList)
                {
                    var newSubRecVerData = ToDerived(item, subsidyRecordVersionDataProperies);
                    newSubRecVerData.Id = 0;
                    newSubRecVerData.Version = newProgramVersion;

                    subsidyRecordVersionDataSaveList.Add(newSubRecVerData);
                }

                // VersionRecord
                var versionRecordList =
                    versionRecordRepo.GetAll().Where(x => x.ProgramVersion.Id == oldProgramVersionId).ToList();
                foreach (var item in versionRecordList)
                {
                    var newVersionRecData = ToDerived(item, versionRecordProperies);
                    newVersionRecData.Id = 0;
                    newVersionRecData.ProgramVersion = newProgramVersion;

                    versionRecordSaveList.Add(newVersionRecData);

                    // VersionRecordStage2
                    if (versionRecordStage2Dict.ContainsKey(item.Id))
                    {
                        var versionRecordStage2List = versionRecordStage2Dict[item.Id];
                        foreach (var versionRecordStage2 in versionRecordStage2List)
                        {
                            var newVersionRecordStage2 = ToDerived(versionRecordStage2, versionRecordStage2Properies);
                            newVersionRecordStage2.Id = 0;
                            newVersionRecordStage2.Stage3Version = newVersionRecData;
                            versionRecordStage2SaveList.Add(newVersionRecordStage2);

                            // PublishedProgramRecord
                            if (publishedProgramRecordDict.ContainsKey(versionRecordStage2.Id))
                            {
                                var publishedProgramRecordList = publishedProgramRecordDict[versionRecordStage2.Id];
                                foreach (var publishedProgramRecord in publishedProgramRecordList)
                                {
                                    var oldPublishProgramId = publishedProgramRecord.PublishedProgram.Id;

                                    var newPblishedProgramRecord = ToDerived(
                                        publishedProgramRecord,
                                        publishedProgramRecordProperies);
                                    newPblishedProgramRecord.Id = 0;
                                    newPblishedProgramRecord.Stage2 = newVersionRecordStage2;
                                    newPblishedProgramRecord.PublishedProgram =
                                        oldNewPublishProgramDict.ContainsKey(oldPublishProgramId)
                                            ? oldNewPublishProgramDict[oldPublishProgramId]
                                            : null;
                                    publishedProgramRecordSaveList.Add(newPblishedProgramRecord);
                                }
                            }

                            // DpkrCorrectionStage2
                            if (dpkrCorrectionStage2Dict.ContainsKey(versionRecordStage2.Id))
                            {
                                var dpkrCorrectionStage2List = dpkrCorrectionStage2Dict[versionRecordStage2.Id];
                                foreach (var dpkrCorrectionStage2 in dpkrCorrectionStage2List)
                                {
                                    var newDpkrCorrectionStage2 = ToDerived(
                                        dpkrCorrectionStage2,
                                        dpkrCorrectionStage2Properies);
                                    newDpkrCorrectionStage2.Id = 0;
                                    newDpkrCorrectionStage2.Stage2 = newVersionRecordStage2;
                                    dpkrCorrectionStage2SaveList.Add(newDpkrCorrectionStage2);
                                }
                            }

                            // VersionRecordStage1
                            if (versionRecordStage1Dict.ContainsKey(versionRecordStage2.Id))
                            {
                                var versionRecordStage1List = versionRecordStage1Dict[versionRecordStage2.Id];
                                foreach (var versionRecordStage1 in versionRecordStage1List)
                                {
                                    var newVersionRecordStage1 = ToDerived(
                                        versionRecordStage1,
                                        versionRecordStage1Properies);
                                    newVersionRecordStage1.Id = 0;
                                    newVersionRecordStage1.Stage2Version = newVersionRecordStage2;
                                    versionRecordStage1SaveList.Add(newVersionRecordStage1);

                                    // TypeWorkCrVersionStage1
                                    if (typeWorkCrVersionStage1Dict.ContainsKey(versionRecordStage1.Id))
                                    {
                                        var typeWorkCrVersionStage1List =
                                            typeWorkCrVersionStage1Dict[versionRecordStage1.Id];
                                        foreach (var typeWorkCrVersionStage1 in typeWorkCrVersionStage1List)
                                        {
                                            var newTypeWorkCrVersionStage1 = ToDerived(
                                                typeWorkCrVersionStage1,
                                                typeWorkCrVersionStage1Properies);
                                            newTypeWorkCrVersionStage1.Id = 0;
                                            newTypeWorkCrVersionStage1.Stage1Version = newVersionRecordStage1;
                                            typeWorkCrVersionStage1SaveList.Add(newTypeWorkCrVersionStage1);
                                        }
                                    }

                                    oldNewVersionRecordStage1Dict.Add(versionRecordStage1.Id, newVersionRecordStage1);
                                }
                            }
                        }
                    }
                }

                // ShortProgramRealityObject
                var shortProgramRoList =
                    shortProgramRoRepo.GetAll().Where(x => x.ProgramVersion.Id == oldProgramVersionId).ToList();
                foreach (var shortProgramRo in shortProgramRoList)
                {
                    var newShortProgramRo = ToDerived(shortProgramRo, shortProgramRoProperties);
                    newShortProgramRo.Id = 0;
                    newShortProgramRo.ProgramVersion = newProgramVersion;

                    shortProgramRoSaveList.Add(newShortProgramRo);

                    // ShortProgramDefectList
                    if (shortProgramRecordDict.ContainsKey(shortProgramRo.Id))
                    {
                        var shortProgramRecordList = shortProgramRecordDict[shortProgramRo.Id];
                        foreach (var shortProgramRecord in shortProgramRecordList)
                        {
                            var oldVersionRecordStage1Id = shortProgramRecord.Stage1 != null 
                                ? shortProgramRecord.Stage1.Id
                                : 0;

                            var newShortProgramRecord = ToDerived(shortProgramRecord, shortProgramRecordProperties);
                            newShortProgramRecord.Id = 0;
                            newShortProgramRecord.ShortProgramObject = newShortProgramRo;
                            newShortProgramRecord.Stage1 =
                                oldNewVersionRecordStage1Dict.ContainsKey(oldVersionRecordStage1Id)
                                    ? oldNewVersionRecordStage1Dict[oldVersionRecordStage1Id]
                                    : null;
                            shortProgramRecordSaveList.Add(newShortProgramRecord);
                        }
                    }

                    // ShortProgramProtocol
                    if (shortProgramProtDict.ContainsKey(shortProgramRo.Id))
                    {
                        var shortProgramProtList = shortProgramProtDict[shortProgramRo.Id];
                        foreach (var shortProgramProt in shortProgramProtList)
                        {
                            var newShortProgramProt = ToDerived(shortProgramProt, shortProgramProtProperties);
                            newShortProgramProt.Id = 0;
                            newShortProgramProt.ShortObject = newShortProgramRo;
                            newShortProgramProt.File = FileService.ReCreateFile(newShortProgramProt.File);
                            shortProgramProtSaveList.Add(newShortProgramProt);
                        }
                    }

                    // ShortProgramDefectList
                    if (shortProgramDefectListDict.ContainsKey(shortProgramRo.Id))
                    {
                        var shortProgramDefectList = shortProgramDefectListDict[shortProgramRo.Id];
                        foreach (var shortProgramDefect in shortProgramDefectList)
                        {
                            var newShortProgramDefect = ToDerived(shortProgramDefect, shortProgramDefectListProperties);
                            newShortProgramDefect.Id = 0;
                            newShortProgramDefect.ShortObject = newShortProgramRo;
                            newShortProgramDefect.File = FileService.ReCreateFile(newShortProgramDefect.File);
                            shortProgramDefectListSaveList.Add(newShortProgramDefect);
                        }
                    }
                }

                // VersionParam
                var versionParamList =
                    versionParamRepo.GetAll().Where(x => x.ProgramVersion.Id == oldProgramVersionId).ToList();
                foreach (var item in versionParamList)
                {
                    var newVersionParamData = ToDerived(item, versionParamProperies);
                    newVersionParamData.Id = 0;
                    newVersionParamData.ProgramVersion = newProgramVersion;

                    versionParamSaveList.Add(newVersionParamData);
                }

                // Сохранение
                newProgramVersion.CopyingState = ProgramVersionCopyingState.InProcess;
                programVersionRepo.Save(newProgramVersion);

                TransactionHelper.InsertInManyTransactions(Container, publishedProgramSaveList, 1000, true, true);
                TransactionHelper.InsertInManyTransactions(Container, subsidyRecordVersionSaveList, 1000, true, true);
                TransactionHelper.InsertInManyTransactions(Container, subsidyRecordVersionDataSaveList, 1000, true, true);
                TransactionHelper.InsertInManyTransactions(Container, versionParamSaveList, 1000, true, true);
                TransactionHelper.InsertInManyTransactions(Container, versionRecordSaveList, 1000, true, true);
                TransactionHelper.InsertInManyTransactions(Container, versionRecordStage2SaveList, 1000, true, true);
                TransactionHelper.InsertInManyTransactions(Container, publishedProgramRecordSaveList, 1000, true, true);
                TransactionHelper.InsertInManyTransactions(Container, dpkrCorrectionStage2SaveList, 1000, true, true);
                TransactionHelper.InsertInManyTransactions(Container, versionRecordStage1SaveList, 1000, true, true);
                TransactionHelper.InsertInManyTransactions(Container, typeWorkCrVersionStage1SaveList, 1000, true, true);
                TransactionHelper.InsertInManyTransactions(Container, shortProgramRoSaveList, 1000, true, true);
                TransactionHelper.InsertInManyTransactions(Container, shortProgramRecordSaveList, 1000, true, true);
                TransactionHelper.InsertInManyTransactions(Container, shortProgramProtSaveList, 1000, true, true);
                TransactionHelper.InsertInManyTransactions(Container, shortProgramDefectListSaveList, 1000, true, true);

                newProgramVersion.CopyingState = ProgramVersionCopyingState.Success;
                programVersionRepo.Save(newProgramVersion);
                return new BaseDataResult { Success = true };
            }
            catch (Exception exc)
            {
                newProgramVersion.CopyingState = ProgramVersionCopyingState.CompletedWithError;
                programVersionRepo.Save(newProgramVersion);
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
            finally
            {
                Container.Release(programVersionRepo);
                Container.Release(subsidyRecordVersionRepo);
                Container.Release(subsidyRecordVersionDataRepo);
                Container.Release(dpkrCorrectionStage2Repo);
                Container.Release(typeWorkCrVersionStage1Repo);
                Container.Release(versionRecordStage1Repo);
                Container.Release(versionRecordStage2Repo);
                Container.Release(versionRecordRepo);
                Container.Release(versionParamRepo);
                Container.Release(publishedProgramRecordRepo);
            }
        }

        public IDataResult GetMainVersionByMunicipality(BaseParams baseParams)
        {
            var muId = baseParams.Params.GetAs<long>("muId");

            var version = ProgramVersionDomain.GetAll().FirstOrDefault(x => x.IsMain && x.Municipality.Id == muId);

            return version == null
                ? new BaseDataResult(false, "В выбранном муниципальном образовании не указана основная версия")
                : new BaseDataResult(version);
        }

        public IDataResult GetDeletedEntriesList(BaseParams baseParams)
        {
            var actualizeService = Container.Resolve<IActualizeVersionService>();

            try
            {
                var loadParams = GetLoadParam(baseParams);

                var version = loadParams.Filter.GetAs<long>("versionId");
                var yearStart = loadParams.Filter.GetAs("yearStart", 0);

                // Теперь получаем те записи 3го этапа которые есть в списке полученных из 1го этапа
                var result = actualizeService.GetDeletedEntriesQueryable(version, yearStart)
                    .Select(x => new
                    {
                        x.Stage2Version.Stage3Version.Id,
                        RealityObject = x.Stage2Version.Stage3Version.RealityObject.Address,
                        x.Stage2Version.Stage3Version.CommonEstateObjects,
                        x.Stage2Version.Stage3Version.Year,
                        x.Stage2Version.Stage3Version.IndexNumber,
                        x.Stage2Version.Stage3Version.Sum,
                        x.Stage2Version.Stage3Version.CorrectYear
                    })
                    .ToList()
                    .Distinct(x => x.Id)
                    .AsQueryable()
                    .Filter(loadParams, Container)
                    .OrderBy(x => x.IndexNumber);

                return new ListDataResult(result.Order(loadParams).Paging(loadParams).ToList(), result.Count());
            }
            finally
            {
                Container.Release(actualizeService);
            }
        }

        /// <inheritdoc />
        public IDataResult ListMainVersions(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var muIds = baseParams.Params.GetAs<string>("municipalityId").ToLongArray();

            return this.ProgramVersionDomain.GetAll()
                .Where(x => x.IsMain)
                .WhereIfContains(muIds.IsNotEmpty(), x => x.Municipality.Id, muIds)
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    Municipality = x.Municipality.Name,
                    x.Name,
                    x.VersionDate,
                    x.IsMain,
                    CopyingState = x.CopyingState == ProgramVersionCopyingState.NotCopied
                        ? string.Empty
                        : x.CopyingState.GetEnumMeta().Display
                })
                .ToListDataResult(loadParams);
        }

        protected virtual LoadParam GetLoadParam(BaseParams baseParams)
        {
            return baseParams.Params.Read<LoadParam>().Execute(Converter.ToLoadParam);
        }
    }
}