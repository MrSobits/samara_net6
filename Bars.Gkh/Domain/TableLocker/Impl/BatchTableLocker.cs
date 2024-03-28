

namespace Bars.Gkh.Domain.TableLocker.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.Utils;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils.Annotations;

    using NHibernate.SqlCommand;

    /// <summary>
    /// Сервис массовой блокировки таблиц
    /// </summary>
    public class BatchTableLocker : TableLocker, IBatchTableLocker
    {
        protected const string BatchOracleIsLockedSelect = @"SELECT COUNT(*) FROM TABLE_LOCK WHERE";
        protected const string BatchOracleIsLockedWhere = @" (LOWER(TABLE_NAME) = LOWER('{0}') AND LOWER(ACTION) IN ({1}))";
        protected const string BatchPostgreIsLockedSelect = @"SELECT count(*) FROM table_lock WHERE";
        protected const string BatchPostgreIsLockedWhere = @" (lower(table_name) = lower('{0}') and lower(action) in ({1}))";

        private readonly IDictionary<string, List<string>> tables;
        private bool autoUnlock;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="provider">Провайдер сессии БД</param>
        public BatchTableLocker(ISessionProvider provider)
            : base(provider)
        {
            this.tables = new Dictionary<string, List<string>>();
        }

        /// <summary>
        /// Установка параметра autoUnlock
        /// </summary>
        /// <param name="auto">значение параметра</param>
        /// <returns>Сервис массовой блокировки таблиц</returns>
        public IBatchTableLocker AutoUnlock(bool auto)
        {
            this.autoUnlock = auto;
            return this;
        }

        /// <summary>
        /// Проверить блокировку
        /// </summary>
        /// <returns>Результат проверки</returns>
        public bool CheckLocked()
        {
            var builder = new SqlStringBuilder();
            builder.Add(this.IsOracle ? BatchOracleIsLockedSelect : BatchPostgreIsLockedSelect);

            var i = 0;
            foreach (var table in this.tables)
            {
                builder.Add(
                    string.Format(
                        this.IsOracle ? BatchOracleIsLockedWhere : BatchPostgreIsLockedWhere,
                        table.Key,
                        string.Join(", ", table.Value.Select(x => string.Format("'{0}'", x.ToLower())))));
                
                if (++i < this.tables.Count)
                {
                    builder.Add(" OR");
                }
            }
            
            return this.Session.CreateSQLQuery(builder.ToString()).UniqueResult().ToLong() > 0;
        }

        /// <summary>
        /// Очистка списка таблиц
        /// </summary>
        public void Clear()
        {
            this.tables.Clear();
        }

        /// <summary>
        /// Заблокировать
        /// </summary>
        /// <returns>Сервис массовой блокировки таблиц</returns>
        public IBatchTableLocker Lock()
        {
            using (var transaction = this.Session.BeginTransaction())
            {
                try
                {
                    foreach (var table in this.tables)
                    {
                        foreach (var action in table.Value)
                        {
                            this.LockInternal(table.Key, action);
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

            return this;
        }

        IBatchTableLocker IBatchTableLocker.ThrowOnAlreadyLocked(bool throwOn)
        {
            base.ThrowOnAlreadyLocked(throwOn);
            return this;
        }

        /// <summary>
        /// Разблокировать
        /// </summary>
        /// <returns>Сервис массовой блокировки таблиц</returns>
        public IBatchTableLocker Unlock()
        {
            using (var transaction = this.Session.BeginTransaction())
            {
                try
                {
                    foreach (var table in this.tables)
                    {
                        this.UnlockInternalPrepare(table.Key, table.Value);
                    }
                    UnlockInternalExecute();
                    ClearQueriesStorage();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return this;
        }

        /// <summary>
        /// Использовать таблицу и действие с ней
        /// </summary>
        /// <param name="tableName">Наименование таблицы</param>
        /// <param name="action">Действие</param>
        /// <returns>Сервис массовой блокировки таблиц</returns>
        public IBatchTableLocker With(string tableName, string action)
        {
            ArgumentChecker.NotNullOrEmpty(tableName, "tableName");
            ArgumentChecker.NotNullOrEmpty(action, "action");

            var actionKey = action.ToUpper();
            this.AssertValidAction(actionKey);

            if (!this.tables.ContainsKey(tableName))
            {
                this.tables[tableName] = new List<string>(3);
            }

            this.tables[tableName].Add(actionKey);

            return this;
        }

        /// <summary>
        /// Использовать таблицу и действие с ней
        /// </summary>
        /// <typeparam name="T">Тип объекта связанного с таблицей БД</typeparam>
        /// <param name="action">Действие</param>
        /// <returns>Сервис массовой блокировки таблиц</returns>
        public IBatchTableLocker With<T>(string action) where T : PersistentObject
        {
            return this.With(this.GetTableName(typeof(T)), action);
        }

        public IBatchTableLocker With(Type type, string action)
        {
            return this.With(this.GetTableName(type), action);
        }

        protected override void Dispose(bool disposing)
        {
            if (this.autoUnlock)
            {
                this.Unlock();
            }

            base.Dispose(disposing);
        }
    }
}