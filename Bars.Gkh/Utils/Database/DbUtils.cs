namespace Bars.Gkh.RegOperator.Utils
{
    using System;
    using System.Data;
    using System.Data.Common;

    using NHibernate;

    /// <summary>Утилитный класс дляя работы с БД</summary>
    public static class DbUtils
    {
        /// <summary>Выполнить функцию</summary>
        /// <typeparam name="T">Тип возвращаемого значения</typeparam>
        /// <param name="connection">подключение</param>
        /// <param name="transaction">Транзакция</param>
        /// <param name="resultType">Тип возвращаемого значения(DbType)</param>
        /// <param name="functionName">Имя функции</param>
        /// <param name="paramNames">Параметры</param>
        /// <param name="paramValues">Значения параметров</param>
        /// <returns>Результат</returns>
        public static T CallFunction<T>(IDbConnection connection, ITransaction transaction, DbType resultType, string functionName, string[] paramNames, object[] paramValues)
        {
            using (var cmd = connection.CreateCommand() as DbCommand)
            {
                cmd.CommandText = functionName;
                cmd.CommandType = CommandType.StoredProcedure;
                if (transaction != null && transaction.IsActive)
                {
                    transaction.Enlist(cmd);
                }

                var paramResult = cmd.CreateParameter();
                paramResult.Direction = ParameterDirection.Output;
                paramResult.DbType = resultType;
                cmd.Parameters.Add(paramResult);

                for (var i = 0; i < paramNames.Length; ++i)
                {
                    var parameter = cmd.CreateParameter();
                    parameter.ParameterName = paramNames[i];
                    parameter.Value = paramValues[i] ?? DBNull.Value;
                    cmd.Parameters.Add(parameter);
                }

                cmd.ExecuteNonQuery();

                var result = (T)Convert.ChangeType(paramResult.Value, typeof(T));

                return result;
            }
        }

        /// <summary>Выполнить процедуру</summary>
        /// <param name="connection">подключение</param>
        /// <param name="transaction">Транзакция</param>
        /// <param name="functionName">Имя функции</param>
        /// <param name="paramNames">Параметры</param>
        /// <param name="paramValues">Значения параметров</param>
        /// <returns>Результат</returns>
        public static void CallProcedure(IDbConnection connection, ITransaction transaction, string functionName, string[] paramNames, object[] paramValues)
        {
            using (var cmd = connection.CreateCommand() as DbCommand)
            {
                cmd.CommandText = functionName;
                cmd.CommandType = CommandType.StoredProcedure;
                if (transaction != null && transaction.IsActive)
                {
                    transaction.Enlist(cmd);
                }

                for (var i = 0; i < paramNames.Length; ++i)
                {
                    var parameter = cmd.CreateParameter();
                    parameter.ParameterName = paramNames[i];
                    parameter.Value = paramValues[i] ?? DBNull.Value;
                    cmd.Parameters.Add(parameter);
                }

                cmd.ExecuteNonQuery();
            }
        }
    }
}