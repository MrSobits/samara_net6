using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Dapper;
using Npgsql;

namespace Bars.Gkh.SqlExecutor
{
    using System.Text;

    using Bars.B4.Utils;

    /// <summary>
    /// Класс для выполнения sql-запросов
    /// </summary>
    public class SqlExecutor : IDisposable
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="connectionString">Строка подключения к БД</param>
        public SqlExecutor(string connectionString)
        {
            if (connectionString.IsEmpty())
            {
                throw new Exception("Не указана строка для подключения к БД");
            }

            Connection = new NpgsqlConnection(connectionString);
            OpenConnection();
        }

        /// <summary>
        /// Соединение с БД
        /// </summary>
        private IDbConnection Connection { get; set; }
        
        /// <summary>
        /// Создание нового запроса 
        /// </summary>
        /// <param name="commandText">Текст команды</param>
        /// <param name="connection">Соединение</param>
        /// <param name="transaction">Транзакция</param>
        /// <param name="timeout">Таймаут времени выполнения</param>
        private IDbCommand NewDbCommand(string commandText, IDbConnection connection, IDbTransaction transaction,
            int timeout)
        {
            var dbCommand = connection.CreateCommand();
            dbCommand.CommandText = commandText;
            dbCommand.Connection = connection;
            dbCommand.Transaction = transaction;
            dbCommand.CommandTimeout = timeout;
            return dbCommand;
        }

        /// <summary>
        /// Открытие транзакции
        /// </summary>
        /// <returns></returns>
        public IDbTransaction BeginTransaction()
        {
            OpenConnection();
            return this.Connection.BeginTransaction();
        }

        /// <summary>
        /// Открытие соединения с БД
        /// </summary>
        /// <returns></returns>
        private void OpenConnection()
        {
            try
            {
                switch (this.Connection.State)
                {
                    case ConnectionState.Closed:
                        Connection.Open();
                        break;

                    case ConnectionState.Broken:
                        Connection.Close();
                        Connection.Open();
                        break;
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Ошибка при открытии соединения с БД! ", exception);
            }
        }

        /// <summary>
        /// Закрытие соединения с БД
        /// </summary>
        /// <returns></returns>
        private void CloseConnection()
        {
            try
            {
                if (this.Connection != null && this.Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Ошибка при закрытии соединения с БД!", exception);
            }
        }

        /// <summary>
        /// Выполнение sql-запроса без транзакции
        /// </summary>
        /// <param name="sqlQuery">Текст sql-запроса</param>
        /// <returns></returns>
        public void ExecuteSql(string sqlQuery)
        {
            ExecuteSql(sqlQuery, this.Connection, null, 3600);
        }

        /// <summary>
        /// Выполнение sql-запроса c транзакцией
        /// </summary>
        /// <param name="sqlQuery">Текст sql-запроса</param>
        /// <param name="transaction">Транзакция</param>
        /// <returns></returns>
        public void ExecuteSql(string sqlQuery, IDbTransaction transaction)
        {
            ExecuteSql(sqlQuery, this.Connection, transaction, 3600);
        }

        /// <summary>
        /// Выполнение sql-запроса
        /// </summary>
        /// <param name="sqlQuery">Текст sql-запроса</param>
        /// <param name="connection">Соединение</param>
        /// <param name="transaction">Транзакция</param>
        /// <param name="dbCommandTimeout">Таймаут выполнения</param>
        /// <returns></returns>
        private void ExecuteSql(string sqlQuery, IDbConnection connection, IDbTransaction transaction, int dbCommandTimeout)
        {
            try
            {
                OpenConnection();

                using (IDbCommand myCommand = NewDbCommand(sqlQuery, connection, transaction, dbCommandTimeout))
                {
                    myCommand.ExecuteNonQuery();
                }

            }
            catch (Exception exception)
            {
                if (transaction != null) 
                    transaction.Rollback();
                throw new Exception(
                    String.Format(" Ошибка при выполнении sql-запроса! \n Текст sql-запроса: {0}\n Текст ошибки: {1} ", sqlQuery, exception.Message), exception);
            }
        }

        /// <summary>
        /// Выполнить запрос со стандартной обработкой ошибок
        /// </summary>
        public IEnumerable<T> ExecuteSql<T>(string sqlQuery)
        {
            try
            {
                return this.ExecuteSqlWithOutErrorHandler<T>(sqlQuery);
            }
            catch (Exception exception)
            {
                throw new Exception(
                    String.Format(" Ошибка при выполнении sql-запроса! \n Текст sql-запроса: {0}\n Текст ошибки: {1} ", sqlQuery, exception.Message), exception);
            }
        }

        /// <summary>
        /// Выполнить запрос без обработки ошибок
        /// </summary>
        public void ExecuteSqlWithOutErrorHandler(string sqlQuery)
        {
            this.OpenConnection();
            this.Connection.Execute(sqlQuery);
        }

        /// <summary>
        /// Выполнить запрос без обработки ошибок
        /// </summary>
        public IEnumerable<T> ExecuteSqlWithOutErrorHandler<T>(string sqlQuery)
        {
            this.OpenConnection();
            return this.Connection.Query<T>(sqlQuery);
        }

        /// <summary>
        /// Выполнение запроса с получением результата
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sqlQuery)
        {
            try
            {
                OpenConnection();
                return this.Connection.ExecuteScalar<T>(sqlQuery);

            }
            catch (Exception exception)
            {
                throw new Exception(
                    String.Format(" Ошибка при выполнении sql-запроса! \n Текст sql-запроса: {0}\n Текст ошибки: {1} ", sqlQuery, exception.Message), exception);
            }
        }

        /// <summary>
        /// Выполнение SQL с возвратом данных типа IDataReader
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public DataTable ExecSqLtoTable(string sqlQuery)
        {

            var resTable = new DataTable();

            try
            {
                OpenConnection();

                using (IDbCommand myCommand = NewDbCommand(sqlQuery, Connection, null, 3600))
                {
                    resTable.Load(myCommand.ExecuteReader());
                }
            }
            catch (Exception exception)
            {
                throw new Exception(
                    String.Format(" Ошибка при выполнении sql-запроса! \n Текст sql-запроса: {0}\n Текст ошибки: {1} ", sqlQuery, exception.Message), exception);
            }

            return resTable;
        }


        /// <summary>
        /// Передача потока напрямую в БД
        /// </summary>
        /// <param name="sqlQuery">Sql-запрос (copy ... from stdin )</param>
        /// <param name="stream">Поток</param>
        public void CopyIn(string sqlQuery, Stream stream, Encoding encoding)
        {
            OpenConnection();
            
            using (var copier = (Connection as NpgsqlConnection).BeginTextImport(sqlQuery))
            {
                using (var sr = new StreamReader(stream, encoding))
                {
                    while (!sr.EndOfStream)
                    {
                        copier.WriteLine(sr.ReadLine());
                    }
                }
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            CloseConnection();
        }
    }
}
