namespace Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations
{
    using System;
    
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Базовая сущность операций начислений
    /// </summary>
    public abstract class ChargeOperationBase : BaseImportableEntity, IChargeOriginator
    {
        /// <summary>
        /// .ctor
        /// </summary>
        protected ChargeOperationBase()
        {
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="period">Период</param>
        protected ChargeOperationBase(ChargePeriod period)
        {
            this.Period = period;
            this.OriginatorGuid = Guid.NewGuid().ToString();
            this.FactOperationDate = DateTime.Now;
        }

        /// <summary>
        /// Период
        /// </summary>
        public virtual ChargePeriod Period { get; set; }

        ///<summary>
        /// Номер документа
        /// </summary>
        public virtual FileInfo Document { get; set; }

        /// <summary>
        /// Дата операции
        /// </summary>
        public virtual DateTime OperationDate { get; set; }

        /// <summary>
        /// Фактическая дата операции
        /// </summary>
        public virtual DateTime FactOperationDate { get; set; }

        /// <summary>
        /// Причина
        /// </summary>
        public virtual string Reason { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public virtual string User { get; set; }

        /// <summary>
        /// Тип источника поступления начислений
        /// </summary>
        public virtual TypeChargeSource ChargeSource { get; set; }

        /// <summary>
        /// Guid инициатора
        /// </summary>
        public virtual string OriginatorGuid { get; set; }

        /// <summary>
        /// Гуид, который запишется либо в SourceGuid, либо в TargetGuid объекта Transfer
        /// </summary>
        public virtual string TransferGuid => this.OriginatorGuid;

        /// <summary>
        /// Создать операцию по передвижению денег
        /// </summary>
        /// <returns><see cref="MoneyOperation"/></returns>
        public virtual MoneyOperation CreateOperation(ChargePeriod period = null)
        {
            return new MoneyOperation(this.OriginatorGuid, period ?? this.Period)
            {
                Document = this.Document,
                OperationDate = this.FactOperationDate
            };
        }
    }
}