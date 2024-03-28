namespace Bars.Gkh.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using B4.Utils.Annotations;

    using Bars.B4;
    using Bars.B4.Application;

    using Castle.Windsor;

    public static class EnumerableExtensions
    {
        /// <summary>
        /// Метод объединения строк через разделитель
        /// </summary>
        public static string AggregateWithSeparator(this IEnumerable<string> source, string separator)
        {
            var result = new StringBuilder();
            if (source != null)
            {
                foreach (var current in source)
                {
                    if (!string.IsNullOrWhiteSpace(current))
                    {
                        if (result.Length > 0)
                        {
                            result.Append(separator);
                        }

                        result.Append(current);
                    }
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Метод объединения строк через разделитель
        /// </summary>
        public static string AggregateWithSeparator<T>(this IEnumerable<T> source, Func<T, string> selector, string separator)
        {
            return AggregateWithSeparator(source.Select(selector), separator);
        }

        /// <summary>
        /// Метод объединения строк через разделитель
        /// </summary>
        public static string AggregateWithSeparator<T>(this IEnumerable<T> source, Func<T, object> selector, string separator)
        {
            var result = new StringBuilder();

            foreach (var current in source)
            {
                var value = selector(current);

                if (value != null)
                {
                    if (result.Length > 0)
                    {
                        result.Append(separator);
                    }

                    result.Append(value);
                }
            }

            return result.ToString();
        }

        public static IEnumerable<T[]> SplitArray<T>(this T[] source, int portionCount = 1000)
        {
            if (source == null)
            {
                throw new ArgumentException("Source cannot be null");
            }

            var length = source.Length;

            for (int i = 0; i < source.Length; i += portionCount)
            {
                yield return source.Skip(i).Take(length - i <= portionCount ? length - i : portionCount).ToArray();
            }
        }

        /// <summary>
        /// Вернуть первое непустое значение
        /// </summary>
        /// <param name="source"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string Coalesce(this IEnumerable<string> source, string defaultValue = null)
        {
            ArgumentChecker.NotNull(source, "source");

            foreach (var item in source)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    return item;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Вернуть первое непустое значение
        /// </summary>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string Coalesce<T>(this IEnumerable<T> source, Func<T, string> selector, string defaultValue = null)
        {
            ArgumentChecker.NotNull(source, "source");
            ArgumentChecker.NotNull(selector, "selector");

            return source.Select(selector).Coalesce(defaultValue);
        }

        /// <summary>
        /// Вернуть первое непустое значение
        /// </summary>
        /// <param name="source"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T Coalesce<T>(this IEnumerable<T> source, T defaultValue = null) where T : class
        {
            ArgumentChecker.NotNull(source, "source");

            foreach (var item in source)
            {
                if (item != null)
                {
                    return item;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Вернуть первое непустое значение
        /// </summary>
        /// <param name="source"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T? Coalesce<T>(this IEnumerable<T?> source, T? defaultValue = null) where T : struct 
        {
            ArgumentChecker.NotNull(source, "source");

            foreach (var item in source)
            {
                if (item != null)
                {
                    return item;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Добавить элемент к коллекции.
        /// <para>Используется для добавления элементов, не реализующих интерфейс <see cref="IList{T}"/></para>
        /// </summary>
        /// <typeparam name="T">Тип элемента</typeparam>
        /// <param name="element">Добавляемый элемент</param>
        /// <param name="collection">Коллекция</param>
        /// <returns>Добавленный элемент</returns>
        public static T AddTo<T>(this T element, ICollection<T> collection)
        {
            ArgumentChecker.NotNull(element, () => element);
            collection.Add(element);

            return element;
        }

        /// <summary>
        /// Приводит коллекцию к <see cref="ListDataResult"/> с фильтрацией и сортировкой
        /// </summary>
        /// <param name="collection">Коллекция элементов</param>
        /// <param name="loadParam">Параметры загрузки списка</param>
        /// <param name="container">IoC</param>
        /// <param name="usePersistentObjectOrdering">Использовать сортировку для составных объектов</param>
        /// <param name="usePaging">Использовать пагинацию</param>
        public static ListDataResult ToListDataResult<T>(this IEnumerable<T> collection, LoadParam loadParam, IWindsorContainer container = null, bool usePersistentObjectOrdering = false, bool usePaging = true)
        {
            container = container ?? ApplicationContext.Current.Container;

            var filteredResult = collection
                .AsQueryable()
                .Filter(loadParam, container);
            var orderedResult = usePersistentObjectOrdering 
                ? filteredResult.OrderUsingObjects(loadParam) 
                : filteredResult.Order(loadParam);
            var result = usePaging 
                ? orderedResult.Paging(loadParam).ToList() 
                : orderedResult.ToList();

            return new ListDataResult(result, filteredResult.Count());
        }

        /// <summary>
        /// Приводит коллекцию к <see cref="ListDataResult"/> с фильтрацией и сортировкой
        /// </summary>
        /// <param name="collection">Коллекция элементов</param>
        /// <param name="loadParam">Параметры загрузки списка</param>
        /// <param name="container">IoC</param>
        public static ListDataResult ToListDataResult<T>(this IEnumerable<T> collection, StoreLoadParams loadParam, IWindsorContainer container = null)
        {
            container = container ?? ApplicationContext.Current.Container;

            var result = collection
                .AsQueryable()
                .Filter(loadParam, container);

            return new ListDataResult(
                result
                    .Order(loadParam)
                    .Paging(loadParam)
                    .ToList(),
                result.Count());
        }

        /// <summary>
        /// Производит замену в свойстве
        /// </summary>
        /// <param name="collection">Коллекция элементов</param>
        /// <param name="propertySelector">Селектор заменяемого свойства</param>
        /// <param name="oldValue">Старое значение</param>
        /// <param name="newValue">Новое значение</param>
        /// <typeparam name="T"></typeparam>
        public static IEnumerable<T> ReplaceStringValue<T>(this IEnumerable<T> collection, Expression<Func<T, string>> propertySelector, string oldValue, string newValue = "")
            where T : new()
        {
            var propertyName = (propertySelector.Body as MemberExpression)?.Member.Name;
            if (collection != null && propertyName != null)
            {
                var queryExpression = Expression.Parameter(typeof(T), "x");
                var resultVariable = Expression.Variable(typeof(T), "result");
                var oldValueConstant = Expression.Constant(oldValue, typeof(string));
                var newValueConstant = Expression.Constant(newValue, typeof(string));
                var nullReferenceConstant = Expression.Constant(null, typeof(object));

                var propertyExpression = Expression.Property(queryExpression, propertyName);
                var replaceMethod = typeof(string).GetMethod("Replace", new[] { typeof(string), typeof(string) });

                var replaceMethodExpression = Expression.Call(propertyExpression, replaceMethod, oldValueConstant, newValueConstant);
                var nullReferenceExpression = Expression.NotEqual(propertyExpression, nullReferenceConstant);

                var newStringValue = Expression.Assign(propertyExpression, replaceMethodExpression);
                var newQueryValue = Expression.Assign(resultVariable, queryExpression);

                var conditionExpression = Expression.IfThen(nullReferenceExpression, newStringValue);

                var blockExpression = Expression.Block(new[] { resultVariable }, conditionExpression, newQueryValue);

                Expression<Func<T, T>> selectorExpression = Expression.Lambda<Func<T, T>>(blockExpression, queryExpression);

                collection = collection.Select(selectorExpression.Compile());
            }

            return collection;
        }

        /// <summary>
        /// Фильтрация если строка пустая или равна null
        /// </summary>
        public static IEnumerable<T> WhereEmptyString<T>(this IEnumerable<T> query, Func<T, string> predicate)
        {
            return query.Where(x => string.IsNullOrEmpty(predicate(x)));
        }

        /// <summary>
        /// Фильтрация если строка не пустая и не равна null
        /// </summary>
        public static IEnumerable<T> WhereNotEmptyString<T>(this IEnumerable<T> query, Func<T, string> predicate)
        {
            return query.Where(x => !string.IsNullOrEmpty(predicate(x)));
        }

        /// <summary>
        /// Применить пейджинг к коллекции
        /// </summary>
        public static IEnumerable<T> Paging<T>(this IEnumerable<T> collection, LoadParam loadParam)
        {
            if (loadParam.Start != 0 || (uint) loadParam.Limit > 0U)
            {
                return collection.Skip(loadParam.Start).Take(loadParam.Limit);
            }
            return collection;
        }

        /// <summary>
        /// Применить пейджинг к коллекции при условии
        /// </summary>
        public static IEnumerable<T> PagingIf<T>(this IEnumerable<T> collection, bool condition, LoadParam loadParam)
        {
            if (condition && (loadParam.Start != 0 || (uint)loadParam.Limit > 0U))
            {
                return collection.Skip(loadParam.Start).Take(loadParam.Limit);
            }
            return collection;
        }
    }
}