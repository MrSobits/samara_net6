namespace Bars.Gkh.SystemDataTransfer.Meta
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.SystemDataTransfer.Meta.Serialization;

    /// <summary>
    /// Генератор описания сущностей, которые будут переносится в другую систему
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    public class TransferEntityMetaBuilder<TEntity> where TEntity : class, IEntity, new()
    {
        private readonly TransferEntityMeta<TEntity> meta;

        internal TransferEntityMetaBuilder(TransferEntityMeta<TEntity> meta)
        {
            this.meta = meta;
        }

        /// <summary>
        /// Сущность выгружается частично, если её не выбрал пользователь
        /// <remarks>Т.е. выгрузятся только те, на которые хоть кто-то ссылается</remarks>
        /// </summary>
        /// <param name="isPartially">Сущность выгружается частично</param>
        public TransferEntityMetaBuilder<TEntity> AsPartially(bool isPartially = true)
        {
            this.meta.IsPartially = isPartially;
            return this;
        }

        /// <summary>
        /// Добавить собственный сериализатор
        /// </summary>
        /// <typeparam name="TSerializer">Тип сериализатора</typeparam>
        public TransferEntityMetaBuilder<TEntity> AddCustomSerializer<TSerializer>(Action<TSerializer> initAction = null) where TSerializer : IDataSerializer<TEntity>, new()
        {
            var serializer = new TSerializer
            {
                Container = ApplicationContext.Current.Container
            };

            initAction?.Invoke(serializer);
            this.meta.Serializer = serializer;

            return this;
        }

        /// <summary>
        /// При импорте сущност всегда создавать новую
        /// </summary>
        /// <param name="createNew">Создать новую</param>
        public TransferEntityMetaBuilder<TEntity> AlwaysCreateNew(bool createNew = true)
        {
            this.meta.CreateNew = createNew;
            return this;
        }

        /// <summary>
        /// Добавить свойство-селектор
        /// </summary>
        /// <param name="propertySelector">Селектор</param>
        public TransferEntityMetaBuilder<TEntity> AddComparer<TProperty>(Expression<Func<TEntity, TProperty>> propertySelector)
        {
            this.meta.AddProperty(propertySelector);
            return this;
        }

        /// <summary>
        /// Добавить свойство-селектор
        /// </summary>
        public TransferEntityMetaBuilder<TEntity> AddComparer<TProperty1, TProperty2>(Expression<Func<TEntity, TProperty1>> propertySelector1, Expression<Func<TEntity, TProperty2>> propertySelector2)
        {
            return this.AddComparer(propertySelector1).AddComparer(propertySelector2);
        }

        /// <summary>
        /// Добавить внешний ключ
        /// </summary>
        /// <typeparam name="TParentEntity">Тип внешней сущности</typeparam>
        /// <typeparam name="TProperty">Свойство</typeparam>
        /// <param name="propertySelectorToParent">Селектор</param>
        /// <param name="propertySelector">Селектор</param>
        /// <returns></returns>
        public TransferEntityMetaBuilder<TEntity> AddComplexComparer<TParentEntity, TProperty>(Expression<Func<TParentEntity, TEntity>> propertySelectorToParent, Expression<Func<TParentEntity, TProperty>> propertySelector)
            where TParentEntity : IEntity
        {
            return this.AddCustomSerializer<ComplexEntitySerializer<TEntity, TParentEntity, TProperty>>(x =>
            {
                x.SetAdditionalEntity(propertySelectorToParent, propertySelector);
                x.Meta = this.meta;
            });
        }

        public TransferEntityMetaBuilder<TEntity> HasBase<TBase>()
            where TBase : IEntity
        {
            if (this.meta.Type.IsNot<TBase>())
            {
                throw new ArgumentException($"Type {typeof(TBase)} is not inherits from {this.meta.Type}");
            }

            this.meta.BaseType = typeof(TBase);
            return this;
        }

        public TransferEntityMetaBuilder<TEntity> IsBase()
        {
            this.meta.IsBase = true;
            return this;
        }

        /// <summary>
        /// Добавить игнорируемое свойство
        /// </summary>
        /// <param name="propertySelector">Селектор</param>
        public TransferEntityMetaBuilder<TEntity> IgnoreProperty<TProperty>(Expression<Func<TEntity, TProperty>> propertySelector)
        {
            this.meta.IgnoreProperties.Add((PropertyInfo)((MemberExpression)propertySelector.Body).Member);
            return this;
        }

        /// <summary>
        /// Фильтрации выгружаемых сущностей
        /// </summary>
        public TransferEntityMetaBuilder<TEntity> Filter(Expression<Func<TEntity, bool>> propertySelector)
        {
            this.meta.FilterExpression = propertySelector;
            return this;
        }
    }
}