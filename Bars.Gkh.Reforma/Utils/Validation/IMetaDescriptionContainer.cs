namespace Bars.Gkh.Reforma.Utils.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Reforma.Utils.Validation.Meta;

    /// <summary>
    /// Интерфейс контейнера мета-описания классов для последующей валидации
    /// </summary>
    public interface IMetaDescriptionContainer
    {
        /// <summary>
        /// Добавить описание
        /// </summary>
        /// <param name="description">Описание типа</param>
        /// <param name="propertyDescriptions">Описание свойств</param>
        void Add(ClassMetaDescription description, IEnumerable<PropertyMetaDescription> propertyDescriptions);

        /// <summary>
        /// Зарегистрировать описание
        /// </summary>
        /// <typeparam name="T"> Типизация построителя </typeparam>
        /// <param name="typeBuilder"> Построитель  </param>
        void Add<T>(TypeMetaBuilder<T> typeBuilder) where T : IEntity;

        /// <summary>
        /// Получить описание типа
        /// </summary>
        /// <param name="type">Наименование типа</param>
        /// <returns>Описатель</returns>
        ClassMetaDescription GetTypeDescription(Type type);

        /// <summary>
        /// Получить описание типа
        /// </summary>
        /// <param name="type">Наименование типа</param>
        /// <param name="name">Наименование свойства</param>
        /// <returns>Описатель</returns>
        PropertyMetaDescription GetPropertyDescription(Type type, string name);

        /// <summary>
        /// Получить описание типа
        /// </summary>
        ///  <typeparam name="T"> Тип описателя </typeparam>
        /// <returns> Описатель </returns>
        ClassMetaDescription<T> GetTypeDescription<T>();

        /// <summary>
        /// Получить описание типа
        /// </summary>
        /// <typeparam name="TParent"> Тип родителя </typeparam>
        /// <typeparam name="TProperty"> Тип свойства </typeparam>
        /// <param name="propertySelector"> Селектор свойства </param>
        /// <returns> Описатель </returns>
        PropertyMetaDescription<TParent, TProperty> GetPropertyDescription<TParent, TProperty>(Expression<Func<TParent, TProperty>> propertySelector);

        /// <summary>
        /// Получить описание типа
        /// </summary>
        /// <typeparam name="TProperty"> Тип свойства </typeparam>
        /// <param name="type"> Тип родителя </param>
        /// <param name="propertySelector"> Селектор свойства </param>
        /// <returns> Описатель </returns>
        PropertyMetaDescription<TProperty> GetPropertyDescription<TProperty>(Type type, Expression<Func<TProperty>> propertySelector);
    }
}