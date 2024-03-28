namespace Bars.Gkh.SystemDataTransfer.Meta.ProviderMeta
{
    using System;

    /// <summary>
    /// Зависимость сущности
    /// </summary>
    public class Dependency
    {
        /// <summary>
        /// Тип зависимой сущности
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Имя свойства
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ссылка на самого себя
        /// </summary>
        public bool IsParent { get; set; }

        /// <summary>
        /// Зависимость на родительскую сущность
        /// </summary>
        public bool IsRoot { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"FullName: {this.Name}, Type: {this.Type.Name}, IsParent: {this.IsParent}";
        }
    }
}