namespace Bars.Gkh.DocIoGenerator
{
    using System.Collections.Generic;
    using System.Data;

    public class TableParam
    {
        public string Name { get; set; }

        public DataTable Table { get; set; }

        public List<int?> ColumnWidth { get; set; }
    }
}