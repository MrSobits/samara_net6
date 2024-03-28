namespace Bars.B4.Modules.Analytics.Extensions
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Bars.B4.Modules.Analytics.Domain;
    using Bars.B4.Modules.Analytics.Filters;
    using Castle.Windsor;

    /// <summary>
    /// 
    /// </summary>
    public static class SystemFilterExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="queryType"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public static Expression BuildFilterExpression(this SystemFilter filter, Type queryType, IWindsorContainer container)
        {
            if (filter == null)
            {
                return null;
            }

            if (filter.Group == SystemFilterGroup.none)
            {
                var filterExprService = container.Resolve<IFilterExpressionService>();
                var filterExpr = filterExprService.Get(filter.ExprProviderKey);
                var expr = filterExpr != null ? filterExpr.GetExpression(queryType) : null;
                return expr;
            }

            if (filter.Filters.Any())
            {
                var expType = filter.Group == SystemFilterGroup.and ? ExpressionType.AndAlso : ExpressionType.OrElse;

                var exp = filter.Filters
                    .Select(f => BuildFilterExpression(f, queryType, container))
                    .Where(e => e != null)
                    .Aggregate((left, right) => Expression.MakeBinary(expType, left, right));

                return exp;
            }

            return null;
        }
    }
}
