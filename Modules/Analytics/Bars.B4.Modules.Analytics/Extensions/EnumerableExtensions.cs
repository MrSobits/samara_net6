namespace Bars.B4.Modules.Analytics.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Методы расширения для <see cref="IEnumerable{T}"/>
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Реализация Left Join для <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="inner"></param>
        /// <param name="pk"></param>
        /// <param name="fk"></param>
        /// <param name="result"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TInner"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TResult>
            LeftJoin<TSource, TInner, TKey, TResult>(this IEnumerable<TSource> source,
                IEnumerable<TInner> inner,
                Func<TSource, TKey> pk,
                Func<TInner, TKey> fk,
                Func<TSource, TInner, TResult> result)
        {
            IEnumerable<TResult> _result = from s in source
                                           join i in inner
                                               on pk(s) equals fk(i) into joinData
                                           from left in joinData.DefaultIfEmpty()
                                           select result(s, left);

            return _result;
        }
    }
}
