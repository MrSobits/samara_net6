namespace Bars.Gkh.RegOperator.Entities
{
    using System;

    using Bars.Gkh.Domain.DatabaseMutex;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    using Domain;
    using Enums;
    using Gkh.Entities;
    using Newtonsoft.Json;

    /// <summary>
    /// Счет оплат дома
    /// </summary>
    public partial class RealityObjectPaymentAccount : TransferOwner, IRealityObjectAccount, IHaveMutexName
    {
        #region Privates

        private Wallet.Wallet _btWallet;

        private Wallet.Wallet _dWallet;

        private Wallet.Wallet _pWallet;

        private Wallet.Wallet _rWallet;

        private Wallet.Wallet _ssWallet;

        private Wallet.Wallet _pwpWallet;

        private Wallet.Wallet _afWallet;

        private Wallet.Wallet _raaWallet;

        private Wallet.Wallet _tsuWallet;

        private Wallet.Wallet _fsuWallet;

        private Wallet.Wallet _rsuWallet;

        private Wallet.Wallet _ssuWallet;

        private Wallet.Wallet _osWallet;

        private Wallet.Wallet _bpWallet;
        #endregion

        public RealityObjectPaymentAccount()
        {
        }

        public RealityObjectPaymentAccount(RealityObject realty)
        {
            this.RealityObject = realty;
        }

        #region Persisted properties

        /// <summary>
        /// Номер счета
        /// </summary>
        public virtual string AccountNumber { get; set; }

        /// <summary>
        /// Дата открытия счета
        /// </summary>
        public virtual DateTime DateOpen { get; set; }

        /// <summary>
        /// Дата закрытия счета
        /// </summary>
        public virtual DateTime? DateClose { get; set; }

        /// <summary>
        /// Объект недвижимости
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Тип счета
        /// </summary>
        public virtual RealityObjectPaymentAccountType AccountType { get; set; }

        /// <summary>
        /// итого по дебету
        /// </summary>
        public virtual decimal DebtTotal { get; set; }

        /// <summary>
        /// Итого по кредиту
        /// </summary>
        public virtual decimal CreditTotal { get; set; }

        /// <summary>
        /// Заблокировано денег
        /// </summary>
        public virtual decimal MoneyLocked { get; set; }

        /// <summary>
        /// Сумма непогашенных займов
        /// </summary>
        public virtual decimal Loan { get; set; }

        /// <summary>
        /// Сумма всех площадей ЛС в доме
        /// </summary>
        public virtual decimal? RealArea { get; set; }

        #region Wallets

        /// <summary>
        /// Кошелек оплат по базовому тарифу
        /// </summary>
        [JsonIgnore]
        public virtual Wallet.Wallet BaseTariffPaymentWallet
        {
            get { return this._btWallet ?? (this._btWallet = new Wallet.Wallet(Guid.NewGuid().ToString(), this, WalletType.BaseTariffWallet)); }
            protected set { this._btWallet = value; }
        }

        /// <summary>
        /// Кошелек оплат по тарифу решения
        /// </summary>
        [JsonIgnore]
        public virtual Wallet.Wallet DecisionPaymentWallet
        {
            get { return this._dWallet ?? (this._dWallet = new Wallet.Wallet(Guid.NewGuid().ToString(), this, WalletType.DecisionTariffWallet)); }
            protected set { this._dWallet = value; }
        }

        /// <summary>
        /// Кошелек оплат по аренде
        /// </summary>
        [JsonIgnore]
        public virtual Wallet.Wallet RentWallet
        {
            get { return this._rWallet ?? (this._rWallet = new Wallet.Wallet(Guid.NewGuid().ToString(), this, WalletType.RentWallet)); }
            protected set { this._rWallet = value; }
        }

        /// <summary>
        /// Кошелек оплат по пени
        /// </summary>
        [JsonIgnore]
        public virtual Wallet.Wallet PenaltyPaymentWallet
        {
            get { return this._pWallet ?? (this._pWallet = new Wallet.Wallet(Guid.NewGuid().ToString(), this, WalletType.PenaltyWallet)); }
            protected set { this._pWallet = value; }
        }

        /// <summary>
        /// Кошелек оплат по соц поддержке
        /// </summary>
        [JsonIgnore]
        public virtual Wallet.Wallet SocialSupportWallet
        {
            get { return this._ssWallet ?? (this._ssWallet = new Wallet.Wallet(Guid.NewGuid().ToString(), this, WalletType.SocialSupportWallet)); }
            protected set { this._ssWallet = value; }
        }

        /// <summary>
        /// Кошелек оплат за выполненные работы
        /// </summary>
        [JsonIgnore]
        public virtual Wallet.Wallet PreviosWorkPaymentWallet
        {
            get { return this._pwpWallet ?? (this._pwpWallet = new Wallet.Wallet(Guid.NewGuid().ToString(), this, WalletType.PreviosWorkPaymentWallet)); }
            protected set { this._pwpWallet = value; }
        }

        /// <summary>
        /// Кошелек по ранее накопленным средствам
        /// </summary>
        [JsonIgnore]
        public virtual Wallet.Wallet AccumulatedFundWallet
        {
            get { return this._afWallet ?? (this._afWallet = new Wallet.Wallet(Guid.NewGuid().ToString(), this, WalletType.AccumulatedFundWallet)); }
            protected set { this._afWallet = value; }
        }

        /// <summary>
        /// Кошелек оплат по мировому соглашению
        /// </summary>
        [JsonIgnore]
        public virtual Wallet.Wallet RestructAmicableAgreementWallet
        {
            get { return this._raaWallet ?? (this._raaWallet = new Wallet.Wallet(Guid.NewGuid().ToString(), this, WalletType.RestructAmicableAgreementWallet)); }
            protected set { this._raaWallet = value; }
        }

        #endregion

        #region Subsidy wallets

        /// <summary>
        /// Кошелек целевых субсидий
        /// </summary>
        [JsonIgnore]
        public virtual Wallet.Wallet TargetSubsidyWallet
        {
            get { return this._tsuWallet ?? (this._tsuWallet = new Wallet.Wallet(Guid.NewGuid().ToString(), this, WalletType.TargetSubsidyWallet)); }
            protected set { this._tsuWallet = value; }
        }

        /// <summary>
        /// Кошелек субсидий фонда
        /// </summary>
        [JsonIgnore]
        public virtual Wallet.Wallet FundSubsidyWallet
        {
            get { return this._fsuWallet ?? (this._fsuWallet = new Wallet.Wallet(Guid.NewGuid().ToString(), this, WalletType.FundSubsidyWallet)); }
            protected set { this._fsuWallet = value; }
        }

        /// <summary>
        /// Кошелек региональных субсидий
        /// </summary>
        [JsonIgnore]
        public virtual Wallet.Wallet RegionalSubsidyWallet
        {
            get { return this._rsuWallet ?? (this._rsuWallet = new Wallet.Wallet(Guid.NewGuid().ToString(), this, WalletType.RegionalSubsidyWallet)); }
            protected set { this._rsuWallet = value; }
        }

        /// <summary>
        /// Кошелек стимулирующей субсидий
        /// </summary>
        [JsonIgnore]
        public virtual Wallet.Wallet StimulateSubsidyWallet
        {
            get { return this._ssuWallet ?? (this._ssuWallet = new Wallet.Wallet(Guid.NewGuid().ToString(), this, WalletType.StimulateSubsidyWallet)); }
            protected set { this._ssuWallet = value; }
        }

        /// <summary>
        /// Кошелек иных поступлений
        /// </summary>
        [JsonIgnore]
        public virtual Wallet.Wallet OtherSourcesWallet
        {
            get { return this._osWallet ?? (this._osWallet = new Wallet.Wallet(Guid.NewGuid().ToString(), this, WalletType.OtherSourcesWallet)); }
            protected set { this._osWallet = value; }
        }

        /// <summary>
        /// Кошелек процентов банка
        /// </summary>
        [JsonIgnore]
        public virtual Wallet.Wallet BankPercentWallet
        {
            get { return this._bpWallet ?? (this._bpWallet = new Wallet.Wallet(Guid.NewGuid().ToString(), this, WalletType.BankPercentWallet)); }
            protected set { this._bpWallet = value; }
        }
        #endregion
        #endregion persisted properties

        /// <inheritdoc />
        protected override Transfer CreateTransferInternal(string sourceGuid, string targetGuid, MoneyStream moneyStream)
        {
            return new RealityObjectTransfer(this, sourceGuid, targetGuid, moneyStream.Amount, moneyStream.Operation);
        }

        /// <inheritdoc />
        public override WalletOwnerType TransferOwnerType => WalletOwnerType.RealityObjectPaymentAccount;

        /// <inheritdoc />
        public override string GetDescription()
        {
            return this.RealityObject.Address;
        }
    }
}