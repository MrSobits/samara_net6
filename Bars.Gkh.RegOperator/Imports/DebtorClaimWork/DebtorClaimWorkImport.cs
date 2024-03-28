namespace Bars.Gkh.RegOperator.Imports.DebtorClaimWork
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Security;
    using Bars.B4.Modules.States;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Config;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.Impl;
    using Bars.Gkh.Modules.ClaimWork.DomainService;
    using Bars.Gkh.Modules.ClaimWork.DomainService.States;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.Debtor;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService;
    using Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService.States.TransitionRules;
    using Bars.Gkh.Utils;

    using NHibernate.Linq;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Испорт ПИР
    /// </summary>
    public partial class DebtorClaimWorkImport : GkhImportBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Ключ импорта
        /// </summary>
        public override string Key => DebtorClaimWorkImport.Id;

        /// <summary>
        /// Код группировки импорта (например группировка в меню)
        /// </summary>
        public override string CodeImport => nameof(DebtorClaimWorkImport);

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public override string Name => "Импорт данных в ПИР";

        /// <summary>
        /// Разрешенные расширения файлов
        /// </summary>
        public override string PossibleFileExtensions => "xls,xlsx";

        /// <summary>
        /// Права
        /// </summary>
        public override string PermissionName => "Import.DebtorClaimWorkImport";

        /// <summary>
        /// Менеджер пользователей
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

        public new ILogImport LogImport { get; set; }

        public new ILogImportManager LogImportManager { get; set; }

        public IDomainService<BasePersonalAccount> PersonalAccountDomain { get; set; }

        public IDomainService<Debtor> DebtorDomain { get; set; }

        public IDomainService<ClaimWorkAccountDetail> ClaimWorkAccountDetailDomain { get; set; }

        public IDomainService<State> StateDomain { get; set; }

        public IDebtorJurInstitutionCache DebtorJurInstitutionCache { get; set; }

        public IDebtorCalcService DebtorCalcService { get; set; }

        public IStateProvider StateProvider { get; set; }

        public IClwStateProvider ClwStateProv { get; set; }

        private const string Success = "Строка успешно импортирована";

        private User user;
        private State defaultState;
        private State finalState;

        private Dictionary<string, int> headersDict;
        private readonly Dictionary<Tuple<int, string>, Dictionary<bool, List<string>>> logDict
            = new Dictionary<Tuple<int, string>, Dictionary<bool, List<string>>>();

        private readonly List<DebtorClaimWorkRecord> records = new List<DebtorClaimWorkRecord>();
        private Dictionary<string, DebtorClaimWorkRecord> recordDict;
        private BasePersonalAccount[] accounts;
        private Dictionary<long, string> accountOwnerDict;

        private List<DebtorClaimWork> claimworksWithLawsuit;
        private List<DebtorClaimWork> claimworksWithCourtOrderClaim;

        private List<Debtor> debtorsForSave;
        private List<DebtorClaimWork> claimworksForSave;
        private List<ClaimWorkAccountDetail> accountDetailsForSave;
        private List<DocumentClw> documentsForSave;

        /// <summary>
        /// Валидация
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <param name="message">Сообщение</param>
        /// <returns>Результат</returns>
        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;

            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            var extention = baseParams.Files["FileImport"].Extention;

            var fileExtentions = this.PossibleFileExtensions.Contains(",") ? this.PossibleFileExtensions.Split(',') : new[] { this.PossibleFileExtensions };
            if (fileExtentions.All(x => x != extention))
            {
                message = $"Необходимо выбрать файл с допустимым расширением: {this.PossibleFileExtensions}";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Импорт
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <param name="ctx">Контекст</param>
        /// <param name="indicator">Индикатор прогресса</param>
        /// <param name="ct">Уведомление об отмене</param>
        /// <returns>Результат</returns>
        protected override ImportResult Import(BaseParams baseParams, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            var file = baseParams.Files["FileImport"];
            var fileExtention = file.Extention;

            this.InitLog(file.FileName);

            indicator.Report(null, 0, "Чтение из файла");

            this.ProcessData(file.Data, fileExtention);

            indicator.Report(null, 5, "Подготовка данных");

            this.InitData();

            indicator.Report(null, 10, "Создание должников и ПИР");

            this.CreateDebtors();

            indicator.Report(null, 30, "Создание документов");

            this.CreateDocuments();

            indicator.Report(null, 50, "Сохранение данных");

            this.SaveChanges();

            indicator.Report(null, 70, "Обновление статусов");

            this.UpdateStates();

            indicator.Report(null, 90, "Запись логов");

            this.WriteLogs();

            this.LogImportManager.Add(file, this.LogImport);
            this.LogImportManager.Save();

            return new ImportResult();
        }

        /// <summary>
        /// Инициализация лога
        /// </summary>
        /// <param name="fileName">Название файла</param>
        public new void InitLog(string fileName)
        {
            this.LogImportManager.FileNameWithoutExtention = fileName;
            this.LogImportManager.UploadDate = DateTime.Now;

            this.LogImport.SetFileName(fileName);
            this.LogImport.ImportKey = this.Key;
        }

        private void AddLog(int rowNum, string accNum, string message, bool success = true)
        {
            var tuple = new Tuple<int, string>(rowNum, accNum);

            if (!this.logDict.ContainsKey(tuple))
            {
                this.logDict[tuple] = new Dictionary<bool, List<string>>();
            }

            var log = this.logDict[tuple];

            if (!log.ContainsKey(success))
            {
                log[success] = new List<string>();
            }

            log[success].Add(message);
        }

        private void WriteLogs()
        {
            foreach (var log in this.logDict.OrderBy(x => x.Key))
            {
                var rowNumber = $"Строка {log.Key.Item1}, лс {log.Key.Item2}";

                foreach (var rowLog in log.Value)
                {
                    if (rowLog.Key)
                    {
                        this.LogImport.Info(rowNumber, rowLog.Value.AggregateWithSeparator("."));
                    }
                    else
                    {
                        this.LogImport.Warn(rowNumber, rowLog.Value.AggregateWithSeparator("."));
                    }
                }
            }

            this.LogImport.CountChangedRows = 0;

            this.LogImport.CountAddedRows = this.debtorsForSave.Count;
        }

        private void InitData()
        {
            this.debtorsForSave = new List<Debtor>();
            this.claimworksForSave = new List<DebtorClaimWork>();
            this.accountDetailsForSave = new List<ClaimWorkAccountDetail>();
            this.documentsForSave = new List<DocumentClw>();

            this.claimworksWithLawsuit = new List<DebtorClaimWork>();
            this.claimworksWithCourtOrderClaim = new List<DebtorClaimWork>();

            var entityInfo = this.StateProvider.GetStatefulEntityInfo(typeof(DebtorClaimWork));
            this.defaultState = this.StateDomain.GetAll()
                .FirstOrDefault(x => x.TypeId == entityInfo.TypeId && x.StartState);

            this.finalState = this.StateDomain.GetAll()
                .FirstOrDefault(x => x.TypeId == entityInfo.TypeId && x.FinalState);

            this.user = this.UserManager.GetActiveUser();
        }

        private void CreateDebtors()
        {
            var allAccounts = this.PersonalAccountDomain.GetAll()
                .WhereContains(x => x.PersonalAccountNum, this.records.Select(x => x.AccountNumber))
                .Select(x => new
                {
                    x.PersonalAccountNum,
                    x.Id
                })
                .ToDictionary(x => x.Id, x=> x.PersonalAccountNum);

            var accountIds = allAccounts.Select(x => x.Key).ToArray();

            var notFoundAccounts = this.records.Where(x => !allAccounts.Values.Contains(x.AccountNumber));
            foreach (var record in notFoundAccounts)
            {
                this.AddLog(record.RowNumber, record.AccountNumber, $"ЛС №{record.AccountNumber} не найден", false);
            }

            var accountsWithDebtors = this.DebtorDomain
                .GetAll()
                .WhereContains(x => x.PersonalAccount.Id, accountIds)
                .Select(x => x.PersonalAccount.Id)
                .ToArray();

            var accountIdsWithDebtorsAndClaimwork = this.ClaimWorkAccountDetailDomain
                .GetAll()
                .WhereContains(x => x.PersonalAccount.Id, accountIds)
                .Select(x => x.PersonalAccount.Id)
                .ToArray()
                .Union(accountsWithDebtors)
                .Distinct()
                .ToArray();

            var accountsWithDebtorsAndClaimwork = accountIdsWithDebtorsAndClaimwork.Select(x => allAccounts[x]);

            var recordsWithClaimork = this.records.Where(x => accountsWithDebtorsAndClaimwork.Any(y => y == x.AccountNumber));
            foreach (var record in recordsWithClaimork)
            {
                this.AddLog(record.RowNumber, record.AccountNumber, $"По лc уже создан должник или ведется ПИР", false);
            }

            this.accounts = this.PersonalAccountDomain.GetAll()
                .Fetch(x => x.AccountOwner)
                .Where(x => accountIds.Contains(x.Id))
                .Where(x => !accountIdsWithDebtorsAndClaimwork.Contains(x.Id))
                .ToArray();

            this.recordDict = this.records.ToDictionary(x => x.AccountNumber, x => x);          
            this.DebtorJurInstitutionCache.InitCache(this.accounts.Select(x => x.Room.RealityObject.Id).ToArray());  

            foreach (var account in this.accounts)
            {
                var record = this.recordDict[account.PersonalAccountNum];

                int months;
                int days = Utils.CalculateDaysAndMonths(record.StartDate, out months);
                var debtor = this.CreateDebtor(account, record, days, months);

                var isDebtor = this.DebtorCalcService.CheckDebtor(debtor);
                if (!isDebtor.Success)
                {
                    this.AddLog(record.RowNumber, record.AccountNumber, isDebtor.Message, false);
                    continue;
                }
                this.debtorsForSave.Add(debtor);

                var debtorClaimwork = this.CreateDebtorClaimwork(account, record, days, months);

                switch (record.ApplicationType)
                {
                    case ApplicationType.Lawsuit:
                        this.claimworksWithLawsuit.Add(debtorClaimwork);
                        break;

                    case ApplicationType.CourtOrderClaim:
                        this.claimworksWithCourtOrderClaim.Add(debtorClaimwork);
                        break;

                    case ApplicationType.NoDocuments:
                        this.AddLog(record.RowNumber, record.AccountNumber, DebtorClaimWorkImport.Success, true);
                        break;
                }
            }
        }
        
        private Debtor CreateDebtor(BasePersonalAccount account, DebtorClaimWorkRecord record, int days, int months)
        {
            var debtor = new Debtor
            {
                PersonalAccount = account,
                DebtSum = record.DebtSum,
                PenaltyDebt = record.PenaltyDebt,
                ExpirationDaysCount = days,
                ExpirationMonthCount = months,
                StartDate = record.StartDate
            };

            this.DebtorJurInstitutionCache.SetJurInstitution(debtor, account);

            return debtor;
        }

        private DebtorClaimWork CreateDebtorClaimwork(BasePersonalAccount account, DebtorClaimWorkRecord record, int days, int months)
        {
            DebtorClaimWork debtorClaimwork;

            if (account.AccountOwner.OwnerType == PersonalAccountOwnerType.Individual)
            {
                debtorClaimwork = new IndividualClaimWork
                {
                    AccountOwner = account.AccountOwner,
                    OrigChargeDebt = record.DebtSum,
                    OrigPenaltyDebt = record.PenaltyDebt,
                    CurrChargeDebt = record.DebtSum,
                    CurrPenaltyDebt = record.PenaltyDebt,
                    State = this.defaultState,
                    User = this.user
                };
            }
            else
            {
                debtorClaimwork = new LegalClaimWork
                {
                    AccountOwner = account.AccountOwner,
                    OrigChargeDebt = record.DebtSum,
                    OrigPenaltyDebt = record.PenaltyDebt,
                    CurrChargeDebt = record.DebtSum,
                    CurrPenaltyDebt = record.PenaltyDebt,
                    State = this.defaultState,
                    User = this.user
                };
            }
            
            var accountDetail = new ClaimWorkAccountDetail
            {
                PersonalAccount = account,
                ClaimWork = debtorClaimwork,
                OrigChargeDebt = record.DebtSum,
                OrigPenaltyDebt = record.PenaltyDebt,
                CurrChargeDebt = record.DebtSum,
                CurrPenaltyDebt = record.PenaltyDebt,
                CountDaysDelay = days,
                CountMonthDelay = months,
                StartingDate = record.StartDate
            };

            debtorClaimwork.AddAccountDetail(accountDetail);

            this.claimworksForSave.Add(debtorClaimwork);
            this.accountDetailsForSave.Add(accountDetail);

            return debtorClaimwork;
        }

        private void CreateDocuments()
        {
            var allIds = this.claimworksWithLawsuit.Select(x => x.Id)
                .Union(this.claimworksWithCourtOrderClaim.Select(x => x.Id))
                .ToHashSet();

            this.ClwStateProv.InitCache(allIds);
            this.MassCreateDocs(ApplicationType.Lawsuit, this.claimworksWithLawsuit);
            this.MassCreateDocs(ApplicationType.CourtOrderClaim, this.claimworksWithCourtOrderClaim);
        }     

        private void SaveChanges()
        {
            TransactionHelper.InsertInManyTransactions(this.Container, this.debtorsForSave, useStatelessSession: true);
            TransactionHelper.InsertInManyTransactions(this.Container, this.claimworksForSave, useStatelessSession: true);
            TransactionHelper.InsertInManyTransactions(this.Container, this.accountDetailsForSave, useStatelessSession: true);
            TransactionHelper.InsertInManyTransactions(this.Container, this.documentsForSave, useStatelessSession: true);
        }
            
        private void MassCreateDocs(ApplicationType type, List<DebtorClaimWork> claimWorkList)
        {
            var docType = type == ApplicationType.Lawsuit
                ? ClaimWorkDocumentType.Lawsuit
                : ClaimWorkDocumentType.CourtOrderClaim;

            var docRules = this.Container.ResolveAll<IClaimWorkDocRule>();

            var docClwRepo = this.Container.ResolveRepository<DocumentClw>();
            var courtClaimRepo = this.Container.ResolveRepository<CourtOrderClaim>();

            try
            {
                var rule = docRules.FirstOrDefault(x => x.ResultTypeDocument == docType);

                if (rule != null)
                {
                    var rules = this.GetNeedDocRules();

                    var claimWorks = claimWorkList
                        .Where(x => this.ClwStateProv.GetAvailableTransitions(x, rules, true).Contains(docType))
                        .ToArray();

                    var wrongAccounts = claimWorkList.SelectMany(x => x.AccountDetails)
                        .Select(x => x.PersonalAccount.PersonalAccountNum)
                        .AsEnumerable()
                        .Except(claimWorks
                            .SelectMany(x => x.AccountDetails)
                            .Select(x => x.PersonalAccount.PersonalAccountNum)
                            .AsEnumerable())
                        .ToArray();

                    foreach (var account in wrongAccounts)
                    {
                        var record = this.recordDict[account];
                        this.AddLog(record.RowNumber, account, $"Документ {type.GetDisplayName()} не может быть создан по текущим правилам ПИР", false);
                    }

                    var result = rule.FormDocument(claimWorks, false);

                    result.ForEach(
                        x =>
                        {
                            var element = type == ApplicationType.Lawsuit
                                ? (Lawsuit)x
                                : (CourtOrderClaim)x;

                            var record = this.recordDict[((DebtorClaimWork) x.ClaimWork).AccountDetails.First().PersonalAccount.PersonalAccountNum];

                            x.DocumentDate = record.ApplicationDate;
                            element.DebtSum = record.LawsuitDebtSum;
                            element.CbDebtSum = record.LawsuitDebtSum;
                            element.PenaltyDebt = record.LawsuitPenaltyDebt;
                            element.CbPenaltyDebt = record.LawsuitPenaltyDebt;
                            this.documentsForSave.Add(element);

                            this.AddLog(record.RowNumber, record.AccountNumber, DebtorClaimWorkImport.Success, true);
                        });
                }
            }
            finally
            {
                this.Container.Release(docRules);
                this.Container.Release(docClwRepo);
                this.Container.Release(courtClaimRepo);
            }
        }

        private List<IClwTransitionRule> GetNeedDocRules()
        {
            return new List<IClwTransitionRule>
            {
                new DebtorDocumentExistsRule(this.Container),
                new IncludeLawsuitRule(this.Container),
                new IncludeCourtOrderClaimRule(this.Container),
                new IncludePretensionRule(this.Container),
                new IncludeNotificationRule(this.Container)
            };
        }

        private void UpdateStates()
        {
            var configProv = this.Container.Resolve<IGkhConfigProvider>();

            using (this.Container.Using(configProv))
            {
                var rules = new IClwTransitionRule[]
                {
                    new IncludeHiLevelDocRule(this.Container), 
                    new IncludeLawsuitRule(this.Container),
                    new IncludeCourtOrderClaimRule(this.Container),
                    new IncludePretensionRule(this.Container),
                    new IncludeNotificationRule(this.Container)
                };
                var nextStateDict = this.GetMassNextState(this.claimworksForSave, rules);

                this.claimworksForSave.ForEach(x =>
                {
                    x.SetDebtorState(nextStateDict[x.Id], x.DebtorState == DebtorState.PaidDebt ? this.finalState : this.defaultState);
                });

                TransactionHelper.InsertInManyTransactions(this.Container, this.claimworksForSave, useStatelessSession: true);
            }
        }

        private Dictionary<long, DebtorState> GetMassNextState(
            List<DebtorClaimWork> claimWorks,
            IEnumerable<IClwTransitionRule> rules)
        {
            var debtorStateProvider = this.Container.Resolve<IDebtorStateProvider>();
            using (this.Container.Using(debtorStateProvider))
            {
                debtorStateProvider.InitCache(claimWorks.Select(x => x.Id));

                var result = new Dictionary<long, DebtorState>();

                foreach (var claimwork in claimWorks)
                {
                    var avaiableDocTypes = this.ClwStateProv.GetAvailableTransitions(claimwork, rules, true);
                    result.Add(claimwork.Id, debtorStateProvider.GetState(claimwork, avaiableDocTypes));
                }

                return result;
            }
        }
    }
}