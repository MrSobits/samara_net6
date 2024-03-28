namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.ExecutionAction.Impl;

    using Dapper;

    /// <summary>
    /// Проставление target_coef=-1 трансферам зачета средств за ранее выполненные работы
    /// </summary>
    public class PerfWorkDistrTargetCoefAction : BaseExecutionAction
    {
        /// <summary>
        /// Код действия
        /// </summary>
        public static string ActionCode = "PerfWorkDistrTargetCoefAction";

        public override string Code => PerfWorkDistrTargetCoefAction.ActionCode;

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Проставление target_coef=-1 трансферам зачета средств за ранее выполненные работы";

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "РегОператор - Исправление трансферов зачета средств за ранее выполненные работы";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => PerfWorkDistrTargetCoefAction.Execute;

        private static BaseDataResult Execute()
        {
            var container = ApplicationContext.Current.Container;

            ISessionProvider sessions = container.Resolve<ISessionProvider>();

            using (var connection = sessions.OpenStatelessSession().Connection)
            {
                using (var tr = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Execute(
                            @"                 
                            update regop_transfer
                            set target_coef = -1
                            where reason like 'Зачет средств за выполненные работы';",
                            transaction: tr,
                            commandTimeout: 10000);
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }

                    tr.Commit();
                }
            }

            return new BaseDataResult();
        }
    }
}