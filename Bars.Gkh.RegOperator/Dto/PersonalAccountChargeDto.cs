namespace Bars.Gkh.RegOperator.Dto
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Refactor;
    using Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Начисления по Л/С
    /// </summary>
    public class PersonalAccountChargeDto : IChargeOriginator
    {
        private readonly TransferOwnerHolder transferOwner;
        
        public PersonalAccountChargeDto(long persAccId)
        {
            this.BasePersonalAccountId = persAccId;
            this.transferOwner = new TransferOwnerHolder(WalletOwnerType.BasePersonalAccount, this.BasePersonalAccountId);
        }

        /// <summary>
        /// Пустой конструктор
        /// </summary>
        protected PersonalAccountChargeDto()
        {
            
        }

        /// <summary>
        /// Конструктор для подтвержденного начисления из неподтвержденного
        /// </summary>
        /// <param name="unacceptedCharge">Неподтвержденные начисления</param>
        /// <param name="period">Период</param>
        /// <param name="calcIsNotActive">Не показывать начисления в ЛС</param>
        public PersonalAccountChargeDto(UnacceptedCharge unacceptedCharge, IPeriod period, bool calcIsNotActive) : this(unacceptedCharge.PersonalAccount.Id)
        {
            var chargeDate = unacceptedCharge.ObjectCreateDate;
            var periodEnd = period.GetEndDate();
            if (chargeDate <= period.StartDate || chargeDate >= periodEnd)
            {
                chargeDate = periodEnd;
            }
            this.ObjectCreateDate = unacceptedCharge.ObjectCreateDate;
            this.ObjectEditDate = unacceptedCharge.ObjectEditDate;
            this.ObjectVersion = unacceptedCharge.ObjectVersion;
            this.Charge = unacceptedCharge.Charge;
            this.ChargeDate = chargeDate;
            this.ChargeTariff = unacceptedCharge.ChargeTariff;
            this.Penalty = unacceptedCharge.Penalty;
            this.Guid = unacceptedCharge.Guid;
            this.OverPlus = unacceptedCharge.TariffOverplus;
            this.RecalcByBaseTariff = unacceptedCharge.RecalcByBaseTariff;
            this.RecalcByDecisionTariff = unacceptedCharge.RecalcByDecision;
            this.RecalcPenalty = unacceptedCharge.RecalcPenalty;
            this.ChargePeriodId = period.Id;
            this.PacketId = unacceptedCharge.Packet.Id;
            this.IsActive = !calcIsNotActive;
        }

        /// <summary>
        /// Конструктор для подтвержденного 
        /// </summary>
        /// <param name="charge">Подтвержденные начисления</param>
        /// <param name="period">Период</param>
        /// <param name="calcIsNotActive">Не показывать начисления в ЛС</param>
        public PersonalAccountChargeDto(PersonalAccountChargeDto charge, IPeriod period, bool calcIsNotActive) : this(charge.BasePersonalAccountId)
        {
            var chargeDate = charge.ObjectCreateDate;
            var periodEnd = period.GetEndDate();
            if (chargeDate <= period.StartDate || chargeDate >= periodEnd)
            {
                chargeDate = periodEnd;
            }
            this.ObjectCreateDate = charge.ObjectCreateDate;
            this.ObjectEditDate = charge.ObjectEditDate;
            this.ObjectVersion = charge.ObjectVersion;
            this.Charge = charge.Charge;
            this.ChargeDate = chargeDate;
            this.ChargeTariff = charge.ChargeTariff;
            this.Penalty = charge.Penalty;
            this.Guid = charge.Guid;
            this.OverPlus = charge.OverPlus;
            this.RecalcByBaseTariff = charge.RecalcByBaseTariff;
            this.RecalcByDecisionTariff = charge.RecalcByDecisionTariff;
            this.RecalcPenalty = charge.RecalcPenalty;
            this.ChargePeriodId = period.Id;
            this.PacketId = charge.PacketId;
            this.IsActive = !calcIsNotActive;
            this.IsFixed = charge.IsFixed;
        }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime ObjectCreateDate { get; set; }

        /// <summary>
        /// Дата последнего редактирования
        /// </summary>
        public DateTime ObjectEditDate { get; set; }

        /// <summary>
        /// Версия объекта
        /// </summary>
        public int ObjectVersion { get; set; }

        /// <summary>
        /// Лицевой счет
        /// </summary>
        public long BasePersonalAccountId { get; set; }

        /// <summary>
        /// Дата начисления
        /// </summary>
        public DateTime ChargeDate { get; set; }

        /// <summary>
        /// Период начисления
        /// </summary>
        public long ChargePeriodId { get; set; }

        /// <summary>
        /// Идентификатор пакета
        /// </summary>
        public long PacketId { get; set; }

        /// <summary>
        /// GUID начисления (для связи с неподтвержденными)
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// Сумма начисления. Складывается из суммы по тарифу, суммы по пени, суммы по перерасчету
        /// </summary>
        public decimal Charge { get; set; }

        /// <summary>
        /// Сумма начисления по тарифу
        /// </summary>
        public decimal ChargeTariff { get; set; }

        /// <summary>
        /// Сумма начисления по пени
        /// </summary>
        public decimal Penalty { get; set; }

        /// <summary>
        /// Сумма перерасчета
        /// </summary>
        public decimal RecalcByBaseTariff { get; set; }

        /// <summary>
        /// Перерасчет по тарифу решения
        /// </summary>
        public decimal RecalcByDecisionTariff { get; set; }

        /// <summary>
        /// Перерасчет пени
        /// </summary>
        public decimal RecalcPenalty { get; set; }

        /// <summary>
        /// Избыток как разница между Решением собственников и базовым тарифом
        /// </summary>
        public decimal OverPlus { get; set; }

        /// <summary>
        /// Зафиксирована
        /// </summary>
        public bool IsFixed { get; set; }

        /// <summary>
        /// Начисление активно
        /// </summary>
        public bool IsActive { get; set; }

        public List<Transfer> CreateChargeTransfers(MoneyOperation moneyOperation, IDictionary<WalletType, string> walletGuids)
        {
            var transfers = new List<Transfer>();
            if (!this.IsFixed)
            {
                return transfers;
            }

            ArgumentChecker.NotNull(walletGuids, nameof(walletGuids));

            var btTransfer = this.transferOwner.TakeMoney(walletGuids[WalletType.BaseTariffWallet],TransferBuilder.Create(this.transferOwner, new MoneyStream(this, moneyOperation, this.ChargeDate, this.ChargeTariff - this.OverPlus)));

            var recalcTransfer = this.transferOwner.TakeMoney(walletGuids[WalletType.BaseTariffWallet], TransferBuilder.Create(this.transferOwner, new MoneyStream(this, moneyOperation, this.ChargeDate, this.RecalcByBaseTariff)));

            var dtTransfer = this.transferOwner.TakeMoney(walletGuids[WalletType.DecisionTariffWallet], TransferBuilder.Create(this.transferOwner, new MoneyStream(this, moneyOperation, this.ChargeDate, this.OverPlus)));

            var dtRecalcTransfer = this.transferOwner.TakeMoney(walletGuids[WalletType.DecisionTariffWallet], TransferBuilder.Create(this.transferOwner, new MoneyStream(this, moneyOperation, this.ChargeDate, this.RecalcByDecisionTariff)));

            var pTransfer = this.transferOwner.TakeMoney(walletGuids[WalletType.PenaltyWallet], TransferBuilder.Create(this.transferOwner, new MoneyStream(this, moneyOperation, this.ChargeDate, this.Penalty)));

            var pRecalcTransfer = this.transferOwner.TakeMoney(walletGuids[WalletType.PenaltyWallet], TransferBuilder.Create(this.transferOwner, new MoneyStream(this, moneyOperation, this.ChargeDate, this.RecalcPenalty)));

            if (btTransfer != null)
            {
                btTransfer.Reason = "Начисление по базовому тарифу";
                transfers.Add(btTransfer);
            }

            if (recalcTransfer != null)
            {
                recalcTransfer.Reason = "Перерасчет по базовому тарифу";
                transfers.Add(recalcTransfer);
            }

            if (dtTransfer != null)
            {
                dtTransfer.Reason = "Начисление по тарифу решения";
                transfers.Add(dtTransfer);
            }

            if (dtRecalcTransfer != null)
            {
                dtRecalcTransfer.Reason = "Перерасчет по тарифу решения";
                transfers.Add(dtRecalcTransfer);
            }

            if (pTransfer != null)
            {
                pTransfer.Reason = "Начисление пени";
                transfers.Add(pTransfer);
            }

            if (pRecalcTransfer != null)
            {
                pRecalcTransfer.Reason = "Перерасчет пени";
                transfers.Add(pRecalcTransfer);
            }

            return transfers;
        }

        /// <inheritdoc />
        public MoneyOperation CreateOperation(ChargePeriod period)
        {
            return new MoneyOperation(this.Guid, period);
        }

        /// <inheritdoc />
        public string TransferGuid => this.Guid;

        /// <inheritdoc />
        public TypeChargeSource ChargeSource => TypeChargeSource.Charge;

        /// <inheritdoc />
        public string OriginatorGuid => this.Guid;
    }
}