namespace Bars.Gkh.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;

    using Castle.Windsor;

    using Fasterflect;

    public static class QueryableExtension
    {
        public static IQueryable<T> TakeIf<T>(
            this IQueryable<T> query,
            bool condition,
            int count)
        {
            if (condition)
            {
                return query.Take(count);
            }

            return query;
        }

        public static IEnumerable<T> SplitWhere<T>(this IQueryable<T> data, Func<T, bool> predicate, int step = 1000)
        {
            var count = data.Count(predicate);

            var result = new List<T>(count);

            for (int i = 0; i < count; i += step)
            {
                result.AddRange(data.Where(predicate).Skip(i).Take(step).ToList());
            }

            return result;
        }

        /// <summary>
        /// Фильтрация если строка пустая или равна null
        /// </summary>
        public static IQueryable<T> WhereEmptyString<T>(this IQueryable<T> query, Expression<Func<T, string>> predicate)
        {
            var nullString = Expression.Constant(null, typeof(string));
            var emptyString = Expression.Constant(string.Empty, typeof(string));

            var expressionAnd = Expression.OrElse(
                Expression.Equal(predicate.Body, nullString),
                Expression.Equal(predicate.Body, emptyString));

            return query.Where(Expression.Lambda<Func<T, bool>>(expressionAnd, predicate.Parameters[0]));
        }

        /// <summary>
        /// Фильтрация если строка не пустая и не равна null
        /// </summary>
        public static IQueryable<T> WhereNotEmptyString<T>(this IQueryable<T> query, Expression<Func<T, string>> predicate)
        {
            var nullString = Expression.Constant(null, typeof(string));
            var emptyString = Expression.Constant(string.Empty, typeof(string));

            var expressionAnd = Expression.AndAlso(
                Expression.NotEqual(predicate.Body, nullString),
                Expression.NotEqual(predicate.Body, emptyString));

            return query.Where(Expression.Lambda<Func<T, bool>>(expressionAnd, predicate.Parameters[0]));
        }

        /// <summary>
        /// Фильтрация если экземпляр не равен null
        /// </summary>
        public static IQueryable<T> WhereNotNull<T>(this IQueryable<T> query, Expression<Func<T, object>> predicate)
        {
            var nullReference = Expression.Constant(null, typeof(object));

            var expression = Expression.NotEqual(predicate.Body, nullReference);

            return query.Where(Expression.Lambda<Func<T, bool>>(expression, predicate.Parameters[0]));
        }

        /// <summary>
        /// Фильтрация с условием если экземпляр не равен null
        /// </summary>
        public static IQueryable<T> WhereIfNotNull<T>(this IQueryable<T> query, bool condition, Expression<Func<T, object>> predicate)
        {
            if (condition)
            {
                return query.WhereNotNull(predicate);
            }

            return query;
        }

        /// <summary>
        /// Фильтрация если экземпляр <see cref="IEntity"/> не равен null
        /// </summary>
        public static IQueryable<T> WhereNotNull<T>(this IQueryable<T> query, Expression<Func<T, IEntity>> predicate)
        {
            var nullReference = Expression.Constant(null, typeof(object));

            var idProperty = Expression.Property(predicate.Body, typeof(IEntity).GetProperty("Id"));

            var expression = Expression.NotEqual(idProperty, nullReference);

            return query.Where(Expression.Lambda<Func<T, bool>>(expression, predicate.Parameters[0]));
        }

        /// <summary>
        /// Фильтрация с условием если экземпляр <see cref="IEntity"/> не равен null
        /// </summary>
        public static IQueryable<T> WhereIfNotNull<T>(this IQueryable<T> query, bool condition, Expression<Func<T, IEntity>> predicate)
        {
            if (condition)
            {
                return query.WhereNotNull(predicate);
            }

            return query;
        }

        /// <summary>
        /// Фильтрация если экземпляр <see cref="DateTime"/>? не равен null и <see cref="DateTime.MinValue"/> &lt; date &lt; <see cref="DateTime.MaxValue"/>
        /// </summary>
        public static IQueryable<T> WhereNotNull<T>(this IQueryable<T> query, Expression<Func<T, DateTime?>> predicate)
        {
            var nullReference = Expression.Constant(null, typeof(DateTime?));
            var minValue = Expression.Constant(DateTime.MinValue, typeof(DateTime?));
            var maxValue = Expression.Constant(DateTime.MaxValue, typeof(DateTime?));

            var expression = Expression.AndAlso(
                Expression.AndAlso(
                    Expression.NotEqual(predicate.Body, nullReference),
                    Expression.NotEqual(predicate.Body, minValue)),
                Expression.NotEqual(predicate.Body, maxValue));

            return query.Where(Expression.Lambda<Func<T, bool>>(expression, predicate.Parameters[0]));
        }

        /// <summary>
        /// Фильтрация с условием если экземпляр <see cref="DateTime"/>? не равен null и <see cref="DateTime.MinValue"/> &lt; date &lt; <see cref="DateTime.MaxValue"/>
        /// </summary>
        public static IQueryable<T> WhereIfNotNull<T>(this IQueryable<T> query, bool condition, Expression<Func<T, DateTime?>> predicate)
        {
            if (condition)
            {
                return query.WhereNotNull(predicate);
            }

            return query;
        }

        /// <summary>
        /// Приводит коллекцию к <see cref="ListDataResult"/> с фильтрацией и сортировкой
        /// </summary>
        /// <param name="query">Коллекция элементов</param>
        /// <param name="loadParam">Параметры загрузки списка</param>
        /// <param name="container">IoC</param>
        public static ListDataResult ToListDataResult<T>(this IQueryable<T> query, LoadParam loadParam, IWindsorContainer container = null)
        {
            container = container ?? ApplicationContext.Current.Container;

            var result = query
                .Filter(loadParam, container);

            return new ListDataResult(
                result
                    .Order(loadParam)
                    .Paging(loadParam)
                    .ToList(),
                result.Count());
        }

        /// <summary>
        /// Приводит коллекцию к <see cref="ListDataResult"/> с фильтрацией и сортировкой
        /// </summary>
        /// <param name="query">Коллекция элементов</param>
        /// <param name="loadParam">Параметры загрузки списка</param>
        /// <param name="container">IoC</param>
        public static ListDataResult ToListDataResult<T>(this IQueryable<T> query, StoreLoadParams loadParam, IWindsorContainer container = null)
        {
            container = container ?? ApplicationContext.Current.Container;

            var result = query
                .Filter(loadParam, container);

            return new ListDataResult(
                result
                    .Order(loadParam)
                    .Paging(loadParam)
                    .ToList(),
                result.Count());
        }

        /// <summary>
        /// Приводит коллекцию к <see cref="ListDataResult"/> с фильтрацией, сортировкой и пагинацией в зависимости от флага usePaging
        /// </summary>
        /// <param name="query">Коллекция элементов</param>
        /// <param name="loadParam">Параметры загрузки списка</param>
        /// <param name="container">IoC</param>
        /// <param name="usePersistentObjectOrdering">Использовать сортировку для составных объектов</param>
        /// <param name="usePaging">Использовать пагинацию</param>
        public static ListDataResult ToListDataResultWithPaging<T>(this IQueryable<T> query, LoadParam loadParam, IWindsorContainer container = null, bool usePersistentObjectOrdering = false, bool usePaging = true)
        {
            container = container ?? ApplicationContext.Current.Container;

            var filteredResult = query
                .Filter(loadParam, container);
            var orderedResult = usePersistentObjectOrdering 
                ? filteredResult.OrderUsingObjects(loadParam) 
                : filteredResult.Order(loadParam);
            var result = usePaging 
                ? orderedResult.QueryablePaging(loadParam).ToList() 
                : orderedResult.ToList();

            return new ListDataResult(result, filteredResult.Count());
        }

        /// <summary>
        /// Применить пейджинг к коллекции
        /// </summary>
        public static IQueryable<T> QueryablePaging<T>(this IQueryable<T> collection, LoadParam loadParam)
        {
            if (loadParam.Start != 0 || (uint)loadParam.Limit > 0U)
            {
                return collection.Skip(loadParam.Start).Take(loadParam.Limit);
            }
            return collection;
        }

        /// <summary>Значение свойства входит в список</summary>
        /// <typeparam name="T">Тип сущности</typeparam>
        /// <typeparam name="TProperty">Тип свойства</typeparam>
        /// <param name="query">Запрос</param>
        /// <param name="propertyExpression">Свойство</param>
        /// <param name="array">Массив</param>
        /// <param name="bulkSize">Размер пачки</param>
        /// <returns>Отфильтрованный запрос</returns>
        public static IQueryable<T> WhereContainsBulked<T, TProperty>(
            this IQueryable<T> query,
            Expression<Func<T, TProperty>> propertyExpression,
            IEnumerable<TProperty> array,
            int bulkSize = 1000)
        {
            var start = 0;
            var arrayData = array as TProperty[] ?? array.ToArray();

            var count = arrayData.Length;

            var entity = propertyExpression.Parameters[0];
            var expressions = new List<Expression>();

            while (start < count)
            {
                var cnt = count - start;
                if (count > bulkSize)
                {
                    cnt = bulkSize;
                }

                var data = arrayData.Skip(start).Take(cnt).ToList();

                expressions.Add(Expression.Call(
                    Expression.Constant(data),
                    data.GetType().GetMethod("Contains"),
                    propertyExpression.Body));

                start += cnt;
            }

            if (expressions.Count > 0)
            {
                /* Собираем через 'ИЛИ' выражение */
                var wholeExpression = expressions.First();
                expressions.Skip(1).ForEach(expr => wholeExpression = Expression.MakeBinary(ExpressionType.OrElse, wholeExpression, expr));

                var full = Expression.Lambda<Func<T, bool>>(wholeExpression, entity);

                query = query.Where(full);
            }
            else
            {
                /* Если фильтры отсутствуют, то ничего не возвращаем */
                query = query.Where(x => false);
            }

            return query;
        }

        /// <summary>Значение свойства входит в список при условии</summary>
        /// <typeparam name="T">Тип сущности</typeparam>
        /// <typeparam name="TProperty">Тип свойства</typeparam>
        /// <param name="query">Запрос</param>
        /// <param name="condition">Условие</param>
        /// <param name="propertyExpression">Свойство</param>
        /// <param name="array">Массив</param>
        /// <param name="bulkSize">Размер пачки</param>
        /// <returns>Отфильтрованный запрос</returns>
        public static IQueryable<T> WhereIfContainsBulked<T, TProperty>(
            this IQueryable<T> query,
            bool condition,
            Expression<Func<T, TProperty>> propertyExpression,
            IEnumerable<TProperty> array,
            int bulkSize = 1000)
        {
            return condition ? query.WhereContainsBulked(propertyExpression, array, bulkSize) : query;
        }


        /// <summary>
        /// Метод получает записи по идентификаторам, запрашивая порциями в размере <paramref name="partSize"/>
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="filterProperty">Селектор свойства</param>
        /// <param name="filterCollection">Коллекция фильтрующих значений</param>
        /// <param name="partSize">Размер порций</param>
        /// <param name="comparer">Метод сравнения объектов</param>
        /// <returns>Список объектов</returns>
        public static IList<T> GetPortioned<T, TProperty>(
            this IQueryable<T> query,
            Expression<Func<T, TProperty>> filterProperty,
            IEnumerable<TProperty> filterCollection,
            int partSize = 1000,
            IEqualityComparer<TProperty> comparer = null)
        {
            var start = 0;
            var arrayData = new HashSet<TProperty>(filterCollection, comparer);
            var count = arrayData.Count;

            var listResult = new List<T>(count);
            var entity = filterProperty.Parameters[0];

            while (start < count)
            {
                var current = count - start;
                if (count > partSize)
                {
                    current = partSize;
                }

                var data = arrayData.Skip(start).Take(current).ToList();

                var filterExpression = Expression.Lambda<Func<T, bool>>(
                    Expression.Call(
                        Expression.Constant(data),
                        typeof(List<TProperty>).GetMethod("Contains"),
                        filterProperty.Body),
                    entity);

                query.Where(filterExpression).AddTo(listResult);

                start += current;
            }

            return listResult;
        }

        /// <summary>Сортировка списка</summary>
        /// <param name="data">Список.</param>
        /// <param name="loadParam">Параметры загрузки списка.</param>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <returns>Отсортированный список.</returns>
        /// <remarks>Список сортируется по простым типам или составным типам, реализующим PersistentObject.
        ///          Если у составного типа нет свойства ID, получим Exception</remarks>
        public static IQueryable<T> OrderUsingObjects<T>(this IQueryable<T> data, LoadParam loadParam)
        {
            IQueryable<T> query = data;

            if (loadParam.Order.Length > 0)
            {
                // Order - массив, но т.к из фронта может прийти только один фильтр, берем первый элемент массива
                var orderField = loadParam.Order.First();

                if (typeof(T).GetProperty(orderField.Name)?.PropertyType.GetProperty("Id") != null)
                {
                    Expression<Func<T, long>> keySelector = obj => (long) obj.TryGetPropertyValue(orderField.Name).TryGetPropertyValue("Id");
                    
                    return query.OrderIf(true, orderField.Asc, keySelector);
                }
            }

            return query.Order(loadParam);
        }

        /// <summary>Условная фильтрация последовательности</summary>
        /// <typeparam name="T">Тип элементов последовательности</typeparam>
        /// <param name="query">Запрос</param>
        /// <param name="condition">Основное условие</param>
        /// <param name="predicateIfTrue">Предикат для отбора элементов true</param>
        /// <param name="predicateIfFalse">Предикат для отбора элементов false</param>
        public static IQueryable<T> WhereIfElse<T>(
            this IQueryable<T> query,
            bool condition,
            Expression<Func<T, bool>> predicateIfTrue,
            Expression<Func<T, bool>> predicateIfFalse)
        {
            return condition ? query.Where(predicateIfTrue) : query.Where(predicateIfFalse);
        }

        /// <summary>Условная фильтрация последовательности</summary>
        /// <typeparam name="T">Тип элементов последовательности</typeparam>
        /// <param name="query">Запрос</param>
        /// <param name="condition">Основное условие</param>
        /// <param name="elseCondition">ElseIf условие</param>
        /// <param name="predicateIfTrue">Предикат для отбора элементов true</param>
        /// <param name="predicateIfFalse">Предикат для отбора элементов false</param>
        public static IQueryable<T> WhereIfElseIf<T>(
            this IQueryable<T> query,
            bool condition,
            bool elseCondition,
            Expression<Func<T, bool>> predicateIfTrue,
            Expression<Func<T, bool>> predicateIfFalse)
        {
            return condition ? query.Where(predicateIfTrue) : elseCondition ? query.Where(predicateIfFalse) : query;
        }
    }
}