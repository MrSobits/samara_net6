namespace Bars.Gkh.SystemDataTransfer.Utils
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    
    using Bars.Gkh.Extensions;

    /// <summary>
    /// Методы расширения для генерации выражений
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Метод создает выражения присвоения свойству значения из подзапроса
        /// </summary>
        /// <typeparam name="TEntity">Сущность, для которой ищем связанный объект</typeparam>
        /// <typeparam name="TInnerEntity">Внешняя сущность</typeparam>
        /// <typeparam name="TProp">Присоединяемый тип</typeparam>
        /// <param name="xArg">Аргумент x - типа <typeparamref name="TEntity"/></param>
        /// <param name="queryable">Внешний запрос</param>
        /// <param name="propertyInfo">Свойство, к которому присваиваем значение</param>
        /// <param name="selectExpression">Выражение подзапроса</param>
        /// <returns>Выражение присовения</returns>
        public static MemberAssignment CreateMemeberAssignment<TEntity, TInnerEntity, TProp>(ParameterExpression xArg, IQueryable<TInnerEntity> queryable, PropertyInfo propertyInfo, Func<IQueryable<TInnerEntity>, Expression<Func<TEntity, TProp>>> selectExpression)
        {
            var expression = selectExpression(queryable);
            return Expression.Bind(propertyInfo, ParameterRebinder.ReplaceParameters(expression.Parameters[0], xArg, expression).Body);
        }
    }
}