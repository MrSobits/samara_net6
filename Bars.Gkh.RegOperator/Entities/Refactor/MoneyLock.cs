namespace Bars.Gkh.RegOperator.Entities.Refactor
{
    using System;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Exceptions;

    /// <summary>
    /// Блокирование денег
    /// </summary>
    public class MoneyLock : BaseImportableEntity
    {
        protected MoneyLock()
        {
            this.LockGuid = Guid.NewGuid().ToString();
        }

        public MoneyLock(MoneyOperation operation, Wallet.Wallet wallet, decimal amount, string targetGuid) : this()
        {
            ArgumentChecker.NotNull(operation, "operation");
            ArgumentChecker.NotNull(wallet, "wallet");
            if (amount <= 0)
            {
                throw new MoneyLockException(string.Format("Can not create lock with Amount = {0}", amount));
            }

            this.LockDate = DateTime.Now;
            this.Operation = operation;
            this.Wallet = wallet;
            this.Amount = amount;
            this.IsActive = true;
            this.TargetGuid = targetGuid;
        }

        /// <summary>
        /// Операция, в рамках которой была произведена блокировка
        /// </summary>
        public virtual MoneyOperation Operation { get; protected set; }

        /// <summary>
        /// Операция, в рамках которой была снята блокировка
        /// </summary>
        public virtual MoneyOperation CancelOperation { get; protected set; }

        /// <summary>
        /// Времы создания блокировки
        /// </summary>
        public virtual DateTime LockDate { get; protected set; }

        /// <summary>
        /// Время снятия блокировки
        /// </summary>
        public virtual DateTime UnlockDate { get; protected set; }

        /// <summary>
        /// Количество заблокированных денег
        /// </summary>
        public virtual decimal Amount { get; protected set; }

        /// <summary>
        /// Уникальный guid блокировки
        /// </summary>
        public virtual string LockGuid { get; protected set; }

        /// <summary>
        /// Кошелек, на котором блокируются деньги
        /// </summary>
        public virtual Wallet.Wallet Wallet { get; protected set; }

        /// <summary>
        /// Получатель денег
        /// </summary>
        public virtual string TargetGuid { get; protected set; }

        /// <summary>
        /// Блокировка снята
        /// </summary>
        public virtual bool IsActive { get; protected set; }

        /// <summary>
        /// Наименование источника
        /// </summary>
        public virtual string SourceName { get; set; }

        /// <summary>
        /// Создание реального трансфера для перевода заблокированных денег
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        [Obsolete("Не реализовано")]
        public virtual Transfer ToTransfer(MoneyOperation operation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Снятие блокировки
        /// </summary>
        public virtual void Cancel(MoneyOperation operation)
        {
            this.CancelOperation = operation;
            this.UnlockDate = DateTime.Now;
            this.IsActive = false;
        }
        
        public override bool Equals(object obj)
        {
            var that = obj as MoneyLock;

            if (that == null) return false;
            if (object.ReferenceEquals(this, that)) return true;
            return this.Id.Equals(that.Id) &&
                   this.LockGuid.Equals(that.LockGuid) &&
                   this.TargetGuid.Equals(that.TargetGuid) &&
                   this.Amount.Equals(that.Amount) &&
                   this.IsActive.Equals(that.IsActive) &&
                   this.Wallet.Equals(that.Wallet) &&
                   this.Operation.Equals(that.Operation);
        }
    }
}
