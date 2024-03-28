namespace Bars.Gkh.RegOperator.DomainEvent.Handlers
{
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountPayment;
    using Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;

    public class PersonalAccountRecalcHandler : IDomainEventHandler<PersonalAccountCloseEvent>
    {
        private readonly IPersonalAccountRecalcEventManager recalcManager;

        private readonly IChargePeriodRepository periodRepository;

        public PersonalAccountRecalcHandler(IPersonalAccountRecalcEventManager recalcManager, IChargePeriodRepository periodRepository)
        {
            this.recalcManager = recalcManager;
            this.periodRepository = periodRepository;
        }

        public void Handle(PersonalAccountCloseEvent args)
        {
            var currentPeriod = this.periodRepository.GetCurrentPeriod();

            if (args.CloseDate < currentPeriod.StartDate)
            {
                this.recalcManager.CreateChargeEvent(args.Account, args.CloseDate, RecalcEventType.ChangeCloseDate, "Закрытие ЛС");
                this.recalcManager.CreatePenaltyEvent(args.Account, args.CloseDate, RecalcEventType.ChangeCloseDate, "Закрытие ЛС");
                this.recalcManager.SaveEvents();
            }
        }
    }
}