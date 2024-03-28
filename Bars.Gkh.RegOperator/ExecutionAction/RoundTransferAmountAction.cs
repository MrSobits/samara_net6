namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Repositories.ChargePeriod;

    /// <summary>
    /// Округление суммы трансферов до двух знаков после запятой
    /// </summary>
    public class RoundTransferAmountAction : BaseExecutionAction
    {
        /// <summary>
        /// Код для регистрации
        /// </summary>
        public string Code => "RoundTransferAmountAction";

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Округление суммы трансферов, если после округления больше 0, то остается, если равен нулю - удаляется";

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "РегОператор - Округление суммы оплат трансферов";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var container = ApplicationContext.Current.Container;

            var periodRepo = container.Resolve<IChargePeriodRepository>();

            var currentPeriod = periodRepo.GetCurrentPeriod();

            if (currentPeriod == null)
            {
                return new BaseDataResult();
            }

            using (var session = container.Resolve<ISessionProvider>().GetCurrentSession())
            {
                session.CreateSQLQuery(@"
update regop_transfer set amount = round(amount, 2)
where operation_date >=:dateStart")
                    .SetDateTime("dateStart", currentPeriod.StartDate)
                    .ExecuteUpdate();

                session.CreateSQLQuery(@"
delete from regop_transfer
where amount=0 and operation_date >=:dateStart")
                    .SetDateTime("dateStart", currentPeriod.StartDate)
                    .ExecuteUpdate();
            }

            return new BaseDataResult();
        }
    }
}