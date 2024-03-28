namespace Bars.B4.Modules.Analytics.Reports.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Класс, содержащий вспомогательные предикатные методы
    /// </summary>
    public static class PredicateBuilder
    {
        /// <summary>
        /// Формирует лямбда-выражение, которое всегда возвращает true.
        /// </summary>
        /// <typeparam name="T">Тип параметра</typeparam>
        /// <returns>Лямбда-выражение</returns>
        public static Expression<Func<T, bool>> True<T>() { return f => true; }

        /// <summary>
        /// Формирует лямбда-выражение, которое всегда возвращает false.
        /// </summary>
        /// <typeparam name="T">Тип параметра</typeparam>
        /// <returns>Лямбда-выражение</returns>
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        /// <summary>
        /// Формирует лямбда-выражение, которое реализует логику работы оператора "Или".
        /// </summary>
        /// <typeparam name="T">Тип параметра</typeparam>
        /// <param name="expr1">Выражение 1</param>
        /// <param name="expr2">Выражение 2</param>
        /// <returns>Лямбда-выражение</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        /// <summary>
        /// Формирует лямбда-выражение, которое реализует логику работы оператор "И".
        /// </summary>
        /// <typeparam name="T">Тип параметра</typeparam>
        /// <param name="expr1">Выражение 1</param>
        /// <param name="expr2">Выражение 2</param>
        /// <returns>Лямбда-выражение</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }

        /// <summary>
        /// Формирует лямбда-выражение, осуществляещее поиск заданного Id в указанном массиве идентификаторов.
        /// При этом, весь массив разбивается на части по 1000 элементов. И поиск происходит по каждой части в отдельности.
        /// Это сделано из-за орграничения Oracle на количество элементов в операторе IN().
        /// </summary>
        /// <typeparam name="T">Тип in параметра</typeparam>
        /// <typeparam name="TValue">Тип out параметра</typeparam>
        /// <param name="selector">Выражение селектора</param>
        /// <param name="ids">Массив идентификаторов</param>
        /// <returns>Лямбда-выражение</returns>
        public static Expression<Func<T, bool>> OracleInRange<T, TValue>(Expression<Func<T, TValue>> selector, IEnumerable<TValue> ids)
        {
            const int oracleLimit = 999;
            MethodInfo method = null;
            foreach (MethodInfo tmp in typeof(Enumerable).GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                if (tmp.Name == "Contains" && tmp.IsGenericMethodDefinition && tmp.GetParameters().Length == 2)
                {
                    method = tmp.MakeGenericMethod(typeof(TValue));
                    break;
                }
            }
			
			if(method == null) {
				throw new InvalidOperationException("Unable to locate Contains");
			}

            var predicate = False<T>();

            for (int i = 0; i < ids.Count(); i = i + oracleLimit)
            {
                var subIds = ids.Skip(i).Take(oracleLimit).ToArray();
                var tmpKeys = Expression.Constant(subIds, typeof(TValue[]));
                var tmpPredicate = Expression.Call(method, tmpKeys, selector.Body);
                var tmpLambda = Expression.Lambda<Func<T, bool>>(tmpPredicate, selector.Parameters[0]);

                predicate = predicate.Or(tmpLambda);
            }

            return predicate;
        }
    }
}
