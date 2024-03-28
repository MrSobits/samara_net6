namespace Bars.Gkh.RegOperator.Utils.OrmChecker.Database
{
    using System.Collections.Generic;

    /// <summary>Описание индекс в СУБД Oracle</summary>
    public class TableIndexInfo
    {
        public TableIndexInfo()
        {
            Columns = new List<string>();
        }

        /// <summary>Имя</summary>
        public string Name { get; set; }

        /// <summary>Уникальный</summary>
        public bool Unique { get; set; }

        /// <summary>Тип</summary>
        public string Type { get; set; }

        /// <summary>Колонки. Важен порядок!</summary>
        public List<string> Columns { get; set; }

        /// <summary>Формула</summary>
        public string Formula { get; set; }

        public bool IsFormula
        {
            get { return Type == "FUNCTION-BASED NORMAL"; }
        }
    }
}