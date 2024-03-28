namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.GeneralState;
    using Bars.Gkh.Import;
    using Bars.Gkh.RegOperator.Tasks.TaskHelpers;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using Domain.ImportExport.ImportMaps;
    using Domain.ImportExport.Mapping;
    using Domain.ImportExport.Serializers;

    using Entities;

    using Enums;

    using Gkh.Domain;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.Entities;

    using Import;

    using Wcf.Contracts.PersonalAccount;

    /// <summary>
    /// Провайдер импорта документов из банка
    /// </summary>
    public class BankDocumentImportProvider : IBankDocumentImportProvider
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Поставщик сессий
        /// </summary>
        public ISessionProvider SessionProvider { get; set; }

        /// <summary>
        /// Менеждер для сохранения изменения состояний
        /// </summary>
        public IGeneralStateHistoryManager StateHistoryManager { get; set; }

        /// <summary>
        /// Домен-сервис для Импорт документа из банка
        /// </summary>
        public IDomainService<BankDocumentImport> BankDocumentImportDomain { get; set; }

        /// <summary>
        /// Домен-сервис для Лицевой счет
        /// </summary>
        public IRepository<BasePersonalAccount> PersonalAccountDomain { get; set; }

        /// <summary>
        /// Домен-сервис для Импортируемая оплата
        /// </summary>
        public IDomainService<ImportedPayment> ImportedPaymentDomain { get; set; }

        /// <summary>
        /// Домен-сервис для Расчетно-кассовый центр
        /// </summary>
        public IDomainService<CashPaymentCenter> PaymentCenterDomain { get; set; }

        /// <summary>
        /// Домен-сервис для Лицевой счет расчетно-кассового центра
        /// </summary>
        public IDomainService<CashPaymentCenterPersAcc> PersAccntPaymentCenterDomain { get; set; }

        /// <summary>
        /// Домен-сервис для Жилой дом расчетно-кассового центра
        /// </summary>
        public IDomainService<CashPaymentCenterRealObj> RealObjPaymentCenterDomain { get; set; }

        /// <summary>
        /// Домен-сервис для Платежный агент
        /// </summary>
        public IDomainService<PaymentAgent> PaymentAgentDomain { get; set; }

        /// <summary>
        /// Домен-сервис для Счет оплат дома
        /// </summary>
        public IDomainService<RealityObjectPaymentAccount> RealityObjectPaymentAccountDomain { get; set; }

        /// <summary>
        /// Индикатор прогресса выполнения
        /// </summary>
        public Action<int, string> IndicateAction { get; set; }

        private Dictionary<string, long> accountsIdsByAccountNumber;
        private Dictionary<string, long> accountsIdsByExternalNum;
        private Dictionary<string, long> accountsIdsByExternalNumWithoutCenterCode;
        private Dictionary<long, AccountInfo> accountById;
        private readonly Dictionary<PersonalAccountPaymentInfoIn, ImportLog> logDict = new Dictionary<PersonalAccountPaymentInfoIn, ImportLog>();
        private readonly Dictionary<long, string> accountNumberByRealityObjectMap = new Dictionary<long, string>();

        /// <summary>
        /// Создать Импорт документа из банка
        /// </summary>
        /// <param name="paymentInfo">Строка импорта платежки</param>
        /// <param name="importType">Тип импорта</param>
        /// <param name="logImport">Лог импорта</param>
        /// <param name="providerCode">Код провайдера</param>
        /// <param name="fileName">Имя файла</param>
        public BankDocumentImport CreateBankDocumentImport(
            IEnumerable<PersonalAccountPaymentInfoIn> paymentInfo,
            BankDocumentImportType importType,
            ILogImport logImport,
            string providerCode,
            string fileName)
        {
            this.InitDicts(paymentInfo);

            var importedSum = 0m;
            var bankDocImport = new BankDocumentImport
            {
                ImportDate = DateTime.Now,
                DocumentType = "Документ оплаты",
                Status = logImport.CountError > 0
                    ? BankDocumentImportStatus.Failed
                    : (logImport.CountWarning > 0
                        ? BankDocumentImportStatus.ImportedWithWarnings
                        : BankDocumentImportStatus.SuccessfullyImported)
            };

            var listImportedPayment = new List<ImportedPayment>();
            var paymentInfoCount = paymentInfo.Count();
            var docProgressPart = 90;
            var startProgress = 30;

            var docProgress = new ProgressSender(paymentInfoCount, this.IndicateAction, docProgressPart, startProgress);
            var current = 1;

            foreach (var pi in paymentInfo)
            {
                var addInfo = new AdditionalImportPaymentInfo
                {
                    ReceiptId = pi.ReceiptId,
                    DocDate = pi.DocumentDate.IsValid() ? pi.DocumentDate : pi.PaymentDate,
                    DocNum = pi.DocumentNumber,
                    PaymentInfo = pi,
                    ImportType = importType,
                    ProviderCode = providerCode,
                    FileName = fileName
                };

                var personalAccount = this.GetAccount(pi);
                if (personalAccount == null)
                {
                    string logMessage;
                    switch (pi.OwnerType)
                    {
                        case PersonalAccountPaymentInfoIn.AccountType.Legal:
                            logMessage = string.Format("Абонент с ИНН '{0}' и КПП '{1}' (адрес {2}) не найден. ",
                                pi.Inn,
                                pi.Kpp,
                                pi.PayerAddress);
                            break;
                        default:
                            logMessage = string.Format("Лицевой счет '{0}' или абонент '{1}' (адрес {2}) не найдены. ",
                                this.GetAccountNumber(personalAccount, pi),
                                string.Format("{0} {1} {2}", pi.Surname, pi.Name, pi.Patronymic).Trim(),
                                pi.PayerAddress);
                            break;
                    }

                    this.AddLog(pi, ImportState.Warning, logMessage);
                }
                if (pi.TargetPaid > 0)
                {
                    importedSum += pi.TargetPaid;
                    listImportedPayment.Add(this.CreateImportedPayment(bankDocImport, personalAccount, addInfo, pi.TargetPaid));

                    pi.TargetPaid = 0;

                    this.AddLog(pi,
                        ImportState.Success,
                        string.Format("Оплата ЛС '{0}' за дату '{1}' загружена", this.GetAccountNumber(personalAccount, pi),
                            pi.PaymentDate.ToString("dd.MM.yyyy")));
                }

                if (pi.SumPaid > 0)
                {
                    importedSum += pi.SumPaid;
                    listImportedPayment.Add(this.CreateImportedPayment(bankDocImport, personalAccount, addInfo, pi.SumPaid));

                    pi.SumPaid = 0;

                    this.AddLog(pi,
                        ImportState.Success,
                        string.Format("Оплата ЛС '{0}' за дату '{1}' загружена", this.GetAccountNumber(personalAccount, pi),
                            pi.PaymentDate.ToString("dd.MM.yyyy")));
                }

                if (pi.PenaltyPaid > 0)
                {
                    importedSum += pi.PenaltyPaid;
                    listImportedPayment.Add(this.CreateImportedPayment(bankDocImport, personalAccount, addInfo, pi.PenaltyPaid));

                    pi.PenaltyPaid = 0;

                    this.AddLog(pi,
                        ImportState.Success,
                        string.Format("Оплата ЛС '{0}' за дату '{1}' загружена", this.GetAccountNumber(personalAccount, pi),
                            pi.PaymentDate.ToString("dd.MM.yyyy")));
                }

                if (pi.SocialSupport > 0)
                {
                    if (personalAccount != null && !string.Equals(personalAccount.AccountOwnerName, pi.Fio, StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.AddLog(pi,
                            ImportState.Warning,
                            string.Format("Номер ЛС {0} не соответствует абоненту {1}", this.GetAccountNumber(personalAccount, pi),
                                pi.Fio));
                    }
                    else
                    {
                        importedSum += pi.SocialSupport;
                        listImportedPayment.Add(this.CreateImportedPayment(bankDocImport, personalAccount, addInfo, pi.SocialSupport));

                        pi.SocialSupport = 0;

                        this.AddLog(pi,
                            ImportState.Success,
                            string.Format("Соц. поддержка по ЛС '{0}' на сумму '{1}' загружена", this.GetAccountNumber(personalAccount, pi),
                                pi.SocialSupport));
                    }
                }

                if (pi.Refund > 0)
                {
                    importedSum += pi.Refund;
                    listImportedPayment.Add(this.CreateImportedPayment(bankDocImport, personalAccount, addInfo, pi.Refund));

                    pi.Refund = 0;

                    this.AddLog(pi,
                        ImportState.Success,
                        string.Format("Отмена оплаты ЛС '{0}' за дату '{1}' загружена", this.GetAccountNumber(personalAccount, pi),
                            pi.PaymentDate.ToString("dd.MM.yyyy")));
                }

                if (pi.PenaltyRefund > 0)
                {
                    importedSum += pi.PenaltyRefund;
                    listImportedPayment.Add(this.CreateImportedPayment(bankDocImport, personalAccount, addInfo, pi.PenaltyRefund));

                    pi.PenaltyRefund = 0;

                    this.AddLog(pi,
                        ImportState.Success,
                        string.Format("Отмена оплаты ЛС '{0}' за дату '{1}' загружена", this.GetAccountNumber(personalAccount, pi),
                            pi.PaymentDate.ToString("dd.MM.yyyy")));
                }

                docProgress.TrySend($"Формирование документов ({Math.Min(current++, paymentInfoCount)}/{paymentInfoCount})");
            }

            bankDocImport.PersonalAccountDeterminationState = this.GetDeterminationState(listImportedPayment);
            bankDocImport.PaymentConfirmationState = PaymentConfirmationState.NotDistributed;

            this.WriteLogs(logImport);

            if (importedSum > 10e14m)
            {
                throw new ValidationException($"Сумма по реестру ({importedSum}) не должна превышать значение {10e14m}");
            }

            this.Container.InTransaction(() =>
            {
                bankDocImport.ImportedSum = importedSum > 0 ? importedSum : (decimal?)null;
                this.BankDocumentImportDomain.Save(bankDocImport);

                var saveProgress = new ProgressSender(listImportedPayment.Count, this.IndicateAction, 95, docProgressPart);
                foreach (var importedPayment in listImportedPayment)
                {
                    this.ImportedPaymentDomain.Save(importedPayment);
                    saveProgress.TrySend("Сохранение платежей");
                }

                saveProgress.ForceSend(96, "Сохранение статусов платежей");
                this.StateHistoryManager.Init(typeof(ImportedPayment));
                listImportedPayment.ForEach(x => this.StateHistoryManager.CreateStateHistory(x, null, ImportedPaymentPaymentConfirmState.NotDistributed));              
                this.StateHistoryManager.SaveStateHistories();
            });

            return bankDocImport;
        }

        private PersonalAccountDeterminationState GetDeterminationState(List<ImportedPayment> payments)
        {
            var result = PersonalAccountDeterminationState.PartiallyDefined;

            if (payments.All(
                x => x.PersonalAccountDeterminationState == ImportedPaymentPersAccDeterminateState.Defined))
            {
                result = PersonalAccountDeterminationState.Defined;
            }
            else if (payments.All(
                x => x.PersonalAccountDeterminationState == ImportedPaymentPersAccDeterminateState.NotDefined))
            {
                result = PersonalAccountDeterminationState.NotDefined;
            }

            return result;
        }

        private void InitDicts(IEnumerable<PersonalAccountPaymentInfoIn> paymentInfo)
        {
            this.IndicateAction(0, "Инициализация кэша");

            var accountNumbers = paymentInfo
                .Select(x => !string.IsNullOrWhiteSpace(x.AccountNumber)
                    ? x.AccountNumber
                    : x.ExternalAccountNumber + "|" + x.PaymentCenterCode)
                .Where(x => !x.IsEmpty())
                // отсекаем ЛС, по которым несколько оплат, так как иначе accounts будет содержать дубликаты, 
                // и дубликаты не пройдут проверку в AccountsIdsByExternalNum на x.Count() == 1, 
                // что как следствие исключит эти ЛС из кэша, и далее они не найдутся
                .Distinct()
                .ToList();

            IDictionary<string, string> paymentCenterByExternalAccount = null;
            var cachPaymentCenterConnectionType = this.Container.GetGkhConfig<RegOperatorConfig>().GeneralConfig.CachPaymentCenterConnectionType;
            var personalAccountsWithRkc = accountNumbers.Where(x => x.Contains('|') && x.Length > 1).ToArray();

            // если нам импортировали только внутренние номера ЛС
            if (personalAccountsWithRkc.Length > 0)
            {
                // в зависимости от настроек тянем РКЦ через связь ЛС-РКЦ или Дом-РКЦ
                if (cachPaymentCenterConnectionType == CachPaymentCenterConnectionType.ByAccount)
                {
                    var paymentCenters = this.PersAccntPaymentCenterDomain.GetAll()
                        .Where(x => x.PersonalAccount.PersAccNumExternalSystems.Trim() != "");

                    paymentCenterByExternalAccount = paymentCenters
                        .Where(x => personalAccountsWithRkc.Contains(x.PersonalAccount.PersAccNumExternalSystems + "|" + x.CashPaymentCenter.Identifier))
                        .Select(x => new
                        {
                            PersAccNumExternalSystems = x.PersonalAccount.PersAccNumExternalSystems,
                            Identifier = x.CashPaymentCenter.Identifier
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.PersAccNumExternalSystems)
                        .ToDictionary(x => x.Key, x => x.Select(y => y.Identifier).First());
                }
            }

            var accounts = new List<AccountInfo>(accountNumbers.Count);

            const int sectionSize = 1000;
            var partsCount = (int)Math.Ceiling(accountNumbers.Count / (float)sectionSize);
            var progress = new ProgressSender(partsCount, this.IndicateAction, 30);
            var proccessedCount = 0;

            progress.ForceSend(0, $"Инициализация кэша (0/{accountNumbers.Count})");

            foreach (var section in accountNumbers.Section(sectionSize))
            {
                var sect = section.ToArray();
                AccountInfo[] part;

                // если грузятся внешние номера + РКЦ
                if (personalAccountsWithRkc.Length > 0)
                {
                    var externalNumbers = sect.Where(x => x.EndsWith("|")).Select(x => x.Split('|')[0].Trim()).ToArray();

                    if (cachPaymentCenterConnectionType == CachPaymentCenterConnectionType.ByAccount)
                    {
                        var selectedPayCenters = this.PersAccntPaymentCenterDomain.GetAll()
                            .Select(x => new
                            {
                                x.PersonalAccount.Id,
                                x.PersonalAccount.PersAccNumExternalSystems,
                                x.CashPaymentCenter.Identifier,
                            })
                            .Where(x => sect.Contains(x.PersAccNumExternalSystems + "|" + x.Identifier))
                            .Select(x => x.Id);

                        part = this.PersonalAccountDomain.GetAll()
                            .Where(x => externalNumbers.Contains(x.PersAccNumExternalSystems.Trim()) || selectedPayCenters.Any(y => y == x.Id))
                            .Select(x => new
                            {
                                x.Id,
                                ExternalNum = x.PersAccNumExternalSystems,
                                AccountNum = x.PersonalAccountNum,
                                AccountOwnerName = x.AccountOwner.Name,
                                RoId = x.Room.RealityObject.Id
                            })
                            .AsEnumerable()
                            .Select(x => new AccountInfo
                            {
                                Id = x.Id,
                                RoId = x.RoId,
                                ExternalNum = x.ExternalNum?.Trim(),
                                AccountNum = x.AccountNum,
                                AccountOwnerName = x.AccountOwnerName,
                                PaymentCenterCode = !string.IsNullOrWhiteSpace(x.ExternalNum)
                                    ? paymentCenterByExternalAccount.Get(x.ExternalNum.Trim())
                                    : null
                            })
                            .ToArray();
                    }
                    else
                    {
                        part = this.SessionProvider.GetCurrentSession()
                            .CreateSQLQuery(@"SELECT
                                  pa.id as ""Id"",
                                  trim(both ' ' from pa.regop_pers_acc_extsyst) as ""ExternalNum"",
                                  trim(both ' ' from pa.acc_num) as ""AccountNum"",
                                  own.name as ""AccountOwnerName"",
                                  ro.id as ""RoId"",
                                  c.identifier as ""PaymentCenterCode""
                                FROM regop_pers_acc pa
                                 join regop_pers_acc_owner own on pa.acc_owner_id = own.id
                                 join gkh_room r on pa.room_id = r.id
                                  join gkh_reality_object ro on r.ro_id = ro.id
                                    join regop_cashpaym_center_real_obj cpo on ro.id = cpo.real_obj_id
                                      join regop_cashpayment_center c on cpo.cashpaym_center_id = c.id
                                WHERE pa.regop_pers_acc_extsyst || '|' || c.identifier in (:arr) 
                                       or pa.regop_pers_acc_extsyst in (:arr_ids)")
                            .SetParameterList("arr", sect)
                            .SetParameterList("arr_ids", externalNumbers)
                            .List<AccountInfo>()
                            .ToArray();
                    }
                }
                else
                {
                    part = this.PersonalAccountDomain.GetAll()
                        .Where(x => sect.Contains(x.PersonalAccountNum))
                        .Select(x => new
                        {
                            x.Id,
                            ExternalNum = x.PersAccNumExternalSystems,
                            AccountNum = x.PersonalAccountNum.Trim(),
                            AccountOwnerName = x.AccountOwner.Name,
                            RoId = x.Room.RealityObject.Id,
                        })
                        .AsEnumerable()
                        .Select(x => new AccountInfo
                        {
                            Id = x.Id,
                            ExternalNum = x.ExternalNum?.Trim(),
                            AccountNum = x.AccountNum.Trim(),
                            AccountOwnerName = x.AccountOwnerName,
                            RoId = x.RoId
                        })
                        .ToArray();
                }

                accounts.AddRange(part);

                proccessedCount += sectionSize;
                progress.Send($"Инициализация кэша ({Math.Min(proccessedCount, accountNumbers.Count)}/{accountNumbers.Count})");
            }

            progress.ForceSend(30, $"Инициализация кэша ({accountNumbers.Count}/{accountNumbers.Count})");

            this.accountById = accounts
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.First());

            this.accountsIdsByExternalNum = accounts
               .Where(x => x.ExternalNum != null)
               .GroupBy(x => x.ExternalNum + "|" + x.PaymentCenterCode)
               .Where(x => x.Count() == 1)
               .ToDictionary(x => x.Key, x => x.Select(y => y.Id).First());

            this.accountsIdsByExternalNumWithoutCenterCode = accounts
               .Where(x => x.ExternalNum != null)
               .GroupBy(x => x.ExternalNum)
               .Where(x => x.Count() == 1)
               .ToDictionary(x => x.Key, x => x.Select(y => y.Id).First());

            this.accountsIdsByAccountNumber = accounts
                .Where(x => x.AccountNum != null)
                .GroupBy(x => x.AccountNum.Trim())
                .ToDictionary(x => x.Key, x => x.First().Id);

            var roIds = accounts.Select(x => x.RoId).Distinct().ToArray();
            foreach (var roIdsPart in roIds.Section(1000))
            {
                var roPaymentAccounts = this.RealityObjectPaymentAccountDomain.GetAll()
                    .WhereContains(x => x.RealityObject.Id, roIdsPart)
                    .Select(x => new
                    {
                        RoId = x.RealityObject.Id,
                        x.AccountNumber
                    });

                foreach (var realityObjectPaymentAccount in roPaymentAccounts)
                {
                    this.accountNumberByRealityObjectMap[realityObjectPaymentAccount.RoId] = realityObjectPaymentAccount.AccountNumber;
                }
            }

            this.logDict.Clear();

            progress.ForceSend(35, "Инициализация кэша завершена");
        }

        private void AddLog(PersonalAccountPaymentInfoIn paymentInfo, ImportState state, string logtext = null)
        {
            if (this.logDict.ContainsKey(paymentInfo))
            {
                var log = this.logDict[paymentInfo];

                if (log.State < state)
                {
                    log.State = state;
                    log.Text = logtext;
                }
                else if (log.State == state)
                {
                    if (string.IsNullOrWhiteSpace(logtext))
                    {
                        log.Text += "; " + logtext;
                    }
                }
                else
                {
                    this.logDict[paymentInfo] = new ImportLog { State = state, Text = log.Text += "; " + logtext };
                }
            }
            else
            {
                this.logDict[paymentInfo] = new ImportLog { State = state, Text = logtext };
            }
        }

        private void WriteLogs(ILogImport logImport)
        {
            foreach (var log in this.logDict)
            {
                var logValue = log.Value;

                if (logValue.State == ImportState.Success)
                {
                    logImport.CountAddedRows++;
                    logImport.Info(logValue.Text, string.Empty);
                }
                else
                {
                    var key = log.Key;

                    var rowIdtext = string.Format(
                        "number = '{0}' surname = '{1}' name = '{2}' patronymic = '{3}' inn = '{4}' kpp = '{5}' date = '{6}'",
                        key.AccountNumber,
                        key.Surname,
                        key.Name,
                        key.Patronymic,
                        key.Inn,
                        key.Kpp,
                        key.PaymentDate);

                    if (logValue.State == ImportState.Warning)
                    {
                        logImport.Warn(rowIdtext, logValue.Text);
                    }
                    else
                    {
                        logImport.Error(rowIdtext, logValue.Text);
                    }
                }
            }
        }

        private AccountInfo GetAccount(PersonalAccountPaymentInfoIn pi)
        {
            if (!pi.AccountNumber.IsEmpty())
            {
                return this.GetAccountByAccountNumber(pi.AccountNumber);
            }

            if (!pi.ExternalAccountNumber.IsEmpty())
            {
                if (pi.PaymentCenterCode.IsNotNull())
                {
                    return this.GetAccountByExternalNumber(pi.ExternalAccountNumber + "|" + pi.PaymentCenterCode);
                }
                else
                {
                    return this.GetAccountByExternalNumberWithoutCenterCode(pi.ExternalAccountNumber);
                }
            }

            return null;
        }

        private ImportedPayment CreateImportedPayment(
            BankDocumentImport bankDocImport,
            AccountInfo accountInfo,
            AdditionalImportPaymentInfo addInfo,
            decimal paymentSum)
        {
            BasePersonalAccount account = null;
            string externalTransaction = null;
            string factReceiverNumber = null;
            PersonalAccountNotDeterminationStateReason? paNotDeterminationStateReason = null;
            var paymentState = ImportedPaymentState.Rno;
            var paDeterminationState = ImportedPaymentPersAccDeterminateState.NotDefined;

            if (accountInfo == null)
            {
                if (addInfo.PaymentInfo.AccountNumber.IsNotEmpty())
                {
                    paNotDeterminationStateReason = PersonalAccountNotDeterminationStateReason.AccountNumber;
                }

                if (addInfo.PaymentInfo.ExternalAccountNumber.IsNotEmpty())
                {
                    paNotDeterminationStateReason = PersonalAccountNotDeterminationStateReason.ExternalAccountNumber;
                }
            }
            else if (accountInfo.RoId > 0)
            {
                account = new BasePersonalAccount
                {
                    Id = accountInfo.Id
                };

                paDeterminationState = ImportedPaymentPersAccDeterminateState.Defined;

                factReceiverNumber = this.GetFactReceiverNumber(accountInfo.RoId);
            }
            else
            {
                paNotDeterminationStateReason = PersonalAccountNotDeterminationStateReason.AccountNumber;
            }
            
            if (addInfo.PaymentInfo.OwnerType == PersonalAccountPaymentInfoIn.AccountType.Suspense)
            {
                paymentState = ImportedPaymentState.Rns;
                externalTransaction = addInfo.PaymentInfo.ExternalSystemTransactionId;
            }

            return new ImportedPayment
            {
                BankDocumentImport = bankDocImport,
                ReceiverNumber = addInfo.PaymentInfo.ReceiverNumber,
                FactReceiverNumber = factReceiverNumber,
                Account = addInfo.PaymentInfo.AccountNumber,
                ExternalAccountNumber = addInfo.PaymentInfo.ExternalAccountNumber,
                PersonalAccount = account,
                OwnerByImport = addInfo.PaymentInfo.Fio,
                AddressByImport = addInfo.PaymentInfo.PayerAddress,
                Sum = paymentSum,
                PaymentDate = addInfo.PaymentInfo.PaymentDate,
                PaymentType = this.GetPaymentType(addInfo),
                PaymentState = paymentState,
                PaymentNumberUs = addInfo.Return(x => x.ReceiptId),
                ExternalTransaction = externalTransaction,
                PersonalAccountDeterminationState = paDeterminationState,
                PersonalAccountNotDeterminationStateReason = paNotDeterminationStateReason,
                PaymentConfirmationState = ImportedPaymentPaymentConfirmState.NotDistributed
            };
        }

        private string GetFactReceiverNumber(long roId)
        {
            return this.accountNumberByRealityObjectMap[roId];
        }

        private string GetAccountNumber(AccountInfo account, PersonalAccountPaymentInfoIn payinfo)
        {
            if (!account.Return(x => x.AccountNum).IsEmpty())
            {
                return account.AccountNum;
            }

            if (!payinfo.Return(x => x.AccountNumber).IsEmpty())
            {
                return payinfo.AccountNumber;
            }

            return payinfo.ExternalAccountNumber;
        }

        private AccountInfo GetAccountByExternalNumber(string externalAccountNumber)
        {
            long id;
            return this.accountsIdsByExternalNum.TryGetValue(externalAccountNumber, out id) ? this.accountById[id] : null;
        }

        private AccountInfo GetAccountByExternalNumberWithoutCenterCode(string externalAccountNumber)
        {
            long id;
            return this.accountsIdsByExternalNumWithoutCenterCode.TryGetValue(externalAccountNumber, out id) ? this.accountById[id] : null;
        }

        private AccountInfo GetAccountByAccountNumber(string accountNumber)
        {
            long id;
            return this.accountsIdsByAccountNumber.TryGetValue(accountNumber, out id) ? this.accountById[id] : null;
        }

        /// <summary>
        /// Определение типа импортируемой оплаты.
        /// </summary>
        /// <param name="addInfo">Дополнительная информация о платеже</param>
        /// <returns>Тим импортируемой оплаты</returns>
        private ImportedPaymentType GetPaymentType(AdditionalImportPaymentInfo addInfo)
        {
            ImportedPaymentType result = ImportedPaymentType.Basic;
            switch (addInfo.ImportType)
            {
                case BankDocumentImportType.SocialSupport:
                    result = ImportedPaymentType.SocialSupport;
                    break;

                case BankDocumentImportType.BankDocument:
                    var importMap = ImportMapHelper.GetMapByKey(addInfo.ProviderCode, this.Container);

                    //универсальный импорт
                    if (importMap is PaPaymentInfoInFsGorodImportMap)
                    {
                        if (addInfo.PaymentInfo.TargetPaid > 0)
                        {
                            result = ImportedPaymentType.ChargePayment;
                        }
                        else if (addInfo.PaymentInfo.PenaltyPaid > 0)
                        {
                            result = ImportedPaymentType.Penalty;
                        }
                        else if (addInfo.PaymentInfo.Refund > 0)
                        {
                            result = ImportedPaymentType.Refund;
                        }
                        else if (addInfo.PaymentInfo.PenaltyRefund > 0)
                        {
                            result = ImportedPaymentType.PenaltyRefund;
                        }
                    }

                    //dbf
                    else if (importMap is PersonalAccountPaymentInfoInDbfImportMap)
                    {
                        /*
                                Если в названии файла (ХXXGMDNN.dbf) ХХХ = значению в поле «Id договора загрузки пени» 
                                (Участники процесса – Роли контрагента – Платежные агенты – Платежный агент) -> Пени, 
                                если = «Id договора загрузки суммы» -> Оплата взноса.
                            */
                        if (addInfo.FileName.IsNotEmpty())
                        {
                            var first3Characters = addInfo.FileName.Length > 2 ? addInfo.FileName.Substring(0, 3) : null;
                            if (first3Characters.IsNotEmpty())
                            {
                                var paymentAgent = this.PaymentAgentDomain.GetAll()
                                    .WhereIf(!first3Characters.IsEmpty(), x => x.PenaltyContractId == first3Characters || x.SumContractId == first3Characters)
                                    .Where(x => x.PenaltyContractId != null)
                                    .Where(x => x.SumContractId != null)
                                    .Select(x => new
                                    {
                                        x.PenaltyContractId,
                                        x.SumContractId
                                    })
                                    .FirstOrDefault();

                                if (paymentAgent != null)
                                {
                                    if (first3Characters == paymentAgent.PenaltyContractId)
                                    {
                                        result = ImportedPaymentType.Penalty;
                                    }
                                    else if (first3Characters == paymentAgent.SumContractId)
                                    {
                                        result = ImportedPaymentType.ChargePayment;
                                    }
                                }
                            }
                        }
                    }

                    //dbf2
                    else if (importMap is PersonalAccountPaymentInfoInDbf2ImportMap)
                    {
                        if (addInfo.PaymentInfo.SumPaid > 0)
                        {
                            result = ImportedPaymentType.ChargePayment;
                        }
                        else if (addInfo.PaymentInfo.SumPenalty > 0)
                        {
                            result = ImportedPaymentType.Penalty;
                        }
                    }

                    //json
                    else if (importMap is PersonalAccountPaymentInfoInDefaultJsonImportMap)
                    {
                        var jsonRecord = (addInfo.PaymentInfo as JsonPaymentInfoSerializer.JsonPaymentInRecord);
                        if (jsonRecord != null)
                        {
                            if (jsonRecord.charged_sum > 0)
                            {
                                result = ImportedPaymentType.ChargePayment;
                            }
                            else if (jsonRecord.charged_penalty > 0)
                            {
                                result = ImportedPaymentType.Penalty;
                            }
                        }
                    }

                    //xml
                    else if (importMap is PersonalAccountPaymentInfoInDefaultXmlImportMap)
                    {
                        if (addInfo.PaymentInfo.SumPaid > 0)
                        {
                            result = ImportedPaymentType.ChargePayment;
                        }
                        else if (addInfo.PaymentInfo.SumPenalty > 0)
                        {
                            result = ImportedPaymentType.Penalty;
                        }
                    }
                    break;
            }

            return result;
        }

        private class AccountInfo
        {
            public long Id { get; set; }
            public string ExternalNum { get; set; }
            public string AccountNum { get; set; }
            public string AccountOwnerName { get; set; }
            public string PaymentCenterCode { get; set; }
            public long RoId { get; set; }
        }

        private class AdditionalImportPaymentInfo
        {
            public string ReceiptId;
            public DateTime DocDate;
            public string DocNum;
            public string ProviderCode;
            public string FileName;
            public BankDocumentImportType ImportType;
            public PersonalAccountPaymentInfoIn PaymentInfo { get; set; }
        }

        private class ImportLog
        {
            public string Text;
            public ImportState State;
        }

        private enum ImportState
        {
            Success = 1,
            Warning = 2
        }
    }
}