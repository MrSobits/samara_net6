namespace Bars.Gkh.RegOperator.DomainEvent.Handlers
{
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Domain.AggregationRoots;
    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccount;
    using Bars.Gkh.RegOperator.DomainEvent.Infrastructure;

    /// <summary>
    /// Перехватчик события <see cref="IDomainEventHandler{PersonalAccountPenaltyChargeUndoEvent}"/>
    /// </summary>
    public class RealtyObjectPersonalAccountPenaltyUndoHandler : 
        IDomainEventHandler<PersonalAccountPenaltyChargeUndoEvent>,

        IRealtyObjectPaymentEventHandler<PersonalAccountPenaltyChargeUndoEvent>
    {
        private readonly IRealtyObjectPaymentSession session;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="session">Провайдер сессий</param>
        public RealtyObjectPersonalAccountPenaltyUndoHandler(IRealtyObjectPaymentSession session)
        {
            this.session = session;
        }

        #region Implementation of IDomainEventHandler<in PersonalAccountPenaltyChargeUndoEvent>

        /// <summary>
        /// Обработать событие
        /// </summary>
        /// <param name="args">Аргумент-событие</param>
        public void Handle(PersonalAccountPenaltyChargeUndoEvent args)
        {
            this.session.FireEvent(args.Account.Room.RealityObject, args);
        }

        #endregion

        #region Implementation of IRealtyObjectPaymentEventHandler<in PersonalAccountPenaltyChargeUndoEvent>

        /// <inheritdoc />
        public void Execute(RealtyObjectPaymentRoot root, PersonalAccountPenaltyChargeUndoEvent args)
        {
            if (args.NewPenalty != 0)
            {
                root.ChargeAccount.UndoCharge(0, args.NewPenalty);
            }
        }

        #endregion
    }
}