using System;
using System.Linq.Expressions;
using Bars.B4.Filter;
namespace Bars.Gkh
{
    public class StringIgnoreSpaceContainsProvider : IStringContainsOperationProvider
    {
        /// <summary>
        /// Создание выражения для операции.
        /// </summary>
        /// <param name="expression"/><param name="value"/>
        /// <returns/>
        public Expression MakeOperationExpression(Expression expression, object value)
        {
            return Expression.Call(
                Expression.Call(
                Expression.Call(Expression.Coalesce(expression, Expression.Constant(string.Empty)),
                typeof(string).GetMethod("Replace",
                            new Type[] { typeof(string), typeof(string) }),
                        Expression.Constant(" "),
                        Expression.Constant(string.Empty)),
                    typeof(string).GetMethod("ToLower", new Type[0])),
                typeof(string).GetMethod("Contains"),
                Expression.Constant(value.ToString().Replace(" ", string.Empty).ToLower()));
        }
    }
}
