namespace Bars.B4.Modules.Analytics.Reports.Proxies
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class ReportCategory
    {
        /// <summary>
        /// 
        /// </summary>
        public ReportCategory()
        {
            Reports = new List<Report>();
        }
        /// <summary>
        /// 
        /// </summary>
        public List<Report> Reports { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }
    }
}
