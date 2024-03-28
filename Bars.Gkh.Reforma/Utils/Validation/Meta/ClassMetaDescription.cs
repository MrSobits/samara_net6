namespace Bars.Gkh.Reforma.Utils.Validation.Meta
{
    using System;

    /// <summary>
    /// Мета описание типа
    /// </summary>
    public class ClassMetaDescription
    {
        /// <summary>
        /// Тип сущности
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// Тип, определенный пользователем
        /// </summary>
        public bool IsUserDefined => this.Type.FullName.StartsWith("Bars.");

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="type">Тип</param>
        /// <param name="name">Наименование</param>
        public ClassMetaDescription(Type type, string name)
        {
            this.Type = type;
            this.Name = name;
        }
    }

    /// <summary>
    /// Мета описание типа
    /// </summary>
    /// <typeparam name="T">Тип класса</typeparam>
    public class ClassMetaDescription<T> : ClassMetaDescription
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="name">Наименование</param>
        public ClassMetaDescription(string name = null)
            : base(typeof(T), name)
        {
        }

        /// <summary>
        /// Указать имя типа
        /// </summary>
        /// <param name="name">Наименование</param>
        public void SetName(string name)
        {
            this.Name = name;
        }
    }
}