namespace Bars.Gkh.MetaValueConstructor
{
    using System;
    using System.Linq.Expressions;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.Extensions.Expressions;

    /// <summary>
    /// Методы расширения для построения деревьев выражений
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Метод собирает <see cref="Expression"/> для фильтрации по периоду раскрытия информации <see cref="EfficiencyRatingPeriod"/>
        /// </summary>
        /// <typeparam name="TInput">Тип входного параметра</typeparam>
        /// <param name="period">Период рейтинга эффективности</param>
        /// <param name="periodStartSelector">Селектор нижней даты</param>
        /// <param name="periodEndSelector">Селектор верхней даты</param>
        /// <returns>Фильтр по периоду</returns>
        public static Expression<Func<TInput, bool>> CreateContainsExpression<TInput>(
            this EfficiencyRatingPeriod period,
            Expression<Func<TInput, DateTime>> periodStartSelector,
            Expression<Func<TInput, DateTime>> periodEndSelector)
        {
            var inputParameter = Expression.Parameter(typeof(TInput), "x");
            var dateStartConstant = Expression.Constant(period.DateStart);

            var clonePeriodStartSelector = ParameterRebinder.ReplaceParameters(
                periodStartSelector.Parameters[0],
                inputParameter,
                periodStartSelector)
                .To<Expression<Func<TInput, DateTime>>>();

            var clonePeriodEndSelector = ParameterRebinder.ReplaceParameters(
                periodEndSelector.Parameters[0],
                inputParameter,
                periodEndSelector)
                .To<Expression<Func<TInput, DateTime>>>();

            var periodDiDateStartProperty = clonePeriodStartSelector.Body;
            var periodDiDateEndProperty = clonePeriodEndSelector.Body;

            // input.DateEnd >= period.DateStart
            var diDateEndGreaterOrEqThanPeriodDateStart = Expression.GreaterThanOrEqual(periodDiDateEndProperty, dateStartConstant);

            Expression totalExpression = diDateEndGreaterOrEqThanPeriodDateStart;

            var dateEndValue = Expression.Constant(period.DateEnd);

            // input.DateStart <= period.DateEnd.Value
            Expression secondExpression = Expression.LessThanOrEqual(periodDiDateStartProperty, dateEndValue);

            // x => (PeriodDi.DateEnd.Value >= period.DateStart) 
            // && (!period.DateEnd.HasValue || PeriodDi.DateStart.Value <= period.DateEnd.Value)
            totalExpression = Expression.AndAlso(totalExpression, secondExpression);
            return Expression.Lambda<Func<TInput, bool>>(totalExpression, inputParameter);
        }
    }
}