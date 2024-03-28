namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Utils;

    /// <summary>
    /// Действие перерасчета исходящего сальдо для всех периодов по лс
    /// </summary>
    public class RecalcAllPeriodsSummarySaldoOutAction : BaseExecutionAction
    {
        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Перерасчет исходящего сальдо с округлением до двух знаков для всех периодов";

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "РегОператор - Перерасчет исходящего сальдо с округлением до двух знаков для всех периодов";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => RecalcAllPeriodsSummarySaldoOutAction.Execute;

        private static BaseDataResult Execute()
        {
            var container = ApplicationContext.Current.Container;
            try
            {
                var periods = container.ResolveDomain<ChargePeriod>().GetAll().ToArray();

                using (var ss = container.Resolve<ISessionProvider>().OpenStatelessSession())
                {
                    var action = new UpdateSaldoSqlAction(ss);

                    action.UpdatePaSaldos(periods);
                }
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }

            return new BaseDataResult();
        }
    }
}