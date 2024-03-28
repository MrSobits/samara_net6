namespace Bars.Gkh.RegOperator.DomainEvent.Handlers
{
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Domain.AggregationRoots;
    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccount;
    using Bars.Gkh.RegOperator.DomainEvent.Infrastructure;

    /// <summary>
    /// Обработчик события <see cref="PersonalAccountSaldoChangeMassEvent"/>
    /// </summary>
    public class RealityObjectPersonalAccountSaldoChangeHandler : 
        IDomainEventHandler<PersonalAccountSaldoChangeMassEvent>,
        IRealtyObjectPaymentEventHandler<PersonalAccountSaldoChangeMassEvent>
    {
        private readonly IRealtyObjectPaymentSession session;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="session"></param>
        public RealityObjectPersonalAccountSaldoChangeHandler(IRealtyObjectPaymentSession session)
        {
            this.session = session;
        }

        /// <summary>
        /// Обработать событие
        /// </summary>
        public void Handle(PersonalAccountSaldoChangeMassEvent args)
        {
            this.session.FireEvent(args.Account.Room.RealityObject, args);
        }

        /// <summary>
        /// Обработать событие на доме
        /// </summary>
        public void Execute(RealtyObjectPaymentRoot root, PersonalAccountSaldoChangeMassEvent args)
        {
            root.ChargeAccount.ApplyChangeSaldo(args.SaldoByBaseTariffDelta, args.SaldoByDecisionTariffDelta, args.SaldoByPenaltyDelta);
        }
    }
}