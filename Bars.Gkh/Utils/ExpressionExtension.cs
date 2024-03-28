namespace Bars.Gkh.Utils
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class ExpressionExtension
    {
        /// <summary>
        /// Заменить входной параметр в дереве выражений
        /// </summary>
        /// <typeparam name="T">Тип делегата</typeparam>
        /// <param name="expression">Исходное выражение</param>
        /// <param name="newParam">Новый параметр</param>
        public static Expression<T> ReplaceParameter<T>(Expression<T> expression, Expression newParam)
        {
            return new ParameterReplacerVisitor<T>(expression, newParam).VisitAndConvert(expression);
        }

        public static LambdaExpression ReplaceParameter(LambdaExpression expression, Expression newParam)
        {
            return new ParameterReplacerVisitor(expression, newParam).VisitAndConvert(expression);
        }
        
        /// <summary>
        /// Получить выражения для метода Contains без учета регистра
        /// </summary>
        /// <param name="propertyName">Свойство объекта, к которому применяется Contains</param>
        /// <param name="value">Значение, по которому происходит поиск</param>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetContainsIgnoreCaseMethodExpression<T>(string propertyName, string value)
        {
            var toUpperMethod = typeof(string).GetMethod("ToUpper", Type.EmptyTypes);
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var expressionParam = Expression.Parameter(typeof(T));
            var expressionProperty = Expression.Property(expressionParam, propertyName);
            var expressionPropertyInUpperCase = Expression.Call(expressionProperty, toUpperMethod);
            var expressionValueInUpperCase = Expression.Call(Expression.Constant(value), toUpperMethod);
            var expressionContainsMethod = Expression.Call(expressionPropertyInUpperCase, containsMethod, expressionValueInUpperCase);

            return Expression.Lambda<Func<T, bool>>(expressionContainsMethod, expressionParam);
        }

        /// <summary>
        /// Получить выражение для сравнения значения и свойства
        /// </summary>
        /// <param name="propertyName">Название свойства объекта для сравнения</param>
        /// <param name="value">Значение для сравнения</param>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetEqualityExpression<T>(string propertyName, string value)
        {
            var expressionParam = Expression.Parameter(typeof(T));
            var expressionProperty = Expression.Property(expressionParam, propertyName);
            var expressionValue = Expression.Constant(value, typeof(string));
            var expressionEquality = Expression.Equal(expressionProperty, expressionValue);

            return Expression.Lambda<Func<T, bool>>(expressionEquality, expressionParam);
        }

        /// <summary>
        /// Получить выражение для Select с созданием нового объекта
        /// </summary>
        /// <param name="propertyMatchingDict">Словарь сопоставления свойств исходного и результирующего объектов.
        /// Ключ - название свойства исходного объекта, Значение - название свойства результирующего объекта</param>
        /// <typeparam name="T">Тип исходного объекта</typeparam>
        /// <typeparam name="R">Тип результирующего объекта</typeparam>
        /// <returns></returns>
        public static Expression<Func<T,R>> GetSelectNewObjectExpression<T,R>(IDictionary<string, string> propertyMatchingDict)
        {
            var expressionParam = Expression.Parameter(typeof(T));
            var expressionSelectObject = Expression.New(typeof(R));
            var expressionObjectPropertyBinding = propertyMatchingDict.Select(x =>
            {
                var expressionProperty = Expression.Property(expressionParam, x.Key);

                return Expression.Bind(typeof(R).GetProperty(x.Value), expressionProperty);
            }).ToArray();
            var expressionSelectObjectInit = Expression.MemberInit(expressionSelectObject, expressionObjectPropertyBinding);

            return Expression.Lambda<Func<T, R>>(expressionSelectObjectInit, expressionParam);
        }

        private class ParameterReplacerVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression source;
            private readonly Expression target;

            public ParameterReplacerVisitor(LambdaExpression expression, Expression target)
            {
                this.source = expression.Parameters.First();
                this.target = target;
            }

            internal LambdaExpression VisitAndConvert(LambdaExpression root)
            {
                return this.VisitLambda(root);
            }

            private LambdaExpression VisitLambda(LambdaExpression node)
            {
                return Expression.Lambda(this.Visit(node.Body), node.Parameters);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node == this.source ? this.target : base.VisitParameter(node);
            }
        }

        /// <summary>
        /// Обходит параметризированное дерево выражений с одним аргументом и подменяет его
        /// </summary>
        private class ParameterReplacerVisitor<T> : ExpressionVisitor
        {
            private readonly ParameterExpression source;
            private readonly Expression target;

            public ParameterReplacerVisitor(Expression<T> expression, Expression target)
            {
                this.source = expression.Parameters.First();
                this.target = target;
            }

            internal Expression<T> VisitAndConvert(Expression<T> root)
            {
                return (Expression<T>)this.VisitLambda(root);
            }

            protected override Expression VisitLambda<TExpr>(Expression<TExpr> node)
            {
                return Expression.Lambda<T>(this.Visit(node.Body), node.Parameters);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node == this.source ? this.target : base.VisitParameter(node);
            }
        }
    }

    /// <summary>
    /// Кэш скомпилированных деревьев выражений
    /// </summary>
    public class ExpressionCache
    {
        private static readonly ConcurrentDictionary<LambdaExpression, Delegate> cache 
            = new ConcurrentDictionary<LambdaExpression, Delegate>();

        /// <summary>
        /// Получить делегат
        /// </summary>
        public static Func<TIn, TOut> AsFunc<TIn, TOut>(LambdaExpression expression)
        {
            return ExpressionCache.cache.GetOrAdd(expression, e => e.Compile()) as Func<TIn, TOut>;
        }

        /// <summary>
        /// Очистить кэш
        /// </summary>
        public static void ClearCache()
        {
            ExpressionCache.cache.Clear();
        }
    }
}