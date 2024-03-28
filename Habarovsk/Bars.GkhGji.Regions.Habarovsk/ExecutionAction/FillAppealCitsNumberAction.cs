namespace Bars.GkhGji.Regions.Habarovsk.ExecutionAction
{
    using System;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.ExecutionAction.Impl;
    using Dapper;

    /// <summary>
    /// ГЖИ - Заполнение номера обращений (Воронеж)
    /// </summary>
    public class FillAppealCitsNumberAction : BaseMandatoryExecutionAction
    {
        private const int CommandTimeOut = 3600 * 5;

        /// <inheritdoc />
        public override string Code => nameof(FillAppealCitsNumberAction);

        /// <inheritdoc />
        public override string Description => "ГЖИ - Действие заполняет поле Number таблицы обращений GJI_APPEAL_CITIZENS";

        /// <inheritdoc />
        public override string Name => "ГЖИ - Заполнение номера обращений";

        /// <inheritdoc />
        public override Func<IDataResult> Action => this.Execute;

        private const string Sql = @"update GJI_APPEAL_CITIZENS set num=substring(document_number from '^(\d+)');";

        /// <summary>
        /// Поставщик сессиий
        /// </summary>
        public ISessionProvider SessionProvider { get; set; }

        private BaseDataResult Execute()
        {
            using (var session = this.SessionProvider.OpenStatelessSession())
            {
                var connection = session.Connection;

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Execute(
                            FillAppealCitsNumberAction.Sql,
                            transaction: transaction,
                            commandTimeout: FillAppealCitsNumberAction.CommandTimeOut);
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