namespace Bars.Gkh.Domain.DatabaseMutex
{
    public interface IDatabaseMutexManager
    {
        IDatabaseLockedMutexHandle Lock(IHaveMutexName haveMutexName, string description);

        bool TryLock(IHaveMutexName haveMutexName, string description, out IDatabaseLockedMutexHandle result);

        IDatabaseLockedMutexHandle Lock(string name, string description);

        bool TryLock(string name, string description, out IDatabaseLockedMutexHandle result);
    }
}