namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Data;

    using Bars.B4;
    using Bars.B4.DataAccess;

    using Castle.Core.Internal;

    using Dapper;

    /// <summary>
    /// Действие выполняющее хранимую процедуру
    /// </summary>
    public class StoredProcedureExecutionAction : BaseExecutionAction
    {
        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "Выполнить хранимую процедуру";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Действие запускает хранимую процедуру с указанным именем";

        /// <summary>
        /// Поставщик сессий
        /// </summary>
        public ISessionProvider SessionProvider { get; set; }

        /// <summary>
        /// Таймаут на команду
        /// </summary>
        private readonly int CommandTimeOut = 3 * 60 * 60;

        private BaseDataResult Execute()
        {
            var procedureName = this.ExecutionParams.Params.GetAs<string>("ProcedureName");

            if (procedureName.IsNullOrEmpty())
            {
                return BaseDataResult.Error("Не указано название хранимой процедуры");
            }

            using (var session = this.SessionProvider.OpenStatelessSession())
            {
                var connection = session.Connection;
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Execute(procedureName, commandType: CommandType.StoredProcedure, transaction: transaction, commandTimeout: this.CommandTimeOut);

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