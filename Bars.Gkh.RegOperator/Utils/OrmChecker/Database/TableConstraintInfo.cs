namespace Bars.Gkh.RegOperator.Utils.OrmChecker.Database
{
    /// <summary>Описание констрайнта в СУБД</summary>
    public class TableConstraintInfo
    {
        /// <summary>Имя</summary>
        public string Name { get; set; }

        /// <summary>Первичный ключ</summary>
        public bool IsPrimary
        {
            get { return Type == "P" || Type == "PRIMARY KEY"; }
        }

        /// <summary>Контроль данных</summary>
        public bool IsCheck
        {
            get { return Type == "C" || Type == "CHECK"; }
        }

        /// <summary>Контроль ссылок</summary>
        public bool IsReferences
        {
            get { return Type == "R" || Type == "FOREIGN KEY"; }
        }

        /// <summary>Тип</summary>
        public string Type { get; set; }

        /// <summary>Текст</summary>
        public string Text { get; set; }

        /// <summary>Имя колонки. Только для ссылок!</summary>
        public string ColumnName { get; set; }

        /// <summary>Текст</summary>
        public string ReferenceTableName { get; set; }

        /// <summary>Текст</summary>
        public string ReferenceColumnName { get; set; }

        /// <summary>Отложеный</summary>
        public bool Deferrable { get; set; }
    }
}