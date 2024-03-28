namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Import;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Wcf.Contracts.PersonalAccount;
    using Castle.Windsor;
    using Gkh.Domain.CollectionExtensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class UnacceptedPaymentProvider : IUnacceptedPaymentProvider
    {
        public IWindsorContainer Container { get; set; }
        public IRepository<BasePersonalAccount> PersonalAccountDomain { get; set; }
        public IDomainService<IndividualAccountOwner> IndividualAccountOwnerDomain { get; set; }
        public IDomainService<LegalAccountOwner> LegalAccountOwnerDomain { get; set; }
        public IDomainService<ImportedPayment> ImportedPaymentDomain { get; set; }
        public IDomainService<PaymentAgent> PaymentAgentDomain { get; set; }
        public IDomainService<CashPaymentCenter> PaymentCenterDomain { get; set; }
        public IDomainService<CashPaymentCenterPersAcc> PersAccntPaymentCenterDomain { get; set; }
        public IDomainService<TransitAccount> TransitAccountDomain { get; set; }
        public IDomainService<UnacceptedPaymentPacket> UnacceptedPaymentPacketDomain { get; set; }

        private Dictionary<string, EntityProxy[]> _dictPaymentAgents;

        private readonly Dictionary<PersonalAccountPaymentInfoIn, ImportLog> _logDict =
            new Dictionary<PersonalAccountPaymentInfoIn, ImportLog>();

        protected Dictionary<string, long> AccountsIdsByAccountNumber;

        protected Dictionary<string, long> AccountsIdsByExternalNum;

        protected Dictionary<long, AccountInfo> AccountById;

        protected sealed class AccountInfo
        {
            public long Id { get; set; }

            public string ExternalNum { get; set; }

            public string AccountNum { get; set; }

            public string AccountOwnerName { get; set; }

            public string PaymentCenterCode { get; set; }
        }

        protected void InitDicts(IEnumerable<PersonalAccountPaymentInfoIn> paymentInfo)
        {
            var accountNumbers = paymentInfo
                .Select(x => !string.IsNullOrWhiteSpace(x.AccountNumber) ? x.AccountNumber : x.ExternalAccountNumber + "|" + x.PaymentCenterCode)
                .Where(x => !x.IsEmpty())
                .Distinct() //отсекаем ЛС, по которым несколько оплат, так как иначе accounts будет содержать дубликаты, 
                //и дубликаты не пройдут проверку в AccountsIdsByExternalNum на x.Count() == 1, 
                //что как следствие исключит эти ЛС из кэша, и далее они не найдутся, и загрузятся как НВС, что неправильно
                .ToList();

            var query = PersonalAccountDomain.GetAll().Where(x => !x.State.FinalState).OrderBy(x => x.Id);
            var paymentCenters =
                PersAccntPaymentCenterDomain.GetAll()
                    .Where(
                        x =>
                            x.PersonalAccount != null &&
                            x.PersonalAccount.PersAccNumExternalSystems.Trim() != "");

            var accounts = new List<AccountInfo>(accountNumbers.Count);

            foreach (var section in accountNumbers.Section(1000))
            {
                var sect = section.ToArray();

                var selectedPayCenters = paymentCenters
                    .Where(pc => query.Contains(pc.PersonalAccount))
                    .Select(x => new
                    {
                        x.CashPaymentCenter.Identifier,
                        x.PersonalAccount.PersonalAccountNum,
                        x.PersonalAccount.PersAccNumExternalSystems
                    })
                    .ToList()
                    .Select(a =>
                        new
                        {
                            a.PersAccNumExternalSystems,
                            a.PersonalAccountNum,
                            a.Identifier,
                            IsInSection = sect.Contains(a.PersAccNumExternalSystems + "|" + a.Identifier)
                        }
                    )
                    .Where(a => a.IsInSection)
                    .Select(a => new { a.PersAccNumExternalSystems, a.Identifier, a.PersonalAccountNum });

                var selectedPayCentersList = selectedPayCenters.Select(x => x.PersAccNumExternalSystems).ToList();

                var part = query
                    .Where(x =>
                        sect.Contains(x.PersonalAccountNum) ||
                        sect.Contains(x.PersAccNumExternalSystems + "|") ||
                        selectedPayCentersList.Contains(x.PersAccNumExternalSystems)
                    )
                    .ToList()
                    .Select(x => new AccountInfo
                    {
                        Id = x.Id,
                        ExternalNum = x.PersAccNumExternalSystems,
                        AccountNum = x.PersonalAccountNum,
                        AccountOwnerName = x.AccountOwner.Name,
                        PaymentCenterCode = selectedPayCenters.Where(y => y.PersAccNumExternalSystems == x.PersAccNumExternalSystems && y.PersonalAccountNum == x.PersonalAccountNum).Select(y => y.Identifier).FirstOrDefault()
                    })
                    .ToArray();

                accounts.AddRange(part);
            }

            AccountById = accounts
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.First());

            AccountsIdsByExternalNum = accounts
               .Where(x => x.ExternalNum != null)
               .GroupBy(x => x.ExternalNum + "|" + x.PaymentCenterCode)
               .Where(x => x.Count() == 1)
               .ToDictionary(x => x.Key, x => x.Select(y => y.Id).First());

            AccountsIdsByAccountNumber = accounts
                .Where(x => x.AccountNum != null)
                .GroupBy(x => x.AccountNum.Trim())
                .ToDictionary(x => x.Key, x => x.First().Id);

            _dictPaymentAgents = PaymentAgentDomain.GetAll()
                .Where(x => x.Code != null)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Contragent.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.Code)
                .ToDictionary(x => x.Key, y => y.Select(x => new EntityProxy { Id = x.Id, Name = x.Name }).ToArray());

            _logDict.Clear();
        }

        public BankDocumentImport CreateUnacceptedPayments(
            ILogImport logImport,
            IEnumerable<PersonalAccountPaymentInfoIn> paymentInfo, string source,
            TransitAccountProxy transitAccountProxy = null)
        {
            InitDicts(paymentInfo);

            var importedSum = 0m;

            var bankDocImport = new BankDocumentImport
            {
                ImportDate = DateTime.Now,
                DocumentType = string.IsNullOrEmpty(source) ? "Документ оплаты" : source,
                Status = logImport.CountError > 0
                    ? BankDocumentImportStatus.Failed
                    : (logImport.CountWarning > 0
                        ? BankDocumentImportStatus.ImportedWithWarnings
                        : BankDocumentImportStatus.SuccessfullyImported)
            };

            if (transitAccountProxy != null)
            {
                bankDocImport.DocumentDate = transitAccountProxy.RegDate != DateTime.MinValue
                    ? transitAccountProxy.RegDate
                    : (DateTime?)null;
                bankDocImport.DocumentNumber = transitAccountProxy.RegNum;
            }

            Container.InTransaction(() =>
            {
                var listSuspAccounts = new List<Tuple<SuspenseAccount, ImportedPayment>>();
                var listUnacceptedPayment = new List<Tuple<UnacceptedPayment, ImportedPayment>>();

                var unaccPaymentDomain = Container.ResolveDomain<UnacceptedPayment>();
                var suspAccDomain = Container.ResolveDomain<SuspenseAccount>();

                var packet = new UnacceptedPaymentPacket
                {
                    CreateDate = DateTime.Now,
                    Sum = 0m,
                    Description = string.Empty
                };

                var listTransitAccounts = new List<TransitAccount>();

                if (transitAccountProxy != null)
                {
                    var transitAccount = CreateTransitAccount(transitAccountProxy, bankDocImport);

                    if (transitAccount != null)
                    {
                        listTransitAccounts.Add(transitAccount);
                    }
                }

                foreach (var pi in paymentInfo)
                {
                    if (pi.AccountNumber.IsEmpty()
                        && pi.ExternalAccountNumber.IsEmpty()
                        && pi.OwnerType != PersonalAccountPaymentInfoIn.AccountType.Legal)
                    {
                        AddLog(pi, ImportState.Warning, "Не указан номер счета. Запись не загружена");
                        continue;
                    }

                    var addInfo = new AdditionalImportPaymentInfo
                    {
                        ReceiptId = pi.ReceiptId,
                        DocDate = pi.DocumentDate,
                        DocNum = pi.DocumentNumber,
                        PaymentInfo = pi,
                        PaymentType =
                            pi.SumPaid > 0
                                ? ImportedPaymentType.Sum
                                : pi.PenaltyPaid > 0 ? ImportedPaymentType.Penalty : 0
                    };

                    var personalAccount = GetAccount(pi);

                    if (pi.OwnerType == PersonalAccountPaymentInfoIn.AccountType.Suspense)
                    {
                        if (pi.TargetPaid > 0)
                        {
                            // создается невыясненная сумма "расход"
                            var newPayment = new SuspenseAccount
                            {
                                AccountBeneficiary = pi.AccountNumber,
                                DetailsOfPayment = pi.Details,
                                DateReceipt = pi.PaymentDate,
                                SuspenseAccountTypePayment = SuspenseAccountTypePayment.Payment,
                                Sum = pi.TargetPaid,
                                DistributeState = DistributionState.NotDistributed,
                                Reason = pi.Reason,
                                MoneyDirection = MoneyDirection.Outcome
                            };

                            importedSum += pi.TargetPaid;

                            listSuspAccounts.Add(new Tuple<SuspenseAccount, ImportedPayment>(newPayment,
                                CreateImportedPayment(newPayment, bankDocImport, addInfo, pi)));

                            AddLog(pi, ImportState.Success,
                                string.Format(
                                    " Оплата ЛС '{0}' за дату '{1}' загружена в счет невыясненных сумм",
                                    GetAccountNumber(personalAccount, pi),
                                    pi.PaymentDate.ToString("dd.MM.yyyy")));
                        }

                        if (pi.SumPaid > 0)
                        {
                            // создается невыясненная сумма "расход"
                            var newPayment = new SuspenseAccount
                            {
                                AccountBeneficiary = pi.AccountNumber,
                                DetailsOfPayment = pi.Details,
                                DateReceipt = pi.PaymentDate,
                                SuspenseAccountTypePayment = SuspenseAccountTypePayment.Payment,
                                Sum = pi.SumPaid,
                                DistributeState = DistributionState.NotDistributed,
                                Reason = pi.Reason,
                                MoneyDirection = MoneyDirection.Outcome
                            };

                            importedSum += pi.TargetPaid;

                            listSuspAccounts.Add(new Tuple<SuspenseAccount, ImportedPayment>(newPayment,
                                CreateImportedPayment(newPayment, bankDocImport, addInfo, pi)));

                            AddLog(pi, ImportState.Success,
                                string.Format(
                                    " Оплата ЛС '{0}' за дату '{1}' загружена в счет невыясненных сумм",
                                    GetAccountNumber(personalAccount, pi), pi.PaymentDate.ToString("dd.MM.yyyy")));
                        }

                        if (pi.PenaltyPaid > 0)
                        {
                            // создается невыясненная сумма "расход"
                            var newPayment = new SuspenseAccount
                            {
                                AccountBeneficiary = pi.AccountNumber,
                                DetailsOfPayment = pi.Details,
                                DateReceipt = pi.PaymentDate,
                                SuspenseAccountTypePayment = SuspenseAccountTypePayment.Payment,
                                Sum = pi.PenaltyPaid,
                                DistributeState = DistributionState.NotDistributed,
                                Reason = pi.Reason,
                                MoneyDirection = MoneyDirection.Outcome
                            };

                            importedSum += pi.PenaltyPaid;

                            listSuspAccounts.Add(new Tuple<SuspenseAccount, ImportedPayment>(newPayment,
                                CreateImportedPayment(newPayment, bankDocImport, addInfo, pi)));

                            AddLog(pi, ImportState.Success,
                                string.Format(
                                    " Оплата ЛС '{0}' за дату '{1}' загружена в счет невыясненных сумм",
                                    GetAccountNumber(personalAccount, pi),
                                    pi.PaymentDate.ToString("dd.MM.yyyy")));
                        }
                    }
                    else
                    {
                        if (personalAccount != null)
                        {
                            // создается неподтвержденная оплата
                            if (pi.TargetPaid > 0)
                            {
                                var account = new BasePersonalAccount
                                {
                                    Id = personalAccount.Id
                                };
                                var newPayment = packet.CreatePayment(
                                    pi,
                                    account,
                                    UnacceptedPaymentPacket.UnacceptedPaymentTypePaid.TargetPaid);

                                importedSum += pi.TargetPaid;
                                packet.Sum += pi.TargetPaid;

                                listUnacceptedPayment.Add(
                                    new Tuple<UnacceptedPayment, ImportedPayment>(
                                        newPayment, CreateImportedPayment(newPayment, bankDocImport, personalAccount, addInfo)));

                                var logText = string.Format(
                                    "Оплата ЛС '{0}' за дату '{1}' загружена",
                                    GetAccountNumber(personalAccount, pi),
                                    pi.PaymentDate.ToString("dd.MM.yyyy"));
                                AddLog(pi, ImportState.Success, logText);
                            }

                            // если есть сумма оплаты суммы (лол), то добавляем запись оплаты с типом "Сумма"
                            if (pi.SumPaid > 0)
                            {
                                var account = new BasePersonalAccount
                                {
                                    Id = personalAccount.Id
                                };
                                var newPaymentPenalty = packet.CreatePayment(pi, account,
                                    UnacceptedPaymentPacket.UnacceptedPaymentTypePaid.SumPaid);

                                importedSum += pi.SumPaid;
                                packet.Sum += pi.SumPaid;

                                listUnacceptedPayment.Add(
                                    new Tuple<UnacceptedPayment, ImportedPayment>(
                                        newPaymentPenalty,
                                        CreateImportedPayment(newPaymentPenalty, bankDocImport, personalAccount, addInfo)));

                                AddLog(pi, ImportState.Success,
                                    string.Format("Оплата ЛС '{0}' за дату '{1}' загружена",
                                        GetAccountNumber(personalAccount, pi),
                                        pi.PaymentDate.ToString("dd.MM.yyyy")));
                            }

                            // если есть сумма оплаты пени, то добавляем такую же запись оплат с типом "Пени"
                            if (pi.PenaltyPaid > 0)
                            {
                                var account = new BasePersonalAccount
                                {
                                    Id = personalAccount.Id
                                };
                                var newPaymentPenalty = packet.CreatePayment(pi, account,
                                    UnacceptedPaymentPacket.UnacceptedPaymentTypePaid.PenaltyPaid);

                                importedSum += pi.PenaltyPaid;
                                packet.Sum += pi.PenaltyPaid;

                                listUnacceptedPayment.Add(
                                    new Tuple<UnacceptedPayment, ImportedPayment>(
                                        newPaymentPenalty,
                                        CreateImportedPayment(newPaymentPenalty, bankDocImport, personalAccount, addInfo)));

                                AddLog(pi, ImportState.Success,
                                    string.Format("Оплата ЛС '{0}' за дату '{1}' загружена",
                                        GetAccountNumber(personalAccount, pi), pi.PaymentDate.ToString("dd.MM.yyyy")));
                            }

                            // если есть сумма соцподдержки, то добавляем такую же запись оплат с типом "Соц поддержка"
                            if (pi.SocialSupport > 0)
                            {
                                if (!string.Equals(personalAccount.AccountOwnerName, pi.Fio, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    var logMessage = string.Format(
                                        "Номер ЛС {0} не соответствует абоненту {1}",
                                        GetAccountNumber(personalAccount, pi),
                                        pi.Fio);
                                    AddLog(pi, ImportState.Warning, logMessage);
                                }
                                else
                                {
                                    var account = new BasePersonalAccount
                                    {
                                        Id = personalAccount.Id
                                    };
                                    var newPayment = packet.CreatePayment(pi, account,
                                    UnacceptedPaymentPacket.UnacceptedPaymentTypePaid.SocialSupport);

                                    importedSum += pi.SocialSupport;
                                    packet.Sum += pi.SocialSupport;

                                    listUnacceptedPayment.Add(
                                        new Tuple<UnacceptedPayment, ImportedPayment>(
                                            newPayment,
                                            CreateImportedPayment(newPayment, bankDocImport, personalAccount, addInfo)));

                                    AddLog(pi, ImportState.Success,
                                        string.Format("Соц. поддержка по ЛС '{0}' на сумму '{1}' загружена в счет неподтвержденных оплат",
                                            GetAccountNumber(personalAccount, pi), pi.SocialSupport));
                                }
                            }
                        }
                        else
                        {
                            string logMessage;

                            switch (pi.OwnerType)
                            {
                                case PersonalAccountPaymentInfoIn.AccountType.Legal:
                                    logMessage = string.Format(
                                        "Абонент с ИНН '{0}' и КПП '{1}' (адрес {4}) не найден. "
                                        + "Оплата с ЛС '{2}' за дату '{3}' загружена в счет невыясненных сумм",
                                        pi.Inn,
                                        pi.Kpp,
                                        GetAccountNumber(personalAccount, pi),
                                        pi.PaymentDate.ToString("dd.MM.yyyy"),
                                        pi.PayerAddress);
                                    break;
                                case PersonalAccountPaymentInfoIn.AccountType.Personal:
                                default:
                                    logMessage = string.Format(
                                        "Лицевой счет '{0}' или абонент '{1}' (адрес {3}) не найдены. "
                                        + "Оплата с ЛС '{0}' за дату '{2}' загружена в счет невыясненных сумм",
                                        GetAccountNumber(personalAccount, pi),
                                        string.Format("{0} {1} {2}", pi.Surname, pi.Name, pi.Patronymic).Trim(),
                                        pi.PaymentDate.ToString("dd.MM.yyyy"),
                                        pi.PayerAddress);
                                    break;
                            }

                            AddLog(pi, ImportState.Warning, logMessage);

                            var reason = string.IsNullOrWhiteSpace(pi.Reason) ? logMessage : pi.Reason;

                            try
                            {
                                var paymentCreated = false;

                                if (pi.TargetPaid > 0)
                                {
                                    // создается невыясненная сумма
                                    var newPayment = new SuspenseAccount
                                    {
                                        AccountBeneficiary = pi.AccountNumber ?? pi.ExternalAccountNumber,
                                        DetailsOfPayment = pi.Details,
                                        DateReceipt = pi.PaymentDate,
                                        SuspenseAccountTypePayment = SuspenseAccountTypePayment.Payment,
                                        Sum = pi.TargetPaid,
                                        DistributeState = DistributionState.NotDistributed,
                                        Reason = reason,
                                        MoneyDirection = MoneyDirection.Income
                                    };

                                    importedSum += pi.TargetPaid;

                                    listSuspAccounts.Add(new Tuple<SuspenseAccount, ImportedPayment>(newPayment,
                                        CreateImportedPayment(newPayment, bankDocImport, addInfo, pi)));

                                    paymentCreated = true;
                                }

                                if (pi.SumPaid > 0)
                                {
                                    // создается невыясненная сумма
                                    var newPayment = new SuspenseAccount
                                    {
                                        AccountBeneficiary = pi.AccountNumber ?? pi.ExternalAccountNumber,
                                        DetailsOfPayment = pi.Details,
                                        DateReceipt = pi.PaymentDate,
                                        SuspenseAccountTypePayment = SuspenseAccountTypePayment.ChargePayment,
                                        Sum = pi.SumPaid,
                                        DistributeState = DistributionState.NotDistributed,
                                        Reason = reason,
                                        MoneyDirection = MoneyDirection.Income
                                    };

                                    importedSum += pi.SumPaid;

                                    listSuspAccounts.Add(new Tuple<SuspenseAccount, ImportedPayment>(newPayment,
                                        CreateImportedPayment(newPayment, bankDocImport, addInfo, pi)));

                                    paymentCreated = true;
                                }

                                if (pi.PenaltyPaid > 0)
                                {
                                    // создается невыясненная сумма
                                    var newPayment = new SuspenseAccount
                                    {
                                        AccountBeneficiary = pi.AccountNumber ?? pi.ExternalAccountNumber,
                                        DetailsOfPayment = pi.Details,
                                        DateReceipt = pi.PaymentDate,
                                        SuspenseAccountTypePayment = SuspenseAccountTypePayment.ChargePayment,
                                        Sum = pi.PenaltyPaid,
                                        DistributeState = DistributionState.NotDistributed,
                                        Reason = reason,
                                        MoneyDirection = MoneyDirection.Income
                                    };

                                    importedSum += pi.PenaltyPaid;

                                    listSuspAccounts.Add(new Tuple<SuspenseAccount, ImportedPayment>(newPayment,
                                        CreateImportedPayment(newPayment, bankDocImport, addInfo, pi)));

                                    paymentCreated = true;
                                }

                                if (paymentCreated)
                                {
                                    AddLog(
                                        pi,
                                        ImportState.Success,
                                        string.Format(
                                            " Оплата ЛС '{0}' за дату '{1}' загружена в счет невыясненных сумм",
                                            GetAccountNumber(personalAccount, pi),
                                            pi.PaymentDate.ToString("dd.MM.yyyy")));
                                }
                            }
                            catch (Exception e)
                            {
                                AddLog(pi, ImportState.Warning, e.Message);
                            }
                        }
                    }
                }

                var bankDocDomain = Container.ResolveDomain<BankDocumentImport>();
                using (Container.Using(bankDocDomain))
                {
                    bankDocDomain.Save(bankDocImport);
                }

                if (listUnacceptedPayment.Any())
                {
                    packet.Cancel();
                    if (listUnacceptedPayment.Any(x => x.Item1.PaymentType == PaymentType.SocialSupport))
                    {
                        packet.Type = UnacceptedPaymentPacketType.SocialSupport;
                    }

                    packet.BankDocumentId = bankDocImport.Id;
                    UnacceptedPaymentPacketDomain.Save(packet);
                }

                foreach (var unacceptedPayment in listUnacceptedPayment)
                {
                    unaccPaymentDomain.Save(unacceptedPayment.Item1);
                    unacceptedPayment.Item2.PaymentId = unacceptedPayment.Item1.Id;
                    ImportedPaymentDomain.Save(unacceptedPayment.Item2);
                }

                foreach (var suspAcc in listSuspAccounts)
                {
                    suspAccDomain.Save(suspAcc.Item1);
                    suspAcc.Item2.PaymentId = suspAcc.Item1.Id;
                    ImportedPaymentDomain.Save(suspAcc.Item2);
                }

                listTransitAccounts.ForEach(x => TransitAccountDomain.Save(x));
            });

            WriteLogs(logImport);

            bankDocImport.ImportedSum =
                bankDocImport.Status == BankDocumentImportStatus.Failed
                    ? (decimal?)null
                    : importedSum;

            Container.Resolve<IDomainService<BankDocumentImport>>().Update(bankDocImport);

            return bankDocImport;
        }

        protected void AddLog(PersonalAccountPaymentInfoIn paymentInfo, ImportState state, string logtext = null)
        {
            if (_logDict.ContainsKey(paymentInfo))
            {
                var log = _logDict[paymentInfo];

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
            }
            else
            {
                _logDict[paymentInfo] = new ImportLog { State = state, Text = logtext };
            }
        }

        protected void WriteLogs(ILogImport logImport)
        {
            foreach (var log in _logDict)
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

        protected AccountInfo GetAccount(PersonalAccountPaymentInfoIn pi)
        {
            if (!pi.AccountNumber.IsEmpty())
            {
                return GetAccountByAccountNumber(pi.AccountNumber);
            }

            if (!pi.ExternalAccountNumber.IsEmpty())
            {
                return GetAccountByExternalNumber(pi.ExternalAccountNumber + "|" + pi.PaymentCenterCode);
            }

            return null;
        }

        private AccountInfo GetAccountByExternalNumber(string externalAccountNumber)
        {
            long id;
            return AccountsIdsByExternalNum.TryGetValue(externalAccountNumber, out id) ? AccountById[id] : null;
        }

        private AccountInfo GetAccountByAccountNumber(string accountNumber)
        {
            long id;
            return AccountsIdsByAccountNumber.TryGetValue(accountNumber, out id) ? AccountById[id] : null;
        }

        protected ImportedPayment CreateImportedPayment(UnacceptedPayment unacceptedPayment,
            BankDocumentImport bankDocImport, AccountInfo accountInfo, AdditionalImportPaymentInfo addInfo)
        {
            if (!bankDocImport.DocumentDate.HasValue && addInfo.DocDate != DateTime.MinValue)
            {
                bankDocImport.DocumentDate = addInfo.DocDate;
            }

            if (bankDocImport.DocumentNumber == null)
            {
                bankDocImport.DocumentNumber = addInfo.DocNum;
            }

            return new ImportedPayment
            {
                BankDocumentImport = bankDocImport,
                ReceiverNumber = addInfo.PaymentInfo.ReceiverNumber,
                Account = unacceptedPayment.PersonalAccount.PersonalAccountNum ?? accountInfo.AccountNum,
                Sum = unacceptedPayment.Sum,
                PaymentDate = unacceptedPayment.PaymentDate,
                PaymentType = getImportedPaymentType(unacceptedPayment, addInfo),
                PaymentState = ImportedPaymentState.Rno,
                PaymentNumberUs = addInfo.Return(x => x.ReceiptId)
            };
        }

        /// <summary>
        /// Определение типа импортируемой оплаты.
        /// </summary>
        private ImportedPaymentType getImportedPaymentType(UnacceptedPayment unacceptedPayment, AdditionalImportPaymentInfo addInfo)
        {
            #region README

            /*
             * Изолировал в данном методе определение типа импортируемой оплаты.
             * Аналитики выясняют соответсвие между типами Неподтвержденных оплат (UnacceptedPayment)
             * и Оплатами платежных агентов (ImportedPayment)
             * TODO: Сделать нормальное определение типа и привести в соответсвие с новыми требованиями.
             */

            #endregion

            if (unacceptedPayment.PaymentType == PaymentType.SocialSupport)
            {
                return ImportedPaymentType.SocialSupport;
            }

            var paymentType = unacceptedPayment.PaymentType == PaymentType.Basic
               ? ImportedPaymentType.Sum
               : ImportedPaymentType.Penalty;
            return addInfo.PaymentType == ImportedPaymentType.Sum
                ? ImportedPaymentType.Sum
                : paymentType;
        }

        protected ImportedPayment CreateImportedPayment(SuspenseAccount suspenseAccount,
            BankDocumentImport bankDocImport,
            AdditionalImportPaymentInfo addInfo, PersonalAccountPaymentInfoIn originPayment)
        {
            if (!bankDocImport.DocumentDate.HasValue && addInfo.DocDate != DateTime.MinValue)
            {
                bankDocImport.DocumentDate = addInfo.DocDate;
            }

            if (bankDocImport.DocumentNumber == null)
            {
                bankDocImport.DocumentNumber = addInfo.DocNum;
            }

            return new ImportedPayment
            {
                BankDocumentImport = bankDocImport,
                Account = suspenseAccount.AccountBeneficiary,
                ReceiverNumber = addInfo.PaymentInfo.ReceiverNumber,
                Sum = suspenseAccount.Sum,
                PaymentDate = suspenseAccount.DateReceipt,
                PaymentType = addInfo.PaymentType == ImportedPaymentType.Sum
                    ? ImportedPaymentType.Sum
                    : addInfo.PaymentType == ImportedPaymentType.Penalty
                        ? ImportedPaymentType.Penalty
                        : suspenseAccount.SuspenseAccountTypePayment == SuspenseAccountTypePayment.Payment
                            ? ImportedPaymentType.Payment
                            : ImportedPaymentType.ChargePayment,
                PaymentState = ImportedPaymentState.Rns,
                PaymentNumberUs = addInfo.Return(x => x.ReceiptId),
                ExternalTransaction = originPayment.ExternalSystemTransactionId
            };
        }

        protected string GetAccountNumber(AccountInfo account, PersonalAccountPaymentInfoIn payinfo)
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

        protected TransitAccount CreateTransitAccount(TransitAccountProxy transitAccountProxy,
            BankDocumentImport bankDocImport)
        {
            if (string.IsNullOrWhiteSpace(transitAccountProxy.AgentId))
            {
                return null;
            }

            if (!_dictPaymentAgents.ContainsKey(transitAccountProxy.AgentId))
            {
                return null;
            }

            if (_dictPaymentAgents[transitAccountProxy.AgentId].Length > 1)
            {
                return null;
            }

            var payagent = _dictPaymentAgents[transitAccountProxy.AgentId].First();

            bankDocImport.PaymentAgentCode = transitAccountProxy.AgentId;
            bankDocImport.PaymentAgentName = payagent.Name;

            return new TransitAccount
            {
                Date = transitAccountProxy.RegDate,
                Number = transitAccountProxy.RegNum,
                Sum = transitAccountProxy.RegSum,
                PaymentAgent = PaymentAgentDomain.Load(payagent.Id)
            };
        }

        protected class AdditionalImportPaymentInfo
        {
            public string ReceiptId;

            public DateTime DocDate;

            public string DocNum;

            public ImportedPaymentType PaymentType;

            public PersonalAccountPaymentInfoIn PaymentInfo { get; set; }
        }

        protected enum ImportState
        {
            Success = 1,
            Warning = 2,
            Error = 3
        }

        private class EntityProxy
        {
            public long Id { get; set; }
            public string Name { get; set; }
        }

        protected class ImportLog
        {
            public string Text;

            public ImportState State;
        }
    }

    public class TransitAccountProxy
    {
        public string AgentId;

        public DateTime RegDate;

        public string RegNum;

        public decimal RegSum;
    }
}
