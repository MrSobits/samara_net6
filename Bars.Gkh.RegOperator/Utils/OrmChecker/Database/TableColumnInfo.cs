namespace Bars.Gkh.RegOperator.Utils.OrmChecker.Database
{
    /// <summary>Описание колонки в СУБД Oracle</summary>
    public class TableColumnInfo
    {
        /// <summary>Имя колонки</summary>
        public string ColumnName { get; set; }

        /// <summary>Тип колонки</summary>
        public string DataType { get; set; }

        /// <summary>Размерность</summary>
        public int Size { get; set; }

        /// <summary>Обязательное</summary>
        public bool NotNull { get; set; }

        /// <summary>Размер целой части</summary>
        public int Precision { get; set; }

        /// <summary>Размер дробной части</summary>
        public int Scale { get; set; }

        /// <summary>Значение по умолчанию</summary>
        public string DefaultValue { get; set; }
    }
}