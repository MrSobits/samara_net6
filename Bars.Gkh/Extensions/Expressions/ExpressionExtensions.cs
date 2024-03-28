namespace Bars.Gkh.Extensions.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.B4.Utils;

    /// <summary>
    /// Расширения для выражений
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Произвести слияние двух выражений оператором <paramref name="merge"/>
        /// </summary>
        /// <typeparam name="T">Тип параметра</typeparam>
        /// <param name="first">Выражение 1</param>
        /// <param name="second">Выражение 2</param>
        /// <param name="merge">Оперция между двумя выражениями (например: "И" или "ИЛИ")</param>
        /// <returns>Лямбда-выражение</returns>
        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        /// <summary>
        /// Формирует лямбда-выражение, которое реализует логику работы оператор "И".
        /// </summary>
        /// <typeparam name="T">Тип параметра</typeparam>
        /// <param name="first">Выражение 1</param>
        /// <param name="second">Выражение 2</param>
        /// <returns>Лямбда-выражение</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }

        /// <summary>
        /// Формирует лямбда-выражение, которое реализует логику работы оператор "ИЛИ".
        /// </summary>
        /// <typeparam name="T">Тип параметра</typeparam>
        /// <param name="first">Выражение 1</param>
        /// <param name="second">Выражение 2</param>
        /// <returns>Лямбда-выражение</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }

        /// <summary>
        /// Формирует выражение из коллекции через оператор ИЛИ
        /// </summary>
        public static Expression CompositeOr(this IEnumerable<Expression> expressions)
        {
            if (expressions.Any())
            {
                var wholeExpression = expressions.First();
                expressions.Skip(1).Where(x => !x.IsEmpty()).ForEach(expr => wholeExpression = Expression.MakeBinary(ExpressionType.OrElse, wholeExpression, expr));

                return wholeExpression;
            }
            else
            {
                return Expression.Empty();
            }
        }

        /// <summary>
        /// Формирует выражение из коллекции через оператор И
        /// </summary>
        public static Expression CompositeAnd(this IEnumerable<Expression> expressions)
        {
            if (expressions.Any())
            {
                var wholeExpression = expressions.First();
                expressions.Skip(1).Where(x => !x.IsEmpty()).ForEach(expr => wholeExpression = Expression.MakeBinary(ExpressionType.AndAlso, wholeExpression, expr));

                return wholeExpression;
            }
            else
            {
                return Expression.Empty();
            }
        }

        /// <summary>
        /// Выражение <see cref="DefaultExpression"/>, содержащее свойство NodeType, равное <see cref="ExpressionType.Default"/>, и свойство Type, равное void
        /// </summary>
        public static bool IsEmpty(this Expression expression)
        {
            return expression.NodeType == ExpressionType.Default && expression.Type == typeof(void);
        }
    }
}