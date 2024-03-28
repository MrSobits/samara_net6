namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Extensions.Expressions;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.DomainService.PartialOperationCancellation;
    using Bars.Gkh.RegOperator.DomainService.PartialOperationCancellation.Utils;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using Newtonsoft.Json;

    /// <summary>
    /// Сервис перераспределения оплат
    /// </summary>
    public class PersonalAccountRepaymentService : IPersonalAccountRepaymentService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Менеджер файлов
        /// </summary>
        public IFileManager FileManager { get; set; }

        /// <summary>
        /// Сервис сохранения истории ЛС
        /// </summary>
        public IPersonalAccountHistoryCreator HistoryCreator { get; set; }

        /// <summary>
        /// Сервис работы с трансферами
        /// </summary>
        public ITransferDomainService TransferDomainService { get; set; }

        /// <summary>
        /// Репозиторий периодов
        /// </summary>
        public IChargePeriodRepository ChargePeriodRepository { get; set; }

        /// <summary>
        /// Сервис частичной отмены операции
        /// </summary>
        public IPartialOperationCancellationService PartialOperationCancellationService { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="BankAccountStatement"/>
        /// </summary>
        public IDomainService<BankAccountStatement> BankAccountStatementDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="DistributionDetail"/>
        /// </summary>
        public IDomainService<DistributionDetail> DistributionDetailDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="BankDocumentImport"/>
        /// </summary>
        public IDomainService<BankDocumentImport> BankDocumentImportDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="ImportedPayment"/>
        /// </summary>
        public IDomainService<ImportedPayment> ImportedPaymentDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="BasePersonalAccount"/>
        /// </summary>
        public IDomainService<BasePersonalAccount> AccountDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="PersonalAccountChange"/>
        /// </summary>
        public IDomainService<PersonalAccountChange> PersonalAccountChangeDomain { get; set; }

        public IDomainService<CalcAccountRealityObject> CalcAccountRealityObjectDomain { get; set; }

        public ICalcAccountService CalcAccountService { get; set; }

        private Dictionary<long, CalcAccount> roPayAccountNums = new Dictionary<long, CalcAccount>();

        /// <inheritdoc />
        public IDataResult Execute(BaseParams baseParams)
        {
            var transferIds = baseParams.Params.GetAs<List<long>>("transferIds");
            var sourceAccountIds = baseParams.Params.GetAs<long[]>("sourceAccountIds");
            var targetAccountId = baseParams.Params.GetAsId("targetAccountId");
            var reason = baseParams.Params.GetAs<string>("reason");

            var sourceAccounts = this.AccountDomain.GetAll()
                .Where(x => sourceAccountIds.Contains(x.Id))
                .ToList();
            
            if (sourceAccounts.Count < 1)
            {
                return BaseDataResult.Error("Необходимо выбрать лицевой счет для списания оплаты");
            }

            var targetAccount = this.AccountDomain.Get(targetAccountId);

            bool isRepayment = false;

            var realitySource = sourceAccounts
                .Where(x => x.Room != null)
                .Select(x => x.Room.RealityObject.Id).FirstOrDefault();
            var realityTarget = targetAccount.Room.RealityObject.Id;
            if (realityTarget == realitySource)
            {
                isRepayment = true;
            }
            if (isRepayment)
            {
                var walletRepo = this.Container.Resolve<IRepository<Wallet>>();
                var payAccDomain = this.Container.Resolve<IDomainService<RealityObjectPaymentAccount>>();
                var payAccWallet = payAccDomain.GetAll()
                    .Where(x => x.RealityObject != null && x.RealityObject.Id == realitySource && x.BaseTariffPaymentWallet != null)
                    .Select(x => x.BaseTariffPaymentWallet.Id).FirstOrDefault();
                var wallet = walletRepo.Get(payAccWallet);
                if (wallet != null)
                {
                    wallet.Repayment = true;
                    walletRepo.Update(wallet);
                }


            }


            if (targetAccount.IsNull())
            {
                return BaseDataResult.Error("Необходимо выбрать лицевой счет для зачисления оплаты");
            }

            if (transferIds.IsEmpty())
            {
                return BaseDataResult.Error("Необходимо выбрать сумму оплаты");
            }

            FileInfo file = null;
            if (baseParams.Files.Any())
            {
                file = this.FileManager.SaveFile(baseParams.Files.First().Value);
            }

            try
            {
                var transferQuery = this.TransferDomainService.GetAll<PersonalAccountPaymentTransfer>().WhereContains(x => x.Id, transferIds);
                var transferSum = transferQuery.Sum(x => x.Amount);

                var transferSumAccount = transferQuery
                    .Select(
                        x => new
                        {
                            x.Owner.Id,
                            Sum = x.Amount
                        })
                        .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Sum(z => z.Sum));

                if (transferSum <= 0)
                {
                    return BaseDataResult.Error("Необходимо выбрать сумму оплаты");
                }

                this.Container.InTransaction(() =>
                {
                    var result = this.PartialOperationCancellationService.UndoAndRepayment(transferQuery, targetAccount);
                    if (!result.Success)
                    {
                        throw new ValidationException(result.Message);
                    }

                    foreach (var sourceAccount in sourceAccounts)
                    {
                        this.PersonalAccountChangeDomain.Save(this.HistoryCreator.CreateChange(
                            sourceAccount,
                            PersonalAccountChangeType.Repayment,
                            $"Перераспределение оплаты с лицевого счета {sourceAccount.PersonalAccountNum} на ЛС {targetAccount.PersonalAccountNum}",
                            transferSumAccount.Get(sourceAccount.Id).RegopRoundDecimal(2).ToString(),
                            null,
                            DateTime.Today,
                            file,
                            reason));

                        this.PersonalAccountChangeDomain.Save(this.HistoryCreator.CreateChange(
                            targetAccount,
                            PersonalAccountChangeType.Repayment,
                            $"Перераспределение оплаты с лицевого счета {sourceAccount.PersonalAccountNum} на ЛС {targetAccount.PersonalAccountNum}",
                            transferSumAccount.Get(sourceAccount.Id).RegopRoundDecimal(2).ToString(),
                            null,
                            DateTime.Today,
                            file,
                            reason));
                    }  
                });

                if (isRepayment)
                {
                    var walletRepo = this.Container.Resolve<IRepository<Wallet>>();
                    var payAccDomain = this.Container.Resolve<IDomainService<RealityObjectPaymentAccount>>();
                    var payAccWallet = payAccDomain.GetAll()
                        .Where(x => x.RealityObject != null && x.RealityObject.Id == realitySource && x.BaseTariffPaymentWallet != null)
                        .Select(x => x.BaseTariffPaymentWallet.Id).FirstOrDefault();
                    var wallet = walletRepo.Get(payAccWallet);
                    if (wallet != null)
                    {
                        wallet.Repayment = false;
                        walletRepo.Update(wallet);
                    }


                }

                return new BaseDataResult();
            }
            catch (ValidationException exception)
            {
                return BaseDataResult.Error(exception.Message);
            }
        }

        /// <inheritdoc />
        public IDataResult GetDataForUI(BaseParams baseParams)
        {
            var accountIds = baseParams.Params.GetAs<long[]>("accIds");
            var startDate = baseParams.Params.GetAs<DateTime>("startDate");
            var endDate = baseParams.Params.GetAs<DateTime>("endDate");

            var roIds = this.AccountDomain.GetAll()
                .Where(x => accountIds.Contains(x.Id))
                .Select(x => x.Room.RealityObject.Id)
                .Distinct()
                .ToArray();

            this.roPayAccountNums = this.CalcAccountService.GetRobjectsAccounts(roIds, DateTime.Today);

            var resultData = new List<TransferDetailProxy>();

            foreach (var accountId in accountIds)
            {
                var account = this.AccountDomain.Get(accountId);

                var walletGuids = account.GetMainWalletGuids();

                var paymentTransfers = this.TransferDomainService.GetAll<PersonalAccountPaymentTransfer>()
                    .Where(x => x.Owner == account && walletGuids.Contains(x.TargetGuid))
                    .Where(x => !x.Operation.IsCancelled && x.Operation.CanceledOperation == null && !x.IsInDirect && x.IsAffect)
                    .Where(this.GetSourceFilterExpression()) // Фильтр по источникам оплат
                    .WhereIf(startDate.IsValid(), x => x.PaymentDate.Date.Date >= startDate.Date)
                    .WhereIf(endDate.IsValid(), x => x.PaymentDate.Date <= endDate.Date)
                    .AsEnumerable()
                    .GroupBy(x => x.SourceGuid)
                    .ToDictionary(x => x.Key);

                var bankAccountStatements = this.BankAccountStatementDomain.GetAll().WhereContains(x => x.TransferGuid, paymentTransfers.Keys).ToDictionary(x => x.TransferGuid);
                var bankDocumentImports = this.BankDocumentImportDomain.GetAll().WhereContains(x => x.TransferGuid, paymentTransfers.Keys).ToDictionary(x => x.TransferGuid);

                var bankAccStamentDetails = this.DistributionDetailDomain.GetAll()
                    .Where(x => x.Object == account.PersonalAccountNum && x.Source == DistributionSource.BankStatement)
                    .WhereContains(x => x.EntityId, bankAccountStatements.Values.Select(x => x.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.EntityId)
                    .ToDictionary(x => x.Key, y => y.Select(x => new DistributionDetailWrapper(bankAccountStatements.Values.First(z => z.Id == y.Key), x)));

                var importedPayments = this.ImportedPaymentDomain.GetAll()
                    .WhereContains(x => x.BankDocumentImport.Id, bankDocumentImports.Values.Select(x => x.Id).ToArray())
                    .Where(x => x.PersonalAccount == account)
                    .AsEnumerable()
                    .GroupBy(x => x.BankDocumentImport.Id)
                    .ToDictionary(x => x.Key, x => x.ToArray());
                
                foreach (var transferGroups in paymentTransfers)
                {
                    var bankDocument = bankDocumentImports.Get(transferGroups.Key);
                    if (bankDocument.IsNotNull())
                    {
                        var payments = importedPayments.Get(bankDocument.Id);
                        resultData.AddRange(this.ProcessPayments(account, payments, transferGroups.Value, false));
                    }

                    var bankStatement = bankAccountStatements.Get(transferGroups.Key);
                    if (bankStatement.IsNotNull())
                    {
                        var payments = bankAccStamentDetails.Get(bankStatement.Id);
                        resultData.AddRange(this.ProcessPayments(account, payments, transferGroups.Value, true));
                    }
                }
            }

            var loadParams = baseParams.GetLoadParam();
            var data = resultData.AsQueryable()
                .OrderIf(loadParams.Order.Length == 0, true, x => x.PaymentDate)
                .Order(loadParams)
                .Paging(loadParams)
                .ToList();

            return new ListDataResult(data, resultData.Count);
        }

        private IEnumerable<TransferDetailProxy> ProcessPayments<TPayment>(
            BasePersonalAccount account,
            IEnumerable<TPayment> payments,
            IEnumerable<PersonalAccountPaymentTransfer> paymentTransfers,
            bool ignoreDates)
            where TPayment : ICancelablePayment
        {
            return PaymentComparator.Compare(account, payments, paymentTransfers, ignoreDates)
                .Select(x => new TransferDetailProxy
                {
                    Id = x.Value.FirstOrDefault().Id,
                    AccountId = account.Id,
                    PersonalAccountNum = account.PersonalAccountNum,
                    Owner = account.AccountOwner.Name,
                    Address = account.Room.RealityObject.Address,
                    RoPayAccountNum = this.roPayAccountNums.Get(account.Room.RealityObject.Id).AccountNumber,
                    AccountFormVariant = account.Room.RealityObject.AccountFormationVariant.GetDisplayName(),
                    PaymentDate = x.Key.PaymentDate,
                    BaseTariffTransfer = x.Value.FirstOrDefault(y => y.TargetGuid == account.GetMainWallet(WalletType.BaseTariffWallet).TransferGuid),
                    DecisionTariffTransfer = x.Value.FirstOrDefault(y => y.TargetGuid == account.GetMainWallet(WalletType.DecisionTariffWallet).TransferGuid),
                    PenaltyTransfer = x.Value.FirstOrDefault(y => y.TargetGuid == account.GetMainWallet(WalletType.PenaltyWallet).TransferGuid)
                });
        }

        private Expression<Func<PersonalAccountPaymentTransfer, bool>> GetSourceFilterExpression()
        {
            Expression<Func<PersonalAccountPaymentTransfer, bool>> result = x => false;

            result = result.Or(x => this.BankDocumentImportDomain.GetAll().Any(y => y.TransferGuid == x.Operation.OriginatorGuid));
            result = result.Or(x => this.BankAccountStatementDomain.GetAll().Any(y => y.TransferGuid == x.Operation.OriginatorGuid));

            return result;
        }

        private class TransferDetailProxy
        {
            /// <summary>
            /// Идентификатор для модели на клиенте
            /// </summary>
            public long Id { get; set; }

            public long AccountId { get; set; }

            public string PersonalAccountNum { get; set; }

            public string Owner { get; set; }

            public string Address { get; set; }

            public string RoPayAccountNum { get; set; }

            public string AccountFormVariant { get; set; }

            public DateTime PaymentDate { get; set; }

            public decimal? BaseTariffSum => this.BaseTariffTransfer?.Amount;
            public decimal? DecisionTariffSum => this.DecisionTariffTransfer?.Amount;
            public decimal? PenaltySum => this.PenaltyTransfer?.Amount;


            public long? BaseTariffTransferId => this.BaseTariffTransfer?.Id;
            public long? DecisionTariffTransferId => this.DecisionTariffTransfer?.Id;
            public long? PenaltyTransferId => this.PenaltyTransfer?.Id;

            [JsonIgnore]
            public Transfer BaseTariffTransfer { private get; set; }
            [JsonIgnore]
            public Transfer DecisionTariffTransfer { private get; set; }
            [JsonIgnore]
            public Transfer PenaltyTransfer { private get; set; }
        }
    }
}