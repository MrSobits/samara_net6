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
    /// Партиционирование больших таблиц и связанных с ними
    /// </summary>
    public class CreateViewForRecalcHistoryCharge : BaseExecutionAction
    {
        /// <summary>
        /// Таймаут на команду
        /// </summary>
        private readonly int CommandTimeOut = 3600 * 1;

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => this.Name;

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "Рефакторинг - Создание материализованного представления для regop_recalc_history с перерасчетами по тарифам";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var container = ApplicationContext.Current.Container;
            var sessions = container.Resolve<ISessionProvider>();
            using (var connection = sessions.OpenStatelessSession().Connection)
            {
                using (var tr = connection.BeginTransaction())
                {
                    try
                    {
                        //создаем материализованное представление для перерасчетов по базовому тарифу и тарифу решения
                        var sql = @"
                        CREATE MATERIALIZED VIEW public.regop_recalc_history_charge 
                        AS SELECT * FROM regop_recalc_history WHERE recalc_type in (10,20)
                        AND calc_period_id<(SELECT MAX(id) FROM regop_period WHERE cis_closed=false)
                        WITH DATA;

                        CREATE INDEX  ON regop_recalc_history_charge USING btree (recalc_period_id);
                        CREATE INDEX ON regop_recalc_history_charge USING btree (account_id);
                        CREATE INDEX  ON regop_recalc_history_charge USING btree (calc_period_id);
                        CREATE UNIQUE INDEX  ON regop_recalc_history_charge USING btree (id);   
    
                        ANALYZE public.regop_recalc_history_charge; ";
                        connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);
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