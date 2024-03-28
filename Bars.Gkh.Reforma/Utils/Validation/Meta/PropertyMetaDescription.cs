namespace Bars.Gkh.Reforma.Utils.Validation.Meta
{
    using System;

    using Bars.B4.Utils.Annotations;

    /// <summary>
    /// Описание свойства (благодаря <see cref="ClassMetaDescription"/> можем строить дерево взаимодействия сущностей)
    /// </summary>
    public class PropertyMetaDescription : ClassMetaDescription
    {
        /// <summary>
        /// Тип родительской сущности
        /// </summary>
        public ClassMetaDescription ParentType { get; set; }

        /// <summary>
        /// Наименование свойства
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="parentType">Описание владельца свойства</param>
        /// <param name="type">тип свойства</param>
        /// <param name="propertyName">Имя свойства</param>
        /// <param name="name">Наименование свойства</param>
        public PropertyMetaDescription(ClassMetaDescription parentType, Type type, string propertyName, string name = null) : base(type, name)
        {
            this.ParentType = parentType;
            this.PropertyName = propertyName;
        }
    }

    /// <summary>
    /// Описание свойства (благодаря <see cref="ClassMetaDescription"/> можем строить дерево взаимодействия сущностей)
    /// </summary>
    /// <typeparam name="T"> Тип свойства </typeparam>
    public class PropertyMetaDescription<T> : PropertyMetaDescription
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="parentType">Описание владельца свойства</param>
        /// <param name="propertyName">Имя свойства</param>
        /// <param name="name">Наименование свойства</param>
        public PropertyMetaDescription(ClassMetaDescription parentType, string propertyName, string name = null) : base(parentType, typeof(T), propertyName, name)
        {
        }
    }

    /// <summary>
    /// Описание свойства (благодаря <see cref="ClassMetaDescription"/> можем строить дерево взаимодействия сущностей)
    /// </summary>
    /// <typeparam name="TParent"> Тип владельца</typeparam>
    /// <typeparam name="TProperty"> Тип свойства </typeparam>
    public class PropertyMetaDescription<TParent, TProperty> : PropertyMetaDescription<TProperty>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="parentType">Описание владельца свойства</param>
        /// <param name="propertyName">Имя свойства</param>
        /// <param name="name">Наименование свойства</param>
        public PropertyMetaDescription(ClassMetaDescription parentType, string propertyName, string name = null) : base(parentType as ClassMetaDescription<TParent>, propertyName, name)
        {
            ArgumentChecker.IsType<ClassMetaDescription<TParent>>(parentType, nameof(parentType));
        }
    }
}