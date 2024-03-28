namespace Bars.GkhGji.Report.Form1Controll
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public abstract class BaseReportSection
    {       
        public string GetCell(int row, int col)
        {
            var report = this;
            var method = report.GetType().GetMethod(string.Format("GetCell{0}_{1}", row, col), BindingFlags.NonPublic | BindingFlags.Instance);
            var result = method == null ? 0 : method.Invoke(report, null);
            return result.ToString();
        }

        protected long[] inspectionsIds;

        protected DateTime dateStart;

        protected DateTime dateEnd;

        public BaseReportSection(List<long> inspections, DateTime dateStart, DateTime dateEnd)
        {
            inspectionsIds = inspections.ToArray();
            this.dateStart = dateStart;
            this.dateEnd = dateEnd;            
        }
    }    
}