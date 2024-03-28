namespace Bars.Gkh.Gis.Reports.SqlManager
{
    using System.Collections.Generic;
    using System.Data;
    using Reports;

    public interface IReportSqlManager<T> where T : StimulReportDynamicExcel
    {
        DataSet GetData(Dictionary<string, string> parameters);
    }
}
