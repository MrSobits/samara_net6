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
    /// Проставление трансферам Установки/изменения сальдо is_affect=false
    /// </summary>
    public class FixChangeBalanceAction : BaseExecutionAction
    {
        /// <summary>
        /// Таймаут на команду
        /// </summary>
        private const int CommandTimeOut = 3600 * 5;

        /// <inheritdoc />
        public static string ActionCode = "FixChangeBalanceAction";

        /// <inheritdoc />
        public override string Code => FixChangeBalanceAction.ActionCode;

        /// <inheritdoc />
        public override string Description => "Проставление трансферам Установки/изменения сальдо is_affect=false";

        /// <inheritdoc />
        public override string Name => "РегОператор - Исправление трансферов Установки/изменения сальдо";

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
                                commandTimeout: FixChangeBalanceAction.CommandTimeOut);

                            if (tableExists)
                            {
                                var sql =
                                    $@"UPDATE regop_transfer_period_{periodId}
                                SET is_affect=false
                                WHERE reason like 'Установка/изменение сальдо' and is_affect;";
                                connection.Execute(sql, transaction: transaction, commandTimeout: FixChangeBalanceAction.CommandTimeOut);
                            }
                        }

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