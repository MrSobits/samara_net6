namespace Bars.Gkh.RegOperator.Domain.Repository.RealityObjectAccount
{
	using System;
	using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
	using Bars.Gkh.Contracts.Params;
	using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
	using Bars.Gkh.RegOperator.DomainService.RealityObjectAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Домен-сервис для работы с расчетными счетами
    /// </summary>
    public class RealityObjectPaymentAccountRepository : IRealityObjectPaymentAccountRepository
    {
        private readonly IDomainService<RealityObjectPaymentAccount> AccountDomain;
        private readonly IBankAccountDataProvider BankProvider;
        private readonly IWindsorContainer Container;
        private readonly IDomainService<BasePersonalAccount> PersonalAccDmn;
		private readonly IDomainService<CalcAccountRealityObject> calcAccountRoDomain;

		/// <summary>
		/// Конструктор для инициализации переменных
		/// </summary>
		/// <param name="accountDomain">Домен-сервис счет оплат дома</param>
		/// <param name="personalAccDmn">Домен-сервис лицевых счетов</param>
		/// <param name="bankProvider">Интерфейс для получения данных банка</param>
		/// <param name="container">IOC контейнер</param>
		public RealityObjectPaymentAccountRepository(
            IDomainService<RealityObjectPaymentAccount> accountDomain,
            IDomainService<BasePersonalAccount> personalAccDmn,
            IBankAccountDataProvider bankProvider,
            IWindsorContainer container,
			IDomainService<CalcAccountRealityObject> calcAccountRoDomain)
        {
            this.AccountDomain = accountDomain;
            this.PersonalAccDmn = personalAccDmn;
            this.BankProvider = bankProvider;
            this.Container = container;
            this.calcAccountRoDomain = calcAccountRoDomain;
        }

        /// <summary>
        /// Получение счетов оплаты дома по Жилому дому
        /// </summary>
        /// <param name="realityObject">жилой дом</param>
        /// <returns></returns>
        public RealityObjectPaymentAccount GetByRealtyObject(RealityObject realityObject)
        {
            return this.AccountDomain.GetAll()
                .FetchAllWallets()
                .FirstOrDefault(x => x.RealityObject.Id == realityObject.Id);
        }

        /// <summary>
        /// Получение счетов оплаты дома по Трансферам по получателю денег
        /// </summary>
        /// <param name="transfers">список трансферов</param>
        /// <returns></returns>
        public IQueryable<RealityObjectPaymentAccount> GetTargetAccountsForTransfers(IEnumerable<Transfer> transfers)
        {
            var walletGuids = transfers.Select(x => x.TargetGuid).ToArray();
            return this.AccountDomain.GetAll()
                .Where(x => walletGuids.Contains(x.BankPercentWallet.WalletGuid)
                            || walletGuids.Contains(x.BaseTariffPaymentWallet.WalletGuid)
                            || walletGuids.Contains(x.DecisionPaymentWallet.WalletGuid)
                            || walletGuids.Contains(x.FundSubsidyWallet.WalletGuid)
                            || walletGuids.Contains(x.OtherSourcesWallet.WalletGuid)
                            || walletGuids.Contains(x.PenaltyPaymentWallet.WalletGuid)
                            || walletGuids.Contains(x.RegionalSubsidyWallet.WalletGuid)
                            || walletGuids.Contains(x.StimulateSubsidyWallet.WalletGuid)
                            || walletGuids.Contains(x.TargetSubsidyWallet.WalletGuid));
        }

        /// <summary>
        /// Получение списка номеров банка и адреса дома
        /// </summary>
        /// <param name="@params">параметры клиента</param>
        /// <returns></returns>
        public GenericListResult<object> ListByPersonalAccountOwner(BaseParams @params)
        {
            var ownerId = @params.Params.GetAsId("ownerId");
            var loadParam = @params.GetLoadParam();

			var data = this.AccountDomain.GetAll()
		        .WhereIf(
			        ownerId > 0,
			        x => this.PersonalAccDmn.GetAll()
				        .Where(p => p.AccountOwner.Id == ownerId)
				        .Any(p => p.Room.RealityObject.Id == x.RealityObject.Id))
		        .Select(x => new
			        {
				        x.RealityObject.Address,
				        RoId = x.RealityObject.Id,
				        BankAccountNumber = this.calcAccountRoDomain.GetAll()
							.Where(y => (y.Account.TypeAccount == TypeCalcAccount.Special && ((SpecialCalcAccount)y.Account).IsActive) || y.Account.TypeAccount == TypeCalcAccount.Regoperator)
							.Where(y => y.RealityObject.Id == x.RealityObject.Id)
							.Where(y => y.Account.DateOpen <= DateTime.Today)
							.Where(y => !y.Account.DateClose.HasValue || y.Account.DateClose.Value >= DateTime.Today)
							.Where(y => (y.Account.TypeAccount == TypeCalcAccount.Special)
										|| (y.Account.TypeAccount == TypeCalcAccount.Regoperator && y.DateStart <= DateTime.Today && (!y.DateEnd.HasValue || y.DateEnd >= DateTime.Today)))
							.OrderByDescending(y => y.Account.DateOpen)
							.Select(y => y.Account.AccountNumber ?? ((RegopCalcAccount)y.Account).ContragentCreditOrg.SettlementAccount)
							.FirstOrDefault()
				})
				.Where(x => x.BankAccountNumber != null && x.BankAccountNumber != "")
                .Filter(loadParam, this.Container)
                .Order(loadParam);

            var count = data.Count();

            return new GenericListResult<object>(data.Paging(loadParam), count);
        }

        /// <summary>
        /// Получение списка счетов оплаты домов по списку домов
        /// </summary>
        /// <param name="realityObjects">список домов</param>
        /// <returns></returns>
        public IQueryable<RealityObjectPaymentAccount> GetByRealtyObjects(IEnumerable<RealityObject> realityObjects)
        {
            var ids = realityObjects.Select(x => x.Id);
            return this.AccountDomain.GetAll().Where(x => ids.Contains(x.RealityObject.Id));
        }

        /// <summary>
        /// Получение счетов оплаты дома по Трансферам по источнику денег
        /// </summary>
        /// <param name="transfers">список трансферов</param>
        /// <returns></returns>
        public IQueryable<RealityObjectPaymentAccount> GetSourceAccountsForTransfers(IEnumerable<Transfer> transfers)
        {
            var walletGuids = transfers.Select(x => x.SourceGuid).ToArray();
            return this.AccountDomain.GetAll()
                .Where(x => walletGuids.Contains(x.BankPercentWallet.WalletGuid)
                            || walletGuids.Contains(x.BaseTariffPaymentWallet.WalletGuid)
                            || walletGuids.Contains(x.DecisionPaymentWallet.WalletGuid)
                            || walletGuids.Contains(x.FundSubsidyWallet.WalletGuid)
                            || walletGuids.Contains(x.OtherSourcesWallet.WalletGuid)
                            || walletGuids.Contains(x.PenaltyPaymentWallet.WalletGuid)
                            || walletGuids.Contains(x.RegionalSubsidyWallet.WalletGuid)
                            || walletGuids.Contains(x.StimulateSubsidyWallet.WalletGuid)
                            || walletGuids.Contains(x.TargetSubsidyWallet.WalletGuid));
        }

        /// <summary>
        /// Сохранение/обновление сущности Счет оплат дома
        /// </summary>
        /// <param name="account">счет оплат дома</param>
        public void SaveOrUpdate(RealityObjectPaymentAccount account)
        {
            if (account.Id > 0)
            {
                this.AccountDomain.Update(account);
            }
            else
            {
                this.AccountDomain.Save(account);
            }
        }
    }
}