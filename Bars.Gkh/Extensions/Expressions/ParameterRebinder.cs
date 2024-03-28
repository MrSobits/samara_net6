namespace Bars.Gkh.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Класс, реализующий переопределение параметров в ламбда выражении
    /// </summary>
    public class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="map">Маппинг замены</param>
        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        /// <summary>
        /// Заменить параметры
        /// </summary>
        /// <param name="map">Маппинг замены</param>
        /// <param name="exp">Выражение</param>
        /// <returns>Результирующие выражение</returns>
        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        /// <summary>
        /// Заменить параметры
        /// </summary>
        /// <param name="parameter1">Заменяемый параметр</param>
        /// <param name="parameter2">Заменяющий параметр</param>
        /// <param name="exp">Выражение</param>
        /// <returns>Результирующие выражение</returns>
        public static Expression ReplaceParameters(ParameterExpression parameter1, ParameterExpression parameter2, Expression exp)
        {
            var map = new Dictionary<ParameterExpression, ParameterExpression>
            {
                { parameter1, parameter2 }
            };

            return new ParameterRebinder(map).Visit(exp);
        }

        /// <summary>
        /// Заменить параметры
        /// </summary>
        /// <param name="parameter1">Заменяемый параметр</param>
        /// <param name="parameter2">Заменяющий параметр</param>
        /// <param name="exp">Выражение</param>
        /// <returns>Результирующие выражение</returns>
        public static Expression<T> ReplaceParameters<T>(ParameterExpression parameter1, ParameterExpression parameter2, Expression<T> exp)
        {
            return (Expression<T>)ParameterRebinder.ReplaceParameters(parameter1, parameter2, (Expression)exp);
        }

        /// <summary>
        /// Заменить параметры
        /// </summary>
        /// <param name="map">Маппинг замены</param>
        /// <param name="exp">Выражение</param>
        /// <returns>Результирующие выражение</returns>
        public static Expression<T> ReplaceParameters<T>(Dictionary<ParameterExpression, ParameterExpression> map, Expression<T> exp)
        {
            return (Expression<T>)ParameterRebinder.ReplaceParameters(map, (Expression)exp);
        }

        /// <summary>
        /// Перегрузка, выполняющая замену
        /// </summary>
        /// <param name="p">Параметр выражения</param>
        /// <returns>Результирующее выражение</returns>
        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;

            if (this.map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }

            return base.VisitParameter(p);
        }
    }
}