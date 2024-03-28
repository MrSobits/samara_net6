namespace Bars.Gkh.RegOperator.Entities.ValueObjects
{
    using System;
    using B4.DataAccess;
    using B4.Utils.Annotations;

    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner;

    /// <summary>
    /// Трансфер между источником и получателем денег
    /// </summary>
    public abstract class Transfer : BaseEntity
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="owner">Владелец трансфера</param>
        /// <param name="sourceGuid">Гуид источник</param>
        /// <param name="targetGuid">Целевой гуид</param>
        /// <param name="amount">Сумма</param>
        /// <param name="operation">Операция</param>
        protected Transfer(ITransferOwner owner, string sourceGuid, string targetGuid, decimal amount, MoneyOperation operation)
        {
            ArgumentChecker.NotNullOrEmpty(sourceGuid, nameof(sourceGuid));
            ArgumentChecker.NotNullOrEmpty(targetGuid, nameof(targetGuid));
            ArgumentChecker.NotNull(operation, nameof(operation));
            ArgumentChecker.NotNull(operation.Period, () => operation.Period);
            ArgumentChecker.NotEquals(sourceGuid, targetGuid, nameof(targetGuid));

            this.Operation = operation;
            this.Amount = amount;
            this.TargetGuid = targetGuid;
            this.SourceGuid = sourceGuid;
            this.OperationDate = DateTime.Now;
            this.PaymentDate = DateTime.Now;
            this.TargetCoef = 1;
            this.ChargePeriod = operation.Period;
            this.Owner = owner;
        }

        /// <summary>
        /// For NH
        /// </summary>
        protected Transfer()
        {
        }

        /// <summary>
        /// Источник денег
        /// </summary>
        public virtual string SourceGuid { get; protected set; }

        /// <summary>
        /// Получатель денег
        /// </summary>
        public virtual string TargetGuid { get; protected set; }

        /// <summary>
        /// Коэффициент суммы у получателя
        /// </summary>
        public virtual int TargetCoef { get; set; }

        /// <summary>
        /// Операция, в рамках которой проводился трансфер
        /// </summary>
        public virtual MoneyOperation Operation { get; set; }

        /// <summary>
        /// Сумма перевода
        /// </summary>
        public virtual decimal Amount { get; protected set; }

        /// <summary>
        /// Причина перевода
        /// </summary>
        public virtual string Reason { get; set; }

        /// <summary>
        /// Плательщик/получатель/основание
        /// </summary>
        public virtual string OriginatorName { get; set; }

        /// <summary>
        /// Дата фактической оплаты. Важно знать, когда оплата садится задним числом
        /// </summary>
        public virtual DateTime PaymentDate { get; set; }

        /// <summary>
        /// Дата операции
        /// </summary>
        public virtual DateTime OperationDate { get; set; }

        /// <summary>
        /// Признак того, что трансфер является "транзитным".
        /// Т.е. сначала трансфер произошел на целевой кошелек, а затем просто закинулся на другой
        /// </summary>
        public virtual bool IsInDirect { get; set; }

        /// <summary>
        /// Первоначальный перевод. В случае отката трансфера здесь будет ссылка на первоначальный перевод
        /// </summary>
        public virtual Transfer Originator { get; set; }

        /// <summary>
        /// Влияющий на баланс
        /// </summary>
        public virtual bool IsAffect { get; set; }

        /// <summary>
        /// Является займом
        /// </summary>
        public virtual bool IsLoan { get; set; }

        /// <summary>
        /// Является возвратом займа
        /// </summary>
        public virtual bool IsReturnLoan { get; set; }

        /// <summary>
        /// Период расчета
        /// </summary>
        public virtual ChargePeriod ChargePeriod { get; set; }

        /// <summary>
        /// Владелец трансфера
        /// </summary>
        public virtual ITransferOwner Owner { get; protected set; }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            var other = obj as Transfer;

            if (other == null)
            {
                return false;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            return this.Id > 0 && other.Id > 0 && this.Id == other.Id;
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode() ^ this.GetType().GetHashCode();
        }

        /// <summary>
        /// Костыль, написан для того чтобы смигрировать данные с реестров непотвержденные оплаты НВС
        /// </summary>
        public virtual void SetSourceGuid(string value)
        {
            this.SourceGuid = value;
        }
    }
}