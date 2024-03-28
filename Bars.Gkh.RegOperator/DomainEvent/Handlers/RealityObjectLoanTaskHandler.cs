namespace Bars.Gkh.RegOperator.DomainEvent.Handlers
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.RegOperator.DomainEvent.Events.RealityObjectLoan;

    public class RealityObjectLoanTaskHandler
        : IDomainEventHandler<RealityObjectLoanTaskEndEvent>,
          IDomainEventHandler<RealityObjectLoanTaskStartEvent>
    {
        private readonly ISessionProvider sessionProvider;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="sessionProvider">Контейнер</param>
        public RealityObjectLoanTaskHandler(ISessionProvider sessionProvider)
        {
            this.sessionProvider = sessionProvider;
        }

        /// <inheritdoc />
        public void Handle(RealityObjectLoanTaskEndEvent args)
        {
            this.sessionProvider.InStatelessTransaction(session =>
            {
                session.CreateSQLQuery($"DELETE FROM REGOP_RO_LOAN_TASK WHERE RO_ID = {args.RealityObject.Id}").ExecuteUpdate();
            });
        }

        /// <inheritdoc />
        public void Handle(RealityObjectLoanTaskStartEvent args)
        {
            if (args.Task.Id > 0)
            {
                this.sessionProvider.InStatelessTransaction(session =>
                {
                    session.CreateSQLQuery($"INSERT INTO REGOP_RO_LOAN_TASK (ro_id, task_id) VALUES ({args.RealityObject.Id}, {args.Task.Id})").ExecuteUpdate();
                });
            }
        }
    }
}