namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccount
{
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    /// <summary>
    /// Событие, вызывающееся после отмены начислений по периоду
    /// </summary>
    public class PersonalAccountChargeUndoEvent: IDomainEvent
    {
        /// <summary>
        /// Лс
        /// </summary>
        public BasePersonalAccount Account { get; private set; }

        /// <summary>
        /// Разница по базовому
        /// </summary>
        public decimal DeltaTariff { get; private set; }

        /// <summary>
        /// Разница по тарифу решений
        /// </summary>
        public decimal DeltaDecision { get; private set; }

        /// <summary>
        /// Разница по пени
        /// </summary>
        public decimal DeltaPenalty { get; private set; }

        /// <summary>
        /// Операция
        /// </summary>
        public MoneyOperation Operation { get; set; }

        /// <summary>
        /// Изменение
        /// </summary>
        public PersonalAccountChangeInfo ChangeInfo { get; set; }

        /// <summary>
        /// Событие отмены начислений
        /// </summary>
        /// <param name="account"></param>
        /// <param name="deltaTariff"></param>
        /// <param name="deltaDecision"></param>
        /// <param name="deltaPenalty"></param>
        /// <param name="changeInfo"></param>
        public PersonalAccountChargeUndoEvent(BasePersonalAccount account, decimal deltaTariff, decimal deltaDecision, decimal deltaPenalty,
            PersonalAccountChangeInfo changeInfo)
        {
            this.Account = account;
            this.DeltaTariff = deltaTariff;
            this.DeltaDecision = deltaDecision;
            this.DeltaPenalty = deltaPenalty;
            this.ChangeInfo = changeInfo;
        }
    }
}
