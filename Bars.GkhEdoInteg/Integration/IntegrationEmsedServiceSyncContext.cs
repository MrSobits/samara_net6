namespace Bars.GkhEdoInteg
{
    using System.Threading;

    public static class IntegrationEmsedServiceSyncContext
    {
        private static readonly object SyncObject = new object();

        public static bool IsReady()
        {
            return Monitor.TryEnter(IntegrationEmsedServiceSyncContext.SyncObject);
        }

        public static void Ready()
        {
            Monitor.Exit(IntegrationEmsedServiceSyncContext.SyncObject);
        }
    }
}