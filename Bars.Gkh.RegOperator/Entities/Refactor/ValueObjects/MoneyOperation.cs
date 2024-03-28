namespace Bars.Gkh.RegOperator.Entities.ValueObjects
{
    using System;
    using B4.DataAccess;
    using B4.Modules.FileStorage;
    using B4.Utils.Annotations;

    using Bars.B4.Application;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Операция, в рамках которой могут происходить различные движения денег
    /// </summary>
    public class MoneyOperation : BaseEntity, ICloneable
    {
        /// <summary>
        /// Операция, которая была отменена данной операцией
        /// </summary>
        public virtual MoneyOperation CanceledOperation { get; protected set; }

        /// <summary>
        /// Создает экземпляр
        /// </summary>
        /// <param name="originatorGuid">Гуид инициатора события</param>
        /// <param name="period">Период операции</param>
        public MoneyOperation(string originatorGuid, ChargePeriod period)
        {
            ArgumentChecker.NotNullOrEmpty(originatorGuid, nameof(originatorGuid));
            ArgumentChecker.NotNull(period, nameof(period));

            this.OperationGuid = Guid.NewGuid().ToString();
            this.OriginatorGuid = originatorGuid;
            this.Period = period;
            this.UserLogin = this.UserManager.GetActiveUser()?.Login;
        }

        /// <summary>
        /// Создает экземпляр
        /// </summary>
        /// <param name="originatorGuid">Гуид инициатора события</param>
        /// <param name="period">Период операции</param>
        /// <param name="document">Документ</param>
        public MoneyOperation(string originatorGuid, ChargePeriod period, FileInfo document) 
            : this(originatorGuid, period)
        {
            ArgumentChecker.NotNullOrEmpty(originatorGuid, nameof(originatorGuid));
            this.Document = document;
        }

        /// <summary>
        /// Создает экземпляр
        /// </summary>
        /// <param name="originatorGuid">Гуид инициатора события</param>
        /// <param name="operationToCancel">Отменяемая операция</param>
        /// <param name="period">Период операции</param>
        /// <param name="amount">Сумма операции</param>
        public MoneyOperation(string originatorGuid, MoneyOperation operationToCancel, ChargePeriod period, decimal amount = 0M)
            : this(originatorGuid, period)
        {
            ArgumentChecker.NotNullOrEmpty(originatorGuid, nameof(originatorGuid));
            ArgumentChecker.NotNull(operationToCancel, nameof(operationToCancel));

            this.CanceledOperation = operationToCancel;
            this.OperationDate = DateTime.Now;
            this.Amount = amount;
        }

        /// <summary>
        /// .ctor Nh
        /// </summary>
        protected MoneyOperation()
        {
        }

        /// <summary>
        /// Гуид операции
        /// </summary>
        public virtual string OperationGuid { get; protected set; }

        /// <summary>
        /// Гуид инициатора
        /// </summary>
        public virtual string OriginatorGuid { get; protected set; }

        /// <summary>
        /// Операция отменена
        /// </summary>
        public virtual bool IsCancelled { get; protected set; }

        /// <summary>
        /// Сумма перевода
        /// </summary>
        public virtual decimal Amount { get; set; }

        /// <summary>
        /// Причина перевода
        /// </summary>
        public virtual string Reason { get; set; }

        /// <summary>
        /// Документ операции
        /// </summary>
        public virtual FileInfo Document { get; set; }

        /// <summary>
        /// Дата операции
        /// </summary>
        public virtual DateTime OperationDate { get; set; }

        /// <summary>
        /// Период
        /// </summary>
        public virtual ChargePeriod Period { get; set; }

        /// <summary>
        /// Логин пользователя, который инициировал совершение операции
        /// </summary>
        public virtual string UserLogin { get; set; }

        private IGkhUserManager userManager;
        private IGkhUserManager UserManager
        {
            get
            {
                return this.userManager ?? (this.userManager = ApplicationContext.Current.Container.Resolve<IGkhUserManager>());
            }
        }

        /// <summary>
        /// Отменить операцию
        /// </summary>
        /// <param name="period">Период операции</param>
        public virtual MoneyOperation Cancel(ChargePeriod period)
        {
            this.IsCancelled = true;

            return new MoneyOperation(this.OriginatorGuid, this, period);
        }

        /// <summary>
        /// Костыль, написан для того чтобы смигрировать данные с реестров непотвержденные оплаты НВС
        /// </summary>
        /// <param name="value">
        /// Значение
        /// </param>
        public virtual void SetOriginatorGuid(string value)
        {
            this.OriginatorGuid = value;
        }

        /// <inheritdoc />
        public virtual object Clone()
        {
            return new MoneyOperation
            {
                Period = this.Period,
                IsCancelled = this.IsCancelled,
                CanceledOperation = this.CanceledOperation,
                Amount = this.Amount,
                Document = this.Document,
                OperationDate = this.OperationDate,
                OperationGuid = Guid.NewGuid().ToString(),
                OriginatorGuid = this.OriginatorGuid,
                Reason = this.Reason,
                UserLogin = this.UserManager.GetActiveUser()?.Login
            };
        }
    }
}