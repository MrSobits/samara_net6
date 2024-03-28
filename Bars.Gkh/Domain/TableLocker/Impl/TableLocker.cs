namespace Bars.Gkh.Domain.TableLocker.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils.Annotations;

    using NHibernate;
    using NHibernate.Persister.Entity;

    /// <summary>
    ///     Сервис блокировки таблиц
    /// </summary>
    public class TableLocker : ITableLocker
    {
        private readonly ISessionProvider _provider;

        protected readonly bool IsOracle;

        private IStatelessSession _session;

        protected bool IsThrowOnAlreadyLocked = true;
        
        private StringBuilder _queriesStorageDropTriggers;

        private StringBuilder _queriesStorageDeleteRecordsFromTableLocker;

        /// <summary>
        ///     .ctor
        /// </summary>
        /// <param name="provider"></param>
        public TableLocker(ISessionProvider provider)
        {
            _provider = provider;
            
            IsOracle = provider.CurrentSession.Connection.GetType().Name.ToLower().Contains("oracle");
            _queriesStorageDropTriggers = new StringBuilder();
            _queriesStorageDeleteRecordsFromTableLocker = new StringBuilder();
        }

        /// <summary>
        ///     Личная сессия
        /// </summary>
        protected IStatelessSession Session
        {
            get
            {
                return _session != null && _session.IsOpen ? _session : (_session = _provider.OpenStatelessSession());
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
        }

        ~TableLocker()
        {
            Dispose(false);
        }

        #region SQL Consts

        #region Oracle

        protected const string OraIsTriggerExists =
            @"SELECT COUNT(*) FROM USER_OBJECTS WHERE OBJECT_TYPE = 'TRIGGER' AND UPPER(OBJECT_NAME) = UPPER(:triggerName)";

        protected const string OraCreateTrigger =
            @"CREATE TRIGGER {2} BEFORE {1} ON {0} FOR EACH ROW CALL TABLE_LOCKER('{0}', '{1}')";

        protected const string OraDropTrigger = @"DROP TRIGGER {0}";

        protected const string OraIsLocked =
            @"SELECT COUNT(*) FROM TABLE_LOCK WHERE LOWER(TABLE_NAME) = LOWER(:tableName) AND LOWER(ACTION) = LOWER(:action)";

        protected const string OraLock =
            @"INSERT INTO TABLE_LOCK(TABLE_NAME, ACTION, LOCK_START) VALUES(:tableName, :action, CURRENT_TIMESTAMP AT TIME ZONE 'UTC')";

        protected const string OraUnlock =
            @"DELETE FROM TABLE_LOCK WHERE TABLE_NAME = :tableName AND ACTION = :action";

        protected const string OraUnlockManyActionsForOneTable =
            @"DELETE FROM TABLE_LOCK WHERE TABLE_NAME = '{0}' {1}";

        #endregion

        #region Postgres

        protected const string PgIsTriggerExists =
            @"SELECT count(*) FROM information_schema.triggers WHERE event_object_table = lower(:tableName) AND lower(trigger_name) = lower(:triggerName)";

        protected const string PgCreateTrigger =
            @"CREATE TRIGGER {2} BEFORE {1} ON {0} FOR EACH ROW EXECUTE PROCEDURE table_locker()";

        protected const string PgDropTrigger = @"ALTER TABLE {1} DISABLE TRIGGER {0}; DROP TRIGGER IF EXISTS {0} ON {1}";

        protected const string PgIsLocked =
            @"SELECT count(*) FROM table_lock WHERE lower(table_name) = lower(:tableName) and lower(action) = lower(:action)";

        protected const string PgLock =
            @"INSERT INTO table_lock(table_name, action, lock_start) VALUES(:tableName, :action, current_timestamp AT TIME ZONE 'UTC')";

        protected const string PgUnlock =
            @"DELETE FROM table_lock WHERE table_name = :tableName and action = :action";

        protected const string PgUnlockForManyActionsOneTable =
            @"DELETE FROM table_lock WHERE table_name = '{0}' {1}";

        protected const string PgUnlockWhereActionsClause =
            @"action = '{0}'";

        #endregion

        #endregion

        #region ITableLocker Implementation

        public bool CheckLocked(string tableName, string action)
        {
            var a = action.ToUpper();
            AssertValidAction(a);

            return CheckLockedInternal(tableName, a);
        }

        public bool CheckLocked(Type type, string action)
        {
            return CheckLocked(GetTableName(type), action);
        }

        public bool CheckLocked<T>(string action)
        {
            return CheckLocked(GetTableName(typeof(T)), action);
        }

        public ITableLocker Lock(string tableName, string action)
        {
            ArgumentChecker.NotNullOrEmpty(tableName, "tableName");

            using (var tr = Session.BeginTransaction())
            {
                try
                {
                    LockInternal(tableName, action);

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }

            return this;
        }

        public ITableLocker Lock<T>(string action) where T : PersistentObject
        {
            return Lock(GetTableName(typeof(T)), action);
        }

        public ITableLocker Lock(Type type, string action)
        {
            return Lock(GetTableName(type), action);
        }

        public ITableLocker Unlock(string tableName, string action)
        {
            ArgumentChecker.NotNullOrEmpty(tableName, "tableName");
            ArgumentChecker.NotNull(action, "action");

            using (var tr = Session.BeginTransaction())
            {
                try
                {
                    UnlockInternal(tableName, action);

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }

            return this;
        }

        public ITableLocker Unlock<T>(string action) where T : PersistentObject
        {
            return Unlock(GetTableName(typeof(T)), action);
        }

        public ITableLocker Unlock(Type type, string action)
        {
            return Unlock(GetTableName(type), action);
        }

        public ITableLocker ThrowOnAlreadyLocked(bool throwOn)
        {
            IsThrowOnAlreadyLocked = throwOn;

            return this;
        }

        #endregion

        #region Internal

        /// <summary>
        ///     Уничтожение экземпляра
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_session != null && _session.IsOpen)
            {
                _session.Close();
            }

            if (disposing)
            {
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        ///     Проверка существования триггера на указанной таблице
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        protected bool IsTriggerExists(string tableName, string action)
        {
            return
                Session.CreateSQLQuery(IsOracle ? OraIsTriggerExists : PgIsTriggerExists)
                       .SetParameter("tableName", tableName)
                       .SetParameter("triggerName", GetTriggerName(tableName, action))
                       .List<long>()[0] > 0;
        }

        /// <summary>
        ///     Создать триггер при необходимости
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="action"></param>
        protected void TryCreateTrigger(string tableName, string action)
        {
            if (!IsTriggerExists(tableName, action))
            {
                Session.CreateSQLQuery(
                    string.Format(
                        IsOracle ? OraCreateTrigger : PgCreateTrigger,
                        tableName,
                        action,
                        GetTriggerName(tableName, action))).ExecuteUpdate();
            }
        }

        /// <summary>
        ///     Убрать триггер при необходимости
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="action"></param>
        protected void TryDropTrigger(string tableName, string action)
        {
            if (IsTriggerExists(tableName, action))
            {
                Session.CreateSQLQuery(
                    string.Format(
                        IsOracle ? OraDropTrigger : PgDropTrigger,
                        GetTriggerName(tableName, action),
                        tableName)).ExecuteUpdate();
            }
        }

        /// <summary>
        ///     Получить имя таблицы по типу сущности
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        protected string GetTableName(Type entityType)
        {
            return
                ((AbstractEntityPersister)Session.GetSessionImplementation().Factory.GetClassMetadata(entityType))
                    .TableName;
        }

        /// <summary>
        ///     Навесить блокировку
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="action"></param>
        protected void LockInternal(string tableName, string action)
        {
            var a = action.ToUpper();
            AssertValidAction(a);

            TryCreateTrigger(tableName, a);

            if (!CheckLockedInternal(tableName, a))
            {
                Session.CreateSQLQuery(IsOracle ? OraLock : PgLock)
                       .SetParameter("tableName", tableName.ToLower())
                       .SetParameter("action", a)
                       .ExecuteUpdate();
            }
            else
            {
                if (IsThrowOnAlreadyLocked)
                {
                    throw new InvalidOperationException(string.Format("Таблица уже заблокирована для {0}", a));
                }
            }
        }

        protected bool CheckLockedInternal(string tableName, string action)
        {
            return
                Session.CreateSQLQuery(IsOracle ? OraIsLocked : PgIsLocked)
                       .SetParameter("tableName", tableName)
                       .SetParameter("action", action)
                       .List<long>()[0] > 0;
        }

        /// <summary>
        ///     Снять блокировку
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="action"></param>
        protected void UnlockInternal(string tableName, string action)
        {
            var a = action.ToUpper();
            AssertValidAction(a);

            TryDropTrigger(tableName, a);

            Session.CreateSQLQuery(IsOracle ? OraUnlock : PgUnlock)
                   .SetParameter("tableName", tableName.ToLower())
                   .SetParameter("action", a)
                   .ExecuteUpdate();
        }

        //убрал проверку IsTriggerExists(tableName, a) потому что команда DROP TRIGGER IF EXISTS ...
        /// <summary>
        ///     Подготовить блок команд для выполнения
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="actions"></param>
        protected void UnlockInternalPrepare(string tableName, List<string> actions)
        {
            string sqlToExecuteForDeleteFromLockTable;
            if (actions == null || !actions.Any())
            {
                return;
            }
            var actionsInUpperCase = actions.Select(act => act.ToUpper()).ToList();
            
            // проверяем все действия на валидность
            actionsInUpperCase.ForEach(AssertValidAction);
            
            var allActionsForTable = " AND (" + string.Join(" OR ", actionsInUpperCase.Select(act => string.Format(PgUnlockWhereActionsClause, act))) + ")";
            var listOfSqlToExecuteForDropTrigger = new List<string>();
            if (IsOracle)
            {
                // храним все имена таблиц в нижнем регистре, а все имена действий в верхнем регистре
                sqlToExecuteForDeleteFromLockTable = new StringBuilder(string.Format(OraUnlockManyActionsForOneTable, tableName.ToLower(), allActionsForTable)).ToString();
                listOfSqlToExecuteForDropTrigger.AddRange(actionsInUpperCase.Select(act => string.Format(OraDropTrigger, GetTriggerName(tableName, act)).ToString()));
            }
            else
            {
                // храним все имена таблиц в нижнем регистре, а все имена действий в верхнем регистре
                sqlToExecuteForDeleteFromLockTable = new StringBuilder(string.Format(PgUnlockForManyActionsOneTable, tableName.ToLower(), allActionsForTable)).ToString();
                listOfSqlToExecuteForDropTrigger.AddRange(actionsInUpperCase.Select(act => string.Format(PgDropTrigger, GetTriggerName(tableName, act), tableName).ToString()));
            }

            if (!string.IsNullOrEmpty(sqlToExecuteForDeleteFromLockTable))
            {
                _queriesStorageDeleteRecordsFromTableLocker.Append(sqlToExecuteForDeleteFromLockTable + ";" + Environment.NewLine);
            }
            if (listOfSqlToExecuteForDropTrigger.Any())
            {
                _queriesStorageDropTriggers.Append(string.Join(";" + Environment.NewLine, listOfSqlToExecuteForDropTrigger));
                //добавляем к последней строке ;
                _queriesStorageDropTriggers.Append(";" + Environment.NewLine);
            }
        }

        protected void UnlockInternalExecute()
        {
            if (_queriesStorageDeleteRecordsFromTableLocker.Length > 0 || _queriesStorageDropTriggers.Length > 0)
            {
                Session.CreateSQLQuery(_queriesStorageDeleteRecordsFromTableLocker.ToString() + _queriesStorageDropTriggers).ExecuteUpdate();
            }
        }

        protected void ClearQueriesStorage()
        {
            _queriesStorageDropTriggers = new StringBuilder();
            _queriesStorageDeleteRecordsFromTableLocker = new StringBuilder();
        }

        /// <summary>
        ///     Получить имя триггера
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        protected string GetTriggerName(string tableName, string action)
        {
            return IsOracle
                       ? string.Format("{0}_{1}_TL", tableName.ToUpper(), action.ToUpper())
                       : string.Format("{0}_tl", action.ToLower());
        }

        /// <summary>
        /// Проверить правильное название действия
        /// </summary>
        /// <param name="action"></param>
        protected void AssertValidAction(string action)
        {
            switch (action)
            {
                case "INSERT":
                case "UPDATE":
                case "DELETE":
                    return;
                default:
                    throw new ArgumentException(string.Format("Недопустимый тип действия: {0}", action), "action");
            }
        }

        #endregion
    }
}