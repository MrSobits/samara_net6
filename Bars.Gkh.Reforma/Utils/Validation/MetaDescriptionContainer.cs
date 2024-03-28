namespace Bars.Gkh.Reforma.Utils.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Reforma.Utils.Validation.Meta;

    /// <summary>
    /// Контейнер мета-описания классов для последующей валидации
    /// </summary>
    public class MetaDescriptionContainer : IMetaDescriptionContainer
    {
        private readonly IDictionary<Type, ClassMetaDescription> descriptions;
        private readonly IDictionary<Tuple<Type, string>, PropertyMetaDescription> propertyDescriptions;

        /// <summary>
        /// .ctor
        /// </summary>
        public MetaDescriptionContainer()
        {
            this.propertyDescriptions = new Dictionary<Tuple<Type, string>, PropertyMetaDescription>();
            this.descriptions = new Dictionary<Type, ClassMetaDescription>();
        }

        /// <summary>
        /// Зарегистрировать описание
        /// </summary>
        /// <typeparam name="T"> Типизация построителя </typeparam>
        /// <param name="typeBuilder"> Построитель  </param>
        public void Add<T>(TypeMetaBuilder<T> typeBuilder) where T : IEntity
        {
            typeBuilder.RegisteredIn(this);
        }

        /// <summary>
        /// Добавить описание
        /// </summary>
        /// <param name="description">Описание типа</param>
        /// <param name="propDescriptions">Описание свойств</param>
        public void Add(ClassMetaDescription description, IEnumerable<PropertyMetaDescription> propDescriptions)
        {
            this.descriptions[description.Type] = description;

            foreach (var propertyMetaDescription in propDescriptions)
            {
                if (propertyMetaDescription.ParentType != description)
                {
                    // не его свойство
                    continue;
                }

                this.propertyDescriptions[Tuple.Create(description.Type, propertyMetaDescription.PropertyName)] = propertyMetaDescription;
            }
        }

        /// <inheritdoc />
        public ClassMetaDescription GetTypeDescription(Type type)
        {
            return this.descriptions.Get(type);
        }

        /// <inheritdoc />
        public PropertyMetaDescription GetPropertyDescription(Type type, string name)
        {
            return this.propertyDescriptions.Get(Tuple.Create(type, name));
        }

        /// <inheritdoc />
        public ClassMetaDescription<T> GetTypeDescription<T>()
        {
            return (ClassMetaDescription<T>)this.GetTypeDescription(typeof(T));
        }

        /// <inheritdoc />
        public PropertyMetaDescription<TParent, TProperty> GetPropertyDescription<TParent, TProperty>(Expression<Func<TParent, TProperty>> propertySelector)
        {
            var memberExpression = propertySelector.Body.To<MemberExpression>();
            return (PropertyMetaDescription<TParent, TProperty>)this.GetPropertyDescription(typeof(TParent), memberExpression.Member.Name);
        }

        /// <inheritdoc />
        public PropertyMetaDescription<TProperty> GetPropertyDescription<TProperty>(Type type, Expression<Func<TProperty>> propertySelector)
        {
            var memberExpression = propertySelector.Body.To<MemberExpression>();
            return (PropertyMetaDescription<TProperty>)this.GetPropertyDescription(type, memberExpression.Member.Name);
        }
    }
}