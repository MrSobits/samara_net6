namespace Bars.Gkh.Domain.DatabaseMutex
{
    using Bars.B4;
    using Bars.B4.DataAccess;

    using Castle.Windsor;

    public class DatabaseMutexManager : IDatabaseMutexManager
    {
        private readonly IWindsorContainer container;

        public DatabaseMutexManager(IWindsorContainer container)
        {
            this.container = container;
        }

        public IDatabaseLockedMutexHandle Lock(IHaveMutexName haveMutexName, string description)
        {
            return this.Lock(haveMutexName.GetMutexName(), description);
        }

        public bool TryLock(IHaveMutexName haveMutexName, string description, out IDatabaseLockedMutexHandle result)
        {
            return this.TryLock(haveMutexName.GetMutexName(), description, out result);
        }

        public IDatabaseLockedMutexHandle Lock(string name, string description)
        {
            var mutexHandle = this.CreateMutexHandle(name, description);
            mutexHandle.Lock();
            return mutexHandle;
        }

        public bool TryLock(string name, string description, out IDatabaseLockedMutexHandle result)
        {
            var mutexHandle = this.CreateMutexHandle(name, description);
            var success = mutexHandle.TryLock();
            result = success ? mutexHandle : null;
            return success;
        }

        private DatabaseLockedMutexHandle CreateMutexHandle(string name, string description)
        {
            long? userId = this.container.Resolve<IUserIdentity>().UserId;
            if (userId == 0)
            {
                userId = null;
            }

            var session = this.container.Resolve<ISessionProvider>().GetCurrentSession();
            var mutexHandle = new DatabaseLockedMutexHandle(session, name, description, userId);
            return mutexHandle;
        }
    }
}