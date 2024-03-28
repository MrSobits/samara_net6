#define PG

namespace Bars.Gkh.Gis.KP_legacy
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Text.RegularExpressions;

    using Bars.B4.Utils;

    using Npgsql;

    public static class DBManager
    {
        /// <summary>
        /// [Informix only]
        /// Поключиться к базе данных
        /// </summary>
        [Flags]
        public enum ConnectToDb
        {
            /// <summary>
            /// База данных App сервера
            /// </summary>
            Host = 1,

            /// <summary>
            /// База данных Web сервера
            /// Не используется для PostgreSQL
            /// </summary>
            Web = 2
        }

        /// <summary>
        /// Аргументы создания таблицы
        /// </summary>
        public enum CreateTableArgs
        {
            None = 0,

            /// <summary>
            /// Создать таблицу, если она не существует
            /// </summary>
            CreateIfNotExists = 1,

            /// <summary>
            /// Удалить таблицу, если она уже существует и создать новую
            /// </summary>
            DropIfExists = 2
        }

        /// <summary>
        /// кол-во затронутых строк при выполнении запроса
        /// </summary>
        [ThreadStatic]
        public static int _affectedRowsCount = -100;

        /// <summary>Создать sql комманду</summary>
        /// <param name="sqlString">sql запрос</param>
        /// <param name="connection">Подключение</param>
        /// <param name="transaction">Транзакция</param>
        /// <returns>Sql комманда</returns>
        public static IDbCommand newDbCommand(string sqlString, IDbConnection connection, IDbTransaction transaction = null)
        {
            // какое подключение передали, то такую команду и возвращаем
            //#if DEBUG

            if (Points.FullLogging && sqlString.IndexOf("Select 1 From " + DBManager.sDefaultSchema + "calc_fon_", StringComparison.Ordinal) < 0
                && sqlString.IndexOf("Select * From " + DBManager.sDefaultSchema + "calc_fon_", StringComparison.Ordinal) < 0
                && sqlString.IndexOf("Select *  From " + DBManager.sDefaultSchema + "bill_fon", StringComparison.Ordinal) < 0)
            {
                
            }
            //#endif

#if PG
            if (connection.GetType() == typeof(NpgsqlConnection))
            {
                return new NpgsqlCommand(sqlString, (NpgsqlConnection)connection, (NpgsqlTransaction)transaction);
            }
            else if (connection.GetType() == typeof(MyDbConnection))
            {
                return new NpgsqlCommand(sqlString, (NpgsqlConnection)(((MyDbConnection)connection).RealConnection), (NpgsqlTransaction)transaction);
            }
            else if (connection.GetType().Name == "DatabaseConnection")
            {
                var command = connection.CreateCommand();
                command.CommandText = sqlString;
                command.Transaction = transaction;
                command.CommandType = CommandType.Text;
                return command;
            }
#else
#endif
            return null;
        }
        
#if PG
#else
        /// <summary>
        /// Время ожидания снятия блокировки с таблицы
        /// </summary>
        public const int WaitingTimeout = 5;
#endif

        public const string ConfPref = "W";

        public const bool LongType = false;

        /// <summary>
        /// Разделитель наименования базы данных (схемы) и таблицы
        /// </summary>
        public static string tableDelimiter
        {
            get
            {
#if PG
                return ".";
#else
                return ":";
#endif
            }
        }

        /// <summary>
        /// ключевые слова для перевода Informix в PostgreSQL
        /// </summary>
#if PG
        public const string sKernelAliasRest = "_kernel.";
        public const string sDataAliasRest = "_data.";
        public const string sUploadAliasRest = "_upload.";
        public const string sSupgAliasRest = "_supg.";
        public const string sDebtAliasRest = "_debt.";
        public const string tbluser = "";
        public const string sDecimalType = "numeric";
        public const string sCharType = "character";
        public const string sUniqueWord = "distinct";
        public const string sNvlWord = "coalesce";
        public const string sConvToNum = "::numeric";
        public const string sConvToInt = "::int";
        public const string sConvToChar = "::character";
        public const string sConvToChar10 = "::character(10)";
        public const string sConvToVarChar = "::varchar";
        public const string sConvToDate = "::date";
        public const string sDefaultSchema = "public.";
        public const string sUnloggedSchema = "unlogged_calc.";
        public const string sPaspSchema = "pasp.";
        public const string s0hour = "interval '0 hour'";
        public const string sUpdStat = "analyze";
        public const string sCrtTempTable = "temp";
        public const string sUnlogTempTable = "";
        public const string sCurDate = "current_date";
        public const string sCurDateTime = "now()";
        public const string DateNullString = "Null::date";
        public const string sFirstWord = "limit";
        public const string sSerialDefault = "default";
        public const string sYearFromDate = "Extract(year from ";
        public const string sMonthFromDate = "Extract(month from ";
        public const string sDateTimeType = "timestamp";
        public const string sLockMode = "";
        public const string sMatchesWord = "similar to";
        public const string sRegularExpressionAnySymbol = "%";
        public const string Limit1 = " limit 1 ";
        public const string sFirst1 = "";
        public const string sLimit1 = " limit 1 ";

#else
        public const string sKernelAliasRest = "_kernel:";
        public const string sDataAliasRest = "_data:";
        public const string sUploadAliasRest = "_upload:";
        public const string tbluser = "are.";
        public const string sDecimalType = "decimal";
        public const string sCharType = "char";
        public const string sUniqueWord = "unique";
        public const string sNvlWord = "nvl";
        public const string sConvToNum = "+0";
        public const string sConvToInt = "+0";
        public const string sConvToChar = "";
        public const string sConvToVarChar = "";
        public const string sConvToDate = "";
        public const string sDefaultSchema = "";
        public const string s0hour = "0 units hour";
        public const string sUpdStat = "Update statistics for table";
        public const string sCrtTempTable = "temp";
        public const string sUnlogTempTable = " with no log";
        public const string sCurDate = "today";
        public const string sCurDateTime = "current";
        public const string DateNullString = "''";
        public const string sUnloggedSchema = "unlogged_calc.";
        public const string sFirstWord = "first";
        public const string sSerialDefault = "0";
        public const string sYearFromDate = "year(";
        public const string sMonthFromDate = "month(";
        public const string sDateTimeType = "datetime year to second";
        public const string sLockMode = " lock mode row";
        public const string sMatchesWord = "matches";
        public const string sRegularExpressionAnySymbol = "*";
        public const string sFirst1 = " first 1 ";
        public const string sLimit1 = "";
#endif

        public static T ExecScalar<T>(IDbConnection connection, IDbTransaction transaction, string sql, bool inlog) where T : struct
        {
            Returns ret;
            var obj = DBManager.ExecScalar(connection, transaction, sql, out ret, inlog);
            return obj == null || obj == DBNull.Value ?
                default(T) : (T)Convert.ChangeType(obj, typeof(T));
        }

        public static object ExecScalar(IDbConnection connection, IDbTransaction transaction, string sql,
            out Returns ret, bool inlog)
        {
            return DBManager.ExecScalar(connection, transaction, sql, out ret, inlog, 300);
        }

        public static object ExecScalar(IDbConnection connection, IDbTransaction transaction, string sql, out Returns ret, bool inlog, int timeout)
        {
            ret = Utils.InitReturns();

            IDbCommand cmd = null;

            try
            {
                cmd = DBManager.newDbCommand(sql, connection, transaction);
                cmd.CommandTimeout = timeout;
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                ret.result = false;
                ret.text = "Ошибка одиночного чтения из базы данных ";
                ret.sql_error = " БД " + connection.Database + " \n '" + sql + "' \n " + ex.Message + " " +
                    DBManager.GetIfxError(ex);
                string err = ret.text + " \n " + ret.sql_error;


                if (inlog)
                {
                    StackTrace stackTrace = new StackTrace();           // get call stack
                    StackFrame[] stackFrames = stackTrace.GetFrames();  // get method calls (frames)

                    // write call stack method names
                    foreach (StackFrame stackFrame in stackFrames)
                    {
                        if (stackFrame.GetMethod().Name.Trim() == "Invoke") break;
                        err += stackFrame.GetMethod().Name + " \n"; // write method name
                    }

                    //#if DEBUG
                    //#endif
                    //string stateObject = String.Format("{0}Состояние объектов: connection.State:{1} ",
                    //    Environment.NewLine, connection.State.ToString());
                    //MonitorLog.WriteLog(stateObject, MonitorLog.typelog.Warn, true);
                }

                if (Constants.Viewerror)
                {
                    ret.text = err;
                }

                return null;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
            }
        }

        /// <summary>
        /// Очистка состояния подключения перед его отдачей в пул
        /// </summary>
        /// <param name="conn">Подключение</param>
        public static void ClearConnection(IDbConnection conn)
        {
            //ExecSQL(conn, "DISCARD ALL", true);
        }

        /// <summary>
        ///  Возвращает содержимо запроса в таблицу
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql">SQL Запрос</param>
        /// <returns>Таблицу с заполненными данными по запросу</returns>
        public static DataTable ExecSQLToTable(IDbConnection connection, string sql)
        {
            return DBManager.ExecSQLToTable(connection, sql, 300);
        }

        /// <summary>
        ///  Возвращает содержимо запроса в таблицу
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql">SQL Запрос</param>
        /// <param name="timeout">Время ожидания</param>
        /// <returns>Таблицу с заполненными данными по запросу</returns>
        public static DataTable ExecSQLToTable(IDbConnection connection, string sql, int timeout)
        {


            DataTable Data_Table = new DataTable();

            IDbCommand cmd = null;
            IDataReader reader = null;
            string err = String.Empty;

            try
            {
                cmd = DBManager.newDbCommand(sql, connection);
                cmd.CommandTimeout = timeout;
                reader = cmd.ExecuteReader();
                Utils.setCulture();
                if (reader != null) Data_Table.Load(reader, LoadOption.OverwriteChanges);
            }
            catch (Exception ex)
            {
                err = " Ошибка чтения из базы данных  \n " +
                      " БД " + connection.Database + " \n '" + sql + "' \n " + ex.Message +
                      " " + DBManager.GetIfxError(ex);

                StackTrace stackTrace = new StackTrace();           // get call stack
                StackFrame[] stackFrames = stackTrace.GetFrames();  // get method calls (frames)

                // write call stack method names
                foreach (StackFrame stackFrame in stackFrames)
                {
                    if (stackFrame.GetMethod().Name.Trim() == "Invoke") break;
                    err += stackFrame.GetMethod().Name + " \n"; // write method name
                }

            }
            finally
            {
                if (reader != null) reader.Dispose();
                if (cmd != null) cmd.Dispose();

            }
            if (err != String.Empty)
                throw new Exception("Ошибка чтения из базы данных ");

            return Data_Table;




        }

        public static string MDY(int month, int day, int year)
        {
#if PG
            return string.Format(" '{0}-{1}-{2}'::timestamp ", year, month, day);
#else
            return string.Format(" mdy({0},{1},{2}) ", month, day, year);
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        static private string GetIfxError(Exception ex)
        {
            string _InformixException = "";
#if PG
#else
            if (ex is IfxException)
            {
                if ((ex as IfxException).Errors.Count > 0)
                {
                    IfxError ifxErr = (ex as IfxException).Errors[0];

                    _InformixException = Environment.NewLine + "-------------------------[Informix Error]-------------------------" + Environment.NewLine +
                    "Message :" + ifxErr.Message + Environment.NewLine +
                    "Native error :" + ifxErr.NativeError + Environment.NewLine +
                    "SQL state :" + ifxErr.SQLState +
                    Environment.NewLine;
                }
            }
#endif
            return _InformixException;
        }

        #region Проверка существования 

        /// <summary>
        /// Проверка наличия таблицы
        /// </summary>
        /// <param name="connection">соединение</param>
        /// <param name="transaction">транзакция</param>
        /// <param name="tab">полное имя таблицы с указанием схемы</param>
        /// <returns></returns>
        public static bool TableExists(IDbConnection connection, IDbTransaction transaction, string tab)
        {
            string sql = string.Format("SELECT to_regclass('{0}') IS NOT NULL; ", tab);
            try
            {
                var version = (connection as NpgsqlConnection)?.PostgreSqlVersion;
                version = version ?? (connection as MyDbConnection)?.RealConnection.CastAs<NpgsqlConnection>().PostgreSqlVersion;
                if (version < new Version(9, 4))//для версий postgresql младше 9.4
                {
                    sql = string.Format("SELECT {1}to_regclass('{0}') IS NOT NULL; ", tab, DBManager.sDefaultSchema);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return DBManager.ExecScalar<bool>(connection, transaction, sql, false);
        }

        /// <summary>
        /// Проверка наличия колонки
        /// </summary>
        /// <param name="connection">соединение</param>
        /// <param name="transaction">транзакция</param>
        /// <param name="tab">полное имя таблицы с указанием схемы</param>
        /// <param name="column">колонка</param>
        /// <returns></returns>
        public static bool ColumnExists(IDbConnection connection, IDbTransaction transaction, string tab, string column)
        {
            var sql = string.Format(" SELECT TRUE " +
                                    " FROM pg_attribute " +
                                    " WHERE  attrelid = '{0}'::regclass" +
                                    " AND    attname = '{1}' AND NOT attisdropped ", tab, column);
            return DBManager.ExecScalar<bool>(connection, transaction, sql, false);
        }

        /// <summary>
        /// Проверка наличия схемы
        /// </summary>
        /// <param name="schema">схема</param>
        /// <param name="connection">соединение</param>
        /// <param name="transaction">транзакция</param>
        /// <returns></returns>
        public static bool SchemaExists(string schema, IDbConnection connection, IDbTransaction transaction = null)
        {
            var sql = string.Format("SELECT count(1)>0 FROM pg_namespace WHERE nspname = '{0}'", schema);
            return DBManager.ExecScalar<bool>(connection, transaction, sql, false);
        }

        #endregion Проверка существования 

        private static readonly List<string> DictForTurnOff = new List<string> { "INDEX", "ANALYZE" };
        private static readonly Regex patternForTurnOff = new Regex("(" + string.Join(")|(", DBManager.DictForTurnOff) + ")", RegexOptions.IgnoreCase);
    }
}