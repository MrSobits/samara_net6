namespace Bars.GkhDi.MetaValueConstructor
{
    using System;
    using System.Linq.Expressions;

    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Методы расширения для конструктора в модуле Di
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Метод собирает <see cref="Expression"/> для фильтрации по периоду раскрытия информации <see cref="PeriodDi"/>
        /// </summary>
        /// <typeparam name="TInput">Тип входного параметра</typeparam>
        /// <param name="period">Период рейтинга эффективности</param>
        /// <param name="periodDiSelector">Селектор периода раскрытия информации для раскрытия</param>
        /// <returns>Фильтр по периоду</returns>
        public static Expression<Func<TInput, bool>> CreateContainsExpression<TInput>(
            this EfficiencyRatingPeriod period,
            Expression<Func<TInput, PeriodDi>> periodDiSelector)
        {
            var inputParameter = periodDiSelector.Parameters[0];
            var constantFalse = Expression.Constant(false);
            var dateStartConstant = Expression.Constant(period.DateStart);

            var periodDiProperty = periodDiSelector.Body;
            var periodDiDateStartProperty = Expression.Property(periodDiProperty, "DateStart");
            var periodDiDateEndProperty = Expression.Property(periodDiProperty, "DateEnd");

            // DateStart
            var perioDiDateStartHasValueProperty = Expression.Property(periodDiDateStartProperty, "HasValue");
            var perioDiDateStartValueProperty = Expression.Property(periodDiDateStartProperty, "Value");
            
            // DateEnd
            var perioDiDateEndHasValueProperty = Expression.Property(periodDiDateEndProperty, "HasValue");
            var perioDiDateEndValueProperty = Expression.Property(periodDiDateEndProperty, "Value");

            var periodDiDateEndHasNoValue = Expression.Equal(perioDiDateEndHasValueProperty, constantFalse);
            var perioDiDateStartHasNoValueExpression = Expression.Equal(perioDiDateStartHasValueProperty, constantFalse);

            // input.PeriodDi.DateEnd.Value >= period.DateStart
            var diDateEndGreaterOrEqThanPeriodDateStart = Expression.GreaterThanOrEqual(perioDiDateEndValueProperty, dateStartConstant);

            // input.PeriodDi.DateStart.HasValue == false
            Expression totalExpression = Expression.OrElse(periodDiDateEndHasNoValue, diDateEndGreaterOrEqThanPeriodDateStart);
            Expression secondExpression = perioDiDateStartHasNoValueExpression;

            var dateEndValue = Expression.Constant(period.DateEnd);

            // input.PeriodDi.DateStart.Value <= period.DateEnd.Value
            var diDateStartLessOrEqThanDateEndValue = Expression.LessThanOrEqual(perioDiDateStartValueProperty, dateEndValue);

            // input.PeriodDi.DateStart.HasValue == false || input.PeriodDi.DateStart.Value <= period.DateEnd.Value
            secondExpression = Expression.OrElse(secondExpression, diDateStartLessOrEqThanDateEndValue);

            // x => (!PeriodDi.DateEnd.HasValue || PeriodDi.DateEnd.Value >= period.DateStart) 
            // && (!period.DateEnd.HasValue || !PeriodDi.DateStart.HasValue || PeriodDi.DateStart.Value <= period.DateEnd.Value)
            totalExpression = Expression.AndAlso(totalExpression, secondExpression);
            return Expression.Lambda<Func<TInput, bool>>(totalExpression, inputParameter);
        }
    }
}