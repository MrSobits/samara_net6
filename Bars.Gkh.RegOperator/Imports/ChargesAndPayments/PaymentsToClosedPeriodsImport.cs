namespace Bars.Gkh.RegOperator.Imports
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using B4;
    using System.Text;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.Gkh.RegOperator.Domain.Repository;

    using Gkh.Entities;

    using Entities;
    using Entities.ValueObjects;
    using Enums;
    using Gkh.Domain;
    using Gkh.Enums.Import;
    using Gkh.Utils;
    using Import;
    using Import.Impl;
    using Utils;

    using Entities.PersonalAccount.Operations;

    using Bars.B4.Modules.Tasks.Common.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Entities.Import;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    /// <summary>
    /// Импорт оплат в закрытый период
    /// </summary>
    public partial class PaymentsToClosedPeriodsImport : GkhImportBase
    {
        /// <summary>
        /// Предупреждение про ЛС при импорте в закрытый период
        /// </summary>
        public IDomainService<AccountWarningInPaymentsToClosedPeriodsImport> AccountWarningInPaymentsToClosedPeriodsImportDomain { get; set; }

        /// <summary> 
        /// Лицевой счёт 
        /// </summary>
        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }

        /// <summary>
        /// Предупреждение про даты при импорте в закрытый период
        /// </summary>
        public IDomainService<DateWarningInPaymentsToClosedPeriodsImport> DateWarningInPaymentsToClosedPeriodsImportDomain { get; set; }

        /// <summary>
        /// Шапака импорта оплаты в закрытый период
        /// </summary>
        public IDomainService<HeaderOfPaymentsToClosedPeriodsImport> HeaderOfPaymentsToClosedPeriodsImportDomain { get; set; }

        /// <summary>
        /// Ключ импорта
        /// </summary>
        public override string Key => this.GetType().FullName;

        /// <summary>
        /// Код группировки импорта (например группировка в меню)
        /// </summary>
        public override string CodeImport => string.Empty;

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public override string Name => "Импорт оплат (в закрытые периоды)";

        /// <summary>
        /// Разрешенные расширения файлов
        /// </summary>
        public override string PossibleFileExtensions => "txt";

        /// <summary>
        /// Права
        /// </summary>
        public override string PermissionName => "Import.PaymentsToClosedPeriods.View";

        /// <summary>
        /// Зависимости от других импортов
        /// </summary>
        public override string[] Dependencies => new[] { "Bars.Gkh.RegOperator.Imports.ChargesToClosedPeriodsImport" };

        /// <summary>
        /// Словарь объектов для создания
        /// </summary>
        private Dictionary<long, PersonalAccountPeriodSummary> DictPersonalAccountPeriodSummaryToCreate { get; set; }

        private Dictionary<long, RealityObjectChargeAccountOperation> DictRoAccountOperationToCreate { get; set; }

        private List<WarningInClosedPeriodsImport> warnings = new List<WarningInClosedPeriodsImport>();

        /// <summary>
        /// Загрузить импорт
        /// </summary>
        /// <param name="params">БазПарамс</param>
        /// <returns></returns>
        protected override ImportResult ImportUsingGkhApi(BaseParams @params)
        {
            var periodId = @params.Params.GetAsId("periodId");
            bool updateSaldo = @params.Params.GetAs<bool>("updateSaldoIn");
            string externalRkcId = @params.Params.GetAs<string>("externalRkcId");

            this.period = this.Container.ResolveDomain<ChargePeriod>().Get(periodId);

            if (this.period == null)
            {
                throw new ArgumentException("Не удалось получить период");
            }

            var file = @params.Files.First().Value;
            Payment[] payments;

            this.Indicate(1, "Чтение данных");

            try
            {
                using (var ms = new MemoryStream(file.Data))
                {
                    payments = new DataTranslator().ExtractData(ms, externalRkcId);
                }
            }
            catch (ValidationException e)
            {
                this.LogError("Чтение файла", e.Message);
                return new ImportResult(StatusImport.CompletedWithError);
            }

            this.Indicate(5, "Инициализация кеша");

            this.InitCache(payments);

            this.Indicate(10, "Обработка данных");

            const int Take = 100;
            var processed = 0;

            this.DictPersonalAccountPeriodSummaryToCreate = new Dictionary<long, PersonalAccountPeriodSummary>();

            this.DictRoAccountOperationToCreate = new Dictionary<long, RealityObjectChargeAccountOperation>();

            foreach (var splited in payments.SplitArray(Take))
            {
                var packet = new UnacceptedPaymentPacket { CreateDate = DateTime.Now, Type = UnacceptedPaymentPacketType.Payment };

                packet.Accept();

                var moneyOperation = packet.CreateOperation(this.period);

                var importSource = new ClosedPeriodPaymentSource(this.period) { OriginatorGuid = moneyOperation.OriginatorGuid, };

                var entitiesForSave = new List<BaseEntity> { packet, moneyOperation, importSource };

                foreach (var payment in splited)
                {
                    var acc = this.GetAccount(payment);

                    if (acc == null)
                    {
                        continue;
                    }

                    if (!this.CheckPaymentInPeriod(payment))
                    {
                        this.LogDateWarning(FormatTitle(payment),
                            $"Дата оплаты {payment.PaymentDate.ToShortDateString()} выходит за рамки периода",
                            payment.PaymentDate);

                        continue;
                    }

                    var periodPaymentsResult = this.CheckPeriodExistingPayments(acc, payment);
                    if (periodPaymentsResult.HasBaseTariffPayment && periodPaymentsResult.HasPenaltyPayment
                        && periodPaymentsResult.HasDecisionTariffPayment)
                    {
                        this.LogDateWarning(FormatTitle(payment),
                            $"Оплата на сумму {payment.BaseTariffPayment.RegopRoundDecimal(2)} и дату {payment.PaymentDate.ToShortDateString()} уже существует",
                            payment.PaymentDate);
                        this.LogDateWarning(FormatTitle(payment),
                            $"Оплата на сумму по тарифу решения {payment.TariffDecisionPayment.RegopRoundDecimal(2)} и дату {payment.PaymentDate.ToShortDateString()} уже существует",
                            payment.PaymentDate);
                        this.LogDateWarning(FormatTitle(payment),
                            $"Оплата на сумму пени {payment.PenaltyPayment.RegopRoundDecimal(2)} и дату {payment.PaymentDate.ToShortDateString()} уже существует",
                            payment.PaymentDate);

                        continue;
                    }

                    entitiesForSave.AddRange(this.ApplyPaymentOrRefund(payment, moneyOperation, packet, acc, importSource, updateSaldo, file.FileName));
                }

                TransactionHelper.InsertInManyTransactions(this.Container, entitiesForSave, entitiesForSave.Count, false, true);

                processed += Take;
                processed = Math.Min(processed, payments.Length);

                var percent = ((decimal)processed / payments.Length) * 80;

                this.Indicate((int)percent + 10, "Обработка данных");

                if (this.IsCancelled())
                {
                    this.LogError("Отмена", "Импорт отменен");
                    break;
                }
            }

            if (!this.IsCancelled())
            {
                this.Indicate(90, "Обновление данных");

                this.UpdateRobjectSummaries(updateSaldo);
            }

            // Записать параметры импорта в "шапку". Для возможности повторного запуска.
            var header = new HeaderOfPaymentsToClosedPeriodsImport
            {
                Task = this.TaskEntryDomain.Load(this.TaskId),
                IsUpdateSaldoIn = updateSaldo,
                Period = this.period,
                ExternalRkcId = externalRkcId
            };
            this.HeaderOfPaymentsToClosedPeriodsImportDomain.Save(header);

            // Записать предупреждения
            TransactionHelper.InsertInManyTransactions(this.Container, warnings, warnings.Count, false, true);

            this.Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();

            return new ImportResult();
        }

        private BasePersonalAccount GetAccount(Payment payment)
        {
            if (!payment.InnerNumber.IsEmpty())
            {
                if (this.accountsByInnerNumber.ContainsKey(payment.InnerNumber))
                {
                    var id = this.accountsByInnerNumber[payment.InnerNumber];

                    return this.accountsById.Get(id);
                }
            }

            if (!payment.ExternalNumber.IsEmpty())
            {
                var possibleAccountIds = this.accountsByExternalNumber.Get(payment.ExternalNumber);

                if (!possibleAccountIds.IsEmpty())
                {
                    if (!payment.InnerRkcId.IsEmpty())
                    {
                        var caId = payment.InnerRkcId.ToLong();

                        if (this.accountsByCashCenterId.ContainsKey(caId))
                        {
                            var accIds = this.accountsByCashCenterId[caId];

                            var accountIds = possibleAccountIds.Where(accIds.Contains).ToArray();

                            if (accountIds.Length > 1)
                            {
                                this.LogAccountWarning(payment, "В указанном РКЦ найдено более одного лицевого счета с таким номером");

                                return null;
                            }

                            if (accountIds.Length == 1)
                            {
                                return this.accountsById.Get(accountIds.First());
                            }
                        }
                    }

                    if (!payment.ExternalRkcId.IsEmpty())
                    {
                        if (this.accountsByCashCenterNumber.ContainsKey(payment.ExternalRkcId))
                        {
                            var accIds = this.accountsByCashCenterNumber[payment.ExternalRkcId];

                            var accountIds = possibleAccountIds.Where(accIds.Contains).ToArray();

                            if (accountIds.Length > 1)
                            {
                                this.LogAccountWarning(payment, "В указанном ркц найдено более одного лицевого счета с таким номером");

                                return null;
                            }

                            if (accountIds.Length == 1)
                            {
                                return this.accountsById.Get(accountIds.First());
                            }
                        }
                    }
                }
            }

            this.LogAccountWarning(payment, "Не удалось определить лицевой счет");
            return null;
        }

        private PersonalAccountPeriodSummary ApplyPaSummary(BasePersonalAccount acc, Payment payment, bool updateSaldo = true)
        {
            var paSummary = this.accountSummaries.Get(acc.Id);

            if (paSummary == null)
            {
                if (!this.DictPersonalAccountPeriodSummaryToCreate.TryGetValue(acc.Id, out paSummary))
                {
                    paSummary = new PersonalAccountPeriodSummary(acc, this.period, 0);
                    this.DictPersonalAccountPeriodSummaryToCreate.Add(acc.Id, paSummary);
                }
            }

            paSummary.ApplyPayment(payment.BaseTariffPayment, payment.TariffDecisionPayment, payment.PenaltyPayment, updateSaldo);
            return paSummary;
        }

        private RealityObjectChargeAccountOperation ApplyRoSummary(BasePersonalAccount acc, Payment payment, bool updateSaldo = true)
        {
            var roSummary = this.roAccountSummaries.Get(acc.Room.RealityObject.Id);

            if (roSummary == null)
            {
                if (!this.DictRoAccountOperationToCreate.TryGetValue(acc.Room.RealityObject.Id, out roSummary))
                {
                    roSummary = new RealityObjectChargeAccountOperation(this.roChargeAccounts.Get(acc.Room.RealityObject.Id), this.period);
                    this.DictRoAccountOperationToCreate.Add(acc.Room.RealityObject.Id, roSummary);
                }
            }

            roSummary.PayTariff(payment.BaseTariffPayment + payment.TariffDecisionPayment, updateSaldo);
            roSummary.PayPenalty(payment.PenaltyPayment);

            return roSummary;
        }

        private IEnumerable<BaseEntity> ApplyPaymentOrRefund(
            Payment payment,
            MoneyOperation operation,
            UnacceptedPaymentPacket packet,
            BasePersonalAccount account,
            PaymentOperationBase baseOperation,
            bool updateSaldo,
            string fileName)
        {
            var result = new List<BaseEntity>();

            var i = 0;
            var count = Enum.GetValues(typeof(WalletType)).Length;
            // Создать трансферы            
            foreach (var paymentType in (WalletType[])Enum.GetValues(typeof(WalletType)))
            {
                var paymentInfo = payment.GetPaymentInfo(paymentType);
                // item1 - Кошелёк
                // item2 - Сумма
                // item3 - Признак оплата/отмена: true - оплата, false - отмена

                if (++i == count && paymentInfo == null)
                {
                    this.LogDateWarning(PaymentsToClosedPeriodsImport.FormatTitle(payment), $"Нулевая оплата ", payment.PaymentDate);
                    continue;
                }

                if (paymentInfo == null)
                {
                    continue;
                }

                var wallet = account.GetMainWallet(paymentInfo.Item1);
                var key = $"{wallet.WalletGuid}#{payment.PaymentDate:dd.MM.yyyy}#{(paymentInfo.Item3 ? paymentInfo.Item2 : -paymentInfo.Item2).RegopRoundDecimal(2):0.00}";
                var targetOrSourceGuid = Guid.NewGuid().ToString();

                // если true - то оплата
                if (this.accountPayments.Get(account.Id).Contains(key))
                {
                    this.LogDateWarning(
                            FormatTitle(payment),
                            paymentInfo.Item3
                                ? $"Оплата {paymentType.GetDisplayName().ToLower()} "
                                  + $"на сумму {paymentInfo.Item2.RegopRoundDecimal(2)} "
                                  + $"и дату {payment.PaymentDate.ToShortDateString()} уже существует"
                                : $"Отмена оплаты {paymentType.GetDisplayName().ToLower()} "
                                  + $"на сумму {paymentInfo.Item2.RegopRoundDecimal(2)} "
                                  + $"и дату {payment.PaymentDate.ToShortDateString()} уже существует",
                           payment.PaymentDate);

                    // Обнулить сумму платежа, чтобы она не пошла в баланс
                    switch (paymentInfo.Item1)
                    {
                        case WalletType.BaseTariffWallet: payment.BaseTariffPayment = 0; break;
                        case WalletType.DecisionTariffWallet: payment.TariffDecisionPayment = 0; break;
                        case WalletType.PenaltyWallet: payment.PenaltyPayment = 0; break;
                    }

                    continue;
                }

                if (paymentInfo.Item3)
                {
                    result.Add(
                        new UnacceptedPayment
                        {
                            Accepted = true,
                            Packet = packet,
                            PaymentType = paymentInfo.Item1 == WalletType.PenaltyWallet ? PaymentType.Penalty : PaymentType.Basic,
                            PaymentDate = payment.PaymentDate,
                            Sum = paymentInfo.Item2,
                            PersonalAccount = account
                        });

                    var transfer = new PersonalAccountPaymentTransfer(account, targetOrSourceGuid, wallet.WalletGuid, paymentInfo.Item2, operation)
                    {
                        Reason = "Оплата " + paymentInfo.Item1.GetDisplayName().ToLower(),
                        PaymentDate = payment.PaymentDate,
                        OperationDate = payment.PaymentDate,
                        IsAffect = true
                    };

                    result.Add(transfer);
                    result.Add(this.CopyIncomeTransfer(transfer, account, paymentInfo.Item1));
                    this.LogInfo(
                        payment,
                        $"Добавлена оплата {paymentType.GetDisplayName().ToLower()} "
                        + $"на сумму {paymentInfo.Item2} "
                        + $"на дату {payment.PaymentDate.ToShortDateString()}");
                }
                else
                {
                    result.Add(
                        new UnacceptedPayment
                        {
                            Accepted = true,
                            Packet = packet,
                            PaymentType = paymentInfo.Item1 == WalletType.PenaltyWallet ? PaymentType.Penalty : PaymentType.Basic,
                            PaymentDate = payment.PaymentDate,
                            Sum = -paymentInfo.Item2,
                            PersonalAccount = account
                        });

                    var transfer = new PersonalAccountPaymentTransfer(account, wallet.WalletGuid, targetOrSourceGuid, paymentInfo.Item2, operation)
                    {
                        Reason = "Отмена оплаты " + paymentInfo.Item1.GetDisplayName().ToLower(),
                        PaymentDate = payment.PaymentDate,
                        OperationDate = payment.PaymentDate,
                        IsAffect = true
                    };

                    result.Add(transfer);
                    result.Add(this.CopyOutcomeTransfer(transfer, account, paymentInfo.Item1));
                    this.LogInfo(
                        payment,
                        $"Добавлена отмена оплаты {paymentType.GetDisplayName().ToLower()} "
                        + $"на сумму {paymentInfo.Item2} "
                        + $"на дату {payment.PaymentDate.ToShortDateString()}");
                }

                var recordImport = new RecordPaymentsToClosedPeriodsImport
                {
                    Period = this.period.Name,
                    Source = TypeTransferSource.ImportsIntoClosedPeriod,
                    DocumentNum = fileName,
                    PaymentAgentName = payment.BankId,
                    PaymentNumberUs = payment.OplataId,
                    OperationDate = DateTime.Now,
                    TransferGuid = targetOrSourceGuid,
                    PaymentOperation = baseOperation
                };

                result.Add(recordImport);
                this.LogImport.CountAddedRows++;
            }

            // Если оплата нулевая - обновлять баланс не надо
            if (payment.BaseTariffPayment != 0 || payment.TariffDecisionPayment != 0 || payment.PenaltyPayment != 0)
            {
                var paSummary = this.ApplyPaSummary(account, payment, updateSaldo);
                if (paSummary != null)
                {
                    result.Add(paSummary);
                }

                var roSummary = this.ApplyRoSummary(account, payment, updateSaldo);
                if (roSummary != null)
                {
                    result.Add(roSummary);
                }
            }

            return result;
        }

        private Transfer CopyIncomeTransfer(Transfer originator, BasePersonalAccount account, WalletType walletType)
        {
            var roWalletGuids = this.walletGuidByRoId.Get(account.Room.RealityObject.Id);

            if (roWalletGuids.Item2.IsEmpty())
            {
                return null;
            }

            var ropa = new RealityObjectPaymentAccount { Id = roWalletGuids.Item1 };
            return new RealityObjectTransfer(ropa, originator.TargetGuid, roWalletGuids.Item2.Get(walletType), originator.Amount, originator.Operation)
            {
                Reason = originator.Reason,
                PaymentDate = originator.PaymentDate,
                OperationDate = originator.OperationDate,
                CopyTransfer = originator.To<PersonalAccountPaymentTransfer>(),
                OriginatorName = account.PersonalAccountNum,
                IsAffect = originator.IsAffect
            };
        }

        private Transfer CopyOutcomeTransfer(Transfer originator, BasePersonalAccount account, WalletType walletType)
        {
            var roWalletGuids = this.walletGuidByRoId.Get(account.Room.RealityObject.Id);

            if (roWalletGuids.Item2.IsEmpty())
            {
                return null;
            }

            var ropa = new RealityObjectPaymentAccount { Id = roWalletGuids.Item1 };
            return new RealityObjectTransfer(ropa, roWalletGuids.Item2.Get(walletType), originator.TargetGuid, originator.Amount, originator.Operation)
            {
                Reason = originator.Reason,
                PaymentDate = originator.PaymentDate,
                OperationDate = originator.OperationDate,
                CopyTransfer = originator.To<PersonalAccountPaymentTransfer>(),
                OriginatorName = account.PersonalAccountNum,
                IsAffect = originator.IsAffect
            };
        }

        private PeriodExistingPaymentsValidationResult CheckPeriodExistingPayments(BasePersonalAccount account, Payment payment)
        {
            var result = new PeriodExistingPaymentsValidationResult();
            var accountPayments = this.accountPayments.Get(account.Id);

            if (accountPayments.IsEmpty())
            {
                return result;
            }

            var paymentKey =
                $"{account.BaseTariffWallet.TransferGuid}#{payment.PaymentDate:dd.MM.yyyy}#{payment.BaseTariffPayment.RegopRoundDecimal(2):0.00}";
            var paymentDecisionKey =
                $"{account.DecisionTariffWallet.TransferGuid}#{payment.PaymentDate:dd.MM.yyyy}#{payment.TariffDecisionPayment.RegopRoundDecimal(2):0.00}";
            var penaltyPaymentKey =
                $"{account.PenaltyWallet.TransferGuid}#{payment.PaymentDate:dd.MM.yyyy}#{payment.PenaltyPayment.RegopRoundDecimal(2):0.00}";

            result.HasBaseTariffPayment = accountPayments.Contains(paymentKey);
            result.HasDecisionTariffPayment = accountPayments.Contains(paymentDecisionKey);
            result.HasPenaltyPayment = accountPayments.Contains(penaltyPaymentKey);

            return result;
        }

        private bool CheckPaymentInPeriod(Payment payment)
        {
            return this.period.Contains(payment.PaymentDate);
        }

        private void UpdateRobjectSummaries(bool updateSaldo)
        {
            try
            {
                var periods =
                    this.Container.ResolveDomain<ChargePeriod>()
                        .GetAll()
                        .Where(x => x.StartDate >= this.period.StartDate) // обновляем только периоды после заданного
                        .ToArray();

                using (var ss = this.Container.Resolve<ISessionProvider>().OpenStatelessSession())
                {
                    var action = new UpdateSaldoSqlAction(ss);

                    const int Take = 10000;
                    int amount = (int)Math.Ceiling((double)this.accountsById.Keys.Count / Take);

                    for (int i = 0; i < amount; i++)
                    {
                        var accountIds = this.accountsById.Keys.ToArray().Skip(i).Take(Take).ToArray();
                        action.UpdatePaSaldos(periods, accountIds, updateSaldo);

                        var percent = ((decimal)i / amount) * 10;
                        this.Indicate((int)percent + 90, "Обновление данных");
                    }

                    action.UpdateRoSaldos(periods, this.walletGuidByRoId.Keys.ToArray());
                }
            }
            catch (Exception)
            {
                this.LogError("Обновление данных", "Во время обновления данных произошла ошибка. Рекомендуется выполнить обновление вручную");
            }
        }

        private void LogInfo(Payment payment, string message)
        {
            this.LogImport.Info(PaymentsToClosedPeriodsImport.FormatTitle(payment), message);
        }

        private void LogError(string title, string message)
        {
            this.LogImport.Error(title, message);
        }

        /// <summary>
        /// Записать предупреждение о ЛС
        /// </summary>
        /// <param name="payment">Запись платежа</param>
        /// <param name="message">Сообщение</param>        
        private void LogAccountWarning(Payment payment, string message)
        {
            var title = PaymentsToClosedPeriodsImport.FormatTitle(payment);

            long? autoComparingAccountId = null;
            string autoComparingInfo = null;
            YesNo isCanAutoCompared = YesNo.No;

            // Если из файла пришли адрес и ФИО - попробовать найти сопоствление. По адресу и ФИО.
            if (payment.Address != string.Empty && payment.Name != string.Empty)
            {
                var cmpr = this.accountRequisites.Get($"{payment.Address}#{payment.Name}".ToUpper());
                if (cmpr != null)
                {
                    autoComparingAccountId = cmpr.Id;
                    autoComparingInfo = $"({cmpr.PersonalAccountNum}) {cmpr.Address} - {cmpr.Name}"; // номер ЛС - адрес - ФИО
                    isCanAutoCompared = YesNo.Yes;
                }
            }

            this.LogImport.Warn(title, message);
            warnings.Add(new AccountWarningInPaymentsToClosedPeriodsImport
            {
                Title = title,
                Message = message,
                InnerNumber = payment.InnerNumber,
                ExternalNumber = payment.ExternalNumber,
                InnerRkcId = payment.InnerRkcId,
                ExternalRkcId = payment.ExternalRkcId,
                Name = payment.Name,
                Address = payment.Address,
                Task = new TaskEntry
                {
                    Id = this.TaskId
                },
                IsProcessed = YesNo.No,
                IsCanAutoCompared = isCanAutoCompared,
                ComparingAccountId = autoComparingAccountId,
                ComparingInfo = autoComparingInfo
            });
        }

        /// <summary>
        /// Записать предупреждение о дате
        /// </summary>
        /// <param name="title">Заголовок</param>
        /// <param name="message">Сообщение</param>
        /// <param name="paymentDate">Дата платежа</param>
        private void LogDateWarning(string title, string message, DateTime paymentDate)
        {
            this.LogImport.Warn(title, message);
            warnings.Add(new DateWarningInPaymentsToClosedPeriodsImport
            {
                Title = title,
                Message = message,
                PaymentDate = paymentDate,
                Task = new TaskEntry
                {
                    Id = this.TaskId
                }
            });
        }

        private static string FormatTitle(Payment payment)
        {
            return string.Format(
                "Лицевой счет \"{0}\" (внешний номер \"{1}\", идентификатор ркц \"{2}\" (\"{3}\"))",
                payment.InnerNumber,
                payment.ExternalNumber,
                payment.InnerRkcId,
                payment.ExternalRkcId);
        }

        private class DataTranslator
        {
            private readonly Dictionary<string, int> headers = new Dictionary<string, int>
            {
                // Значение - порядковый номер колонки в файле. Потребуется, если импортируется файл без заголовка.
                { "vn_ls".ToUpper(), 0 }, // Номер лицевого счета
                { "house_id".ToUpper(), 1 }, // Идентификатор дома
                { "place_type".ToUpper(), 2 }, // Тип населенного пункта
                { "place_name".ToUpper(), 3 }, // Наименование населенного пункта
                { "street_type".ToUpper(), 4 }, // Признак (улица, проспект, переулок и т.д.)
                { "street_name".ToUpper(), 5 }, // Наименование (улицы, проспекта, переулка и т.д.)
                { "house_number".ToUpper(), 6 }, // Номер дома
                { "house_housing".ToUpper(), 7 }, // Корпус
                { "flat_number".ToUpper(), 8 }, // Номер квартиры
                { "flat_chumber".ToUpper(), 9 }, // Номер комнаты
                { "name".ToUpper(), 10 }, // Владелец (Ф.И.О.)
                { "area".ToUpper(), 11 }, // Общая площадь
                { "house_type".ToUpper(), 12 }, // Тип многоквартирного дома (Деревянный = 1; Панельный без лифта = 2; Панельный с лифтом = 3; Иные дома без лифта = 4; Иные дома с лифтом = 5)
                { "flat_type".ToUpper(), 13 }, // Тип помещения (жилое=1, нежилое=0)
                { "id_oplata".ToUpper(), 14 }, // Уникальный номер платежа
                { "id_bank".ToUpper(), 15 }, // Наименование организации, принявшей платеж. (ОАО «Ханты-Мансийский банк», ЗАО «Сургутнефтегазбанк», «Сбербанк России» и т.д.)
                { "payment_type".ToUpper(), 16 }, // Тип платежа (Банк, Касса, Инфокиоски и т.д.)
                { "date".ToUpper(), 17 }, // Дата и время платежа (ДД.ММ.ГГГГ ЧЧ:ММ:СС)
                { "summ_BaseTariff".ToUpper(), 18 }, // Сумма платежа за кап. ремонт (в копейках)
                { "summa_penni".ToUpper(), 19 }, // Сумма платежа за пени (в копейках)               
                // Дополнительные поля, их нет в описании структуры файла
                { "ls".ToUpper(), -1 },
                { "rkc_id".ToUpper(), -1 },
                { "identifikator_rkc".ToUpper(), -1 },
                { "summ_DecisionTariff".ToUpper(), -1 }
            };

            public Payment[] ExtractData(MemoryStream stream, string externalRkcId)
            {
                var result = new List<Payment>();

                using (var sr = new StreamReader(stream, Encoding.GetEncoding(1251)))
                {
                    // Читать до первой непустой строки или пока не будет достигнут конец потока
                    while (!this.InitHeaders(sr) && !sr.EndOfStream)
                    {
                    }

                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();

                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }

                        var row = line.Split(';');

                        result.Add(new Payment(row, this.headers, externalRkcId));
                    }
                }

                return result.ToArray();
            }

            /// <summary>
            /// Инициализировать заголовки
            /// </summary>
            /// <param name="sr">Строка</param>
            /// <remarks>Побочный эффект - сдвиг «курсора» чтения потока на одну строку вперёд</remarks>
            /// <returns>Найден ли заголовок</returns>
            private bool InitHeaders(StreamReader sr)
            {
                // Может быть 2 варианта файла:
                // 1) с заголовком;
                // 2) без заголовка.
                // Если файл без заголовка - в первой строке идут итоги.
                // На счёт первой строки - она пропускается в любом случае, хоть файл с заголовком, хоть без.

                var headersRow = sr.ReadLine();

                // В начале файла есть пропуски (пустые строки).
                if (string.IsNullOrWhiteSpace(headersRow))
                {
                    return false; // Заголовок (или строка итогов) - не найден
                }

                var headers = headersRow.Split(';');

                // Назначить инедксы колонок на основании заголовка пришедшего файла.                
                // Заодно проверить, есть ли вообще у файла заголовок.
                var isHeaderFounded = false; // Заголовок найден
                var isHeaderJustFounded = true; // Заголовок найден только что
                for (int i = 0; i < headers.Length; i++)
                {
                    var h = headers[i].ToUpper();

                    if (this.headers.ContainsKey(h))
                    {
                        this.headers[h] = i;

                        // Нашлась хоть одна метка - считать что у файла есть заголовок
                        isHeaderFounded = true;
                        // Если заголовок найден, то не актуальны индексы колонок прописанные под режим "без заголовка"
                        if (isHeaderJustFounded)
                        {
                            var otherHeaders = this.headers.Where(x => x.Key.ToUpper() != h).Select(x => x.Key).ToList();
                            otherHeaders.ForEach(x => this.headers[x] = -1);
                            isHeaderJustFounded = false;
                        }
                    }
                }

                // Проверить заголовок. Имеет смысл только если у файла он (заголовок) найдент.
                if (isHeaderFounded)
                {
                    this.CheckHeaders();
                }

                return true;
            }

            /// <summary>
            /// Можем ли определять лицевые счета
            /// </summary>
            private void CheckHeaders()
            {
                if (this.headers["ls".ToUpper()] >= 0)
                {
                    return;
                }

                if (this.headers["vn_ls".ToUpper()] >= 0 && (this.headers["rkc_id".ToUpper()] >= 0 || this.headers["identifikator_rkc".ToUpper()] >= 0))
                {
                    return;
                }

                throw new ValidationException("Не удалось получить столбцы для определения ЛС");
            }
        }

        private class Payment
        {
            public Payment(string[] row, Dictionary<string, int> headers, string externalRkcId)
            {
                // 1. Инициализировать поля
                this.InnerNumber = this.GetValue("ls", row, headers);
                this.ExternalNumber = this.GetValue("vn_ls", row, headers);
                this.InnerRkcId = this.GetValue("rkc_id", row, headers);
                this.ExternalRkcId = this.GetValue("identifikator_rkc", row, headers);

                // Из файла не пришёл внешний номер РКЦ
                if (this.ExternalRkcId == string.Empty)
                {
                    // Но пришё в параметрах импорта
                    if (!String.IsNullOrEmpty(externalRkcId))
                    {
                        this.ExternalRkcId = externalRkcId;
                    }
                }

                this.PaymentDate = this.GetValue("date", row, headers).ToDateTime();
                this.BaseTariffPayment = this.GetValue("summ_BaseTariff", row, headers).Replace('.', ',').ToDecimal(); // Если оплата приходит в копейках, добавить / 100
                this.TariffDecisionPayment = this.GetValue("summ_DecisionTariff", row, headers).Replace('.', ',').ToDecimal(); // Если оплата приходит в копейках, добавить / 100
                this.PenaltyPayment = this.GetValue("summa_penni", row, headers).Replace('.', ',').ToDecimal(); // Если оплата приходит в копейках, добавить / 100
                this.BankId = this.GetValue("id_bank", row, headers);
                this.OplataId = this.GetValue("id_oplata", row, headers);

                this.Name = this.GetValue("name", row, headers);

                // 2. Собрать адрес
                var placeType = this.GetValue("place_type", row, headers);
                var placeName = this.GetValue("place_name", row, headers);
                var streetType = this.GetValue("street_type", row, headers);
                var streetName = this.GetValue("street_name", row, headers);
                var houseNumber = this.GetValue("house_number", row, headers);
                var houseHousing = this.GetValue("house_housing", row, headers);
                var flatNumber = this.GetValue("flat_number", row, headers);
                var flatChumber = this.GetValue("flat_chumber", row, headers);

                // Все составные части адреса должны быть в файле. Ну кроме "корпус дома" и "комната" - они могут быть пустыми.
                if (placeType != string.Empty &&
                        placeName != string.Empty &&
                        streetType != string.Empty &&
                        streetName != string.Empty &&
                        houseNumber != string.Empty &&
                        flatNumber != string.Empty)
                {
                    this.Address = string.Format("{0} {1}, {2} {3}, д. {4}, кв. {5}",
                        /*0*/ placeType,
                        /*1*/ placeName,
                        /*2*/ streetType,
                        /*3*/ streetName,
                        /*4*/ houseNumber + (houseHousing != String.Empty ? ", корп. " + houseHousing : ""),
                        /*5*/ flatNumber + (flatChumber != String.Empty ? ", ком. " + flatChumber : ""));
                }
            }

            /// <summary>
            /// Внутренний номер лс
            /// </summary>
            public string InnerNumber { get; private set; }

            /// <summary>
            /// Внешний номер лс
            /// </summary>
            public string ExternalNumber { get; private set; }

            /// <summary>
            /// Внутренний id ркц
            /// </summary>
            public string InnerRkcId { get; private set; }

            /// <summary>
            /// Внешний id ркц
            /// </summary>
            public string ExternalRkcId { get; private set; }

            /// <summary>
            /// Дата оплаты
            /// </summary>
            public DateTime PaymentDate { get; private set; }

            /// <summary>
            /// Сумма оплаты
            /// </summary>
            public decimal BaseTariffPayment { get; set; }

            /// <summary>
            /// Сумма оплаты по тарифу решения
            /// </summary>
            public decimal TariffDecisionPayment { get; set; }

            /// <summary>
            /// Сумма пенни
            /// </summary>
            public decimal PenaltyPayment { get; set; }

            /// <summary>
            /// Идентификатор платежного агента
            /// </summary>
            public string BankId { get; private set; }

            /// <summary>
            /// Номер платежа в УС агента
            /// </summary>
            public string OplataId { get; private set; }

            /// <summary>
            /// ФИО
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// Адрес
            /// </summary>
            public string Address { get; private set; }

            public Tuple<WalletType, decimal, bool> GetPaymentInfo(WalletType walletType)
            {
                switch (walletType)
                {
                    case WalletType.BaseTariffWallet:
                        return this.BaseTariffPayment != 0
                            ? new Tuple<WalletType, decimal, bool>(WalletType.BaseTariffWallet, Math.Abs(this.BaseTariffPayment), this.BaseTariffPayment > 0)
                            : null;

                    case WalletType.DecisionTariffWallet:
                        return this.TariffDecisionPayment != 0
                            ? new Tuple<WalletType, decimal, bool>(WalletType.DecisionTariffWallet, Math.Abs(this.TariffDecisionPayment), this.TariffDecisionPayment > 0)
                            : null;

                    case WalletType.PenaltyWallet:
                        return this.PenaltyPayment != 0
                            ? new Tuple<WalletType, decimal, bool>(WalletType.PenaltyWallet, Math.Abs(this.PenaltyPayment), this.PenaltyPayment > 0)
                            : null;
                    default:
                        return null;
                }
            }

            /// <summary>
            /// По идентификатору колонки, считать значение ячейки из строки
            /// </summary>
            /// <param name="key">Идентификатор колонки</param>
            /// <param name="row">Строка</param>
            /// <param name="headers">Заголовок</param>
            /// <returns>Пустая строка, если колонка не найдена</returns>
            private string GetValue(string key, string[] row, Dictionary<string, int> headers)
            {
                key = key.ToUpper();

                if (headers.ContainsKey(key))
                {
                    var index = headers.Get(key);

                    if (index >= 0)
                    {
                        return row[index] ?? string.Empty;
                    }
                }

                return string.Empty;
            }
        }

        private class PeriodExistingPaymentsValidationResult
        {
            public bool HasBaseTariffPayment { get; set; }

            public bool HasDecisionTariffPayment { get; set; }

            public bool HasPenaltyPayment { get; set; }
        }
    }
}