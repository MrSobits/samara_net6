namespace Bars.Gkh.SystemDataTransfer.Meta.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.SystemDataTransfer.Meta.ProviderMeta;
    using Bars.Gkh.SystemDataTransfer.Utils;

    using NHibernate.Linq;

    /// <summary>
    /// Сериализатор сложной сущности
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    /// <typeparam name="TParentEntity">Тип дополнительной/родительской сущности</typeparam>
    /// <typeparam name="TProperty">Тип свойства для сопоставления, которое будет добавлено в переносимые данные</typeparam>
    public class ComplexEntitySerializer<TEntity, TParentEntity, TProperty> : AbstractDataSerializer<TEntity>
        where TEntity : class, IEntity
        where TParentEntity : IEntity
    {
        private static string ComplexKeyProperty => "ComplexKey";

        private ComplexEntityBuilder<TEntity> ComplexEntityBuilder => new ComplexEntityBuilder<TEntity>(this.Meta);

        public TransferEntityMeta<TEntity> Meta { get; set; }

        private static readonly MethodInfo whereMethod = 
            typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .First(x => x.Name == "Where")
            .MakeGenericMethod(typeof(TParentEntity));

        private static readonly MethodInfo selectMethod = 
            typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .First(x => x.Name == "Select")
            .MakeGenericMethod(typeof(TParentEntity), typeof(TProperty));

        private static readonly MethodInfo firstOrDefaultMethod = 
            typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .First(x => x.Name == "FirstOrDefault" && x.GetParameters().Length == 1)
            .MakeGenericMethod(typeof(TProperty));

        private static readonly MethodInfo selectIdMethod = 
            typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .First(x => x.Name == "Select")
            .MakeGenericMethod(typeof(TParentEntity), typeof(long));

        private static readonly MethodInfo firstOrDefaultIdMethod = 
            typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .First(x => x.Name == "FirstOrDefault" && x.GetParameters().Length == 1)
            .MakeGenericMethod(typeof(long));
        
        private IRepository<TEntity> entityDomain;
        private IRepository<TEntity> EntityDomain => new StatelessNhRepository<TEntity>(IntegrationSessionAccessor.GetSession());

        private Expression<Func<TParentEntity, TEntity>> propertySelectorToParent;
        private Expression<Func<TParentEntity, TProperty>> propertySelector;

        /// <inheritdoc />
        public override bool HasComplexProperty => true;

        public void SetAdditionalEntity(Expression<Func<TParentEntity, TEntity>> propertySelectorToParent, Expression<Func<TParentEntity, TProperty>> propertySelector)
        {
            this.propertySelectorToParent = propertySelectorToParent;
            this.propertySelector = propertySelector;
        }

        /// <inheritdoc />
        public override IQueryable EntitySelector(IQueryable<TEntity> query)
        {
            return query.Project(this.Meta).To(this.ComplexEntityBuilder.GetDefinedType(), false);
        }

        /// <inheritdoc />
        public override Expression GetEntitySelectExpression(ParameterExpression parameterExpression)
        {
            var query = IntegrationSessionAccessor.GetSession()?.Query<TParentEntity>();
            return this.GetAssignment(parameterExpression, query);
        }

        private Expression GetAssignment(ParameterExpression xArg, IQueryable<TParentEntity> queryable)
        {
            var yArg = Expression.Parameter(typeof(TParentEntity), "y");

            var equalsExpression = Expression.Equal(
                xArg, 
                ParameterRebinder.ReplaceParameters(this.propertySelectorToParent.Parameters[0], yArg, this.propertySelectorToParent.Body));

            Expression callWhereExpression = Expression.Call(null, whereMethod, Expression.Constant(queryable, typeof(IQueryable<TParentEntity>)), Expression.Lambda(equalsExpression, yArg));

            var selectPropertyExpression = ParameterRebinder.ReplaceParameters(
                this.propertySelector.Parameters[0],
                yArg,
                this.propertySelector.Body);

            // костыль, иначе хватаем null reference при сборе кэша
            if (selectPropertyExpression.Type.Is<IEntity>())
            {
                selectPropertyExpression = Expression.Property(selectPropertyExpression, "Id");

                Expression callSelectExpression = Expression.Call(
                    null,
                    selectIdMethod,
                    callWhereExpression,
                    Expression.Lambda(selectPropertyExpression, yArg));

                return Expression.Call(null, firstOrDefaultIdMethod, callSelectExpression);
            }
            else
            {
                Expression callSelectExpression = Expression.Call(
                    null,
                    selectMethod,
                    callWhereExpression,
                    Expression.Lambda(selectPropertyExpression, yArg));

                return Expression.Call(null, firstOrDefaultMethod, callSelectExpression);
            }
        }

        /// <inheritdoc />
        public override object Serialize(object entity)
        {
            var properties = new Dictionary<string, object>();
            foreach (var propertyInfo in this.Meta.GetExportableProperties())
            {
                var value = entity.GetValue(propertyInfo.Name);
                properties[propertyInfo.Name] = (value as IEntity)?.Id ?? value;
            }

            var keyObject = ((IComplexImportEntity)entity).ComplexKey;
            properties[ComplexKeyProperty] = (keyObject as IEntity)?.Id ?? keyObject;

            return new Item
            {
                Properties = properties
            };
        }

        /// <inheritdoc />
        public override IEntity Deserializer(IEntity saveEntity, Item item, Stream stream)
        {
            var persistentObject = saveEntity as PersistentObject;
            if (persistentObject == null)
            {
                return null;
            }

            var baseEntity = persistentObject as BaseEntity;
            if (baseEntity != null)
            {
                baseEntity.ObjectEditDate = DateTime.Now;
            }

            if(persistentObject.Id == 0)
            {
                if (baseEntity != null)
                {
                    baseEntity.ObjectCreateDate = DateTime.Now;
                }

                this.EntityDomain.Save(persistentObject);
            }
            else
            {
                if (baseEntity != null)
                {
                    baseEntity.ObjectVersion += 1;
                }

                this.EntityDomain.Update(persistentObject);
            }

            return saveEntity;
        }

        /// <inheritdoc />
        public override void Flush(long id, Stream stream)
        {
        }

        /// <inheritdoc />
        public override object Serialize(TEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override string GetFileName(long id)
        {
            return null;
        }
    }
}