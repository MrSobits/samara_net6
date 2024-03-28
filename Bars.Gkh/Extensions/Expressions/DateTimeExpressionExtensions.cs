namespace Bars.Gkh.Extensions
{
    using System;
    using System.Linq.Expressions;

    using Bars.B4.Utils;
    using Bars.Gkh.Extensions.Expressions;

    /// <summary>
    /// Методы-расширения для формирования запросов на пересечения периодов
    /// </summary>
    public static class DateTimeExpressionExtensions
    {
        /// <summary>
        /// Метод формирует выражение для задания условия вхождения периода по датам.
        /// <para>Период 1 включается в период 2 (включен = равен или меньше)</para>
        /// </summary>
        /// <typeparam name="TInput">Тип сущности, в которой имеются даты начала и окончания искомого периода</typeparam>
        /// <param name="dateStart">Дата начала периода 1</param>
        /// <param name="dateEnd">Дата окончания периода 1</param>
        /// <param name="secondPeriodStartSelector">Дата начала периода 2</param>
        /// <param name="secondPeriodEndSelector">Дата окончания периода 2</param>
        /// <returns>Выражение</returns>
        public static Expression<Func<TInput, bool>> CreatePeriodIncludesExpression<TInput>(
            DateTime dateStart,
            DateTime dateEnd,
            Expression<Func<TInput, DateTime>> secondPeriodStartSelector,
            Expression<Func<TInput, DateTime>> secondPeriodEndSelector)
        {
            var inputParameter = Expression.Parameter(typeof(TInput), "x");

            var dateStartConstant = Expression.Constant(dateStart);
            var dateEndConstant = Expression.Constant(dateEnd);

            var clonePeriodStartSelector = ParameterRebinder.ReplaceParameters(
                secondPeriodStartSelector.Parameters[0],
                inputParameter,
                secondPeriodStartSelector)
                .To<Expression<Func<TInput, DateTime>>>();

            var clonePeriodEndSelector = ParameterRebinder.ReplaceParameters(
                secondPeriodEndSelector.Parameters[0],
                inputParameter,
                secondPeriodEndSelector)
                .To<Expression<Func<TInput, DateTime>>>();

            // Необходимое выражение: x => periodStart >= x.DateStart && periodEnd <= x.DateEnd
            // 1. x => periodStart >= x.DateStart
            var firstExpression = Expression.GreaterThanOrEqual(dateStartConstant, clonePeriodStartSelector.Body);

            // 2. x => periodEnd <= x.DateEnd
            var secondExpression = Expression.LessThanOrEqual(dateEndConstant, clonePeriodEndSelector.Body);

            return Expression.Lambda<Func<TInput, bool>>(Expression.AndAlso(firstExpression, secondExpression), inputParameter);
        }

        /// <summary>
        /// Метод формирует выражение для задания условия периода больше другого периода
        /// <para>Период 1 > Период 2</para>
        /// </summary>
        /// <typeparam name="TInput">Тип сущности, в которой имеются даты начала и окончания искомого периода</typeparam>
        /// <param name="dateStart">Дата начала периода 1</param>
        /// <param name="dateEnd">Дата окончания периода 1</param>
        /// <param name="secondPeriodStartSelector">Дата начала периода 2</param>
        /// <param name="secondPeriodEndSelector">Дата окончания периода 2</param>
        /// <param name="canIntersect">Если передать true, то период 1 может начинаться до даты окончания периода 2</param>
        /// <returns>Выражение</returns>
        public static Expression<Func<TInput, bool>> CreatePeriodGreaterThanExpression<TInput>(
            DateTime dateStart,
            DateTime dateEnd,
            Expression<Func<TInput, DateTime>> secondPeriodStartSelector,
            Expression<Func<TInput, DateTime>> secondPeriodEndSelector,
            bool canIntersect = true)
        {
            var inputParameter = Expression.Parameter(typeof(TInput), "x");

            var dateStartConstant = Expression.Constant(dateStart);
            var dateEndConstant = Expression.Constant(dateEnd);

            var clonePeriodStartSelector = ParameterRebinder.ReplaceParameters(
                secondPeriodStartSelector.Parameters[0],
                inputParameter,
                secondPeriodStartSelector)
                .To<Expression<Func<TInput, DateTime>>>();

            var clonePeriodEndSelector = ParameterRebinder.ReplaceParameters(
                secondPeriodEndSelector.Parameters[0],
                inputParameter,
                secondPeriodEndSelector)
                .To<Expression<Func<TInput, DateTime>>>();

            // Необходимое выражение: x => periodStart > x.DateEnd && periodEnd > x.DateEnd                                 - если canIntersect = false
            // и:                     x => periodStart >= x.DateStart && periodStart <= x.DateEnd  && periodEnd > x.DateEnd - если canIntersect = true
            Expression firstExpression;

            if (canIntersect)
            {
                firstExpression = Expression.GreaterThanOrEqual(dateStartConstant, clonePeriodStartSelector.Body);
                var secondExpression = Expression.LessThanOrEqual(dateStartConstant, clonePeriodEndSelector.Body);

                firstExpression = Expression.AndAlso(firstExpression, secondExpression);
            }
            else
            {
                firstExpression = Expression.GreaterThan(dateStartConstant, clonePeriodEndSelector.Body);
            }

            var lastExpression = Expression.GreaterThan(dateEndConstant, clonePeriodEndSelector.Body);

            return Expression.Lambda<Func<TInput, bool>>(Expression.AndAlso(firstExpression, lastExpression), inputParameter);
        }

        /// <summary>
        /// Метод формирует выражение для задания условия периода меньше другого периода
        /// <para>Период 1 меньше Период 2</para>
        /// </summary>
        /// <typeparam name="TInput">Тип сущности, в которой имеются даты начала и окончания искомого периода</typeparam>
        /// <param name="dateStart">Дата начала периода 1</param>
        /// <param name="dateEnd">Дата окончания периода 1</param>
        /// <param name="secondPeriodStartSelector">Дата начала периода 2</param>
        /// <param name="secondPeriodEndSelector">Дата окончания периода 2</param>
        /// <param name="canIntersect">Если передать true, то период 1 может начинаться до даты окончания периода 2</param>
        /// <returns>Выражение</returns>
        public static Expression<Func<TInput, bool>> CreatePeriodLessThanExpression<TInput>(
            DateTime dateStart,
            DateTime dateEnd,
            Expression<Func<TInput, DateTime>> secondPeriodStartSelector,
            Expression<Func<TInput, DateTime>> secondPeriodEndSelector,
            bool canIntersect = true)
        {
            var inputParameter = Expression.Parameter(typeof(TInput), "x");

            var dateStartConstant = Expression.Constant(dateStart);
            var dateEndConstant = Expression.Constant(dateEnd);

            var clonePeriodStartSelector = ParameterRebinder.ReplaceParameters(
                 secondPeriodStartSelector.Parameters[0],
                 inputParameter,
                 secondPeriodStartSelector)
                .To<Expression<Func<TInput, DateTime>>>();

            var clonePeriodEndSelector = ParameterRebinder.ReplaceParameters(
                secondPeriodEndSelector.Parameters[0],
                inputParameter,
                secondPeriodEndSelector)
                .To<Expression<Func<TInput, DateTime>>>();

            // Необходимое выражение: x => periodStart < x.DateStart && periodEnd < x.DateStart                             - если canIntersect = false
            // и:                     x => periodStart < x.DateStart && periodEnd >= x.DateStart && periodEnd <= x.DateEnd  - если canIntersect = true
            Expression expression = Expression.LessThan(dateStartConstant, clonePeriodStartSelector.Body);

            if (canIntersect)
            {
                var firstExpression = Expression.GreaterThanOrEqual(dateEndConstant, clonePeriodStartSelector.Body);
                var secondExpression = Expression.LessThanOrEqual(dateEndConstant, clonePeriodEndSelector.Body);

                expression = Expression.AndAlso(expression, firstExpression);
                expression = Expression.AndAlso(expression, secondExpression);
            }
            else
            {
                var secondExpression = Expression.LessThan(dateEndConstant, clonePeriodEndSelector.Body);
                expression = Expression.AndAlso(expression, secondExpression);
            }

            return Expression.Lambda<Func<TInput, bool>>(expression, inputParameter);
        }

        /// <summary>
        /// Метод формирует выражение для задания условия: больше другого (обратное CreatePeriodIncludesExpression)
        /// <para>Период 1 больше Период 2</para>
        /// </summary>
        /// <typeparam name="TInput">Тип сущности, в которой имеются даты начала и окончания искомого периода</typeparam>
        /// <param name="dateStart">Дата начала периода 1</param>
        /// <param name="dateEnd">Дата окончания периода 1</param>
        /// <param name="secondPeriodStartSelector">Дата начала периода 2</param>
        /// <param name="secondPeriodEndSelector">Дата окончания периода 2</param>
        /// <param name="canEquals">Если передать true, то период 1 может быть равен периоду 2</param>
        /// <returns>Выражение</returns>
        public static Expression<Func<TInput, bool>> CreatePeriodAllOrMoreExpression<TInput>(
            DateTime dateStart,
            DateTime dateEnd,
            Expression<Func<TInput, DateTime>> secondPeriodStartSelector,
            Expression<Func<TInput, DateTime>> secondPeriodEndSelector,
            bool canEquals = true)
        {
            var inputParameter = Expression.Parameter(typeof(TInput), "x");

            var dateStartConstant = Expression.Constant(dateStart);
            var dateEndConstant = Expression.Constant(dateEnd);

            var clonePeriodStartSelector = ParameterRebinder.ReplaceParameters(
                 secondPeriodStartSelector.Parameters[0],
                 inputParameter,
                 secondPeriodStartSelector)
                .To<Expression<Func<TInput, DateTime>>>();

            var clonePeriodEndSelector = ParameterRebinder.ReplaceParameters(
                secondPeriodEndSelector.Parameters[0],
                inputParameter,
                secondPeriodEndSelector)
                .To<Expression<Func<TInput, DateTime>>>();

            // Необходимое выражение: x => periodStart < x.DateStart && periodEnd > x.DateEnd   - если canIntersect = false
            // и:                     x => periodStart =< x.DateStart && periodEnd >= x.DateEnd - если canIntersect = true
            Expression expression;

            if (canEquals)
            {
                var firstExpression = Expression.LessThanOrEqual(dateStartConstant, clonePeriodStartSelector.Body);
                var secondExpression = Expression.GreaterThanOrEqual(dateEndConstant, clonePeriodEndSelector.Body);

                expression = Expression.AndAlso(firstExpression, secondExpression);
            }
            else
            {
                var firstExpression = Expression.LessThan(dateStartConstant, clonePeriodStartSelector.Body);
                var secondExpression = Expression.GreaterThan(dateEndConstant, clonePeriodEndSelector.Body);

                expression = Expression.AndAlso(firstExpression, secondExpression);
            }

            return Expression.Lambda<Func<TInput, bool>>(expression, inputParameter);
        }

        /// <summary>
        /// Метод формирует выражение для задания условия: период действует на момент
        /// </summary>
        /// <typeparam name="TInput">Тип сущности, в которой имеются даты начала и окончания искомого периода</typeparam>
        /// <param name="dateStart">Дата начала периода 1</param>
        /// <param name="dateEnd">Дата окончания периода 1</param>
        /// <param name="secondPeriodStartSelector">Дата начала периода 2</param>
        /// <param name="secondPeriodEndSelector">Дата окончания периода 2</param>
        /// <returns>Выражение</returns>
        public static Expression<Func<TInput, bool>> CreatePeriodActiveInExpression<TInput>(
            DateTime dateStart,
            DateTime dateEnd,
            Expression<Func<TInput, DateTime>> secondPeriodStartSelector,
            Expression<Func<TInput, DateTime>> secondPeriodEndSelector)
        {
            var firstExpression = DateTimeExpressionExtensions.CreatePeriodIncludesExpression(dateStart, dateEnd, secondPeriodStartSelector, secondPeriodEndSelector);
            var secondExpression = DateTimeExpressionExtensions.CreatePeriodAllOrMoreExpression(dateStart, dateEnd, secondPeriodStartSelector, secondPeriodEndSelector);

            return firstExpression.Or(secondExpression);
        }

        /// <summary>
        /// Метод формирует выражение для задания условия: период не действует на момент
        /// </summary>
        /// <typeparam name="TInput">Тип сущности, в которой имеются даты начала и окончания искомого периода</typeparam>
        /// <param name="dateStart">Дата начала периода 1</param>
        /// <param name="dateEnd">Дата окончания периода 1</param>
        /// <param name="secondPeriodStartSelector">Дата начала периода 2</param>
        /// <param name="secondPeriodEndSelector">Дата окончания периода 2</param>
        /// <returns>Выражение</returns>
        public static Expression<Func<TInput, bool>> CreatePeriodActiveNotInExpression<TInput>(
            DateTime dateStart,
            DateTime dateEnd,
            Expression<Func<TInput, DateTime>> secondPeriodStartSelector,
            Expression<Func<TInput, DateTime>> secondPeriodEndSelector)
        {
            var firstExpression = DateTimeExpressionExtensions.CreatePeriodLessThanExpression(dateStart, dateEnd, secondPeriodStartSelector, secondPeriodEndSelector, false);
            var secondExpression = DateTimeExpressionExtensions.CreatePeriodGreaterThanExpression(dateStart, dateEnd, secondPeriodStartSelector, secondPeriodEndSelector, false);

            return firstExpression.Or(secondExpression);
        }
    }
}