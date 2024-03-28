namespace Bars.B4.Modules.Analytics.Domain
{
    using System.Linq;
    using Bars.B4.Modules.Analytics.Filters;
    using Castle.Windsor;

    public class FilterExpressionService : IFilterExpressionService
    {
        private readonly IWindsorContainer _container;

        public FilterExpressionService(IWindsorContainer container)
        {
            _container = container;
        }

        public IQueryable<FilterExprProvider> GetAll()
        {
            return _container.ResolveAll<FilterExprProvider>().AsQueryable();
        }

        public FilterExprProvider Get(string key)
        {
            return _container.ResolveAll<FilterExprProvider>().FirstOrDefault(x => x.Key == key);
        }
    }
}
