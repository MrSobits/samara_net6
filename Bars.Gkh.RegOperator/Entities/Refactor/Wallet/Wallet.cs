namespace Bars.Gkh.RegOperator.Entities.Wallet
{
    using B4.Utils;
    using B4.Utils.Annotations;
    using Bars.B4.Application;
    using Bars.B4.Config;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Extenstions;
    using Castle.Windsor;
    using Exceptions;
    using Newtonsoft.Json;
    using Refactor;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.ServiceModel;

    /// <summary>
    /// Кошелек оплат. Содержит информацию по оплатам
    /// </summary>
    public class Wallet : BaseImportableEntity, IWallet
    {
        protected IList<RealityObjectTransfer> realityObjectInTransfers, realityObjectOutTransfers;
        protected IList<PersonalAccountPaymentTransfer> accountPaymenInTransfers, accountPaymenOutTransfers;
        protected IList<PersonalAccountChargeTransfer> accountChargeInTransfers, accountChargeOutTransfers;
        private IList<MoneyLock> moneyLocks;
        private Dictionary<long, decimal> balanceByPeriod;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="guid">Гуид</param>
        /// <param name="owner">Владелец</param>
        /// <param name="type">Тип кошелька</param>
        public Wallet(string guid, ITransferOwner owner, WalletType type) : this()
        {
            ArgumentChecker.NotNullOrEmpty(guid, nameof(guid));
            this.WalletGuid = guid;
            this.OwnerType = owner.TransferOwnerType;
            this.WalletType = type;
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <remarks>Конструктор должен быть публичным, иначе импорт абонентов не будет работать. Проблема кроется в инициализаторе кэша Redis, 
        /// в CustomPersistentInvalidationStrategy требуется публичный конструктор без параметров</remarks>
        public Wallet()
        {
            this.realityObjectInTransfers = new List<RealityObjectTransfer>();
            this.realityObjectOutTransfers = new List<RealityObjectTransfer>();
            this.accountPaymenInTransfers = new List<PersonalAccountPaymentTransfer>();
            this.accountPaymenOutTransfers = new List<PersonalAccountPaymentTransfer>();
            this.accountChargeInTransfers = new List<PersonalAccountChargeTransfer>();
            this.accountChargeOutTransfers = new List<PersonalAccountChargeTransfer>();

            this.moneyLocks = new List<MoneyLock>();
            this.balanceByPeriod = new Dictionary<long, decimal>();

            this.InTransfersDict = new Dictionary<ChargePeriod, Transfer[]>();
            this.OutTransfersDict = new Dictionary<ChargePeriod, Transfer[]>();
        }

        /// <summary>
        /// Уникальный гуид кошелька
        /// </summary>
        public virtual string WalletGuid { get; protected set; }

        /// <summary>
        /// Баланс кошелька
        /// </summary>
        public virtual decimal Balance { get; protected set; }

        /// <summary>
        /// Признак перераспределения оплат внутри кошелька
        /// </summary>
        public virtual bool Repayment { get; set; }

        /// <summary>
        /// Вспомогательное свойство для фильтрации по новым операциям
        /// </summary>
        public virtual bool HasNewOperations { get; protected set; }

        /// <summary>
        /// Тип владельца
        /// </summary>
        public virtual WalletOwnerType OwnerType { get; set; }

        /// <summary>
        /// Тип кошелька
        /// </summary>
        public virtual WalletType WalletType { get; set; }

        /// <summary>
        /// Операции оплат по кошельку дома
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual IEnumerable<RealityObjectTransfer> RealityObjectInTransfers => this.realityObjectInTransfers ?? (this.realityObjectInTransfers = new List<RealityObjectTransfer>());

        /// <summary>
        /// Операции взятия денег по кошельку дома
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual IEnumerable<RealityObjectTransfer> RealityObjectOutTransfers => this.realityObjectOutTransfers ?? (this.realityObjectOutTransfers = new List<RealityObjectTransfer>());

        /// <summary>
        /// Операции оплат по кошельку оплат ЛС
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual IEnumerable<PersonalAccountPaymentTransfer> AccountPaymenInTransfers => this.accountPaymenInTransfers ?? (this.accountPaymenInTransfers = new List<PersonalAccountPaymentTransfer>());

        /// <summary>
        /// Операции взятия денег по кошельку оплат ЛС
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual IEnumerable<PersonalAccountPaymentTransfer> AccountPaymenOutTransfers => this.accountPaymenOutTransfers ?? (this.accountPaymenOutTransfers = new List<PersonalAccountPaymentTransfer>());

        /// <summary>
        /// Операции оплат по кошельку начислений ЛС
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual IEnumerable<PersonalAccountChargeTransfer> AccountChargeInTransfers => this.accountChargeInTransfers ?? (this.accountChargeInTransfers = new List<PersonalAccountChargeTransfer>());

        /// <summary>
        /// Операции взятия денег по кошельку начислений ЛС
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual IEnumerable<PersonalAccountChargeTransfer> AccountChargeOutTransfers => this.accountChargeOutTransfers ?? (this.accountChargeOutTransfers = new List<PersonalAccountChargeTransfer>());

        /// <summary>
        /// Входящие трансферы по типу владельца
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual IEnumerable<Transfer> InTransfers
        {
            get
            {
                switch (this.OwnerType)
                {
                    case WalletOwnerType.BasePersonalAccount:
                        return this.AccountPaymenInTransfers.Cast<Transfer>().Union(this.AccountChargeInTransfers);

                    case WalletOwnerType.RealityObjectPaymentAccount:
                        return this.RealityObjectInTransfers;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Исходящие трансферы по типу владельца
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual IEnumerable<Transfer> OutTransfers
        {
            get
            {
                switch (this.OwnerType)
                {
                    case WalletOwnerType.BasePersonalAccount:
                        return this.AccountPaymenOutTransfers.Cast<Transfer>().Union(this.AccountChargeOutTransfers);

                    case WalletOwnerType.RealityObjectPaymentAccount:
                        return this.RealityObjectOutTransfers;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Блокировки денег
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual IEnumerable<MoneyLock> MoneyLocks => this.moneyLocks ?? (this.moneyLocks = new List<MoneyLock>());

        /// <summary>
        /// Активные блокировки
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual IEnumerable<MoneyLock> ActiveMoneyLocks
        {
            get { return this.moneyLocks != null ? this.moneyLocks.Where(x => x.IsActive) : (this.moneyLocks = new List<MoneyLock>()); }
        } 

        /// <summary>
        /// Гуид для трансфера
        /// </summary>
        public virtual string TransferGuid => this.WalletGuid;

        /// <summary>
        /// Список закэшированный входящих трансферов по периодам
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual IDictionary<ChargePeriod, Transfer[]> InTransfersDict { get; protected set; }

        /// <summary>
        /// Список закэшированный исходящих трансферов по периодам
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual IDictionary<ChargePeriod, Transfer[]> OutTransfersDict { get; protected set; }

        #region Public Methods
        /// <summary>
        /// Зачислить средства на кошелек
        /// </summary>
        /// <param name="tranferBuilder">Построитель</param>
        /// <returns>Созданные трансферы</returns>
        public virtual Transfer StoreMoney(TransferBuilder tranferBuilder)
        {
            /*
              Внимание! Метод может принимать отрицательные сумма. Не сочтите за ошибку
            */
            var howMuch = tranferBuilder.MoneyStream;
            var amount = howMuch.Amount;
            if (amount == 0)
            {
                return null;
            }

            this.HasNewOperations = true;

            if (howMuch.IsAffect)
            {
                this.Balance += howMuch.Amount;
            }

            return tranferBuilder.SetTargetGuid(this).Build();
        }

        /// <summary>
        /// Метод взятия средств с кошелька
        /// </summary>
        /// <param name="tranferBuilder">Построитель</param>
        /// <returns>Созданные трансферы</returns>
        public virtual Transfer TakeMoney(TransferBuilder tranferBuilder)
        {
            /*
             Внимание! Метод может принимать отрицательные сумма. Не сочтите за ошибку
             */
            bool allowNegativeOp = false;

            try
            {
                var cp = ApplicationContext.Current.Container.Resolve<IConfigProvider>();
                var config = cp.GetConfig().GetModuleConfig("Bars.Regoperator");
                allowNegativeOp = config.GetAs<bool>("AllowTakeMoneyFromNegativeWallet");
            
            }
            catch (Exception e)
            {

            }


            var howMuch = tranferBuilder.MoneyStream;
            var amount = howMuch.Amount;
            if (amount == 0)
            {
                return null;
            }

            if (howMuch.IsAffect && !this.Repayment && !allowNegativeOp)
            {
                if (amount > this.Balance)
                {
                    throw new WalletBalanceException(
                        tranferBuilder.Owner,
                        this,
                        $" Недостаточно средств. Счёт '{tranferBuilder.Owner.GetDescription()}', Баланс кошелька = {this.Balance} рублей.");
                }
            }

            this.HasNewOperations = true;

            if (howMuch.IsAffect)
            {
                this.Balance -= howMuch.Amount;
            }

            return tranferBuilder.SetSourceGuid(this).Build();
        }

        /// <summary>
        /// Метод создает трансфер переноса средств на другой кошелёк (не влияет на баланс)
        /// </summary>
        /// <param name="tranferBuilder">Построитель</param>
        /// <returns>Созданные трансферы</returns>
        public virtual Transfer MoveToAnotherWallet(TransferBuilder tranferBuilder)
        {
            tranferBuilder.MoneyStream.IsAffect = false;
            var transfer = this.TakeMoney(tranferBuilder);

            if (transfer.IsNotNull())
            {
                transfer.IsInDirect = true;
            }

            return transfer;
        }

        /// <summary>
        /// Метод создает трансфер переноса средств на на кошелек другого ЛС
        /// </summary>
        /// <param name="source">Построитель источника средств</param>
        /// <param name="target">Построитель цели средств</param>
        /// <param name="targetWallet">Целевой кошелек</param>
        /// <returns>Созданные трансферы</returns>
        public virtual IList<Transfer> MoveToAnotherAccount(TransferBuilder source, TransferBuilder target, IWallet targetWallet)
        {
            var result = new List<Transfer>();

            var takeBuilder = source.SetSourceGuid(this);
            var enrollBuilder = target.SetTargetGuid(targetWallet);

            takeBuilder.Build().AddTo(result);
            enrollBuilder.Build().AddTo(result);

            return result;
        }

        /// <summary>
        /// Блокирование денег на кошельке
        /// </summary>
        /// <param name="operation">Операция</param>
        /// <param name="amount">Сумма</param>
        /// <param name="targetGuid">Целевой Guid</param>
        /// <returns></returns>
        public virtual MoneyLock LockMoney(
            MoneyOperation operation,
            decimal amount, 
            string targetGuid)
        {
            ArgumentChecker.NotNull(operation, nameof(operation));
            ArgumentChecker.NotNull(targetGuid, nameof(targetGuid));

            if (amount > this.Balance)
            {
                throw new MoneyLockException($" Недостаточно средств.Остаток на счёте {this.Balance} рублей. ID кошелька = '{this.Id}' ");
            }

            var moneyLock = new MoneyLock(operation, this, amount, targetGuid);

            this.moneyLocks.Add(moneyLock);

            this.Balance -= amount;

            return moneyLock;
        }

        /// <summary>
        /// Операция снятия блокирования денег
        /// </summary>
        /// <param name="moneyLock">Блокировка денег</param>
        /// <param name="operation">Операция</param>
        public virtual void UnlockMoney(MoneyLock moneyLock, MoneyOperation operation)
        {
            ArgumentChecker.NotNull(moneyLock, nameof(moneyLock));
            if (!this.Equals(moneyLock.Wallet))
            {
                throw new MoneyUnlockException("Can not unlock money from other wallet");
            }
            if (!moneyLock.IsActive)
            {
                throw new MoneyUnlockException("Can not unlock non-active lock");
            }

            if (!this.moneyLocks.Contains(moneyLock))
            {
                return;
            }

            this.Balance += moneyLock.Amount;

            moneyLock.Cancel(operation);
        }

        /// <summary>
        /// Подсчёт баланса кошелька в периоде
        /// </summary>
        /// <param name="period">Период</param>
        /// <returns></returns>
        public virtual decimal GetPeriodBalance(ChargePeriod period)
        {
            if (this.InTransfers.IsEmpty() && this.OutTransfers.IsEmpty())
            {
                return 0m;
            }

            decimal periodBalance;

            if (this.balanceByPeriod.TryGetValue(period.Id, out periodBalance))
            {
                return periodBalance;
            }

            var start = period.StartDate;
            var end = period.EndDate.GetValueOrDefault(DateTime.MaxValue);

            periodBalance = this.InTransfers.Where(x => x.ChargePeriod.Id == period.Id).Sum(x => x.Amount)
                - this.OutTransfers.Where(x => x.ChargePeriod.Id == period.Id).Sum(x => x.Amount)
                - this.MoneyLocks.Where(x => x.LockDate.Date >= start && x.LockDate < end && x.UnlockDate.Date >= end).Sum(x => x.Amount);

            this.balanceByPeriod[period.Id] = periodBalance;

            return periodBalance;
        }

        /// <summary>
        /// Метод возвращает входящие трансферы за период
        /// </summary>
        /// <param name="period">
        /// Период
        /// </param>
        /// <returns>
        /// Трансферы
        /// </returns>
        public virtual IEnumerable<Transfer> GetInTransfers(ChargePeriod period)
        {
            return this.InTransfersDict.Get(period) ?? this.InTransfers.Where(x => x.ChargePeriod.Id == period.Id);
        }

        /// <summary>
        /// Метод возвращает исходящие трансферы за период
        /// </summary>
        /// <param name="period">
        /// Период
        /// </param>
        /// <returns>
        /// Трансферы
        /// </returns>
        public virtual IEnumerable<Transfer> GetOutTransfers(ChargePeriod period)
        {
            return this.OutTransfersDict.Get(period) ?? this.OutTransfers.Where(x => x.ChargePeriod.Id == period.Id);
        }
        #endregion

        /// <summary>
        /// Определяет, равен ли заданный объект текущему объекту.
        /// </summary>
        /// <param name="obj">
        /// Объект, который требуется сравнить с текущим объектом. 
        /// </param>
        /// <returns>Значение true, если указанный объект равен текущему объекту; в противном случае — значение false.</returns>
        public override bool Equals(object obj)
        {
            var that = obj as Wallet;

            if (that == null)
            {
                return false;
            }

            if (object.ReferenceEquals(that, this))
            {
                return true;
            }

            return this.Id == that.Id && this.WalletGuid.Equals(that.WalletGuid);
        }
    }
}