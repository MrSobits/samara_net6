namespace Bars.Gkh.Extensions
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Utils.Caching;

    /// <summary>
    /// Класс методов-расширений для <see cref="IQueryable{T}"/>
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Метод получает количество записей текущего запроса и кэширует в сессии по FormData запроса.
        /// <para>Не зависит от пагинации или сортировки</para>
        /// </summary>
        /// <typeparam name="T">Тип запроса</typeparam>
        /// <param name="query">Запрос</param>
        /// <param name="cacheKey">Ключ кэша</param>
        /// <returns>Количество элементов, которые будут возвращены запросом</returns>
        public static int GetCountCached<T>(this IQueryable<T> query, string cacheKey = null)
        {
            return CountCacheHelper.GetCountCached(query, cacheKey);
        }

        /// <summary>
        /// Метод сортировки с возможностью явного отключения сортировки по первичному ключу
        /// </summary>
        /// <param name="data">
        /// Запрос
        /// </param>
        /// <param name="loadParam">
        /// Параметры загрузки списка
        /// </param>
        /// <param name="addOrderById">
        /// Добавить в конце сортировку по Id.
        /// <para>В случае, если сортируем по неуникальному полю, порядок данных может поменяться</para>
        /// </param>
        /// <typeparam name="T">
        /// Тип данных запроса
        /// </typeparam>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public static IQueryable<T> GkhOrder<T>(this IQueryable<T> data, LoadParam loadParam, bool addOrderById)
        {
            var result = data;

            var first = true;
            foreach (var field in loadParam.Order)
            {
                var x = Expression.Parameter(typeof(T), "x");

                var expression = Expression.Lambda<Func<T, object>>(Expression.Convert(ReflectionUtil.GetPropertyForExpression(x, field.Name), typeof(object)), x);

                result = first
                    ? result.OrderIf(true, field.Asc, expression)
                    : result.OrderThenIf(true, field.Asc, expression);

                first = false;
            }

            // Добавляем сортировку по ID для стабильной сортировки
            if (addOrderById && typeof(T).GetProperty("Id") != null)
            {
                var x = Expression.Parameter(typeof(T), "x");
                var expression = Expression.Lambda<Func<T, object>>(Expression.Convert(ReflectionUtil.GetPropertyForExpression(x, "Id"), typeof(object)), x);

                var sortAsc = true;
                if (loadParam.Order.Length > 0)
                {
                    sortAsc = loadParam.Order[loadParam.Order.Length - 1].Asc;
                }

                result = first
                    ? result.OrderIf(true, sortAsc, expression)
                    : result.OrderThenIf(true, sortAsc, expression);
            }

            return result;
        }

        /// <summary>
        /// Метод выполняет сортировку по базовым параметрам, в случае, если они пришли, 
        /// иначе сортировка производится по указанным вручную
        /// </summary>
        /// <typeparam name="TSource">Тип коллекции</typeparam>
        /// <typeparam name="TKey">Тип ключа сортировки</typeparam>
        /// <param name="data">Входной подзапрос</param>
        /// <param name="loadParam">Параметры загрузки списка</param>
        /// <param name="asc">Сортировать по возрастанию</param>
        /// <param name="elseSortExpression">Селектор свойства, по которому будет произведена сортировка в случае отсутствия параметров с клиента</param>
        /// <returns>Результирующий подзапрос</returns>
        public static IQueryable<TSource> OrderIfHasParams<TSource, TKey>(
            this IQueryable<TSource> data,
            LoadParam loadParam,
            bool asc,
            Expression<Func<TSource, TKey>> elseSortExpression)
        {
            ArgumentChecker.NotNull(elseSortExpression, nameof(elseSortExpression));

            IQueryable<TSource> resultQueryable;

            if (loadParam.Order.IsNotEmpty())
            {
                resultQueryable = data.Order(loadParam);
            }
            else if (asc)
            {
                resultQueryable = data.OrderBy(elseSortExpression);
            }
            else
            {
                resultQueryable = data.OrderByDescending(elseSortExpression);
            }

            return resultQueryable;
        }

        /// <summary>
        /// Метод фильтрует запрос для задания условия: период действует на момент
        /// </summary>
        /// <typeparam name="T">Тип сущности, в которой имеются даты начала и окончания искомого периода</typeparam>
        /// <param name="data">Запрос</param>
        /// <param name="dateStart">Дата начала периода 1</param>
        /// <param name="dateEnd">Дата окончания периода 1</param>
        /// <param name="secondPeriodStartSelector">Дата начала периода 2</param>
        /// <param name="secondPeriodEndSelector">Дата окончания периода 2</param>
        /// <returns>Подзапрос</returns>
        public static IQueryable<T> WherePeriodActiveIn<T>(
            this IQueryable<T> data,
            DateTime dateStart,
            DateTime dateEnd,
            Expression<Func<T, DateTime>> secondPeriodStartSelector,
            Expression<Func<T, DateTime>> secondPeriodEndSelector)
        {
            return
                data.Where(DateTimeExpressionExtensions.CreatePeriodActiveInExpression(
                    dateStart,
                    dateEnd,
                    secondPeriodStartSelector,
                    secondPeriodEndSelector));
        }

        /// <summary>
        /// Метод фильтрует запрос для задания условия: период не действует на момент
        /// </summary>
        /// <typeparam name="T">Тип сущности, в которой имеются даты начала и окончания искомого периода</typeparam>
        /// <param name="data">Запрос</param>
        /// <param name="dateStart">Дата начала периода 1</param>
        /// <param name="dateEnd">Дата окончания периода 1</param>
        /// <param name="secondPeriodStartSelector">Дата начала периода 2</param>
        /// <param name="secondPeriodEndSelector">Дата окончания периода 2</param>
        /// <returns>Подзапрос</returns>
        public static IQueryable<T> WherePeriodActiveNotIn<T>(
            this IQueryable<T> data,
            DateTime dateStart,
            DateTime dateEnd,
            Expression<Func<T, DateTime>> secondPeriodStartSelector,
            Expression<Func<T, DateTime>> secondPeriodEndSelector)
        {
            return
                data.Where(DateTimeExpressionExtensions.CreatePeriodActiveNotInExpression(
                    dateStart,
                    dateEnd,
                    secondPeriodStartSelector,
                    secondPeriodEndSelector));
        }
    }
}