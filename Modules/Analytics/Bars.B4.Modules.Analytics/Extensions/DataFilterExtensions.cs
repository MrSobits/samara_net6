namespace Bars.B4.Modules.Analytics.Extensions
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Bars.B4.Modules.Analytics.Filters;
    using Castle.Windsor;

    /// <summary>
    /// 
    /// </summary>
    public static class DataFilterExtensions
    {

        public static Expression BuildAllDataFilterExpression(this DataFilter filter, Type queryType,
            ParameterExpression x, IWindsorContainer container)
        {
            if (filter == null || filter.Value == null)
            {
                return null;
            }

            Expression expression = null;
            var properties = queryType.GetProperties().Where(p => p.PropertyType == typeof(string));
            var value = PreprocessMacrosExpr(filter.Value.ToLower(), container);

            foreach (var propertyInfo in properties)
            {
                var propertyExpression = Expression.Property(x, propertyInfo.Name);
                var fieldExpr =
                    Expression.Call(
                        Expression.Call(propertyExpression, typeof(string).GetMethod("ToLower", new Type[0])),
                        typeof(string).GetMethod("Contains"), Expression.Constant(value));

                if (expression == null)
                {
                    expression = fieldExpr;
                }
                else
                {
                    expression = Expression.MakeBinary(ExpressionType.OrElse, expression, fieldExpr);
                }
            }

            return expression;
        }

        public static Expression BuildDataFilterExpression(this DataFilter filter, Type queryType, ParameterExpression x,
            IWindsorContainer container)
        {
            if (filter == null)
            {
                return null;
            }

            if (filter.Group == DataFilterGroup.none)
            {
                MemberExpression propertyExpression;
                if (filter.DataIndex.Contains('.'))
                {
                    var fields = filter.DataIndex.Split('.');
                    propertyExpression = Expression.Property(x, fields[0]);
                    for (var i = 1; i < fields.Length; i++)
                    {
                        propertyExpression = Expression.Property(propertyExpression, fields[i]);
                    }
                }
                else
                {
                    propertyExpression = Expression.Property(x, filter.DataIndex);
                }

                return DataFilterExpressionBuilder.GetPropertyFilterExpression(propertyExpression,
                    PreprocessMacrosExpr(filter.Value, container),
                    container, filter.Operand);
            }

            if (filter.Group == DataFilterGroup.all)
            {
                return BuildAllDataFilterExpression(filter, queryType, x, container);
            }

            if (filter.Filters.Any())
            {
                var expType = filter.Group == DataFilterGroup.and ? ExpressionType.AndAlso : ExpressionType.OrElse;

                var exp = filter.Filters
                    .Select(f => BuildDataFilterExpression(f, queryType, x, container))
                    .Where(e => e != null)
                    .Aggregate((left, right) => Expression.MakeBinary(expType, left, right));

                return exp;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="macrosExpr"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public static object PreprocessMacrosExpr(string macrosExpr, IWindsorContainer container)
        {
            if (macrosExpr == null || !macrosExpr.StartsWith("@"))
            {
                return macrosExpr;
            }

            if (macrosExpr.StartsWith("@@"))
            {
                return macrosExpr.Substring(1);
            }

            var key = macrosExpr.Substring(1, macrosExpr.TakeWhile(Char.IsLetter).Count());
            var macros = container.Resolve<IMacrosContainer>().Get(key);

            return macros.GetValue();
        }


    }
}
