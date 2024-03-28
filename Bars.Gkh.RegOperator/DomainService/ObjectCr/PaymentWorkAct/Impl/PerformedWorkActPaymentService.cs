namespace Bars.Gkh.RegOperator.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using B4;
    using B4.DataAccess;
    using B4.Modules.FileStorage;
    using B4.Modules.Reports;
    using B4.Utils;

    using Bars.Gkh.Authentification;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;


    using Gkh.Domain;
    using Domain.Repository.RealityObjectAccount;
    using Entities;
    using Entities.Refactor;
    using Entities.ValueObjects;
    using Entities.Wallet;
    using GkhCr.Entities;

    using Castle.Windsor;
    using Gkh.Domain.CollectionExtensions;

    public class PerformedWorkActPaymentService : IPerformedWorkActPaymentService
    {
        public PerformedWorkActPaymentService(
            IWindsorContainer container,
            IDomainService<Wallet> walletDomain,
            IDomainService<PerformedWorkActPayment> paymentDomain,
            IDomainService<PerformedWorkAct> actDomain,
            IDomainService<MoneyLock> moneyLockDomain,
            IDomainService<MoneyOperation> moneyOperationDomain,
            IDomainService<PaymentOrderDetail> detailDomain,
            IChargePeriodRepository chargePeriodRepository,
            IRealityObjectPaymentAccountRepository ropayaccRepo)
        {
            this.container = container;
            this.walletDomain = walletDomain;
            this.paymentDomain = paymentDomain;
            this.actDomain = actDomain;
            this.moneyLockDomain = moneyLockDomain;
            this.moneyOperationDomain = moneyOperationDomain;
            this.detailDomain = detailDomain;
            this.ropayaccRepo = ropayaccRepo;
            this.chargePeriodRepository = chargePeriodRepository;
        }

        private readonly IWindsorContainer container;
        private readonly IDomainService<Wallet> walletDomain;
        private readonly IDomainService<PerformedWorkActPayment> paymentDomain;
        private readonly IDomainService<PerformedWorkAct> actDomain;
        private readonly IDomainService<MoneyLock> moneyLockDomain;
        private readonly IDomainService<MoneyOperation> moneyOperationDomain;
        private readonly IDomainService<PaymentOrderDetail> detailDomain;
        private readonly IRealityObjectPaymentAccountRepository ropayaccRepo;
        private readonly IChargePeriodRepository chargePeriodRepository;

        public IDataResult SaveWithDetails(BaseParams baseParams)
        {
            var proxy = baseParams.Params.GetAs<PerformedWorkActPaymentProxy>("payment");
            var details = baseParams.Params.GetAs<PaymentDetailProxy[]>("details");

            if (proxy == null)
            {
                return BaseDataResult.Error("Некорректные данные по оплате");
            }

            if (proxy.DatePayment.HasValue || proxy.Paid > 0M)
            {
                return BaseDataResult.Error("Редактирование невозможно. По данному распоряжению уже проведена оплата.");
            }

            if (details.SafeSum(x => x.Amount) == 0m)
            {
                return BaseDataResult.Error("Сумма по источникам должна быть больше нуля.");
            }

            var walletIds = details.Select(x => x.WalletId).ToArray();
            var wallets = this.walletDomain.GetAll()
                .Where(x => walletIds.Contains(x.Id))
                .ToDictionary(x => x.Id);

            PerformedWorkActPayment payment;

            if (proxy.Id == 0)
            {
                var act = this.actDomain.Get(proxy.PerformedWorkAct);
                payment = act.CreatePayment(proxy.TypeActPayment, details.Sum(x => x.Amount), DateTime.Now);
            }
            else
            {
                payment = this.paymentDomain.Get(proxy.Id);
                payment.ApplyChanges(proxy);
            }

            var detailSources = details
                .Select(x => new PaymentOrderDetailSource(payment, x, wallets.Get(x.WalletId))
                {
                    WalletName = x.WalletName
                })
                .ToArray();

            return this.SaveWithDetails(payment, detailSources);
        }

        public IDataResult SaveWithDetails(PerformedWorkActPayment payment, PaymentOrderDetailSource[] details)
        {
            var userManager = this.container.Resolve<IGkhUserManager>();

            try
            {
                this.container.InTransaction(() => this.SaveInternal(payment, details, userManager.GetActiveUser()?.Login));
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(false, e.Message);
            }
            catch (Exception)
            {
                return new BaseDataResult(false, "Ошибка при сохранении");
            }
            finally
            {
                this.container.Release(userManager);
            }

            return new BaseDataResult();
        }

        private void SaveInternal(PerformedWorkActPayment payment, PaymentOrderDetailSource[] details, string login)
        {
            this.paymentDomain.SaveOrUpdate(payment); // Перенес сюда, чтобы все операции с БД проходили в одной транзакции

            var account = this.ropayaccRepo.GetByRealtyObject(payment.PerformedWorkAct.Realty);

            var op = new MoneyOperation(payment.TransferGuid, this.chargePeriodRepository.GetCurrentPeriod());
            this.moneyOperationDomain.Save(op);

            var existMoneyLocks = this.moneyLockDomain.GetAll()
                .Where(x => x.IsActive)
                .Where(x => x.TargetGuid == payment.TransferGuid)
                .ToList();

            //Удаляем существующие локи денег
            foreach (var existMoneyLock in existMoneyLocks)
            {
                account.UnlockMoney(existMoneyLock.Wallet, op, existMoneyLock);
                this.walletDomain.Update(existMoneyLock.Wallet);
            }

            //Если деньги > 0, то создаем новые локи
            foreach (var d in details)
            {
                if (d.Detail.Amount == 0)
                {
                    this.detailDomain.DeleteIfExists(d.Detail);
                    continue;
                }

                var moneyLock = account
                    .LockMoney(
                        d.Detail.Wallet,
                        op,
                        d.Detail.Amount,
                        payment.TransferGuid);

                moneyLock.SourceName = d.WalletName;

                this.walletDomain.Update(d.Detail.Wallet);
                this.detailDomain.SaveOrUpdate(d.Detail);
                this.moneyLockDomain.Save(moneyLock);
            }

            this.ropayaccRepo.SaveOrUpdate(account);

            var allDetails = this.detailDomain.GetAll().Where(x => x.PaymentOrder.Id == payment.Id).ToList();
            payment.Sum = allDetails.SafeSum(x => x.Amount);

            this.CreateReport(payment, details);
            this.paymentDomain.Update(payment);
        }

        public IDataResult ExportToTxt(BaseParams baseParams)
        {
            var perfWorkActPaymentDomain = this.container.ResolveDomain<PerformedWorkActPayment>();
            var regopCalcAccRoDomain = this.container.ResolveDomain<CalcAccountRealityObject>();
            var buildContractWorkDomain = this.container.ResolveDomain<BuildContractTypeWork>();
            var regopCalcAccountDomain = this.container.ResolveDomain<RegopCalcAccount>();
            var fileManager = this.container.Resolve<IFileManager>();

            var paymentIds = baseParams.Params.GetAs("paymentId", string.Empty).ToLongArray();

            try
            {
                var text = new StringBuilder();

                text.AppendFormat("ДатаСоздания={0}\r\n", DateTime.Now.ToShortDateString());

                foreach (var paymentId in paymentIds)
                {

                    var payment = perfWorkActPaymentDomain.Get(paymentId);
                    var calcAccountId = regopCalcAccRoDomain.GetAll().Where(x => perfWorkActPaymentDomain.GetAll()
                        .Where(y => y.Id == paymentId)
                        .Any(y => y.PerformedWorkAct.ObjectCr.RealityObject.Id == x.RealityObject.Id))
                        .Select(x => x.Account.Id)
                        .FirstOrDefault();

                    var regopCalcAccount = regopCalcAccountDomain.GetAll()
                        .Where(x => x.Id == calcAccountId)
                        .Select(x => new
                        {
                            x.Id,
                            x.AccountNumber,
                            ContragentCreditOrg =
                                x.ContragentCreditOrg != null
                                    ? new
                                    {
                                        x.ContragentCreditOrg.Id,
                                        Name = x.ContragentCreditOrg.CreditOrg.Name ?? x.ContragentCreditOrg.Name,
                                        x.ContragentCreditOrg.CreditOrg.Inn,
                                        Bik = x.ContragentCreditOrg.CreditOrg.Bik ?? x.ContragentCreditOrg.Bik,
                                        CorrAccount =
                                            x.ContragentCreditOrg.CreditOrg.CorrAccount
                                            ?? x.ContragentCreditOrg.CorrAccount,
                                        Okpo = x.ContragentCreditOrg.CreditOrg.Okpo ?? x.ContragentCreditOrg.Okpo,
                                        x.ContragentCreditOrg.SettlementAccount
                                    }
                                    : null
                        }).FirstOrDefault();

                    var buildContract =
                        buildContractWorkDomain.GetAll().FirstOrDefault(x => perfWorkActPaymentDomain.GetAll()
                            .Where(y => y.Id == paymentId)
                            .Any(y => y.PerformedWorkAct.TypeWorkCr.Id == x.TypeWork.Id));
                    

                    text.AppendLine("Документ=Платежное поручение");
                    text.AppendFormat("ДатаРаспоряжения={0}\r\n",
                        payment.DatePayment.HasValue ? payment.DatePayment.Value.ToShortDateString() : string.Empty);
                    text.AppendFormat("СуммаРаспоряжения={0}\r\n", payment.Sum.RegopRoundDecimal(2));
                    text.AppendFormat("ПлательщикСчет={0}\r\n", regopCalcAccount.Return(x => x.AccountNumber));
                    text.AppendFormat("ПлательщикИНН={0}\r\n", regopCalcAccount.Return(x => x.ContragentCreditOrg.Inn));
                    text.AppendFormat("ПлательщикРасчСчет={0}\r\n", regopCalcAccount.Return(x => x.AccountNumber));
                    text.AppendFormat("ПлательщикБанк={0}\r\n", regopCalcAccount.Return(x => x.ContragentCreditOrg.Name));
                    text.AppendFormat("ПлательщикБИК={0}\r\n", regopCalcAccount.Return(x => x.ContragentCreditOrg.Bik));
                    text.AppendFormat("ПлательщикКорсчет={0}\r\n",
                        regopCalcAccount.Return(x => x.ContragentCreditOrg.CorrAccount));
                    text.AppendFormat("ПолучательИНН={0}\r\n",
                        buildContract.Return(x => x.BuildContract.Builder.Contragent.Inn));
                    text.AppendFormat("Получатель={0}\r\n",
                        buildContract.Return(x => x.BuildContract.Builder.Contragent.Name));
                    text.AppendFormat("ПолучательКПП={0}\r\n",
                        buildContract.Return(x => x.BuildContract.Builder.Contragent.Kpp));
                    text.AppendLine("НазначениеПлатежа=Оплата акта");
                    text.AppendLine("КонецДокумента");
                }

                text.AppendLine("КонецФайла");

                var byteArray = Encoding.UTF8.GetBytes(text.ToStr());

                return new BaseDataResult(fileManager.SaveFile("Выгрузка", "txt", byteArray));
            }
            finally
            {
                this.container.Release(perfWorkActPaymentDomain);
                this.container.Release(regopCalcAccRoDomain);
                this.container.Release(buildContractWorkDomain);
            }
        }

        private IEnumerable<PaymentDetailProxy> GetProxies(RealityObjectPaymentAccount account)
        {
            var proxies = new List<PaymentDetailProxy>
            {
                new PaymentDetailProxy(account.BankPercentWallet),
                new PaymentDetailProxy(account.BaseTariffPaymentWallet),
                new PaymentDetailProxy(account.DecisionPaymentWallet),
                new PaymentDetailProxy(account.FundSubsidyWallet),
                new PaymentDetailProxy(account.OtherSourcesWallet),
                new PaymentDetailProxy(account.PenaltyPaymentWallet),
                new PaymentDetailProxy(account.RegionalSubsidyWallet),
                new PaymentDetailProxy(account.StimulateSubsidyWallet),
                new PaymentDetailProxy(account.TargetSubsidyWallet),
                new PaymentDetailProxy(account.SocialSupportWallet)
            };

            return proxies;
        }

        private void CreateReport(PerformedWorkActPayment payment, PaymentOrderDetailSource[] details)
        {
            IPrintForm printForm;

            if (this.container.Kernel.HasComponent("PaymentSrcFinanceDetailsReport"))
            {
                printForm = this.container.Resolve<IPrintForm>("PaymentSrcFinanceDetailsReport");
            }
            else
            {
                return;
            }

            var baseparams = new BaseParams
            {
                Params = new DynamicDictionary
                {
                    {"paymentId", payment.Id},
                    {"details", details}
                }
            };

            /*// такой костыль нужен для отчета потомуч то он принимает параметр такой
            baseParams.Params.Add("paymentId", payment.Id);*/

            var rp = new ReportParams();
            printForm.SetUserParams(baseparams);
            printForm.PrepareReport(rp);
            var template = printForm.GetTemplate();

            IReportGenerator generator;
            if (printForm is IGeneratedPrintForm)
            {
                generator = printForm as IGeneratedPrintForm;
            }
            else
            {
                generator = this.container.Resolve<IReportGenerator>("XlsIoGenerator");
            }

            var fileManager = this.container.Resolve<IFileManager>();
            var fileDomain = this.container.ResolveDomain<B4.Modules.FileStorage.FileInfo>();

            using (var result = new MemoryStream())
            {
                generator.Open(template);
                generator.Generate(result, rp);
                result.Seek(0, SeekOrigin.Begin);

                var fileInfo = fileManager.SaveFile(result, "PaymentSrcFinanceDetailsReport.xlsx");
                fileDomain.Save(fileInfo);

                payment.Document = fileInfo;
                // TODO убрать костыль (29236)
                payment.HandMade = true;
            }
        }
    }

    public class PaymentOrderDetailSource
    {
        public PaymentOrderDetailSource(PerformedWorkActPayment payment, PaymentDetailProxy proxy, Wallet wallet)
        {
            this.Detail = new PaymentOrderDetail(payment, wallet, proxy.Amount)
            {
                Id = proxy.Id
            };
        }

        public PaymentOrderDetail Detail { get; set; }

        public string WalletName { get; set; }
    }

    public class PaymentDetailProxy
    {
        public PaymentDetailProxy()
        {
        }

        public PaymentDetailProxy(Wallet wallet, decimal amount = 0, long id = 0)
        {
            this.Id = id;
            this.WalletId = wallet.Id;
            this.Balance = wallet.Balance;
            this.WalletGuid = wallet.WalletGuid;
            this.Amount = amount;
        }
        public long Id { get; set; }
        public long WalletId { get; set; }
        public string WalletName { get; set; }
        public decimal Balance { get; set; }
        public decimal Amount { get; set; }
        public string WalletGuid { get; set; }
    }
}