namespace Bars.Gkh.RegOperator.Domain.ValueObjects
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;

    using Newtonsoft.Json;

    /// <summary>
    /// Данные по массовой отмене начислений
    /// </summary>
    public class PersonalAccountCancelChargeInfo
    {
        /// <summary>
        /// Идентификатор ЛС
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Лицевой счет
        /// </summary>
        [JsonIgnore]
        public BasePersonalAccount Account { get; set; }

        /// <summary>
        /// Период
        /// </summary>
        [JsonIgnore]
        public ChargePeriod Period { get; set; }

        /// <summary>
        /// Сумма начислений по базовому тарифу
        /// </summary>
        [JsonIgnore]
        public decimal BaseTariffSum { get; set; }

        /// <summary>
        /// Сумма начислений по тарифу решения
        /// </summary>
        [JsonIgnore]
        public decimal DecisionTariffSum { get; set; }

        /// <summary>
        /// Сумма начислений по Пени
        /// </summary>
        [JsonIgnore]
        public decimal PenaltySum { get; set; }

        /// <summary>
        /// Сумма изменений по базовому тарифу
        /// </summary>
        [JsonIgnore]
        public decimal BaseTariffChange { get; set; }

        /// <summary>
        /// Сумма изменений по тарифу решения
        /// </summary>
        [JsonIgnore]
        public decimal DecisionTariffChange { get; set; }

        /// <summary>
        /// Сумма изменений по Пени
        /// </summary>
        [JsonIgnore]
        public decimal PenaltyChange { get; set; }

        /// <summary>
        /// Муниципальный район
        /// </summary>
        public string Municipality { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string RoomAddress { get; set; }

        /// <summary>
        /// Номер ЛС
        /// </summary>
        public string PersonalAccountNum { get; set; }

        /// <summary>
        /// Сумма отмены по базовому тарифу
        /// </summary>
        public decimal CancelBaseTariffSum => this.BaseTariffSum + this.BaseTariffChange;

        /// <summary>
        /// Сумма отмены по тарифу решения
        /// </summary>
        public decimal CancelDecisionTariffSum => this.DecisionTariffSum + this.DecisionTariffChange;

        /// <summary>
        /// Сумма отмены по Пени
        /// </summary>
        public decimal CancelPenaltySum => this.PenaltySum + this.PenaltyChange;
    }
}
