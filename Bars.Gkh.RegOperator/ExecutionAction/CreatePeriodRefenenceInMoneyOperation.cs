namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.ExecutionAction.Impl;

    using Dapper;

    /// <summary>
    /// РегОператор - Добавление периода в MoneyOperation
    /// </summary>
    public class CreatePeriodRefenenceInMoneyOperation : BaseExecutionAction
    {
        /// <summary>
        /// Таймаут на команду
        /// </summary>
        private const int CommandTimeOut = 3600 * 5;

        /// <inheritdoc />
        public override string Code => nameof(CreatePeriodRefenenceInMoneyOperation);

        /// <inheritdoc />
        public override string Description => "Действие добавляет и заполняет атрибут period_id таблицы regop_money_operation";

        /// <inheritdoc />
        public override string Name => "РегОператор - Добавление периода в MoneyOperation";

        /// <inheritdoc />
        public override Func<IDataResult> Action => this.Execute;

        /// <summary>
        /// Домен-сервис <see cref="ChargePeriod" />
        /// </summary>
        public IDomainService<ChargePeriod> ChargePeriodDomain { get; set; }

        /// <summary>
        /// Поставщик сессиий
        /// </summary>
        public ISessionProvider SessionProvider { get; set; }

        private BaseDataResult Execute()
        {
            var periodIds = this.ChargePeriodDomain.GetAll().Select(x => x.Id).ToArray();

            using (var connection = this.SessionProvider.OpenStatelessSession().Connection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var sql = @"delete from regop_bank_stmnt_op
                        where not exists(SELECT NULL from regop_transfer tr where tr.op_id = regop_bank_stmnt_op.op_id);";

                        // здесь таймаут больше, т.к. слабые сервера бд не успевают выполнить запрос после партиционирования трансферов
                        connection.Execute(sql, transaction: transaction, commandTimeout: CreatePeriodRefenenceInMoneyOperation.CommandTimeOut * 3);

                        sql = @"DROP INDEX idx_regop_money_op_g;
                            DROP INDEX idx_regop_money_op_o_g;
                            DROP INDEX ind_regop_money_op_doc;
                            DROP INDEX ind_regop_money_op_op_nnull;
                            DROP INDEX ind_regop_money_op_op_null;";
                        connection.Execute(sql, transaction: transaction, commandTimeout: CreatePeriodRefenenceInMoneyOperation.CommandTimeOut);

                        sql = @"ALTER TABLE regop_money_operation ADD COLUMN period_id BIGINT;";
                        connection.Execute(sql, transaction: transaction, commandTimeout: CreatePeriodRefenenceInMoneyOperation.CommandTimeOut);

                        sql = @"ALTER TABLE regop_money_operation
                        ADD CONSTRAINT fk_money_operation_period_id
                        FOREIGN KEY  (period_id) REFERENCES regop_period (id)
                        MATCH SIMPLE
                        ON UPDATE NO ACTION ON DELETE NO ACTION;";
                        connection.Execute(sql, transaction: transaction, commandTimeout: CreatePeriodRefenenceInMoneyOperation.CommandTimeOut);

                        var tableExistsQuery = @"SELECT EXISTS (
                       SELECT 1
                       FROM   information_schema.tables
                       WHERE  table_schema = 'public'
                       AND    table_name = 'regop_transfer_period_{0}');";

                        foreach (var periodId in periodIds)
                        {
                            var tableExists = connection.ExecuteScalar<bool>(
                                tableExistsQuery.FormatUsing(periodId),
                                transaction: transaction,
                                commandTimeout: CreatePeriodRefenenceInMoneyOperation.CommandTimeOut);
                            if (tableExists)
                            {
                                sql =
                                    $@"UPDATE regop_money_operation
                                SET period_id = {periodId}
                                FROM (SELECT op_id FROM regop_transfer_period_{periodId}) q where q.op_id = id";
                                connection.Execute(sql, transaction: transaction, commandTimeout: CreatePeriodRefenenceInMoneyOperation.CommandTimeOut);
                            }
                        }

                        sql = @"update regop_money_operation mo
                          set period_id = q.period_id
                        FROM (
                          SELECT ml.operation_id, p.id period_id FROM regop_money_lock ml
                            join regop_period p on ml.lock_date::date BETWEEN p.cstart::date and coalesce(p.cend, 'infinity')) q
                        where q.operation_id = mo.id;";
                        connection.Execute(sql, transaction: transaction, commandTimeout: CreatePeriodRefenenceInMoneyOperation.CommandTimeOut);

                        sql = @"update regop_money_operation mo
                            set period_id = q.period_id
                        FROM (
                            SELECT ml.cancel_operation_id, p.id period_id FROM regop_money_lock ml
                            join regop_period p on ml.unlock_date::date BETWEEN p.cstart::date and coalesce(p.cend, 'infinity')
                            where cancel_operation_id is not null) q
                        where q.cancel_operation_id = mo.id";
                        connection.Execute(sql, transaction: transaction, commandTimeout: CreatePeriodRefenenceInMoneyOperation.CommandTimeOut);

                        sql = @"CREATE UNIQUE INDEX idx_regop_money_op_g ON regop_money_operation(op_guid)";
                        connection.Execute(sql, transaction: transaction, commandTimeout: CreatePeriodRefenenceInMoneyOperation.CommandTimeOut);

                        sql = @"CREATE INDEX idx_regop_money_op_o_g ON regop_money_operation(originator_guid)";
                        connection.Execute(sql, transaction: transaction, commandTimeout: CreatePeriodRefenenceInMoneyOperation.CommandTimeOut);

                        sql = @"CREATE INDEX ind_regop_money_op_doc ON regop_money_operation(document_id)";
                        connection.Execute(sql, transaction: transaction, commandTimeout: CreatePeriodRefenenceInMoneyOperation.CommandTimeOut);

                        sql = @"CREATE INDEX ind_regop_money_op_op_nnull ON regop_money_operation(canceled_op_id) WHERE canceled_op_id IS NOT NULL";
                        connection.Execute(sql, transaction: transaction, commandTimeout: CreatePeriodRefenenceInMoneyOperation.CommandTimeOut);

                        sql = @"CREATE INDEX ind_regop_money_op_op_null ON regop_money_operation(canceled_op_id) WHERE canceled_op_id IS NULL";
                        connection.Execute(sql, transaction: transaction, commandTimeout: CreatePeriodRefenenceInMoneyOperation.CommandTimeOut);

                        sql = @"CREATE INDEX ind_money_op_p
                        ON regop_money_operation
                        USING btree
                        (period_id);";
                        connection.Execute(sql, transaction: transaction, commandTimeout: CreatePeriodRefenenceInMoneyOperation.CommandTimeOut);

                        sql = "ANALYSE regop_money_operation;";
                        connection.Execute(sql, transaction: transaction, commandTimeout: CreatePeriodRefenenceInMoneyOperation.CommandTimeOut);

                        sql = "DELETE FROM regop_money_operation WHERE period_id is NULL;";
                        connection.Execute(sql, transaction: transaction, commandTimeout: CreatePeriodRefenenceInMoneyOperation.CommandTimeOut);

                        sql = "ALTER TABLE regop_money_operation ALTER COLUMN period_id SET NOT NULL;";
                        connection.Execute(sql, transaction: transaction, commandTimeout: CreatePeriodRefenenceInMoneyOperation.CommandTimeOut);

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return new BaseDataResult();
        }
    }
}