namespace Bars.Gkh.Gis.Reports.SqlManager.Impl
{
    using System.Collections.Generic;
    using System.Data;
    using Castle.Windsor;
    using Reports;

    public abstract class BaseReportSqlManager<T> : IReportSqlManager<T> where T : StimulReportDynamicExcel
    {
        protected IWindsorContainer Container;

        public abstract DataSet GetData(Dictionary<string, string> parameters);

        protected BaseReportSqlManager(IWindsorContainer container)
        {
            Container = container;
        }

        protected abstract string GetSql(Dictionary<string, string> parameters);
    }
}
