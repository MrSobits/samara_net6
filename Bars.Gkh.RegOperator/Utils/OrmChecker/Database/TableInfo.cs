namespace Bars.Gkh.RegOperator.Utils.OrmChecker.Database
{
    using System.Collections.Generic;

    /// <summary>Описание таблицы в СУБД Oracle</summary>
    public class TableInfo
    {
        public TableInfo()
        {
            Columns = new List<TableColumnInfo>();
            Constraints = new List<TableConstraintInfo>();
            Indexes = new List<TableIndexInfo>();
        }

        /// <summary>Имя таблицы</summary>
        public string TableName { get; set; }

        /// <summary>Является SubClass'м</summary>
        public bool IsJoinedSubClass { get; set; }

        /// <summary>Родитель. Указывается только в случае если IsJoinedSubClass == true</summary>
        public TableInfo Parent { get; set; }

        /// <summary>Колонки</summary>
        public List<TableColumnInfo> Columns { get; set; }

        /// <summary>Констрайнты</summary>
        public List<TableConstraintInfo> Constraints { get; set; }

        /// <summary>Индексы</summary>
        public List<TableIndexInfo> Indexes { get; set; }
    }
}