namespace Bars.Gkh.SystemDataTransfer.Utils
{
    using System;

    using Bars.Gkh.Domain.Cache;

    using NHibernate;

    /// <summary>
    /// Помощник для работы с сессиями
    /// </summary>
    public static class IntegrationSessionAccessor
    {
        [ThreadStatic]
        public static IStatelessSession Session;

        /// <summary>
        /// Возвращает текущий экземпляр сессии
        /// </summary>
        /// <remarks>
        /// Если в текущем потоке выполняется экспорт или импорт, то сессия обязательно проставляется в IntegrationSessionAccessor.Session
        /// В противном случае пытаетмся достать сесиию из GkhCache во время сбора кэша при импорте
        /// </remarks>
        public static IStatelessSession GetSession()
        {
            return IntegrationSessionAccessor.Session ?? GkhCache.StatelessSession;
        }
    }
}