namespace Bars.Gkh.Utils.LinqInline
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bars.B4.Utils;

    public static class LinqInliner
    {
        /// <summary>
        /// Заменить вызовы методов, помеченные атрибутом <see cref="LinqInlineAttribute"/> деревьями выражений
        /// </summary>
        public static IQueryable<T> Inlining<T>(this IQueryable<T> source)
        {
            var visitor = new Visitor();
            var expression = visitor.Visit(source.Expression);

            return source.Provider.CreateQuery<T>(expression);
        }

        private class Visitor : ExpressionVisitor
        {
            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                var attribute = node.Method.GetAttribute<LinqInlineAttribute>(false);

                if (attribute == null)
                {
                    return base.VisitMethodCall(node);
                }

                var ownerClass = node.Method.ReflectedType ?? node.Method.DeclaringType;
                if (ownerClass == null)
                {
                    return base.VisitMethodCall(node);
                }

                var expanderFieldName = attribute.ExpressionName ?? node.Method.Name + "Expression";
                var expanderField = ownerClass.GetField(expanderFieldName) 
                    ?? ownerClass.GetField(expanderFieldName, BindingFlags.Public | BindingFlags.Static);
                if (expanderField == null)
                {
                    return base.VisitMethodCall(node);
                }

                var expansionTree = expanderField.GetValue(null) as LambdaExpression;
                if (expansionTree == null)
                {
                    return base.VisitMethodCall(node);
                }
                var expansionBody = ExpressionExtension.ReplaceParameter(expansionTree, node.Object).Body;
                var visitor = new BodyVisitor(expansionTree.Parameters, node.Arguments);
                return visitor.Visit(expansionBody);
            }
        }

        private class BodyVisitor : ExpressionVisitor
        {
            private readonly Dictionary<ParameterExpression, Expression> bindedValues;

            public BodyVisitor(IList<ParameterExpression> parameters, IList<Expression> arguments)
            {
                this.bindedValues = arguments.Select((x, i) => new
                    {
                        value = x,
                        argument = parameters[i]
                    })
                    .ToDictionary(x => x.argument, x => x.value);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                Expression result;
                if (!this.bindedValues.TryGetValue(node, out result))
                {
                    result = base.VisitParameter(node);
                }

                return result;
            }
        }
    }
}