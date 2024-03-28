namespace Bars.Gkh.RegOperator.DomainEvent.Handlers.PersonalAccountPayment
{
    using B4.Utils;

    using Bars.Gkh.DomainEvent.Infrastructure;

    using Domain.Repository.StatefulEntity;
    using Entities;
    using Events.PersonalAccountPayment;

    public class PersonalAccountDebtIsZeroAfterPaymentEventHandler : 
        IDomainEventHandler<PersonalAccountDebtIsZeroEvent>
    {
        private readonly IStatefulEntityRepository stateRepo;

        public PersonalAccountDebtIsZeroAfterPaymentEventHandler(IStatefulEntityRepository stateRepo)
        {
            this.stateRepo = stateRepo;
        }

        /// <summary>
        /// Обработать событие
        /// </summary>
        /// <param name="args">Аргумент-событие</param>
        public void Handle(PersonalAccountDebtIsZeroEvent args)
        {
            if (!args.Account.IsClosedWithCredit()) return;

            var closeState = this.stateRepo.GetStateByName<BasePersonalAccount>("закрыт");

            if (closeState.IsNotNull())
            {
                args.Account.State = closeState;
            }
        }
    }
}