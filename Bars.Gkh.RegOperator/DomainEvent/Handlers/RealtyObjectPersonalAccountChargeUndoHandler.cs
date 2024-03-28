namespace Bars.Gkh.RegOperator.DomainEvent.Handlers
{
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Domain.AggregationRoots;
    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccount;
    using Bars.Gkh.RegOperator.DomainEvent.Infrastructure;

    /// <summary>
    /// Обработчик отмены начислений на доме
    /// </summary>
    public class RealtyObjectPersonalAccountChargeUndoHandler: IDomainEventHandler<PersonalAccountChargeUndoEvent>,
        IRealtyObjectPaymentEventHandler<PersonalAccountChargeUndoEvent>
    {
        private readonly IRealtyObjectPaymentSession session;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="session"></param>
        public RealtyObjectPersonalAccountChargeUndoHandler(IRealtyObjectPaymentSession session)
        {
            this.session = session;
        }

        #region Implementation of IDomainEventHandler<in PersonalAccountPenaltyChargeUndoEvent>

        /// <summary>
        /// Обработать событие
        /// </summary>
        /// <param name="args">Аргумент-событие</param>
        public void Handle(PersonalAccountChargeUndoEvent args)
        {
            this.session.FireEvent(args.Account.Room.RealityObject, args);
        }

        #endregion

        #region Implementation of IRealtyObjectPaymentEventHandler<in PersonalAccountPenaltyChargeUndoEvent>

        public void Execute(RealtyObjectPaymentRoot root, PersonalAccountChargeUndoEvent args)
        {
            var delta = args.DeltaTariff + args.DeltaDecision;
            var penaltyDelta = args.DeltaPenalty;

            if (delta != 0 || penaltyDelta != 0)
            {
                root.ChargeAccount.UndoCharge(delta, penaltyDelta);
            }
        }

        #endregion
    }
}
