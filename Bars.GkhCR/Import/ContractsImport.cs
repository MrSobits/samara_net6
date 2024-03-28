namespace Bars.GkhCr.Import
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.GkhExcel;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.Cr;
    using Bars.Gkh.ConfigSections.Cr.Enums;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Utils.EntityExtensions;

    using Gkh.Import.Impl;

    using Microsoft.Extensions.Logging;

    using NHibernate.Linq;

    /// <summary>
    /// Импорт договоров и актов
    /// </summary>
    public class ContractsImport : GkhImportBase
    {
        /// <summary>
        /// Идентификатор импорта
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Ключ импорта
        /// </summary>
        public override string Key => ContractsImport.Id;

        /// <summary>
        /// Код импорта
        /// </summary>
        public override string CodeImport => "ObjectCr";

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public override string Name => "Импорт договоров и актов";

        /// <summary>
        /// Доступные расширения файла
        /// </summary>
        public override string PossibleFileExtensions => "xls,xlsx";

        /// <summary>
        /// Наименование разрешения
        /// </summary>
        public override string PermissionName => "Import.ContractsImport.View";

        private const int ColumnCount = 92;
        private const int HeaderRowCount = 2;
        private int readRowIdx;

        private HashSet<long> RealityObjectIds { get; set; }
        private IDictionary<string, long> ProgramCrIds { get; set; }
        private IDictionary<long, Dictionary<long, long>> ObjectCrIds { get; set; }
        private IDictionary<long, Dictionary<string, TypeWorkCr>> TypeWorkCrDict { get; set; }
        private IDictionary<long, Dictionary<long, PerformedWorkAct>> PerformedWorkActDict { get; set; }
        private IDictionary<long, IGrouping<long, ContractCr>> ContractCrDict { get; set; }
        private IDictionary<long, IGrouping<long, BuildContract>> BuildContractDict { get; set; }
        private IDictionary<string, Dictionary<string, long>> ContragentDict { get; set; }
        private IDictionary<long, long> BuilderDict { get; set; }
        private State PerformedWorkActState { get; set; }
        private State ContractCrDraftState { get; set; }
        private State ContractCrApprovedState { get; set; }
        private State BuildContractDraftState { get; set; }
        private State BuildContractApprovedState { get; set; }

        private IList<PerformedWorkAct> PerformedWorkActsToSave { get; set; }
        private IList<PerformedWorkAct> PerformedWorkActsToUpdate { get; set; }
        private IList<ContractCr> ContractCrToSave { get; set; }
        private IList<ContractCr> ContractCrToUpdate { get; set; }
        private IList<BuildContract> BuildContractToSave { get; set; }
        private IList<BuildContract> BuildContractToUpdate { get; set; }
        private IList<TypeWorkCr> TypeWorkCrToUpdate { get; set; }
        
        private ILogger log;
        
        /// <summary>
        ///     Логгер
        /// </summary>
        protected ILogger Log
        => this.log ?? (this.log = ApplicationContext.Current.Container.Resolve<ILogger>());
        
        public IStateProvider StateProvider { get; set; }
        public IRepository<State> StateRepository { get; set; }

        /// <inheritdoc />
        protected override ImportResult ImportUsingGkhApi(BaseParams baseParams)
        {
            var message = string.Empty;
            var fileData = baseParams.Files["FileImport"];
            var extention = baseParams.Files["FileImport"].Extention;

            var excel = this.GetProvider(extention);
            using (this.Container.Using(excel))
            {
                using (var memoryStreamFile = new MemoryStream(fileData.Data))
                {
                    excel.Open(memoryStreamFile);
                    try
                    {
                        this.InitCache();
                        this.Container.InTransaction(() =>
                        {
                            this.readRowIdx += ContractsImport.HeaderRowCount;
                            excel.GetRows(0, 0)
                                .Skip(ContractsImport.HeaderRowCount)
                                .ForEach(this.ImportRow);

                            this.SavePerformedWorkAct();
                        });

                        TransactionHelper.InsertInManyTransactions(this.Container, this.TypeWorkCrToUpdate, useStatelessSession: true);
                        TransactionHelper.InsertInManyTransactions(this.Container, this.ContractCrToSave, useStatelessSession: true);
                        TransactionHelper.InsertInManyTransactions(this.Container, this.ContractCrToUpdate, useStatelessSession: true);
                        TransactionHelper.InsertInManyTransactions(this.Container, this.BuildContractToSave, useStatelessSession: true);
                        TransactionHelper.InsertInManyTransactions(this.Container, this.BuildContractToUpdate, useStatelessSession: true);
                        TransactionHelper.InsertInManyTransactions(this.Container, this.PerformedWorkActsToSave, useStatelessSession: true);
                        TransactionHelper.InsertInManyTransactions(this.Container, this.PerformedWorkActsToUpdate, useStatelessSession: true);

                        this.LogImport.CountAddedRows += this.PerformedWorkActsToSave.Count;
                        this.LogImport.CountChangedRows += this.PerformedWorkActsToUpdate.Count;
                        this.LogImport.CountChangedRows += this.TypeWorkCrToUpdate.Count;
                        this.LogImport.CountAddedRows += this.ContractCrToSave.Count;
                        this.LogImport.CountChangedRows += this.ContractCrToUpdate.Count;
                        this.LogImport.CountAddedRows += this.BuildContractToSave.Count;
                        this.LogImport.CountChangedRows += this.BuildContractToUpdate.Count;

                        if (this.LogImport.CountError > 0)
                        {
                            this.LogImport.IsImported = false;
                            message = "Произошла неизвестная ошибка.";
                        }
                        else
                        {
                            this.LogImport.IsImported = true;
                        }
                    }
                    catch (Exception e)
                    {
                        this.LogImport.IsImported = false;
                        Log.LogError(e, "Импорт");
                        message = "Произошла неизвестная ошибка.";
                        this.LogImport.Error(this.Name, "Произошла неизвестная ошибка. Обратитесь к администратору");
                    }
                    finally
                    {
                        this.RealityObjectIds?.Clear();
                        this.ProgramCrIds?.Clear();
                        this.ObjectCrIds?.Clear();
                        this.TypeWorkCrDict?.Clear();
                        this.PerformedWorkActDict?.Clear();
                        this.ContractCrDict?.Clear();
                        this.BuildContractDict?.Clear();
                        this.ContragentDict?.Clear();
                        this.BuilderDict?.Clear();
                    }
                }
            }

            return this.GetResult(message);
        }

        /// <inheritdoc />
        public override bool Validate(BaseParams baseParams, out string message)
        {
            try
            {
                if (!baseParams.Files.ContainsKey("FileImport"))
                {
                    message = "Не выбран файл для импорта";
                    return false;
                }

                var bytes = baseParams.Files["FileImport"].Data;
                var extention = baseParams.Files["FileImport"].Extention;

                var fileExtentions = this.PossibleFileExtensions.Contains(",")
                    ? this.PossibleFileExtensions.Split(',')
                    : new[] { this.PossibleFileExtensions };
                if (fileExtentions.All(x => x != extention))
                {
                    message = $"Необходимо выбрать файл с допустимым расширением: {this.PossibleFileExtensions}";
                    return false;
                }

                var excel = this.GetProvider(extention);
                var workbookId = 0;
                var worksheetId = 0;
                var rowId = 0;
                using (this.Container.Using(excel))
                {
                    using (var memoryStreamFile = new MemoryStream(bytes))
                    {
                        excel.Open(memoryStreamFile);
                        if (excel.IsEmpty(workbookId, worksheetId))
                        {
                            message = $"Не удалось обнаружить записи в файле: {this.PossibleFileExtensions}";
                            return false;
                        }

                        var title = excel.GetRow(workbookId, worksheetId, rowId);

                        if (title.Length < ContractsImport.ColumnCount)
                        {
                            message = "Количество столбцов в файле не совпадает с шаблоном. "
                                + $"Ожидаемое количество столбцов: {ContractsImport.ColumnCount}, в файле: {title.Length}";

                            return false;
                        }
                    }
                }

                message = string.Empty;
                return true;
            }
            catch (Exception e)
            {
                Log.LogError(e, "Валидация файла импорта");
                message = "Произошла неизвестная ошибка при проверке формата файла";
                return false;
            }
        }

        private void InitCache()
        {
            var realityObjectRepository = this.Container.ResolveRepository<RealityObject>();
            var programCrRepository = this.Container.ResolveRepository<ProgramCr>();
            var objectCrRepository = this.Container.ResolveRepository<ObjectCr>();
            var typeWorkCrRepository = this.Container.ResolveRepository<TypeWorkCr>();
            var contragentRepository = this.Container.ResolveRepository<Contragent>();
            var builderRepository = this.Container.ResolveRepository<Builder>();
            var contractCrRepository = this.Container.ResolveRepository<ContractCr>();
            var buildContractRepository = this.Container.ResolveRepository<BuildContract>();
            var performedWorkActRepository = this.Container.ResolveRepository<PerformedWorkAct>();

            try
            {
                this.RealityObjectIds = realityObjectRepository.GetAll().Select(x => x.Id).ToList().ToHashSet();
                this.ProgramCrIds = programCrRepository.GetAll()
                    .Where(x => x.TypeVisibilityProgramCr == TypeVisibilityProgramCr.Full)
                    .Where(x => x.TypeProgramStateCr == TypeProgramStateCr.New)
                    .Select(x => new
                    {
                        x.Id,
                        x.Period.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Name, x => x.Id)
                    .ToDictionary(x => x.Key, x => x.FirstOrDefault());

                this.ObjectCrIds = objectCrRepository.GetAll()
                    .WhereNotNull(x => x.RealityObject)
                    .WhereNotNull(x => x.ProgramCr)
                    .Select(x => new
                    {
                        x.Id,
                        RoId = x.RealityObject.Id,
                        ProgramCrId = x.ProgramCr.Id,
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key,
                        x => x.GroupBy(y => y.ProgramCrId, y => y.Id)
                            .ToDictionary(y => y.Key, y => y.FirstOrDefault()));

                this.TypeWorkCrDict = typeWorkCrRepository.GetAll()
                    .WhereNotNull(x => x.ObjectCr)
                    .WhereNotNull(x => x.Work)
                    .Fetch(x => x.Work)
                    .AsEnumerable()
                    .GroupBy(x => x.ObjectCr.Id)
                    .ToDictionary(x => x.Key,
                        x => x.GroupBy(y => y.Work.Code)
                            .ToDictionary(y => y.Key, y => y.FirstOrDefault()));

                this.PerformedWorkActDict = performedWorkActRepository.GetAll()
                    .WhereNotNull(x => x.ObjectCr)
                    .WhereNotNull(x => x.TypeWorkCr)
                    .AsEnumerable()
                    .GroupBy(x => x.ObjectCr.Id)
                    .ToDictionary(x => x.Key,
                        x => x.GroupBy(y => y.TypeWorkCr.Id)
                            .ToDictionary(y => y.Key, y => y.FirstOrDefault()));

                this.ContractCrDict = contractCrRepository.GetAll()
                    .AsEnumerable()
                    .GroupBy(x => x.ObjectCr.Id)
                    .ToDictionary(x => x.Key);

                this.BuildContractDict = buildContractRepository.GetAll()
                    .AsEnumerable()
                    .GroupBy(x => x.ObjectCr.Id)
                    .ToDictionary(x => x.Key);

                this.ContragentDict = contragentRepository.GetAll()
                    .WhereNotEmptyString(x => x.Name)
                    .WhereNotEmptyString(x => x.Inn)
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Inn
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Name)
                    .ToDictionary(x => x.Key,
                        x => x.GroupBy(y => y.Inn, y => y.Id)
                            .ToDictionary(y => y.Key, y => y.FirstOrDefault()));

                this.BuilderDict = builderRepository.GetAll()
                    .Where(x => (long?)x.Contragent.Id != null)
                    .Select(x => new
                    {
                        x.Id,
                        ContragentId = x.Contragent.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.ContragentId, x => x.Id)
                    .ToDictionary(x => x.Key, x => x.First());

                this.PerformedWorkActState = this.GetFinalState<PerformedWorkAct>();
                this.ContractCrApprovedState = this.GetFinalState<ContractCr>();
                this.ContractCrDraftState = this.GetStartState<ContractCr>();
                this.BuildContractApprovedState = this.GetFinalState<BuildContract>();
                this.BuildContractDraftState = this.GetStartState<BuildContract>();

                this.PerformedWorkActsToSave = new List<PerformedWorkAct>();
                this.PerformedWorkActsToUpdate = new List<PerformedWorkAct>();
                this.TypeWorkCrToUpdate = new List<TypeWorkCr>();
                this.ContractCrToSave = new List<ContractCr>();
                this.ContractCrToUpdate = new List<ContractCr>();
                this.BuildContractToSave = new List<BuildContract>();
                this.BuildContractToUpdate = new List<BuildContract>();
            }
            finally
            {
                this.Container.Release(realityObjectRepository);
                this.Container.Release(programCrRepository);
                this.Container.Release(objectCrRepository);
                this.Container.Release(typeWorkCrRepository);
                this.Container.Release(contragentRepository);
                this.Container.Release(builderRepository);
                this.Container.Release(contractCrRepository);
                this.Container.Release(buildContractRepository);
                this.Container.Release(performedWorkActRepository);
            }
        }

        private void ImportRow(GkhExcelCell[] row)
        {
            this.readRowIdx++;

            if (this.IsContinue(row))
            {
                return;
            }

            var importRow = new RowDto
            {
                RoId = row[6].Value.ToLong(),
                MainWorkCode = row[9].Value,
                SummValue = row[10].ToDecimalNullable(),

                ContractCrWinner = row[15].Value,
                ContractCrInn = row[16].Value,
                ContractCrWorkCode = row[17].Value,
                ContractCrNumber = row[18].Value,
                ContractCrDate = row[19].ToDateTimeNullable(),
                ContractCrAuctionPrice = row[20].ToDecimalNullable(),
                ContractCrPlanEndDate = row[21].ToDateTimeNullable(),
                CalculationVolume = row[33].ToDecimalNullable(),

                PcdWorkPrice = row[43].ToDecimalNullable(),
                PcdContractDate = row[45].ToDateTimeNullable(),

                SmrWinner = row[52].Value,
                SmrInn = row[53].Value,
                SmrContractNumber = row[54].Value,
                SmrContractDate = row[55].ToDateTimeNullable(),
                SmrAuctionPrice = row[56].ToDecimalNullable(),
                SmrContractPlanEndDate = row[59].ToDateTimeNullable(),
                ProgramYear = row[60].Value,

                Ks2WorkPrice = row[72].ToDecimalNullable(),
                Ks2Date = row[73].ToDateTimeNullable(),

                BuildControlWinner = row[81].Value,
                BuildControlInn = row[82].Value,
                BuildControlWorkCode = row[83].Value,
                BuildControlContractNumber = row[84].Value,
                BuildControlContractDate = row[85].ToDateTimeNullable(),
                BuildControlAuctionPrice = row[87].ToDecimalNullable(),
                BuildControlContractPlanEndDate = row[88].ToDateTimeNullable(),

                ActWorkPrice = row[89].ToDecimalNullable(),
                ActDate = row[90].ToDateTimeNullable()
            };

            if (this.ValidateRow(importRow) == false)
            {
                return;
            }

            var realityObjectId = importRow.RoId;
            var programCrId = 0L;
            if (!this.ProgramCrIds.TryGetValue(importRow.ProgramYear, out programCrId) && programCrId == 0)
            {
                this.RowError("Не найдена краткосрочная программа");
                return;
            }

            var objectCr = this.GetObjectCr(realityObjectId, programCrId);
            if (objectCr == null)
            {
                this.RowError("Не найден Объект КР");
                return;
            }

            var contractCrTypeWork = this.GetTypeWorkCr(objectCr.Id, importRow.MainWorkCode);
            if (contractCrTypeWork == null)
            {
                this.RowError($"Не найдена работа по объекту КР с кодом '{importRow.MainWorkCode}'");
                return;
            }

            var buildContractTypeWork = this.GetTypeWorkCr(objectCr.Id, importRow.ContractCrWorkCode);
            if (buildContractTypeWork == null)
            {
                this.RowError($"Не найдена работа по объекту КР с кодом '{importRow.ContractCrWorkCode}'");
                return;
            }

            var buildControlContractTypeWork = this.GetTypeWorkCr(objectCr.Id, importRow.BuildControlWorkCode);
            if (buildControlContractTypeWork == null)
            {
                this.RowError($"Не найдена работа по объекту КР с кодом '{importRow.BuildControlWorkCode}'");
                return;
            }

            var contractCr = new ContractDto
            {
                ContragentName = importRow.ContractCrWinner,
                ContragentInn = importRow.ContractCrInn,
                Number = importRow.ContractCrNumber,
                StartDate = importRow.ContractCrDate,
                AuctionPrice = importRow.ContractCrAuctionPrice,
                PlanEndDate = importRow.ContractCrPlanEndDate
            };
            var buildContract = new ContractDto
            {
                ContragentName = importRow.SmrWinner,
                ContragentInn = importRow.SmrInn,
                Number = importRow.SmrContractNumber,
                StartDate = importRow.SmrContractDate,
                AuctionPrice = importRow.SmrAuctionPrice,
                PlanEndDate = importRow.SmrContractPlanEndDate
            };
            var buildControlContract = new ContractDto
            {
                ContragentName = importRow.BuildControlWinner,
                ContragentInn = importRow.BuildControlInn,
                Number = importRow.BuildControlContractNumber,
                StartDate = importRow.BuildControlContractDate,
                AuctionPrice = importRow.BuildControlAuctionPrice,
                PlanEndDate = importRow.BuildControlContractPlanEndDate
            };

            var psdAct = new ActDto
            {
                DocumentNum = importRow.ContractCrWorkCode,
                WorkPrice = importRow.PcdWorkPrice,
                Date = importRow.PcdContractDate
            };
            var ks2Act = new ActDto
            {
                WorkPrice = importRow.Ks2WorkPrice,
                Date = importRow.Ks2Date,
                Volume = importRow.CalculationVolume
            };
            var buildControlAct = new ActDto
            {
                WorkPrice = importRow.ActWorkPrice,
                Date = importRow.ActDate
            };

            {
                // Работы в Объекте КР
                buildContractTypeWork.Sum = importRow.SummValue;

                // Работа в Объекте КР, Акт выполненных работ
                buildContractTypeWork.Volume = importRow.CalculationVolume;

                this.TypeWorkCrToUpdate.Add(buildContractTypeWork);
            }

            // Договор СМР
            if (!this.ImportContractCr(objectCr, contractCrTypeWork, contractCr))
            {
                return;
            }

            // Акт выполненных работ ПСД
            this.ImportPerformedWorkAct(objectCr, buildContractTypeWork, psdAct);

            // Договор СМР
            if (!this.ImportBuildContract(objectCr, buildContractTypeWork, buildContract))
            {
                return;
            }
            
            // Акт выполненных КС2
            this.ImportPerformedWorkAct(objectCr, contractCrTypeWork, ks2Act);

            // Стройконтроль
            if (!this.ImportContractCr(objectCr, buildControlContractTypeWork, buildControlContract))
            {
                return;
            }

            // Акт выполненных СК
            this.ImportPerformedWorkAct(objectCr, buildControlContractTypeWork, buildControlAct);
        }

        private void SavePerformedWorkAct()
        {
            if (this.Container.GetGkhConfig<GkhCrConfig>().General.TypeWorkActNumeration == TypeWorkActNumeration.Automatic)
            {
                var maxNumber = 0;
                this.Container.UsingForResolved<IRepository<PerformedWorkAct>>((c, repo) =>
                {
                    maxNumber = repo.GetAll()
                        .Select(x => x.DocumentNum)
                        .AsEnumerable()
                        .Select(x => x.ToInt())
                        .SafeMax(x => x);
                });

                this.PerformedWorkActsToSave.Where(x => x.DocumentNum.IsEmpty())
                    .ForEach(x => x.DocumentNum = (maxNumber++).ToString());
                this.PerformedWorkActsToUpdate.Where(x => x.DocumentNum.IsEmpty())
                    .ForEach(x => x.DocumentNum = (maxNumber++).ToString());
            }
        }

        private void ImportPerformedWorkAct(ObjectCr objectCr, TypeWorkCr typeWork, ActDto act)
        {
            var performWorkActCr = this.GetPerformedWorkAct(objectCr.Id, typeWork.Id);

            var performWorkAct = performWorkActCr ?? new PerformedWorkAct
            {
                ObjectCr = objectCr,
                TypeWorkCr = typeWork
            };

            if (act.Volume.HasValue)
            {
                performWorkAct.Volume = act.Volume;
            }
            performWorkAct.DocumentNum = act.DocumentNum;
            performWorkAct.Sum = act.WorkPrice;
            performWorkAct.DateFrom = act.Date;
            performWorkAct.State = this.PerformedWorkActState;

            if (performWorkAct.Id == 0)
            {
                this.PerformedWorkActsToSave.Add(performWorkAct);
            }
            else
            {
                this.PerformedWorkActsToUpdate.Add(performWorkAct);
            }
        }

        private bool ImportContractCr(ObjectCr objectCr, TypeWorkCr typeWork, ContractDto importContract)
        {
            IGrouping<long, ContractCr> contractsByObjectCr;
            this.ContractCrDict.TryGetValue(objectCr.Id, out contractsByObjectCr);

            var oldContract = contractsByObjectCr?.FirstOrDefault(x => x.TypeWork?.Id == typeWork.Id);

            var newContract = oldContract;
            var newContragent = oldContract?.Contragent;

            if (newContract == null || oldContract.Contragent.Inn != importContract.ContragentInn || oldContract.DateFrom != importContract.StartDate)
            {
                newContract = oldContract?.BaseEntityClone(true) ?? new ContractCr
                {
                    ObjectCr = objectCr,
                    TypeWork = typeWork,
                    State = this.ContractCrApprovedState
                };

                newContragent = this.GetContragent(importContract.ContragentName, importContract.ContragentInn);

                if (oldContract != null)
                {
                    oldContract.State = this.ContractCrDraftState;
                }
            }

            if (newContragent == null)
            {
                this.RowError($"Не найден контрагент '{importContract.ContragentName}' ИНН: {importContract.ContragentInn}");
                return false;
            }

            newContract.Contragent = newContragent;

            newContract.DateStartWork = importContract.StartDate;
            newContract.DateFrom = importContract.StartDate;
            newContract.DocumentNum = importContract.Number;
            newContract.SumContract = importContract.AuctionPrice;
            newContract.DateEndWork = importContract.PlanEndDate;

            if (oldContract == null)
            {
                this.ContractCrToSave.Add(newContract);
            }
            else if (newContract.Id == oldContract.Id)
            {
                newContract.State = this.ContractCrApprovedState;
                this.ContractCrToUpdate.Add(newContract);
            }
            else
            {
                this.ContractCrToSave.Add(newContract);
                this.ContractCrToUpdate.Add(oldContract);
            }

            return true;
        }

        private bool ImportBuildContract(ObjectCr objectCr, TypeWorkCr typeWork, ContractDto importContract)
        {
            IGrouping<long, BuildContract> contractsByObjectCr;
            this.BuildContractDict.TryGetValue(objectCr.Id, out contractsByObjectCr);

            var oldContract = contractsByObjectCr?.FirstOrDefault(x => x.TypeWork?.Id == typeWork.Id);

            var newContract = oldContract;
            var newContragent = oldContract?.Contragent;
            var newBuilder = oldContract?.Builder;

            if (newContract == null
                || oldContract.Builder.Contragent.Inn != importContract.ContragentInn
                || oldContract.DateStartWork != importContract.StartDate
                || oldContract.DocumentDateFrom != importContract.StartDate)
            {
                newContract = oldContract?.BaseEntityClone(true) ?? new BuildContract
                {
                    ObjectCr = objectCr,
                    TypeWork = typeWork,
                    State = this.BuildContractApprovedState
                };
            }

            newContragent = this.GetContragent(importContract.ContragentName, importContract.ContragentInn);

            if (oldContract != null)
            {
                oldContract.State = this.BuildContractDraftState;
            }

            if (newContragent == null)
            {
                this.RowError($"Не найден контрагент '{importContract.ContragentName}' ИНН: {importContract.ContragentInn}");
                return false;
            }

            newBuilder = this.GetBuilder(newContragent);

            if (newBuilder == null)
            {
                this.RowError($"Не найден застройщик '{importContract.ContragentName}' ИНН: {importContract.ContragentInn}");
                return false;
            }

            newContract.Builder = newBuilder;

            newContract.DocumentDateFrom = importContract.StartDate;
            newContract.DateStartWork = importContract.StartDate;
            newContract.DocumentNum = importContract.Number;
            newContract.Sum = importContract.AuctionPrice;
            newContract.DateEndWork = importContract.PlanEndDate;

            if (oldContract == null)
            {
                this.BuildContractToSave.Add(newContract);
            }
            else if (newContract.Id == oldContract.Id)
            {
                newContract.State = this.BuildContractApprovedState;
                this.BuildContractToUpdate.Add(newContract);
            }
            else
            {
                this.BuildContractToSave.Add(newContract);
                this.BuildContractToUpdate.Add(oldContract);
            }

            return true;
        }

        private IGkhExcelProvider GetProvider(string extention)
        {
            var code = "ExcelEngineProvider";
            var provider = this.Container.Resolve<IGkhExcelProvider>(code);
            if (provider == null)
            {
                throw new Exception($"Не найдена реализация интерфейса IGkhExcelProvider с кодом '{code}'");
            }

            if (extention.ToLower() == "xlsx")
            {
                provider.UseVersionXlsx();
            }

            return provider;
        }

        private ImportResult GetResult(string message)
        {
            message += this.LogImportManager.GetInfo();

            var status = !this.LogImport.IsImported
                ? StatusImport.CompletedWithError
                : (this.LogImport.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);

            return new ImportResult(status, message, string.Empty, this.LogImportManager.LogFileId);
        }

        private bool IsContinue(GkhExcelCell[] row)
        {
            return row[0].Value.ToIntNullable() == null;
        }

        private bool ValidateRow(RowDto row)
        {
            var hasError = false;
            if (row.RoId == 0)
            {
                this.RowError("Обязательное поле 'Код МКД (в Барсе)' не заполнено");
                hasError = true;
            }
            else if (!this.RealityObjectIds.Contains(row.RoId))
            {
                this.RowError($"Не найден МКД с идентификатором '{row.RoId}'");
                hasError = true;
            }

            if (!row.SummValue.HasValue && !string.IsNullOrEmpty(row.MainWorkCode))
            {
                this.RowError("Обязательное поле 'Сумма по постановлению' не заполнено");
                hasError = true;
            }
            if (string.IsNullOrEmpty(row.ContractCrWinner) && !string.IsNullOrEmpty(row.ContractCrInn) && !string.IsNullOrEmpty(row.MainWorkCode)
                && row.ContractCrDate.HasValue)
            {
                this.RowError("Обязательное поле 'Победитель' не заполнено");
                hasError = true;
            }
            if (string.IsNullOrEmpty(row.ContractCrNumber) && !string.IsNullOrEmpty(row.ContractCrInn) && !string.IsNullOrEmpty(row.MainWorkCode)
                && row.ContractCrDate.HasValue)
            {
                this.RowError("Обязательное поле 'Номер договора' не заполнено");
                hasError = true;
            }
            if (!row.ContractCrAuctionPrice.HasValue && !string.IsNullOrEmpty(row.ContractCrInn) && !string.IsNullOrEmpty(row.MainWorkCode)
                && row.ContractCrDate.HasValue)
            {
                this.RowError("Обязательное поле 'Цена по итогам аукциона (в соответствии с заключенными договорами)' не заполнено");
                hasError = true;
            }
            if (!row.ContractCrPlanEndDate.HasValue && !string.IsNullOrEmpty(row.ContractCrInn) && !string.IsNullOrEmpty(row.MainWorkCode)
                && row.ContractCrDate.HasValue)
            {
                this.RowError("Обязательное поле 'Плановая дата завершения работ (услуг) по договору подряда по разработке ПСД' не заполнено");
                hasError = true;
            }
            if (!row.CalculationVolume.HasValue && !string.IsNullOrEmpty(row.MainWorkCode) && !string.IsNullOrEmpty(row.ProgramYear))
            {
                this.RowError("Обязательное поле 'Объемы из калькуляции' не заполнено");
                hasError = true;
            }
            if (!row.PcdWorkPrice.HasValue && !string.IsNullOrEmpty(row.ContractCrWorkCode) && !string.IsNullOrEmpty(row.ProgramYear))
            {
                this.RowError("Обязательное поле 'Стоимость услуги ПСД' не заполнено");
                hasError = true;
            }
            if (!row.PcdContractDate.HasValue && !string.IsNullOrEmpty(row.ContractCrWorkCode) && !string.IsNullOrEmpty(row.ProgramYear))
            {
                this.RowError("Обязательное поле 'Дата подписания акта приемки ПСД собственниками' не заполнено");
                hasError = true;
            }
            if (string.IsNullOrEmpty(row.SmrWinner) && !string.IsNullOrEmpty(row.MainWorkCode) && !string.IsNullOrEmpty(row.SmrInn)
                && row.SmrContractDate.HasValue)
            {
                this.RowError("Обязательное поле 'Победитель' не заполнено");
                hasError = true;
            }
            if (string.IsNullOrEmpty(row.SmrContractNumber) && !string.IsNullOrEmpty(row.MainWorkCode) && !string.IsNullOrEmpty(row.SmrInn)
                && row.SmrContractDate.HasValue)
            {
                this.RowError("Обязательное поле 'Номер договора' не заполнено");
                hasError = true;
            }
            if (!row.SmrAuctionPrice.HasValue && !string.IsNullOrEmpty(row.MainWorkCode) && !string.IsNullOrEmpty(row.SmrInn)
                && row.SmrContractDate.HasValue)
            {
                this.RowError("Обязательное поле 'Цена по итогам аукциона' не заполнено");
                hasError = true;
            }
            if (!row.SmrContractPlanEndDate.HasValue && !string.IsNullOrEmpty(row.MainWorkCode) && !string.IsNullOrEmpty(row.SmrInn)
                && row.SmrContractDate.HasValue)
            {
                this.RowError("Обязательное поле 'Плановая дата завершения работ по СМР по договору подряда' не заполнено");
                hasError = true;
            }
            if (string.IsNullOrEmpty(row.ProgramYear))
            {
                this.RowError("Обязательное поле 'Год завершения работ' не заполнено");
                hasError = true;
            }
            if (!row.Ks2WorkPrice.HasValue && !string.IsNullOrEmpty(row.MainWorkCode))
            {
                this.RowError("Обязательное поле 'Стоимость работ по КС2 (принятым комиссией)' не заполнено");
                hasError = true;
            }
            if (!row.Ks2Date.HasValue && !string.IsNullOrEmpty(row.MainWorkCode))
            {
                this.RowError("Обязательное поле 'Дата КС2' не заполнено");
                hasError = true;
            }
            if (string.IsNullOrEmpty(row.BuildControlWinner) && !string.IsNullOrEmpty(row.BuildControlInn) && !string.IsNullOrEmpty(row.BuildControlWorkCode)
                && row.BuildControlContractDate.HasValue)
            {
                this.RowError("Обязательное поле 'Победитель' не заполнено");
                hasError = true;
            }
            if (string.IsNullOrEmpty(row.BuildControlContractNumber) && !string.IsNullOrEmpty(row.BuildControlInn)
                && !string.IsNullOrEmpty(row.BuildControlWorkCode) && row.BuildControlContractDate.HasValue)
            {
                this.RowError("Обязательное поле 'Номер договора' не заполнено");
                hasError = true;
            }
            if (!row.BuildControlAuctionPrice.HasValue && !string.IsNullOrEmpty(row.BuildControlInn) && !string.IsNullOrEmpty(row.BuildControlWorkCode)
                && row.BuildControlContractDate.HasValue)
            {
                this.RowError("Обязательное поле 'Цена по итогам аукциона' не заполнено");
                hasError = true;
            }
            if (!row.BuildControlContractPlanEndDate.HasValue && !string.IsNullOrEmpty(row.BuildControlInn)
                && !string.IsNullOrEmpty(row.BuildControlWorkCode) && row.BuildControlContractDate.HasValue)
            {
                this.RowError("Обязательное поле 'Плановая дата завершения услуг по СК' не заполнено");
                hasError = true;
            }
            if (!row.ActWorkPrice.HasValue && !string.IsNullOrEmpty(row.BuildControlWorkCode))
            {
                this.RowError("Обязательное поле 'Стоимость услуг СК (принятым комиссией)' не заполнено");
                hasError = true;
            }
            if (!row.ActDate.HasValue && !string.IsNullOrEmpty(row.BuildControlWorkCode))
            {
                this.RowError("Обязательное поле 'Дата акта' не заполнено");
                hasError = true;
            }

            return !hasError;
        }

        private void RowError(string message)
        {
            this.LogImport.Error($"{this.Name}. Строка {this.readRowIdx}", message);
        }

        private ObjectCr GetObjectCr(long realityObjectId, long programCrId)
        {
            Dictionary<long, long> programDict;
            var objectCrId = 0L;

            if (!this.ObjectCrIds.TryGetValue(realityObjectId, out programDict) || !(programDict?.TryGetValue(programCrId, out objectCrId) ?? false)
                || objectCrId == 0)
            {
                return null;
            }

            return new ObjectCr(new RealityObject { Id = realityObjectId }, new ProgramCr { Id = programCrId }) { Id = objectCrId };
        }

        private TypeWorkCr GetTypeWorkCr(long objectCrId, string workCode)
        {
            Dictionary<string, TypeWorkCr> workDict;
            TypeWorkCr typeWorkCr = null;

            if (this.TypeWorkCrDict.TryGetValue(objectCrId, out workDict))
            {
                workDict?.TryGetValue(workCode, out typeWorkCr);
            }

            return typeWorkCr;
        }

        private Contragent GetContragent(string name, string inn)
        {
            Dictionary<string, long> innDict;
            var contragentId = 0L;

            if (!this.ContragentDict.TryGetValue(name, out innDict) || !(innDict?.TryGetValue(inn, out contragentId) ?? false)
                || contragentId == 0)
            {
                return null;
            }

            return new Contragent { Id = contragentId };
        }

        private Builder GetBuilder(Contragent contragent)
        {
            var builderId = this.BuilderDict.Get(contragent.Id);

            if (builderId == 0)
            {
                return null;
            }

            return new Builder { Id = builderId };
        }

        private PerformedWorkAct GetPerformedWorkAct(long objectCrId, long typeWorkId)
        {
            Dictionary<long, PerformedWorkAct> typeWorkDict;
            PerformedWorkAct performedWorkAct = null;

            if (this.PerformedWorkActDict.TryGetValue(objectCrId, out typeWorkDict))
            {
                typeWorkDict?.TryGetValue(typeWorkId, out performedWorkAct);
            }

            return performedWorkAct;
        }

        private State GetStartState<T>()
        {
            var typeId = this.StateProvider.GetStatefulEntityInfo(typeof(T))?.TypeId;

            return this.StateRepository.GetAll()
                .Where(x => x.TypeId == typeId)
                .FirstOrDefault(x => x.StartState);
        }

        private State GetFinalState<T>()
        {
            var typeId = this.StateProvider.GetStatefulEntityInfo(typeof(T))?.TypeId;

            return this.StateRepository.GetAll()
                .Where(x => x.TypeId == typeId)
                .FirstOrDefault(x => x.FinalState);
        }

        private class ActDto
        {
            public string DocumentNum { get; set; }
            public decimal? Volume { get; set; }
            public decimal? WorkPrice { get; set; }
            public DateTime? Date { get; set; }
        }

        private class ContractDto
        {
            public string ContragentName { get; set; }
            public string ContragentInn { get; set; }
            public string Number { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? PlanEndDate { get; set; }
            public decimal? AuctionPrice { get; set; }
        }

        private class RowDto
        {
            /// <summary> Столбец 7 </summary>
            public long RoId { get; set; }
            /// <summary> Столбец 10 </summary>
            public string MainWorkCode { get; set; }
            /// <summary> Столбец 11 </summary>
            public decimal? SummValue { get; set; }

            /// <summary> Столбец 16 </summary>
            public string ContractCrWinner { get; set; }
            /// <summary> Столбец 17 </summary>
            public string ContractCrInn { get; set; }
            /// <summary> Столбец 18 </summary>
            public string ContractCrWorkCode { get; set; }
            /// <summary> Столбец 19 </summary>
            public string ContractCrNumber { get; set; }
            /// <summary> Столбец 20 </summary>
            public DateTime? ContractCrDate { get; set; }
            /// <summary> Столбец 21 </summary>
            public decimal? ContractCrAuctionPrice { get; set; }
            /// <summary> Столбец 22 </summary>
            public DateTime? ContractCrPlanEndDate { get; set; }
            /// <summary> Столбец 34 </summary>
            public decimal? CalculationVolume { get; set; }

            /// <summary> Столбец 44 </summary>
            public decimal? PcdWorkPrice { get; set; }
            /// <summary> Столбец 46 </summary>
            public DateTime? PcdContractDate { get; set; }

            /// <summary> Столбец 53 </summary>
            public string SmrWinner { get; set; }
            /// <summary> Столбец 54 </summary>
            public string SmrInn { get; set; }
            /// <summary> Столбец 55 </summary>
            public string SmrContractNumber { get; set; }
            /// <summary> Столбец 56 </summary>
            public DateTime? SmrContractDate { get; set; }
            /// <summary> Столбец 57 </summary>
            public decimal? SmrAuctionPrice { get; set; }
            /// <summary> Столбец 60 </summary>
            public DateTime? SmrContractPlanEndDate { get; set; }
            /// <summary> Столбец 61 </summary>
            public string ProgramYear { get; set; }

            /// <summary> Столбец 73 </summary>
            public decimal? Ks2WorkPrice { get; set; }
            /// <summary> Столбец 74 </summary>
            public DateTime? Ks2Date { get; set; }

            /// <summary> Столбец 82 </summary>
            public string BuildControlWinner { get; set; }
            /// <summary> Столбец 83 </summary>
            public string BuildControlInn { get; set; }
            /// <summary> Столбец 84 </summary>
            public string BuildControlWorkCode { get; set; }
            /// <summary> Столбец 85 </summary>
            public string BuildControlContractNumber { get; set; }
            /// <summary> Столбец 86 </summary>
            public DateTime? BuildControlContractDate { get; set; }
            /// <summary> Столбец 88 </summary>
            public decimal? BuildControlAuctionPrice { get; set; }
            /// <summary> Столбец 89 </summary>
            public DateTime? BuildControlContractPlanEndDate { get; set; }

            /// <summary> Столбец 90 </summary>
            public decimal? ActWorkPrice { get; set; }
            /// <summary> Столбец 91 </summary>
            public DateTime? ActDate { get; set; }
        }
    }
}
