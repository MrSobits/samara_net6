namespace Bars.Gkh.Domain.DatabaseMutex
{
    using System;
    using System.Data;

    using Bars.Gkh.RegOperator.Utils;

    using NHibernate;

    public class DatabaseLockedMutexHandle : IDatabaseLockedMutexHandle
    {
        private long _historyId;

        public DatabaseLockedMutexHandle()
        {
            GC.SuppressFinalize(this);
        }

        public DatabaseLockedMutexHandle(ISession session, string mutexName, string description, long? userId)
        {
            this.Session = session;
            this.MutexName = mutexName;
            this.Description = description;
            this.UserId = userId;
        }

        public ISession Session { get; set; }

        public string MutexName { get; set; }

        public string Description { get; set; }

        public long? UserId { get; set; }

        public bool IsLocked { get; private set; }

        public void Dispose()
        {
            this.Close();
        }

        public void Lock()
        {
            if (!this.Session.Transaction.IsActive)
            {
                throw new DatabaseMutexWithoutTransactionException();
            }

            using (var otherSession = this.Session.SessionFactory.OpenStatelessSession())
            {
                DbUtils.CallProcedure(
                    otherSession.Connection,
                    otherSession.Transaction,
                    "B4_MUTEX_CREATE",
                    new[] { "MUTEX_NAME" },
                    new object[] { this.MutexName });
            }

            this._historyId = DbUtils.CallFunction<long>(
                this.Session.Connection,
                this.Session.Transaction,
                DbType.Int64,
                "B4_MUTEX_LOCK",
                new[] { "MUTEX_NAME", "USER_ID", "DESCRIPTION" },
                new object[] { this.MutexName, this.UserId, this.Description });

            GC.ReRegisterForFinalize(this);
            this.IsLocked = true;
        }

        public bool TryLock()
        {
            if (!this.Session.Transaction.IsActive)
            {
                throw new DatabaseMutexWithoutTransactionException();
            }

            using (var otherSession = this.Session.SessionFactory.OpenStatelessSession())
            {
                DbUtils.CallProcedure(
                    otherSession.Connection,
                    otherSession.Transaction,
                    "b4_mutex_create",
                    new[] { "mutex_name" },
                    new object[] { this.MutexName });
            }

            this._historyId = DbUtils.CallFunction<long>(
                this.Session.Connection,
                this.Session.Transaction,
                DbType.Int64,
                "b4_mutex_try_lock",
                new[] { "mutex_name", "user_id", "description" },
                new object[] { this.MutexName, this.UserId, this.Description });

            if (this._historyId > 0)
            {
                GC.ReRegisterForFinalize(this);
                this.IsLocked = true;
                return true;
            }

            return false;
        }

        public void Close()
        {
            if (!this.IsLocked)
            {
                return;
            }

            this.IsLocked = false;

            DbUtils.CallProcedure(
                this.Session.Connection,
                this.Session.Transaction,
                "b4_mutex_unlock",
                new[] { "history_id" },
                new object[] { this._historyId });
        }
    }
}