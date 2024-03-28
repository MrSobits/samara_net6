namespace Bars.Gkh.Utils
{
    using Log;

    public static class ProcessLogExtensions
    {
        public static void SafeInfo(this IProcessLog log, object message, object obj = null)
        {
            if(log == null) return;

            log.Info(message, obj);
        }

        public static void SafeDebug(this IProcessLog log, object message, object obj = null)
        {
            if (log == null) return;

            log.Debug(message, obj);
        }

        public static void SafeWarning(this IProcessLog log, object message, object obj = null)
        {
            if (log == null) return;

            log.Warning(message, obj);
        }

        public static void SafeError(this IProcessLog log, object message, object obj = null)
        {
            if (log == null) return;

            log.Error(message, obj);
        }
    }
}