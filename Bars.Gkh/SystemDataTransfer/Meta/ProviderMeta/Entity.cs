namespace Bars.Gkh.SystemDataTransfer.Meta.ProviderMeta
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Выгружаемая сущность
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// Полное наименование (имя таблицы)
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Тип
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Зависимости
        /// </summary>
        public List<Dependency> Dependencies { get; set; }
    }
}