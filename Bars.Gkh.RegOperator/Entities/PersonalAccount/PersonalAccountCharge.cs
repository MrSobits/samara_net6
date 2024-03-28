namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using B4.Utils.Annotations;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;

    using DomainModelServices;
    using ValueObjects;

    /// <summary>
    /// Начисления по Л/С
    /// </summary>
    public class PersonalAccountCharge : BaseImportableEntity, IChargeOriginator
    {  
        /// <summary>
        /// Пустой конструктор PersonalAccountCharge
        /// </summary>
        public PersonalAccountCharge()
        {
            this.IsActive = true;
        }

        /// <summary>
        /// Конструктор PersonalAccountCharge
        /// </summary>
        public PersonalAccountCharge(UnacceptedCharge unacceptedCharge, DateTime chargeDate) : this()
        {
            ArgumentChecker.NotNull(unacceptedCharge, nameof(unacceptedCharge));
            ArgumentChecker.NotNull(unacceptedCharge.PersonalAccount, () => unacceptedCharge.PersonalAccount);

            this.BasePersonalAccount = unacceptedCharge.PersonalAccount;
            this.ChargeDate = chargeDate;
            this.Guid = unacceptedCharge.Guid;
            this.Charge = unacceptedCharge.Charge;
            this.ChargeTariff = unacceptedCharge.ChargeTariff;
            this.Penalty = unacceptedCharge.Penalty;
            this.OverPlus = unacceptedCharge.TariffOverplus;
            this.RecalcByBaseTariff = unacceptedCharge.RecalcByBaseTariff;
            this.Packet = unacceptedCharge.Packet;
        }

        /// <summary>
        /// Лицевой счет
        /// </summary>
        public virtual BasePersonalAccount BasePersonalAccount { get; set; }

        /// <summary>
        /// Дата начисления
        /// </summary>
        public virtual DateTime ChargeDate { get; set; }

        /// <summary>
        /// Период начисления
        /// </summary>
        public virtual ChargePeriod ChargePeriod { get; set; }

        /// <summary>
        /// GUID начисления (для связи с неподтвержденными)
        /// </summary>
        public virtual string Guid { get; set; }

        /// <summary>
        /// Сумма начисления. Складывается из суммы по тарифу, суммы по пени, суммы по перерасчету
        /// </summary>
        public virtual decimal Charge { get; set; }

        /// <summary>
        /// Сумма начисления по тарифу
        /// <para>По Базовому тарифу</para>
        /// </summary>
        public virtual decimal ChargeTariff { get; set; }

        /// <summary>
        /// Сумма начисления по пени
        /// </summary>
        public virtual decimal Penalty { get; set; }

        /// <summary>
        /// Сумма перерасчета
        /// </summary>
        public virtual decimal RecalcByBaseTariff { get; set; }

        /// <summary>
        /// Перерасчет по тарифу решения
        /// </summary>
        public virtual decimal RecalcByDecisionTariff { get; set; }

        /// <summary>
        /// Перерасчет пени
        /// </summary>
        public virtual decimal RecalcPenalty { get; set; }

        /// <summary>
        /// Избыток как разница между Решением собственников и базовым тарифом
        /// <para>По Тарифу решения</para>
        /// </summary>
        public virtual decimal OverPlus { get; set; }

        /// <summary>
        /// Зафиксирована
        /// </summary>
        public virtual bool IsFixed { get; set; }

        /// <summary>
        /// Начисление активно
        /// <para>Если имеет значение true, то на основании этого начисления будут созданы трансферы при закрытии периода</para>
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Пакет подтвержденных начилсений для группировки
        /// </summary>
        public virtual UnacceptedChargePacket Packet { get; set; }

        /// <summary>
        /// Гуид, который запишется либо в SourceGuid, либо в TargetGuid объекта Transfer
        /// </summary>
        string ITransferParty.TransferGuid => this.Guid;

        /// <summary>
        /// Создать операцию по передвижению денег
        /// </summary>
        /// <returns><see cref="MoneyOperation"/></returns>
        public virtual MoneyOperation CreateOperation(ChargePeriod period = null)
        {
            return new MoneyOperation(this.Guid, period ?? this.ChargePeriod)
            {
                Reason = "Начисление",
                OperationDate = this.ChargeDate
            };
        }

        /// <inheritdoc />
        public virtual TypeChargeSource ChargeSource => TypeChargeSource.Charge;

        /// <inheritdoc />
        public virtual string OriginatorGuid => this.Guid;
    }
}