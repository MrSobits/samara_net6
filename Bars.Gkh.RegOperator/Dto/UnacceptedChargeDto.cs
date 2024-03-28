namespace Bars.Gkh.RegOperator.Dto
{
    using System;

    using Bars.B4.DataAccess.Attributes;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Неподтвержденное начисление
    /// </summary>
    public class UnacceptedChargeDto
    {

        /// <summary>
        /// Конструктор принимающий неподтвержденные начисления
        /// </summary>
        /// <param name="source"></param>
        public UnacceptedChargeDto(UnacceptedCharge source)
        {
            this.ObjectCreateDate = source.ObjectCreateDate;
            this.ObjectEditDate = source.ObjectEditDate;
            this.ObjectVersion = source.ObjectVersion;
            this.PacketId = source.Packet.Id;
            this.PersonalAccountId = source.PersonalAccount.Id;
            this.Guid = source.Guid;
            this.Charge = source.Charge;
            this.ChargeTariff = source.ChargeTariff;
            this.TariffOverplus = source.TariffOverplus;
            this.Penalty = source.Penalty;
            this.RecalcPenalty = source.RecalcPenalty;
            this.RecalcByBaseTariff = source.RecalcByBaseTariff;
            this.RecalcByDecision = source.RecalcByDecision;
            this.Accepted = source.Accepted;
            this.Description = source.Description;
            this.AccountStateId = source.AccountState.Id;
            this.ContragentAccountNumber = source.ContragentAccountNumber;
            this.CrFundFormationDecisionTypeId = (long)source.CrFundFormationDecisionType;
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
        /// Ссылка на пакет неподтвержденных начислений
        /// </summary>
        public long PacketId { get; set; }

        /// <summary>
        /// ЛС
        /// </summary>
        public long PersonalAccountId { get; set; }

        /// <summary>
        /// GUID начисления
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// Сумма начисления. Складывается из суммы по базовому тарифу, суммы переплаты, суммы по пени, суммы по перерасчету
        /// </summary>
        public decimal Charge { get; set; }

        /// <summary>
        /// Сумма начисления по тарифу
        /// </summary>
        public decimal ChargeTariff { get; set; }

        /// <summary>
        /// Сумма начисления, которая пришла сверх базового тарифа
        /// </summary>
        public decimal TariffOverplus { get; set; }

        /// <summary>
        /// Сумма начисления по пени
        /// </summary>
        public decimal Penalty { get; set; }

        /// <summary>
        /// Перерасчет пени
        /// </summary>
        public decimal RecalcPenalty { get; set; }

        /// <summary>
        /// Сумма перерасчета по базовому тарифу
        /// </summary>
        public decimal RecalcByBaseTariff { get; set; }

        /// <summary>
        /// Перерасчет по тарифу решения
        /// </summary>
        public decimal RecalcByDecision { get; set; }

        /// <summary>
        /// Подтверждено
        /// </summary>
        public bool Accepted { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Статус ЛС на момент расчета
        /// </summary>
        public long AccountStateId { get; set; }

        /// <summary>
        /// Номер расчетного счета
        /// </summary>
        public string ContragentAccountNumber { get; set; }


        /// <summary>
        /// Способ формирования фонда КР
        /// </summary>
        public long CrFundFormationDecisionTypeId { get; set; }
    }
}