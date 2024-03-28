namespace Bars.GkhCr.Import
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.FiasHelper;
    using Bars.Gkh.Import.Impl;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Bars.GkhExcel;

    public class ProgramCrImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;
        
        private ILogImport logImport;

        /// <summary>
        /// Заменить этим хелпером старый, и сделать ссылку нормально через DI
        /// </summary>
        private UnstrongFiasHelper fiasHelper; 
        private Dictionary<string, RobjectProxy> robjectByAddressCache;
        private Dictionary<string, List<RealtyObjectAddressProxy>> realityObjectsByFiasGuidDict;
        private Dictionary<long, ObjectCr> objectsCr;
        private Dictionary<long, List<TypeWorkCr>> typeWorks;
        private readonly Dictionary<TypeWorkCr, List<int>> typeWorkYears = new Dictionary<TypeWorkCr, List<int>>();
        private Dictionary<long, string> realObjAddress = new Dictionary<long, string>();
        private HashSet<long> typeWorksWithLinks = new HashSet<long>();
        private bool hasRoIdColumn;

        private Dictionary<string, KeyValuePair<string, long>> fiasIdByMunicipalityNameDict;

        private readonly List<ObjectCr> listToSaveObjectCr = new List<ObjectCr>();

        private readonly List<MonitoringSmr> listToSaveMonitoringSmr = new List<MonitoringSmr>();

        private readonly List<TypeWorkCr> listToSaveTypeWorkCr = new List<TypeWorkCr>();
        private readonly HashSet<long> listToDeleteTypeWorkCr = new HashSet<long>();
        private State objectCrState;

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IRepository<RealityObject> RealityObjectRepository { get; set; }

        public IDomainService<FiasAddress> FiasAddressDomain { get; set; }

        public IDomainService<ProgramCr> ProgramCrDomain { get; set; }

        public IDomainService<ObjectCr> ObjectCrDomain { get; set; }

        public IDomainService<TypeWorkCr> TypeWorkCrDomain { get; set; }

        public IProgramCrImportService ProgramCrImportService { get; set; }

        public IDomainService<ProgramCrChangeJournal> ProgChangeJournalDomain { get; set; }

        public IGkhUserManager UserManager { get; set; }
        
        public IProgramCRImportRealityObject RealityObjectAddresses { get; set; }

        private Dictionary<Work, WorkInfoLines> WorkLines { get; set; }

        public override string Key => ProgramCrImport.Id;

        public override string CodeImport => "ObjectCr";

        public override string Name => "Импорт программы кап.ремонта";

        public override string PossibleFileExtensions => "xls,xlsx";

        public override string PermissionName => "Import.ProgramCrImport.View";


        public ProgramCrImport(ILogImportManager logManager, ILogImport logImport)
        {
            this.LogImportManager = logManager;
            this.LogImport = logImport;
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var file = baseParams.Files["FileImport"];
            var extention = baseParams.Files["FileImport"].Extention;
            var programCrId = baseParams.Params.GetAsId("ProgramCr");
            var financeCut = baseParams.Params.GetAsId("FinanceCut");
            var hasLinkOverhaul = baseParams.Params.GetAs<bool>("HasLinkOverhaul");
            var replaceObjectCr = baseParams.Params.GetAs<bool>("ReplaceObjectCr");
            var replaceTypeWork = baseParams.Params.GetAs<bool>("ReplaceTypeWork");

            var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");

            using (this.Container.Using(excel))
            {
                if (extention == "xlsx")
                {
                    excel.UseVersionXlsx();
                }

                using (var memoryStreamFile = new MemoryStream(file.Data))
                {
                    memoryStreamFile.Seek(0, SeekOrigin.Begin);

                    excel.Open(memoryStreamFile);

                    var rows = excel.GetRows(0, 0);

                    this.hasRoIdColumn = rows[0][1].Value.Trim().ToUpper() == "ID ДОМА";

                    this.InitHeader();

                    this.InitLog(file.FileName);

                    this.InitDictionaries(programCrId);

                    this.InitWorkInfoLines(rows[0]);

                    for (int i = 1; i < rows.Count; i++)
                    {
                        if (String.IsNullOrEmpty(rows[i][1].Value.Trim()))
                            continue;

                        var roId = this.GetRealObjId(rows[i], i, this.hasRoIdColumn);

                        if (roId == 0)
                        {
                            continue;
                        }

                        var objectCr = this.GetObjectCr(roId, programCrId);

                        this.SaveTypeWorkCr(rows[i], objectCr, replaceTypeWork, replaceObjectCr, financeCut);
                    }

                    this.SaveData();

                    if (hasLinkOverhaul && this.ProgramCrImportService != null)
                    {
                        this.ProgramCrImportService.SaveDpkrLink(this.LogImport, this.typeWorkYears);

                        var user = this.UserManager.GetActiveOperator();

                        this.ProgChangeJournalDomain.Save(
                            new ProgramCrChangeJournal
                            {
                                ProgramCr = this.ProgramCrDomain.Load(programCrId),
                                ChangeDate = DateTime.Now,
                                TypeChange = TypeChangeProgramCr.FromDpkr,
                                UserName = user != null ? user.Name : "Администратор"
                            });
                    }

                    fiasHelper.ClearCache();
                }
            }

            this.LogImportManager.Add(file, this.LogImport);
            this.LogImportManager.Save();

            var message = this.LogImportManager.GetInfo();
            var status = this.LogImportManager.CountError > 0
                ? StatusImport.CompletedWithError
                : (this.LogImportManager.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
            return new ImportResult(status, message, string.Empty, this.LogImportManager.LogFileId);
        }

        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;
            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            var file = baseParams.Files["FileImport"];
            var extention = baseParams.Files["FileImport"].Extention;

            var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
            using (this.Container.Using(excel))
            {
                if (excel == null)
                {
                    throw new ImportException("Не найдена реализация интерфейса IGkhExcelProvider");
                }

                if (extention == "xlsx")
                {
                    excel.UseVersionXlsx();
                }

                using (var memoryStreamFile = new MemoryStream(file.Data))
                {
                    memoryStreamFile.Seek(0, SeekOrigin.Begin);

                    excel.Open(memoryStreamFile);

                    var firstRow = excel.GetRows(0, 0)[0];

                    var hasRoIdColumn = firstRow.Length > 1 && firstRow[1].Value.Trim().ToUpper() == "ID ДОМА";

                    if (firstRow.Length < (hasRoIdColumn ? 10 : 9))
                    {
                        throw new ImportException("Отсутствуют необходимые столбцы!");
                    }

                    var headers = new Dictionary<int, string>();

                    headers = this.RealityObjectAddresses.InitNewHeader(headers, hasRoIdColumn);
                    
                    foreach (var header in headers)
                    {
                        if (firstRow[header.Key].Value.Trim().ToUpper() != header.Value)
                        {
                            throw new ImportException("Необходимые столбцы не обнаружены!");
                        }
                    }
                }
            }

            return true;
        }

        private ObjectCr GetObjectCr(long realObjId, long programCrId)
        {
            var objectCr = this.objectsCr.Get(realObjId);

            if (objectCr == null)
            {
                objectCr = new ObjectCr(this.RealityObjectDomain.Load(realObjId), this.ProgramCrDomain.Load(programCrId));
                objectCr.State = this.objectCrState;

                this.listToSaveObjectCr.Add(objectCr);

                var monitoringSmr = new MonitoringSmr { ObjectCr = objectCr };
                var stateProvider = Container.Resolve<IStateProvider>();
                stateProvider.SetDefaultState(monitoringSmr);
                this.listToSaveMonitoringSmr.Add(monitoringSmr);

                this.LogImport.Info("Информация", "Добавлен объект КР. Адрес: {0}".FormatUsing(this.realObjAddress.Get(realObjId)));
                this.LogImport.CountAddedRows++;
            }

            return objectCr;
        }

        private void SaveTypeWorkCr(GkhExcelCell[] row, ObjectCr objectCr, bool replaceTypeWork, bool replaceObjectCr, long financeCutForm)
        {
            var financeSource = this.Container.Resolve<IDomainService<FinanceSource>>();

            var existTypeWorks = this.typeWorks.Get(objectCr.Id) ?? new List<TypeWorkCr>();

            var noDeleteTypeWorks = new List<string>();

            if (replaceObjectCr)
            {
                existTypeWorks.Where(x => !this.typeWorksWithLinks.Contains(x.Id)).ForEach(x => this.listToDeleteTypeWorkCr.Add(x.Id));
                existTypeWorks.Where(x => this.typeWorksWithLinks.Contains(x.Id)).ForEach(x => noDeleteTypeWorks.Add(x.Work.Name));
            }

            // Если стоит отметка "Заменить данные по видам работ", то в объекте КР удаляем виды работ,
            // которые не имеют блокирующих связей, и добавляем новые из файла
            if (replaceTypeWork)
            {
                foreach (var existTypeWork in existTypeWorks)
                {
                    if (this.typeWorksWithLinks.Contains(existTypeWork.Id))
                    {
                        noDeleteTypeWorks.Add(existTypeWork.Work.Name);
                        continue;
                    }

                    this.listToDeleteTypeWorkCr.Add(existTypeWork.Id);
                }
            }

            foreach (var workLine in this.WorkLines)
            {
                var typeWork = existTypeWorks.FirstOrDefault(x => x.Work.Id == workLine.Key.Id);

                if (!replaceTypeWork && typeWork != null)
                {
                    continue;
                }

                var years = workLine.Value.YearLines.Select(x => row[x].Value.ToInt()).Where(x => x > 0).ToList();
                var sum = workLine.Value.SumLines.SafeSum(x => row[x].Value.ToDecimal());
                var volume = workLine.Value.VolumeLines.SafeSum(x => row[x].Value.ToDecimal());
                var financeCut = workLine.Value.FinanceLines.SafeSum(x => row[x].Value.ToDecimal());

                var financeDictionary = financeCut != 0 
                    ? financeSource.GetAll().FirstOrDefault(x => x.Code == financeCut.ToStr()) 
                    : financeCutForm == 0 
                        ? null 
                        : new FinanceSource{ Id = financeCutForm};

                if (sum == 0 && volume == 0)
                {
                    continue;
                }

                typeWork = new TypeWorkCr
                {
                    ObjectCr = objectCr,
                    Sum = sum,
                    Volume = volume,
                    FinanceSource = financeDictionary ?? null,
                    YearRepair = years.Count > 0 ? (int?)years.Max() : null,
                    Work = workLine.Key,
                    IsActive = true
                };

                this.listToSaveTypeWorkCr.Add(typeWork);

                this.LogImport.Info(
                    "Информация",
                    "Добавлен вид работы: {0}. Адрес: {1}".FormatUsing(workLine.Key.Name, this.realObjAddress.Get(objectCr.RealityObject.Id)));
                this.LogImport.CountAddedRows++;

                if (years.Count > 0)
                {
                    this.typeWorkYears.Add(typeWork, years);
                }
            }

            if (noDeleteTypeWorks.Count > 0)
            {
                var description = "Невозможно удалить виды работ. Адрес: {0}. Виды работ: {1}".FormatUsing(
                    this.realObjAddress.Get(objectCr.RealityObject.Id),
                    noDeleteTypeWorks.AggregateWithSeparator(", "));
                this.LogImport.Warn("Предупреждение", description);
            }

            this.Container.Release(financeSource);
        }

        private void SaveData()
        {
            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var session = sessionProvider.OpenStatelessSession();

            try
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        if (this.listToDeleteTypeWorkCr.Count > 0)
                        {
                            var sql = string.Format(
                                @"delete from CR_OBJ_CMP_ARCHIVE where TYPE_WORK_CR_ID in({0}) ",
                                this.listToDeleteTypeWorkCr.Select(x => x.ToStr()).AggregateWithSeparator(", "));
                            session.CreateSQLQuery(sql).ExecuteUpdate();

                            sql = string.Format(
                                @"delete from CR_OBJ_TYPE_WORK_HIST where TYPE_WORK_ID in({0}) ",
                                this.listToDeleteTypeWorkCr.Select(x => x.ToStr()).AggregateWithSeparator(", "));
                            session.CreateSQLQuery(sql).ExecuteUpdate();

                            if (this.ProgramCrImportService != null)
                            {
                                this.ProgramCrImportService.DeleteDpkrLink(session, this.listToDeleteTypeWorkCr);
                                this.ProgramCrImportService.DeleteTypeWorkCrVersionStage1(session, this.listToDeleteTypeWorkCr);
                            }

                            sql = string.Format(
                                @"delete from CR_OBJ_FIN_SOURCE_RES where TYPE_WORK_ID in({0}) ",
                                this.listToDeleteTypeWorkCr.Select(x => x.ToStr()).AggregateWithSeparator(", "));
                            session.CreateSQLQuery(sql).ExecuteUpdate();

                            sql = string.Format(
                                @"delete from CR_COMPETITION_LOT_TW where TYPE_WORK_ID in({0}) ",
                                this.listToDeleteTypeWorkCr.Select(x => x.ToStr()).AggregateWithSeparator(", "));
                            session.CreateSQLQuery(sql).ExecuteUpdate();
                            
                            sql = string.Format(
                                @"delete from cr_obj_design_asgmnt_type_work where type_work_id in({0}) ",
                                this.listToDeleteTypeWorkCr.Select(x => x.ToStr()).AggregateWithSeparator(", "));
                            session.CreateSQLQuery(sql).ExecuteUpdate();

                            sql = string.Format(
                                @"delete from CR_OBJ_TYPE_WORK where ID in({0}) ",
                                this.listToDeleteTypeWorkCr.Select(x => x.ToStr()).AggregateWithSeparator(", "));
                            session.CreateSQLQuery(sql).ExecuteUpdate();
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
                sessionProvider.CloseCurrentSession();
            }

            TransactionHelper.InsertInManyTransactions(this.Container, this.listToSaveObjectCr, 10000, true, true);
            TransactionHelper.InsertInManyTransactions(this.Container, this.listToSaveTypeWorkCr, 10000, true, true);
            TransactionHelper.InsertInManyTransactions(this.Container, this.listToSaveMonitoringSmr, 10000, true, true);
        }

        private void InitHeader()
        {
            this.RealityObjectAddresses.InitHeader(this.hasRoIdColumn);
        }

        private void InitWorkInfoLines(GkhExcelCell[] firstRow)
        {
            var workDomain = this.Container.ResolveDomain<Work>();
            this.WorkLines = new Dictionary<Work, WorkInfoLines>();

            try
            {
                var works = workDomain.GetAll()
                    .Select(
                        x => new
                        {
                            x.Name,
                            Work = x
                        })
                    .ToList()
                    .GroupBy(x => x.Name.Trim().ToUpper())
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Work).First());

                for (var i = this.RealityObjectAddresses.InitHeader(this.hasRoIdColumn).Count; i < firstRow.Length; i++)
                {
                    var strs = firstRow[i].Value.Trim().ToUpper().Split(new[] { '_' }, 2);

                    if (strs.Count() != 2)
                        continue;

                    var workName = strs[0];
                    var fieldName = strs[1];

                    if (works.ContainsKey(workName))
                    {
                        var work = works.Get(workName);

                        if (!this.WorkLines.ContainsKey(work))
                        {
                            this.WorkLines.Add(work, new WorkInfoLines());
                        }

                        var workInfoLines = this.WorkLines.Get(work);

                        switch (fieldName)
                        {
                            case "ОБЪЕМ":
                                workInfoLines.VolumeLines.Add(i);
                                break;
                            case "СТОИМОСТЬ":
                                workInfoLines.SumLines.Add(i);
                                break;
                            case "ГОД":
                                workInfoLines.YearLines.Add(i);
                                break;
                            case "РАЗРЕЗ ФИНАНСИРОВАНИЯ":
                                workInfoLines.FinanceLines.Add(i);
                                break;
                            default:
                                this.LogImport.Error("Ошибка", "Столбец №{0}. Не определено поле вида работ: {1}".FormatUsing(i, fieldName));
                                break;
                        }
                    }
                    else
                    {
                        this.LogImport.Error("Ошибка", "Столбец №{0}. Не найдена работа: {1}".FormatUsing(i, workName));
                    }
                }
            }
            finally
            {
                this.Container.Release(workDomain);
            }
        }

        private void InitDictionaries(long programCrId)
        {
            this.fiasHelper = new UnstrongFiasHelper { Container = this.Container };
            this.fiasHelper.Initialize();

            var municipalityDomain = this.Container.Resolve<IRepository<Municipality>>();
            var fiasDomain = this.Container.Resolve<IRepository<Fias>>();
            var defectDomain = this.Container.ResolveDomain<DefectList>();
            var estimateCalculationDomain = this.Container.ResolveDomain<EstimateCalculation>();
            var performedWorkActDomain = this.Container.ResolveDomain<PerformedWorkAct>();
            var typeWorkCrRemovalDomain = this.Container.ResolveDomain<TypeWorkCrRemoval>();
            var stateProvider = this.Container.Resolve<IStateProvider>();
            var stateDomain = this.Container.ResolveDomain<State>();
            try
            {
                var municipalities = municipalityDomain.GetAll().ToArray();

                this.fiasIdByMunicipalityNameDict = municipalities
                    .Where(x => this.Name != null)
                    .Select(x => new { x.Name, x.FiasId, x.Id })
                    .AsEnumerable()
                    .GroupBy(x => x.Name.ToUpper().Trim())
                    .ToDictionary(
                        x => x.Key,
                        x =>
                        {
                            var first = x.First();
                            return new KeyValuePair<string, long>(first.FiasId, first.Id);
                        });

                var actualAddressQuery = fiasDomain.GetAll()
                    .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                    .WhereNotEmptyString(x => x.AOGuid);

                var streetList = this.FiasAddressDomain.GetAll()
                    .Where(x => actualAddressQuery.Any(z => z.AOGuid == x.StreetGuidId))
                    .Join(this.RealityObjectRepository.GetAll(),
                        x => x.Id,
                        x => x.FiasAddress.Id,
                        (fa, ro) => new RealtyObjectAddressProxy
                        {
                            AoGuid = fa.StreetGuidId,
                            Id = fa.Id,
                            House = fa.House,
                            Housing = fa.Housing,
                            Building = fa.Building,
                            RealityObjectId = ro.Id,
                            MunicipalityId = ro.Municipality.Id,
                            Liter = fa.Letter
                        })
                    .AsEnumerable();
                var placeList = this.FiasAddressDomain.GetAll()
                    .Where(x => x.StreetGuidId == null)
                    .Where(x => actualAddressQuery.Any(z => z.AOGuid == x.PlaceGuidId))
                    .Join(this.RealityObjectRepository.GetAll(),
                        x => x.Id,
                        x => x.FiasAddress.Id,
                        (fa, ro) => new RealtyObjectAddressProxy
                        {
                            AoGuid = fa.PlaceGuidId,
                            Id = fa.Id,
                            House = fa.House,
                            Housing = fa.Housing,
                            Building = fa.Building,
                            RealityObjectId = ro.Id,
                            MunicipalityId = ro.Municipality.Id,
                            Liter = fa.Letter
                        })
                    .AsEnumerable();

                this.realityObjectsByFiasGuidDict = streetList
                    .Union(placeList)
                    .GroupBy(x => x.AoGuid)
                    .ToDictionary(x => x.Key, x => x.ToList());

                var fiasCache = fiasDomain.GetAll()
                    .Select(
                        x => new
                        {
                            x.AOGuid,
                            x.ShortName,
                            x.FormalName,
                            x.ActStatus
                        })
                    .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                    .ToList()
                    .GroupBy(x => x.AOGuid)
                    .ToDictionary(x => x.Key, y => y.First());

                this.robjectByAddressCache = this.RealityObjectDomain.GetAll()
                    .Where(x => x.FiasAddress != null)
                    .Select(
                        x => new
                        {
                            RoId = x.Id,
                            Mr = x.Municipality.Name.ToLower().Trim(),
                            Mu = x.MoSettlement.Name.ToLower().Trim(),
                            x.FiasAddress.PlaceGuidId,
                            x.FiasAddress.StreetGuidId,
                            House = x.FiasAddress.House.ToLower().Trim(),
                            Housing = x.FiasAddress.Housing.ToLower().Trim(),
                            Letter = x.FiasAddress.Letter.ToLower().Trim(),
                            Building = x.FiasAddress.Building.ToLower().Trim(),
                        })
                    .AsEnumerable()
                    .Select(
                        x => new
                        {
                            Key = string.Format(
                                "{0}#{1}#{2}#{3}#{4}{5}{6}",
                                x.Mr,
                                x.PlaceGuidId.IsEmpty()
                                    ? string.Empty
                                    : fiasCache.Get(x.PlaceGuidId)
                                        .Return(
                                            y =>
                                                "{0} {1}".FormatUsing(this.ToLowerTrim(y.FormalName), this.ToLowerTrim(y.ShortName))),
                                x.StreetGuidId.IsEmpty()
                                    ? " "
                                    : fiasCache.Get(x.StreetGuidId)
                                        .Return(
                                            y =>
                                                "{0} {1}".FormatUsing(this.ToLowerTrim(y.FormalName), this.ToLowerTrim(y.ShortName))),
                                x.House,
                                x.Housing,
                                x.Letter,
                                x.Building),
                            x.RoId
                        })
                    .GroupBy(x => x.Key)
                    .ToDictionary(
                        x => x.Key,
                        y => new RobjectProxy
                        {
                            Count = y.Count(),
                            roId = y.First().RoId
                        });

                this.objectsCr = this.ObjectCrDomain.GetAll()
                    .Where(x => x.ProgramCr.Id == programCrId)
                    .Select(
                        x => new
                        {
                            RoId = x.RealityObject.Id,
                            ObjectCr = x
                        })
                    .ToList()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.ObjectCr).First());

                this.typeWorks = this.TypeWorkCrDomain.GetAll()
                    .Where(x => x.ObjectCr.ProgramCr.Id == programCrId)
                    .Select(
                        x => new
                        {
                            x.ObjectCr.Id,
                            TypeWork = x
                        })
                    .ToList()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.TypeWork).ToList());

                this.realObjAddress = this.RealityObjectDomain.GetAll()
                    .Select(
                        x => new
                        {
                            x.Address,
                            x.Id
                        })
                    .ToDictionary(x => x.Id, y => y.Address);

                var defectListQuery = defectDomain.GetAll().Where(x => x.ObjectCr.ProgramCr.Id == programCrId);

                this.typeWorksWithLinks = this.TypeWorkCrDomain.GetAll()
                    .Where(
                        x => defectListQuery.Any(y => y.ObjectCr.Id == x.ObjectCr.Id && y.Work.Id == x.Work.Id) ||
                            estimateCalculationDomain.GetAll().Any(y => x.Id == y.TypeWorkCr.Id) ||
                            performedWorkActDomain.GetAll().Any(y => x.Id == y.TypeWorkCr.Id) ||
                            typeWorkCrRemovalDomain.GetAll().Any(y => x.Id == y.TypeWorkCr.Id))
                    .Select(x => x.Id)
                    .ToList()
                    .ToHashSet();

                var stateInfo = stateProvider.GetStatefulEntityInfo(typeof(ObjectCr));
                this.objectCrState = stateDomain.GetAll().FirstOrDefault(x => x.TypeId == stateInfo.TypeId && x.StartState);
            }
            finally
            {
                this.Container.Release(municipalityDomain);
                this.Container.Release(fiasDomain);
                this.Container.Release(defectDomain);
                this.Container.Release(estimateCalculationDomain);
                this.Container.Release(performedWorkActDomain);
                this.Container.Release(typeWorkCrRemovalDomain);
            }
        }

        private void InitLog(string fileName)
        {
            if (this.LogImportManager == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImportManager");
            }

            this.LogImportManager.FileNameWithoutExtention = fileName;
            this.LogImportManager.UploadDate = DateTime.Now;

            if (this.LogImport == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImport");
            }

            this.LogImport.SetFileName(fileName);
            this.LogImport.ImportKey = this.Key;
        }


        private string ToLowerTrim(string value)
        {
            return (value ?? string.Empty).ToLower().Trim(' ', '.');
        }
        
        private long GetRealObjId(GkhExcelCell[] row, int rowNumber, bool hasRoIdColumn )
        {
            var record = this.RealityObjectAddresses.InitRecord(row, rowNumber, hasRoIdColumn);     

            if (row.Length <= 1)
            {
                return 0;
            }

            if (this.hasRoIdColumn)
            {
                var roIdValue = this.RealityObjectAddresses.GetValue(row, "RO_ID", this.hasRoIdColumn).Trim();
                var roId = !string.IsNullOrEmpty(roIdValue) ? roIdValue.ToLong() : 0;
                if (roId > 0 && this.realObjAddress.ContainsKey(roId))
                {
                    return roId;
                }
            }

            var municipalityName = this.RealityObjectAddresses.GetValue(row, "MUNICIPALITY", this.hasRoIdColumn).Trim().ToUpper();
            if (string.IsNullOrEmpty(municipalityName))
            {
                this.LogImport.Error(string.Empty, $"Муниципальное образование не задано. Строка: {rowNumber}");
                return 0;
            }

            if (string.IsNullOrEmpty(record.House))
            {
                this.LogImport.Error(string.Empty, string.Format("Не задан номер дома. Строка: {0}", rowNumber));
                return 0;
            }

            if (!this.fiasIdByMunicipalityNameDict.ContainsKey(municipalityName))
            {
                var errText = string.Format("В справочнике муниципальных образований не найдена запись: {0}", this.RealityObjectAddresses.GetValue(row, "MUNICIPALITY", this.hasRoIdColumn).Trim());
                this.LogImport.Error(string.Empty, errText);
                return 0;
            }

            var municipality = this.fiasIdByMunicipalityNameDict[municipalityName];
            var fiasGuid = municipality.Key;

            if (string.IsNullOrWhiteSpace(fiasGuid))
            {
                this.LogImport.Error(string.Empty, string.Format("Муниципальное образование не привязано к ФИАС. Строка: {0}", rowNumber));
                return 0;
            }

            if (!this.fiasHelper.IncludeInBranch(fiasGuid))
            {
                this.LogImport.Error(
                    string.Empty,
                    string.Format("В структуре ФИАС не найдена актуальная запись для муниципального образования. Строка: {0}", rowNumber));
                return 0;
            }

            var faultReason = string.Empty;
            DynamicAddress address;

            if (string.IsNullOrEmpty(record.StreetName) && !string.IsNullOrEmpty(record.House))
            {
                var addressKey = string.Format(
                    "{0}#{1}#{2}#{3}#{4}",
                    this.ToLowerTrim(municipalityName),
                    this.ToLowerTrim(record.LocalityName),
                    " ",
                    this.ToLowerTrim(record.House),
                    this.ToLowerTrim(record.Housing));

                if (this.robjectByAddressCache.ContainsKey(addressKey)
                    && this.robjectByAddressCache[addressKey].Count == 1)
                {
                    return this.robjectByAddressCache[addressKey].roId;
                }
            }

            if (!this.fiasHelper.FindInBranch(fiasGuid, record.LocalityName, record.StreetName, ref faultReason, out address))
            {
                this.LogImport.Error(string.Empty, string.Format("{0}. Строка: {1}", faultReason, rowNumber));
                return 0;
            }

            record.Address = address;
            record.MunicipalityId = municipality.Value;

            // Проверяем есть ли дома на данной улице
            if (this.realityObjectsByFiasGuidDict.ContainsKey(record.Address.GuidId))
            {
                // Составляем список домов на данной улице (!) с проверкой привязки МО (это не бред, после разделения одного МО многое может произойти)
                var existingRealityObjects = this.realityObjectsByFiasGuidDict[record.Address.GuidId].Where(x => x.MunicipalityId == record.MunicipalityId)
                    .ToList();

                if (existingRealityObjects.Any())
                {
                    var result = existingRealityObjects.Where(x => x.House == record.House).ToList();
                    
                    return this.RealityObjectAddresses.GetRealityObjectAddress(result, record, this.LogImport);
                }

                if (!existingRealityObjects.Any())
                {
                    this.LogImport.Error(
                        string.Empty,
                        string.Format("У данного адреса другое муниципальное образование. Строка: {0}", rowNumber));
                    return 0;

                }
            }

            return 0;
        }

        private class RobjectProxy
        {
            public int Count { get; set; }

            public long roId { get; set; }
        }

        public class WorkInfoLines
        {
            public WorkInfoLines()
            {
                this.SumLines = new List<int>();
                this.YearLines = new List<int>();
                this.VolumeLines = new List<int>();
                this.FinanceLines = new List<int>();
            }

            public List<int> SumLines { get; set; }

            public List<int> YearLines { get; set; }

            public List<int> VolumeLines { get; set; }

            public List<int> FinanceLines { get; set; }
        }

        public sealed class RealtyObjectAddress
        {
            public string KladrCode { get; set; }

            public string AoGuid { get; set; }

            public long Id { get; set; }

            public string House { get; set; }

            public string Housing { get; set; }

            public string Building { get; set; }

            public long RealityObjectId { get; set; }

            public long MunicipalityId { get; set; }

            public string Liter { get; set; }
        }
    }
}