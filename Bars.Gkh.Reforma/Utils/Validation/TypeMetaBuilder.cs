namespace Bars.Gkh.Reforma.Utils.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Reforma.Utils.Validation.Meta;

    /// <summary>
    /// Построитель мета-описания
    /// </summary>
    public abstract class TypeMetaBuilder
    {
        /// <summary>
        /// Описание типа
        /// </summary>
        protected readonly ClassMetaDescription Description;

        /// <summary>
        /// Описание свойств типа
        /// </summary>
        protected readonly IDictionary<string, PropertyMetaDescription> PropertyDescriptions;
        
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="description">Тип</param>
        protected TypeMetaBuilder(ClassMetaDescription description)
        {
            this.Description = description;
            this.PropertyDescriptions = new Dictionary<string, PropertyMetaDescription>();
        }

        /// <summary>
        /// Создаёт экземпляр описателя
        /// </summary>
        /// <typeparam name="T">Тип сущyоcти</typeparam>
        /// <returns>Построитель</returns>
        public static TypeMetaBuilder<T> For<T>() where T : IEntity
        {
            return TypeMetaBuilder<T>.Create();
        }
    }

    /// <summary>
    /// Типизированный построитель мета-описания
    /// </summary>
    /// <typeparam name="T"> Тип сущности </typeparam>
    public class TypeMetaBuilder<T> : TypeMetaBuilder where T : IEntity
    {
        /// <summary>
        /// .ctor
        /// </summary>
        private TypeMetaBuilder() : base(new ClassMetaDescription<T>())
        {
            
        }

        /// <summary>
        /// Создать экземпляр построителя
        /// </summary>
        /// <returns><see cref="TypeMetaBuilder{T}"/></returns>
        public static TypeMetaBuilder<T> Create()
        {
            return new TypeMetaBuilder<T>();
        }

        /// <summary>
        /// Добавить описание сущности
        /// </summary>
        /// <param name="name">Наименование сущности</param>
        /// <returns><see cref="TypeMetaBuilder{T}"/></returns>
        public TypeMetaBuilder<T> WithDescription(string name)
        {
            this.Description.To<ClassMetaDescription<T>>().SetName(name);
            return this;
        }

        /// <summary>
        /// Добавить описание свойства
        /// </summary>
        /// <typeparam name="TProperty">Тип свойства</typeparam>
        /// <param name="propertySelector">Селектор поля</param>
        /// <param name="descpription">Описание</param>
        /// <returns><see cref="TypeMetaBuilder{T}"/></returns>
        public TypeMetaBuilder<T> HasProperty<TProperty>(Expression<Func<T, TProperty>> propertySelector, string descpription = null)
        {
            ArgumentChecker.NotNull(propertySelector, nameof(propertySelector));

            var memberExpression = propertySelector.Body.To<MemberExpression>();

            // если передали x => x.Obj1.Obj2...
            if (memberExpression.Expression is MemberExpression)
            {
                // игнорим
                return this;
            }

            var member = memberExpression.Member;
            this.PropertyDescriptions[member.Name] = new PropertyMetaDescription<T, TProperty>(this.Description, member.Name, descpription);

            return this;
        }

        /// <summary>
        /// Добавить к контейнеру
        /// </summary>
        /// <param name="container">Контейнер описания</param>
        /// <returns><see cref="TypeMetaBuilder{T}"/></returns>
        public TypeMetaBuilder<T> RegisteredIn(IMetaDescriptionContainer container)
        {
            container.Add(this.Description, this.PropertyDescriptions.Values);
            return this;
        }
    }
}