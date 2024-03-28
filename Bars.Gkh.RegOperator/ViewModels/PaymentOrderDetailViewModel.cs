namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.IoC;

    using Domain.Extensions;
    using Domain.Repository.RealityObjectAccount;

    using Entities;
    using Entities.Wallet;

    using Gkh.Entities;

    using GkhCr.Entities;

    /// <summary>
    /// Вьюха для отображения источников финансирования при оплате акта выполненных работ
    /// </summary>
    public class PaymentOrderDetailViewModel : BaseViewModel<PaymentOrderDetail>
    {
        /// <summary>
        /// Вернуть список источников
        /// </summary>
        /// <param name="domainService">Домен-сервис "Детализация распоряжения об оплате акта выполненных работ"</param>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Список источников</returns>
        public override IDataResult List(IDomainService<PaymentOrderDetail> domainService, BaseParams baseParams)
        {
            var paymentDomain = this.Container.Resolve<IDomainService<PerformedWorkActPayment>>();
            var actDomain = this.Container.Resolve<IDomainService<PerformedWorkAct>>();
            var detailsDomain = this.Container.Resolve<IDomainService<PaymentOrderDetail>>();
            var paymentAccountRepo = this.Container.Resolve<IRealityObjectPaymentAccountRepository>();
            var existDetailWalletIds = new long[0];
            var details = new List<PaymentOrderDetail>();
            using (this.Container.Using(paymentDomain, actDomain, detailsDomain, paymentAccountRepo))
            {
                RealityObject ro;
                var loadParam = this.GetLoadParam(baseParams);

                var paymentId = baseParams.Params.GetAs<long>("paymentId");
                if (paymentId == 0)
                {
                    paymentId = loadParam.Filter.GetAs<long>("paymentId");
                }

                var payment = paymentDomain.FirstOrDefault(x => x.Id == paymentId);
                if (payment == null)
                {
                    var actId = baseParams.Params.GetAs<long>("performedWorkActId");
                    if (actId == 0)
                    {
                        actId = loadParam.Filter.GetAs<long>("performedWorkActId");
                    }

                    var act = actDomain.FirstOrDefault(x => x.Id == actId);
                    if (act == null)
                    {
                        return new BaseDataResult(false, "Данного акта не существует");
                    }

                    ro = act.Realty;
                }
                else
                {
                    details = detailsDomain.GetAll().Where(x => x.PaymentOrder.Id == payment.Id).ToList();
                    existDetailWalletIds = details.Select(x => x.Wallet.Id).ToArray();
                    ro = payment.PerformedWorkAct.Realty;
                }

                if (ro == null)
                {
                    return new BaseDataResult(false, "Необходимого дома не существует");
                }

                var paymentAccount = paymentAccountRepo.GetByRealtyObject(ro);

                if (paymentAccount == null)
                {
                    return new BaseDataResult(false, "Необходимого счета дома не существует");
                }

                var data = details
                    .Select(x => new PaymentDetailProxy(x.Wallet, x.Wallet.GetWalletName(this.Container, paymentAccount), x.Amount, x.Id))
                    .Union(this.GetProxies(paymentAccount).Where(x => !existDetailWalletIds.Contains(x.WalletId)))
                    .Where(x => x.Balance > 0 || x.Amount > 0)
                    .AsQueryable()
                    .Filter(loadParam, this.Container);

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToArray(), data.Count());
            }
        }

        /// <summary>
        /// Класс-прокси для источников финансирования
        /// </summary>
        public class PaymentDetailProxy
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public PaymentDetailProxy()
            {
            }

            /// <summary>
            /// Конструктор с параметрами
            /// </summary>
            /// <param name="wallet">Кошелёк</param>
            /// <param name="walletName">Наименование кошелька</param>
            /// <param name="amount">Сумма</param>
            /// <param name="id">Идентификатор</param>
            public PaymentDetailProxy(Wallet wallet, string walletName, decimal amount = 0, long id = 0)
            {
                this.Id = id;
                this.WalletId = wallet.Id;
                this.WalletName = walletName;
                this.Balance = wallet.Balance;
                this.WalletGuid = wallet.WalletGuid;
                this.Amount = amount;
            }

            /// <summary>
            /// Идентификатор
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// Идентификатор кошелька
            /// </summary>
            public long WalletId { get; set; }

            /// <summary>
            /// Наименование кошелька
            /// </summary>
            public string WalletName { get; set; }

            /// <summary>
            /// Баланс кошелька
            /// </summary>
            public decimal Balance { get; set; }

            /// <summary>
            /// Сумма
            /// </summary>
            public decimal Amount { get; set; }

            /// <summary>
            /// Guid кошелька
            /// </summary>
            public string WalletGuid { get; set; }
        }

        private IEnumerable<PaymentDetailProxy> GetProxies(RealityObjectPaymentAccount account)
        {
            var proxies = new List<PaymentDetailProxy>
            {
                new PaymentDetailProxy(account.BankPercentWallet, account.BankPercentWallet.GetWalletName(this.Container, account, false)),
                new PaymentDetailProxy(account.BaseTariffPaymentWallet, account.BaseTariffPaymentWallet.GetWalletName(this.Container, account, false)),
                new PaymentDetailProxy(account.DecisionPaymentWallet, account.DecisionPaymentWallet.GetWalletName(this.Container, account, false)),
                new PaymentDetailProxy(account.FundSubsidyWallet, account.FundSubsidyWallet.GetWalletName(this.Container, account, false)),
                new PaymentDetailProxy(account.OtherSourcesWallet, account.OtherSourcesWallet.GetWalletName(this.Container, account, false)),
                new PaymentDetailProxy(account.PenaltyPaymentWallet, account.PenaltyPaymentWallet.GetWalletName(this.Container, account, false)),
                new PaymentDetailProxy(account.RegionalSubsidyWallet, account.RegionalSubsidyWallet.GetWalletName(this.Container, account, false)),
                new PaymentDetailProxy(account.StimulateSubsidyWallet, account.StimulateSubsidyWallet.GetWalletName(this.Container, account, false)),
                new PaymentDetailProxy(account.TargetSubsidyWallet, account.TargetSubsidyWallet.GetWalletName(this.Container, account, false)),
                new PaymentDetailProxy(account.SocialSupportWallet, account.SocialSupportWallet.GetWalletName(this.Container, account, true)),
                new PaymentDetailProxy(account.AccumulatedFundWallet, account.AccumulatedFundWallet.GetWalletName(this.Container, account, false)),
                new PaymentDetailProxy(account.RentWallet, account.RentWallet.GetWalletName(this.Container, account, false)),
                new PaymentDetailProxy(account.RestructAmicableAgreementWallet, account.RestructAmicableAgreementWallet.GetWalletName(this.Container, account, false)),
                new PaymentDetailProxy(account.PreviosWorkPaymentWallet, account.PreviosWorkPaymentWallet.GetWalletName(this.Container, account, false))
            };

            return proxies;
        }
    }
}
