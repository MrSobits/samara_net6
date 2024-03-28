namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Domain.ParameterVersioning;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Enums;
  

    using Newtonsoft.Json;

    /// <summary>
    /// Лицевой счет
    /// </summary>
    /// TODO: protected setters for invariant properties
    public partial class BasePersonalAccount : TransferOwner, IStatefulEntity, IHasDateActualChange
    {
        /// <summary>
        /// Открыт
        /// </summary>
        public const string StateOpenedCode = "1";

        /// <summary>
        /// Закрыт
        /// </summary>
        public const string StateCloseCode = "2";

        /// <summary>
        /// Закрыт с долгом
        /// </summary>
        public const string StateCloseDebtCode = "3";

        /// <summary>
        /// Не активен
        /// </summary>
        public const string StateNonActiveCode = "4";


        private IList<PersonalAccountCharge> _charges;
        private Wallet.Wallet _btWallet;
        private Wallet.Wallet _dWallet;
        private Wallet.Wallet _pWallet;
        private Wallet.Wallet _rWallet;
        private Wallet.Wallet _ssWallet;
        private Wallet.Wallet _pwpWallet;
        private Wallet.Wallet _afWallet;
        private Wallet.Wallet _raaWallet;
        private IList<PersonalAccountPeriodSummary> _summaries;

        public BasePersonalAccount()
        {
            _charges = new List<PersonalAccountCharge>();
            _summaries = new List<PersonalAccountPeriodSummary>();
            ServiceType = PersAccServiceType.NotSelected;
        }

        #region Persisted properties

        /// <summary>
        /// ГИС ЖКХ GUID
        /// </summary>
        public virtual string GisGkhGuid { get; set; }

        /// <summary>
        /// ГИС ЖКХ TransportGUID
        /// </summary>
        public virtual string GisGkhTransportGuid { get; set; }

        /// <summary>
        /// Единый ЛС
        /// </summary>
        public virtual string UnifiedAccountNumber { get; set; }


        /// <summary>
        /// Идентификатор жилищно-коммунальной услуги
        /// </summary>
        public virtual string ServiceId { get; set; }

        /// <summary>
        /// Помещение
        /// </summary>
        public virtual Room Room { get; set; }

        /// <summary>
        /// Абонент
        /// </summary>
        public virtual PersonalAccountOwner AccountOwner { get; set; }

        /// <summary>
        /// Целочисленный номер для генерации номера ркц
        /// </summary>
        public virtual int IntNumber { get; set; }

        /// <summary>
        /// Номер Л/С
        /// </summary>
        public virtual string PersonalAccountNum { get; set; }

        /// <summary>
        /// Номер Л/С во внешних системах
        /// </summary>
        public virtual string PersAccNumExternalSystems { get; set; }

        /// <summary>
        /// Доля собственности
        /// </summary>
        public virtual decimal AreaShare { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Дата открытия ЛС
        /// </summary>
        public virtual DateTime OpenDate { get; set; }

        /// <summary>
        /// Дата закрытия ЛС
        /// <remarks></remarks>
        /// </summary>
        public virtual DateTime CloseDate { get; protected set; }

        /// <summary>
        /// Удалить после прочтения
        /// </summary>
        public virtual decimal Tariff { get; set; }

        /// <summary>
        /// Номер заключения договора
        /// </summary>
        public virtual string ContractNumber { get; set; }

        /// <summary>
        /// Дата заключения договора
        /// </summary>
        public virtual DateTime ContractDate { get; set; }

        /// <summary>
        /// Дата отправки договора
        /// </summary>
        public virtual DateTime ContractSendDate { get; set; }

        /// <summary>
        /// Документ договора
        /// </summary>
        public virtual FileInfo ContractDocument { get; set; }

        /// <summary>
        /// Тип документа о праве собственности
        /// </summary>
        public virtual string OwnershipDocumentType { get; set; }

        /// <summary>
        /// Тип собственности для разделения ЛС
        /// </summary>
        public virtual RoomOwnershipType OwnershipTypeNewLs { get; set; }

        /// <summary>
        /// Тип услуги
        /// </summary>
        public virtual PersAccServiceType ServiceType { get; set; }

        /// <summary>
        /// Код/номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Дата оформления документа
        /// </summary>
        public virtual DateTime DocumentRegistrationDate { get; set; }

        DateTime IHasDateActualChange.ActualChangeDate
        {
            get { return OpenDate; }
        }

        /// <summary>
        /// Площадь, относящаяся к лс
        /// </summary>
        public virtual decimal Area { get; set; }

        /// <summary>
        /// Жилая площадь, относящаяся к лс
        /// </summary>
        public virtual decimal LivingArea { get; set; }

        /// <summary>
        /// Признак электронная квитанция
        /// </summary>
        public virtual YesNo DigitalReceipt { get; set; }

        /// <summary>
        /// Начисления по ЛС
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual IEnumerable<PersonalAccountCharge> Charges
        {
            get { return _charges ?? (_charges = new List<PersonalAccountCharge>()); }
        }

        /// <summary>
        /// Информация по периодам по ЛС
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual IEnumerable<PersonalAccountPeriodSummary> Summaries
        {
            get { return _summaries ?? (_summaries = new List<PersonalAccountPeriodSummary>()); }
        }

        #region Wallets

        /// <summary>
        /// Кошелек оплат по базовому тарифу
        /// <para>Если нет кошелька, создается новый</para>
        /// </summary>
        [JsonIgnore]
        // [IgnoreDataMember]
        public virtual Wallet.Wallet BaseTariffWallet
        {
            get { return _btWallet ?? (_btWallet = new Wallet.Wallet(Guid.NewGuid().ToString(), this, WalletType.BaseTariffWallet)); }
            protected set { _btWallet = value; }
        }

        /// <summary>
        /// Кошелек оплат по тарифу решения
        /// </summary>
        [JsonIgnore]
        // [IgnoreDataMember]
        public virtual Wallet.Wallet DecisionTariffWallet
        {
            get { return _dWallet ?? (_dWallet = new Wallet.Wallet(Guid.NewGuid().ToString(), this, WalletType.DecisionTariffWallet)); }
            protected set { _dWallet = value; }
        }

        /// <summary>
        /// Кошелек оплат по аренде
        /// </summary>
        [JsonIgnore]
        //  [IgnoreDataMember]
        public virtual Wallet.Wallet RentWallet
        {
            get { return _pWallet ?? (_pWallet = new Wallet.Wallet(Guid.NewGuid().ToString(), this, WalletType.RentWallet)); }
            protected set { _pWallet = value; }
        }

        /// <summary>
        /// Кошелек оплат по пени
        /// </summary>
        [JsonIgnore]
        // [IgnoreDataMember]
        public virtual Wallet.Wallet PenaltyWallet
        {
            get { return _rWallet ?? (_rWallet = new Wallet.Wallet(Guid.NewGuid().ToString(), this, WalletType.PenaltyWallet)); }
            protected set { _rWallet = value; }
        }

        /// <summary>
        /// Кошелек оплат по соц поддержке
        /// </summary>
        [JsonIgnore]
        // [IgnoreDataMember]
        public virtual Wallet.Wallet SocialSupportWallet
        {
            get { return _ssWallet ?? (_ssWallet = new Wallet.Wallet(Guid.NewGuid().ToString(), this, WalletType.SocialSupportWallet)); }
            protected set { _ssWallet = value; }
        }

        /// <summary>
        /// Кошелек оплат за выполненные работы
        /// </summary>
        [JsonIgnore]
        //  [IgnoreDataMember]
        public virtual Wallet.Wallet PreviosWorkPaymentWallet
        {
            get { return _pwpWallet ?? (_pwpWallet = new Wallet.Wallet(Guid.NewGuid().ToString(), this, WalletType.PreviosWorkPaymentWallet)); }
            protected set { _pwpWallet = value; }
        }

        /// <summary>
        /// Кошелек по ранее накопленным средствам
        /// </summary>
        [JsonIgnore]
        //   [IgnoreDataMember]
        public virtual Wallet.Wallet AccumulatedFundWallet
        {
            get { return _afWallet ?? (_afWallet = new Wallet.Wallet(Guid.NewGuid().ToString(), this, WalletType.AccumulatedFundWallet)); }
            protected set { _afWallet = value; }
        }

        /// <summary>
        /// Кошелек оплат по мирововму соглашению
        /// </summary>
        [JsonIgnore]
        //  [IgnoreDataMember]
        public virtual Wallet.Wallet RestructAmicableAgreementWallet
        {
            get { return _raaWallet ?? (_raaWallet = new Wallet.Wallet(Guid.NewGuid().ToString(), this, WalletType.RestructAmicableAgreementWallet)); }
            protected set { _raaWallet = value; }
        }

        #endregion

        #region Money

        /// <summary>
        /// Текущая задолженность по базовому тарифу
        /// </summary>
        public virtual decimal TariffChargeBalance { get; protected set; }

        /// <summary>
        /// Текущая задолженность по тарифу решения
        /// </summary>
        public virtual decimal DecisionChargeBalance { get; protected set; }

        /// <summary>
        /// Текущая задолжнность по пеням
        /// </summary>
        public virtual decimal PenaltyChargeBalance { get; protected set; }

        #endregion Money

        #endregion Persisted properties

        /// <summary>
        /// Информация по ЛС в текущем периоде, не хранимое!
        /// <remarks>Устанавливается через метод</remarks>
        /// </summary>
        public virtual PersonalAccountPeriodSummary OpenedPeriodSummary { get; protected set; }

        /// <summary>
        /// Признак, по которому ЛС не попадет в реестр должников
        /// </summary>
        public virtual bool IsNotDebtor { get; set; }
        /// <summary>
        ///Признак рассрочки на ЛС 
        /// </summary>
        public virtual bool InstallmentPlan { get; set; }
    }
}