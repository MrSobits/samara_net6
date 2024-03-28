namespace Bars.Gkh.Map
{
    using System;
    using System.Linq.Expressions;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Mapping.Interfaces.Chains;
    using Bars.B4.Modules.Mapping.Mappers;

    /// <summary>
    /// Маппинг подкласса сущности наследованной от <see cref="BaseEntity"/>
    /// </summary>
    public abstract class GkhJoinedSubClassMap<T> : JoinedSubClassMap<T>
        where T : class, IEntity
    {
        /// <inheritdoc />
        protected GkhJoinedSubClassMap(string tableName)
            : base(typeof(T).FullName, tableName)
        {
        }

        /// <summary>
        /// Обертка над <see cref="BaseMap{T}.Property{TProperty}"/>
        /// <para>
        /// Имя параметра передается как property.Name
        /// </para>
        /// </summary>
        public ISimpleMapDescriptorChain Property<TProperty>(Expression<Func<T, TProperty>> property)
        {
            return this.EntityDescriptorChain.Property(property, property.Name);
        }

        /// <summary>
        /// Обертка над <see cref="BaseMap{T}.Reference{TProperty}"/>
        /// <para>
        /// Имя параметра передается как property.Name
        /// </para>
        /// </summary>
        public IReferenceMapDescriptorChain Reference<TProperty>(Expression<Func<T, TProperty>> property) where TProperty : class
        {
            return this.EntityDescriptorChain.Reference(property, property.Name);
        }
    }
}