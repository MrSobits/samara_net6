namespace Bars.Gkh.Nhibernate
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Utils.EntityExtensions;

    using NHibernate.Hql.Ast;
    using NHibernate.Linq;
    using NHibernate.Linq.Visitors;

    public class GetIdGenerator : BaseGkhMethodHqlGenerator
    {
        private static readonly Expression<Func<IHaveId, long>> IdExpression = x => x.Id;
        private static readonly Expression<Func<IHaveExportId, long>> ExportIdExpression = x => x.ExportId;
        private static readonly Lazy<Dictionary<MethodInfo, LambdaExpression>> AllowMethods = new Lazy<Dictionary<MethodInfo, LambdaExpression>>(() => new Dictionary<MethodInfo, LambdaExpression>
        {
            { ReflectionHelper.GetMethod(() => default(IHaveId).GetId()), GetIdGenerator.IdExpression },
            { ReflectionHelper.GetMethod(() => default(IHaveId).GetNullableId()), GetIdGenerator.IdExpression},
            { ReflectionHelper.GetMethod(() => default(IHaveExportId).GetId()), GetIdGenerator.ExportIdExpression},
            { ReflectionHelper.GetMethod(() => default(IHaveExportId).GetNullableId()), GetIdGenerator.ExportIdExpression}
        });

        public GetIdGenerator()
        {
            this.AllowedMethods = GetIdGenerator.AllowMethods.Value.Keys;
        }

        /// <inheritdoc />
        public override HqlTreeNode BuildHql(MethodInfo method, Expression targetObject, ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor)
        {
            var expression = GetIdGenerator.AllowMethods.Value[method];

            return visitor.Visit(ExpressionExtension.ReplaceParameter(expression, arguments[0])).AsExpression();
        }
    }
}