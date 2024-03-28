namespace Bars.Gkh.DomainEvent.Handlers
{
    using Bars.B4;
    using Bars.Gkh.DomainEvent.Events;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Entities;
    using Bars.Gkh.GeneralState;

    /// <summary>
    /// Обработчик события смены обощенного статуса
    /// </summary>
    public class GeneralStateChangeHandler : IDomainEventHandler<GeneralStateChangeEvent>
    {
        private readonly IGeneralStateHistoryService stateService;
        private readonly IDomainService<GeneralStateHistory> historyDomain;

        public GeneralStateChangeHandler(
            IGeneralStateHistoryService stateService,
            IDomainService<GeneralStateHistory> historyDomain)
        {
            this.stateService = stateService;
            this.historyDomain = historyDomain;
        }

        /// <inheritdoc />
        public void Handle(GeneralStateChangeEvent args)
        {
            this.historyDomain.Save(this.stateService.CreateStateHistory(args.Entity, args.OldValue, args.NewValue, args.PropertyName));
        }
    }
}