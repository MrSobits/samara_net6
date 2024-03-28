namespace Bars.B4.Modules.Analytics.Domain
{
    using System.Linq;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Utils;
    using Castle.Windsor;

    /// <summary>
    /// 
    /// </summary>
    public class DataProviderService : IDataProviderService
    {
        private readonly IWindsorContainer _container;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        public DataProviderService(IWindsorContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="noHidden"></param>
        /// <returns></returns>
        public IQueryable<IDataProvider> GetAll(bool noHidden = true)
        {
            return _container.ResolveAll<IDataProvider>().WhereIf(noHidden, x => !x.IsHidden).AsQueryable();
        }


        /// <summary>
        /// Возвращает поставщика данных по переданному ключу.
        /// </summary>
        /// <param name="key">Ключ поставщика данных</param>
        /// <returns></returns>
        public IDataProvider Get(string key)
        {
            var providers = _container.ResolveAll<IDataProvider>();
            return providers.FirstOrDefault(x => x.Key == key);
        }
    }
}
