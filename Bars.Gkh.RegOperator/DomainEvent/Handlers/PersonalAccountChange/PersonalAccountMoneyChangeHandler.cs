namespace Bars.Gkh.RegOperator.DomainEvent.Handlers
{
    using System;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccount;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;

    public class PersonalAccountMoneyChangeHandler : IDomainEventHandler<PersonalAccountPenaltyChargeUndoEvent>
    {
        private readonly IPersonalAccountHistoryCreator historyCreator;
        private readonly IDomainService<PersonalAccountChange> changeDomain;

        public PersonalAccountMoneyChangeHandler(
            IPersonalAccountHistoryCreator historyCreator,
            IDomainService<PersonalAccountChange> changeDomain)
        {
            this.historyCreator = historyCreator;
            this.changeDomain = changeDomain;
        }

        #region Implementation of IDomainEventHandler<in PersonalAccountPenaltyChargeUndoEvent>

        /// <summary>
        /// Обработать событие
        /// </summary>
        /// <param name="args">Аргумент-событие</param>
        public void Handle(PersonalAccountPenaltyChargeUndoEvent args)
        {
            if (args.OldPenalty != args.NewPenalty)
            {
                changeDomain.Save(
                    historyCreator.CreateChange(
                        args.Account,
                        PersonalAccountChangeType.PenaltyUndo,
                        "Отмена начисления пени",
                        args.NewPenalty.ToString(),
                        args.OldPenalty.ToString(),
                        DateTime.UtcNow,
                        args.Operation.Return(x => x.Document),
                        args.ChangeInfo.With(c => c.Reason)));
            }
        }

        #endregion
    }
}