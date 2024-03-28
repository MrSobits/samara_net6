namespace Bars.Gkh.RegOperator.Utils.OrmChecker.Entity
{
    using System;

    using Bars.Gkh.RegOperator.Utils.OrmChecker.Database;

    /// <summary>Описане свойства хранимой сущности</summary>
    public class EntityPropertyInfo
    {
        /// <summary>Наименование</summary>
        public string Name { get; set; }

        /// <summary>Описание</summary>
        public string Description { get; set; }

        /// <summary>Тип</summary>
        public Type Type { get; set; }

        /// <summary>Фвляется формулой</summary>
        public bool IsFormula { get; set; }

        /// <summary>Формула</summary>
        public string Formula { get; set; }

        /// <summary>Колонка в СУБД</summary>
        public TableColumnInfo DbColumn { get; set; }
    }
}