namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccount
{
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Событие смены сальдо
    /// </summary>
    public class PersonalAccountSaldoChangeMassEvent : IDomainEvent
    {
        /// <summary>
        /// Лицевой счет
        /// </summary>
        public BasePersonalAccount Account { get; set; }

        /// <summary>
        /// Изменение сальдо по базовому тарифу
        /// </summary>
        public decimal SaldoByBaseTariffDelta { get; set; }

        /// <summary>
        /// Изменение сальдо по тарифу решения
        /// </summary>
        public decimal SaldoByDecisionTariffDelta { get; set; }

        /// <summary>
        /// Изменение сальдо по пени
        /// </summary>
        public decimal SaldoByPenaltyDelta { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        public PersonalAccountSaldoChangeMassEvent(
            BasePersonalAccount account,
            decimal saldoByBaseTariffDelta,
            decimal saldoByDecisionTariffDelta,
            decimal saldoByPenaltyDelta)
        {
            this.Account = account;
            this.SaldoByBaseTariffDelta = saldoByBaseTariffDelta;
            this.SaldoByDecisionTariffDelta = saldoByDecisionTariffDelta;
            this.SaldoByPenaltyDelta = saldoByPenaltyDelta;
        }
    }
}