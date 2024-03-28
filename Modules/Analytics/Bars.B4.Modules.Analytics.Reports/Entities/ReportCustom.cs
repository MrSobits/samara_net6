namespace Bars.B4.Modules.Analytics.Reports.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Reports.Enums;

    /// <summary>
    /// 
    /// </summary>
    public class ReportCustom : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public ReportCustom()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="template"></param>
        /// <param name="format"></param>
        public ReportCustom(string key, byte[] template, ReportPrintFormat format)
        {
            CodedReportKey = key;
            Template = template;
            PrintFormat = format;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string CodedReportKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual byte[] Template { get; set; }

        /// <summary>
        /// Формат печати отчета.
        /// </summary>
        public virtual ReportPrintFormat PrintFormat { get; set; }

    }
}
