namespace Bars.B4.Modules.Analytics.Domain
{
    using System.Linq;
    using Bars.B4.Modules.Analytics.Filters;

    /// <summary>
    /// 
    /// </summary>
    public interface IFilterExpressionService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IQueryable<FilterExprProvider> GetAll();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        FilterExprProvider Get(string key);
    }
}
