namespace Bars.Gkh.RegOperator.Domain.ValueObjects
{
    using System;

    using B4.Utils.Annotations;

    using Bars.Gkh.Entities;

    using DomainModelServices;

    using Entities;
    using Entities.ValueObjects;

    /// <summary>
    /// Объект потока денег. Используется при перемещении денег от источника получателю
    /// </summary>
    public class MoneyStream
    {
        /// <summary>
        ///  Создать экземпляр <see cref="MoneyStream"/> на основе трансфера
        /// </summary>
        /// <param name="originalTransfer">Трансфер инициатор</param>
        public MoneyStream(Transfer originalTransfer)
            : this(
                originalTransfer.SourceGuid,
                originalTransfer.Operation,
                originalTransfer.PaymentDate,
                originalTransfer.Amount)
        {
            ArgumentChecker.NotNull(originalTransfer, nameof(originalTransfer));
            
            this.OriginalTransfer = originalTransfer;
        }

        /// <summary>
        /// Создать экземпляр
        /// </summary>
        /// <param name="source">Инициатор</param>
        /// <param name="operation">Операция, в рамках которой передвигаются деньги</param>
        /// <param name="operationFactDate">Фактическая дата операции</param>
        /// <param name="amount">Количество денег</param>
        public MoneyStream(ITransferParty source, MoneyOperation operation, DateTime operationFactDate, decimal amount)
            : this(source.TransferGuid, operation, operationFactDate, amount)
        {
            this.Source = source;
        }

        /// <summary>
        /// Создать экземпляр
        /// </summary>
        /// <param name="sourceOrTargetGuid">Источник денег</param>
        /// <param name="operation">Операция, в рамках которой передвигаются деньги</param>
        /// <param name="operationFactDate">Фактическая дата операции</param>
        /// <param name="amount">Количество денег</param>
        public MoneyStream(string sourceOrTargetGuid, MoneyOperation operation, DateTime operationFactDate, decimal amount)
        {
            ArgumentChecker.NotNull(operation, nameof(operation));
            ArgumentChecker.NotNull(operation.Period, () => operation.Period);
            ArgumentChecker.NotNullOrEmpty(sourceOrTargetGuid, nameof(sourceOrTargetGuid));

            this.Amount = amount;
            this.SourceOrTargetGuid = sourceOrTargetGuid;
            this.Operation = operation;
            this.OperationFactDate = operationFactDate;

            this.OperationDate = operation.Period.GetCurrentInPeriodDate();
            this.CurrentPeriod = operation.Period;
        }

        /// <summary>
        /// Текущий период
        /// </summary>
        public ChargePeriod CurrentPeriod { get; set; }

        /// <summary>
        /// Дата операции
        /// </summary>
        public DateTime OperationDate { get; set; }

        /// <summary>
        /// Операция, в рамках которой идет перемещение денег
        /// </summary>
        public MoneyOperation Operation { get; protected set; }

        /// <summary>
        /// Источник денег
        /// </summary>
        public string SourceOrTargetGuid { get; private set; }

        /// <summary>
        /// Фактическая дата операции
        /// </summary>
        public DateTime OperationFactDate { get; set; }

        /// <summary>
        /// Описание операции
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Получатель денег (лс, дом и тд)
        /// </summary>
        public string OriginatorName { get; set; }

        /// <summary>
        /// Количество денег
        /// </summary>
        public decimal Amount { get; private set; }

        /// <summary>
        /// Оригинальный трансфер. Например, когда идет проброс из ЛС в дом
        /// </summary>
        public Transfer OriginalTransfer { get; set; }

        /// <summary>
        /// Влияет на баланс
        /// </summary>
        public bool IsAffect { get; set; }

        /// <summary>
        /// Инициатор (может быть null)
        /// </summary>
        public ITransferParty Source { get; private set; }
    }
}