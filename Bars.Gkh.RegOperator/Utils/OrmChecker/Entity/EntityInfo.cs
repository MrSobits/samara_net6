namespace Bars.Gkh.RegOperator.Utils.OrmChecker.Entity
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.RegOperator.Utils.OrmChecker.Database;

    /// <summary>Описание хранимой сущности</summary>
    public class EntityInfo
    {
        public EntityInfo()
        {
            Properties = new List<EntityPropertyInfo>();
        }

        /// <summary>Наименование</summary>
        public string Name { get; set; }

        /// <summary>Полное наименование</summary>
        public string FullName { get; set; }

        /// <summary>Описание</summary>
        public string Description { get; set; }

        /// <summary>Является SubClass'м</summary>
        public bool IsJoinedSubClass { get; set; }

        /// <summary>Родитель. Указывается только в случае если IsJoinedSubClass == true</summary>
        public EntityInfo Parent { get; set; }

        /// <summary>Тип</summary>
        public Type Type { get; set; }

        /// <summary>Таблица в СУБД</summary>
        public TableInfo Table { get; set; }

        /// <summary>Свойства</summary>
        public List<EntityPropertyInfo> Properties { get; set; }
    }
}