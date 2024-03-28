namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using DomainModelServices;
    using ValueObjects;

    /// <summary>
    /// Оплата Л/С
    /// </summary>
    public class PersonalAccountPayment : BaseImportableEntity, IMoneyOperationSource
    {
        /// <summary>
        /// Дата оплаты
        /// </summary>
        public virtual DateTime PaymentDate { get; set; }

        /// <summary>
        /// Сумма оплаты
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Лицевой счет
        /// </summary>
        public virtual BasePersonalAccount BasePersonalAccount { get; set; }

        /// <summary>
        /// Тип оплаты
        /// </summary>
        public virtual PaymentType Type { get; set; }

        /// <summary>
        /// Guid (для связи с неподтвержденной оплатой)
        /// </summary>
        public virtual string Guid { get; set; }

        /// <summary>
        /// Создать операцию по передвижению денег
        /// </summary>
        /// <returns><see cref="MoneyOperation"/></returns>
        public virtual MoneyOperation CreateOperation(ChargePeriod period)
        {
            return new MoneyOperation(this.Guid, period);
        }
    }
}