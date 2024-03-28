namespace Bars.Gkh.Nhibernate
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bars.Gkh.Utils;

    using NHibernate.Hql.Ast;
    using NHibernate.Linq.Visitors;

    public class ToNullableGenerator : BaseGkhMethodHqlGenerator
    {
        private static readonly Lazy<List<MethodInfo>> AllowMethods = new Lazy<List<MethodInfo>>(() => new List<MethodInfo>
        {
            typeof(NullableExtensions).GetMethod("ToNullable")
        });

        public ToNullableGenerator()
        {
            this.AllowedMethods = ToNullableGenerator.AllowMethods.Value;
        }

        /// <inheritdoc />
        public override HqlTreeNode BuildHql(MethodInfo method, Expression targetObject, ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor)
        {
            return visitor.Visit(arguments[0]);
        }
    }
}