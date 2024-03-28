namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Microsoft.Extensions.Logging;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.ConfigSections.RegOperator.Enums;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.RegOperator.Domain.Extensions;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.Domain.Repository.RealityObjectAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Refactor;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Exceptions;
    using Bars.Gkh.RegOperator.Extenstions;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using Ionic.Zip;
    using Ionic.Zlib;

    using Converter = Bars.B4.DomainService.BaseParams.Converter;

    /// <summary>
    /// Сервис для работы с сущностью "Заявка на перечисление средств подрядчикам"
    /// </summary>
    public class TransferCtrService : ITransferCtrService
    {
        public IWindsorContainer Сontainer { get; set; }
        public IDomainService<TransferCtr> TransferCtrDomain { get; set; }
        public IDomainService<TransferCtrPaymentDetail> DetailDomain { get; set; }
        public IDomainService<MoneyOperation> MoneyOperationDomain { get; set; }
        public IDomainService<MoneyLock> MoneyLockDomain { get; set; }
        public IDomainService<Wallet> WalletDomain { get; set; }
        public IDomainService<ContragentBankCreditOrg> ContragentBankCreditOrgDomain { get; set; }
        public IRealityObjectPaymentAccountRepository RopayaccRepo { get; set; }
        public IChargePeriodRepository ChargePeriodRepository { get; set; }
        public IFileManager FileManager { get; set; }

        /// <summary>
        /// Экспорт в txt формат
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Zip архив со сформированными документами</returns>
        public IDataResult ExportToTxt(BaseParams baseParams)
        {
            var transferIds = baseParams.Params.GetAs("transferIds", string.Empty).ToLongArray();
            var isPrintOldDoc = baseParams.Params.GetAs<bool>("isPrintOldDoc");

            var cultInfo = new CultureInfo("en-US", false);

            const string header = @"1CClientBankExchange
ВерсияФормата=1.02
Кодировка=Windows
Отправитель=ВДГБ: Бухгалтерия для некоммерческой организации, редакция 4.4
Получатель=Система ДБО BS-Client";

            var transferCtrQuery = this.TransferCtrDomain.GetAll()
                .Where(x => transferIds.Contains(x.Id));

            var transfersCtr = transferCtrQuery
                .Select(
                    x => new
                    {
                        x.Id,
                        x.DateFrom,
                        x.DocumentNum,
                        x.DocumentNumPp,
                        x.DateFromPp,
                        x.PaymentPurposeDescription,
                        x.RegopCalcAccount.AccountNumber,
                        x.IsExport,
                        x.Sum,
                        x.KindPayment,
                        RegOperator = x.RegOperator.Contragent.Name,
                        RegOperatorInn = x.RegOperator.Contragent.Inn,
                        RegOperatorKpp = x.RegOperator.Contragent.Kpp,
                        CreditOrg = x.RegopCalcAccount.ContragentCreditOrg.CreditOrg.Name,
                        CreditOrgBik = x.RegopCalcAccount.ContragentCreditOrg.CreditOrg.Bik,
                        CreditOrgCorrAccount = x.RegopCalcAccount.ContragentCreditOrg.CreditOrg.CorrAccount,
                        CreditOrgPlaceName = x.RegopCalcAccount.ContragentCreditOrg.CreditOrg.FiasAddress.PlaceName,
                        BuilderName = x.Builder.Contragent.Name,
                        BuilderInn = x.Builder.Contragent.Inn,
                        BuilderKpp = x.Builder.Contragent.Kpp,
                        CtrgBankId = x.ContragentBank.Id,
                        CtrgBankSettleAcc = x.ContragentBank.SettlementAccount,
                        CtrgBankName = x.ContragentBank.Name,
                        CtrgBankBik = x.ContragentBank.Bik,
                        CtrgBankCorrAccount = x.ContragentBank.CorrAccount,
                        x.Document,
                        x.PaymentPriority
                    })
                .ToList();

            var ctrgBankCreditOrg = this.ContragentBankCreditOrgDomain.GetAll()
                .Where(x => transferCtrQuery.Any(y => y.ContragentBank.Id == x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.CreditOrg.FiasAddress.PlaceName
                })
                .ToDictionary(x => x.Id, y => y.PlaceName);

            var minDate = transfersCtr.Where(x => x.DateFrom.HasValue).SafeMin(x => x.DateFrom);
            var maxDate = transfersCtr.SafeMax(x => x.DateFrom);
            var calcAccountNumbers = transfersCtr.Where(x => !x.AccountNumber.IsEmpty()).Select(x => x.AccountNumber).Distinct().ToList();

            if (calcAccountNumbers.Count > 1)
            {
                throw new ValidationException(@"В выбранных заявках счет плательщика не совпадает. Формирование платежных поручений не доступно.
Необходимо выбрать заявки с одинаковым счетом плательщика и сформировать платежные поручения.");
            }

            var exportsZip = new ZipFile(Encoding.UTF8)
            {
                CompressionLevel = CompressionLevel.Level3,
                AlternateEncoding = Encoding.GetEncoding("cp866")
            };

            var updateList = new List<TransferCtr>();

            foreach (var transferCtr in transfersCtr)
            {
                if (isPrintOldDoc && transferCtr.Document != null)
                {
                    //для существующих документов печать не нужна
                    continue;
                }

                var transfersCtrInfo = new StringBuilder();
                transfersCtrInfo.AppendLine(header);
                transfersCtrInfo.AppendLine($"ДатаСоздания={DateTime.Now.ToShortDateString()}");
                transfersCtrInfo.AppendLine($"ВремяСоздания={DateTime.Now:HH:mm:ss}");
                transfersCtrInfo.AppendLine($"ДатаНачала={(minDate.HasValue ? minDate.Value.ToShortDateString() : string.Empty)}");
                transfersCtrInfo.AppendLine($"ДатаКонца={(maxDate.HasValue ? maxDate.Value.ToShortDateString() : string.Empty)}");
                transfersCtrInfo.AppendLine($"РасчСчет={calcAccountNumbers.FirstOrDefault()}");

                transfersCtrInfo.AppendLine("Документ=Платежное поручение");
                transfersCtrInfo.AppendLine("СекцияДокумент=Платежное поручение");
                transfersCtrInfo.AppendLine($"Номер={transferCtr.DocumentNumPp}");
                transfersCtrInfo.AppendLine($"Дата={(transferCtr.DateFromPp.HasValue ? transferCtr.DateFromPp.Value.ToShortDateString() : string.Empty)}");
                transfersCtrInfo.AppendLine(string.Format(cultInfo, "Сумма={0:0.00}", decimal.Round(transferCtr.Sum, 2)));
                transfersCtrInfo.AppendLine($"ПлательщикСчет={transferCtr.AccountNumber}");
                transfersCtrInfo.AppendLine("ДатаСписано=");
                transfersCtrInfo.AppendLine($"Плательщик=ИНН{transferCtr.RegOperatorInn} {transferCtr.RegOperator}");
                transfersCtrInfo.AppendLine($"ПлательщикИНН={transferCtr.RegOperatorInn}");
                transfersCtrInfo.AppendLine($"Плательщик1={transferCtr.RegOperator}");
                transfersCtrInfo.AppendLine($"ПлательщикРасчСчет={transferCtr.AccountNumber}");
                transfersCtrInfo.AppendLine($"ПлательщикБанк1={transferCtr.CreditOrg}");
                transfersCtrInfo.AppendLine($"ПлательщикБанк2={transferCtr.CreditOrgPlaceName}");
                transfersCtrInfo.AppendLine($"ПлательщикБИК={transferCtr.CreditOrgBik}");
                transfersCtrInfo.AppendLine($"ПлательщикКорсчет={transferCtr.CreditOrgCorrAccount}");
                transfersCtrInfo.AppendLine($"ПолучательСчет={transferCtr.CtrgBankSettleAcc}");
                transfersCtrInfo.AppendLine("ДатаПоступило=");
                transfersCtrInfo.AppendLine($"Получатель=ИНН{transferCtr.BuilderInn} {transferCtr.BuilderName}");
                transfersCtrInfo.AppendLine($"ПолучательИНН={transferCtr.BuilderInn}");
                transfersCtrInfo.AppendLine($"Получатель1={transferCtr.BuilderName}");
                transfersCtrInfo.AppendLine($"ПолучательРасчСчет={transferCtr.CtrgBankSettleAcc}");
                transfersCtrInfo.AppendLine($"ПолучательБанк1={transferCtr.CtrgBankName}");
                transfersCtrInfo.AppendLine($"ПолучательБанк2={ctrgBankCreditOrg.Get(transferCtr.CtrgBankId)}");
                transfersCtrInfo.AppendLine($"ПолучательБИК={transferCtr.CtrgBankBik}");
                transfersCtrInfo.AppendLine($"ПолучательКорсчет={transferCtr.CtrgBankCorrAccount}");
                transfersCtrInfo.AppendLine($"ВидПлатежа={transferCtr.KindPayment.GetEnumMeta().Display}");
                transfersCtrInfo.AppendLine("ВидОплаты=01");
                transfersCtrInfo.AppendLine($"ПлательщикКПП={transferCtr.RegOperatorKpp}");
                transfersCtrInfo.AppendLine($"ПолучательКПП={transferCtr.BuilderKpp}");
                transfersCtrInfo.AppendLine($"Очередность={transferCtr.PaymentPriority}");
                transfersCtrInfo.AppendLine($"НазначениеПлатежа={transferCtr.PaymentPurposeDescription}");
                transfersCtrInfo.AppendLine("НазначениеПлатежа1=");
                transfersCtrInfo.AppendLine("НазначениеПлатежа2=");
                transfersCtrInfo.AppendLine("КонецДокумента");

                transfersCtrInfo.AppendLine("КонецФайла");

                var byteArray = Encoding.GetEncoding(1251).GetBytes(transfersCtrInfo.ToStr());

                var rec = this.TransferCtrDomain.Load(transferCtr.Id);
                rec.Document = this.FileManager.SaveFile(transferCtr.DocumentNum, "txt", byteArray);
                rec.IsExport = true;

                exportsZip.AddEntry($"{transferCtr.DocumentNum}.txt", this.FileManager.GetFile(rec.Document));

                updateList.Add(rec);
            }

            TransactionHelper.InsertInManyTransactions(this.Сontainer, updateList, 10000, true, true);

            using (var exportFile = new MemoryStream())
            {
                exportsZip.Save(exportFile);

                return new BaseDataResult(this.FileManager.SaveFile(exportFile, "Выгрузка.zip"));
            }
        }

        /// <summary>
        /// Сохранить с детализацией
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Сохраненная запись</returns>
        public IDataResult SaveWithDetails(BaseParams baseParams)
        {
            var transferCtr =
                baseParams.Params.Read<SaveParam<TransferCtr>>()
                    .Execute(c => Converter.ToSaveParam<TransferCtr>(c, false))
                    .First().AsObject();

            var details = baseParams.Params.GetAs<PaymentDetailProxy[]>("details");
            var loanFormationVariant = this.Сontainer.GetGkhConfig<RegOperatorConfig>().GeneralConfig.LoanConfig.LoanFormationType;

            // необходимо пересчитать сумму заявки: если сумма 0 или настройки о формировании займа не на основании заявок
            var calcSum = loanFormationVariant != LoanFormationType.ByTransferCtr || transferCtr.Sum == 0;

            if (!calcSum && details.SafeSum(x => x.Amount) > transferCtr.Sum)
            {
                return BaseDataResult.Error("Сумма заявки не может быть меньше сумм распределяемых оплат (столбец \"Оплата\")");
            }

            try
            {
                this.Сontainer.InTransaction(
                    () =>
                    {
                        var newTransfer = transferCtr.Id == 0;

                        var res = newTransfer ? this.TransferCtrDomain.Save(baseParams) : this.TransferCtrDomain.Update(baseParams);

                        transferCtr = res.Data.To<List<TransferCtr>>().First();

                        if (newTransfer)
                        {
                            transferCtr.TransferGuid = Guid.NewGuid().ToString();
                        }

                        this.SaveNumber(transferCtr);

                        this.SaveWithDetails(transferCtr, this.Convert(details, transferCtr), calcSum);
                    });
            }
            catch (ValidationException e)
            {
                return BaseDataResult.Error(e.Message);
            }
            catch (MoneyLockException)
            {
                return BaseDataResult.Error("Значение в поле «Оплата, руб.». не должно превышать баланс источника. Сохранение не проведено.");
            }
            catch (Exception e)
            {
                this.Сontainer.Resolve<ILogger>()
                    .LogError(
                        "method: {0} message: {1} stacktrace: {2}",
                        "TransferCtrService.SaveWithDetails(BaseParams)",
                        e.Message,
                        e.StackTrace);
                return BaseDataResult.Error(e.Message);
            }

            return new SaveDataResult(new[] {transferCtr});
        }

        /// <summary>
        /// Сохранить с детализацией
        /// </summary>
        /// <param name="transferCtr">Заявка на перечисление средств подрядчикам</param>
        /// <param name="details">Детализация оплат заявки на перечисление средств подрядчикам</param>
        /// <param name="calcSum">Подсчитывать сумму заявки по оплатам из распределения</param>
        public void SaveWithDetails(TransferCtr transferCtr, IEnumerable<TransferCtrPaymentDetail> details, bool calcSum = true)
        {
            var account = this.RopayaccRepo.GetByRealtyObject(transferCtr.ObjectCr.RealityObject);
            var wallets = account.GetWallets();

            foreach (var wallet in wallets.Where(x => x.Id == 0))
            {
                this.WalletDomain.Save(wallet);
            }

            var moneyOperation = new MoneyOperation(transferCtr.TransferGuid, this.ChargePeriodRepository.GetCurrentPeriod());
            this.MoneyOperationDomain.Save(moneyOperation);

            var existMoneyLocks = this.MoneyLockDomain.GetAll()
                .Where(x => x.TargetGuid == transferCtr.TransferGuid)
                .Where(x => x.IsActive)
                .ToList();

            //Удаляем существующие локи денег
            foreach (var existMoneyLock in existMoneyLocks)
            {
                account.UnlockMoney(existMoneyLock.Wallet, moneyOperation, existMoneyLock);
                this.WalletDomain.Update(existMoneyLock.Wallet);
            }

            //Если деньги > 0, то создаем новые локи
            foreach (var detail in details)
            {
                if (detail.Amount == 0)
                {
                    this.DetailDomain.DeleteIfExists(detail);
                    continue;
                }

                var moneyLock = account
                    .LockMoney(
                        detail.Wallet,
                        moneyOperation,
                        detail.Amount,
                        transferCtr.TransferGuid);

                moneyLock.SourceName = detail.Wallet.GetWalletName(this.Сontainer, account);

                this.WalletDomain.Update(detail.Wallet);
                this.DetailDomain.SaveOrUpdate(detail);
                this.MoneyLockDomain.Save(moneyLock);
            }

            this.RopayaccRepo.SaveOrUpdate(account);

            var allDetails = this.DetailDomain.GetAll().Where(x => x.TransferCtr.Id == transferCtr.Id).ToArray();

            if (calcSum)
            {
                transferCtr.Sum = allDetails.SafeSum(x => x.Amount);
            }

            this.TransferCtrDomain.Update(transferCtr);
        }

        /// <summary>
        /// Скопировать заявку
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult Copy(BaseParams baseParams)
        {
            var transferCtrRecordId = baseParams.Params.GetAs<long>("transferCtrRecordId");
            var sourceTransfer = this.TransferCtrDomain.Get(transferCtrRecordId);

            if (sourceTransfer != null)
            {
                var targetTransfer = new TransferCtr
                {
                    DateFrom = DateTime.Now,
                    TypeProgramRequest = sourceTransfer.TypeProgramRequest,
                    ProgramCrType = sourceTransfer.ProgramCrType,
                    ProgramCr = sourceTransfer.ProgramCr,
                    ObjectCr = sourceTransfer.ObjectCr,
                    TypeWorkCr = sourceTransfer.TypeWorkCr,
                    FinSource = sourceTransfer.FinSource,
                    Builder = sourceTransfer.Builder,
                    ContragentBank = sourceTransfer.ContragentBank,
                    Contract = sourceTransfer.Contract,
                    RegOperator = sourceTransfer.RegOperator,
                    RegopCalcAccount = sourceTransfer.RegopCalcAccount,
                    Perfomer = sourceTransfer.Perfomer,
                    File = sourceTransfer.File,
                    Comment = sourceTransfer.Comment,
                    PaymentType = sourceTransfer.PaymentType,
                    KindPayment = sourceTransfer.KindPayment,
                    PaymentPriority = sourceTransfer.PaymentPriority,
                    TypeCalculationNds = sourceTransfer.TypeCalculationNds,
                    PaymentPurposeDescription = sourceTransfer.PaymentPurposeDescription
                };

                var lastNumber = this.TransferCtrDomain
                    .GetAll()
                    .SafeMax(x => x.DocumentNumPp);

                targetTransfer.DocumentNumPp = lastNumber + 1;

                this.TransferCtrDomain.Save(targetTransfer);

                this.DetailDomain.GetAll()
                    .Where(x => x.TransferCtr.Id == sourceTransfer.Id)
                    .ForEach(
                        x =>
                        {
                            var targetTransdferDetail = new TransferCtrPaymentDetail
                            {
                                TransferCtr = targetTransfer,
                                Amount = x.Amount,
                                PaidSum = x.PaidSum,
                                RefundSum = x.RefundSum,
                                Wallet = x.Wallet
                            };

                            this.DetailDomain.Save(targetTransdferDetail);
                        });

                return new BaseDataResult(targetTransfer);
            }

            return new BaseDataResult(false, "Исходная заявка не найдена");
        }

        /// <summary>
        /// Список записей TransferCtr
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public IQueryable<TransferCtrProxy> List(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var municipalities = baseParams.Params.GetAs<string>("municipalities").ToLongArray();
            var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");
            var showWithPaidSum = baseParams.Params.GetAs<bool>("showWithPaidSum");

            return this.TransferCtrDomain.GetAll()
                .WhereIf(municipalities.Any(), x => municipalities.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .WhereIf(dateStart != DateTime.MinValue, x => x.DateFrom >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DateFrom <= dateEnd)
                .WhereIf(showWithPaidSum, x => x.PaidSum > 0)
                .Select(
                    x => new TransferCtrProxy
                    {
                        Id = x.Id,
                        DocumentNum = x.DocumentNum,
                        DateFrom = x.DateFrom,
                        State = x.State,
                        IsExport = x.IsExport,
                        Sum = x.Sum,
                        PaidSum = x.PaidSum,
                        TypeProgramRequest = x.ProgramCr.TypeProgramCr,
                        Municipality = x.ObjectCr.RealityObject.Municipality.Name,
                        ObjectCr = x.ObjectCr.RealityObject.Address,
                        ProgramCr = x.ProgramCr.Name,
                        Builder = x.Builder.Contragent.Name,
                        Contract = x.Contract.DocumentName,
                        BuilderInn = x.Builder.Contragent.Inn,
                        BuilderSettlAcc = x.ContragentBank.SettlementAccount,
                        CalcAccNumber = x.RegopCalcAccount.AccountNumber,
                        Perfomer = x.Perfomer,
                        Document = x.Document,
                        TypeWorkCr = x.TypeWorkCr.Work.Name
                    })
                .Filter(loadParams, this.Сontainer);
        }

        private IEnumerable<TransferCtrPaymentDetail> Convert(PaymentDetailProxy[] proxies, TransferCtr transferCtr)
        {
            var walletIds = proxies.Select(x => x.WalletId).Distinct().ToArray();

            var existsItems = this.DetailDomain.GetAll()
                .Where(x => x.TransferCtr.Id == transferCtr.Id)
                .Where(x => walletIds.Contains(x.Wallet.Id))
                .ToDictionary(x => x.Wallet.Id);

            var wallets = this.WalletDomain.GetAll()
                .Where(x => walletIds.Contains(x.Id))
                .ToDictionary(x => x.Id);

            var result = new List<TransferCtrPaymentDetail>();

            foreach (var proxy in proxies)
            {
                var item =
                    existsItems.Get(proxy.WalletId)
                        ?? new TransferCtrPaymentDetail(transferCtr, wallets.Get(proxy.WalletId));

                item.Amount = proxy.Amount;

                result.Add(item);
            }

            return result;
        }

        private void SaveNumber(TransferCtr transferCtr)
        {
            if (transferCtr.DocumentNumPp < 0)
            {
                throw new ValidationException("Номер не соответствует формату: целое неотрицательное число.");
            }

            if (transferCtr.DocumentNumPp != 0 &&
                this.TransferCtrDomain.GetAll().Where(x => x.Id != transferCtr.Id).Any(x => transferCtr.DocumentNumPp == x.DocumentNumPp))
            {
                throw new ValidationException("Такой номер уже существует. Укажите новое значение.");
            }

            if (transferCtr.DocumentNumPp == 0)
            {
                var lastNumber = this.TransferCtrDomain
                    .GetAll()
                    .Where(x => x.Id != transferCtr.Id)
                    .SafeMax(x => x.DocumentNumPp);

                transferCtr.DocumentNumPp = lastNumber + 1;
            }
        }

        /// <summary>
        /// Прокси класс для сущности "Детализация оплат заявки на перечисление средств подрядчикам"
        /// </summary>
        protected class PaymentDetailProxy
        {
            /// <summary>
            /// Идентификатор кошелька
            /// </summary>
            public long WalletId { get; set; }

            /// <summary>
            /// Количество
            /// </summary>
            public decimal Amount { get; set; }
        }
    }
}