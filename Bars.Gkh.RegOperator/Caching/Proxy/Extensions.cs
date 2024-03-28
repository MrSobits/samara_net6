namespace Bars.Gkh.RegOperator.Caching.Proxy
{
    using Bars.B4.Modules.Caching.Interfaces;

    /// <summary>
    /// Методы-рсширения для работы с клиентом кэша
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Вернуть клиент, который оборачивает объекты, отдаваемые кэш в прокси для ленивой загрузки
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="cache">Экземепляр кэша</param>
        /// <returns>Клиент</returns>
        public static ICacheClient<T> GetLazyClient<T>(this IAppCache cache)
            where T : class, new()
        {
            return new LazyLoadCacheClientDecorator<T>(cache);
        }
    }
}