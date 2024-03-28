namespace Bars.Gkh.RegOperator.Entities
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;

    using Enums;
    using ValueObjects;

    /// <summary>
    /// Неподтвержденная оплата
    /// </summary>
    public class UnacceptedPayment : BaseImportableEntity
    {
        public UnacceptedPayment()
        {
            this.TransferGuid = System.Guid.NewGuid().ToString();
            this.Guid = this.TransferGuid;
        }

        /// <summary>
        /// Ссылка на пакет неподтвержденных оплат
        /// </summary>
        public virtual UnacceptedPaymentPacket Packet { get; set; }

        /// <summary>
        /// Л/С
        /// </summary>
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        /// <summary>
        /// Сумма оплаты
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Сумма оплаты пени
        /// </summary>
        public virtual decimal? PenaltySum { get; set; }

        /// <summary>
        /// Дата оплаты
        /// </summary>
        public virtual DateTime PaymentDate { get; set; }

        /// <summary>
        /// Тип оплаты
        /// </summary>
        public virtual PaymentType PaymentType { get; set; }

        /// <summary>
        /// Guid оплаты
        /// </summary>
        public virtual string Guid { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public virtual string Comment { get; set; }
        

        /// <summary>
        /// Подтверждено
        /// </summary>
        public virtual bool Accepted { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocNumber { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocDate { get; set; }

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
            return new MoneyOperation(this.TransferGuid, period)
            {
                Amount = this.Sum + (this.PenaltySum ?? 0m),
                Reason = "Подтверждение оплаты"
            };
        }

        /// <summary>
        /// Костыль, написан для того чтобы восстановить непотвержденные оплаты
        /// </summary>
        /// <returns><see cref="MoneyOperation"/></returns>
        public virtual void SetGuid(string value)
        {
            this.TransferGuid = value;
            this.Guid = value;
        }
    }
}