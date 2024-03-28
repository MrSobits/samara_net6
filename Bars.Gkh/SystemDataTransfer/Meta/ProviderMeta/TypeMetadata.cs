namespace Bars.Gkh.SystemDataTransfer.Meta.ProviderMeta
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using NHibernate.Metadata;

    /// <summary>
    /// Мета-информация о способе хранения сущности
    /// </summary>
    public class TypeMetadata
    {
        /// <summary>
        /// Тип
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Информация о маппинге
        /// </summary>
        public IClassMetadata PersistentMetadata { get; set; }

        /// <summary>
        /// Имя таблицы
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Колонки
        /// </summary>
        public Dictionary<string, PropertyInfo> Columns { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        public TypeMetadata()
        {
            this.Columns = new Dictionary<string, PropertyInfo>();
        }
    }
}