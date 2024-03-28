namespace Bars.Gkh.RegOperator.Entities.PersonalAccount
{
    using System;
    using System.Collections.Generic;
    using B4.Modules.FileStorage;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;

    using Newtonsoft.Json;
    using ValueObjects;

    /// <summary>
    /// Изменение исходящего сальдо счета
    /// </summary>
    public class PeriodSummaryBalanceChange : BaseImportableEntity, IChargeOriginator
    {
        private readonly List<MoneyOperation> operations;

        protected PeriodSummaryBalanceChange()
        {
            this.TransferGuid = Guid.NewGuid().ToString();
            this.operations = new List<MoneyOperation>();
        }

        public PeriodSummaryBalanceChange(
            PersonalAccountPeriodSummary periodSummary,
            DateTime operationDate,
            decimal currentValue,
            decimal newValue,
            FileInfo document,
            string reason) : this()
        {
            this.PeriodSummary = periodSummary;
            this.OperationDate = operationDate;
            this.CurrentValue = currentValue;
            this.NewValue = newValue;
            this.Document = document;
            this.Reason = reason;
        }

        /// <summary>
        /// Ситуация за период по лицевому счету
        /// </summary>
        public virtual PersonalAccountPeriodSummary PeriodSummary { get; protected set; }

        /// <summary>
        /// Дата операции
        /// </summary>
        public virtual DateTime OperationDate { get; protected set; }

        /// <summary>
        /// Значение исходящего сальдо перед изменением
        /// </summary>
        public virtual decimal CurrentValue { get; protected set; }

        /// <summary>
        /// Новое значение исходящего сальдо
        /// </summary>
        public virtual decimal NewValue { get; protected set; }

        /// <summary>
        /// Документ-основание для изменения
        /// </summary>
        public virtual FileInfo Document { get; protected set; }

        /// <summary>
        /// Причина изменения
        /// </summary>
        public virtual string Reason { get; protected set; }

        /// <summary>
        /// Гуид, который запишется либо в SourceGuid, либо в TargetGuid объекта Transfer
        /// </summary>
        public virtual string TransferGuid { get; protected set; }

        /// <summary>
        /// Создать операцию по передвижению денег
        /// </summary>
        /// <returns><see cref="MoneyOperation"/></returns>
        public virtual MoneyOperation CreateOperation(ChargePeriod period)
        {
            var newOper = new MoneyOperation(this.TransferGuid, period);
            this.Operations.Add(newOper);
            return newOper;
        }

        /// <summary>
        /// Денежные операции, связанные с экземпляром изменения сальдо
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<MoneyOperation> Operations
        {
            get { return this.operations; }
            protected set
            {
                this.operations.Clear();
                if (value != null)
                {
                    this.operations.AddRange(value);
                }
            }
        }

        /// <summary>
        /// Тип источника поступления начислений (не хранимый, не использовать в запросах!)
        /// </summary>
        public virtual TypeChargeSource ChargeSource => TypeChargeSource.BalanceChange;

        /// <summary>
        /// Guid инициатора (не хранимый, не использовать в запросах!)
        /// </summary>
        public virtual string OriginatorGuid => this.TransferGuid;
    }
}